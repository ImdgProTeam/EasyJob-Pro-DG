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
    /// Class contains methods to read and parse .edi file and create dg list, list of all containers and reefer list. 
    /// As well, voyage data will be updated from the .edi file.
    /// </summary>
    public static class ReadBaplieFile
    {
        #region fields
        private static string _segmentDef;
        private static EdiSegmentArray _segmentArray;
        private static ShipProfile _ship;

        private static CargoPlan cargoPlan;
        private static List<string> WrongList;
        private static int WrongContainersCount;

        private const string SYMBOLS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        #endregion

        public static CargoPlan GetCargoPlan()
        {
            return cargoPlan;
        }
        public static Voyage GetVoyage()
        {
            return cargoPlan.VoyageInfo;
        }

        /// <summary>
        /// Creates <see cref="CargoPlan"/> and <see cref="Voyage"/> from baplie file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ship"></param>
        /// <param name="isIftdgn">If the file is IFTDGN file. If it is declared as false when it actually is, it will be read as IFTDGN.</param>
        public static void ReadBaplie(string file, ShipProfile ship, ref bool isIftdgn)
        {
            _ship = ship;

            CreateSegmentArrayFromBaplieFile(file);

            //Create container list
            cargoPlan = new CargoPlan();
            WrongList = new List<string>();
            WrongContainersCount = 0;

            if (!isIftdgn)
                if (!DefineSegments())
                    isIftdgn = true;

            if (isIftdgn)
                cargoPlan = ReadIftdgnFile.ReadSegments(_segmentArray, _ship);
        }

        /// <summary>
        /// Creates <see cref="_segmentArray"/> from baplie file.
        /// </summary>
        /// <param name="file">Baplie file.</param>
        private static void CreateSegmentArrayFromBaplieFile(string file)
        {
            //read from file
            using var reader = new StreamReader(file);
            {
                string text = reader.ReadToEnd();
                text = text.Replace("\r", "").Replace("\n", "");
                _segmentArray = new EdiSegmentArray(text);
                reader.Close();
            }
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
                dgUnit.AssignSegregationTableRowNumber();
                dgUnit.DefineCompatibilityGroup();

                //DG UNNO
                dgUnit.Unno = Convert.ToUInt16(dgSegment[3]);
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
                if (dgSegment[4].Contains(":CEL")) dgUnit.FlashPointAsDecimal = Convert.ToDecimal(dgSegment[4].Substring
                    (0, dgSegment[4].IndexOf(':')));
                else if (dgSegment[4].Contains(":FAH")) dgUnit.FlashPointAsDecimal = Convert.ToDecimal
                    (dgSegment[4].Substring(0, dgSegment[4].IndexOf(':'))).ToCelcium();
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
                }
            }
            //DG subclasses
            if (dgSegment.Length <= 10) return true;
            var split = dgSegment[10].Split(':');
            for (int i = 0; i < split.Length && i < 2; i++)
            {
                dgUnit.DgSubClassArray[i] = split[i];
            }
            return true;
        }


        /// <summary>
        /// Method defines what information is contained in a segment and updates respective fields of dg list, 
        /// container list or vessel information
        /// </summary>
        private static bool DefineSegments()
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
                        ReadLOCsegment(ref a, segment);
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
                            Data.LogWriter.Write($"Reading segment {segment} caused an exception {e}");
                            dateValue = new DateTime(1900, 01, 01);
                            cargoPlan.VoyageInfo.DepartureDate = dateValue;
                            continue;
                        }
                        break;
                    #endregion

                    #region DG
                    case "DGS":
                        a.DgCountInContainer++;
                        Dg dgUnit = new Dg();

                        //copy general container info into Dg
                        dgUnit.CopyContainerInfo(a);
                        //Gather all dg information from ediSegment and copy it to dgList
                        if (ReadDgSegment(segment, dgUnit))
                            cargoPlan.DgList.Add(dgUnit);
                        break;
                    #endregion

                    #region Equipment
                    case "EQD":
                        string[] subsegments = segment.Split('+');
                        if (!string.Equals(subsegments[1], "CN")) break;

                        a.ContainerNumber = subsegments[2].Replace(" ", "");
                        if (a.ContainerNumber.Length == 0) NameNonamer(a);
                        else if (a.ContainerNumber.Length != 11)
                        {
                            WrongList.Add(segment.Substring(7, segment.IndexOf('+', 7) - 7));
                            WrongContainersCount++;
                        }

                        a.ContainerType = subsegments[3].Replace(" ", "");
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
                        if (string.IsNullOrEmpty(cargoPlan.VoyageInfo.VoyageNumber))
                            cargoPlan.VoyageInfo.VoyageNumber = segment.Substring(7);
                        break;

                    case "FTX":
                        bool hasReadLq = false;
                        bool hasReadMp = false;
                        bool isMp = false;

                        if (UserSettings.ReadLQfromBaplie)
                            if (segment.Contains("LQ") || segment.Replace(" ", "").Contains("LTDQTY"))
                                hasReadLq = true;

                        if (UserSettings.ReadMPfromBaplie)
                        {
                            if (segment.Replace(" ", "").Contains("MARPOL") || segment.StartsWith("FTX+AAC++P"))
                            {
                                hasReadMp = true;
                                isMp = true;
                            }
                            if (segment.StartsWith("FTX+AAC++N"))
                            {
                                hasReadMp = true;
                                isMp = false;
                            }
                        }
                        if (!hasReadMp && !hasReadLq) break;

                        if (a.DgCountInContainer > 0)
                        {
                            var unit = cargoPlan.DgList[cargoPlan.DgList.Count - 1];
                            if (hasReadLq) unit.IsLq = true;
                            if (hasReadMp)
                            {
                                unit.IsMp = isMp;
                                unit.mpDetermined = true;
                            }
                        }

                        break;
                    case "DIM":
                        break;
                    #endregion

                    #region Reefes
                    case "TMP":
                        a.IsRf = true;
                        string temp = segment.Substring(6);

                        decimal tmp;
                        temp = temp.Remove(temp.IndexOf(':'));
                        bool isParsed = decimal.TryParse(temp, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.CreateSpecificCulture("en-GB"), out tmp);
                        if (!isParsed) tmp = -99;

                        if (segment.Contains("CEL"))
                            a.SetTemperature = tmp;
                        else if (segment.Contains("FAH"))
                            a.SetTemperature = tmp.ToCelcium();
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
        /// Assignes correct value to LOC-related Container and Voyage properties from LOC segment.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="segment"></param>
        private static void ReadLOCsegment(ref Container a, string segment)
        {
            switch (segment.Split('+')[1])
            {
                //Port of departure
                case "5":
                    cargoPlan.VoyageInfo.PortOfDeparture = ParseLOCsegment(segment);
                    break;

                //Next port of call
                case "61":
                    cargoPlan.VoyageInfo.PortOfDestination = ParseLOCsegment(segment);
                    break;

                //Container location
                case "147":
                    {
                        a = new Container { Location = ParseLOCsegment(segment) };
                        a.HoldNr = ShipProfile.DefineCargoHoldNumber(a.Bay);
                        cargoPlan.Containers.Add(a);
                        break;
                    }

                //POL
                case "6":
                case "9":
                    a.POL = ParseLOCsegment(segment);
                    break;

                //POD
                case "11":
                case "12":
                    a.POD = ParseLOCsegment(segment);
                    break;

                //Final destination
                case "83":
                    a.FinalDestination = ParseLOCsegment(segment);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Withdraws meaningful part of an LOC segment
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        internal static string ParseLOCsegment(string segment)
        {
            var subsegments = segment.Split('+');
            var result = subsegments[2].Contains(":")
                ? subsegments[2].Split(':')[0]
                : subsegments[2];
            return result;
        }

        /// <summary>
        /// Creates an unique consequitive name for <see cref="Container"/> a without number in accordance with naming rules.
        /// </summary>
        /// <param name="a"></param>
        private static void NameNonamer(Container a)
        {
            if (a.ContainerNumber is not null && !string.Equals(a.ContainerNumber, string.Empty)) return;

            a.ContainerNumber = ProgramDefaultSettingValues.NoNamePrefix + GenerateNextNonamerNumber(cargoPlan.NextNonamerNumber++);
        }

        /// <summary>
        /// Generates consequtive next number based on argument value. 
        /// </summary>
        /// <param name="next">Integer value to be used to generate unique number.</param>
        /// <returns>Unique string consisting of three chars.</returns>
        private static string GenerateNextNonamerNumber(int next)
        {
            if (next < 1000) return next.ToString("000");

            int numberOfVariants = SYMBOLS.Length;
            int exceed = next - 1000;

            byte numberOfLetters = exceed < numberOfVariants * 100 ? (byte)1
                : exceed < numberOfVariants * numberOfVariants * 10 + numberOfVariants * 100 ? (byte)2
                : (byte)3;

            int firstDivider = numberOfLetters == 1 ? 100
                : numberOfLetters == 2
                ? numberOfVariants * 10 : numberOfVariants * numberOfVariants;
            int secondDivider = numberOfLetters < 3 ? 10 : numberOfVariants;
            int value = numberOfLetters == 1 ? exceed
                : numberOfLetters == 2 ? exceed - numberOfVariants * 100
                : exceed - numberOfVariants * numberOfVariants * 10 - numberOfVariants * 100;

            int[] calculatedValues = new int[3];
            calculatedValues[0] = value / firstDivider;
            calculatedValues[1] = value % firstDivider / secondDivider;
            calculatedValues[2] = value % firstDivider % secondDivider;

            char[] result = new char[3];
            result[0] = SYMBOLS[calculatedValues[0]];
            result[1] = numberOfLetters == 1 ? Char.Parse(calculatedValues[1].ToString()) : SYMBOLS[calculatedValues[1]];
            result[2] = numberOfLetters < 3 ? Char.Parse(calculatedValues[2].ToString()) : SYMBOLS[calculatedValues[2]];

            return new string(result);
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
                        if (i % 2 != 0)
                            temp = temp.Insert(i, " - ");
                        else
                            temp = temp.Insert(i, " ");
                    }
                    return temp;
                }
            }
            return ems;
        }

    }
}