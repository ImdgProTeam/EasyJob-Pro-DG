using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Linq.Expressions;
using System.Net.Configuration;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;


namespace EasyJob_Pro_DG
{
    public partial class ShipProfile
    {
        /// <summary>
        /// Reading and saving to config.file to be implemented
        /// </summary>
        private string shipName;
        private string callsign;
        public bool passenger;
        public CH[] Holds;
        public byte numberOfAccommodations;
        private byte accommodation;
        public List<byte> accommodationBays;
        private List<byte> accommodation12;
        public List<outerRow> seaSides;
        public List<CellPosition> LivingQuartersList;
        public List<CellPosition> HeatedStructuresList;
        public List<CellPosition> LSAList;
        public DOC doc;
        private bool filecorrupt;
        public static bool alwaysOpenDefaultProfile = true;
        public static bool multiprofile = false;
        public static string defaultShipProfile = "ShipProfile.ini";
        const string shipProfileExtension = ".ini";
        public static DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);


        [Flags]
        public enum MotorFacing : byte
        {
            NotDefined, Aft, Forward
        }

        #region DOC classes
        public static List<string> DOCclasses = new List<string>()
        {
            "1.1-1.6\nExplosives",                                          //0
            "1.4(S)\nExplosives, Division 1.4 Compatibiliyu group 'S'",     //1
            "2.1\nFlammable gases",                                         //2
            "2.2\nNon-flammable, Non-toxic gases",                          //3
            "2.3\nToxic gases (flammable)",                                 //4
            "2.3\nToxic gases (non-flammable)",                             //5
            "3\nFlammable liquids - low and intermediate flashpoint, <23°C",
            "3\nFlammable liquids - high flashpoint, ≥23°C but ≤60°C",
            "4.1\nFlammable solids, self-reactive substances and solid desensitized explosives",
            "4.2\nSolids liable to spontaneous combustion",
            "4.3\nSubstances which, in contact with water, emit flammable gases (liquids)",
            "4.3\nSubstances which, in contact with water, emit flammable gases (solids)",
            "5.1\nOxidising substances (agents)",
            "5.2\nOrganic peroxides",
            "6.1\nToxic substances (liquids) - low and intermediate flashpoint, <23°C",
            "6.1\nToxic substances (liquids) - high flashpoint, ≥23°C but ≤60°C",
            "6.1\nToxic substances (liquids) - non flammable",
            "6.1\nToxic substances (solids)",
            "8\nCorrosives (liquids) - low and intermediate flashpoint, <23°C",
            "8\nCorrosives (liquids) - high flashpoint, ≥23°C but ≤60°C",
            "8\nCorrosives (liquids) - non flammable",
            "8\nCorrosives (solids)",
            "9\nMiscellaneous Dangerous Substances and Articles"
        };
#endregion

        /// <summary>
        /// List contains list of errors found by CheckShipProfile method
        /// </summary>
        private List<string> errorList;

        /// <summary>
        /// Property to easily add unique errors in the errorList
        /// </summary>
        public string ErrorList
        {
            set
            {
                if (!this.errorList.Contains(value)) this.errorList.Add(value);
            }
            get
            {
                string result = "";
                foreach (var str in errorList)
                    result += result == "" ? "" : ", " + str;
                return result;
            }
        }


        //--------------- Properties and methods to display and get info from xaml -----------------------------------

        public static List<string> MotorFacingList => new List<string>() {"Not defined", "Aft", "Forward"};

        //--------------- Properties and a method to set and get ship profile items ----------------------------------

        public List<byte> Accommodation { get; set; }

        public void SetAccommodation(byte bay)
        {
            accommodation = bay;
            if (accommodationBays.Contains(0)) accommodationBays.Remove(0);
            accommodationBays.Add(bay % 2 == 0? (byte)(bay + 1) : bay);
            if (Accommodation == null) Accommodation = new List<byte>();
            if (Accommodation.Contains(bay)) return;
            Accommodation.Add(bay);
            Accommodation.Add((byte)(bay + 1));
            Accommodation.Add((byte)(bay + 2));
            Accommodation.Add((byte)(bay + 3));
            if (bay % 2 != 0) Accommodation.Add((byte)(bay - 1));
            if (bay % 2 == 0) Accommodation.Add((byte)(bay + 4));
        }
        public CellPosition LivingQuarters
        {
            set {LivingQuartersList.Add(value); }
        }
        public CellPosition HeatedStructures
        {
            set { HeatedStructuresList.Add(value);}
        }
        public CellPosition LSA
        {
            set { LSAList.Add(value); }
        }
        public string ShipName
        {
            get
            {
                return shipName;
            }
            set { shipName = value; }
        }
        public string CallSign
        {
            get { return callsign; }
            set { callsign = value.ToUpper(); }
        }
        public int numberOfHolds { get; set; }
        public byte rfMotor { get; set; }
        public bool row00exists { get; set; }


        // ------------------------------ Ship profile constructors ------------------------------------------------------

        public ShipProfile(string shipFile)
        {
            filecorrupt = false;
            seaSides = new List<outerRow>() { new outerRow(0,99,99) };
            LivingQuartersList = new List<CellPosition>();
            HeatedStructuresList = new List<CellPosition>();
            LSAList = new List<CellPosition>();
            rfMotor = (byte)MotorFacing.NotDefined;
            row00exists = true;
            accommodationBays = new List<byte>() { 0 };
            numberOfHolds = 1;
            Holds = new CH[] {new CH(1, 199) };
            doc = new DOC(2);
            int linecount = 0;
            errorList = new List<string>();

            Exception ex = new Exception();

                using (var reader = new StreamReader(shipFile))
                {
                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            var line = reader.ReadLine();
                            if (linecount == 0 && line != "***ShipProfile***")
                                throw new Exception("Ship profile file is wrong or modified.");
                            if (line != null && !line.StartsWith("//") && line.Length != 0 && line.Contains("="))
                            {
                                line = line.Replace("=  ", "=").Replace("= ", "=");
                                string lineValue = line.Substring(line.IndexOf('=') + 1);
                                int i;
                                string lineDescr = line.Substring(0, line.IndexOf('=')).Replace(" ", "")
                                    .Replace("\'", "").ToLower();
                                switch (lineDescr)
                                {
                                    case "profilename": break;
                                    case "shipname":
                                        ShipName = lineValue;
                                        break;
                                    case "callsign":
                                        CallSign = lineValue;
                                        break;
                                    case "numberofaccommodations":
                                        ex.Source = "accommodation";
                                        numberOfAccommodations = byte.Parse(lineValue);
                                        break;
                                    case "row00exists":
                                        ex.Source = "row00exists";
                                        row00exists = bool.Parse(lineValue);
                                        break;
                                    case "passenger":
                                        ex.Source = "passenger";
                                        passenger = bool.Parse(lineValue);
                                        break;
                                    case "seasides":
                                        ex.Source = "seasides";
                                        outerRow instance = new outerRow();
                                        i = 0;
                                        foreach (string figure in lineValue.Split(','))
                                        {
                                            instance[i] = int.Parse(figure);
                                            i++;
                                        }

                                        if (instance.bay == 0)
                                            seaSides.RemoveAt(0);
                                        seaSides.Add(instance);
                                        break;
                                    case "reefermotorsfacing":
                                        ex.Source = "reefermotor";
                                        rfMotor = byte.Parse(lineValue);
                                        break;
                                    case "accommodation":
                                        ex.Source = "accommodation";
                                        SetAccommodation(byte.Parse(lineValue));
                                        break;
                                    case "accommodation1":
                                        ex.Source = "accommodation";
                                        SetAccommodation(byte.Parse(lineValue));
                                        break;
                                    case "accommodation2":
                                        ex.Source = "accommodation";
                                        SetAccommodation(byte.Parse(lineValue));
                                        break;
                                    case "accommodation3":
                                        ex.Source = "accommodation";
                                        SetAccommodation(byte.Parse(lineValue));
                                        break;
                                    case "accommodation4":
                                        ex.Source = "accommodation";
                                        SetAccommodation(byte.Parse(lineValue));
                                        break;
                                    case "lq":
                                        ex.Source = "lq";
                                        string[] lq = lineValue.Split(',');
                                        LivingQuarters = new CellPosition(byte.Parse(lq[0]), byte.Parse(lq[1]),
                                            byte.Parse(lq[2]), byte.Parse(lq[3]), byte.Parse(lq[4]));
                                        break;
                                    case "hs":
                                        ex.Source = "hs";
                                        string[] hs = lineValue.Split(',');
                                        HeatedStructures = new CellPosition(byte.Parse(hs[0]), byte.Parse(hs[1]),
                                            byte.Parse(hs[2]), byte.Parse(hs[3]), byte.Parse(hs[4]));
                                        break;
                                    case "lsa":
                                        ex.Source = "lsa";
                                        string[] lsa = lineValue.Split(',');
                                        LSA = new CellPosition(byte.Parse(lsa[0]), byte.Parse(lsa[1]),
                                            byte.Parse(lsa[2]), byte.Parse(lsa[3]), byte.Parse(lsa[4]));
                                        break;
                            }

                                if (lineDescr.StartsWith("hold"))
                                {
                                    ex.Source = "holds";
                                    if (lineDescr == "holdsnumber")
                                    {
                                        numberOfHolds = int.Parse(lineValue);
                                        this.Holds = new CH[numberOfHolds];
                                        doc = new DOC(numberOfHolds);

                                    }
                                    else
                                    {
                                        var FandLbays = lineValue.Split(',');
                                        this.Holds[int.Parse(lineDescr.Substring(4)) - 1] =
                                            new CH(int.Parse(FandLbays[0]), int.Parse(FandLbays[1]));
                                    }
                                }

                                if (lineDescr.StartsWith("doc"))
                                {
                                    ex.Source = "doc";
                                    if (lineDescr.Substring(3).StartsWith("weather"))
                                        doc.SetDOCtable(lineValue, (byte) (numberOfHolds + 1));
                                    else
                                        doc.SetDOCtable(lineValue, byte.Parse(lineDescr.Substring(7)));
                                }
                            }
                        }
                        catch
                        {
                            ErrorList = ex.Source;
                            MessageBox.Show("An error occurred while opening the ship profile.");
                            filecorrupt = true;
                        }
                        linecount++;
                }
                    //Style.AnswerStyle("\nShip profile loaded...\n");
                }

            if (!CheckErrorsInShipProfile())
            {
                MessageBoxResult result = MessageBox.Show("Errors occurred while opening file. Do you wish to check them? Y/N", "Error message", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes) MessageBox.Show("Not implemented");
            }

        }


        /// <summary>
        /// Additional constructor to create new file
        /// </summary>
        /// <param name="shipName"></param>
        /// <param name="passenger"></param>
        public ShipProfile(string shipName, bool passenger)
        {
            ShipName = shipName;
            this.passenger = passenger;
            seaSides = new List<outerRow>();
            LivingQuartersList=new List<CellPosition>();
            HeatedStructuresList=new List<CellPosition>();
            LSAList = new List<CellPosition>();
            rfMotor = (byte)MotorFacing.NotDefined;
            row00exists = true;
            accommodationBays = new List<byte>();
            errorList =new List<string>();

        }

        /// <summary>
        /// Constructor for default ship with one cargo hold
        /// </summary>
        public ShipProfile()
        {
            accommodationBays = new List<byte>();
            SetAccommodation(100);
            numberOfHolds = 1;
            Holds = new CH[numberOfHolds];
            Holds[0] = new CH(1, 100);
            LivingQuartersList = new List<CellPosition>();
            LivingQuarters=new CellPosition(99,199,199,199,00);
            HeatedStructuresList = new List<CellPosition>();
            HeatedStructures = new CellPosition(99,199,199,199,0);
            LSAList = new List<CellPosition>();
            LSA = new CellPosition(99, 199, 199, 199, 0);
            row00exists = true;
            rfMotor = (byte)MotorFacing.NotDefined;
            seaSides = new List<outerRow> { new outerRow(0, 99, 99) };
            doc = new DOC(numberOfHolds);
            doc.SetDOCtable("1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1", 1);
            doc.SetDOCtable("1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1", 2);
            errorList = new List<string>();

        }



        // ----------------- Supportive methods to determine certain locations of given units ----------------------------

        public bool IsInLivingQuarters(Dg dg)
        {
            //Clear of accommodation
            return Accommodation.Contains(dg.bay) || IsWithinCells(dg, LivingQuartersList);
        }

        public bool IsInHeatedStructures(Dg dg)
        {
            return IsWithinCells(dg, HeatedStructuresList);
        }

        /// <summary>
        /// Method checks is dg located within 12m from LSA and accommodation and returns true if it is less than 12m
        /// </summary>
        /// <param name="dg"></param>
        /// <returns></returns>
        public bool IsNotClearOfLSA(Dg dg)
        {
            //1. accommodation
            if (accommodation12 == null)
            {
                accommodation12 = new List<byte> {accommodation, (byte)(accommodation+1), (byte)(accommodation + 2), (byte)(accommodation + 3), (byte)(accommodation + 4), (byte)(accommodation + 5), (byte)(accommodation - 1), (byte)(accommodation - 2)};
            }

            if (accommodation12.Contains(dg.bay)) return true;

            //2. LSA
            foreach (CellPosition lsa in LSAList)
            {
                List<byte> ath = new List<byte>
                #region create list of rows
                {
                    lsa.Row,
                    (byte) (lsa.Row + 2),
                    (byte) (lsa.Row + 4),
                    (byte) (lsa.Row + 6),
                    (byte) (lsa.Row + 8),
                    (byte) (lsa.Row + 10),
                    (byte) (lsa.Row - 2),
                    (byte) (lsa.Row - 4),
                    (byte) (lsa.Row - 6),
                    (byte) (lsa.Row - 8),
                    (byte) (lsa.Row - 10)
                };
            if (lsa.Row == 0) { ath.Add(1); ath.Add(3); ath.Add(5); ath.Add(7); ath.Add(9); }
            if (lsa.Row == 1)
            {
                ath.Add(2); ath.Add(4);
                ath.Add(6); ath.Add(8);
                ath.Add(!row00exists ? (byte) 10 : (byte) 0);
            }
            if (lsa.Row == 2)
            {
                ath.Add(1); ath.Add(3); ath.Add(5); ath.Add(7);
                    if (!row00exists) ath.Add(9);
            }
            if (lsa.Row == 3)
            {
                ath.Add(2); ath.Add(4); ath.Add(6);
                    if (!row00exists) ath.Add(8);
            }
            if (lsa.Row == 4)
            {
                ath.Add(1); ath.Add(3); ath.Add(5);
                    if (!row00exists) ath.Add(7);
            }
            if (lsa.Row == 5)
            {
                ath.Add(2); ath.Add(4);
                if (!row00exists) ath.Add(6);
            }
            if (lsa.Row == 6)
            {
                ath.Add(1); ath.Add(3);
                if (!row00exists) ath.Add(5);
            }
            if (lsa.Row == 7)
            {
                ath.Add(2);
                if (!row00exists) ath.Add(4);
            }
            if (lsa.Row == 8)
            {
                ath.Add(1);
                if (!row00exists) ath.Add(3);
            }
            if (lsa.Row == 9 && !row00exists) ath.Add(2);
            if (lsa.Row == 10 && !row00exists) ath.Add(1);
            if (lsa.Row <= 10 && !ath.Contains(0)) ath.Add(0);
#endregion

                List<byte> bays = new List<byte>
                {
                    lsa.Bay, (byte)(lsa.Bay + 1), (byte)(lsa.Bay + 2), (byte)(lsa.Bay + 3), (byte)(lsa.Bay + 4), (byte)(lsa.Bay + 5),
                    lsa.Bay, (byte)(lsa.Bay - 1), (byte)(lsa.Bay - 2), (byte)(lsa.Bay - 3), (byte)(lsa.Bay - 4), (byte)(lsa.Bay - 5)
                };

                if (ath.Contains(dg.bay) && bays.Contains(dg.bay)) return true;
            }

            return false;
        }

        /// <summary>
        /// The Method defines in which cargo hold locates the bay. This is literally, in which cargo hold or over which cargo hold is a container loaction
        /// </summary>
        /// <param name="bay"></param>
        /// <returns></returns>
        public byte DefineCargoHoldNumber(byte bay)
        {
            byte chNr = 0;
            for (int i = 0; i < Holds.Length; i++)
            {
                if (bay <= Holds[i].lastBay && bay >= Holds[i].firstBay)
                {
                    chNr = (byte)(i + 1);
                    break;
                }
            }
            return chNr;
        }

        /// <summary>
        /// Method determines if dg unit falls within given list of positions
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="cells"></param>
        /// <returns></returns>
        private bool IsWithinCells(Dg dg, List<CellPosition> cells)
        {
            foreach (CellPosition cell in cells)
                if (cell == dg)
                    return true;
            return false;
        }

        /// <summary>
        /// Method determines if given bay exists in seaSides.
        /// </summary>
        /// <param name="bay"></param>
        /// <returns></returns>
        public bool SeasidesBayDefined(byte bay)
        {
            foreach (outerRow oRow in seaSides)
                if (oRow.bay == bay)
                    return true;
            return false;
        }

        public bool IsOnSeaSide(Dg unit)
        {
            bool defined = SeasidesBayDefined(unit.bay);
            foreach (outerRow outerRow in seaSides)
            {
                if (defined)
                { if (outerRow.bay == unit.bay && outerRow == unit)  return true;}
                else if (outerRow == unit) return true;
            }

            return false;
        }


        // ----------------------- Additional classes in supporting ship profile features -----------------------------

        public class DOC
        {
            public byte[,] DOCtable;
            public string[] DOCadditional;


            public void SetDOCtable(string line, byte holdNr)
            {
                string[] lineSplit = line.Split(',');
                if(lineSplit.Length!=23) MessageBox.Show("Error number of classes handed over to DOC!");
                int i = 0;
                foreach (string figure in lineSplit)
                {
                    DOCtable[holdNr-1, i] = byte.Parse(figure);
                    i++;
                }
            }
            public DOC(int numberofholds)
            {
                //Table from DOC with permission to load certain class into certain CH
                //0 - NOT ALLOWED, 1 - PACKAGED GOODS ALLOWED, 2- ALLOWED WITH REMARK
                //Last row - Weather deck
                DOCtable = new byte[numberofholds+1, 23];
                for (byte i = 1; i <= numberofholds + 1; i++)
                {
                    SetDOCtable("1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1", i);
                }
                //{     
                //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
                //    { 1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
                //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
                //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
                //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
                //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
                //    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1},
                //    { 0,1,0,1,1,1,0,1,1,1,0,1,1,0,0,1,1,1,1,1,1,1,1},
                //    { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}        //Weather deck
                //};

                //List of dg classes defined in DOC and titles to them
                List<string> DOCclasses = new List<string>()
                {
                    "1.1-1.6\nExplosives",                                          //0
                    "1.4(S)\nExplosives, Division 1.4 Compatibiliyu group 'S'",     //1
                    "2.1\nFlammable gases",                                         //2
                    "2.2\nNon-flammable, Non-toxic gases",                          //3
                    "2.3\nToxic gases (flammable)",                                 //4
                    "2.3\nToxic gases (non-flammable)",                             //5
                    "3\nFlammable liquids - low and intermediate flashpoint, <23°C",
                    "3\nFlammable liquids - high flashpoint, >=23°C but <=60°C",
                    "4.1\nFlammable solids, self-reactive substances and solid desensitized explosives",
                    "4.2\nSolids liable to spontaneous combustion",
                    "4.3\nSubstances which, in contact with water, emit flammable gases (liquids)",
                    "4.3\nSubstances which, in contact with water, emit flammable gases (solids)",
                    "5.1\nOxidising substances (agents)",
                    "5.2\nOrganic peroxides",
                    "6.1\nToxic substances (liquids) - low and intermediate flashpoint, <23°C",
                    "6.1\nToxic substances (liquids) - high flashpoint, >=23°C but <=60°C",
                    "6.1\nToxic substances (liquids) - non flammable",
                    "6.1\nToxic substances (solids)",
                    "8\nCorrosives (liquids) - low and intermediate flashpoint, <23°C",
                    "8\nCorrosives (liquids) - high flashpoint, >=23°C but <=60°C",
                    "8\nCorrosives (liquids) - non flammable",
                    "8\nCorrosives (solids)",
                    "9\nMiscellaneous Dangerous Substances and Articles"
                };

                DOCadditional=new string[]
                {
                    "Goods of class 1 shall not be stowed within a horizontal distance of 6m from potential sources of fire, machinery exhausts, galley uptakes, lockers used for combustible stores or other potential sources of ignition and not less than a horizontal distance of 8 meters from the brige, living quarters and life-saving appliances.",
                    "When dangerous goods are classes 2.1,2.3,3,4,5,6.1(B),6.1(D),8(B),8(C), and 9 are carried under deck, they are to be carried out in closed freight containers only.",
                    "For classes 2,3,4 liquids,5.1 liquids, 6.1, and 8 and 9 when carried in closed freight containers in purpose built dedicated container cargo spaces, the ventilation rate may be reduced to not less than two air exchanges per hour.",
                    "Power ventilation is not required for class 4 (solid) and 5.1 (liquid) when carried out in closed freight containers",
                    "Stowage of class 5.2 underdeck is prohibited",
                    "Stowage of class 2.3 having subsidiary risk class 2.1 underdeck is prohibited",
                    "Stowage of class 4.3 liquids having a flashpoint less than 23°C under deck is prohibited",
                    "No special requirements for class 6.2/7 or for the goods in limited quantities",
                    "Dangerous goods of class 9 in package form, which according to the IMDG code emmit flammable vapours, not to be carried in hold No 8"
                };
                


            }
            
        }

        public class CH
        {
            public int firstBay;
            public int lastBay;

            public CH()
            {
                firstBay = 0;
                lastBay = 0;
            }
            public CH(int fbay, int lbay)
            {
                firstBay = fbay;
                lastBay = lbay;
            }
        }

        /// <summary>
        /// Class describes the most port and the most starboard side row (seaside) for each bay individually if value 'bay' is set. If 'bay' is set as '0' or not defined then the seaside rows will apply to all bays which are not separately specified.
        /// </summary>
        public class outerRow
        {
            public int starboardMost;
            public int portMost;
            public int bay;

            public outerRow()
            {

            }

            public int this[int index]
            {
                set
                {
                    switch (index)
                    {
                        case 0:
                            bay = value;
                            break;
                        case 1:
                            portMost = value;
                            break;
                        case 2:
                            starboardMost = value;
                            break;
                    }
                }
            }
            public outerRow(int bay, int portM, int stbdM)
            {
                this.bay = bay;
                portMost = portM;
                starboardMost = stbdM;
            }

            public outerRow(int portM, int stbdM)
            {
                this.bay = 0;
                portMost = portM;
                starboardMost = stbdM;
            }

            public static outerRow Create(int bay, int portM, int stbdM)
            {
                return new outerRow(bay, portM, stbdM);
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                if (obj.GetType() == typeof(Dg)) return Equals((Dg)obj);
                if (obj.GetType() == typeof(Container)) return Equals((Container)obj);
                if (obj.GetType() != this.GetType()) return false;
                return Equals((outerRow)obj);
            }
            public bool Equals(outerRow obj)
            {
                if (bay == obj.bay || bay == 0 || obj.bay == 0)
                    if (this.starboardMost == obj.starboardMost && this.portMost == obj.portMost)
                        return true;
                return false;
            }
            public bool Equals(Dg obj)
            {
                if (bay == obj.bay || bay == obj.bay + 1 || bay == obj.bay-1 || bay == 0)
                    if (portMost == obj.row || starboardMost == obj.row)
                        return true;
                return false;
            }
            public bool Equals(Container obj)
            {
                if (bay == obj.bay || bay == obj.bay + 1 || bay == obj.bay - 1 || bay == 0)
                    if (portMost == obj.row || starboardMost == obj.row)
                        return true;
                return false;
            }
            public static bool operator == (outerRow a, outerRow b)
            {
                return a.Equals(b);
            }
            public static bool operator !=(outerRow a, outerRow b)
            {
                return !(a == b);
            }
            public static bool operator ==(outerRow a, Dg b)
            {
                return a.Equals(b);
            }
            public static bool operator !=(outerRow a, Dg b)
            {
                return !(a == b);
            }
            public static bool operator ==(outerRow a, Container b)
            {
                return a.Equals(b);
            }
            public static bool operator !=(outerRow a, Container b)
            {
                return !(a == b);
            }

        }

        }
    }
