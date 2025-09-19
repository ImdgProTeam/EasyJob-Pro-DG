using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Utility.Messages;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace EasyJob_ProDG.UI.ViewModel
{
    public abstract class DataGridViewModelBase : Observable
    {
        // ----- Private and protected fields -----

        private readonly CollectionViewSource unitsPlanView = new CollectionViewSource();

        protected object selectionObject;
        protected IMessageDialogService _messageDialogService => MessageDialogService.Connect();
        protected Dispatcher dispatcher;


        // ----- Public properties -----

        /// <summary>
        /// Used for DataGrid binding 
        /// </summary>
        public ICollectionView UnitsPlanView => unitsPlanView?.View;
        public CargoPlanWrapper WorkingCargoPlan => ViewModelLocator.MainWindowViewModel.WorkingCargoPlan;
        public string StatusBarText { get; protected set; } = "None";

        /// <summary>
        /// Property used to selectmultiple items from ViewModel by Id or ContainerNumber
        /// </summary>
        public List<string> ItemsToSelect
        {
            get => itemsToSelect;
            set
            {
                itemsToSelect = value;
                OnPropertyChanged();
            }
        }
        private List<string> itemsToSelect;

        #region Constructor
        // ----- Constructor -----

        public DataGridViewModelBase()
        {
            dispatcher = Dispatcher.CurrentDispatcher;

            LoadServices();

            RegisterBaseInDataMessenger();
            RegisterInDataMessenger();

            LoadBaseCommands();
            LoadCommands();

            SetDataView();
            SetVisualElements();

            unitsPlanView.Filter += OnUnitsListFiltered;
            unitsPlanView.Filter += OnAdvanceFiltered;
        }

        #endregion

        #region Startup Logic

        protected abstract void SetDataView();

        /// <summary>
        /// Used to set up View.Sourse for DataGrid from an inherited class
        /// </summary>
        /// <param name="unitsPlan"></param>
        protected void SetPlanViewSource(IEnumerable unitsPlan)
        {
            unitsPlanView.Source = unitsPlan;
        }

        private void LoadBaseCommands()
        {
            SelectionChangedCommand = new DelegateCommand(OnSelectionChanged);
            ClearFilterCommand = new DelegateCommand(ClearFilterExecuted);
        }

        private void RegisterBaseInDataMessenger()
        {
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnCargoDataUpdated, "cargodataupdated");
            DataMessenger.Default.Register<CargoPlanUnitPropertyChanged>(this, OnCargoPlanUnitPropertyChanged);
        }

        protected virtual void LoadServices() { }
        protected virtual void LoadCommands() { }
        protected virtual void RegisterInDataMessenger() { }

        /// <summary>
        /// Sets required properties values of various visual elements
        /// </summary>
        private void SetVisualElements()
        {
            SetInitialAddMenuProperties();
        }

        #endregion

        #region Filter logic
        // ----- Filter logic -----

        protected string textToFilter;

        public string TextToFilter
        {
            get { return textToFilter; }
            set
            {
                if (textToFilter == value) return;
                textToFilter = value;
                UnitsPlanView.Refresh();
            }
        }

        protected virtual void OnUnitsListFiltered(object sender, FilterEventArgs e)
        {
            // Checks section

            if (string.IsNullOrEmpty(textToFilter)) return;

            if (!(e.Item is ContainerWrapper c) || c.ContainerNumber is null)
            {
                e.Accepted = false;
                return;
            }

            //Logic section

            var searchText = textToFilter.ToLower().Replace(" ", "");

            if (c.ContainerNumber.ToLower().Contains(searchText)) return;
            if (c.Location.Replace(" ", "").Contains(searchText)) return;

            e.Accepted = false;
        }

        protected List<string> filteredContainerNumbers;
        public bool AdvancedFilterApplied { get; private set; }

        /// <summary>
        /// Applies advanced filter to UnitsPlanView based on List of ContainerNumbers as set in <see cref="filteredContainerNumbers"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAdvanceFiltered (object sender, FilterEventArgs e)
        {
            if (filteredContainerNumbers == null) return;

            if (!(e.Item is ContainerWrapper c) || c.ContainerNumber is null)
            {
                e.Accepted = false;
                return;
            }
            if (filteredContainerNumbers.Contains(c.ContainerNumber)) return;

            e.Accepted = false;
        }

        /// <summary>
        /// Sets additional filter to UnitsPlanView
        /// </summary>
        /// <param name="filteredItems">List of filtered container numbers</param>
        internal void SetAdditionalFilter(List<string> filteredItems)
        {
            filteredContainerNumbers = filteredItems;
            UnitsPlanView.Refresh();

            AdvancedFilterApplied = true;
            OnPropertyChanged(nameof(AdvancedFilterApplied));
        }

        /// <summary>
        /// Clears additinal filter
        /// </summary>
        internal void ClearAdditionalFilter()
        {
            filteredContainerNumbers = null;
            UnitsPlanView?.Refresh();

            AdvancedFilterApplied = false;
            OnPropertyChanged(nameof(AdvancedFilterApplied));
        }

        private void ClearFilterExecuted(object obj)
        {
            ClearAdditionalFilter();

            // message will update FilterTool if open
            DataMessenger.Default.Send(new ChangeSelectionMessage(), "selected data grid changed");
        }

        #endregion

        #region AddUnitLogic

        public virtual bool CanUserAddUnit => !string.IsNullOrEmpty(UnitToAddNumber);

        protected string unitToAddNumber;
        public string UnitToAddNumber
        {
            get => unitToAddNumber;
            set
            {
                unitToAddNumber = value?.Trim();
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanUserAddUnit));
            }
        }

        protected string unitToAddLocation;
        public string UnitToAddLocation
        {
            get => unitToAddLocation;
            set
            {
                unitToAddLocation = value.LimitMaxContainerLocationInput();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Used to set visibility of AddMenu
        /// </summary>
        public System.Windows.Visibility MenuVisibility { get; set; }

        /// <summary>
        /// Sets initial view properties of Add menu
        /// </summary>
        private void SetInitialAddMenuProperties()
        {
            MenuVisibility = System.Windows.Visibility.Collapsed;
        }

        protected abstract void OnAddNewUnit(object obj);

        #endregion

        #region Methods
        // ----- DataGrid interactions -----

        /// <summary>
        /// Called when selection in DataGrid changes
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnSelectionChanged(object obj)
        {
            SetSelectionStatusBar(obj);
            DataMessenger.Default.Send(new ChangeSelectionMessage(), "Selected unit changed");
        }

        protected virtual void SetSelectionStatusBar(object selectionObject)
        {
            StatusBarText = SelectionStatusBarSetter.GetSelectionStatusBarTextForContainer(selectionObject);
            OnPropertyChanged(nameof(StatusBarText));
        }


        // ----- Message methods -----

        /// <summary>
        /// Invokes OnPropertyChanged method for relevant properties.
        /// </summary>
        /// <param name="obj">none</param>
        private void OnCargoDataUpdated(CargoDataUpdated obj)
        {
            dispatcher.Invoke(() =>
            {
                SetDataView();
                OnPropertyChanged(nameof(WorkingCargoPlan));
                OnPropertyChanged(nameof(UnitsPlanView));
            });
            PostCargoDataUpdated();
        }

        /// <summary>
        /// Method will be called in the end of OnCargoDataUpdated method.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual void PostCargoDataUpdated()
        {
        }

        private void OnCargoPlanUnitPropertyChanged(CargoPlanUnitPropertyChanged changed)
        {
            SetSelectionStatusBar(selectionObject);
        }

        #endregion

        #region Commands
        //--------------- Commands ----------------------------------------
        public ICommand SelectionChangedCommand { get; private set; }

        public ICommand ClearFilterCommand { get; private set; }

        #endregion
    }
}
