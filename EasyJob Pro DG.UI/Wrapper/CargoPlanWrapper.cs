using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using EasyJob_ProDG.Model.Transport;
using Container = EasyJob_ProDG.Model.Cargo.Container;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class CargoPlanWrapper : ModelWrapper<CargoPlan>
    {
        // ----------------- Private fields ------------------------------------

        private IMessageDialogService _messageDialogService = new MessageDialogService();
        private CargoPlanUnitPropertyChanger _propertyChanger;


        // -------------- Public properties -----------------------------------------

        public DgWrapperList DgList { get; set; }
        public ObservableCollection<ContainerWrapper> Containers { get; set; }
        public ObservableCollection<ContainerWrapper> Reefers { get; set; }
        public Voyage VoyageInfo
        {
            get { return GetValue<Voyage>(); }
            set{SetValue(value);}
        }

        public int ContainerCount => Containers.Count;
        public int ReeferCount => Reefers.Count;
        public int DgContainerCount => Containers.Count(c => c.ContainsDgCargo);
        public decimal TotalDgNetWeight => Model.TotalDgNetWeight;
        internal bool IsEmpty => Model.IsEmpty;


        // -------------- Constructors ----------------------------------------------

        public CargoPlanWrapper(CargoPlan model) : base(model)
        {
            ConvertToCargoPlanWrapper(model);

            _propertyChanger = new CargoPlanUnitPropertyChanger();
            _propertyChanger.SetUp(this);

            SubscribeToMessenger();

            DgList.CollectionChanged += DgListChanged;
        }

        public void Destructor()
        {
            UnsubscribeToMessenger();
        }

        // -------------- Private methods -------------------------------------------

        /// <summary>
        /// Registers to messages from Messenger
        /// </summary>
        private void SubscribeToMessenger()
        {
            DataMessenger.Default.Register<UpdateCargoPlan>(this, OnDgRemoveRequested, "Remove dg");
            DataMessenger.Default.Register<UpdateCargoPlan>(this, OnNetWeightChanged, "Net weight changed");
        }

        /// <summary>
        /// Unregisters from messages in Messenger
        /// </summary>
        private void UnsubscribeToMessenger()
        {
            DataMessenger.Default.Unregister(this);
            DataMessenger.Default.Unregister(this, "Remove dg");
        }

        /// <summary>
        /// Converts CargoPlan into CargoPlanWrapper
        /// </summary>
        /// <param name="cargoplan">Plain CargoPlan</param>
        private void ConvertToCargoPlanWrapper(CargoPlan cargoplan)
        {
            DgList = new DgWrapperList(cargoplan.DgList);

            Containers = new ObservableCollection<ContainerWrapper>();
            foreach (Container unit in cargoplan.Containers)
                Containers.Add(new ContainerWrapper(unit));

            Reefers = new ObservableCollection<ContainerWrapper>();
            foreach (Container unit in cargoplan.Reefers)
                Reefers.Add(new ContainerWrapper(unit));
        }

        /// <summary>
        /// Updates total dg net weight.
        /// </summary>
        /// <param name="obj">nil</param>
        private void OnNetWeightChanged(UpdateCargoPlan obj)
        {
            OnPropertyChanged("TotalDgNetWeight");
        }

        /// <summary>
        /// Invoked when DgList changed
        /// NO ACTION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {

            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {

            }
        }

        /// <summary>
        /// Removes selected dg from CargoPlan
        /// </summary>
        /// <param name="obj"></param>
        private void OnDgRemoveRequested(UpdateCargoPlan obj)
        {
            foreach (var dgWrapper in obj.DgWrappersChanged)
            {
                RemoveDg(dgWrapper);
                RemoveRemovedDgFromCargoPlan(dgWrapper.ContainerNumber);
            }

            DataMessenger.Default.Send(new ConflictListToBeUpdatedMessage());
            OnPropertyChanged("DgContainerCount");
            OnPropertyChanged("TotalDgNetWeight");
        }

        /// <summary>
        /// Method removes dg from DgWrapperList and from model as well
        /// </summary>
        /// <param name="dg"></param>
        private void RemoveDg(DgWrapper dg)
        {
            dg.ClearSubscriptions();
            DgList.Remove(dg);
            Model.DgList.Remove(dg.Model);
        }

        /// <summary>
        /// Reduces number of DgInContainer property of all Containers in CargoPlan
        /// </summary>
        /// <param name="containerNumber"></param>
        private void RemoveRemovedDgFromCargoPlan(string containerNumber)
        {
            var container = Containers.FirstOrDefault(c => c.ContainerNumber == containerNumber);
            container?.Model.RemoveDgFromContainer();
            container?.Refresh();
        }


        // -------------- Public methods --------------------------------------------

        /// <summary>
        /// Creates new CargoPlanWrapper from a plain cargoPlan.
        /// Updates summary values.
        /// Called when updating ShipProfile.
        /// </summary>
        /// <param name="cargoplan">Plain CargoPlan</param>
        internal void CreateCargoPlanWrapper(CargoPlan cargoplan)
        {
            ConvertToCargoPlanWrapper(cargoplan);
            UpdateCargoPlanValues();
        }

        /// <summary>
        /// Clears all cargo in all the lists
        /// TO DELETE IF NOT CALLED
        /// </summary>
        internal void Clear()
        {
            DgList.Clear();
            Reefers.Clear();
            Containers.Clear();
        }

        /// <summary>
        /// Calls OnPropertyChanged on CargoPlan count values
        /// </summary>
        internal void UpdateCargoPlanValues()
        {
            OnPropertyChanged("ContainerCount");
            OnPropertyChanged("ReeferCount");
            OnPropertyChanged("DgContainerCount");
        }


        // -------------- Add/Remove/Modify methods ---------------------------------

        /// <summary>
        /// Adds Dg and respective container (if not yet exists) to CargoPlanWrapper and its Model
        /// </summary>
        /// <param name="dg">Dg to be added</param>
        internal void AddDg(Dg dg)
        {
            Model.DgList.Add(dg);
            DgList.Add(new DgWrapper(dg));

            var container = dg.ConvertToContainer();

            if (!Model.Containers.Contains(container))
            {
                var containerWrapper = new ContainerWrapper(container);
                Model.Containers.Add(container);
                Containers.Add(containerWrapper);
                if (container.IsRf)
                {
                    Model.Reefers.Add(container);
                    Reefers.Add(containerWrapper);
                }
            }

            UpdateCargoPlanValues();
        }

        /// <summary>
        /// Adds DgWrapper and respective Container (if not yet exists) to CargoPlanWrapper and its Model
        /// </summary>
        /// <param name="dg">DgWrapper to be added</param>
        internal void AddDg(DgWrapper dg)
        {
            AddDg(dg.ConvertBackToDg());
        }

        /// <summary>
        /// Adds a Container to Reefers list
        /// </summary>
        /// <param name="unit">Container to be added</param>
        internal void AddReefer(Container unit)
        {
            if (Model.Reefers.Contains(unit)) return;
            Model.Reefers.Add(unit);
            Reefers.Add(new ContainerWrapper(unit));
            unit.IsRf = true;
        }

        /// <summary>
        /// Adds a ContainerWrapper to Reefers list
        /// </summary>
        /// <param name="unit">ContainerWrapper to be added</param>
        internal void AddReefer(ContainerWrapper unit)
        {
            AddReefer(unit.Model);
        }

        /// <summary>
        /// Removes reefer unit from CargoPlan and Model Reefers
        /// </summary>
        /// <param name="unit">Reefer to be removed</param>
        /// <returns>true if reefer deleted</returns>
        internal void RemoveReefer(ContainerWrapper unit)
        {
            Reefers.Remove(Reefers.SingleOrDefault(r => r.ContainerNumber == unit.ContainerNumber));
            Model.Reefers.Remove(Model.Reefers.SingleOrDefault(r => r.ContainerNumber == unit.ContainerNumber));
        }


        // -------------- Static methods and converters -----------------------------

        /// <summary>
        /// Checks if a CargoPlan unit exists in a collection
        /// </summary>
        /// <param name="container">CargoPlan unit to be found</param>
        /// <param name="collection">Collection of the units to be searched</param>
        /// <returns></returns>
        public static bool ContainedInList<T, TM>(T container, ICollection<TM> collection)
            where T : IContainer, ILocationOnBoard
            where TM : IContainer, ILocationOnBoard
        {
            return collection.Any(c =>
                c.ContainerNumber == container.ContainerNumber && c.Location == container.Location);
        }

        /// <summary>
        /// Converts each DgWrapper in DgList into plain Dg
        /// </summary>
        /// <returns>New List of Dg</returns>
        public List<Dg> ExtractPocoDgList()
        {
            List<Dg> tempList = new List<Dg>();
            foreach (var wrapper in DgList)
            {
                tempList.Add(wrapper.ConvertBackToDg());
            }
            return tempList;
        }

        /// <summary>
        /// Converts this CargoPlanWrapper into a new CargoPlan
        /// </summary>
        /// <returns>New CargoPlan</returns>
        internal CargoPlan ConvertCargoPlanWrapperToPlainCargoPlan()
        {
            //Creating a new instance of CargoPlan
            CargoPlan result = new CargoPlan();

            //converting DgList
            result.DgList = ExtractPocoDgList();

            //converting Containers
            ICollection<Container> containers = result.Containers;
            foreach (var containerW in Containers)
            {
                containers.Add(containerW.ConvertBackToPlainContainer());
            }

            //converting Reefers
            ICollection<Container> reefers = result.Reefers;
            foreach (var reeferW in Reefers)
            {
                containers.Add(reeferW.ConvertBackToPlainContainer());
            }

            return result;
        }

    }
}
