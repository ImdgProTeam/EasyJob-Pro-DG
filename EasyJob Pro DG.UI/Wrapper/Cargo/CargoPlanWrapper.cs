using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class CargoPlanWrapper : ModelWrapper<CargoPlan>
    {

        // -------------- Public properties -----------------------------------------

        public DgWrapperList DgList { get; set; }
        public ObservableCollection<ContainerWrapper> Containers { get; set; }
        public ObservableCollection<ContainerWrapper> Reefers { get; set; }
        public Voyage VoyageInfo
        {
            get { return GetValue<Voyage>(); }
            set { SetValue(value); }
        }

        public int ContainerCount => Containers.Count;
        public int ReeferCount => Reefers.Count;
        public int DgContainerCount => Containers.Count(c => c.ContainsDgCargo);
        public decimal TotalDgNetWeight => Model.TotalDgNetWeight;
        public decimal TotalMPNetWeight => Model.TotalMPNetWeight;
        internal bool IsEmpty => Model.IsEmpty;



        // -------------- Private methods -------------------------------------------

        /// <summary>
        /// Registers to messages from Messenger
        /// </summary>
        private void SubscribeToMessenger()
        {
            DataMessenger.Default.Register<UpdateCargoPlan>(this, OnNetWeightChanged, "Net weight changed");
        }

        /// <summary>
        /// Unregisters from messages in Messenger
        /// </summary>
        private void UnsubscribeToMessenger()
        {
            DataMessenger.Default.Unregister(this);
        }

        /// <summary>
        /// Converts WorkingCargoPlan into CargoPlanWrapper
        /// </summary>
        /// <param name="cargoplan">Plain WorkingCargoPlan</param>
        private void ConvertToCargoPlanWrapper(CargoPlan cargoplan)
        {
            DgList = new DgWrapperList(cargoplan.DgList);

            Containers = new ObservableCollection<ContainerWrapper>(Model.Containers.Select(c => new ContainerWrapper(c)));
            Reefers = new ObservableCollection<ContainerWrapper>(Model.Reefers.Select(r => new ContainerWrapper(r)));

            RegisterCollection(DgList, cargoplan.DgList);
            RegisterCollection(Containers, Model.Containers);
            RegisterCollection(Reefers, Model.Reefers);
        }





        /// <summary>
        /// Updates total dg net weight.
        /// </summary>
        /// <param name="obj">nil</param>
        private void OnNetWeightChanged(UpdateCargoPlan obj = null)
        {
            OnPropertyChanged(nameof(TotalDgNetWeight));
            OnPropertyChanged(nameof(TotalMPNetWeight));
        }

        /// <summary>
        /// Updates HoldNr property for all units in <see cref="CargoPlanWrapper"/>
        /// </summary>
        internal void UpdateCargoHoldNumbers()
        {
            HandleCargoPlanUnits.OnCargoHoldsUpdated(Model);
        }



        /// <summary>
        /// Calls OnPropertyChanged on WorkingCargoPlan count values
        /// </summary>
        internal void RefreshCargoPlanValues()
        {
            OnPropertyChanged(nameof(ContainerCount));
            OnPropertyChanged(nameof(ReeferCount));
            OnPropertyChanged(nameof(DgContainerCount));
            OnNetWeightChanged();
        }





        // -------------- Public methods --------------------------------------------

        /// <summary>
        /// Creates new CargoPlanWrapper from a plain cargoPlan.
        /// Updates summary values.
        /// Called when updating ShipProfile.
        /// </summary>
        /// <param name="cargoplan">Plain WorkingCargoPlan</param>
        internal void CreateCargoPlanWrapper(CargoPlan cargoplan)
        {
            ConvertToCargoPlanWrapper(cargoplan);
            RefreshCargoPlanValues();
        }


        #region Constructor and destructor

        // -------------- Constructors ----------------------------------------------

        public CargoPlanWrapper(CargoPlan model) : base(model)
        {
            ConvertToCargoPlanWrapper(model);

            SubscribeToMessenger();
        }

        public void Dispose()
        {
            UnsubscribeToMessenger();
        }

        #endregion

    }
}
