using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper.Cargo;
using System.Runtime.CompilerServices;
using Container = EasyJob_ProDG.Model.Cargo.Container;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ContainerWrapper : AbstractContainerWrapper<Container>, IReefer
    {
        /// <summary>
        /// True if ContainerWrapper contains dg cargo
        /// </summary>
        public bool ContainsDgCargo => GetValue<bool>();
        public byte DgCountInContainer => GetValue<byte>();

        /// <summary>
        /// Any user defined text remarks or comments
        /// </summary>
        public string Remarks
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        #region IReefer properties

        public decimal SetTemperature
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
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
        public decimal LoadTemperature
        {
            get { return GetValue<decimal>(); }
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


        //--------------- Public methods --------------------------------------------

        /// <summary>
        /// Returns Model
        /// </summary>
        /// <returns></returns>
        public Container ConvertBackToPlainContainer()
        {
            return Model;
        }

        internal void RefreshIReefer()
        {
            OnPropertyChanged(nameof(Commodity));
            OnPropertyChanged(nameof(SetTemperature));
            OnPropertyChanged(nameof(VentSetting));
            OnPropertyChanged(nameof(LoadTemperature));
            OnPropertyChanged(nameof(ReeferSpecial));
            OnPropertyChanged(nameof(ReeferRemark));
        }



        //--------------- Protected methods -------------------------------------------

        /// <summary>
        /// Sends request to CargoPlanWrapper to set new value to all containers in the plan
        /// </summary>
        /// <param name="value">new value to be set</param>
        /// <param name="oldValue">old value</param>
        /// <param name="propertyName">property of which value to be changed</param>
        protected override void SetToAllContainersInPlan(object value, object oldValue = null, [CallerMemberName] string propertyName = null)
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
