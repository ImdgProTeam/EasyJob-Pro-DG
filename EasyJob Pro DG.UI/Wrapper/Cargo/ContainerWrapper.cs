﻿using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.View.Sort;
using System.Runtime.CompilerServices;
using Container = EasyJob_ProDG.Model.Cargo.Container;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ContainerWrapper : ModelWrapper<Container>, ILocationOnBoard, IContainer, IUpdatable, IReefer
    {
        //--------------- Private fields --------------------------------------------



        //--------------- Public properties -----------------------------------------

        /// <summary>
        /// Container number
        /// </summary>
        public string ContainerNumber
        {
            get => GetValue<string>();
            set
            {
                var oldValue = ContainerNumber;

                if(!SetValue(value.ToUpper())) return;
                SetToAllContainersInPlan(value.ToUpper(), oldValue);
            }
        }

        /// <summary>
        /// Container location on board
        /// </summary>
        public string Location
        {
            get => GetValue<string>();
            set
            {
                var oldValue = Location;

                if(!SetValue(value)) return;
                _locationSortable = null;

                if (CurrentProgramData.OwnShip != null) 
                    Model.HoldNr = CurrentProgramData.OwnShip.DefineCargoHoldNumber(Bay);
                SetToAllContainersInPlan(GetValue<string>(), oldValue);
                RefreshLocation();
            }
        }

        /// <summary>
        /// True if ContainerWrapper contains dg cargo
        /// </summary>
        public bool ContainsDgCargo => (Model.DgCountInContainer > 0);

        public byte DgCountInContainer => GetValue<byte>();

        /// <summary>
        /// Used to sort containers in certain order
        /// </summary>
        private SortableLocation _locationSortable;
        public SortableLocation LocationSortable => _locationSortable ??= new SortableLocation(this);

        /// <summary>
        /// Any user defined text remarks or comments
        /// </summary>
        public string Remarks
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        #region LocationOnBoard properties


        //All setters are blank
        public byte Bay
        {
            get => Model.Bay;
            set { }
        }
        public byte HoldNr
        {
            get => Model.HoldNr;
            set { }
        }
        public byte Row
        {
            get => Model.Row;
            set { }
        }
        public byte Size
        {
            get => Model.Size;
            set { }
        }
        public byte Tier
        {
            get => Model.Tier;
            set { }
        }
        public bool IsUnderdeck
        {
            get => Model.IsUnderdeck;
            set { }
        }
        #endregion

        #region IContainer properties

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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public bool ContainerTypeRecognized { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                Model.UpdateContainerType();
                SetToAllContainersInPlan(value.ToUpper());

                OnPropertyChanged();
                OnPropertyChanged($"IsClosed");
                OnPropertyChanged($"IsOpen");
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
                OnPropertyChanged();
            }
        }

        #endregion

        #region IReefer properties

        public double SetTemperature
        {
            get { return GetValue<double>(); }
            set { SetValue(value);           }
        }
        public string Commodity
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string VentSetting
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public double LoadTemperature
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }
        public string ReeferSpecial
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string ReeferRemark
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Removes all reefer-related property values.
        /// </summary>
        public void ResetReefer()
        {
            Model.ResetReefer();
        }

        #endregion

        #region IUpdatable properties
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
                if (value) IsToImport = false;
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
                if(value) IsNotToImport = false;
                SetToAllContainersInPlan(value);
            }
        }

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
            get => GetValue<string>();
            set { }
        } 
        #endregion


        //--------------- Public methods --------------------------------------------

        /// <summary>
        /// Returns Model
        /// </summary>
        /// <returns></returns>
        public Container ConvertBackToPlainContainer()
        {
            return Model;
        }

        /// <summary>
        /// Clears and registrations and subscriptions, e.g. before deleting
        /// </summary>
        internal void ClearSubscriptions()
        {
            DataMessenger.Default.Unregister(this);
        }


        //--------------- Public methods related to representation ------------------

        /// <summary>
        /// Calls OnPropertyChanged for a number of properties
        /// </summary>
        internal void Refresh()
        {
            OnPropertyChanged("ContainsDgCargo");
            OnPropertyChanged("DgCountInContainer");
            OnPropertyChanged("ContainerNumber");
            OnPropertyChanged("POL");
            OnPropertyChanged("POD");
            OnPropertyChanged("Type");
            OnPropertyChanged("IsClosed");
            OnPropertyChanged(nameof(IsRf));

            RefreshLocation();
        }

        internal void RefreshIReefer()
        {
            OnPropertyChanged("Commodity");
            OnPropertyChanged("SetTemperature");
            OnPropertyChanged("VentSetting");
            OnPropertyChanged("LoadTemperature");
            OnPropertyChanged("ReeferSpecial");
            OnPropertyChanged("ReeferRemark");
        }

        /// <summary>
        /// Calls OnPropertyChanged for all location related properties
        /// </summary>
        private void RefreshLocation()
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

        

        //--------------- Private methods -------------------------------------------

        /// <summary>
        /// Sends request to CargoPlanWrapper to set new value to all containers in the plan
        /// </summary>
        /// <param name="value">new value to be set</param>
        /// <param name="oldValue">old value</param>
        /// <param name="propertyName">property of which value to be changed</param>
        private void SetToAllContainersInPlan(object value, string oldValue = null, [CallerMemberName] string propertyName = null)
        {
            DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(this, value, oldValue, propertyName, false));
        }

        //--------------- Constructors ----------------------------------------------

        public ContainerWrapper(Container model) : base(model)
        {

        }


        // -------------- Override methods and explicit operators -------------------

        public static explicit operator Container(ContainerWrapper containerWrapper)
        {
            return containerWrapper.ConvertBackToPlainContainer();
        }


        // -------------- Events ----------------------------------------------------

    }
}
