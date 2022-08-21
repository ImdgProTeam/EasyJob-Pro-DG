using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.View.Sort;
using System;

namespace EasyJob_ProDG.UI.Wrapper
{
    public partial class DgWrapper : ModelWrapper<Dg>, ILocationOnBoard, IContainer, IUpdatable
    {
        //Private fields
        private SortableLocation _locationSortable;


        // --------------- Public properties ----------------------------

        public bool IsInList { get; set; }


        #region LocationOnBoard properties
        //--------------- Properties related to container location ------------------

        public bool IsUnderdeck
        {
            get { return GetValue<bool>(); }
            set
            {
            }
        }
        public byte Bay
        {
            get { return GetValue<byte>(); }
            set
            { }
        }
        public byte Row
        {
            get { return GetValue<byte>(); }
            set { }
        }
        public byte Size
        {
            get => GetValue<byte>();
            set { }
        }
        public byte Tier
        {
            get { return GetValue<byte>(); }
            set { }
        }
        public byte HoldNr
        {
            get { return GetValue<byte>(); }
            set
            {
            }
        }

        /// <summary>
        /// Container position on board (slot address)
        /// </summary>
        public string Location
        {
            get => GetValue<string>();
            set
            {
                var oldValue = Location;
                if (oldValue == value) return;

                SetValue(value);
                _locationSortable = null;
                if (!IsInList) return;

                if (CurrentProgramData.OwnShip != null)
                    Model.HoldNr = CurrentProgramData.OwnShip.DefineCargoHoldNumber(Bay);
                SetToAllContainersInPlan(GetValue<string>(), oldValue);
                UpdateLocationPresentation();
            }
        }

        /// <summary>
        /// Property used to sort DgWrappers in chosen order
        /// </summary>
        public SortableLocation LocationSortable => _locationSortable ??= new SortableLocation(this);
        #endregion


        #region IUpdatable
        //--------------- Properties related to IUpdatable -------------------------- 

        public bool IsPositionLockedForChange
        {
            get => GetValue<bool>();
            set
            {
                if (!SetValue(value)) return;
                SetToAllContainersInPlan(value);
            }
        }
        public bool IsToBeKeptInPlan
        {
            get => GetValue<bool>();
            set
            {
                if (!SetValue(value)) return;
                SetToAllContainersInPlan(value);
            }
        }
        public bool IsNotToImport
        {
            get => GetValue<bool>();
            set
            {
                if (!SetValue(value)) return;
                if (value) IsToImport = false;
                SetToAllContainersInPlan(value);
            }
        }
        public bool IsToImport
        {
            get => GetValue<bool>();
            set
            {
                if (!SetValue(value)) return;
                if(value) IsNotToImport = false;
                SetToAllContainersInPlan(value);
            }
        }

        //properties with empty setters
        public bool IsNewUnitInPlan
        {
            get => GetValue<bool>();
            set { }
        }
        public bool HasLocationChanged
        {
            get => GetValue<bool>();
            set { }
        }
        public bool HasUpdated
        {
            get => GetValue<bool>();
            set { }
        }
        public bool HasPodChanged
        {
            get => GetValue<bool>();
            set { }
        }
        public bool HasContainerTypeChanged
        {
            get => GetValue<bool>();
            set { }
        }

        public string LocationBeforeRestow
        {
            get => GetValue<string>(); set { }
        }
        #endregion


        #region IContainer
        //--------------- Properties related to IContainer --------------------------- 

        /// <summary>
        /// Container number
        /// </summary>
        public string ContainerNumber
        {
            get => GetValue<string>();
            set
            {
                var oldValue = ContainerNumber;
                if (oldValue == value) return;
                SetValue(value.ToUpper());
                SetToAllContainersInPlan(value.ToUpper(), oldValue);
            }
        }

        /// <summary>
        /// Port of loading
        /// </summary>
        public string POL
        {
            get => GetValue<string>();
            set
            {
                if(!SetValue(value)) return;
                if (IsInList)
                {
                    SetToAllContainersInPlan(value);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Port of discharging
        /// </summary>
        public string POD
        {
            get => GetValue<string>();
            set
            {
                if(!SetValue(value)) return;
                if (IsInList)
                {
                    SetToAllContainersInPlan(value);
                }
            }
        }

        /// <summary>
        /// Port of final destination
        /// </summary>
        public string FinalDestination
        {
            get => GetValue<string>();
            set
            {
                if(!SetValue(value)) return;
                if (IsInList)
                {
                    SetToAllContainersInPlan(value);
                }
            }
        }

        /// <summary>
        /// Container type code
        /// </summary>
        public string ContainerType
        {
            get => GetValue<string>();
            set
            {
                if (!SetValue(value.ToUpper())) return;
                if (IsInList)
                {
                    Model.UpdateContainerType();
                    SetToAllContainersInPlan(value.ToUpper());

                    OnPropertyChanged($"IsClosed");
                    OnPropertyChanged($"IsOpen");
                }
            }
        }

        /// <summary>
        /// True if unit is a Closed Freight Container
        /// </summary>
        public bool IsClosed
        {
            get => GetValue<bool>();
            set
            {
                if (!SetValue(value)) return;
                SetToAllContainersInPlan(value);

                OnPropertyChanged($"IsOpen");
            }
        }

        /// <summary>
        /// True if container is of open type. Opposite to 'IsClosed'
        /// </summary>
        public bool IsOpen
        {
            get => !IsClosed;
            set => IsClosed = !value;
        }

        /// <summary>
        /// True if the container is a live reefer
        /// </summary>
        public bool IsRf
        {
            get => GetValue<bool>();
            set
            {
                if (!SetValue(value)) return;
                SetToAllContainersInPlan(value);

            }
        }

        /// <summary>
        /// Container operator
        /// </summary>
        public string Carrier
        {
            get => GetValue<string>();
            set
            {
                if (!SetValue(value)) return;
                if (IsInList)
                    SetToAllContainersInPlan(value);
            }
        }

        public bool ContainerTypeRecognized { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion


        #region Dg properties with complex functionality
        //--------------- Properties related to Dangerous Cargo ---------------------

        /// <summary>
        /// UN no of a dangerous good
        /// </summary>
        public ushort Unno
        {
            get => GetValue<ushort>();
            set
            {
                if (Unno == value) return;
                if (!DataHelper.CheckForExistingUnno(value)) return;

                SetValue(value);
                if (value != 1950) IsMax1L = false;
                UpdateDgInfoAndUploadChanges();
                UpdateConflictList();
            }
        }

        /// <summary>
        /// Set: adds string dg class to dgclass and will update allDgClasses.
        /// Get: returns string with dgclass.
        /// </summary>
        public string DgClass
        {
            get => GetValue<string>();
            set
            {
                string[] tempdgsubclasses = Model.DgSubclassArray;
                Model.ClearAllDgClasses();
                SetValue(value.Replace(" ",""));
                foreach (var dgsubclass in tempdgsubclasses)
                {
                    Model.DgSubclass = dgsubclass;
                }
                if (IsInList)
                {
                    OnPropertyChanged($"AllDgClasses");
                    UpdateConflictList();
                }
            }
        }

        /// <summary>
        /// Set: will add string dg class to dgsubclass and will update allDgClasses.
        /// Get: will return string with all subclasses listed and separated with comma.
        /// </summary>
        public string DgSubclass
        {
            get => GetValue<string>();
            set
            {
                string tempDgClass = DgClass;
                Model.ClearAllDgClasses();
                Model.DgClass = tempDgClass;

                string[] array = ParseMultipleClasses(value);
                foreach (var val in array)
                {
                    if (!string.IsNullOrEmpty(val))
                        SetValue(val);
                }

                if (IsInList)
                {
                    OnPropertyChanged($"AllDgClasses");
                    UpdateConflictList();
                }
            }
        }

        /// <summary>
        /// Set: will parse string with packing group and record it to dgpg.
        /// Get: will return string containing better view of a packing group.
        /// </summary>
        public string PackingGroup
        {
            get { return GetValue<string>(); }
            set
            {
                if(!SetValue(value)) return;
                if (IsInList)
                {
                    OnUpdatePackingGroup();
                    UpdateConflictList();
                }
            }
        }

        public decimal DgNetWeight
        {
            get { return GetValue<decimal>(); }
            set
            {
                if(!SetValue(value)) return;
                OnNetWeightChanged();
            }
        }

        public bool IsLq
        {
            get => GetValue<bool>();
            set
            {
                if(!SetValue(value)) return;
                UpdateConflictList();
            }
        }

        public bool IsMp
        {
            get { return GetValue<bool>(); }
            set
            {
                if(!SetValue(value)) return;
                UpdateDgStowageConflicts();
                OnNetWeightChanged();
            }
        }

        public bool IsMax1L
        {
            get => GetValue<bool>();
            set
            {
                if (IsMax1L == value) return;
                if (value && Unno != 1950) return;

                if(!SetValue(value)) return;
                if (value)
                {
                    if (!Name.ToLower().Replace(" ", "").Replace(".","").Contains("max1l")) Name += ", Max 1L";
                    if (IsWaste) IsWaste = false;
                }
                else
                {
                    Name = Name.Replace(", Max 1L", "");
                }

                UpdateConflictList();
            }
        }

        public bool IsWaste
        {
            get => GetValue<bool>();
            set
            {
                if(!SetValue(value)) return;

                if (value)
                {
                    if (Unno == 1950) IsMax1L = false;
                }

                UpdateDgStowageConflicts();

                OnPropertyChanged("StowageCat");
                OnPropertyChanged("StowageSW");
                OnPropertyChanged("Name");
            }
        }
        public bool IsStabilized
        {
            get { return GetValue<bool>(); }
            set
            {
                if(!SetValue(value)) return;

                UpdateDgStowageConflicts();

                OnPropertyChanged("StowageCat");
                OnPropertyChanged("StowageSW");
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Proper shipping name
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(); }
            set
            {
                string oldName = Name.ToLower().Replace(" ", "").Replace(".", ""); ;
                string newName = value.ToLower().Replace(" ", "").Replace(".", "");

                if(!SetValue(value)) return;
                IsNameChanged = !string.Equals(OriginalNameFromCode, value);

                if (oldName.Contains("waste") && !newName.Contains("waste"))
                    IsWaste = false;
                else if (newName.Contains("waste")) IsWaste = true;
                else if (oldName.Contains("max1l") && !newName.Contains("max1l"))
                    IsMax1L = false;
                else if (newName.Contains("max1l")) IsMax1L = true;
                else if (oldName.Contains("stabilized") && !newName.Contains("stabilized"))
                    IsStabilized = false;
                else if (newName.Contains("stabilized")) IsStabilized = true;

                // AS COOLANT OR AS CONDITIONER 5.5.3.2.1
                if (oldName.Contains("coolant") && !newName.Contains("coolant") ||
                    oldName.Contains("conditioner") && !newName.Contains("conditioner") ||
                    !oldName.Contains("coolant") && newName.Contains("coolant") ||
                    !oldName.Contains("conditioner") && newName.Contains("conditioner"))
                    UpdateConflictList();
            }
        }

        public bool IsSelfReactive
        {
            get { return GetValue<bool>(); }
            set
            {
                if(!SetValue(value)) return;
                OnPropertyChanged();
            }
        }

        public char StowageCat
        {
            get { return GetValue<char>(); }
            set
            {
                if(!SetValue(value)) return;
                UpdateDgStowageConflicts();
            }
        }
        #endregion


        #region Dg properties without additional functionality
        // --------- Dg properties without additional functionality -------

        public Conflicts Conflicts
        {
            get { return GetValue<Conflicts>(); }
            set { SetValue(value); }
        }
        public string DgEMS
        {
            get
            {
                return GetValue<string>();
            }
            set
            {
                SetValue(value);
            }
        }
        public string EmergencyContacts
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public bool EmitFlammableVapours
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
            }
        }
        public bool Flammable
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
            }
        }
        public string FlashPoint
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        /// <summary>
        /// True if proper shipping name is different from original from DgList.
        /// </summary>
        public bool IsNameChanged
        {
            get
            {
                return GetValue<bool>();
            }
            set
            {
                SetValue(value);
            }
        }
        public bool IsTechnicalNameIncluded
        {
            get
            {
                return GetValue<bool>();
            }
            set
            {
                SetValue(value);
            }
        }
        public bool Liquid
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
            }
        }
        public string NumberAndTypeOfPackages
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string Remarks
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string SegregationGroup
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Technical name
        /// </summary>
        public string TechnicalName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }
        #endregion


        #region Readonly Dg properties
        // ---------- Readonly Dg properties ------------------------------

        public bool IsConflicted => GetValue<bool>();
        public string AllDgClasses => GetValue<string>();
        public string Properties => GetValue<string>();
        public string SegregationSG
        {
            get { return GetValue<string>(); }
        }
        public string Special
        {
            get { return GetValue<string>(); }
        }
        public string StowageSW
        {
            get { return GetValue<string>(); }
        }
        public string Surrounded => GetValue<string>();
        public string OriginalNameFromCode => GetValue<string>();

        #endregion
    }
}
