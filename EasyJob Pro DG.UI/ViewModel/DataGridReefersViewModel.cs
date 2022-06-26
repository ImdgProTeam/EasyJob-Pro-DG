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
                string newValue = value?.Replace(" ", "");

                if (newValue?.Length > 6 && int.Parse(newValue) > 2559999)
                {
                    reeferToAddLocation = newValue?.Substring(1);
                }
                else
                    reeferToAddLocation = value?.Trim();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Used to set visibility of AddMenu
        /// </summary>
        public System.Windows.Visibility MenuVisibility { get; set; }

        private void OnAddReefer(object obj)
        {
            //Correct location
            string location;

            if (string.IsNullOrEmpty(reeferToAddLocation))
                location = "000000";
            else if (reeferToAddLocation.Length < 5)
                location = "0000" + reeferToAddLocation;
            else location = reeferToAddLocation;

            //Action
            CargoPlan.AddNewReefer(new Model.Cargo.Container()
            {
                ContainerNumber = reeferToAddNumber,
                Location = location
            }); ;
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

        public ICommand AddReeferCommand { get; private set; }
        public ICommand DisplayAddReeferMenuCommand { get; private set; }
        #endregion

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
            AddReeferCommand = new DelegateCommand(OnAddReefer);
            DisplayAddReeferMenuCommand = new DelegateCommand(OnDisplayAddReeferMenu);
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

    }
}
