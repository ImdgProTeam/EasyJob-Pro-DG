using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.Model.Validators;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Utility.Messages;
using EasyJob_ProDG.UI.View.Sort;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class DgWrapper : ModelWrapper<Dg>, ILocationOnBoard, IContainer, IUpdatable
    {
        static readonly DgDataBaseDataService dgDataBaseDataService = new DgDataBaseDataService();
        private static IMessageDialogService _messageDialogService = new MessageDialogService();
        private readonly XDocument _dgDataBase = dgDataBaseDataService.GetDgDataBase();


        //Private fields
        private SortableLocation _locationSortable;


        // --------------- Public properties ----------------------------

        public bool IsInList { get; set; }


        //--------------- Properties related to container location ------------------

        #region LocationOnBoard properties
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
        #endregion

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

                if (CurrentProgramData.OwnShip != null) Model.HoldNr = CurrentProgramData.OwnShip.DefineCargoHoldNumber(Bay);
                SetToAllContainersInPlan(GetValue<string>(), oldValue);
                UpdateLocationPresentation();
            }
        }

        /// <summary>
        /// Property used to sort DgWrappers in chosen order
        /// </summary>
        public SortableLocation LocationSortable => _locationSortable ??= new SortableLocation(this);


        //--------------- Properties related to IUpdatable -------------------------- 

        public bool IsPositionLockedForChange
        {
            get => GetValue<bool>();
            set
            {
                if (IsPositionLockedForChange == value) return;
                SetValue(value);
                SetToAllContainersInPlan(value);
            }
        }
        public bool IsToBeKeptInPlan
        {
            get => GetValue<bool>();
            set
            {
                if (IsToBeKeptInPlan == value) return;
                SetValue(value);
                SetToAllContainersInPlan(value);
            }
        }
        public bool IsNotToImport
        {
            get => GetValue<bool>();
            set
            {
                if (IsNotToImport == value) return;
                SetValue(value);
                SetToAllContainersInPlan(value);
            }
        }
        public bool IsToImport
        {
            get => GetValue<bool>();
            set
            {
                if (IsToImport == value) return;
                SetValue(value);
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
                if (POL == value) return;
                SetValue(value);
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
                if (POD == value) return;
                SetValue(value);
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
                if (FinalDestination == value) return;
                SetValue(value);
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
                if (ContainerType == value.ToUpper()) return;
                SetValue(value.ToUpper());
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
                if (IsClosed == value) return;
                SetValue(value);
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
                if (IsRf == value) return;
                SetValue(value);
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
                if (Carrier == value) return;
                SetValue(value);
                if (IsInList)
                    SetToAllContainersInPlan(value);
            }
        }


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
                if (!CheckForExistingUnno(value)) return;

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
                SetValue(value);
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
                SetValue(value);
                if (IsInList)
                {
                    OnUpdatePackingGroup();
                    UpdateConflictList();
                }
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

        public decimal DgNetWeight
        {
            get { return GetValue<decimal>(); }
            set
            {
                SetValue(value);
                OnNetWeightChanged();
            }
        }

        public bool IsLq
        {
            get => GetValue<bool>();
            set
            {
                SetValue(value);
                UpdateConflictList();
            }
        }

        public bool IsMp
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
                UpdateConflictList();
            }
        }

        public bool IsMax1L
        {
            get => GetValue<bool>();
            set
            {
                if (IsMax1L == value) return;
                if (value && Unno != 1950) return;

                SetValue(value);
                if (value)
                {
                    if (!Name.ToLower().Replace(" ", "").Contains("max1l")) Name += ", Max 1L";
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
                if (IsWaste == value) return;
                SetValue(value);
                if (value)
                {
                    if (Unno == 1950) IsMax1L = false;
                    if (!Name.ToLower().Replace(" ", "").Contains("waste")) Name += ", WASTE";
                }
                else
                {
                    Name = Name.Replace(", WASTE", "");
                }
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
                SetValue(value);
                IsNameChanged = !string.Equals(OriginalNameFromCode, value);
                if (value.ToLower().Replace(" ", "").Contains("waste")) IsWaste = true;
                else if (value.ToLower().Replace(" ", "").Contains("max1l")) IsMax1L = true;
            }
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

        public string OriginalNameFromCode => GetValue<string>();

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

        public string NumberAndTypeOfPackages
        {
            get { return GetValue<string>(); }
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
        public bool Liquid
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
        public bool EmitFlammableVapours
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
            }
        }
        public bool IsStabilized
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
                OnPropertyChanged();
                //dgPropertyChanged.Invoke(this, EventArgs.Empty);
                OnPropertyChanged("StowageCat");
                OnPropertyChanged("StowageSW");
            }
        }
        public bool IsSelfReactive
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
                OnPropertyChanged();
                //dgPropertyChanged.Invoke(this, EventArgs.Empty);
            }
        }
        public string Properties => GetValue<string>();

        public string EmergencyContacts
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string Remarks
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Special
        {
            get { return GetValue<string>(); }
        }
        public string StowageSW
        {
            get { return GetValue<string>(); }
        }
        public string SegregationSG
        {
            get { return GetValue<string>(); }
        }
        public char StowageCat
        {
            get { return GetValue<char>(); }
            set
            {
                SetValue(value);
                UpdateConflictList();
            }
        }
        public string SegregationGroup
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public bool IsConflicted
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
            }
        }
        public Conflict Conflict
        {
            get { return GetValue<Conflict>(); }
            set { SetValue(value); }
        }
        public string AllDgClasses => GetValue<string>();
        public string Surrounded => GetValue<string>();

        public bool ContainerTypeRecognized { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        // --------------- Methods ---------------------------------------

        /// <summary>
        /// Clears and registrations and subscriptions, e.g. before deleting
        /// </summary>
        internal void ClearSubscriptions()
        {
            DataMessenger.Default.Unregister(this);
        }

        /// <summary>
        /// Checks if input value is less then '10' and adds '0' before the digit if the case
        /// </summary>
        /// <param name="value"></param>
        /// <param name="numberOfZeros"></param>
        /// <returns></returns>
        private string AddZeroIfRequired(byte value, byte numberOfZeros = 1)
        {
            string result = "";
            if (value < 10) result += "0";
            if (value < 100 && numberOfZeros == 2) result += "0";
            result += value.ToString();
            return result;
        }

        /// <summary>
        /// Supporting method to parse multiple classes into array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string[] ParseMultipleClasses(string value)
        {
            char[] separator = { ' ', ',' };
            string[] array = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return array;
        }

        /// <summary>
        /// Compiles location of input of bay, row or tier
        /// </summary>
        /// <param name="value">Bay, row or tier</param>
        /// <param name="propertyName">Specified property</param>
        /// <returns></returns>
        public string CompileLocation(byte value, [CallerMemberName] string propertyName = null)
        {
            switch (propertyName.ToLower())
            {
                case "bay":
                    return AddZeroIfRequired(value, 2) + AddZeroIfRequired(Row) + AddZeroIfRequired(Tier);
                case "row":
                    return AddZeroIfRequired(Bay, 2) + AddZeroIfRequired(value) + AddZeroIfRequired(Tier);
                case "tier":
                    return AddZeroIfRequired(Bay, 2) + AddZeroIfRequired(Row) + AddZeroIfRequired(value);
                default:
                    return Location;
            }
        }

        /// <summary>
        /// Checks if UN no exists in database and prompts user to confirm weather to continue if UN does not exist.
        /// </summary>
        /// <param name="unno">UN no being checked</param>
        /// <returns>If UN no is valid or user acknowledge</returns>
        private bool CheckForExistingUnno(ushort unno)
        {
            if (UnnoValidator.Validate(unno)) return true;
            if (_messageDialogService.ShowYesNoDialog(
                    $"UN no {unno:0000} does not exist in the DataBase. \nDo you wish to proceed?", "Attention!") ==
                MessageDialogResult.Yes) return true;
            return false;
        }

        /// <summary>
        /// Adds TechnicalName, if any, to ProperShippingName and changes status of IsTechnicalNameIncluded.
        /// </summary>
        public void IncludeTechnicalName()
        {
            if (IsTechnicalNameIncluded) return;
            if (string.IsNullOrEmpty(TechnicalName)) return;
            Name += "\n" + TechnicalName;
            IsTechnicalNameIncluded = true;
        }

        /// <summary>
        /// Removes TechnicalName, if any, to ProperShippingName and changes status of IsTechnicalNameIncluded.
        /// </summary>
        public void RemoveTechnicalName()
        {
            if (!IsTechnicalNameIncluded) return;
            if (string.IsNullOrEmpty(TechnicalName)) return;
            Name = Name.Replace(TechnicalName, "");
            if (Name.EndsWith("\n"))
                Name = Name.Remove(Name.Length - 1);
            IsTechnicalNameIncluded = false;
        }


        // --------------- Methods affecting view changes ---------------------------

        /// <summary>
        /// Updates Dg info from DgListDataBase for the unit and updates presentation
        /// </summary>
        public void UpdateDgInfoAndUploadChanges()
        {
            Model.UpdateDgInfo(_dgDataBase);
            UpdateDgDataPresentation();
        }

        /// <summary>
        /// Called when packing group is updated. Contains logic to process the PKG change.
        /// </summary>
        private void OnUpdatePackingGroup()
        {
            //TODO: Check how packing group change affects other values, if updated manually
            Model.AssignFromDgList(_dgDataBase, false, true);
            OnDgPackingGroupChangedEventHandler.Invoke(this);
            UpdateDgDataPresentation();
        }

        /// <summary>
        /// Notifies CargoPlan of change in DgNetWeight.
        /// </summary>
        private void OnNetWeightChanged()
        {
            DataMessenger.Default.Send<UpdateCargoPlan>(new UpdateCargoPlan(), "Net weight changed");
        }

        /// <summary>
        /// Invokes event to update conflict list
        /// </summary>
        private void UpdateConflictList()
        {
            OnConflictListToBeChangedEventHandler.Invoke(this);
        }

        /// <summary>
        /// Calls OnPropertyChanged for most of Dg properties
        /// </summary>
        private void UpdateDgDataPresentation()
        {
            OnPropertyChanged("Unno");
            OnPropertyChanged("DgClass");
            OnPropertyChanged("DgSubclass");
            OnPropertyChanged("Name");
            OnPropertyChanged("PackingGroup");
            OnPropertyChanged("AllDgClasses");
            OnPropertyChanged("Liquid");
            OnPropertyChanged("Flammable");
            OnPropertyChanged("EmitFlammableVapours");
            OnPropertyChanged("StowageCat");
            OnPropertyChanged("StowageSW");
            OnPropertyChanged("SegregationSG");
            OnPropertyChanged("SegregationGroup");
            OnPropertyChanged("Special");
            OnPropertyChanged("IsMax1L");
            OnPropertyChanged("IsWaste");
        }

        /// <summary>
        /// Calls OnPropertyChanged for all location related properties
        /// </summary>
        private void UpdateLocationPresentation()
        {
            OnPropertyChanged("Location");
            OnPropertyChanged("Bay");
            OnPropertyChanged("Row");
            OnPropertyChanged("Tier");
            OnPropertyChanged("HoldNr");
            OnPropertyChanged("IsUnderdeck");
            OnPropertyChanged("Size");
            OnPropertyChanged("LocationSortable");
        }

        /// <summary>
        /// Sends message to synchronise changes with CargoPlan
        /// </summary>
        /// <param name="value">new value set</param>
        /// <param name="oldValue">old value</param>
        /// <param name="propertyName">property that is changed</param>
        private void SetToAllContainersInPlan(object value, object oldValue = null, [CallerMemberName] string propertyName = null)
        {
            DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(this, value, oldValue, propertyName));
        }


        // --------------- Validation -----------------------------------------------

        /// <summary>
        /// Validation logic for properties
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(DgClass):
                    {
                        if (!DgClassValidator.IsValidDgClass(DgClass))
                        {
                            yield return "Dg class is invalid";
                        }


                        break;
                    }
                case nameof(ContainerType):
                    {
                        if (ContainerType.Length != 4)
                        {
                            yield return "Inappropriate unit type length";
                        }
                        break;
                    }
                case nameof(Unno):
                    {
                        if (Unno.ToString().Length != 4)
                        {
                            yield return "Inappropriate UN number length";
                        }
                        break;
                    }
                default:
                    break;
            }
        }


        // --------------- Converters -----------------------------------------------

        /// <summary>
        /// Converts DgWrapper into a ContainerWrapper
        /// </summary>
        /// <returns>ContainerWrapper instance</returns>
        public ContainerWrapper ConvertToContainerWrapper()
        {
            Container newContainer = new Container { ContainerNumber = ContainerNumber, Location = Location };
            newContainer.DgCountInContainer++;
            return new ContainerWrapper(newContainer);
        }

        /// <summary>
        /// Converts DgWrapper to Dg (returns its model)
        /// </summary>
        /// <returns>Dg instance</returns>
        public Dg ConvertBackToDg()
        {
            return (Dg)Model;
        }


        // --------------- Public constructors --------------------------------------

        public DgWrapper() : base(new Dg())
        {
            IsNameChanged = false;
            FlashPoint = "";
        }

        public DgWrapper(Dg model) : base(model)
        {
            IsNameChanged = false;
        }


        // --------------- Events ---------------------------------------------------

        public delegate void DgPackingGroupChangedEventHandler(object sender);
        public static event DgPackingGroupChangedEventHandler OnDgPackingGroupChangedEventHandler = null;

        public delegate void ConflictListToBeChangedEventHandler(object sender);
        public static event ConflictListToBeChangedEventHandler OnConflictListToBeChangedEventHandler = null;


        // -------------- Overriding methods and operators --------------------------

        public override string ToString()
        {
            return ContainerNumber + " in " + Location + " class " + DgClass + " (unno " + Unno + ")";
        }

        public static explicit operator Dg(DgWrapper dgWrapper)
        {
            return dgWrapper.ConvertBackToDg();
        }

        public static explicit operator ContainerWrapper(DgWrapper dgWrapper)
        {
            return dgWrapper.ConvertToContainerWrapper();
        }


    }
}
