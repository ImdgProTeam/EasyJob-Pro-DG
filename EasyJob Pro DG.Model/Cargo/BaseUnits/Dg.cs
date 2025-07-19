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
        public byte DgRowInDOC;
        public char CompatibilityGroup = '0';
        public string SegregatorClass;

        //from IFTDGN
        internal ushort numberOfPackages;
        internal string typeOfPackages;
        internal string typeOfPackagesDescription;
        #endregion

        #region Public properties
        //----------------- public properties -----------------------------

        public int ID { get; }
        public ushort Unno { get; set; }


        #region Class and Sub class

        protected string dgclass;
        protected string[] dgsubclass;
        private readonly List<string> allDgClasses;
        private string dgClassFromIMDGCode;

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
        public List<string> AllDgClasses => allDgClasses;

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


        #region Packing group and flash point

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
        protected byte packingGroup;

        public string FlashPoint
        {
            get
            {
                return FlashPointNotDefined ? "" : flashPoint.ToString(CultureInfo.InvariantCulture);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) flashPoint = ProgramDefaultValues.DefaultFlashPointValue;
                else decimal.TryParse(value, out flashPoint);
            }
        }
        public decimal FlashPointAsDecimal
        {
            set { flashPoint = value; }
            get { return flashPoint; }
        }
        private decimal flashPoint;

        /// <summary>
        /// The value is true if FlashPoint value has not been set. 
        /// </summary>
        public bool FlashPointNotDefined => 
            Math.Abs(FlashPointAsDecimal - ProgramDefaultValues.DefaultFlashPointValue) < 1;

        #endregion


        #region Segregation groups

        /// <summary>
        /// Set: will check if segregation group exists in the list and will assign it to dg unit.
        /// Get: returns string containing all segregation groups separated with comma.
        /// </summary>
        public string SegregationGroup
        {
            get
            {
                return string.Join(", ", segregationGroupsListBytes.Select(g => IMDGCode.SegregationGroups[g]));
            }
            set
            {
                byte segregationGroupIndex;

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
                        SegregationGroup = group.Trim();
                    }
                    return;
                }
                // Else - handle obsolete groups
                else segregationGroupIndex = IMDGCode.HandleObsoleteGroups(value);

                if (segregationGroupIndex > 0 && !segregationGroupsListBytes.Contains(segregationGroupIndex))
                    segregationGroupsListBytes.Add(segregationGroupIndex);
            }
        }

        /// <summary>
        /// Used to add a <see cref="SegregationGroup"/> given as a byte value (index).
        /// </summary>
        public byte SegregationGroupByte
        {
            set
            {
                if (segregationGroupsListBytes.Contains(value)) return;
                if (value > IMDGCode.SegregationGroupsNumber)
                    value = IMDGCode.HandleObsoleteGroups(value);
                segregationGroupsListBytes.Add(value);
            }
        }

        /// <summary>
        /// Contains indexes of segregation groups according to <see cref="IMDGCode.SegregationGroups"/>
        /// </summary>
        public List<byte> SegregationGroupList => segregationGroupsListBytes;
        private readonly List<byte> segregationGroupsListBytes;

        /// <summary>
        /// Returns string with all segregation group codes (e.g. "SSG1") listed.
        /// </summary>
        public string SegregationGroupCodes
        {
            get => string.Join(", ", SegregationGroupList.Select(s => IMDGCode.SegregationGroupsCodes[s]));
        }

        #endregion


        #region Stowage category

        public char StowageCat { get; set; }

        /// <summary>
        /// Contains stowage category as stated in IMDG code.
        /// </summary>
        public char StowageCategoryFromIMDGCode
        {
            get { return stowageCatFromIMDGCode; }
            set { stowageCatFromIMDGCode = value; }
        }
        private char stowageCatFromIMDGCode;

        #endregion


        #region SW and SG

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
        public List<string> SegregationSGList => segregationSG;
        protected List<string> segregationSG;

        public string? StowageSW
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
        public List<string> StowageSWList => stowageSW;
        protected List<string> stowageSW;
        private List<string> stowageSWfromDgList;

        #endregion


        #region Properties and special

        public string Properties { get; protected set; }
        public string Special
        {
            get
            {
                return string.Join(", ", special);
            }
        }
        protected List<ushort> special;

        #endregion


        #region User input properties

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
                    if(!string.IsNullOrWhiteSpace(Name))
                        Name = Name.Replace(", STABILIZED", "");
                    StowageCat = stowageCatFromIMDGCode;
                    stowageSW = stowageSWfromDgList;
                }
            }
        }
        private bool isStabilizedWordInOriginalProperShippingName => OriginalNameFromCode?.Contains("STABILIZED") ?? false;
        private bool isStabilizedWordAddedToProperShippingName => !string.IsNullOrEmpty(Name)
            && Name.ToUpper().Contains("STABILIZED") && !isStabilizedWordInOriginalProperShippingName;

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
                    if(!string.IsNullOrWhiteSpace(Name))
                        Name = Name.Replace(", WASTE", "");
                    StowageCat = stowageCatFromIMDGCode;
                }
            }
        }
        private bool isWaste;

        public bool IsMax1L { get; set; }
        public bool IsAsCoolantOrConditioner
        {
            get => !string.IsNullOrEmpty(Name) && (Name.ToUpper().Contains("COOLANT") || Name.ToUpper().Contains("CONDITIONER"));
        }

        public bool IsSelfReactive
        {
            get { return isSelfReactive; }
            set { isSelfReactive = value; }
        }
        private bool isSelfReactive = false;

        public bool IsLiquid { get => isLiquid; set => isLiquid = value; }
        protected bool isLiquid;
        protected bool isFlammable;
        private bool isEmitFlammableVapours;

        #endregion

        // ---------------- auto-properties ------------------------------

        public bool IsEmitFlammableVapours { get => isEmitFlammableVapours; set => isEmitFlammableVapours = value; }
        public bool IsFlammable { get => isFlammable; set => isFlammable = value; }
        public bool IsConflicted => !this.Conflicts?.IsEmpty ?? false;
        public bool IsLq { get; set; }
        public bool IsMp { get; set; }
        public Conflicts Conflicts { get; set; }
        public decimal DgNetWeight { get; set; }
        public string EmergencyContacts { get; set; }
        public string Remarks { get; set; }
        public string NumberAndTypeOfPackages { get; set; }
        public string TechnicalName { get; set; }
        public string DgEMS { get; set; }

        /// <summary>
        /// Proper shipping name
        /// </summary>
        public string Name { get; set; }
        public string OriginalNameFromCode { get; private set; }
        public bool IsNameChanged { get; set; }
        public bool IsTechnicalNameIncluded { get; set; }
        public string Surrounded { get; set; }
        public SegregatorException SegregatorException { get; set; }


        #endregion


        #region Public and Internal Methods

        //---------------------- Supporting methods ----------------------------------------------------------------------

        /// <summary>
        /// Method clears or sets to default all the dg related properties and fields of the selected dg.
        /// If unno is given, then the unit parameter will be updated accordingly. By default unno will become '0'.
        /// </summary>
        /// <param name="unno">Target UNNo can be set on Clear, otherwise default is 0.</param>
        public void Clear(ushort unno = 0)
        {
            Unno = unno;
            dgclass = null;
            dgsubclass[0] = dgsubclass[1] = null;
            allDgClasses.Clear();
            allDgClasses.Add(null);
            packingGroup = 0;
            flashPoint = ProgramDefaultValues.DefaultFlashPointValue;
            DgEMS = null;
            mpDetermined = false;
            IsMp = false;
            special.Clear();
            stowageSW.Clear();
            stowageSWfromDgList?.Clear();
            segregationSG.Clear();
            segregationGroupsListBytes.Clear();
            StowageCat = '0';
            stowageCatFromIMDGCode = '0';
            Properties = null;
            Name = null;
            OriginalNameFromCode = null;
            IsFlammable = false;
            IsLiquid = false;
            IsEmitFlammableVapours = false;
            dgClassFromIMDGCode = null;
            DgRowInDOC = 0;
            IsLq = false;
            IsWaste = false;
            IsMax1L = false;
            IsStabilized = false;
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

        /// <summary>
        /// Sets information - private and read only fields - from the given [imdg] <see cref="Dg"/> 
        /// </summary>
        /// <param name="dgFromIMDGCode"></param>
        internal void SetIMDGCodeValues(Dg dgFromIMDGCode, bool pkgChanged = false)
        {
            if (pkgChanged || StowageCat == '0' || StowageCat == '\0')
                StowageCat = dgFromIMDGCode.StowageCat;

            stowageCatFromIMDGCode = dgFromIMDGCode.StowageCat;
            stowageSW = dgFromIMDGCode.stowageSW;
            stowageSWfromDgList = dgFromIMDGCode.stowageSW.ToList();
            segregationSG = dgFromIMDGCode.segregationSG;
            special = dgFromIMDGCode.special;
            Properties = dgFromIMDGCode.Properties;
            OriginalNameFromCode = dgFromIMDGCode.Name;
            dgClassFromIMDGCode = dgFromIMDGCode.dgclass;
            isLiquid = dgFromIMDGCode.isLiquid;
            isFlammable = dgFromIMDGCode.isFlammable;
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
            dgsubclass = new string[2];
            allDgClasses = new() { null };
            Unno = 0;
            flashPoint = ProgramDefaultValues.DefaultFlashPointValue;
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
            IsFlammable = false;
            IsLiquid = false;
            IsEmitFlammableVapours = false;

            ID = RandomizeID.GetNewID();
        }

        /// <summary>
        /// Constructor to create Dg unit having only class and row for means of segregation between two classes
        /// </summary>
        /// <param name="_class"></param>
        public Dg(string dgClass)
        {
            dgclass = dgClass;
        }

        #endregion

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
