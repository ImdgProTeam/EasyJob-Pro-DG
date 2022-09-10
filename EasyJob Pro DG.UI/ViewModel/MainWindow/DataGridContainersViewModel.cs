using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridContainersViewModel : Observable
    {
        //--------------- Private fields --------------------------------------------
        SettingsService uiSettings;
        IMessageDialogService _messageDialogService => MessageDialogService.Connect();
        private readonly CollectionViewSource containerPlanView = new CollectionViewSource();
        Dispatcher dispatcher;

        //--------------- Public properties -----------------------------------------
        public CargoPlanWrapper CargoPlan
        {
            get { return ViewModelLocator.MainWindowViewModel.WorkingCargoPlan; }
        }

        /// <summary>
        /// Used for ContainerDataGrid binding 
        /// </summary>
        public ICollectionView ContainerPlanView => containerPlanView?.View;
        public ContainerWrapper SelectedContainer { get; set; }
        public List<ContainerWrapper> SelectedContainerArray { get; set; }
        public UserUISettings.DgSortOrderPattern ContainerSortOrderDirection { get; set; }



        // ---------- Constructor ---------------
        public DataGridContainersViewModel()
        {
            dispatcher = Dispatcher.CurrentDispatcher;

            SetDataView();
            RegisterInDataMessenger();
            LoadCommands();
            SetVisualElements();
            containerPlanView.Filter += OnContainerListFiltered;
        }

        #region Filter Logic
        // ----------- Filter logic ----------------
        private string textToFilter;

        public string TextToFilter
        {
            get { return textToFilter; }
            set
            {
                if (textToFilter == value) return;
                textToFilter = value;
                ContainerPlanView.Refresh();
            }
        }

        /// <summary>
        /// Implements logic to filter content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContainerListFiltered(object sender, FilterEventArgs e)
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
            if (c.Location.Replace(" ","").Contains(searchText)) return;

            e.Accepted = false;
        }
        #endregion

        #region AddContainer Logic
        public bool CanUserAddContainer => !string.IsNullOrEmpty(ContainerToAddNumber);

        string containerToAddNumber;
        public string ContainerToAddNumber
        {
            get => containerToAddNumber;
            set
            {
                containerToAddNumber = value?.Trim();
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanUserAddContainer));
            }
        }

        string containerToAddLocation;
        public string ContainerToAddLocation
        {
            get => containerToAddLocation;
            set
            {
                containerToAddLocation = value.LimitMaxContainerLocationInput();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Used to set visibility of AddMenu
        /// </summary>
        public System.Windows.Visibility MenuVisibility { get; set; }

        /// <summary>
        /// Method changes SelectedContainer to match with the selected number (e.g. with ConflictPanelItem object)
        /// </summary>
        /// <param name="obj">Selected container number</param>
        internal void SelectContainer(string containerNumber)
        {
            SelectedContainer = null;
            OnPropertyChanged(nameof(SelectedContainer));

            //Set new selection
            foreach (ContainerWrapper container in ContainerPlanView)
            {
                if (string.Equals(container.ContainerNumber, containerNumber))
                {
                    SelectedContainer = container;
                    break;
                }
            }
            OnPropertyChanged(nameof(SelectedContainer));
        }

        private void OnAddNewContainer(object obj)
        {
            //Correct location
            string location = containerToAddLocation.CorrectFormatContainerLocation();

            //Action
            CargoPlan.AddNewContainer(new Model.Cargo.Container()
            {
                ContainerNumber = containerToAddNumber,
                Location = location
            }); ;

            //Scroll into the new Container
            SelectedContainer = CargoPlan.Containers[CargoPlan.Containers.Count - 1];
            OnPropertyChanged(nameof(SelectedContainer));
        }

        /// <summary>
        /// Actions on displaying AddDg menu (on click 'Add' button)
        /// </summary>
        /// <param name="obj"></param>
        internal void OnDisplayAddContainerMenu(object obj = null)
        {
            ContainerToAddNumber = SelectedContainer?.ContainerNumber;
            ContainerToAddLocation = SelectedContainer?.Location;

            MenuVisibility = System.Windows.Visibility.Visible;
            OnPropertyChanged(nameof(MenuVisibility));
        }

        /// <summary>
        /// Sets initial view properties of AddDg menu
        /// </summary>
        private void SetInitialAddMenuProperties()
        {
            MenuVisibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Switches to add a new Dg in DataGridDg with selected container number.
        /// </summary>
        /// <param name="obj"></param>
        private void OnAddNewDg(object obj)
        {
           ViewModelLocator.MainWindowViewModel.AddNewDgCommand.Execute(obj);
        }


        private bool CanAddNewDg(object obj)
        {
            return SelectedContainer != null;
        }

        #endregion

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        private void SetDataView()
        {

            containerPlanView.Source = CargoPlan.Containers;
        }

        /// <summary>
        /// Sets required properties values of various visual elements
        /// </summary>
        private void SetVisualElements()
        {
            SetInitialAddMenuProperties();
        }

        /// <summary>
        /// Assigns handler methods for commands
        /// </summary>
        private void LoadCommands()
        {
            AddNewContainerCommand = new DelegateCommand(OnAddNewContainer);
            AddNewDgCommand = new DelegateCommand(OnAddNewDg, CanAddNewDg);
            DisplayAddContainerMenuCommand = new DelegateCommand(OnDisplayAddContainerMenu);
            SelectionChangedCommand = new DelegateCommand(OnSelectionChanged);
            DeleteContainerCommand = new DelegateCommand(OnDeleteContainersRequested);
        }

        private void OnDeleteContainersRequested(object obj)
        {
            if (SelectedContainer == null) return;
            var count = ((ICollection)obj).Count;

            if (_messageDialogService.ShowYesNoDialog($"Do you want to delete selected container" + (count > 1 ? $"s ({count})" : "") + "?", "Delete cargo")
                == MessageDialogResult.No) return;

            var list = new List<string>();
            foreach (ContainerWrapper item in (ICollection)obj)
            {
                list.Add(item.ContainerNumber);
            }

            foreach (var number in list)
            {
                CargoPlan.RemoveContainer(number);
            }
        }

        /// <summary>
        /// Invokes OnPropertyChanged method for relevant properties.
        /// </summary>
        /// <param name="obj">none</param>
        private void OnCargoDataUpdated(CargoDataUpdated obj)
        {
            dispatcher.Invoke(() =>
            {
                SetDataView();
                OnPropertyChanged($"CargoPlan");
                OnPropertyChanged("ContainerPlanView");
            });
        }

        /// <summary>
        /// Registers for messages in DataMessenger
        /// </summary>
        private void RegisterInDataMessenger()
        {
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnCargoDataUpdated, "cargodataupdated");
        }

        private void OnSelectionChanged(object obj)
        {
            if (SelectedContainer is null) return;

            if (MenuVisibility == System.Windows.Visibility.Visible)
            {
                if (SelectedContainer.ContainerNumber != containerToAddNumber)
                {
                    ContainerToAddNumber = SelectedContainer?.ContainerNumber;
                    ContainerToAddLocation = SelectedContainer?.Location;
                }
            }
        }

        #region Commands
        //--------------- Commands ----------------------------------------
        public ICommand SelectionChangedCommand { get; private set; }
        public ICommand AddNewContainerCommand { get; private set; }
        public ICommand AddNewDgCommand { get; private set; }
        public ICommand DisplayAddContainerMenuCommand { get; private set; }
        public ICommand DeleteContainerCommand { get; private set; }
        #endregion
    }
}
