using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public static class IMDGCodeDgListHandler
    {
        private static XDocument xmlDoc => ProgramFiles.DgDataBase;
        private static List<string> _messagesToUser;


        /// <summary>
        /// Method to update Dg unit with info from dglist.xml
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="unitIsNew"></param>
        /// <param name="pkgChanged"></param>
        public static void AssignFromDgList(this Dg dg, bool unitIsNew = false, bool pkgChanged = false)
        {
            _messagesToUser = new();

            try
            {
                List<DgFromIMDGCode> imdgRecords;
                imdgRecords = GetRecordsFromXml(dg.Unno);

                int orderInList = ChooseOneOfMultipleEntries(imdgRecords, dg.PackingGroupAsByte);

                //Transfer data from record to dg item
                DgFromIMDGCode dgFromImdgCode = imdgRecords[orderInList];
                dgFromImdgCode.SpecialClass();

                //copy original property text instead of "see entry above"
                dgFromImdgCode.GetSeeEntryAboveProperty(imdgRecords, orderInList);

                // if only packing group changed
                if (pkgChanged)
                {
                    dg.SetIMDGCodeValues(dgFromImdgCode, pkgChanged: true);
                    return;
                }

                dg.UpdateDgClassAndSubclass(unitIsNew, dgFromImdgCode);
                dg.UpdateOtherInformation(dgFromImdgCode);
            }
            catch (Exception e)
            {
                _messagesToUser.Add($"Could not update information from IMDG Code for unit {dg.ContainerNumber} UNNO {dg.Unno}.");
                string message = $"Error {e.Message} occurred while attempting to read dg list information for UNNO {dg.Unno} of unit {dg.ContainerNumber}";
                Data.LogWriter.Write(message);
            }
        }


        /// <summary>
        /// Selects all records (one for each packing group) from IMDG code DgList with matching UN no.
        /// </summary>
        /// <param name="unno">Specified UN no.</param>
        /// <param name="xmlDoc">IMDG Code DG List in xml format.</param>
        /// <returns>List of Dg of specified UN no with raw information from IMDG code.</returns>
        private static List<DgFromIMDGCode> GetRecordsFromXml(ushort unno)
        {
            var chosenEntries = from entry in xmlDoc.Descendants("DG")
                                 where (int)entry.Attribute("unno") == unno
                                 select entry;

            List<DgFromIMDGCode> list = new();

            //Assigning data to temporary item (record) from chosenEntries and complete temporary list
            foreach (var entry in chosenEntries)
            {
                DgFromIMDGCode record = new()
                {
                    Unno = unno,
                    DgClassInherited = entry.Attribute("class").Value,
                    PackingGroupAsByte = byte.Parse(entry.Attribute("pg").Value)
                };

                string[] array = entry.Attribute("subrisk").Value.Split('/');
                foreach (string x in array)
                    if (x != "–")
                        if (string.IsNullOrWhiteSpace(record.DgSubClassArray[0]))
                            record.DgSubClassArray[0] = x;
                        else record.DgSubClassArray[1] = x;

                var temp = entry.Attribute("MP").Value;
                if (temp == "true") record.IsMp = true;

                record.Name = entry.Element("name").Value;

                array = entry.Attribute("specialprovisions").Value.Split(' ');
                foreach (string x in array)
                    record.SpecialInherited.Add(x != "–" ? Convert.ToUInt16(x) : (ushort)0);

                record.StowageCat = entry.Element("Stowage").Attribute("category").Value.Length > 1 ?
                                    entry.Element("Stowage").Attribute("category").Value[1] :
                                    entry.Element("Stowage").Attribute("category").Value[0];

                array = entry.Element("Stowage").Attribute("provision").Value.Split(' ');
                foreach (string x in array) record.StowageSWInherited.Add(x);

                array = entry.Element("Segregation").Value.Split(' ');
                foreach (string x in array) record.SegregationSGInherited.Add(x);

                record.SetProperties(entry.Element("Propertiesandobservations").Value);
                record.DgEMS = entry.Element("EMS").Value;

                record.IsLiquidInherited = entry.Element("Liquid").Value == "true" ? true : false;
                record.IsFlammableInherited = entry.Element("Flammable").Value == "true" ? true : false;

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
        private static int ChooseOneOfMultipleEntries(List<DgFromIMDGCode> imdgRecords, byte packingGroup)
        {
            //Solving case with multiple entries found
            int orderInList = 0;
            if (imdgRecords.Count > 1)
            {
                if (packingGroup != 0)
                {

                    for (int i = 0; i < imdgRecords.Count; i++)
                    {
                        if (imdgRecords[i].PackingGroupAsByte == packingGroup)
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
                        pkg = imdgRecords[i].PackingGroupAsByte == 1
                            ? imdgRecords[i].PackingGroupAsByte
                            : (pkg < imdgRecords[i].PackingGroupAsByte ? pkg : imdgRecords[i].PackingGroupAsByte);
                        if (imdgRecords[i].PackingGroupAsByte == pkg) orderInList = i;
                    }
                }
            }

            return orderInList;
        }

        /// <summary>
        /// Method to deal with SP in subrisk in DG list.
        /// Used by raw records from IMDG code.
        /// </summary>
        private static void SpecialClass(this DgFromIMDGCode dg)
        {
            for (int i = 0; i < dg.DgSubClassInherited.Length; i++)
            {
                string s = dg.DgSubClassInherited[i];
                if (string.IsNullOrWhiteSpace(s)) continue;

                var clearSubclass = false;

                if (s == "–") 
                    dg.DgSubClassInherited[i] = null;
                else if (s.StartsWith("See SP"))
                {
                    switch (s.Remove(0, 6))
                    {
                        case "63":
                            dg.DgClass = "2.1";
                            dg.DgSubClassInherited = new string[2];
                            clearSubclass = true;
                            break;
                        case "172":
                            dg.DgSubClassInherited = new string[2];
                            clearSubclass = true;
                            break;
                        case "181":
                            dg.DgSubClassInherited = new string[2];
                            dg.DgSubClassInherited[0] = "1.3";
                            _messagesToUser.Add("Caution! Class 1.3 added in accordance with SP181.");
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
                            _messagesToUser.Add($"For subrisk refer to special provision list.");
                            break;
                    }
                }
                if (clearSubclass) break;
            }
            //unno 2037 assigned class 2 in imdg code with responsibility to determine subdivision given to shippers.
            //for the purpose of calculation, if not specified in .edi, it will be assigned the most stringent subdivision
            if (dg.Unno == 2037)
            {
                dg.DgClass = "2.1";
                _messagesToUser.Add("UNNO 2037 assigned class 2 in imdg code with responsibility to determine subdivision given to shippers.\r\n" +
                    "for the purpose of calculation, if not specified in .edi, it is assigned the most stringent subdivision");
            }
        }

        /// <summary>
        /// Updates Dg class and subclasses of dg unit with values from IMDG code, if required.
        /// </summary>
        /// <param name="unitIsNew">Bool: true - all values will be updated.</param>
        /// <param name="dgFromImdgCode">Dg record from IMDG code from which classes will be copied.</param>
        private static void UpdateDgClassAndSubclass(this Dg dg, bool unitIsNew, DgFromIMDGCode dgFromImdgCode)
        {
            if (unitIsNew || string.IsNullOrEmpty(dg.DgClass))
            {
                dg.DgClass = dgFromImdgCode.DgClass;
            }
            else
            {
                if (!string.Equals(dgFromImdgCode.DgClass, dg.DgClass))
                {
                    //AEROSOLS
                    if (dgFromImdgCode.Unno == 1950)
                    {
                        if (dg.DgClass == "2") dg.DgClass = dgFromImdgCode.DgClass;
                        else
                        {
                            _messagesToUser.Add(
                                $"Caution! For correct assigning of dg class and subrisk of AEROSOLS " +
                                $"(UNNO 1950) in unit {dg.ContainerNumber} refer to DG manifest and special provision 63 " +
                                "of IMDG code Ch 3.");
                        }
                    }
                    //Unno 2037
                    else if (dgFromImdgCode.Unno == 2037)
                        if (dg.DgClass == "2")
                            dg.DgClass = dgFromImdgCode.DgClass;
                        else
                            dg.differentClass = false;
                    //All other cases
                    else dg.differentClass = true;
                }
            }
            if (dg.DgSubclassCount == 0) dg.DgSubClassArray = dgFromImdgCode.DgSubClassInherited;
        }

        /// <summary>
        /// Updates retrieved information from IMDG code, if missing (null).
        /// </summary>
        /// <param name="dgFromImdgCode"><see cref="DgFromIMDGCode"/> record from IMDG code from which info will be copied.</param>
        private static void UpdateOtherInformation(this Dg dg, DgFromIMDGCode dgFromImdgCode)
        {
            dg.PackingGroupAsByte = dg.PackingGroupAsByte != 0 ? dg.PackingGroupAsByte : dgFromImdgCode.PackingGroupAsByte;
            if (string.IsNullOrEmpty(dg.DgEMS)) dg.DgEMS = dgFromImdgCode.DgEMS;
            dg.IsMp = dg.mpDetermined ? dg.IsMp : dgFromImdgCode.IsMp;

            dg.SetIMDGCodeValues(dgFromImdgCode);

            if (string.IsNullOrEmpty(dg.Name)) dg.Name = dgFromImdgCode.Name;
        }

        /// <summary>
        /// Copies Properties from the first entry in the series of same UNNo entries,
        /// in case Property states 'See entry above.'
        /// </summary>
        /// <param name="dgFromImdgCode"></param>
        /// <param name="imdgRecords"></param>
        /// <param name="orderInList"></param>
        private static void GetSeeEntryAboveProperty(this DgFromIMDGCode dgFromImdgCode, List<DgFromIMDGCode> imdgRecords, int orderInList)
        {
            while (dgFromImdgCode.Properties == "See entry above.")
            {
                dgFromImdgCode.SetProperties(imdgRecords[orderInList - 1].Properties);
                orderInList--;
                if (orderInList == 0) break;
            }
        }


        /// <summary>
        /// Class extends <see cref="Dg"/> only for the purpose of enable set private and readonly fields and properties from outside.
        /// </summary>
        private class DgFromIMDGCode : Dg
        {
            internal string[] DgSubClassInherited
            {
                get => dgsubclass;
                set => dgsubclass = value;
            }

            internal string DgClassInherited
            {
                get => dgclass;
                set => dgclass = value;
            }

            internal List<ushort> SpecialInherited
            {
                get => special;
                set => special = value;
            }

            internal List<string> StowageSWInherited
            {
                get => stowageSW;
                set => stowageSW = value;
            }

            internal List<string> SegregationSGInherited
            {
                get => segregationSG;
                set => segregationSG = value;
            }

            internal bool IsLiquidInherited
            {
                get => isLiquid;
                set => isLiquid = value;
            }
            internal bool IsFlammableInherited
            {
                get => isFlammable;
                set => isFlammable = value;
            }

            internal void SetProperties(string properties)
            {
                Properties = properties;
            }
        }
    }
}
