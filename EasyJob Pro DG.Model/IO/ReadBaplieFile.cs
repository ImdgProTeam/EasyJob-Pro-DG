using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.Model.IO
{
    /// <summary>
    /// Class contains methods to read and parse .edi file and create dg list, list of all containers and reefer list. As well, voyage data will be updated from the .edi file.
    /// </summary>
    public class ReadBaplieFile
    {
        #region fields
        private string _segmentDef;
        readonly EdiSegmentArray _segmentArray;
        private readonly ShipProfile _ship;

        public int ContainerCount;
        public int DgContainerCount;
        public int RfContainerCount;
        public int ContainersLoaded;
        public int DgContainersLoaded;
        public int RfContainersLoaded;
        private CargoPlan cargoPlan;
        public List<string> WrongList;
        public int WrongContainers;
        #endregion

        public CargoPlan GetCargoPlan()
        {
            return cargoPlan;
        }

        public Voyage GetVoyage()
        {
            return cargoPlan.VoyageInfo;
        }

        /// <summary>
        /// The constructor will read .edi file, define segment information, and will display working information and summary.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ship"></param>
        public ReadBaplieFile(string file, ShipProfile ship, ref bool isIftdgn)
        {
            //TODO: Change constructor to method returning CargoPlan

            _ship = ship;

            //read from file
            var reader = new StreamReader(file);
            string text = reader.ReadToEnd();
            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            _segmentArray = new EdiSegmentArray(text);
            reader.Close();

            //Print working information
            //Output.DisplayLine("\nTotal {0} segments.", segmentArray.count);
            //Output.Display("\nTotal {0} containers.", segmentArray.containerCount.ToString());
            //Output.DisplayLine("\nEnd of Reader.\nPress any key to complete container list");
            //Output.ReadKey();

            //Create container list
            cargoPlan = new CargoPlan();
            WrongList = new List<string>();
            WrongContainers = 0;

            if (!isIftdgn)
                if (!DefineSegments())
                {
                    isIftdgn = true;
                }

            if(isIftdgn) 
                 cargoPlan = ReadIftdgnFile.ReadSegments(_segmentArray, _ship);

            //Print wrong container numbers info
            //if (wrongContainers > 0)
            //{
            //    Style.ErrorStyle(string.Concat("\nContainers with wrong number: ", wrongContainers));
            //    Program.textToExport.AppendLine().Append("Containers with wrong number: " + wrongContainers)
            //        .AppendLine();
            //    Style.QuestionStyle("Do you want to print the list? Y/N");
            //    if (Output.IfReadKey('y', false))
            //        foreach (string number in wrongList)
            //            Style.GreenStyle(number);
            //}

            //Gathering summary of containers
            foreach (Container unit in cargoPlan.Containers)
            {
                if (unit.POL != cargoPlan.VoyageInfo.PortOfDeparture) continue;
                ContainersLoaded++;
                if (unit.DgCountInContainer > 0)
                    DgContainersLoaded++;
                if (unit.IsRf)
                    RfContainersLoaded++;
            }
        }

        /// <summary>
        /// Method defines what information is contained in a segment and updates respective fields of dg list, container list or vessel information
        /// </summary>
        private bool DefineSegments()
        {
            Container a = new Container();
            int dateLevel = 0;

            for (int i = 0; i < _segmentArray.Count; i++)
            {
                string segment = _segmentArray.Array[i];
                _segmentDef = segment.Length > 3 ? segment.Remove(3) : _segmentArray.Array[i];

                switch (_segmentDef)
                {
                    #region IFTDGN
                    case "UNH":
                        if (segment.Contains("IFTDGN"))
                            return false;
                        break;
                    #endregion

                    #region Location
                    case "LOC":
                        if (segment.StartsWith("LOC+5"))
                        {
                            //Port of departure
                            if (segment.Substring(6).Contains(":"))
                                segment = segment.Substring(0, segment.IndexOf(':'));
                            cargoPlan.VoyageInfo.PortOfDeparture = segment.Length > 10 ? segment.Substring(6, 5) : segment.Substring(6, segment.Length - 6);
                            break;
                        }
                        else if (segment.StartsWith("LOC+61"))
                        {
                            //Next port of call
                            cargoPlan.VoyageInfo.PortOfDestination = segment.Length > 11 ? segment.Substring(7, 5) : segment.Substring(7, segment.Length - 7);
                            break;
                        }
                        else if (segment.StartsWith("LOC+147"))
                        {
                            //Container location
                            if (a.Location != null)
                            {
                                ContainerCount++;
                                //if (!a.typeRecognized && a.DgInContainer > 0)
                                //{
                                //    Style.FontColor("Red");
                                //    Output.DisplayLine(
                                //        "Attention! Container {0} has not recognized type {1}. It will be considered as 'closed freight container'", new string[] { a.Number, a.ctnrType });
                                //    Output.ReadKey();

                                //}
                            }
                            a = new Container { Location = segment.Substring(8, 7) };
                            a.HoldNr = _ship.DefineCargoHoldNumber(a.Bay);
                            cargoPlan.Containers.Add(a);
                            break;
                        }
                        else if (segment.StartsWith("LOC+6+"))
                        {
                            //POL
                            a.POL = segment.Length > 10 ? segment.Substring(6, 5) : segment.Substring(6, segment.Length - 6);
                            break;
                        }
                        else if (segment.StartsWith("LOC+9+"))
                        {
                            //POL
                            a.POL = segment.Length > 10 ? segment.Substring(6, 5) : segment.Substring(6, segment.Length - 6);
                            break;
                        }
                        else if (segment.StartsWith("LOC+11"))
                        {
                            //POD
                            a.POD = segment.Length > 11 ? segment.Substring(7, 5) : segment.Substring(7);
                            break;
                        }
                        else if (segment.StartsWith("LOC+12"))
                        {
                            //POD
                            a.POD = segment.Length > 11 ? segment.Substring(7, 5) : segment.Substring(7, segment.Length - 7);
                            break;
                        }
                        else if (segment.StartsWith("LOC+83"))
                        {
                            //Final destination
                            a.FinalDestination = segment.Length > 11 ? segment.Substring(7, 5) : segment.Substring(7, segment.Length - 7);
                            break;
                        }
                        else
                            break;
                    #endregion

                    #region Dates
                    case "DTM":

                        DateTime dateValue;

                        try
                        {
                            int dateFormat = Convert.ToInt16(segment.Substring(segment.LastIndexOf(':') + 1));

                            if (dateFormat == 103 || dateFormat == 203 || dateFormat == 303)
                            {
                                dateValue = new DateTime(
                                    Convert.ToInt16(segment.Substring(8, 4)),
                                    Convert.ToInt16(segment.Substring(12, 2)),
                                    Convert.ToInt16(segment.Substring(14, 2))
                                );
                            }
                            else
                            {
                                dateValue = new DateTime(
                                    Convert.ToInt16(segment.Substring(8, 2)) + 2000,
                                    Convert.ToInt16(segment.Substring(10, 2)),
                                    Convert.ToInt16(segment.Substring(12, 2))
                                );
                            }

                            switch (segment.Substring(4, 3))
                            {
                                case "132":
                                    if (dateLevel < 2)
                                    {
                                        dateLevel = 1;
                                        cargoPlan.VoyageInfo.DepartureDate = dateValue;
                                    }
                                    break;
                                case "133":
                                    if (dateLevel < 4)
                                    {
                                        dateLevel = 3;
                                        cargoPlan.VoyageInfo.DepartureDate = dateValue;
                                    }
                                    break;
                                case "136":
                                    dateLevel = 4;
                                    cargoPlan.VoyageInfo.DepartureDate = dateValue;
                                    break;
                                case "178":
                                    if (dateLevel < 3)
                                    {
                                        dateLevel = 2;
                                        cargoPlan.VoyageInfo.DepartureDate = dateValue;
                                    }
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            ProgramFiles.EnterLog(ProgramFiles.LogStreamWriter, e.ToString());
                            dateValue = new DateTime(1900, 01, 01);
                            cargoPlan.VoyageInfo.DepartureDate = dateValue;
                            continue;
                        }
                        break;
                    #endregion

                    #region DG
                    case "DGS":
                        if(a.DgCountInContainer == 0) DgContainerCount++;
                        a.DgCountInContainer++;
                        Dg dgUnit = new Dg();

                        //copy general container info into Dg
                        dgUnit.CopyContainerInfo(a);
                        //Gather all dg information from ediSegment and copy it to dgList
                        if(ReadDgSegment(segment, dgUnit))
                            cargoPlan.DgList.Add(dgUnit);
                        break;
                    #endregion

                    #region Equipment
                    case "EQD":
                        if (segment.Substring(4, 2) == "CN")
                        {
                            segment = segment.Replace(" ", "");
                            a.ContainerNumber = segment.Substring(7, segment.IndexOf("+", 7, StringComparison.Ordinal) - 7);
                            if (a.ContainerNumber.Contains('+') || a.ContainerNumber.Length < 11)
                            {
                                //Output.DisplayLine("Wrong container Nr: {0}",
                                WrongList.Add(segment.Substring(7, segment.IndexOf('+', 7) - 7));
                                WrongContainers++;
                                //Output.ReadKey();
                            }
                            a.ContainerType = segment.Substring(segment.IndexOf("+", 7, StringComparison.Ordinal) + 1, 4);
                        }
                        break;
                    #endregion

                    #region Others
                    case "NAD":
                        a.Carrier = segment.Substring(7, segment.IndexOf(':') - 7);
                        break;

                    case "TDT":
                        //TDT+20+VOYNO+
                        if (segment.StartsWith("TDT+20+"))
                        {
                            cargoPlan.VoyageInfo.VoyageNumber =
                                segment.Substring(7, segment.IndexOf('+', 7) - 7);
                        }
                        break;

                    case "RFF+VO":
                        if(string.IsNullOrEmpty(cargoPlan.VoyageInfo.VoyageNumber))
                            cargoPlan.VoyageInfo.VoyageNumber = segment.Substring(7);
                        break;

                    case "FTX":
                        break;
                    case "DIM":
                        break;
                    #endregion

                    #region Reefes
                    case "TMP":
                        RfContainerCount++;
                        a.IsRf = true;
                        string temp = segment.Substring(6);//.Replace('.', ',');

                        double tmp;
                        temp = temp.Remove(temp.IndexOf(':'));
                        bool isParsed = double.TryParse(temp, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,CultureInfo.CreateSpecificCulture("en-GB"), out tmp);
                        if (!isParsed) tmp = -99;

                        if (segment.Contains("CEL"))
                            a.SetTemperature = tmp;
                        else if (segment.Contains("FAH"))
                            a.SetTemperature = AdditionalFunctions.ToCelcium(tmp);
                        break;
                    #endregion

                    default:
                        break;
                }

                if (!a.IsRf) continue;
                if (!cargoPlan.Reefers.Contains(a)) cargoPlan.Reefers.Add(a);
                if (a.DgCountInContainer > 0)
                    foreach (Dg dg in cargoPlan.DgList)
                        if (dg.ContainerNumber == a.ContainerNumber)
                            dg.IsRf = a.IsRf;
            }

            return true;
        }

        /// <summary>
        /// Method to read segment containing dg information and to add the information to dg unit.
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="dgUnit"></param>
        internal static bool ReadDgSegment(string segment, Dg dgUnit)
        {
            string[] dgSegment = segment.Split('+');

            try
            {
                //DG CLASS
                dgUnit.DgClass = dgSegment[2].Contains(':')
                    ? dgSegment[2].Substring(0, dgSegment[2].IndexOf(':'))
                    : dgSegment[2];
                dgUnit.AssignRowNumber();
                dgUnit.DefineCompatibilityGroup();

                //DG UNNO
                dgUnit.Unno = Convert.ToInt16(dgSegment[3]);
                dgUnit.AssignSegregationGroup();
            }
            catch
            {
                if (string.IsNullOrEmpty(dgUnit.DgClass)) return false;
                dgUnit.Unno = 0;
                return true;
            }

            //DG FLASH POINT
            if (dgSegment.Length <= 4) return true;
            try
            {
                if (dgSegment[4].Contains(":CEL")) dgUnit.FlashPointDouble = (Convert.ToDouble(dgSegment[4].Substring
                    (0, dgSegment[4].IndexOf(':'))));
                else if (dgSegment[4].Contains(":FAH")) dgUnit.FlashPointDouble = AdditionalFunctions.ToCelcium(Convert.ToDouble
                    (dgSegment[4].Substring(0, dgSegment[4].IndexOf(':'))));
            }
            catch (Exception e)
            {
                // ignored
            }

            //DG Packing group
            if (dgSegment.Length <= 5) return true;
            dgUnit.PackingGroup = dgSegment[5];

            if (dgSegment.Length > 6)
            {
                //DG EMS
                dgUnit.DgEMS = FormatEms(dgSegment[6]);
                if (dgSegment.Length > 8 && dgSegment[8] != "")
                {
                    Output.ThrowMessage(dgSegment[8]);
                    //Output.DisplayLine(dgSegment[8]);
                    //Output.ReadKey();
                }
            }
            //DG subclasses
            if (dgSegment.Length <= 9) return true;
            dgUnit.DgSubclassArray = dgSegment[9].Split(':');
            return true;
        }

        /// <summary>
        /// Formats EMS string to standard good-looking string.
        /// </summary>
        /// <param name="ems">Original EMS string.</param>
        /// <returns></returns>
        private static string FormatEms(string ems)
        {

            if (ems.Contains(' ') && ems.Length == 7 || ems.Length == 11)
                return ems;
            if (!ems.Contains(' '))
            {
                if (ems.Length == 6)
                    return ems.Insert(3, " ");
                if (!ems.Contains('-') && ems.Length == 4)
                {
                    string temp = ems;
                    for (int i = 3; i > 0; i--)
                    {
                        if(i%2 != 0)
                            temp = temp.Insert(i, " - ");
                        else
                        {
                            temp = temp.Insert(i, " ");
                        }
                    }

                    return temp;
                }
            }

            return ems;
        }
    }
}