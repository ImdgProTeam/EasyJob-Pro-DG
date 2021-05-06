//TO ADD NEW EJC VERSION - READ BOTTOM OF THE FILE

using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;
using System;
using System.Globalization;
using System.IO;

namespace EasyJob_ProDG.Model.IO.EasyJobCondition
{
    /// <summary>
    /// Saves and Reads EasyJobCondition to/from text file.
    /// </summary>
    internal static class EasyJobCondition
    {
        // -------------- Public methods --------------------------------------------

        /// <summary>
        /// CargoPlan will be converted to EJC format and recorded into a new file
        /// </summary>
        /// <param name="fileName">Name of a file to be created</param>
        /// <param name="cargoPlan">CargoPlan to be recorded</param>
        public static void SaveCondition(string fileName, CargoPlan cargoPlan)
        {
            StreamWriter writer = new StreamWriter(fileName);
            WriteCargoPlanAssociatedToStream(new CargoPlanAssociated(cargoPlan), writer);
            writer.Close();
        }

        /// <summary>
        /// .ejc file will be read and a CargoPlan created.
        /// </summary>
        /// <param name="fileName">File name with full path.</param>
        /// <param name="ship">Current ShipProfile.</param>
        /// <returns>Plain CargoPlan</returns>
        public static CargoPlan LoadCondition(string fileName, ShipProfile ship)
        {
            CargoPlan cargoPlan;
            //try
            //{
            StreamReader reader = new StreamReader(fileName);


            cargoPlan = CreateCargoPlanFromStream(reader, ship);

            reader.Close();
            //}
            //catch
            //{
            //    cargoPlan = new CargoPlan();
            //}
            return cargoPlan;
        }


        // -------------- Private read - write methods ------------------------------

        /// <summary>
        /// Create records for all ConditionUnits in CargoPlanAssociated and writes them to StreamWriter
        /// </summary>
        /// <param name="cargoPlanAssociated">All ConditionUnits of CargoPlanAssociated will be recorded in writer</param>
        /// <param name="writer">StreamWriter to write the records into a file</param>
        private static void WriteCargoPlanAssociatedToStream(CargoPlanAssociated cargoPlanAssociated, StreamWriter writer)
        {
            string beginningOfFile = "EasyJob Pro DG\nCondition file\nv. 0.90\n\n| CPC |";
            writer.WriteLine(beginningOfFile);
            writer.WriteLine(CreateVoyageRecord(cargoPlanAssociated.VoyageInfo));
            foreach (var unit in cargoPlanAssociated)
            {
                writer.Write(CreateConditionUnitRecord(unit));
            }

            writer.WriteLine("\n| CPE |");
        }

        /// <summary>
        /// Reads .ejc file from stream line by line, defines version and creates CargoPlan.
        /// Reads till the next line is null.
        /// </summary>
        /// <param name="reader">Stream reader of .ejc file</param>
        /// <param name="ship">Current ShipProfile.</param>
        /// <param name="voyage">Reference to Voyage to be updated.</param>
        /// <returns>Plain CargoPlan</returns>
        private static CargoPlan CreateCargoPlanFromStream(StreamReader reader, ShipProfile ship = null)
        {
            CargoPlan cargoPlan = new CargoPlan();
            byte ejcVersion = 0;
            bool readingCondition = false;

            while (true)
            {
                var line = reader.ReadLine();
                if (line == null) break;

                //DETERMINE COMMENCEMENT OF CONDITION
                if (line == "| CPC |") readingCondition = true;

                //ENTER VERSION VALIDATION
                else if (line.StartsWith("v. "))
                {
                    switch (line.Remove(0, 3))
                    {
                        case "0.8b":
                            ejcVersion = (byte)ConditionVersion.V08b;
                            continue;
                        case "0.8c":
                            ejcVersion = (byte)ConditionVersion.V08c;
                            continue;
                        case "0.89":
                            ejcVersion = (byte)ConditionVersion.V089;
                            continue;
                        case "0.90":
                            ejcVersion = (byte)ConditionVersion.V090;
                            continue;
                        default:
                            continue;
                    }
                }

                //New record
                else if (line.StartsWith("C:"))
                {
                    //Define version of a condition file for proper parsing
                    switch (ejcVersion)
                    {
                        case (byte)ConditionVersion.V08b:
                            AddUnitToCargoPlanV08b(line, cargoPlan, ship);
                            continue;
                        case (byte)ConditionVersion.V08c:
                            AddUnitToCargoPlanV08c(line, cargoPlan, ship);
                            continue;
                        case (byte)ConditionVersion.V089:
                            AddUnitToCargoPlanV089(line, cargoPlan, ship);
                            continue;
                        case (byte)ConditionVersion.V090:
                            AddUnitToCargoPlanV090(line, cargoPlan, ship);
                            continue;
                        default:
                            continue;
                    }
                }

                //Voyage info
                else if (line.StartsWith("V:"))
                {
                    ReadVoyageInfo(line, cargoPlan.VoyageInfo);
                    continue;
                }

                //Cargo plan end
                else if (line == "| CPE |") break;

            }

            return cargoPlan;
        }


        /// <summary>
        /// Reads line related to Voyage and updates respective voyage properties.
        /// </summary>
        /// <param name="line">Line containing voyage info.</param>
        /// <param name="voyage">Voyage to be updated.</param>
        private static void ReadVoyageInfo(string line, Voyage voyage = null)
        {
            if (voyage == null) return;

            //V:VOYAGENUMBER|PORTOFDEPARTURE|PORTOFDESTINATION|
            string[] readVoyage = line.Replace("V:", "").Split('|');
            voyage.VoyageNumber = readVoyage[0];
            voyage.PortOfDeparture = readVoyage[1];
            voyage.PortOfDestination = readVoyage[2];
        }


        // -------------- VARIOUS VERSIONS OF EJC -----------------------------------

        /// <summary>
        /// Parses a single line of .ejc and converts it into Container, Reefer and Dg and records to cargo plan
        /// </summary>
        /// <param name="line">The line will be parsed and converted into new instances</param>
        /// <param name="cargoPlan">New instances will be recorded in the cargoPlan</param>
        /// <param name="ship">Current ShipProfile.</param>
        private static void AddUnitToCargoPlanV08b(string line, CargoPlan cargoPlan, ShipProfile ship = null)
        {
            string[] segmentArray = line.Split('|');
            var container = new Container();
            Dg dg = new Dg();
            int count = 0;

            foreach (var segment in segmentArray)
            {
                switch (count)
                {
                    #region Container
                    case 0:
                        {
                            if (!segment.StartsWith("C:")) break;
                            container.ContainerNumber = segment.Remove(0, 2);
                            count++;
                            break;
                        }
                    case 1:
                        {
                            container.Location = segment;
                            if (ship != null)
                            {
                                container.HoldNr = ship.DefineCargoHoldNumber(container.Bay);
                            }
                            count++;
                            break;
                        }
                    case 2:
                        {
                            container.POL = segment;
                            count++;
                            break;
                        }
                    case 3:
                        {
                            container.POD = segment;
                            count++;
                            break;
                        }
                    case 4:
                        {
                            container.FinalDestination = segment;
                            count++;
                            break;
                        }
                    case 5:
                        {
                            container.Carrier = segment;
                            count++;
                            break;
                        }
                    case 6:
                        {
                            container.ContainerType = segment;
                            count++;
                            break;
                        }
                    case 7:
                        {
                            container.IsClosed = segment != "O";
                            count++;
                            break;
                        }
                    #endregion

                    #region case 8 (Reefer or Dg)
                    //Reefer or Dg
                    case 8:
                        {
                            if (segment.StartsWith("R:"))
                            {
                                container.IsRf = true;
                                container.SetTemperature = double.Parse(segment.Remove(0, 2));
                                cargoPlan.Reefers.Add(container);
                                count++;
                                break;
                            }

                            if (segment.StartsWith("D:"))
                            {
                                count++;
                                goto case 9;
                            }
                            break;
                        }
                    #endregion

                    #region Dg
                    case 9:
                        {
                            if (segment.StartsWith("D:"))
                            {
                                dg = new Dg()
                                {
                                    Unno = int.Parse(segment.Remove(0, 2))
                                };
                                dg.AssignSegregationGroup();
                                count++;
                            }
                            break;
                        }
                    case 10:
                        {
                            dg.DgClass = segment;
                            count++;
                            break;
                        }
                    case 11:
                        {
                            dg.DgSubclass = segment;
                            count++;
                            break;
                        }
                    case 12:
                        {
                            dg.DgNetWeight = decimal.Parse(segment);
                            count++;
                            break;
                        }
                    case 13:
                        {
                            dg.PackingGroup = segment;
                            count++;
                            break;
                        }
                    case 14:
                        {
                            dg.FlashPoint = segment;
                            count++;
                            break;
                        }
                    case 15:
                        {
                            dg.IsMp = (segment == "P");
                            count++;
                            break;
                        }
                    case 16:
                        {
                            dg.IsLq = (segment == "LQ");
                            count++;
                            break;
                        }
                    case 17:
                        {
                            if (!segment.StartsWith("\"(")) break;
                            dg.Name = segment.Replace("\"(", "").Replace(")\"", "");
                            count++;
                            break;
                        }
                    case 18:
                        {
                            dg.IsNameChanged = (segment == "Y");
                            count++;
                            break;

                        }
                    case 19:
                        {
                            dg.StowageCat = Char.Parse(segment.Replace("'", ""));
                            count++;
                            break;
                        }

                    //In case there are more than one Dg in Container
                    case 20:
                        {
                            if (segment.StartsWith("D:"))
                            {
                                AddDgToCargoPlan(dg, container, cargoPlan);
                                count = 9;
                                goto case 9;
                            }
                            break;
                        }
                    #endregion

                    default:
                        continue;
                }
            }

            cargoPlan.Containers.Add(container);

            if (count > 10)
            {
                AddDgToCargoPlan(dg, container, cargoPlan);
            }
        }

        /// <summary>
        /// Parses a single line of .ejc and converts it into Container, Reefer and Dg and records to cargo plan
        /// </summary>
        /// <param name="line">The line will be parsed and converted into new instances</param>
        /// <param name="cargoPlan">New instances will be recorded in the cargoPlan</param>
        /// <param name="ship">Current ShipProfile</param>
        private static void AddUnitToCargoPlanV08c(string line, CargoPlan cargoPlan, ShipProfile ship = null)
        {
            string[] segmentArray = line.Split('|');
            var container = new Container();
            Dg dg = new Dg();
            int count = 0;

            foreach (var segment in segmentArray)
            {
                switch (count)
                {
                    #region Container
                    case 0:
                        {
                            if (!segment.StartsWith("C:")) break;
                            container.ContainerNumber = segment.Remove(0, 2);
                            count++;
                            break;
                        }
                    case 1:
                        {
                            container.Location = segment;
                            if (ship != null)
                            {
                                container.HoldNr = ship.DefineCargoHoldNumber(container.Bay);
                            }
                            count++;
                            break;
                        }
                    case 2:
                        {
                            container.POL = segment;
                            count++;
                            break;
                        }
                    case 3:
                        {
                            container.POD = segment;
                            count++;
                            break;
                        }
                    case 4:
                        {
                            container.FinalDestination = segment;
                            count++;
                            break;
                        }
                    case 5:
                        {
                            container.Carrier = segment;
                            count++;
                            break;
                        }
                    case 6:
                        {
                            container.ContainerType = segment;
                            count++;
                            break;
                        }
                    case 7:
                        {
                            container.IsClosed = segment != "O";
                            count++;
                            break;
                        }
                    #endregion

                    #region IUpdatable
                    case 8:
                        {
                            if (segment.StartsWith("U:"))
                            {
                                container.IsPositionLockedForChange = int.Parse(segment.Remove(0, 2)) == 1;
                                count++;
                            }
                            else throw new Exception("Condition file you're trying to read is possibly corrupt or wrong version");
                            break;
                        }
                    case 9:
                        {
                            container.IsToBeKeptInPlan = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 10:
                        {
                            container.IsNewUnitInPlan = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 11:
                        {
                            container.HasLocationChanged = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 12:
                        {
                            container.HasUpdated = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    #endregion

                    #region case 13 (Reefer or Dg)
                    //Reefer or Dg
                    case 13:
                        {
                            if (segment.StartsWith("R:"))
                            {
                                container.IsRf = true;
                                container.SetTemperature = double.Parse(segment.Remove(0, 2));
                                cargoPlan.Reefers.Add(container);
                                count++;
                                break;
                            }

                            if (segment.StartsWith("D:"))
                            {
                                count++;
                                goto case 14;
                            }
                            break;
                        }
                    #endregion

                    #region Dg
                    case 14:
                        {
                            if (segment.StartsWith("D:"))
                            {
                                dg = new Dg()
                                {
                                    Unno = int.Parse(segment.Remove(0, 2))
                                };
                                dg.AssignSegregationGroup();
                                count++;
                            }
                            break;
                        }
                    case 15:
                        {
                            dg.DgClass = segment;
                            count++;
                            break;
                        }
                    case 16:
                        {
                            dg.DgSubclass = segment;
                            count++;
                            break;
                        }
                    case 17:
                        {
                            dg.DgNetWeight = decimal.Parse(segment);
                            count++;
                            break;
                        }
                    case 18:
                        {
                            dg.PackingGroup = segment;
                            count++;
                            break;
                        }
                    case 19:
                        {
                            dg.FlashPoint = segment;
                            count++;
                            break;
                        }
                    case 20:
                        {
                            dg.IsMp = (segment == "P");
                            dg.mpDetermined = true;
                            count++;
                            break;
                        }
                    case 21:
                        {
                            dg.IsLq = (segment == "LQ");
                            count++;
                            break;
                        }
                    case 22:
                        {
                            if (!segment.StartsWith("\"(")) break;
                            dg.Name = segment.Replace("\"(", "").Replace(")\"", "");
                            count++;
                            break;
                        }
                    case 23:
                        {
                            dg.IsNameChanged = (segment == "Y");
                            count++;
                            break;

                        }
                    case 24:
                        {
                            dg.StowageCat = Char.Parse(segment.Replace("'", ""));
                            count++;
                            break;
                        }
                    case 25:
                        {
                            dg.IsMax1L = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 26:
                        {
                            dg.IsWaste = (segment == "W");
                            count++;
                            break;
                        }

                    //In case there are more than one Dg in Container
                    case 27:
                        {
                            if (segment.StartsWith("D:"))
                            {
                                AddDgToCargoPlan(dg, container, cargoPlan);
                                count = 14;
                                goto case 14;
                            }
                            break;
                        }
                    #endregion

                    default:
                        continue;
                }
            }

            cargoPlan.Containers.Add(container);

            if (count > 15)
            {
                AddDgToCargoPlan(dg, container, cargoPlan);
            }
        }


        /// <summary>
        /// Parses a single line of .ejc and converts it into Container, Reefer and Dg and records to cargo plan
        /// </summary>
        /// <param name="line">The line will be parsed and converted into new instances</param>
        /// <param name="cargoPlan">New instances will be recorded in the cargoPlan</param>
        /// <param name="ship">Current ShipProfile</param>
        private static void AddUnitToCargoPlanV089(string line, CargoPlan cargoPlan, ShipProfile ship = null)
        {
            string[] segmentArray = line.Split('|');
            var container = new Container();
            Dg dg = new Dg();
            int count = 0;

            foreach (var segment in segmentArray)
            {
                switch (count)
                {
                    #region Container
                    case 0: //Container number
                        {
                            if (!segment.StartsWith("C:")) break;
                            container.ContainerNumber = segment.Remove(0, 2);
                            count++;
                            break;
                        }
                    case 1: //Location
                        {
                            container.Location = segment;
                            if (ship != null)
                            {
                                container.HoldNr = ship.DefineCargoHoldNumber(container.Bay);
                            }
                            count++;
                            break;
                        }
                    case 2: //POL
                        {
                            container.POL = segment;
                            count++;
                            break;
                        }
                    case 3: //POD
                        {
                            container.POD = segment;
                            count++;
                            break;
                        }
                    case 4: //FinalDestination
                        {
                            container.FinalDestination = segment;
                            count++;
                            break;
                        }
                    case 5: //Carrier
                        {
                            container.Carrier = segment;
                            count++;
                            break;
                        }
                    case 6: //Type
                        {
                            container.ContainerType = segment;
                            count++;
                            break;
                        }
                    case 7: //IsClosed
                        {
                            container.IsClosed = segment != "O";
                            count++;
                            break;
                        }
                    case 8: //Container remarks
                        {
                            container.Remarks = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    #endregion

                    //reserved position
                    case 9:
                        count++;
                        goto case 10;

                    #region IUpdatable
                    case 10: //IsLocked
                        {
                            if (segment.StartsWith("U:"))
                            {
                                container.IsPositionLockedForChange = int.Parse(segment.Remove(0, 2)) == 1;
                                count++;
                            }
                            else throw new Exception("Condition file you're trying to read is possibly corrupt or wrong version");
                            break;
                        }
                    case 11: //IsToBeKeptInPlan
                        {
                            container.IsToBeKeptInPlan = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 12: //IsNotToImport
                        {
                            container.IsNotToImport = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 13: //IsNewInPlan
                        {
                            container.IsNewUnitInPlan = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 14: //HasLocationChanged
                        {
                            container.HasLocationChanged = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 15: //HasUpdated
                        {
                            container.HasUpdated = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 16: //HasPodChanged
                        {
                            container.HasPodChanged = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 17: //HasContainerTypeChanged
                        {
                            container.HasContainerTypeChanged = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    #endregion

                    //reserved positions
                    case 18:
                    case 19:
                        count = 20;
                        goto case 20;

                    #region case 20 Reefer (or Dg)
                    //Reefer or Dg
                    case 20: //Reefer: Set Point
                        {
                            //In case of reefer
                            if (segment.StartsWith("R:"))
                            {
                                container.IsRf = true;
                                container.ResetReefer();

                                container.SetTemperature = double.Parse(segment.Remove(0, 2));

                                count++;
                            }

                            //In case of Dg
                            else if (segment.StartsWith("D:"))
                            {
                                count = 40;
                                goto case 40;
                            }

                            break;
                        }
                    case 21: //Vent settings
                        {
                            container.VentSetting = segment;
                            count++;
                            break;
                        }
                    case 22: //Commodity
                        {
                            container.Commodity = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 23: //Loading temperature
                        {
                            container.LoadTemperature = double.Parse(segment);
                            count++;
                            break;
                        }
                    case 24: //Special
                        {
                            container.ReeferSpecial = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 25: //Reefer remark
                        {
                            container.ReeferRemark = ConvertNewLineSymbolsOfRecord(segment);

                            cargoPlan.Reefers.Add(container);

                            count++;
                            break;
                        }
                    #endregion

                    //reserved
                    case 26:
                        count = 40;
                        goto case 40;

                    #region Dg
                    case 40: //Unno
                        {
                            if (segment.StartsWith("D:"))
                            {
                                dg = new Dg()
                                {
                                    Unno = int.Parse(segment.Remove(0, 2))
                                };
                                dg.AssignSegregationGroup();
                                count++;
                            }
                            break;
                        }
                    case 41: //DgClass
                        {
                            dg.DgClass = segment;
                            count++;
                            break;
                        }
                    case 42: //DgSubClasses
                        {
                            dg.DgSubclass = segment;
                            count++;
                            break;
                        }
                    case 43: //Net weight
                        {
                            dg.DgNetWeight = decimal.Parse(segment);
                            count++;
                            break;
                        }
                    case 44: //Packing group
                        {
                            dg.PackingGroup = segment;
                            count++;
                            break;
                        }
                    case 45: //Flash point
                        {
                            dg.FlashPoint = segment;
                            count++;
                            break;
                        }
                    case 46: //Marine pollutant
                        {
                            dg.IsMp = (segment == "P");
                            dg.mpDetermined = true;
                            count++;
                            break;
                        }
                    case 47: //Limited quantity
                        {
                            dg.IsLq = (segment == "LQ");
                            count++;
                            break;
                        }
                    case 48: //ProperShippingName
                        {
                            if (!segment.StartsWith("\"(")) break;
                            dg.Name = ConvertNewLineSymbolsOfRecord
                                (segment.Replace("\"(", "").Replace(")\"", ""));
                            count++;
                            break;
                        }
                    case 49: //Technical name
                        {
                            dg.TechnicalName = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 50: //IsNameChanged
                        {
                            dg.IsNameChanged = (segment == "Y");
                            count++;
                            break;

                        }
                    case 51: //IsTechnicalNameIncluded
                        {
                            dg.IsTechnicalNameIncluded = (segment == "Y");
                            count++;
                            break;

                        }
                    case 52: //Stowage category
                        {
                            dg.StowageCat = Char.Parse(segment.Replace("'", ""));
                            count++;
                            break;
                        }
                    case 53: //Max 1 l
                        {
                            dg.IsMax1L = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 54: //IsWaste
                        {
                            dg.IsWaste = (segment == "W");
                            count++;
                            break;
                        }
                    case 55: //Ems
                        {
                            dg.DgEMS = segment;
                            count++;
                            break;
                        }
                    case 56: //Segregation group
                        {
                            dg.SegregationGroup = segment;
                            count++;
                            break;
                        }
                    case 57: //Packages
                        {
                            dg.NumberAndTypeOfPackages = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 58: //Emergency contacts
                        {
                            dg.EmergencyContacts = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 59: //Dg remarks
                        {
                            dg.Remarks = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }

                    //In case there are more than one Dg in Container
                    case 60:
                        {
                            if (segment.StartsWith("D:"))
                            {
                                AddDgToCargoPlan(dg, container, cargoPlan);
                                count = 40;
                                goto case 40;
                            }
                            break;
                        }
                    #endregion

                    default:
                        continue;
                }
            }

            cargoPlan.Containers.Add(container);

            if (count > 41)
            {
                AddDgToCargoPlan(dg, container, cargoPlan);
            }
        }

        /// <summary>
        /// Parses a single line of .ejc and converts it into Container, Reefer and Dg and records to cargo plan
        /// </summary>
        /// <param name="line">The line will be parsed and converted into new instances</param>
        /// <param name="cargoPlan">New instances will be recorded in the cargoPlan</param>
        /// <param name="ship">Current ShipProfile</param>
        private static void AddUnitToCargoPlanV090(string line, CargoPlan cargoPlan, ShipProfile ship = null)
        {

            string[] segmentArray = line.Split('|');
            var container = new Container();
            Dg dg = new Dg();
            int count = 0;

            foreach (var segment in segmentArray)
            {
                switch (count)
                {
                    #region Container
                    case 0: //Container number
                        {
                            if (!segment.StartsWith("C:")) break;
                            container.ContainerNumber = segment.Remove(0, 2);
                            count++;
                            break;
                        }
                    case 1: //Location
                        {
                            container.Location = segment;
                            if (ship != null)
                            {
                                container.HoldNr = ship.DefineCargoHoldNumber(container.Bay);
                            }
                            count++;
                            break;
                        }
                    case 2: //POL
                        {
                            container.POL = segment;
                            count++;
                            break;
                        }
                    case 3: //POD
                        {
                            container.POD = segment;
                            count++;
                            break;
                        }
                    case 4: //FinalDestination
                        {
                            container.FinalDestination = segment;
                            count++;
                            break;
                        }
                    case 5: //Carrier
                        {
                            container.Carrier = segment;
                            count++;
                            break;
                        }
                    case 6: //Type
                        {
                            container.ContainerType = segment;
                            count++;
                            break;
                        }
                    case 7: //IsClosed
                        {
                            container.IsClosed = segment != "O";
                            count++;
                            break;
                        }
                    case 8: //Container remarks
                        {
                            container.Remarks = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 9: //Old location
                        {
                            container.LocationBeforeRestow = segment;
                            count++;
                            break;
                        }
                    #endregion


                    #region IUpdatable
                    case 10: //IsLocked
                        {
                            if (segment.StartsWith("U:"))
                            {
                                container.IsPositionLockedForChange = int.Parse(segment.Remove(0, 2)) == 1;
                                count++;
                            }
                            else throw new Exception("Condition file you're trying to read is possibly corrupt or wrong version");
                            break;
                        }
                    case 11: //IsToBeKeptInPlan
                        {
                            container.IsToBeKeptInPlan = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 12: //IsToImport
                        {
                            container.IsToImport = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 13: //IsNotToImport
                        {
                            container.IsNotToImport = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 14: //IsNewInPlan
                        {
                            container.IsNewUnitInPlan = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 15: //HasLocationChanged
                        {
                            container.HasLocationChanged = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 16: //HasUpdated
                        {
                            container.HasUpdated = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 17: //HasPodChanged
                        {
                            container.HasPodChanged = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 18: //HasContainerTypeChanged
                        {
                            container.HasContainerTypeChanged = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    #endregion

                    //reserved positions
                    case 19:
                        count = 20;
                        goto case 20;

                    #region case 20 Reefer (or Dg)
                    //Reefer or Dg
                    case 20: //Reefer: Set Point
                        {
                            //In case of reefer
                            if (segment.StartsWith("R:"))
                            {
                                container.IsRf = true;
                                container.ResetReefer();

                                container.SetTemperature = double.Parse(segment.Remove(0, 2));

                                count++;
                            }

                            //In case of Dg
                            else if (segment.StartsWith("D:"))
                            {
                                count = 40;
                                goto case 40;
                            }

                            break;
                        }
                    case 21: //Vent settings
                        {
                            container.VentSetting = segment;
                            count++;
                            break;
                        }
                    case 22: //Commodity
                        {
                            container.Commodity = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 23: //Loading temperature
                        {
                            container.LoadTemperature = double.Parse(segment);
                            count++;
                            break;
                        }
                    case 24: //Special
                        {
                            container.ReeferSpecial = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 25: //Reefer remark
                        {
                            container.ReeferRemark = ConvertNewLineSymbolsOfRecord(segment);

                            cargoPlan.Reefers.Add(container);

                            count++;
                            break;
                        }
                    #endregion

                    //reserved
                    case 26:
                        count = 40;
                        goto case 40;

                    #region Dg
                    case 40: //Unno
                        {
                            if (segment.StartsWith("D:"))
                            {
                                dg = new Dg()
                                {
                                    Unno = int.Parse(segment.Remove(0, 2))
                                };
                                dg.AssignSegregationGroup();
                                count++;
                            }
                            break;
                        }
                    case 41: //DgClass
                        {
                            dg.DgClass = segment;
                            count++;
                            break;
                        }
                    case 42: //DgSubClasses
                        {
                            dg.DgSubclass = segment;
                            count++;
                            break;
                        }
                    case 43: //Net weight
                        {
                            dg.DgNetWeight = decimal.Parse(segment);
                            count++;
                            break;
                        }
                    case 44: //Packing group
                        {
                            dg.PackingGroup = segment;
                            count++;
                            break;
                        }
                    case 45: //Flash point
                        {
                            dg.FlashPoint = segment;
                            count++;
                            break;
                        }
                    case 46: //Marine pollutant
                        {
                            dg.IsMp = (segment == "P");
                            dg.mpDetermined = true;
                            count++;
                            break;
                        }
                    case 47: //Limited quantity
                        {
                            dg.IsLq = (segment == "LQ");
                            count++;
                            break;
                        }
                    case 48: //ProperShippingName
                        {
                            if (!segment.StartsWith("\"(")) break;
                            dg.Name = ConvertNewLineSymbolsOfRecord
                                (segment.Replace("\"(", "").Replace(")\"", ""));
                            count++;
                            break;
                        }
                    case 49: //Technical name
                        {
                            dg.TechnicalName = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 50: //IsNameChanged
                        {
                            dg.IsNameChanged = (segment == "Y");
                            count++;
                            break;

                        }
                    case 51: //IsTechnicalNameIncluded
                        {
                            dg.IsTechnicalNameIncluded = (segment == "Y");
                            count++;
                            break;

                        }
                    case 52: //Stowage category
                        {
                            dg.StowageCat = Char.Parse(segment.Replace("'", ""));
                            count++;
                            break;
                        }
                    case 53: //Max 1 l
                        {
                            dg.IsMax1L = int.Parse(segment) == 1;
                            count++;
                            break;
                        }
                    case 54: //IsWaste
                        {
                            dg.IsWaste = (segment == "W");
                            count++;
                            break;
                        }
                    case 55: //Ems
                        {
                            dg.DgEMS = segment;
                            count++;
                            break;
                        }
                    case 56: //Segregation group
                        {
                            dg.SegregationGroup = segment;
                            count++;
                            break;
                        }
                    case 57: //Packages
                        {
                            dg.NumberAndTypeOfPackages = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 58: //Emergency contacts
                        {
                            dg.EmergencyContacts = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }
                    case 59: //Dg remarks
                        {
                            dg.Remarks = ConvertNewLineSymbolsOfRecord(segment);
                            count++;
                            break;
                        }

                    //In case there are more than one Dg in Container
                    case 60:
                        {
                            if (segment.StartsWith("D:"))
                            {
                                AddDgToCargoPlan(dg, container, cargoPlan);
                                count = 40;
                                goto case 40;
                            }
                            break;
                        }
                    #endregion

                    default:
                        continue;
                }

            }

            cargoPlan.Containers.Add(container);

            if (count > 41)
            {
                AddDgToCargoPlan(dg, container, cargoPlan);
            }

        }

        // -------------- Supporting methods ----------------------------------------

        /// <summary>
        /// Returns string with Voyage info coded
        /// </summary>
        /// <param name="voyageInfo"></param>
        /// <returns></returns>
        private static string CreateVoyageRecord(Voyage voyageInfo)
        {
            if (voyageInfo == null)
                return "V:|||";

            RecordsCreator.AddNewSpecifiedRecord("V");
            RecordsCreator.AppendRecord(voyageInfo.VoyageNumber);
            RecordsCreator.AppendRecord(voyageInfo.PortOfDeparture);
            RecordsCreator.AppendRecord(voyageInfo.PortOfDestination);

            return RecordsCreator.ReturnEntry();
        }

        /// <summary>
        /// Creates a record from a condition unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>String record for a single unit</returns>
        private static string CreateConditionUnitRecord(ConditionUnit unit)
        {
            RecordsCreator.AddNewRecord();
            RecordsCreator.AppendRecord(unit.ContainerNumber);
            RecordsCreator.AppendRecord(unit.Location);
            RecordsCreator.AppendRecord(unit.POL);
            RecordsCreator.AppendRecord(unit.POD);
            RecordsCreator.AppendRecord(unit.FinalDestination);
            RecordsCreator.AppendRecord(unit.Carrier);
            RecordsCreator.AppendRecord(unit.ContainerType);
            RecordsCreator.AppendRecord(unit.IsClosed ? "C" : "O");
            RecordsCreator.AppendRecord(ConvertNewLineSymbolsOfRecord(unit.Remarks));
            RecordsCreator.AppendRecord(unit.LocationBeforeRestow);

            RecordsCreator.AppendWitNewType("U");
            RecordsCreator.AppendRecord(unit.IsPositionLockedForChange ? "1" : "0");
            RecordsCreator.AppendRecord(unit.IsToBeKeptInPlan ? "1" : "0");
            RecordsCreator.AppendRecord(unit.IsToImport ? "1" : "0");
            RecordsCreator.AppendRecord(unit.IsNotToImport ? "1" : "0");
            RecordsCreator.AppendRecord(unit.IsNewUnitInPlan ? "1" : "0");
            RecordsCreator.AppendRecord(unit.HasLocationChanged ? "1" : "0");
            RecordsCreator.AppendRecord(unit.HasUpdated ? "1" : "0");
            RecordsCreator.AppendRecord(unit.HasPodChanged ? "1" : "0");
            RecordsCreator.AppendRecord(unit.HasContainerTypeChanged ? "1" : "0");

            if (unit.IsRf)
            {
                RecordsCreator.AppendWitNewType("R");
                RecordsCreator.AppendRecord(unit.SetTemperature.ToString(CultureInfo.InvariantCulture));
                RecordsCreator.AppendRecord(unit.VentSetting);
                RecordsCreator.AppendRecord(ConvertNewLineSymbolsOfRecord(unit.Commodity));
                RecordsCreator.AppendRecord(unit.LoadTemperature.ToString(CultureInfo.InvariantCulture));
                RecordsCreator.AppendRecord(ConvertNewLineSymbolsOfRecord(unit.ReeferSpecial));
                RecordsCreator.AppendRecord(ConvertNewLineSymbolsOfRecord(unit.ReeferRemark));
            }

            if (unit.DgCountInContainer > 0)
            {
                foreach (var dg in unit.DgCargoInContainer)
                {
                    RecordsCreator.AppendWitNewType("D");
                    RecordsCreator.AppendRecord(dg.Unno.ToString());
                    RecordsCreator.AppendRecord(dg.DgClass);
                    RecordsCreator.AppendRecord(dg.DgSubclass);
                    RecordsCreator.AppendRecord(dg.DgNetWeight.ToString(CultureInfo.InvariantCulture));
                    RecordsCreator.AppendRecord(dg.PackingGroupByte.ToString());
                    RecordsCreator.AppendRecord(dg.FlashPoint);
                    RecordsCreator.AppendRecord(dg.IsMp ? "P" : "N");
                    RecordsCreator.AppendRecord(dg.IsLq ? "LQ" : "N");
                    RecordsCreator.AppendRecord(ConvertNewLineSymbolsOfRecord("\"(" + dg.Name + ")\""));
                    RecordsCreator.AppendRecord(ConvertNewLineSymbolsOfRecord(dg.TechnicalName));
                    RecordsCreator.AppendRecord(dg.IsNameChanged ? "Y" : "N");
                    RecordsCreator.AppendRecord(dg.IsTechnicalNameIncluded ? "Y" : "N");
                    RecordsCreator.AppendRecord("\'" + dg.StowageCat + "\'");
                    RecordsCreator.AppendRecord(dg.IsMax1L ? "1" : "0");
                    RecordsCreator.AppendRecord(dg.IsWaste ? "W" : "0");
                    RecordsCreator.AppendRecord(RemoveNewLines(dg.DgEMS));
                    RecordsCreator.AppendRecord(dg.SegregationGroupCodes);
                    RecordsCreator.AppendRecord(ConvertNewLineSymbolsOfRecord(dg.NumberAndTypeOfPackages));
                    RecordsCreator.AppendRecord(ConvertNewLineSymbolsOfRecord(dg.EmergencyContacts));
                    RecordsCreator.AppendRecord(ConvertNewLineSymbolsOfRecord(dg.Remarks));
                }
            }

            return RecordsCreator.ReturnEntry();
        }

        /// <summary>
        /// Uploads dg to container and to DgList of cargoPlan
        /// </summary>
        /// <param name="dg">Dg will be uploaded</param>
        /// <param name="container">Container info will be updated</param>
        /// <param name="cargoPlan">Dg will be added to DgList</param>
        private static void AddDgToCargoPlan(Dg dg, Container container, CargoPlan cargoPlan)
        {
            dg.CopyContainerInfo(container);
            cargoPlan.DgList.Add(dg);
            container.DgCountInContainer++;
        }

        /// <summary>
        /// Converts system newLine symbols into ConditionUnit specified ones in string record, and vice versa.
        /// </summary>
        /// <param name="line">Line with symbols to be converted.</param>
        /// <returns>New line with converted symbols.</returns>
        private static string ConvertNewLineSymbolsOfRecord(string line)
        {
            if (string.IsNullOrEmpty(line)) return line;

            const string symbolOfNewLine = "<#ln!>";

            if (line.Contains("\n"))
            {
                return line.Replace("\n", symbolOfNewLine);
            }
            else if (line.Contains(symbolOfNewLine))
            {
                return line.Replace(symbolOfNewLine, "\n");
            }
            else
            {
                return line;
            }
        }

        /// <summary>
        /// Removes all symbols for new lines from original line.
        /// </summary>
        /// <param name="line">Original line.</param>
        /// <returns>Original line without new line symbols.</returns>
        private static string RemoveNewLines(string line)
        {
            return line.Replace("\n", "").Replace("\r", "");
        }

        // -------------- Versions enumeration --------------------------------------

        /// <summary>
        /// Enumeration of known .ejc versions
        /// </summary>
        private enum ConditionVersion : byte
        {
            V08b = 0,
            V08c,
            V089,
            V090
        }
    }
}

//TO ADD NEW VERSION
//
// 0. Type new format in EJC Template
//
// 1. Create new version in ConditionVersion() enumeration
//
// 2. Create new method AddUnitToCargoPlanXXX() for handling new version
//
// 3. Amend method CreateConditionUnitRecord() to record as per new format
//
// 4. Add new cases into method CreateCargoPlanFromStream()
//
// 5. Amend version number in method WriteCargoPlanAssociatedToStream()
//
// 6. Amend ConditionUnit.ToConditionUnit(Container)
//
// 7. Amend Dg.CopyContainerInfo(Container)
