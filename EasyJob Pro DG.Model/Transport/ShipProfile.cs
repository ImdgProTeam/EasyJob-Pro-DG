using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.Cargo;
using System;
using System.Collections.Generic;
using System.IO;

namespace EasyJob_ProDG.Model.Transport
{
    public partial class ShipProfile
    {
        /// <summary>
        /// Reading and saving to config.file to be implemented
        /// </summary>
        private string _callsign;
        private byte _accommodation;
        private List<byte> _accommodation12;
        private bool _filecorrupt;
        private bool containsErrors;
        private bool isDefault;

        /// <summary>
        /// List contains list of errors found by CheckShipProfile method
        /// </summary>
        private List<string> _errorList;

        [Flags]
        public enum MotorFacing : byte
        {
            NotDefined, Aft, Forward
        }
        public static List<string> MotorFacingList => new List<string>() { "Not defined", "Aft", "Forward" };


        //--------------- Properties and a method to set and get ship profile items ----------------------------------

        #region Public ShipProfile properties
        // -------------- Public properties -------------------

        /// <summary>
        /// <see langword="true"/>if ShipProfile contains errors.
        /// </summary>
        public bool ContainsErrors => containsErrors;

        /// <summary>
        /// <see langword="true"/>if default ShipProfile has been loaded due to corrupt or missing file.
        /// </summary>
        public bool IsDefault => isDefault;
        public string ShipName { get; set; }
        public string CallSign
        {
            get { return _callsign; }
            set { _callsign = value?.ToUpper() ?? ""; }
        }
        public byte NumberOfHolds { get; set; }
        public byte RfMotor { get; set; }
        public bool Row00Exists { get; set; }
        public bool Passenger { get; set; }
        public List<CargoHold> Holds { get; set; }
        public byte NumberOfAccommodations { get; set; }
        public List<byte> Accommodation { get; set; }
        public List<byte> AccommodationBays { get; set; }
        public List<OuterRow> SeaSides { get; set; }
        public List<CellPosition> LivingQuartersList { get; set; }
        public List<CellPosition> HeatedStructuresList { get; set; }
        public List<CellPosition> LSAList { get; set; }
        public DOC Doc { get; set; }
        /// <summary>
        /// Property to easily add unique errors in the errorList
        /// </summary>
        public string ErrorList
        {
            set
            {
                if (!_errorList.Contains(value)) _errorList.Add(value);
            }
            get
            {
                string result = "";
                foreach (var str in _errorList)
                    result += result == "" ? "" : ", " + str;
                return result;
            }
        } 
        #endregion



        #region Constructors

        // ------------------------------ Ship profile constructors ------------------------------------------------------

        /// <summary>
        /// Constructor reads ShipProfile.ini and creates Ship model as per content
        /// </summary>
        /// <param name="shipFile">File path.</param>
        /// <param name="containsErrors">out parameter: True if errors appeared on checking ShipProfile after reading.</param>
        public ShipProfile(string shipFile)
        {
            SetDefaultValues();

            Exception ex = new Exception();
            ReadShipProfileFromFile(shipFile, ex);

            //checking profile for errors and assigning respective value to out parameter.
            if (!CheckErrorsInShipProfile())
                containsErrors = true;
            else containsErrors = false;
        }

        /// <summary>
        /// Additional constructor to create a new file
        /// </summary>
        /// <param name="shipName"></param>
        /// <param name="passenger"></param>
        public ShipProfile(string shipName, bool passenger)
        {
            ShipName = shipName;
            Passenger = passenger;
            SeaSides = new List<OuterRow>();
            LivingQuartersList = new List<CellPosition>();
            HeatedStructuresList = new List<CellPosition>();
            LSAList = new List<CellPosition>();
            RfMotor = (byte)MotorFacing.NotDefined;
            Row00Exists = true;
            AccommodationBays = new List<byte>();
            _errorList = new List<string>();
        }

        /// <summary>
        /// Constructor for default ship with one cargo hold
        /// </summary>
        public ShipProfile()
        {
            AccommodationBays = new List<byte>();
            SetAccommodation(100);
            NumberOfHolds = 1;
            Holds = new List<CargoHold>(NumberOfHolds);
            Holds.Add(new CargoHold(1, 100));
            LivingQuartersList = new List<CellPosition>();
            LivingQuarters = new CellPosition(99, 199, 199, 199, 00);
            HeatedStructuresList = new List<CellPosition>();
            HeatedStructures = new CellPosition(99, 199, 199, 199, 0);
            LSAList = new List<CellPosition>();
            LSA = new CellPosition(99, 199, 199, 199, 0);
            Row00Exists = true;
            RfMotor = (byte)MotorFacing.NotDefined;
            SeaSides = new List<OuterRow> { new OuterRow(0, 99, 99) };
            Doc = new DOC(NumberOfHolds);
            _errorList = new List<string>();
        }


        /// <summary>
        /// Initializes fields with default values in order to get them ready for further update.
        /// </summary>
        private void SetDefaultValues()
        {
            _filecorrupt = false;
            _errorList = new List<string>();
            SeaSides = new List<OuterRow>() { new OuterRow(0, 99, 99) };
            LivingQuartersList = new List<CellPosition>();
            HeatedStructuresList = new List<CellPosition>();
            LSAList = new List<CellPosition>();
            RfMotor = (byte)MotorFacing.NotDefined;
            Row00Exists = true;
            AccommodationBays = new List<byte>() { 0 };
            NumberOfHolds = 1;
            Holds = new List<CargoHold> { new CargoHold(1, 199) };
            Doc = new DOC(2);
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Reads ShipProfile from a file.
        /// </summary>
        /// <param name="shipFile"></param>
        /// <param name="ex"></param>
        private void ReadShipProfileFromFile(string shipFile, Exception ex)
        {
            int linecount = 0;
            using (var reader = new StreamReader(shipFile))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        var line = reader.ReadLine();
                        if (linecount == 0 && line != "***ShipProfile***")
                        {
                            LogWriter.Write("Ship profile file is wrong or modified.");
                            _filecorrupt = true;
                            return;
                        }
                        if (line != null && !line.StartsWith("//") && line.Length != 0 && line.Contains("="))
                        {
                            line = line.Replace("=  ", "=").Replace("= ", "=");
                            string lineValue = line.Substring(line.IndexOf('=') + 1);
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
                                    NumberOfAccommodations = byte.Parse(lineValue);
                                    break;
                                case "row00exists":
                                    ex.Source = "row00exists";
                                    Row00Exists = bool.Parse(lineValue);
                                    break;
                                case "passenger":
                                    ex.Source = "passenger";
                                    Passenger = bool.Parse(lineValue);
                                    break;
                                case "seasides":
                                    ex.Source = "seasides";
                                    OuterRow instance = new OuterRow();
                                    byte i = 0;
                                    foreach (string figure in lineValue.Split(','))
                                    {
                                        instance[i] = int.Parse(figure);
                                        i++;
                                    }

                                    if (instance.Bay == 0)
                                        SeaSides.RemoveAt(0);
                                    SeaSides.Add(instance);
                                    break;
                                case "reefermotorsfacing":
                                    ex.Source = "reefermotor";
                                    RfMotor = byte.Parse(lineValue);
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
                                    NumberOfHolds = byte.Parse(lineValue);
                                    Holds = new List<CargoHold>();
                                    Doc = new DOC(NumberOfHolds);
                                }
                                else
                                {
                                    var FandLbays = lineValue.Split(',');
                                    Holds.Add(new CargoHold(byte.Parse(FandLbays[0]), byte.Parse(FandLbays[1])));
                                }
                            }

                            if (lineDescr.StartsWith("doc"))
                            {
                                ex.Source = "doc";
                                if (lineDescr.Substring(3).StartsWith("weather"))
                                    Doc.SetDOCTableRow(lineValue, 0);
                                else
                                    Doc.SetDOCTableRow(lineValue, byte.Parse(lineDescr.Substring(7)));
                            }
                        }
                    }
                    catch
                    {
                        ErrorList = ex.Source;
                        LogWriter.Write("An error occurred while opening the ship profile.");
                        _filecorrupt = true;
                    }

                    linecount++;
                }
            }
        }


        // ----------------- Supportive methods to determine certain locations of given units ----------------------------


        // ------------- Set only properties ------------------

        private CellPosition LivingQuarters
        {
            set { LivingQuartersList.Add(value); }
        }
        private CellPosition HeatedStructures
        {
            set { HeatedStructuresList.Add(value); }
        }
        private CellPosition LSA
        {
            set { LSAList.Add(value); }
        }

        // --------------- Methods ---------------------------
        // ----- Private -----

        /// <summary>
        /// Method determines if dg unit falls within given list of positions
        /// </summary>
        /// <param name="container"></param>
        /// <param name="cells"></param>
        /// <returns></returns>
        private bool IsWithinCells(ILocationOnBoard container, List<CellPosition> cells)
        {
            foreach (CellPosition cell in cells)
                if (cell == container)
                    return true;
            return false;
        }

        #endregion

        #region Internal and Public methods

        // ----- Internal -----

        internal bool IsInLivingQuarters(ILocationOnBoard container)
        {
            //Clear of accommodation
            return (Accommodation != null && Accommodation.Contains(container.Bay)) || IsWithinCells(container, LivingQuartersList);
        }

        internal bool IsInHeatedStructures(Dg dg)
        {
            return IsWithinCells(dg, HeatedStructuresList);
        }

        /// <summary>
        /// Method checks is dg located within 12m from LSA and accommodation and returns true if it is less than 12m
        /// </summary>
        /// <param name="dg"></param>
        /// <returns></returns>
        internal bool IsNotClearOfLSA(Dg dg)
        {
            //1. accommodation
            if (_accommodation12 == null)
            {
                _accommodation12 = new List<byte> { _accommodation, (byte)(_accommodation + 1), (byte)(_accommodation + 2), (byte)(_accommodation + 3), (byte)(_accommodation + 4), (byte)(_accommodation + 5), (byte)(_accommodation - 1), (byte)(_accommodation - 2) };
            }

            if (_accommodation12.Contains(dg.Bay)) return true;

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
                    ath.Add(!Row00Exists ? (byte)10 : (byte)0);
                }
                if (lsa.Row == 2)
                {
                    ath.Add(1); ath.Add(3); ath.Add(5); ath.Add(7);
                    if (!Row00Exists) ath.Add(9);
                }
                if (lsa.Row == 3)
                {
                    ath.Add(2); ath.Add(4); ath.Add(6);
                    if (!Row00Exists) ath.Add(8);
                }
                if (lsa.Row == 4)
                {
                    ath.Add(1); ath.Add(3); ath.Add(5);
                    if (!Row00Exists) ath.Add(7);
                }
                if (lsa.Row == 5)
                {
                    ath.Add(2); ath.Add(4);
                    if (!Row00Exists) ath.Add(6);
                }
                if (lsa.Row == 6)
                {
                    ath.Add(1); ath.Add(3);
                    if (!Row00Exists) ath.Add(5);
                }
                if (lsa.Row == 7)
                {
                    ath.Add(2);
                    if (!Row00Exists) ath.Add(4);
                }
                if (lsa.Row == 8)
                {
                    ath.Add(1);
                    if (!Row00Exists) ath.Add(3);
                }
                if (lsa.Row == 9 && !Row00Exists) ath.Add(2);
                if (lsa.Row == 10 && !Row00Exists) ath.Add(1);
                if (lsa.Row <= 10 && !ath.Contains(0)) ath.Add(0);
                #endregion

                List<byte> bays = new List<byte>
                {
                    lsa.Bay, (byte)(lsa.Bay + 1), (byte)(lsa.Bay + 2), (byte)(lsa.Bay + 3), (byte)(lsa.Bay + 4), (byte)(lsa.Bay + 5),
                    lsa.Bay, (byte)(lsa.Bay - 1), (byte)(lsa.Bay - 2), (byte)(lsa.Bay - 3), (byte)(lsa.Bay - 4), (byte)(lsa.Bay - 5)
                };

                if (ath.Contains(dg.Bay) && bays.Contains(dg.Bay)) return true;
            }

            return false;
        }

        /// <summary>
        /// Method determines if given bay exists in seaSides.
        /// </summary>
        /// <param name="bay"></param>
        /// <returns></returns>
        internal bool SeasidesBayDefined(byte bay)
        {
            foreach (OuterRow oRow in SeaSides)
                if (oRow.Bay == bay)
                    return true;
            return false;
        }

        internal bool IsOnSeaSide(Dg unit)
        {
            bool defined = SeasidesBayDefined(unit.Bay);
            foreach (OuterRow outerRow in SeaSides)
            {
                if (defined)
                { if (outerRow.Bay == unit.Bay && outerRow == unit) return true; }
                else if (outerRow == unit) return true;
            }

            return false;
        }


        // ----- Public -----

        /// <summary>
        /// The Method defines in which cargo hold locates the bay. This is literally, in which cargo hold or over which cargo hold is a container loaction
        /// </summary>
        /// <param name="bay"></param>
        /// <returns></returns>
        public byte DefineCargoHoldNumber(byte bay)
        {
            byte chNr = 0;
            for (int i = 0; i < Holds.Count; i++)
            {
                if (bay <= Holds[i].LastBay && bay >= Holds[i].FirstBay)
                {
                    chNr = (byte)(i + 1);
                    break;
                }
            }
            return chNr;
        }

        /// <summary>
        /// Sets bays numbers surrounding Accommodation.
        /// </summary>
        /// <param name="bay"></param>
        public void SetAccommodation(byte bay)
        {
            _accommodation = bay;
            if (AccommodationBays.Contains(0)) AccommodationBays.Remove(0);
            AccommodationBays.Add(bay % 2 == 0 ? (byte)(bay + 1) : bay);
            if (Accommodation == null) Accommodation = new List<byte>();
            if (Accommodation.Contains(bay)) return;
            Accommodation.Add(bay);
            Accommodation.Add((byte)(bay + 1));
            Accommodation.Add((byte)(bay + 2));
            Accommodation.Add((byte)(bay + 3));
            if (bay % 2 != 0) Accommodation.Add((byte)(bay - 1));
            if (bay % 2 == 0) Accommodation.Add((byte)(bay + 4));
        }

        #endregion

    }
}
