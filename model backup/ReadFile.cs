using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;



namespace EasyJob_Pro_DG
{
    /// <summary>
    /// Class contains methods to read and parse .edi file and create dg list, list of all containers and reefer list. As well, voyage data will be updated from the .edi file.
    /// </summary>
    public class ReadFile
    {
        private string segmentDef;
        public int containerCount = 0;
        public int dgContainerCount = 0;
        public int rfContainerCount = 0;
        public int containersLoaded = 0;
        public int dgContainersLoaded = 0;
        public int rfContainersLoaded = 0;

        readonly ediSegmentArray segmentArray;
        public List<Container> Containers;
        //public List<Container> Reefers;
        public Transport vessel;
        public List<Dg> dgList;
        public List<string> wrongList;
        public int wrongContainers;
        private readonly ShipProfile ship;

        /// <summary>
        /// The constructor will read .edi file, define segment information, and will display working information and summary.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="_ship"></param>
        /// <param name="reefers"></param>
        public ReadFile(string file, ShipProfile _ship, out List<Container> reefers)
        {
            ship = _ship;
            var reader = new StreamReader(file);
            string text = reader.ReadToEnd();
            text = text.Replace("\r", "");
            text = text.Replace("\n", "");

            segmentArray = new ediSegmentArray(text);
            reader.Close();


            ///Print working information
            //Output.DisplayLine("\nTotal {0} segments.", segmentArray.count);
            //Output.Display("\nTotal {0} containers.", segmentArray.containerCount.ToString());
            //Output.DisplayLine("\nEnd of Reader.\nPress any key to complete container list");
            //Output.ReadKey();

            ///Create container list
            Containers = new List<Container>();
            reefers = new List<Container>();
            vessel = new Transport();
            wrongList = new List<string>();
            wrongContainers = 0;

            defineSegments(reefers);

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

            ///Gathering summary of containers
            foreach (Container unit in Containers)
            {
                if (unit.cnPOL != vessel.portOfDeparture) continue;
                containersLoaded++;
                if (unit.dgInContainer > 0)
                    dgContainersLoaded++;
                if (unit.RF)
                    rfContainersLoaded++;
            }
        }

        /// <summary>
        /// Method defines what information is contained in a segment and updates respective fields of dg list, container list or vessel information
        /// </summary>
        /// <param name="reefers"></param>
        public void defineSegments(List<Container> reefers)
        {
            Container a = new Container();
            dgList = new List<Dg>();
            int dateLevel = 0;

            for (int i = 0; i < segmentArray.count; i++)
            {
                string segment = segmentArray.array[i];
                segmentDef = segment.Length > 3 ? segment.Remove(3) : segmentArray.array[i];

                switch (segmentDef)
                {
                    #region Location
                    case "LOC":
                        if (segment.StartsWith("LOC+5"))
                        {
                            //Port of departure
                            if (segment.Substring(6).Contains(":"))
                                segment = segment.Substring(0, segment.IndexOf(':'));
                            vessel.portOfDeparture = segment.Length > 10 ? segment.Substring(6, 5) : segment.Substring(6, segment.Length - 6);
                            break;
                        }
                        else if (segment.StartsWith("LOC+61"))
                        {
                            //Next port of call
                            vessel.portOfDestination = segment.Length > 11 ? segment.Substring(7, 5) : segment.Substring(7, segment.Length - 7);
                            break;
                        }
                        else if (segment.StartsWith("LOC+147"))
                        {
                            //Container location
                            if (a.cntrLocation != null)
                            {
                                containerCount++;
                                //if (!a.typeRecognized && a.dgInContainer > 0)
                                //{
                                //    Style.FontColor("Red");
                                //    Output.DisplayLine(
                                //        "Attention! Container {0} has not recognized type {1}. It will be considered as 'closed freight container'", new string[] { a.cntrNr, a.ctnrType });
                                //    Output.ReadKey();

                                //}
                            }
                            a = new Container { Location = segment.Substring(8, 7) };
                            a.holdNr = ship.DefineCargoHoldNumber(a.bay);
                            Containers.Add(a);
                            break;
                        }
                        else if (segment.StartsWith("LOC+6+"))
                        {
                            //POL
                            a.cnPOL = segment.Length > 10 ? segment.Substring(6, 5) : segment.Substring(6, segment.Length - 6);
                            break;
                        }
                        else if (segment.StartsWith("LOC+9+"))
                        {
                            //POL
                            a.cnPOL = segment.Length > 10 ? segment.Substring(6, 5) : segment.Substring(6, segment.Length - 6);
                            break;
                        }
                        else if (segment.StartsWith("LOC+11"))
                        {
                            //POD
                            a.cnPOD = segment.Length > 11 ? segment.Substring(7, 5) : segment.Substring(7);
                            break;
                        }
                        else if (segment.StartsWith("LOC+12"))
                        {
                            //POD
                            a.cnPOD = segment.Length > 11 ? segment.Substring(7, 5) : segment.Substring(7, segment.Length - 7);
                            break;
                        }
                        else if (segment.StartsWith("LOC+83"))
                        {
                            //Final destination
                            a.finalDestination = segment.Length > 11 ? segment.Substring(7, 5) : segment.Substring(7, segment.Length - 7);
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
                                        vessel.departureDate = dateValue;
                                    }
                                    break;
                                case "133":
                                    if (dateLevel < 4)
                                    {
                                        dateLevel = 3;
                                        vessel.departureDate = dateValue;
                                    }
                                    break;
                                case "136":
                                    dateLevel = 4;
                                    vessel.departureDate = dateValue;
                                    break;
                                case "178":
                                    if (dateLevel < 3)
                                    {
                                        dateLevel = 2;
                                        vessel.departureDate = dateValue;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            ProgramFiles.EnterLog(ProgramFiles.logStreamWriter, e.ToString());
                            dateValue = new DateTime(1900, 01, 01);
                            vessel.departureDate = dateValue;
                            continue;
                        }
                        break;
                    #endregion

                    #region DG
                    case "DGS":
                        a.dgInContainer++;
                        //create new dglist inside container a, if not exists & increseases the number of containers containing dg
                        if (a.dg == null)
                        {
                            a.dg = new List<Dg> { new Dg() };
                            dgContainerCount++;
                        }
                        else a.dg.Add(new Dg());
                        Dg dgUnit = a.dg[a.dgInContainer - 1];

                        //copy general container info into Dg
                        dgUnit.CopyContainerInfo(a);
                        //Gather all dg information from ediSegment and copy it to dgList
                        ReadDgSegment(segment, dgUnit);
                        dgList.Add(dgUnit);
                        break;
                    #endregion

                    #region Equipment
                    case "EQD":
                        if (segment.Substring(4, 2) == "CN")
                        {
                            segment = segment.Replace(" ", "");
                            a.cntrNr = segment.Substring(7, segment.IndexOf("+", 7, StringComparison.Ordinal) - 7);
                            if (a.cntrNr.Contains('+') || a.cntrNr.Length < 11)
                            {
                                //Output.DisplayLine("Wrong container Nr: {0}",
                                wrongList.Add(segment.Substring(7, segment.IndexOf('+', 7) - 7));
                                wrongContainers++;
                                //Output.ReadKey();
                            }
                            a.cntrType = segment.Substring(segment.IndexOf("+", 7, StringComparison.Ordinal) + 1, 4);
                            foreach (CodesDictionary.Types t in CodesDictionary.types)
                            {
                                if (t.code != a.cntrType) continue;
                                a.closed = t.closed;
                                a.typeRecognized = true;
                                break;
                            }
                        }
                        break;
                    #endregion

                    #region Others
                    case "NAD":
                        a.carrier = segment.Substring(7, segment.IndexOf(':') - 7);
                        break;

                    case "TDT":
                        break;

                    case "REF":
                        vessel.voyageNr = segment.Substring(7);
                        break;

                    case "FTX":
                        break;
                    case "DIM":
                        break;
                    #endregion

                    #region Reefes
                    case "TMP":
                        rfContainerCount++;
                        a.RF = true;
                        string temp = segment.Substring(6).Replace('.', ',');

                        double tmp = double.Parse(temp.Remove(temp.IndexOf(':')));
                        if (segment.Contains("CEL"))
                            a.RFtmp = tmp;
                        else if (segment.Contains("FAH"))
                            a.RFtmp = AdditionalFunctions.ToCelcium(tmp);
                        break;
                    #endregion

                    default:
                        break;
                }
                if (a.RF)
                {
                    if (!reefers.Contains(a)) reefers.Add(a);
                    if (a.dgInContainer > 0)
                        foreach (Dg dg in a.dg)
                            dg.RF = a.RF;
                }

            }

        }

        /// <summary>
        /// Method to read segment containing dg information and to add the information to dg unit.
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="dgUnit"></param>
        public void ReadDgSegment(string segment, Dg dgUnit)
        {
            string[] dgSegment = segment.Split('+');
            //DG CLASS
            dgUnit.Dgclass = dgSegment[2].Contains(':') ?
                             dgSegment[2].Substring(0, dgSegment[2].IndexOf(':')) : dgSegment[2];
            dgUnit.AssignRowNumber();
            dgUnit.DefineCompatibilityGroup();

            //DG UNNO
            dgUnit.unno = Convert.ToInt16(dgSegment[3]);
            dgUnit.AssignSegregationGroup();

            //DG FLASH POINT
            if (dgSegment.Length <= 4) return;
            if (dgSegment[4].Contains('.')) dgSegment[4] = dgSegment[4].Replace('.', ',');
            if (dgSegment[4].Contains(":CEL")) dgUnit.dgfp = (Convert.ToDouble(dgSegment[4].Substring
                                             (0, dgSegment[4].IndexOf(':')).Replace(".", "").Replace(",", ""))) / 10;
            else if (dgSegment[4].Contains(":FAH")) dgUnit.dgfp = AdditionalFunctions.ToCelcium(Convert.ToDouble
                                                                    (dgSegment[4].Substring(0, dgSegment[4].IndexOf(':'))));
            //DG Packing group
            if (dgSegment.Length <= 5) return;
            dgUnit.PKG = dgSegment[5];

            if (dgSegment.Length > 6)
            {
                //DG EMS
                dgUnit.dgems = dgSegment[6];
                if (dgSegment.Length > 8 && dgSegment[8] != "")
                {
                    MessageBox.Show(dgSegment[8]);
                    //Output.DisplayLine(dgSegment[8]);
                    //Output.ReadKey();
                }
            }

            //DG subclasses
            if (dgSegment.Length <= 9) return;
            dgUnit.DgsubclassArray = dgSegment[9].Split(':');
        }
    }

    /// <summary>
    /// Supporting class to facilitate definind of segments
    /// </summary>
    internal class ediSegmentArray
    {
        public string[] array;
        public int count;
        public int containerCount;

        public ediSegmentArray(string text)
        {
            array = text.Split('\'');
            count = array.Length;
            for (int i = 0; i < count; i++)
            {
                if (array[i].StartsWith("LOC+147"))
                {
                    containerCount++;
                }
            }

        }

        public int Count()
        {
            return count;
        }
    }

}
