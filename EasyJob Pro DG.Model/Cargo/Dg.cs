using EasyJob_ProDG.Data.Info_data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public partial class Dg : LocationOnBoard, IContainer, IO.IUpdatable
    {
        #region Fields Declarations
        //Read from edi.
        //DGS+IMD+3(Class):+1234(UN)+-23:CEL(FP)+2(PG)+F-AS-E(EMS)+++:+::'
        //FTX+AAD+++NIL(description):125(NW):'
        private byte packingGroup;
        private double flashPoint;
        private readonly List<string> dgsubclass;
        private readonly List<string> allDgClasses;
        private string dgclass;

        //assign from DG List
        private bool isStabilizedWordInProperShippingName;
        private bool isStabilizedWordAddedToProperShippingName;
        private bool isSelfReactive = false;
        private char stowageCatFromDgList;
        private List<string> segregationSG;
        private List<int> special;
        private List<string> stowageSW;
        private List<string> stowageSWfromDgList;
        public bool differentClass;
        public bool mpDetermined;

        //from other sources
        private readonly List<int> segregationGroup;
        public byte DgRowInDOC;
        public byte dgRowInTable;
        public byte[] Stack;
        public char CompatibilityGroup = '0';
        public string SegregatorClass;


        //from IFTDGN
        internal int numberOfPackages;
        internal string typeOfPackages;
        internal string typeOfPackagesDescription;
        public string NumberAndTypeOfPackages { get; set; }
        public string TechnicalName { get; set; }

        #endregion

        #region Public properties
        //----------------- public properties -----------------------------
        // ---------------- computed properties ---------------------------

        public bool IsStabilized
        {
            get
            {
                return isStabilizedWordInProperShippingName ? true : isStabilizedWordAddedToProperShippingName;
            }
            set
            {
                isStabilizedWordAddedToProperShippingName = value;
                if (value == true)
                    if (isStabilizedWordInProperShippingName == false)
                    {
                        StowageCat = 'D';
                        stowageSW.Add("SW1");
                    }
                if (value == false)
                    if (isStabilizedWordInProperShippingName == false)
                    {
                        StowageCat = stowageCatFromDgList;
                        stowageSW = stowageSWfromDgList;
                    }
            }
        }
        public bool IsSelfReactive
        {
            get { return isSelfReactive; }
            set { isSelfReactive = value; }
        }
        public byte PackingGroupByte
        {
            get { return packingGroup; }
            set { packingGroup = value; }
        }
        public double FlashPointDouble
        {
            set { flashPoint = value; }
            get { return flashPoint; }
        }
        public int Unno { get; set; }

        /// <summary>
        /// Set: adds string dg class to dgclass and will update allDgClasses.
        /// Get: returns string with dgclass.
        /// </summary>
        public string DgClass
        {
            get
            {
                return dgclass;
            }
            set
            {
                dgclass = value;
                AllDgClasses = value;
            }
        }

        public string FlashPoint
        {
            get
            {
                return Math.Abs(flashPoint - 9999) < 1 ? "" : flashPoint.ToString(CultureInfo.InvariantCulture);
            }
            set
            {
                if (string.IsNullOrEmpty(value)) flashPoint = 9999;
                else flashPoint = double.Parse(value);
            }
        }
        /// <summary>
        /// Set: will add string dg class to dgsubclass and will update allDgClasses.
        /// Get: will return string with all subclasses listed and separated with comma.
        /// </summary>
        /// 
        public string DgSubclass
        {
            get
            {
                string temp = null;
                if (dgsubclass.Count <= 0) return "";
                foreach (var x in dgsubclass) temp += temp == null ? x.ToString(CultureInfo.InvariantCulture) : ", " + x.ToString(CultureInfo.InvariantCulture);
                return temp;
            }

            set
            {
                if (dgsubclass.Contains(value) || string.IsNullOrEmpty(value)) return;
                dgsubclass.Add(value);
                AllDgClasses = value;
            }
        }

        /// <summary>
        /// Set: will parse string with packing group and record it to dgpg.
        /// Get: will return string containing better view of a packing group.
        /// </summary>
        public string PackingGroup
        {
            get
            {
                string temp = null;
                for (int i = 0; i < packingGroup; i++)
                    temp += "I";
                return temp;
            }

            set
            {
                try
                {
                    packingGroup = Convert.ToByte(value);
                    if (packingGroup > 3) packingGroup = 3;
                }
                catch (Exception)
                {
                    switch (value.ToUpper())
                    {
                        case "I":
                            packingGroup = 1;
                            break;
                        case "II":
                            packingGroup = 2;
                            break;
                        case "III":
                            packingGroup = 3;
                            break;
                        default:
                            packingGroup = 0;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Set: will check if segregation group exists in the list and will assign it to dg unit.
        /// Get: returns string containing all segregation groups separated with comma.
        /// </summary>
        public string SegregationGroup
        {
            get
            {
                string result = null;
                foreach (int itemNr in segregationGroup)
                {
                    string group = IMDGCode.SegregationGroups[itemNr];
                    result += result == null ? group : ", " + group;
                }
                return result;
            }
            set
            {
                int segregationGroupIndex = 0;

                //By full group title as mentioned in IMDG code
                if (IMDGCode.SegregationGroups.Contains(value) &&
                   !segregationGroup.Contains(Array.IndexOf(IMDGCode.SegregationGroups, value)))
                {
                    segregationGroup.Add(Array.IndexOf(IMDGCode.SegregationGroups, value));
                }
                //By code only
                else if (IMDGCode.SegregationGroupsCodes.Contains(value))
                {
                    segregationGroupIndex = Array.IndexOf(IMDGCode.SegregationGroupsCodes, value);
                    if(!segregationGroup.Contains(segregationGroupIndex))
                        segregationGroup.Add(segregationGroupIndex);
                }
                //Parse multiple values
                else if (value.Contains(","))
                {
                    foreach (var group in value.Replace(" ","").Split(','))
                    {
                        SegregationGroup = group;
                    }
                }
                //By index in dictionary
                else if(int.TryParse(value, out segregationGroupIndex))
                {
                    if(!segregationGroup.Contains(segregationGroupIndex))
                        segregationGroup.Add(segregationGroupIndex);
                }
                
                    
            }
        }

        /// <summary>
        /// Returns string with all segregation group codes listed.
        /// </summary>
        public string SegregationGroupCodes
        {
            get
            {
                string result = "";
                foreach (var index in SegregationGroupList)
                {
                    if (!string.IsNullOrEmpty(result))
                        result += ", ";
                    result += IMDGCode.SegregationGroupsCodes[index];
                }
                return result;
            }
        }


        // ---------------- auto-properties ------------------------------

        public bool EmitFlammableVapours { get; set; }
        public bool Flammable { get; set; }
        public bool IsConflicted { get; set; }
        public bool IsLq { get; set; }
        public bool Liquid { get; set; }
        public bool IsMp { get; set; }
        public bool IsMax1L { get; set; }
        public bool IsWaste { get; set; }
        public char StowageCat { get; set; }
        public Conflict Conflict { get; set; }
        public decimal DgNetWeight { get; set; }
        public string EmergencyContacts { get; set; }
        public string Remarks { get; set; }

        /// <summary>
        /// Property returns number of dg subclasses
        /// </summary>
        public int DgsubclassCount => dgsubclass.Count;
        public List<int> SegregationGroupList => segregationGroup;
        public string DgEMS { get; set; }
        public string Name { get; set; } //Proper shipping name
        public string OriginalNameFromCode { get; private set; }
        public bool IsNameChanged { get; set; }
        public bool IsTechnicalNameIncluded { get; set; }
        public string Surrounded { get; set; }

        // -------------- Properties copied from Container class ---------------------------

        public string ContainerNumber { get; set; }
        public string ContainerType { get; set; }
        public bool ContainerTypeRecognized { get; set; }
        public bool IsClosed { get; set; }
        public bool IsRf { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public string FinalDestination { get; set; }
        public string Carrier { get; set; }

        //--------------- Readonly properties -----------------------------------------------

        public List<string> SegregationSGList => segregationSG;
        public List<string> StowageSWList => stowageSW;
        public SegregatorException SegregatorException { get; set; }
        public string Properties { get; private set; }
        public string SegregationSG
        {
            get
            {
                string result = null;
                foreach (string item in segregationSG)
                {
                    result += (result == null ? "" : ", ") + item;
                }
                return result;
            }
        }
        public string Special
        {
            get
            {
                string result = null;
                foreach (int item in special)
                {
                    result += (result == null ? "" : ", ") + item;
                }
                return result;
            }
        }
        public string StowageSW
        {
            get
            {
                string result = null;
                foreach (string item in stowageSW)
                {
                    result += (string.IsNullOrEmpty(result) ? "" : ", ") + (item == "0" ? null : item);
                }
                return result;
            }
        }



        // -------------- inherited properties ----------------------------------------------

        public new bool IsUnderdeck
        {
            get { return base.IsUnderdeck; }
            set { base.IsUnderdeck = value; }
        }
        public new byte Bay
        {
            get { return base.Bay; }
            set { base.Bay = value; }
        }
        public new byte HoldNr
        {
            get { return base.HoldNr; }
            set { base.HoldNr = value; }
        }
        public new byte Row
        {
            get { return base.Row; }
            set { base.Row = value; }
        }
        public new byte Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }
        public new byte Tier
        {
            get { return base.Tier; }
            set { base.Tier = value; }
        }

        #endregion

        #region Other properties
        // ------------------- Other properties --------------------------------------------        

        internal byte[] ALocation => new[] { Bay, Row, Tier };
        public List<string> AllDgClassesList => allDgClasses;

        /// <summary>
        /// Set: will add string dg class to allDgClasses.
        /// Get: will return string with allDgClasses listed and separated with a comma.
        /// </summary>
        public string AllDgClasses
        {
            get
            {
                string _temp = "";
                foreach (string x in allDgClasses)
                    _temp += _temp == "" ? x : ", " + x;
                return _temp;
            }
            set
            {
                if (!allDgClasses.Contains(value)) allDgClasses.Add(value);
            }
        }

        /// <summary>
        /// Set: reads array of string and records them to dgsubclass as well as to allDgclasses.
        /// </summary>
        public string[] DgSubclassArray
        {
            set
            {
                foreach (string x in value) if (x != "") DgSubclass = x;
            }
            get { return dgsubclass.ToArray(); }
        }

        #endregion

        #region Constructors
        //------------------------ Constructors --------------------------------------------------------------------------------------

        /// <summary>
        /// Public constructor of a blanc Dg
        /// </summary>
        public Dg()
        {
            dgclass = null;
            dgsubclass = new List<string>();
            allDgClasses = new List<string>();
            Unno = 0;
            flashPoint = 9999;
            packingGroup = 0;
            DgEMS = null;
            DgNetWeight = 0;
            IsMp = false;
            mpDetermined = false;
            special = new List<int>();
            stowageSW = new List<string>();
            segregationSG = new List<string>();
            segregationGroup = new List<int>();
            IsLq = false;
            Flammable = false;
            Liquid = false;
            EmitFlammableVapours = false;
            isStabilizedWordAddedToProperShippingName = false;
            isStabilizedWordAddedToProperShippingName = false;
        }

        /// <summary>
        /// Constructor to create Dg unit having only class and row for means of segregation between two classes
        /// </summary>
        /// <param name="_class"></param>
        public Dg(string _class)
        {
            dgclass = _class;
            AssignRowNumber();
        }
        #endregion


        //---------------------- Supporting methods ----------------------------------------------------------------------

        /// <summary>
        /// Method creates dg.conflict if not yet created
        /// </summary>
        public void AddConflict()
        {
            Conflict ??= new Conflict();

            IsConflicted = true;
        }

        /// <summary>
        /// Method adds conflict to Dg if 'add' is 'true'. Stowage or segregation conflict will be added as defined. Method combines other conflict related methods in Dg class to reduce nesting.
        /// </summary>
        /// <param name="add"></param>
        /// <param name="stoworsegr"></param>
        /// <param name="code"></param>
        /// <param name="b"></param>
        public void AddConflict(bool add, string stoworsegr, string code, Dg b = null)
        {
            if (!add) return;
            AddConflict();
            if (stoworsegr == "stowage") Conflict.AddStowConflict(code);
            else Conflict.AddSegrConflict(code, b);
        }

        /// <summary>
        /// Method assigns a stack to dg unit, taking into account 20 and 40'
        /// </summary>
        public void AssignStack()
        {
            Stack = new byte[3];
            Stack[0] = (byte)(Bay - 1);
            Stack[1] = Bay;
            Stack[2] = (byte)(Bay + 1);
        }

        /// <summary>
        /// Method to assign row number in segregation table.
        /// </summary>
        public void AssignRowNumber()
        {
            byte tableRow;

            var _index = dgclass.Length > 3 ? dgclass.Substring(0, 3) : dgclass;
            switch (_index)
            {
                case "1.1":
                    tableRow = 0;
                    break;
                case "1.2":
                    tableRow = 0;
                    break;
                case "1.3":
                    tableRow = 1;
                    break;
                case "1.4":
                    tableRow = 2;
                    break;
                case "1.5":
                    tableRow = 0;
                    break;
                case "1.6":
                    tableRow = 1;
                    break;
                case "2.1":
                    tableRow = 3;
                    break;
                case "2.2":
                    tableRow = 4;
                    break;
                case "2.3":
                    tableRow = 5;
                    break;
                case "3":
                    tableRow = 6;
                    break;
                case "4.1":
                    tableRow = 7;
                    break;
                case "4.2":
                    tableRow = 8;
                    break;
                case "4.3":
                    tableRow = 9;
                    break;
                case "5.1":
                    tableRow = 10;
                    break;
                case "5.2":
                    tableRow = 11;
                    break;
                case "6.1":
                    tableRow = 12;
                    break;
                case "6.2":
                    tableRow = 13;
                    break;
                case "7":
                    tableRow = 14;
                    break;
                case "8":
                    tableRow = 15;
                    break;
                case "9":
                    tableRow = 16;
                    break;
                default:
                    tableRow = 0;
                    break;
            }
            dgRowInTable = tableRow;
        }

        /// <summary>
        /// Method clears all the data of selected dg.
        /// If unno is given, then the unit parameter will be updated accordingly. By default unno will become '0'.
        /// </summary>
        /// <param name="unno"></param>
        public void Clear(int unno = 0)
        {
            Unno = unno;
            dgsubclass.Clear();
            allDgClasses.Clear();
            packingGroup = 0;
            DgEMS = null;
            mpDetermined = false;
            IsMp = false;
            special.Clear();
            stowageSW.Clear();
            segregationSG.Clear();
            segregationGroup.Clear();
            StowageCat = 'A';
            Name = null;
            Flammable = false;
            Liquid = false;
            EmitFlammableVapours = false;
            dgClassFromList = null;
            DgRowInDOC = 0;
            dgRowInTable = 0;
        }

        /// <summary>
        /// Method clears all alldgclasses properties
        /// </summary>
        public void ClearAllDgClasses()
        {
            dgclass = null;
            dgsubclass.Clear();
            allDgClasses.Clear();
        }

        /// <summary>
        /// Method clears all conflicts in dg unit and changes 'conflicted' status to false
        /// </summary>
        public void ClearConflicts()
        {
            if (!IsConflicted) return;
            IsConflicted = false;
            Conflict.SegrConflicts.Clear();
            Conflict.StowConflicts.Clear();
            Conflict.FailedSegregation = false;
            Conflict.FailedStowage = false;
        }

        /// <summary>
        /// Method to copy information from dg container to dg unit
        /// </summary>
        /// <param name="a"></param>
        public void CopyContainerInfo(Container a)
        {
            ContainerNumber = a.ContainerNumber;
            ContainerType = a.ContainerType;
            Location = a.Location;
            LocationBeforeRestow = a.LocationBeforeRestow;
            Bay = a.Bay;
            Row = a.Row;
            Tier = a.Tier;
            IsUnderdeck = a.IsUnderdeck;
            Size = a.Size;
            HoldNr = a.HoldNr;
            IsClosed = a.IsClosed;
            POD = a.POD;
            POL = a.POL;
            FinalDestination = a.FinalDestination;
            IsRf = a.IsRf;
            Carrier = a.Carrier;

            IsPositionLockedForChange = a.IsPositionLockedForChange;
            IsToBeKeptInPlan = a.IsToBeKeptInPlan;
            IsToImport = a.IsToImport;
            IsNotToImport = a.IsNotToImport;
            IsNewUnitInPlan = a.IsNewUnitInPlan;
            HasLocationChanged = a.HasLocationChanged;
            HasUpdated = a.HasUpdated;
            HasContainerTypeChanged = a.HasContainerTypeChanged;
            HasPodChanged = a.HasPodChanged;
        }

        /// <summary>
        /// Converts Dg back to plain Container
        /// </summary>
        /// <returns>Type of Container</returns>
        public Container ConvertToContainer()
        {
            return new Container()
            {
                ContainerNumber = this.ContainerNumber,
                ContainerType = this.ContainerType,
                Location = this.Location,
                Bay = this.Bay,
                Row = this.Row,
                Tier = this.Tier,
                IsUnderdeck = this.IsUnderdeck,
                Size = this.Size,
                HoldNr = this.HoldNr,
                IsClosed = this.IsClosed,
                POD = this.POD,
                POL = this.POL,
                FinalDestination = this.FinalDestination,
                Carrier = this.Carrier,
                IsRf = this.IsRf
        };
        }

        /// <summary>
        /// Defines compatibility group for segregation of class 1
        /// </summary>
        public void DefineCompatibilityGroup()
        {
            foreach (string s in allDgClasses)
                if (s.StartsWith("1"))
                    CompatibilityGroup = dgclass.Length > 3 ? char.ToUpper(dgclass[3]) : '0';
        }

        /// <summary>
        /// Method calls UpdateDgInfo method from HandlingDg on the instance
        /// </summary>
        /// <param name="dgDataBase"></param>
        public void UpdateDgInfo(XDocument dgDataBase)
        {
            HandlingDg.UpdateDgInfo(this, dgDataBase);
        }

        /// <summary>
        /// Chooses and copies relevant dg info from another Dg.
        /// </summary>
        /// <param name="dg">Dg from which info to be copied.</param>
        public void UpdateDgInfo(Dg dg)
        {
            this.CopyContainerInfo((Container)dg);

            DgClass = dg.DgClass;
            DgSubclassArray = dg.DgSubclassArray;
            FlashPointDouble = dg.FlashPointDouble;
            IsMp = dg.IsMp;
            DgEMS =!string.IsNullOrEmpty(dg.DgEMS) ?  dg.DgEMS : DgEMS;
            PackingGroupByte = dg.PackingGroupByte;
            SegregationGroupList.Clear();
            SegregationGroup = dg.SegregationGroup;
            IsLq = dg.IsLq;
            IsMax1L = dg.IsMax1L;
            IsWaste = dg.IsWaste;
            TechnicalName = dg.TechnicalName;
            OriginalNameFromCode = dg.OriginalNameFromCode;
            Name = dg.Name;
            DgNetWeight = dg.DgNetWeight;

            numberOfPackages = dg.numberOfPackages;
            typeOfPackages = dg.typeOfPackages;
            typeOfPackagesDescription = dg.typeOfPackagesDescription;
            dg.MergePackagesInfo();

            isStabilizedWordInProperShippingName = dg.isStabilizedWordInProperShippingName;
            isStabilizedWordAddedToProperShippingName = dg.isStabilizedWordAddedToProperShippingName;
            isSelfReactive = dg.isSelfReactive;
            stowageCatFromDgList = dg.stowageCatFromDgList;
            segregationSG = dg.segregationSG;
            special = dg.special;
            stowageSW = dg.stowageSW;
            stowageSWfromDgList = dg.stowageSWfromDgList;
            differentClass = dg.differentClass;
            mpDetermined = dg.mpDetermined;
            DgRowInDOC = dg.DgRowInDOC;
            dgRowInTable = dg.dgRowInTable;
            Stack = dg.Stack;
            CompatibilityGroup = dg.CompatibilityGroup;
            SegregatorClass = dg.SegregatorClass;

        }

        /// <summary>
        /// Calls method from Container class to update container type.
        /// </summary>
        public void UpdateContainerType()
        {
            Container.UpdateContainerType(this);
        }

        /// <summary>
        /// Merges withdrawn from ITFDGN information of packages into one string
        /// </summary>
        internal void MergePackagesInfo()
        {
            NumberAndTypeOfPackages = (numberOfPackages != 0 ? numberOfPackages + " " : "") 
                                      + typeOfPackagesDescription 
                                      + (numberOfPackages != 0 || typeOfPackagesDescription!= "" ? ", " : "")
                                      + typeOfPackages;
        }

        internal void SetOriginalName(string value)
        {
            OriginalNameFromCode = value;
        }

        /// <summary>
        /// Copies all non-importable information from source Dg.
        /// </summary>
        /// <param name="dgCopyFrom">Dg from which info shall be copied.</param>
        public void CopyNonImportableInfo(Dg dgCopyFrom)
        {
            Remarks = dgCopyFrom.Remarks;
            EmergencyContacts = dgCopyFrom.EmergencyContacts;
        }

        // ---------------- System override methods ------------------------------------------

        /// <summary>
        /// Supporting method for easier identification of units when debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ContainerNumber + " in " + Location + " class " + dgclass + " (unno " + Unno + ")";
        }

        public static explicit operator Container(Dg dg)
        {
            return dg.ConvertToContainer();
        }

        // -------------- IUpdatable ------------------------------------------------
        public bool IsToBeKeptInPlan { get; set; }
        public bool IsPositionLockedForChange { get; set; }
        public bool IsToImport { get; set; }
        public bool IsNotToImport { get; set; }
        public bool IsNewUnitInPlan { get; set; }
        public bool HasLocationChanged { get; set; }
        public bool HasUpdated { get; set; }
        public bool HasPodChanged { get; set; }
        public bool HasContainerTypeChanged { get; set; }
        public string LocationBeforeRestow { get; set; }


    }
}
