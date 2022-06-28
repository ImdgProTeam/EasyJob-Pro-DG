using EasyJob_ProDG.Data;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using static EasyJob_ProDG.UI.Settings.UserUISettings;
using System.Diagnostics;
using System.Windows;
using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridDgViewModel : Observable
    {
        //--------------- Private fields --------------------------------------------
        private SettingsService uiSettings;
        private IMessageDialogService _messageDialogService => MessageDialogService.Connect();
        private readonly CollectionViewSource dgPlanView = new CollectionViewSource();

        private const byte totalNumberOfColumns = 44;

        //--------------- Public static properties ----------------------------------
        public static IList<char> StowageCategories => new List<char>() { 'A', 'B', 'C', 'D', 'E' };

        //--------------- Public properties -----------------------------------------
        public CargoPlanWrapper CargoPlan
        {
            get { return ViewModelLocator.MainWindowViewModel.WorkingCargoPlan; }
        }
        public ObservableCollection<DgTableColumnSettings> ColumnSettings { get; set; }

        /// <summary>
        /// Used for DgDataGrid binding 
        /// </summary>
        public ICollectionView DgPlanView => dgPlanView?.View;
        public DgWrapper SelectedDg { get; set; }
        public List<DgWrapper> SelectedDgArray { get; set; }
        public DgSortOrderPattern DgSortOrderDirection { get; set; }
        public bool IsTechnicalNameIncluded { get; set; }


        //--------------- Constructor -----------------------------------------------
        public DataGridDgViewModel()
        {
            LoadServices();

            RegisterInDataMessenger();

            LoadColumnSettings();

            LoadCommands();

            SetDataView();

            SetVisualElements();

            dgPlanView.Filter += OnDgListFiltered;

        }


        #region StartUp Logic

        /// <summary>
        /// Initiates required services
        /// </summary>
        private void LoadServices()
        {
            uiSettings = new SettingsService();
        }

        /// <summary>
        /// Registers for messages in DataMessenger
        /// </summary>
        private void RegisterInDataMessenger()
        {
            DataMessenger.Default.Register<ApplicationClosingMessage>(this, OnApplicationClosingMessageReceived, "closing");
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnCargoDataUpdated, "cargodataupdated");
            DataMessenger.Default.Register<ConflictPanelItemViewModel>(this, OnConflictSelectionChanged,
                "conflict selection changed");
        }

        /// <summary>
        /// Loads column settings from file for DgDataGrid.
        /// Number of settings hard coded.
        /// </summary>
        private void LoadColumnSettings()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DgTableColumnSettings>));
                using (Stream s = File.OpenRead(ProgramDefaultSettingValues.ProgramDirectory + "columnsettings.xml"))
                    ColumnSettings = (ObservableCollection<DgTableColumnSettings>)xs.Deserialize(s);

                if (ColumnSettings.Count != totalNumberOfColumns)
                    throw new Exception("Number of columns read from settings file is wrong.");
                Debug.WriteLine("----> Column settings successfully loaded.");
            }
            catch (Exception e)
            {
                Debug.WriteLine("---> Number of columns read from settings file is wrong.");
                ColumnSettings = new ObservableCollection<DgTableColumnSettings>();
                for (int i = 0; i < totalNumberOfColumns; i++)
                {
                    ColumnSettings.Add(new DgTableColumnSettings(i));
                }
                Debug.WriteLine("----> Default column settings created.");
            }
        }

        /// <summary>
        /// Assigns handler methods for commands
        /// </summary>
        private void LoadCommands()
        {
            AddDgCommand = new DelegateCommand(OnAddDg);
            DeleteDg = new DelegateCommand(OnDgDeleteRequested);
            IncludeTechnicalNameCommand = new DelegateCommand(IncludeTechnicalNameOnExecuted);
            DisplayAddDgMenuCommand = new DelegateCommand(OnDisplayAddDgMenu);
            SelectionChangedCommand = new DelegateCommand(OnSelectionChanged);
        }

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        private void SetDataView()
        {
            dgPlanView.Source = CargoPlan.DgList;
        }

        /// <summary>
        /// Sets required properties values of various visual elements
        /// </summary>
        private void SetVisualElements()
        {
            SetInitialAddMenuProperties();
        }


        #endregion

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
                DgPlanView.Refresh();
            }
        }

        /// <summary>
        /// Implements logic to filter content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDgListFiltered(object sender, FilterEventArgs e)
        {
            // Checks section

            if (string.IsNullOrEmpty(textToFilter)) return;

            if (!(e.Item is DgWrapper dg) || dg.ContainerNumber is null)
            {
                e.Accepted = false;
                return;
            }

            //Logic section

            var searchText = textToFilter.ToLower().Replace(" ", "");

            if (dg.ContainerNumber.ToLower().Contains(searchText)) return;
            if (dg.Unno.ToString().Contains(searchText)) return;
            if (dg.Location.Replace(" ", "").Contains(searchText)) return;

            e.Accepted = false;
        }
        #endregion

        #region AddDg Logic
        public bool CanUserAddDg => !string.IsNullOrEmpty(DgToAddNumber) && DgToAddUnno > 0;

        string dgToAddNumber;
        public string DgToAddNumber
        {
            get => dgToAddNumber;
            set
            {
                dgToAddNumber = value?.Trim();
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanUserAddDg));
            }
        }

        string dgToAddLocation;
        public string DgToAddLocation
        {
            get => dgToAddLocation;
            set
            {
                dgToAddLocation = value.LimitMaxContainerLocationInput();
                OnPropertyChanged();
            }
        }

        ushort dgToAddUnno;
        public ushort DgToAddUnno
        {
            get => dgToAddUnno;
            set
            {
                dgToAddUnno = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanUserAddDg));
            }
        }

        /// <summary>
        /// Used to set visibility of AddMenu
        /// </summary>
        public Visibility MenuVisibility { get; set; }

        private void OnAddDg(object obj)
        {
            //Correct location
            string location = dgToAddLocation.CorrectFormatContainerLocation();

            //Existing unno
            if (!DataHelper.CheckForExistingUnno(dgToAddUnno))
                return;

            //Action
            CargoPlan.AddDg(new Model.Cargo.Dg()
            {
                Unno = dgToAddUnno,
                ContainerNumber = dgToAddNumber,
                Location = location
            }); ;

            //Scroll into the new Container
            SelectedDg = CargoPlan.DgList[CargoPlan.DgList.Count - 1];
            OnPropertyChanged(nameof(SelectedDg));
        }

        /// <summary>
        /// Actions on displaying AddDg menu (on click 'Add' button)
        /// </summary>
        /// <param name="obj"></param>
        internal void OnDisplayAddDgMenu(object obj = null)
        {
            var container = (Model.Cargo.Container)obj;

            DgToAddNumber = container == null ? SelectedDg?.ContainerNumber : container.ContainerNumber;
            DgToAddLocation = container == null ? SelectedDg?.Location : container.Location;

            MenuVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(MenuVisibility));
        }

        /// <summary>
        /// Sets initial view properties of AddDg menu
        /// </summary>
        private void SetInitialAddMenuProperties()
        {
            MenuVisibility = Visibility.Collapsed;
        }
        #endregion

        #region Private methods
        //--------------- Private methods -------------------------------------------

        /// <summary>
        /// Includes or removes TechnicalName to ProperShippingName of all Dg in CargoPlan
        /// </summary>
        /// <param name="obj"></param>
        private void IncludeTechnicalNameOnExecuted(object obj)
        {
            if (IsTechnicalNameIncluded)
            {
                foreach (var dg in CargoPlan.DgList)
                {
                    dg.RemoveTechnicalName();
                }

                IsTechnicalNameIncluded = false;
            }
            else
            {
                foreach (var dg in CargoPlan.DgList)
                {
                    dg.IncludeTechnicalName();
                }

                IsTechnicalNameIncluded = true;
            }
            OnPropertyChanged("IsTechnicalNameIncluded");
        }

        /// <summary>
        /// Requests user weather to delete selected dg(s) and sends message to CargoPlan respectively
        /// </summary>
        /// <param name="obj"></param>
        private void OnDgDeleteRequested(object obj)
        {
            if (SelectedDg == null) return;

            if (_messageDialogService.ShowYesNoDialog($"Do you want to delete selected Dg(s) ({((ICollection)obj).Count})?", "Delete cargo")
                == MessageDialogResult.No) return;

            List<DgWrapper> selectedDgArray = new List<DgWrapper>();
            foreach (var data in (ICollection)obj)
            {
                var dg = data as DgWrapper;
                selectedDgArray.Add(dg);
            }

            DataMessenger.Default.Send<UpdateCargoPlan>(new UpdateCargoPlan(selectedDgArray), "Remove dg");
        }

        /// <summary>
        /// Method changes SelectedDg to match with ConflictPanelItem object
        /// </summary>
        /// <param name="obj">Selected conflict</param>
        private void OnConflictSelectionChanged(ConflictPanelItemViewModel obj)
        {
            if (obj == null)
                return;

            //CLear selection
            SelectedDg = null;
            OnPropertyChanged("SelectedDg");

            //Set new selection
            foreach (DgWrapper dg in DgPlanView)
            {
                if (dg.Model.ID == obj.DgID)
                {
                    SelectedDg = dg;
                    break;
                }
            }
            OnPropertyChanged("SelectedDg");
        }

        /// <summary>
        /// Invokes OnPropertyChanged method for relevant properties.
        /// </summary>
        /// <param name="obj">none</param>
        private void OnCargoDataUpdated(CargoDataUpdated obj)
        {
            SetDataView();
            OnPropertyChanged($"CargoPlan");
            OnPropertyChanged("DgPlanView");
        }

        private void OnSelectionChanged(object obj)
        {
            if (SelectedDg is null) return;

            if (MenuVisibility == Visibility.Visible)
            {
                if (SelectedDg.ContainerNumber != DgToAddNumber)
                {
                    DgToAddNumber = SelectedDg?.ContainerNumber;
                    DgToAddLocation = SelectedDg?.Location;
                }
            }

        }

        /// <summary>
        /// Contains logic to be performed before closing the application
        /// </summary>
        /// <param name="obj"></param>
        private void OnApplicationClosingMessageReceived(ApplicationClosingMessage obj)
        {
            WriteColumnSettings();
        }

        /// <summary>
        /// Saves current DgDataGrid column settings on disk
        /// </summary>
        private void WriteColumnSettings()
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DgTableColumnSettings>));
            using (Stream s = File.Create(ProgramDefaultSettingValues.ProgramDirectory + "columnsettings.xml"))
            {
                xs.Serialize(s, ColumnSettings);
            }
        }
        #endregion


        #region Commands
        //--------------- Commands --------------------------------------------------

        public ICommand SelectionChangedCommand { get; private set; }
        public ICommand IncludeTechnicalNameCommand { get; set; }
        public ICommand ToExcel { get; private set; }
        public ICommand DeleteDg { get; private set; }
        public ICommand AddDgCommand { get; private set; }
        public ICommand DisplayAddDgMenuCommand { get; private set; }
        #endregion

        #region Events
        //--------------- Events ----------------------------------------------------

        public delegate void SelectionChanged(object obj);
        public static event SelectionChanged OnSelectionChangedEventHandler = null;
        #endregion
    }



}
