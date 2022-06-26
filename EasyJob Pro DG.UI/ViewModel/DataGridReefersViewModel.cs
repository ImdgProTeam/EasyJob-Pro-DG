using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridReefersViewModel : Observable
    {
        //--------------- Private fields --------------------------------------------
        SettingsService uiSettings;
        IMessageDialogService _messageDialogService;
        private readonly CollectionViewSource reeferPlanView = new CollectionViewSource();

        //--------------- Public properties -----------------------------------------
        public CargoPlanWrapper CargoPlan
        {
            get { return ViewModelLocator.MainWindowViewModel.WorkingCargoPlan; }
            set
            {
            }
        }
        /// <summary>
        /// Used for ReeferDataGrid binding 
        /// </summary>
        public ICollectionView ReeferPlanView => reeferPlanView?.View;
        public ContainerWrapper SelectedReefer { get; set; }
        public List<ContainerWrapper> SelectedReeferArray { get; set; }
        public UserUISettings.DgSortOrderPattern ReeferSortOrderDirection { get; set; }


        // ---------- Constructor ---------------
        public DataGridReefersViewModel()
        {
            SetDataView();
            RegisterInDataMessenger();
            LoadCommands();
            SetVisualElements();

            reeferPlanView.Filter += OnReeferListFiltered;
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
                ReeferPlanView.Refresh();
            }
        }

        /// <summary>
        /// Implements logic to filter content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReeferListFiltered(object sender, FilterEventArgs e)
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
        #endregion

        #region AddReefer Logic
        public bool CanUserAddReefer => !string.IsNullOrEmpty(ReeferToAddNumber);

        string reeferToAddNumber;
        public string ReeferToAddNumber
        {
            get => reeferToAddNumber;
            set
            {
                reeferToAddNumber = value?.Trim();
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanUserAddReefer));
            }
        }

        string reeferToAddLocation;
        public string ReeferToAddLocation
        {
            get => reeferToAddLocation;
            set
            {
                reeferToAddLocation = value.LimitMaxContainerLocationInput();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Used to set visibility of AddMenu
        /// </summary>
        public System.Windows.Visibility MenuVisibility { get; set; }

        private void OnAddNewReefer(object obj)
        {
            //Correct location
            string location = reeferToAddLocation.CorrectFormatContainerLocation();

            //Action
            CargoPlan.AddNewReefer(new Model.Cargo.Container()
            {
                ContainerNumber = reeferToAddNumber,
                Location = location
            });

            //Scroll into the new Container
            SelectedReefer = CargoPlan.Reefers[CargoPlan.Reefers.Count - 1];
            OnPropertyChanged(nameof(SelectedReefer));
        }

        /// <summary>
        /// Actions on displaying AddDg menu (on click 'Add' button)
        /// </summary>
        /// <param name="obj"></param>
        internal void OnDisplayAddReeferMenu(object obj = null)
        {
            ReeferToAddNumber = SelectedReefer?.ContainerNumber;
            ReeferToAddLocation = SelectedReefer?.Location;

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

        #endregion

        #region Private methods
        //-------------- Private methods --------------------------------------------
        /// <summary>
        /// Sets data source to View property
        /// </summary>
        private void SetDataView()
        {
            reeferPlanView.Source = CargoPlan.Reefers;
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
            AddNewReeferCommand = new DelegateCommand(OnAddNewReefer);
            DisplayAddReeferMenuCommand = new DelegateCommand(OnDisplayAddReeferMenu);
            SelectionChangedCommand = new DelegateCommand(OnSelectionChanged);
        }

        /// <summary>
        /// Invokes OnPropertyChanged method for relevant properties.
        /// </summary>
        /// <param name="obj">none</param>
        private void OnCargoDataUpdated(CargoDataUpdated obj)
        {
            SetDataView();
            OnPropertyChanged($"CargoPlan");
            OnPropertyChanged("ReeferPlanView");
            ReeferPlanView.Refresh();
        }

        /// <summary>
        /// Updates reeferView update after import manifest info
        /// </summary>
        /// <param name="obj"></param>
        private void OnReeferInfoUpdated(CargoDataUpdated obj)
        {
            SetDataView();
            OnPropertyChanged("ReeferPlanView");
            ReeferPlanView.Refresh();
        }
        /// <summary>
        /// Registers for messages in DataMessenger
        /// </summary>
        private void RegisterInDataMessenger()
        {
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnCargoDataUpdated, "cargodataupdated");
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnReeferInfoUpdated, "reeferinfoupdated");
        }

        private void OnSelectionChanged(object obj)
        {
            if (SelectedReefer is null) return;

            if (MenuVisibility == System.Windows.Visibility.Visible)
            {
                if (SelectedReefer.ContainerNumber != reeferToAddNumber)
                {
                    ReeferToAddNumber = SelectedReefer?.ContainerNumber;
                    ReeferToAddLocation = SelectedReefer?.Location;
                }
            }
        } 
        #endregion


        #region Commands
        //--------------- Commands ----------------------------------------
        public ICommand SelectionChangedCommand { get; private set; }
        public ICommand AddNewReeferCommand { get; private set; }
        public ICommand DisplayAddReeferMenuCommand { get; private set; } 
        #endregion
    }
}
