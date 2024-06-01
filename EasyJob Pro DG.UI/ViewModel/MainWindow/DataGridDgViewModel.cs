using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel.MainWindow;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridDgViewModel : Observable
    {
        //--------------- Private fields --------------------------------------------
        private SettingsService uiSettings;
        private IMessageDialogService _messageDialogService => MessageDialogService.Connect();
        private readonly CollectionViewSource dgPlanView = new CollectionViewSource();
        Dispatcher dispatcher;

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
        public DgWrapper SelectedDg
        {
            get;
            set;
        }
        private object _selectionObject;
        public string StatusBarText { get; private set; } = "None";
        public bool IsTechnicalNameIncluded { get; set; }


        //--------------- Constructor -----------------------------------------------
        public DataGridDgViewModel()
        {
            dispatcher = Dispatcher.CurrentDispatcher;

            LoadServices();

            RegisterInDataMessenger();

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
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnCargoDataUpdated, "cargodataupdated");
            DataMessenger.Default.Register<CargoPlanUnitPropertyChanged>(this, OnCargoPlanUnitPropertyChanged);
            DataMessenger.Default.Register<DgListSelectedItemUpdatedMessage>(this, OnCargoPlanSelectedItemUpdatedMessage, "selectionpropertyupdated");

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

        #region Public methods
        //--------------- Public methods -------------------------------------------

        /// <summary>
        /// Method changes SelectedDg to match with the selected DgID (e.g. with ConflictPanelItem object)
        /// </summary>
        /// <param name="obj">Selected dg id</param>
        internal void SelectDg(int id)
        {
            SelectedDg = null;
            OnPropertyChanged(nameof(SelectedDg));

            //Set new selection
            foreach (DgWrapper dg in DgPlanView)
            {
                if (dg.Model.ID == id)
                {
                    SelectedDg = dg;
                    break;
                }
            }
            OnPropertyChanged(nameof(SelectedDg));
        }

        #endregion

        #region Private methods
        //--------------- Private methods -------------------------------------------

        /// <summary>
        /// Includes or removes TechnicalName to ProperShippingName of all Dg in WorkingCargoPlan
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
            OnPropertyChanged(nameof(IsTechnicalNameIncluded));
        }

        /// <summary>
        /// Requests user weather to delete selected dg(s) and sends message to WorkingCargoPlan respectively
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
        /// Invokes OnPropertyChanged method for relevant properties.
        /// </summary>
        /// <param name="obj">none</param>
        private void OnCargoDataUpdated(CargoDataUpdated obj)
        {
            dispatcher.Invoke(() =>
            {
                SetDataView();
                OnPropertyChanged(nameof(CargoPlan));
                OnPropertyChanged(nameof(DgPlanView));
            });
        }

        private void OnSelectionChanged(object obj)
        {
            SetSelectionStatusBar(obj);

            if (SelectedDg is null) return;

            if (MenuVisibility == Visibility.Visible)
            {
                if (SelectedDg.ContainerNumber != DgToAddNumber)
                {
                    DgToAddNumber = SelectedDg?.ContainerNumber;
                    DgToAddLocation = SelectedDg?.Location;
                }
            }
            _selectionObject = obj;
        }

        private void SetSelectionStatusBar(object obj)
        {
            StatusBarText = SelectionStatusBarSetter.GetSelectionStatusBarTextForDg(obj);
            OnPropertyChanged(nameof(StatusBarText));
        }

        // ----- Methods called by received messages

        /// <summary>
        /// Called to update <see cref="StatusBarText"/> when Dg property changes
        /// </summary>
        /// <param name="message"></param>
        private void OnCargoPlanSelectedItemUpdatedMessage(DgListSelectedItemUpdatedMessage message)
        {
            SetSelectionStatusBar(_selectionObject);
        }

        /// <summary>
        /// Called to update <see cref="StatusBarText"/> when Container property changes
        /// </summary>
        /// <param name="message"></param>
        private void OnCargoPlanUnitPropertyChanged(CargoPlanUnitPropertyChanged message)
        {
            SetSelectionStatusBar(_selectionObject);
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
    }



}
