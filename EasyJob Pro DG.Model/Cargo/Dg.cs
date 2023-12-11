using EasyJob_ProDG.Data.Info_data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public partial class Dg : ContainerAbstract, IO.IUpdatable
    {
        #region Fields Declarations
        // read from edi
        protected byte packingGroup;
        private decimal flashPoint;

        //assign from DG List
        private bool isStabilizedWordInOriginalProperShippingName => OriginalNameFromCode?.Contains("STABILIZED") ?? false;
        private bool isSelfReactive = false;
        private char stowageCatFromDgList;
        protected List<string> segregationSG;
        protected List<ushort> special;
        protected List<string> stowageSW;
        private List<string> stowageSWfromDgList;
        private string dgClassFromIMDGCode;

        /// <summary>
        /// Indicates that the primary dg class of the unit is different from IMDG code record for the same UNNo.
        /// Except: for UNNOs allowing various classes (1950, 2037).
        /// </summary>
        public bool differentClass;

        /// <summary>
        /// Indicates if MP property is determined from .edi or any other source
        /// </summary>
        public bool mpDetermined;

        //from other sources
        private readonly List<byte> segregationGroupsListBytes;
        public byte DgRowInDOC;
        public byte DgRowInSegregationTable;
        public char CompatibilityGroup = '0';
        public string SegregatorClass;

        //User input fields
        private bool isStabilizedWordAddedToProperShippingName
            => !string.IsNullOrEmpty(Name) && Name.ToUpper().Contains("STABILIZED") && !isStabilizedWordInOriginalProperShippingName;
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

        #region Class and Sub class

        protected string dgclass;
        protected string[] dgsubclass;
        private readonly List<string> allDgClasses;

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
                allDgClasses[0] = value;
            }
        }

        /// <summary>
        /// Set: will add string dg class to dgsubclass and will update allDgClasses.
        /// Get: will return string with all subclasses listed and separated with space.
        /// </summary>
        /// 
        public string DgSubClass
        {
            get => !string.IsNullOrWhiteSpace(dgsubclass[1]) ? string.Join(" ", dgsubclass) : dgsubclass[0];
            set
            {
                var array = value.Trim().Split(' ');
                dgsubclass[0] = array[0] ?? null;
                dgsubclass[1] = array.Length > 1 ? array[1] : null;
                UpdateAllDgClasses();
            }
        }

        /// <summary>
        /// Summary list of DgClass and all DgSubclasses.
        /// </summary>
        internal List<string> AllDgClasses => allDgClasses;

        /// <summary>
        /// Set: reads array of string and records them to dgsubclass as well as to allDgclasses.
        /// </summary>
        public string[] DgSubClassArray
        {
            get => dgsubclass;
            set
            {
                dgsubclass = value;
                UpdateAllDgClasses();
            }
        }

        /// <summary>
        /// Count of meaningful dg subclasses.
        /// </summary>
        public byte DgSubclassCount => (byte)dgsubclass.Count(x => !string.IsNullOrWhiteSpace(x));

        /// <summary>
        /// Updates <see cref="allDgClasses"/> with correspondent values from <see cref="dgclass"/> and <see cref="dgsubclass"/>
        /// </summary>
        private void UpdateAllDgClasses()
        {
            if (allDgClasses.Count > 1)
            {
                allDgClasses.Clear();
                allDgClasses.Add(dgclass);
            }
            foreach (var item in dgsubclass)
            {
                if (!string.IsNullOrWhiteSpace(item))
                    allDgClasses.Add(item);
            }
        }

        #endregion


        /// <summary>
        /// Set: will parse string with packing group and record it to <see cref="packingGroup"/>.
        /// Get: will return string containing better view of a packing group.
        /// </summary>
        public string PackingGroup
        {
            get
            {
                return packingGroup switch
                {
                    1 => "I",
                    2 => "II",
                    3 => "III",
                    _ => string.Empty
                };
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    packingGroup = 0;
                    return;
                }
                if (byte.TryParse(value, out packingGroup))
                {
                    if (packingGroup > 3)
                        packingGroup = 3;
                }
                else switch (value.ToUpper())
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
        public byte PackingGroupAsByte
        {
            get { return packingGroup; }
            set { packingGroup = value; }
        }
        public string FlashPoint
        {
            get
            {
                return flashPoint == 9999 ? "" : flashPoint.ToString(CultureInfo.InvariantCulture);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) flashPoint = 9999;
                else decimal.TryParse(value, out flashPoint);
            }
        }
        public decimal FlashPointAsDecimal
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
                byte segregationGroupIndex = 0;

                //By full group title as mentioned in IMDG code
                if (IMDGCode.SegregationGroups.Contains(value))
                {
                    segregationGroupIndex = (byte)Array.IndexOf(IMDGCode.SegregationGroups, value);
                }
                //By code only
                else if (IMDGCode.SegregationGroupsCodes.Contains(value))
                {
                    segregationGroupIndex = (byte)Array.IndexOf(IMDGCode.SegregationGroupsCodes, value);
                }
                //By index in dictionary
                else if (byte.TryParse(value, out segregationGroupIndex))
                {

                }
                //Parse multiple values
                else if (value.Contains(","))
                {
                    foreach (var group in value.Replace(" ", "").Split(','))
                    {
                        SegregationGroup = group;
                    }
                    return;
                }
                // Else - handle obsolete groups
                else segregationGroupIndex = HandleObsoleteGroups(value);

                if (segregationGroupIndex > 0 && !segregationGroupsListBytes.Contains(segregationGroupIndex))
                    segregationGroupsListBytes.Add(segregationGroupIndex);
            }
        }

        public byte SegregationGroupByte
        {
            set
            {
                if (segregationGroupsListBytes.Contains(value)) return;
                if (value > IMDGCode.SegregationGroupsNumber)
                    value = HandleObsoleteGroups(value);
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

        /// <summary>
        /// Contains stowage category as stated in IMDG code.
        /// </summary>
        public char StowageCategoryFromIMDGCode
        {
            get { return stowageCatFromDgList; }
            set { stowageCatFromDgList = value; }
        }



        // ---------------- User input properties ------------------------------
        public bool IsStabilized
        {
            get
            {
                return isStabilizedWordInOriginalProperShippingName || isStabilizedWordAddedToProperShippingName;
            }
            set
            {
                //Applicable only if the word 'STABILIZED' is not alrady included as a part of Proper Shipping Name into IMDG code
                if (isStabilizedWordInOriginalProperShippingName) return;

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


        //--------------- Readonly properties -----------------------------------------------

        public List<string> SegregationSGList => segregationSG;
        public List<string> StowageSWList => stowageSW;
        public SegregatorException SegregatorException { get; set; }
        public string Properties { get; protected set; }
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
        public List<byte> SegregationGroupList => segregationGroupsListBytes;


        #endregion

        #region Other properties
        // ------------------- Other properties --------------------------------------------        



        #endregion

        #region Constructors
        //------------------------ Constructors --------------------------------------------------------------------------------------

        /// <summary>
        /// Public constructor of a blanc Dg
        /// </summary>
        public Dg()
        {
            dgclass = null;
            dgsubclass = new string[2];
            allDgClasses = new() { null };
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
        /// Will define row number in IMDG Code segregation table and assign it to DgRowInSegregationTable
        /// </summary>
        public void AssignSegregationTableRowNumber()
        {
            DgRowInSegregationTable = IMDGCode.AssignSegregationTableRowNumber(dgclass);
        }

        /// <summary>
        /// Method clears or sets to default all the dg related properties and fields of the selected dg.
        /// If unno is given, then the unit parameter will be updated accordingly. By default unno will become '0'.
        /// </summary>
        /// <param name="unno"></param>
        public void Clear(ushort unno = 0)
        {
            Unno = unno;
            dgsubclass[0] = dgsubclass[1] = null;
            allDgClasses.Clear();
            allDgClasses.Add(null);
            packingGroup = 0;
            flashPoint = 9999;
            DgEMS = null;
            mpDetermined = false;
            IsMp = false;
            special.Clear();
            stowageSW.Clear();
            segregationSG.Clear();
            segregationGroupsListBytes.Clear();
            StowageCat = '0';
            Name = null;
            OriginalNameFromCode = null;
            Flammable = false;
            Liquid = false;
            EmitFlammableVapours = false;
            dgClassFromIMDGCode = null;
            DgRowInDOC = 0;
            DgRowInSegregationTable = 0;
        }

        /// <summary>
        /// Method clears all alldgclasses properties
        /// </summary>
        public void ClearAllDgClasses()
        {
            dgclass = null;
            dgsubclass[0] = dgsubclass[1] = null;
            allDgClasses[0] = allDgClasses[1] = allDgClasses[2] = null;
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

        ///// <summary>
        ///// Method calls UpdateDgInfo method from HandlingDg on the instance
        ///// </summary>
        ///// <param name="dgDataBase"></param>
        //public void UpdateDgInfo()
        //{
        //    HandleDg.UpdateDgInfo(this);
        //}

        /// <summary>
        /// Chooses and copies relevant dg info from another Dg.
        /// </summary>
        /// <param name="dg">Dg from which info to be copied.</param>
        public void UpdateDgInfo(Dg dg)
        {
            this.CopyContainerInfo((Container)dg);

            dgclass = dg.dgclass;
            dgsubclass = dg.dgsubclass;
            FlashPointAsDecimal = dg.FlashPointAsDecimal;
            IsMp = dg.IsMp;
            DgEMS = !string.IsNullOrEmpty(dg.DgEMS) ? dg.DgEMS : DgEMS;
            PackingGroupAsByte = dg.PackingGroupAsByte;
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
            DgRowInSegregationTable = dg.DgRowInSegregationTable;
            CompatibilityGroup = dg.CompatibilityGroup;
            SegregatorClass = dg.SegregatorClass;

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

        /// <summary>
        /// Sets information - private and read only fields - from the given [imdg] <see cref="Dg"/> 
        /// </summary>
        /// <param name="dgFromIMDGCode"></param>
        internal void SetIMDGCodeValues(Dg dgFromIMDGCode, bool pkgChanged = false)
        {
            if (pkgChanged || StowageCat == '0' || StowageCat == '\0')
                StowageCat = dgFromIMDGCode.StowageCat;

            stowageCatFromDgList = dgFromIMDGCode.StowageCat;
            stowageSW = dgFromIMDGCode.stowageSW;
            stowageSWfromDgList = dgFromIMDGCode.stowageSW.ToList();
            segregationSG = dgFromIMDGCode.segregationSG;
            special = dgFromIMDGCode.special;
            Properties = dgFromIMDGCode.Properties;
            OriginalNameFromCode = dgFromIMDGCode.Name;
            dgClassFromIMDGCode = dgFromIMDGCode.dgclass;
        }

        /// <summary>
        /// Method ensures compatibility with older conditions saved
        /// </summary>
        /// <param name="groupNr"></param>
        /// <returns></returns>
        private byte HandleObsoleteGroups(object value)
        {
            byte result;
            if (value is byte || value is int)
            {
                //Strong acids removed in 41-22
                result = (byte)((int)value == 19 ? 1 : 0);
            }
            else
            {
                //Strong acids removed in 41-22
                result = (byte)(string.Equals(value.ToString(), "SGG1a") ? 1 : 0);
            }
            return result;
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
            return dg?.ConvertToContainer();
        }
        #endregion

    }
}
