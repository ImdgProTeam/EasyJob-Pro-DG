using EasyJob_ProDG.Data.Info_data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
        private bool isStabilizedWordInProperShippingName => OriginalNameFromCode.Contains("STABILIZED");
        private bool isSelfReactive = false;
        private char stowageCatFromDgList;
        private List<string> segregationSG;
        private List<ushort> special;
        private List<string> stowageSW;
        private List<string> stowageSWfromDgList;
        public bool differentClass;
        public bool mpDetermined;

        //from other sources
        private readonly List<byte> segregationGroupsListBytes;
        public byte DgRowInDOC;
        public byte dgRowInTable;
        public byte[] Stack;
        public char CompatibilityGroup = '0';
        public string SegregatorClass;

        //User input fields
        private bool isStabilizedWordAddedToProperShippingName
            => Name.ToUpper().Contains("STABILIZED") && !isStabilizedWordInProperShippingName;
        private bool isWaste;

        //from IFTDGN
        internal ushort numberOfPackages;
        internal string typeOfPackages;
        internal string typeOfPackagesDescription;
        #endregion

        #region Public properties
        //----------------- public properties -----------------------------
        // ---------------- computed properties ---------------------------
        public int ID { get; }
        public ushort Unno { get; set; }
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

        /// <summary>
        /// Set: will add string dg class to dgsubclass and will update allDgClasses.
        /// Get: will return string with all subclasses listed and separated with comma.
        /// </summary>
        /// 
        public string DgSubclass
        {
            get
            {
                if (dgsubclass.Count <= 0) return "";
                StringBuilder sb = new StringBuilder();

                foreach (var x in dgsubclass)
                {
                    sb.Append(string.IsNullOrEmpty(sb.ToString()) ? x.ToString(CultureInfo.InvariantCulture) : ", " + x.ToString(CultureInfo.InvariantCulture));
                }
                return sb.ToString();
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
        public byte PackingGroupByte
        {
            get { return packingGroup; }
            set { packingGroup = value; }
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
        public double FlashPointDouble
        {
            set { flashPoint = value; }
            get { return flashPoint; }
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
                foreach (int itemNr in segregationGroupsListBytes)
                {
                    string group = IMDGCode.SegregationGroups[itemNr];
                    result += result == null ? group : ", " + group;
                }
                return result;
            }
            set
            {
                byte segregationGroupIndex;

                //By full group title as mentioned in IMDG code
                if (IMDGCode.SegregationGroups.Contains(value) &&
                   !segregationGroupsListBytes.Contains((byte)Array.IndexOf(IMDGCode.SegregationGroups, value)))
                {
                    segregationGroupsListBytes.Add((byte)Array.IndexOf(IMDGCode.SegregationGroups, value));
                }
                //By code only
                else if (IMDGCode.SegregationGroupsCodes.Contains(value))
                {
                    segregationGroupIndex = (byte)Array.IndexOf(IMDGCode.SegregationGroupsCodes, value);
                    if (!segregationGroupsListBytes.Contains(segregationGroupIndex))
                        segregationGroupsListBytes.Add(segregationGroupIndex);
                }
                //Parse multiple values
                else if (value.Contains(","))
                {
                    foreach (var group in value.Replace(" ", "").Split(','))
                    {
                        SegregationGroup = group;
                    }
                }
                //By index in dictionary
                else if (byte.TryParse(value, out segregationGroupIndex))
                {
                    if (!segregationGroupsListBytes.Contains(segregationGroupIndex))
                        segregationGroupsListBytes.Add(segregationGroupIndex);
                }


            }
        }

        public byte SegregationGroupByte
        {
            set
            {
                if (segregationGroupsListBytes.Contains(value)) return;
                segregationGroupsListBytes.Add(value);
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

        public bool IsSelfReactive
        {
            get { return isSelfReactive; }
            set { isSelfReactive = value; }
        }


        // ---------------- User input properties ------------------------------
        public bool IsStabilized
        {
            get
            {
                return isStabilizedWordInProperShippingName || isStabilizedWordAddedToProperShippingName;
            }
            set
            {
                //Applicable only if the word 'STABILIZED' is not alrady included as a part of Proper Shipping Name into IMDG code
                if (isStabilizedWordInProperShippingName) return;

                if (value)
                {
                    if (!Name.ToLower().Replace(" ", "").Contains("stabilized"))
                        Name += ", STABILIZED";
                    StowageCat = 'D';
                    stowageSW.Add("SW1");

                }
                if (!value)
                {
                    Name = Name.Replace(", STABILIZED", "");
                    StowageCat = stowageCatFromDgList;
                    stowageSW = stowageSWfromDgList;
                }
            }
        }
        public bool IsWaste
        {
            get => isWaste;
            set
            {
                if (value)
                {
                    isWaste = true;
                    if (!Name.ToLower().Replace(" ", "").Contains("waste")) Name += ", WASTE";
                    if (IMDGCode.SW22RelatedUnnos.Contains(Unno))
                    {
                        StowageCat = 'C';
                    }
                }
                else
                {
                    isWaste = false;
                    Name = Name.Replace(", WASTE", "");
                    StowageCat = stowageCatFromDgList;
                }
            }
        }
        public bool IsMax1L { get; set; }
        public bool IsAsCoolantOrConditioner
        {
            get => !string.IsNullOrEmpty(Name) && (Name.ToUpper().Contains("COOLANT") || Name.ToUpper().Contains("CONDITIONER"));
        }



        // ---------------- auto-properties ------------------------------

        public bool EmitFlammableVapours { get; set; }
        public bool Flammable { get; set; }
        public bool IsConflicted => !this.Conflicts?.IsEmpty ?? false;
        public bool IsLq { get; set; }
        public bool Liquid { get; set; }
        public bool IsMp { get; set; }
        public char StowageCat { get; set; }
        public Conflicts Conflicts { get; set; }
        public decimal DgNetWeight { get; set; }
        public string EmergencyContacts { get; set; }
        public string Remarks { get; set; }
        public string NumberAndTypeOfPackages { get; set; }
        public string TechnicalName { get; set; }
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

        /// <summary>
        /// Returns number of dg subclasses
        /// </summary>
        public byte DgsubclassCount => (byte)dgsubclass.Count;
        public List<byte> SegregationGroupList => segregationGroupsListBytes;


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
        internal List<string> AllDgClassesList => allDgClasses;

        /// <summary>
        /// Set: will add string dg class to allDgClasses.
        /// Get: will return string with allDgClasses listed and separated with a comma.
        /// </summary>
        internal string AllDgClasses
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
            special = new List<ushort>();
            stowageSW = new List<string>();
            segregationSG = new List<string>();
            segregationGroupsListBytes = new List<byte>();
            IsLq = false;
            Flammable = false;
            Liquid = false;
            EmitFlammableVapours = false;

            ID = RandomizeID.GetNewID();
        }

        /// <summary>
        /// Constructor to create Dg unit having only class and row for means of segregation between two classes
        /// </summary>
        /// <param name="_class"></param>
        public Dg(string dgClass)
        {
            dgclass = dgClass;
            AssignSegregationTableRowNumber();
        }
        #endregion


        //---------------------- Supporting methods ----------------------------------------------------------------------

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
        /// Will define row number in IMDG Code segregation table and assign it to dgRowInTable
        /// </summary>
        public void AssignSegregationTableRowNumber()
        {
            dgRowInTable = IMDGCode.AssignSegregationTableRowNumber(dgclass);
        }

        /// <summary>
        /// Method clears all the data of selected dg.
        /// If unno is given, then the unit parameter will be updated accordingly. By default unno will become '0'.
        /// </summary>
        /// <param name="unno"></param>
        public void Clear(ushort unno = 0)
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
            segregationGroupsListBytes.Clear();
            StowageCat = '0';
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
            DgEMS = !string.IsNullOrEmpty(dg.DgEMS) ? dg.DgEMS : DgEMS;
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

            //isStabilizedWordAddedToProperShippingName = dg.isStabilizedWordAddedToProperShippingName;
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
                                      + ((numberOfPackages != 0 || typeOfPackagesDescription != "") && typeOfPackages != "" ? ", " : "")
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
        internal void CopyNonImportableInfo(Dg dgCopyFrom)
        {
            Remarks = dgCopyFrom.Remarks;
            EmergencyContacts = dgCopyFrom.EmergencyContacts;
        }


        #region System override methods
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
        #endregion


        #region IUpdatable
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
        #endregion


    }
}
