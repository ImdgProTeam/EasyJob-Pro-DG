using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Wrapper.Cargo;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    public partial class DgWrapper : AbstractContainerWrapper<Dg>
    {

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
                NotifyOfChangedProperties();
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
                SetValue(value.Trim().Replace(" ", ""));
                if (Model.DgSubClassArray.Contains(value))
                {
                    string[] tempdgsubclasses = new string[2];
                    for (int i = 0; i < Model.DgSubClassArray.Length; i++)
                    {
                        string subclass = Model.DgSubClassArray[i];
                        if (!string.Equals(subclass, value))
                            tempdgsubclasses[i] = subclass;
                    }
                    Model.DgSubClassArray = tempdgsubclasses;
                }
                OnPropertyChanged(nameof(AllDgClasses));
                UpdateConflictList();
            }
        }

        /// <summary>
        /// Set: will add string dg class to dgsubclass and will update allDgClasses.
        /// Get: will return string with all subclasses listed and separated with comma.
        /// </summary>
        public string DgSubClass
        {
            get => GetValue<string>();
            set
            {
                //TODO: Implement input check
                //Check if DgClass already has the subrisk being input
                var setvalue = value.Replace(",", " ").Replace("  ", " ").Trim();
                SetValue(setvalue);

                OnPropertyChanged(nameof(AllDgClasses));
                UpdateConflictList();
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
                if (!SetValue(value)) return;
                OnUpdatePackingGroup();
                UpdateConflictList();

            }
        }

        public decimal DgNetWeight
        {
            get { return GetValue<decimal>(); }
            set
            {
                if (!SetValue(value)) return;
                OnNetWeightChanged();
            }
        }

        public bool IsLq
        {
            get => GetValue<bool>();
            set
            {
                if (!SetValue(value)) return;
                UpdateConflictList();
                NotifyOfChangedProperties();
            }
        }

        public bool IsMp
        {
            get { return GetValue<bool>(); }
            set
            {
                if (!SetValue(value)) return;
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

                if (!SetValue(value)) return;
                if (value)
                {
                    if (!Name.ToLower().Replace(" ", "").Replace(".", "").Contains("max1l")) Name += ", Max 1L";
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
                if (!SetValue(value)) return;

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
                if (!SetValue(value)) return;

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

                if (!SetValue(value)) return;
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
                if (!SetValue(value)) return;
                OnPropertyChanged();
            }
        }

        public char StowageCat
        {
            get { return GetValue<char>(); }
            set
            {
                if (!SetValue(value)) return;
                UpdateDgStowageConflicts();
            }
        }

        public string FlashPoint
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
                UpdateConflictList();
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
