using EasyJob_ProDG.Model.Cargo;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EasyJob_ProDG.ModelTests.Cargo")]
namespace EasyJob_ProDG.Model.Transport
{
    public partial class ShipProfile
    {
        #region Singleton

        private static ShipProfile _profile;
        public static ShipProfile Instance => _profile; 

        #endregion


        #region Private fields

        /// <summary>
        /// The last bay before the only one or the first one supersturcture
        /// </summary>
        private byte _superstructure1;

        /// <summary>
        /// The last bay before the second superstructure, if any.
        /// </summary>
        private byte _superstructure2;

        /// <summary>
        /// Bays within 12 m from superstructures
        /// </summary>
        private List<byte> Bays12metersAroundSuperstructures
        {
            get
            {
                if (_bays12mAroundSuperstructures == null)
                    SetSupersturcture12meters();
                return _bays12mAroundSuperstructures;
            }
        }
        private List<byte> _bays12mAroundSuperstructures;

        /// <summary>
        /// Bays and Rows within 12 m of LSA from <see cref="LSA"/> list.
        /// </summary>
        private List<byte>[,] LSA12mBaysAndRows
        {
            get
            {
                if (_lsa12mBaysAndRows == null)
                    Set12mOfLSABaysAndRows();
                return _lsa12mBaysAndRows;
            }
        }
        private List<byte>[,] _lsa12mBaysAndRows;

        #endregion


        #region Reefer motor facing

        [Flags]
        public enum MotorFacing : byte
        {
            NotDefined, Aft, Forward
        }
        public static List<string> MotorFacingList => new List<string>() { "Not defined", "Aft", "Forward" };

        #endregion


        #region Public ShipProfile properties
        // -------------- Public properties -------------------

        /// <summary>
        /// <see langword="true"/>if ShipProfile contains errors.
        /// </summary>
        public bool ContainsErrors => containsErrors;
        private bool containsErrors;

        /// <summary>
        /// <see langword="true"/>if default ShipProfile has been loaded due to corrupt or missing file.
        /// </summary>
        public bool IsDefault => isDefault;
        private bool isDefault;

        /// <summary>
        /// Indicates that the ship profile .ini file has not been found.
        /// </summary>
        public bool IsShipProfileNotFound => isShipProfileNotFound;
        private bool isShipProfileNotFound;

        public string ShipName { get; set; }
        public string CallSign
        {
            get => _callsign;
            set => _callsign = value?.ToUpper() ?? "";
        }
        private string _callsign;

        public byte NumberOfHolds
        {
            get => _numberOfHolds;
            set
            {
                if (_numberOfHolds == value) return;
                if (CargoHolds != null)
                {
                    if(_numberOfHolds < value)
                    {
                        for (byte i = (byte)(_numberOfHolds + 1); i < value + 1; i++)
                        {
                            CargoHolds.Add(new CargoHold(i));
                        }
                    } 
                    if(_numberOfHolds > value)
                    {
                        for (byte i = _numberOfHolds; i > value; i--)
                        {
                            CargoHolds.RemoveAt(i - 1);
                        }
                    }
                }
                _numberOfHolds = value;
            }
        }
        private byte _numberOfHolds;
        public byte RfMotor { get; set; }
        public bool Row00Exists { get; set; }
        public bool Passenger { get; set; }
        public List<CargoHold> CargoHolds { get; set; }
        public byte NumberOfSuperstructures { get; set; }

        /// <summary>
        /// Contains bays numbers surrounding superstructures
        /// </summary>
        public List<byte> BaysSurroundingSuperstructure { get; set; }

        /// <summary>
        /// Contains bay just in front of the superstructures
        /// </summary>
        public List<byte> BaysInFrontOfSuperstructures { get; set; }

        /// <summary>
        /// List of <see cref="OuterRow"/> representing sea sides.
        /// </summary>
        public List<OuterRow> SeaSides { get; set; }

        /// <summary>
        /// List of Living quarters in accordance with IMDG code definitions
        /// </summary>
        public List<CellPosition> LivingQuarters { get; set; }

        /// <summary>
        /// List of Heating structures in accordance with IMDG code definitions
        /// </summary>
        public List<CellPosition> HeatedStructures { get; set; }

        /// <summary>
        /// List of Life-saving appliances other than main ones in accordance with IMDG code definitions
        /// </summary>
        public List<CellPosition> LSA { get; set; }

        /// <summary>
        /// Document of compliance
        /// </summary>
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
        /// <summary>
        /// List contains list of errors found by CheckShipProfile method
        /// </summary>
        private List<string> _errorList;

        #endregion


        #region Public methods

        // ----- Static methods -----

        /// <summary>
        /// The Method defines in which cargo hold the bay is located. This is literally, in which cargo hold or over which cargo hold is a container location
        /// </summary>
        /// <param name="bay"></param>
        /// <returns></returns>
        public static byte DefineCargoHoldNumber(byte bay)
        {
            byte chNr = 0;
            for (int i = 0; i < _profile?.CargoHolds?.Count; i++)
            {
                if (bay <= _profile.CargoHolds[i].LastBay && bay >= _profile.CargoHolds[i].FirstBay)
                {
                    chNr = (byte)(i + 1);
                    break;
                }
            }
            return chNr;
        }

        public static ShipProfile GetDefaultShipProfile()
        {
            return new ShipProfile();
        }


        // ----- Non-static -----

        /// <summary>
        /// Sets this <see cref="ShipProfile"/> to be the one in use for all operations.
        /// </summary>
        internal void SetThisShipProfileToShip()
        {
            _profile = this;
        }

        /// <summary>
        /// Update method to run private methods to update private fields.
        /// Updates 12 m of superstructures and LSA.
        /// </summary>
        public void UpdatePrivateProperties()
        {
            SetSupersturcture12meters();
            Set12mOfLSABaysAndRows();
        }

        /// <summary>
        /// Creates and sets values to <see cref="BaysInFrontOfSuperstructures"/> and <see cref="BaysSurroundingSuperstructure"/> properites.
        /// Older values will be vanished.
        /// Order of parameters has no effect.
        /// </summary>
        /// <param name="bay1">The last bay in front of the only or the first superstructure.</param>
        /// <param name="bay2">The last bay in front of the second superstructure, if any.</param>
        public void SetSuperstructuresBaysProperties(byte bay1, byte bay2 = 255)
        {
            _superstructure1 = bay1;
            _superstructure2 = bay2;
            BaysInFrontOfSuperstructures = new List<byte>();
            BaysSurroundingSuperstructure = new List<byte>();

            AddSuperstructuresBaysProperties(bay1);
            if (NumberOfSuperstructures == 2)
                AddSuperstructuresBaysProperties(bay2);
        }

        /// <summary>
        /// The Method defines in which cargo hold the bay is located. This is literally, in which cargo hold or over which cargo hold is a container location.
        /// Same method as <see cref="DefineCargoHoldNumber(byte)"/>, but called non-statically
        /// </summary>
        /// <param name="bay"></param>
        /// <returns></returns>
        public byte DefineCargoHoldNumberOfBay(byte bay)
        {
            return DefineCargoHoldNumber(bay);
        }

        #endregion

        #region Internal methods
        // ----- Internal methods -----

        /// <summary>
        /// Changes IsDefault property to 'false'
        /// </summary>
        internal void SetIsNotDefault()
        {
            isDefault = false;
        }

        /// <summary>
        /// Adds necessary bays to <see cref="BaysInFrontOfSuperstructures"/> and <see cref="BaysSurroundingSuperstructure"/> properties.
        /// </summary>
        /// <param name="bay">The last bay in front of a superstructure.</param>
        internal void AddSuperstructuresBaysProperties(byte bay)
        {
            BaysInFrontOfSuperstructures.Add(bay % 2 == 0 ? (byte)(bay + 1) : bay);

            if (BaysSurroundingSuperstructure == null) BaysSurroundingSuperstructure = new List<byte>();
            if (BaysSurroundingSuperstructure.Contains(bay)) return;
            BaysSurroundingSuperstructure.Add(bay);
            BaysSurroundingSuperstructure.Add((byte)(bay + 1));
            BaysSurroundingSuperstructure.Add((byte)(bay + 2));
            BaysSurroundingSuperstructure.Add((byte)(bay + 3));
            if (bay % 2 != 0) BaysSurroundingSuperstructure.Add((byte)(bay - 1));
            if (bay % 2 == 0) BaysSurroundingSuperstructure.Add((byte)(bay + 4));
        }

        internal bool IsInLivingQuarters(ILocationOnBoard container)
        {
            //Clear of accommodation
            return (BaysSurroundingSuperstructure != null && BaysSurroundingSuperstructure.Contains(container.Bay)) || IsWithinCells(container, LivingQuarters);
        }

        internal bool IsInHeatedStructures(ILocationOnBoard dg)
        {
            return IsWithinCells(dg, HeatedStructures);
        }

        /// <summary>
        /// Method checks is dg located within 12m from LSA and accommodation and returns true if it is less than 12m
        /// </summary>
        /// <param name="dg"></param>
        /// <returns></returns>
        internal bool IsNotClearOfLSA(ILocationOnBoard dg)
        {
            //1. accommodation
            if (Bays12metersAroundSuperstructures.Contains(dg.Bay)) return true;

            //2. LSA
            if (IsWithin12mofLSA(dg)) return true;

            return false;
        }

        /// <summary>
        /// Defines if the unit is located on a sea side from <see cref="SeaSides"/>
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal bool IsOnSeaSide(ILocationOnBoard unit)
        {
            bool defined = IsSeasidesContainBay(unit.Bay);
            foreach (OuterRow outerRow in SeaSides)
            {
                if (defined)
                { if (outerRow.Bay == unit.Bay && outerRow == unit) return true; }
                else if (outerRow == unit) return true;
            }
            return false;
        }

        #endregion

        #region Private methods
        // ----- Private methods -----

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

        /// <summary>
        /// Defines if an ILocationOnBoard unit is within 12 m of LSA from <see cref="LSA"/>
        /// </summary>
        /// <param name="dg"><see cref="ILocationOnBoard"/></param>
        /// <returns>True if within 12 m</returns>
        private bool IsWithin12mofLSA(ILocationOnBoard dg)
        {
            for (int i = 0; i < LSA12mBaysAndRows.Length / 2; i++)
            {
                if (LSA12mBaysAndRows[i, 0].Contains(dg.Bay) && LSA12mBaysAndRows[i, 1].Contains(dg.Row)) return true;
            }
            return false;
        }

        /// <summary>
        /// Creates <see cref="Bays12metersAroundSuperstructures"/> - List of bays representing 12 m around superstructure
        /// </summary>
        private void SetSupersturcture12meters()
        {
            _bays12mAroundSuperstructures = new List<byte>
                        {        _superstructure1,
                          (byte)(_superstructure1 + 1),
                          (byte)(_superstructure1 + 2),
                          (byte)(_superstructure1 + 3),
                          (byte)(_superstructure1 + 4),
                          (byte)(_superstructure1 + 5),
                          (byte)(_superstructure1 - 1),
                          (byte)(_superstructure1 - 2) };

            if (NumberOfSuperstructures == 2)
            {
                _bays12mAroundSuperstructures.AddRange(new List<byte>()
                         {       _superstructure2,
                          (byte)(_superstructure2 + 1),
                          (byte)(_superstructure2 + 2),
                          (byte)(_superstructure2 + 3),
                          (byte)(_superstructure2 + 4),
                          (byte)(_superstructure2 + 5),
                          (byte)(_superstructure2 - 1),
                          (byte)(_superstructure2 - 2)});
            }
        }

        /// <summary>
        /// Creates <see cref="LSA12mBaysAndRows"/> array containing list of bays and rows within 12 m of each <see cref="LSA"/> position.
        /// </summary>
        private void Set12mOfLSABaysAndRows()
        {
            _lsa12mBaysAndRows = new List<byte>[LSA.Count, 2];

            int index = 0;
            foreach (CellPosition lsa in LSA)
            {
                #region create list of rows
                // Create list of rows
                List<byte> ath = new List<byte>();

                if (lsa.Row == 99)
                {
                    for (byte i = 0; i < 42; i++)
                    {
                        ath.Add(i);
                    }
                }
                else
                {
                    ath.AddRange(new[]
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
                    });
                    if (lsa.Row == 0) { ath.Add(1); ath.Add(3); ath.Add(5); ath.Add(7); ath.Add(9); }
                    else if (lsa.Row == 1)
                    {
                        ath.Add(2); ath.Add(4);
                        ath.Add(6); ath.Add(8);
                        ath.Add(!Row00Exists ? (byte)10 : (byte)0);
                    }
                    else if (lsa.Row == 2)
                    {
                        ath.Add(1); ath.Add(3); ath.Add(5); ath.Add(7);
                        if (!Row00Exists) ath.Add(9);
                    }
                    else if (lsa.Row == 3)
                    {
                        ath.Add(2); ath.Add(4); ath.Add(6);
                        if (!Row00Exists) ath.Add(8);
                    }
                    else if (lsa.Row == 4)
                    {
                        ath.Add(1); ath.Add(3); ath.Add(5);
                        if (!Row00Exists) ath.Add(7);
                    }
                    else if (lsa.Row == 5)
                    {
                        ath.Add(2); ath.Add(4);
                        if (!Row00Exists) ath.Add(6);
                    }
                    else if (lsa.Row == 6)
                    {
                        ath.Add(1); ath.Add(3);
                        if (!Row00Exists) ath.Add(5);
                    }
                    else if (lsa.Row == 7)
                    {
                        ath.Add(2);
                        if (!Row00Exists) ath.Add(4);
                    }
                    else if (lsa.Row == 8)
                    {
                        ath.Add(1);
                        if (!Row00Exists) ath.Add(3);
                    }
                    else if (lsa.Row == 9 && !Row00Exists) ath.Add(2);
                    else if (lsa.Row == 10 && !Row00Exists) ath.Add(1);
                    else if (lsa.Row <= 10 && !ath.Contains(0)) ath.Add(0);
                }

                _lsa12mBaysAndRows[index, 1] = ath;
                #endregion

                // create list of bays
                List<byte> bays = new List<byte>
                {
                    lsa.Bay, (byte)(lsa.Bay + 1), (byte)(lsa.Bay + 2), (byte)(lsa.Bay + 3), (byte)(lsa.Bay + 4), (byte)(lsa.Bay + 5),
                    lsa.Bay, (byte)(lsa.Bay - 1), (byte)(lsa.Bay - 2), (byte)(lsa.Bay - 3), (byte)(lsa.Bay - 4), (byte)(lsa.Bay - 5)
                };
                _lsa12mBaysAndRows[index, 0] = bays;

                index++;
            }
        }

        /// <summary>
        /// Method determines if given bay exists in seaSides.
        /// </summary>
        /// <param name="bay"></param>
        /// <returns></returns>
        private bool IsSeasidesContainBay(byte bay)
        {
            foreach (OuterRow oRow in SeaSides)
                if (oRow.Bay == bay)
                    return true;
            return false;
        }

        #endregion


        #region Constructors

        // ------------------------------ Ship profile constructors ------------------------------------------------------

        /// <summary>
        /// Constructor for default ship with one cargo hold
        /// </summary>
        private ShipProfile()
        {
            SetSuperstructuresBaysProperties(100);
            NumberOfHolds = 1;
            CargoHolds = new List<CargoHold>(NumberOfHolds) { new CargoHold(1, 1, 199) };
            LivingQuarters = new List<CellPosition>() { new CellPosition(99, 199, 199, 199, 0) };
            HeatedStructures = new List<CellPosition>() { new CellPosition(99, 199, 199, 199, 0) };
            LSA = new List<CellPosition>() { new CellPosition(99, 199, 199, 199, 0) };
            Row00Exists = true;
            RfMotor = (byte)MotorFacing.NotDefined;
            SeaSides = new List<OuterRow> { new OuterRow(0, 99, 99) };
            Doc = new DOC(NumberOfHolds);
            _errorList = new List<string>();
            isDefault = true;
        }

        #endregion
    }
}
