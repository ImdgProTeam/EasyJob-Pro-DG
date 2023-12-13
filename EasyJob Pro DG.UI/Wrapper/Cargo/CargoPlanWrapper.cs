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
using System;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class CargoPlanWrapper : ModelWrapper<CargoPlan>
    {
        // ----------------- Private fields ------------------------------------

        private IMessageDialogService _messageDialogService => MessageDialogService.Connect();
        private CargoPlanUnitPropertyChanger _propertyChanger;


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


        // -------------- Constructors ----------------------------------------------

        public CargoPlanWrapper(CargoPlan model) : base(model)
        {
            ConvertToCargoPlanWrapper(model);

            _propertyChanger = new CargoPlanUnitPropertyChanger();
            _propertyChanger.SetUp(this);

            SubscribeToMessenger();

            DgList.CollectionChanged += DgListChanged;
            Reefers.CollectionChanged += ReefersCollectionChanged;
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
        /// Converts WorkingCargoPlan into CargoPlanWrapper
        /// </summary>
        /// <param name="cargoplan">Plain WorkingCargoPlan</param>
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
            OnPropertyChanged(nameof(TotalDgNetWeight));
            OnPropertyChanged(nameof(TotalMPNetWeight));
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
        private void ReefersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    foreach (var item in e.OldItems)
            //    {
            //        var container = Containers.FindContainerByContainerNumber((IContainer)item);
            //        container.IsRf = false;
            //    }
            //}
        }

        /// <summary>
        /// Removes selected dg from WorkingCargoPlan
        /// </summary>
        /// <param name="obj"></param>
        private void OnDgRemoveRequested(UpdateCargoPlan obj)
        {
            foreach (var dgWrapper in obj.DgWrappersChanged)
            {
                RemoveDg(dgWrapper);
                RemoveRemovedDgFromCargoPlan(dgWrapper);
            }

            DataMessenger.Default.Send(new ConflictListToBeUpdatedMessage());
            OnPropertyChanged("DgContainerCount");
            OnPropertyChanged("TotalDgNetWeight");
            OnPropertyChanged(nameof(TotalMPNetWeight));
        }

        /// <summary>
        /// Calls OnPropertyChanged on WorkingCargoPlan count values
        /// </summary>
        internal void RefreshCargoPlanValues()
        {
            OnPropertyChanged("ContainerCount");
            OnPropertyChanged("ReeferCount");
            OnPropertyChanged("DgContainerCount");
        }

        /// <summary>
        /// Method updates summary and sends a message to update the conflicts list.
        /// </summary>
        private void UpdateCargoPlanValuesAndConflicts()
        {
            DataMessenger.Default.Send(new ConflictListToBeUpdatedMessage());
            RefreshCargoPlanValues();
        }

        /// <summary>
        /// Method refreshes the wrapper, updates summary values and sends a message to update conflicts list.
        /// </summary>
        /// <param name="wrapper">The ContainerWrapper which PropertyChange resulted in change of plan.</param>
        private void UpdateCargoPlanValuesAndConflicts(ContainerWrapper wrapper)
        {
            wrapper.Refresh();
            if (wrapper.IsRf) Reefers.FindContainerByContainerNumber(wrapper).Refresh();

            DataMessenger.Default.Send(new ConflictListToBeUpdatedMessage());
            RefreshCargoPlanValues();
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
        /// Updates HoldNr property for all units in <see cref="CargoPlanWrapper"/>
        /// </summary>
        internal void UpdateCargoHoldNumbers()
        {
            HandleCargoPlanUnits.OnCargoHoldsUpdated(Model);
        }



        // -------------- Add/Remove/Modify methods ---------------------------------

        internal void AddNewContainer(Container container)
        {
            if (Model.Containers.ContainsUnitWithSameContainerNumberInList(container)) return;
            if (Containers.ContainsUnitWithSameContainerNumberInList(container)) return;

            container.HoldNr = ShipProfile.DefineCargoHoldNumber(container.Bay);

            //add to Model
            if (!Model.AddContainer(container)) return;

            var containerWrapper = new ContainerWrapper(container);
            Containers.Add(containerWrapper);
            if (container.IsRf)
                Reefers.Add(containerWrapper);

            UpdateCargoPlanValuesAndConflicts(containerWrapper);
        }

        /// <summary>
        /// Adds a Dg to WorkingCargoPlan and its Wrapper to CargoPlanWrapper
        /// </summary>
        /// <param name="dg"></param>
        internal void AddDg(Dg dg)
        {
            if (dg == null) return;

            dg.HoldNr = ShipProfile.DefineCargoHoldNumber(dg.Bay);

            this.Model.AddDg(dg);

            var containerWrapper = Containers.FindContainerByContainerNumber(dg);
            if (containerWrapper is null)
            {
                var container = Model.Containers.FindContainerByContainerNumber(dg);
                containerWrapper = new ContainerWrapper(container);
                Containers.Add(containerWrapper);
                if (container.IsRf) Reefers.Add(containerWrapper);
            }
            DgList.Add(new DgWrapper(dg));

            UpdateCargoPlanValuesAndConflicts(containerWrapper);
        }

        /// <summary>
        /// Adds a new reefer Container to WorkingCargoPlan
        /// </summary>
        /// <param name="unit">Container to be added</param>
        internal void AddNewReefer(Container unit)
        {
            //if already exists -> no action
            if (Model.Reefers.ContainsUnitWithSameContainerNumberInList(unit)) return;

            unit.HoldNr = ShipProfile.DefineCargoHoldNumber(unit.Bay);

            //add to Model
            if (!this.Model.AddReefer(unit)) return;

            //add to WorkingCargoPlan
            ContainerWrapper containerWrapper;
            if (!Containers.ContainsUnitWithSameContainerNumberInList(unit))
            {
                var container = Model.Containers.FindContainerByContainerNumber(unit);
                containerWrapper = new ContainerWrapper(container);
                Containers.Add(containerWrapper);
            }
            else
            {
                containerWrapper = Containers.FindContainerByContainerNumber(unit);
                if (containerWrapper == null) throw new ArgumentException($"Container with ContainerNumber {unit.ContainerNumber} cannot be found in CargoPlan.Containers despite it is expected");
                containerWrapper.IsRf = true;
            }
            Reefers.Add(containerWrapper);

            UpdateCargoPlanValuesAndConflicts(containerWrapper);
        }

        /// <summary>
        /// Adds the Container to Reefers list
        /// </summary>
        /// <param name="unit">Container to be added</param>
        internal void AddReefer(Container unit)
        {
            if (Model.Reefers.Contains(unit)) return;
            Model.Reefers.Add(unit);
            Reefers.Add(new ContainerWrapper(unit));
            unit.IsRf = true;

            UpdateCargoPlanValuesAndConflicts();
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
        /// Removes container from WorkingCargoPlan (also from Reefers and DgList).
        /// </summary>
        /// <param name="containerNumber">ContainerNumber of a container to be deleted.</param>
        internal void RemoveContainer(string containerNumber)
        {
            var unit = Containers.FindContainerByContainerNumber(containerNumber);

            unit.ClearSubscriptions();
            Containers.Remove(unit);
            Model.Containers.Remove(unit.Model);

            if (unit.IsRf)
            {
                var reefer = Reefers.FindContainerByContainerNumber(containerNumber);
                Reefers.Remove(reefer);
                Model.Reefers.Remove(reefer.Model);
            }

            for (int i = 0; i < DgList.Count; i++)
            {
                var d = DgList[i];

                if (d.ContainerNumber != containerNumber) continue;

                d.ClearSubscriptions();
                DgList.Remove(d);
                Model.DgList.Remove(d.Model);
                i--;
            }

            UpdateCargoPlanValuesAndConflicts();
        }

        /// <summary>
        /// Removes reefer unit from WorkingCargoPlan and Model Reefers
        /// </summary>
        /// <param name="unit">Reefer to be removed</param>
        /// <returns>Void</returns>
        internal void RemoveReefer(ContainerWrapper unit)
        {
            Reefers.Remove(Reefers.FindContainerByContainerNumber(unit));
            Model.Reefers.Remove(Model.Reefers.FindContainerByContainerNumber(unit));
        }

        /// <summary>
        /// Removes reefer unit from WorkingCargoPlan and Model Reefers.
        /// </summary>
        /// <param name="unitNumber">Reefer to be removed ContainerNumber.</param>
        /// <param name="toUpdateInCargoPlan">If required to update IsRf property in Dg and Containers.</param>
        internal void RemoveReefer(string unitNumber, bool toUpdateInCargoPlan = false)
        {
            Reefers.Remove(Reefers.FindContainerByContainerNumber(unitNumber));
            Model.Reefers.Remove(Model.Reefers.FindContainerByContainerNumber(unitNumber));

            if (toUpdateInCargoPlan)
            {
                var container = Containers.FindContainerByContainerNumber(unitNumber);
                container.Model.IsRf = false;
                container.ResetReefer();
                container.Refresh();

                foreach (var dg in DgList.Where(x => x.ContainerNumber == unitNumber))
                {
                    dg.Model.IsRf = false;
                    dg.UpdateReeferProperty();
                }

                UpdateCargoPlanValuesAndConflicts();
            }
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
        /// Reduces number of DgInContainer property of all Containers in WorkingCargoPlan
        /// </summary>
        /// <param name="containerNumber"></param>
        private void RemoveRemovedDgFromCargoPlan(IContainer unit)
        {
            var container = Containers.FindContainerByContainerNumber(unit);
            if (container is null) return;

            container?.Model.RemoveDgFromContainer();
            container?.Refresh();

            if (container.IsRf)
            {
                var reefer = Reefers.FindContainerByContainerNumber(unit);
                reefer?.Refresh();
            }
        }


        // -------------- Static methods and converters -----------------------------

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
        /// Converts this CargoPlanWrapper into a new WorkingCargoPlan
        /// </summary>
        /// <returns>New WorkingCargoPlan</returns>
        internal CargoPlan ConvertCargoPlanWrapperToPlainCargoPlan()
        {
            //Creating a new instance of WorkingCargoPlan
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
