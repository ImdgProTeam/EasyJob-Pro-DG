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
        protected bool _isRefreshing;

        // ----- Public properties -----

        /// <summary>
        /// Used for DataGrid binding 
        /// </summary>
        public ICollectionView UnitsPlanView => unitsPlanView?.View;
        public CargoPlanWrapper WorkingCargoPlan => ViewModelLocator.MainWindowViewModel.WorkingCargoPlan;
        public string StatusBarText { get; protected set; } = "Selected: None";

        /// <summary>
        /// Provides access to selectionObject containing selected units
        /// </summary>
        /// <returns></returns>
        internal IList<object> GetSelectionObjectList() => (IList<object>)selectionObject;

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

                _isRefreshing = true;
                UnitsPlanView?.Refresh();
                _isRefreshing = false;

                //filter text input
                if (!string.IsNullOrEmpty(textToFilter))
                {
                    IsFilterApplied = true;
                    SetStatusBarOnFilter();
                }
                //both search filter and advanced filters are clear
                else if (filteredContainerNumbers is null || filteredContainerNumbers.Count == 0)
                {
                    IsFilterApplied = false;
                    SetStatusBarOnFilterCleared();
                }
                //advanced filter not clear
                else
                    SetStatusBarOnFilter();

                OnPropertyChanged(nameof(IsFilterApplied));
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
            if(isPOLIncluded && c.POL.Replace(" ","").ToLower().Contains(searchText)) return;
            if(isPODIncluded && c.POD.Replace(" ","").ToLower().Contains(searchText)) return;

            e.Accepted = false;
        }

        protected List<string> filteredContainerNumbers;
        public bool IsFilterApplied { get; private set; }

        /// <summary>
        /// Applies advanced filter to UnitsPlanView based on List of ContainerNumbers as set in <see cref="filteredContainerNumbers"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAdvanceFiltered(object sender, FilterEventArgs e)
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
            UnitsPlanView?.Refresh();

            IsFilterApplied = true;
            OnPropertyChanged(nameof(IsFilterApplied));

            SetStatusBarOnFilter();
        }

        /// <summary>
        /// Clears additinal filter
        /// </summary>
        internal void ClearFilters()
        {
            textToFilter = null;
            OnPropertyChanged(nameof(TextToFilter));

            filteredContainerNumbers = null;

            ((IEditableCollectionView)UnitsPlanView)?.CommitEdit(); // clears edit mode to enable .Refresh()
            UnitsPlanView?.Refresh();

            IsFilterApplied = false;
            OnPropertyChanged(nameof(IsFilterApplied));

            SetStatusBarOnFilterCleared();
        }

        private void ClearFilterExecuted(object obj)
        {
            ClearFilters();

            // message will update FilterTool if open
            DataMessenger.Default.Send(new ChangeSelectionMessage(), "selected data grid changed");
        }

        #endregion

        #region SearchBox options

        protected static bool isPOLIncluded;
        protected static bool isPODIncluded;

        public bool IsPOLIncluded
        { get => isPOLIncluded;
            set
            {
                isPOLIncluded = value;
                ReSetTextToFilter();
            }
        }

        public bool IsPODIncluded 
        { 
            get => isPODIncluded;
            set
            {
                isPODIncluded = value;
                ReSetTextToFilter();
            }
        }

        private void ReSetTextToFilter()
        {
            var temp = textToFilter;
            textToFilter = string.Empty;
            TextToFilter = temp;
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

        /// <summary>
        /// Sets StatusBar test upon filter or clear filter.
        /// </summary>
        protected virtual void SetStatusBarOnFilter()
        {
            StatusBarText = $"Filtered: {((CollectionView)(unitsPlanView.View)).Count} units";
            OnPropertyChanged(nameof(StatusBarText));
        }

        private void SetStatusBarOnFilterCleared()
        {
            StatusBarText = "No filter applied.";
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
