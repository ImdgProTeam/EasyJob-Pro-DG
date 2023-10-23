using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.Sort;
using System;
using System.Runtime.CompilerServices;

namespace EasyJob_ProDG.UI.Wrapper.Cargo
{
    public abstract class AbstractContainerWrapper<T> : ModelWrapper<T>, ILocationOnBoard, IContainer, IUpdatable
        where T : ContainerAbstract
    {
        /// <summary>
        /// Used to sort containers in certain order
        /// </summary>
        private SortableLocation _locationSortable;
        public SortableLocation LocationSortable => _locationSortable ??= new SortableLocation(this);


        #region ILocationOnBoard
        public byte Size => GetValue<byte>();
        public byte Bay => GetValue<byte>();
        public byte Row => GetValue<byte>();
        public byte Tier => GetValue<byte>();
        public bool IsUnderdeck => GetValue<bool>();
        public byte HoldNr => GetValue<byte>();

        /// <summary>
        /// Container location on board
        /// </summary>
        public string Location
        {
            get => GetValue<string>();
            set
            {
                var oldValue = Location;

                if (!SetValue(value)) return;
                _locationSortable = null;

                if (CurrentProgramData.OwnShip != null)
                    Model.HoldNr = CurrentProgramData.OwnShip.DefineCargoHoldNumber(Bay);
                SetToAllContainersInPlan(GetValue<string>(), oldValue);
                RefreshLocation();
            }
        }

        #endregion

        #region IContainer
        /// <summary>
        /// Container number
        /// </summary>
        public string ContainerNumber
        {
            get => GetValue<string>();
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                var oldValue = ContainerNumber;
                var newValue = value.ToUpper();

                if (!SetValue(newValue)) return;
                OnPropertyChanged(nameof(HasNoNumber));
                SetToAllContainersInPlan(newValue, oldValue);
                OnPropertyChanged(nameof(DisplayContainerNumber));
            }
        }

        /// <summary>
        /// ContainerNumber in format in accordance with UserSettings selected.
        /// </summary>
        public string DisplayContainerNumber
        {
            get => UserSettings.ContainerNumberToDisplay(ContainerNumber);
            set
            {
                ContainerNumber = UserSettings.ContainerNumberFromDisplay(value);
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
                OnPropertyChanged(nameof(IsOpen));
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

        public bool ContainerTypeRecognized { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool HasNoNumber => GetValue<bool>();

        /// <summary>
        /// Port of discharging
        /// </summary>
        public string POD
        {
            get => GetValue<string>();
            set
            {
                if (!SetValue(value)) return;
                SetToAllContainersInPlan(value);
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
                if (!SetValue(value)) return;
                SetToAllContainersInPlan(value);
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
                if (!SetValue(value)) return;
                SetToAllContainersInPlan(value);

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
                SetToAllContainersInPlan(value.ToUpper());
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
                SetToAllContainersInPlan(value);
            }
        }

        #endregion

        #region IUpdatable
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
                if (value) IsNotToImport = false;
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


        /// <summary>
        /// Clears and registrations and subscriptions, e.g. before deleting
        /// </summary>
        internal void ClearSubscriptions()
        {
            DataMessenger.Default.Unregister(this);
        }

        /// <summary>
        /// Calls OnPropertyChanged for a number of properties
        /// </summary>
        internal void Refresh()
        {
            OnPropertyChanged("ContainsDgCargo");
            OnPropertyChanged("DgCountInContainer");

            OnPropertyChanged(nameof(ContainerNumber));
            OnPropertyChanged(nameof(DisplayContainerNumber));
            OnPropertyChanged(nameof(POL));
            OnPropertyChanged(nameof(POD));
            OnPropertyChanged(nameof(FinalDestination));
            OnPropertyChanged(nameof(Carrier));
            OnPropertyChanged(nameof(ContainerType));
            OnPropertyChanged(nameof(IsClosed));
            OnPropertyChanged(nameof(IsOpen));

            UpdateReeferProperty();
            RefreshLocation();
            RefreshUpdatables();
        }

        /// <summary>
        /// Calls OnPropertyChanged for IsRf property
        /// </summary>
        internal void UpdateReeferProperty()
        {
            OnPropertyChanged(nameof(IsRf));
        }



        /// <summary>
        /// Calls OnPropertyChanged for all location related properties
        /// </summary>
        private void RefreshLocation()
        {
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(Bay));
            OnPropertyChanged(nameof(Row));
            OnPropertyChanged(nameof(Tier));
            OnPropertyChanged(nameof(HoldNr));
            OnPropertyChanged(nameof(IsUnderdeck));
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(LocationSortable));
        }

        /// <summary>
        /// Calls OnPropertyChanged for all IUpdatable related settable properties
        /// </summary>
        private void RefreshUpdatables()
        {
            OnPropertyChanged(nameof(IsToBeKeptInPlan));
            OnPropertyChanged(nameof(IsPositionLockedForChange));
            OnPropertyChanged(nameof(IsToImport));
            OnPropertyChanged(nameof(IsNotToImport));
        }


        /// <summary>
        /// Sends request to CargoPlanWrapper to set new value to all containers in the plan
        /// </summary>
        /// <param name="value">new value to be set</param>
        /// <param name="oldValue">old value</param>
        /// <param name="propertyName">property of which value to be changed</param>
        protected abstract void SetToAllContainersInPlan(object value, object oldValue = null, [CallerMemberName] string propertyName = null);


        #region Constructor

        public AbstractContainerWrapper(T model) : base(model)
        {
        }

        #endregion
    }
}
