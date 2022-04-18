using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public partial class Dg
    {
        public string dgClassFromList;

        /// <summary>
        /// Method to update Dg unit (derived from a file) with info from dglist.xml
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="unitIsNew"></param>
        /// <param name="pkgChanged"></param>
        public void AssignFromDgList(XDocument xmlDoc, bool unitIsNew = false, bool pkgChanged = false)
        {
            try
            {
                List<Dg> imdgRecords;
                imdgRecords = GetRecordsFromXml(Unno, xmlDoc);

                int orderInList;
                orderInList = ChooseOneOfMultipleEntries(imdgRecords);

                //Transfer data from record to dg item
                Dg dgFromImdgCode = imdgRecords[orderInList];

                if (pkgChanged)
                {
                    StowageCat = dgFromImdgCode.StowageCat;
                    stowageSW = dgFromImdgCode.stowageSW;
                    segregationSG = dgFromImdgCode.segregationSG;
                    special = dgFromImdgCode.special;
                    Properties = dgFromImdgCode.Properties;
                    return;
                }

                dgFromImdgCode.SpecialClass();
                dgClassFromList = dgFromImdgCode.dgclass;

                UpdateDgClassAndSubclass(unitIsNew, dgFromImdgCode);
                UpdateOtherInformation(dgFromImdgCode);

                //copy original property text instead of "see entry above"
                while (Properties == "See entry above.")
                {
                    Properties = imdgRecords[orderInList - 1].Properties;
                    orderInList--;
                }

                SetStabilizedValueIfContainedInDgList();
            }
            catch (Exception e)
            {
                //TODO: IMPLEMENT NOTIFICATION OF USER
            }
        }

        /// <summary>
        /// Selects all records (one for each packing group) from IMDG code DgList with matching UN no.
        /// </summary>
        /// <param name="unno">Specified UN no.</param>
        /// <param name="xmlDoc">IMDG Code DG List in xml format.</param>
        /// <returns>List of Dg of specified UN no with raw information from IMDG code.</returns>
        private static List<Dg> GetRecordsFromXml(ushort unno, XDocument xmlDoc)
        {
            var chosenEntries = (from entry in xmlDoc.Descendants("DG")
                                 where (int)entry.Attribute("unno") == unno
                                 select entry);

            List<Dg> list = new List<Dg>();

            //Assigning data to temporary item (record) from chosenEntries and complete temporary list
            foreach (var entry in chosenEntries)
            {
                Dg record = new Dg
                {
                    Unno = unno,
                    dgclass = entry.Attribute("class").Value,
                    packingGroup = byte.Parse(entry.Attribute("pg").Value)
                };


                string[] array = entry.Attribute("subrisk").Value.Split('/');
                foreach (string x in array) if (x != "–") record.DgSubclass = x;

                var temp = entry.Attribute("MP").Value;
                if (temp == "true") record.IsMp = true;

                record.Name = entry.Element("name").Value;

                array = entry.Attribute("specialprovisions").Value.Split(' ');
                foreach (string x in array) record.special.Add(x != "–" ? Convert.ToUInt16(x) : (ushort)0);

                record.StowageCat = (entry.Element("Stowage").Attribute("category").Value).Length > 1 ?
                                    (entry.Element("Stowage").Attribute("category").Value)[1] :
                                    (entry.Element("Stowage").Attribute("category").Value)[0];

                array = entry.Element("Stowage").Attribute("provision").Value.Split(' ');
                foreach (string x in array) record.stowageSW.Add(x);

                array = entry.Element("Segregation").Value.Split(' ');
                foreach (string x in array) record.segregationSG.Add(x);

                record.Properties = entry.Element("Propertiesandobservations").Value;
                record.DgEMS = entry.Element("EMS").Value;

                list.Add(record);
            }

            return list;
        }

        /// <summary>
        /// Chooses correct record from the list with matching packing group.
        /// Otherwise returns a record with the highest risk. 
        /// </summary>
        /// <param name="imdgRecords">List of records from imdg code.</param>
        /// <returns></returns>
        private int ChooseOneOfMultipleEntries(List<Dg> imdgRecords)
        {
            //Solving case with multiple entries found
            int orderInList = 0;
            if (imdgRecords.Count > 1)
            {
                if (packingGroup != 0)
                {

                    for (int i = 0; i < imdgRecords.Count; i++)
                    {
                        if (imdgRecords[i].packingGroup == packingGroup)
                        {
                            orderInList = i;
                            break;
                        }
                    }
                }
                else
                {
                    //Find highest risk from choice (lowest packing group)
                    var pkg = 3;
                    for (int i = 0; i < imdgRecords.Count; i++)
                    {
                        pkg = imdgRecords[i].packingGroup == 1 ? imdgRecords[i].packingGroup : (pkg < imdgRecords[i].packingGroup ? pkg : imdgRecords[i].packingGroup);
                        if (imdgRecords[i].packingGroup == pkg) orderInList = i;
                    }
                }
            }

            return orderInList;
        }

        /// <summary>
        /// Method to deal with SP in subrisk in DG list.
        /// Used by raw records from IMDG code.
        /// </summary>
        private void SpecialClass()
        {
            foreach (string s in dgsubclass)
            {
                var clearSubclass = false;
                if (s == "–") dgsubclass.Remove(s);
                if (s.StartsWith("See SP"))
                {
                    switch (s.Remove(0, 4))
                    {
                        case "SP63":
                            dgclass = "2.1";
                            dgsubclass.Clear();
                            clearSubclass = true;
                            break;
                        case "172":
                            dgsubclass.Clear();
                            clearSubclass = true;
                            break;
                        case "181":
                            dgsubclass.Clear();
                            dgsubclass.Add("1.3");
                            Output.ThrowMessage("Caution! Class 1.3 added to {0} in accordance with SP181.", ContainerNumber);
                            clearSubclass = true;
                            break;
                        case "204":
                            break;
                        case "271":
                            break;
                        case "290":
                            break;
                        case "943":
                            break;
                        default:
                            Output.ThrowMessage($"For subrisk of unit {ContainerNumber} refer to special provision list {s.Remove(0, 4)}");
                            break;
                    }
                }
                if (clearSubclass) break;
            }
            //unno 2037 assigned class 2 in imdg code with responsibility to determine subdivision given to shippers.
            //for the purpose of calculation, if not specified in .edi, it will be assigned the most stringent subdivision
            if (Unno == 2037)
                dgclass = "2.1";
        }

        /// <summary>
        /// Updates Dg class and subclasses of dg unit with values from IMDG code, if required.
        /// </summary>
        /// <param name="unitIsNew">Bool: true - all values will be updated.</param>
        /// <param name="dgFromImdgCode">Dg record from IMDG code from which classes will be copied.</param>
        private void UpdateDgClassAndSubclass(bool unitIsNew, Dg dgFromImdgCode)
        {
            if (unitIsNew)
            {
                DgClass = dgFromImdgCode.dgclass;

            }
            else
            {
                if (dgFromImdgCode.dgclass != dgclass)
                {
                    //AEROSOLS
                    if (dgFromImdgCode.Unno == 1950)
                    {
                        if (dgclass == "2") dgclass = dgFromImdgCode.dgclass;
                        else
                            Output.ThrowMessage(
                                "Caution! For correct assigning of dg class and subrisk of AEROSOLS " +
                                "(UNNO 1950) in unit {0} refer to DG manifest and special provision 63 " +
                                "of IMDG code Ch 3.", ContainerNumber);
                    }
                    //Unno 2037
                    else if (dgFromImdgCode.Unno == 2037)
                        if (dgclass == "2")
                            DgClass = dgFromImdgCode.dgclass;
                        else
                            differentClass = false;
                    //All other cases
                    else differentClass = true;
                }
            }
                if (dgsubclass.Count == 0) DgSubclassArray = dgFromImdgCode.dgsubclass.ToArray();
        }

        /// <summary>
        /// Updates retrieved information from IMDG code, if missing (null).
        /// </summary>
        /// <param name="dgFromImdgCode">Dg record from IMDG code from which info will be copied.</param>
        private void UpdateOtherInformation(Dg dgFromImdgCode)
        {
            packingGroup = packingGroup != 0 ? packingGroup : dgFromImdgCode.packingGroup;
            if(string.IsNullOrEmpty(DgEMS)) DgEMS = dgFromImdgCode.DgEMS;
            IsMp = mpDetermined ? IsMp : dgFromImdgCode.IsMp;
            special = dgFromImdgCode.special;
            stowageSW = dgFromImdgCode.stowageSW;
            stowageSWfromDgList = dgFromImdgCode.stowageSW.ToList();
            segregationSG = dgFromImdgCode.segregationSG;
            StowageCat = StowageCat == '0' || StowageCat == '\0' ? dgFromImdgCode.StowageCat : StowageCat;
            stowageCatFromDgList = dgFromImdgCode.StowageCat;
            if (string.IsNullOrEmpty(Name)) Name = dgFromImdgCode.Name;
            OriginalNameFromCode = dgFromImdgCode.Name;
            Properties = dgFromImdgCode.Properties;
        }

        /// <summary>
        /// Set value IsStabilizedWordInProperShippingName to true, if word "Stabilized" contained in dg list
        /// </summary>
        private void SetStabilizedValueIfContainedInDgList()
        {
            if (!Name.ToLower().Contains("stabilized"))
            {
                isStabilizedWordInProperShippingName = false;
                return;
            }
            isStabilizedWordInProperShippingName = true;
            isStabilizedWordAddedToProperShippingName = false;
        }
    }
}
