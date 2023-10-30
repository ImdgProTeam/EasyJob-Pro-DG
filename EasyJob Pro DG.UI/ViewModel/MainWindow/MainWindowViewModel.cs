using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.DialogWindows;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Threading.Tasks;
using System.Windows.Markup;
using Application = System.Windows.Application;
using Window = System.Windows.Window;

namespace EasyJob_ProDG.UI.ViewModel
{
    [MarkupExtensionReturnType(typeof(MainWindowViewModel))]
    public partial class MainWindowViewModel : Observable
    {
        #region Private fields

        LoadDataService loadDataService;
        IWindowDialogService windowDialogService;
        IMappedDialogWindowService mappedDialogWindowService;
        ICargoDataService cargoDataService;
        ConflictDataService conflictDataService;
        SettingsService uiSettingsService;
        IMessageDialogService _messageDialogService;
        ITitleService _titleService;

        private DataGridReefersViewModel reefersDataGridVM => ViewModelLocator.DataGridReefersViewModel;
        private DataGridContainersViewModel containersDataGridVM => ViewModelLocator.DataGridContainersViewModel;

        #endregion


        #region Public properties to be used in View

        public string WindowTitle { get; private set; }
        public ConflictsList Conflicts { get; set; }
        public VentilationRequirements Vents { get; set; }
        public CargoPlanWrapper WorkingCargoPlan { get; set; }
        public Voyage VoyageInfo => WorkingCargoPlan.VoyageInfo ?? null;
        public UserUISettings UISettings { get; set; }
        public StatusBarViewModel StatusBarControl { get; set; }
        public int SelectedDataGridIndex { get; set; }
        public DataGridDgViewModel DgDataGridVM => ViewModelLocator.DataGridDgViewModel;

        /// <summary>
        /// Property indicating if any loading/saving etc. process is running at the moment.
        /// </summary>
        public bool IsLoading { get; set; } = true;

        /// <summary>
        /// Property indicating if dimmed overlay shall be visible due to inactive main window.
        /// </summary>
        public bool IsDimmedOverlayVisible
        {
            get => isDimmedOverlayVisible;
            set
            {
                isDimmedOverlayVisible = value;
                OnPropertyChanged(nameof(IsDimmedOverlayVisible));
            }
        }
        private bool isDimmedOverlayVisible;

        #endregion


        #region Constructor
        public MainWindowViewModel()
        {
            //starting status bar         
            StatusBarControl = new StatusBarViewModel();

            LoadCommands();
            SubscribeToMessenger();
            LoadServices();
            SetupDialogService();

            LoadData();

            SetWindowTitle();

            IsLoading = false;

        }
        #endregion

        //-----------------------------------------------------------------------------------------------------------------------------

        #region StartUp logic
        //---------- StartUp logic

        private void LoadServices()
        {
            loadDataService = new LoadDataService();
            cargoDataService = new CargoDataService();
            conflictDataService = new ConflictDataService();
            uiSettingsService = new SettingsService();
            mappedDialogWindowService = new MappedDialogWindowService(Application.Current.MainWindow);
            windowDialogService = new WindowDialogService();
            _messageDialogService = MessageDialogService.Connect();
            _titleService = new TitleService();

        }
        private void LoadData()
        {
            uiSettingsService.LoadSettings();

            //Connecting Program files
            if (!loadDataService.ConnectProgramFiles())
            {
                _messageDialogService.ShowOkDialog("The program is unable to connect DataBase file.\nProDG will be stopped and closed.", "Error");
                Environment.Exit(0);
            }

            loadDataService.LoadData();

            UISettings = uiSettingsService.GetSettings();

            GetCargoData();
        }

        /// <summary>
        /// Gets public properties values from cargo and conflict data services
        /// </summary>
        private void GetCargoData()
        {
            //Clear deleted conflicts list on opening of new condition
            ViewModelLocator.ConflictListViewModel?.ClearDeletedConflictsList();

            //Get data from cargoDataService
            WorkingCargoPlan = cargoDataService.GetCargoPlan();
            Conflicts = conflictDataService.GetConflicts();
            Vents = conflictDataService.GetVentilationRequirements();

            //OnPropertyChange
            RefreshView();

            //Notify DgDataGrid of change
            DataMessenger.Default.Send<CargoDataUpdated>(new CargoDataUpdated(), "cargodataupdated");
        }
        private void SubscribeToMessenger()
        {
            DataMessenger.Default.Register<ShipProfileWrapperMessage>(this, OnShipProfileSaved, "ship profile saved");
            DataMessenger.Default.Register<ConflictPanelItemViewModel>(this, OnConflictSelectionChanged,
                "conflict selection changed");
        }

        /// <summary>
        /// Updates MainWindow title
        /// </summary>
        private void SetWindowTitle()
        {
            WindowTitle = _titleService.GetTitle();
            OnPropertyChanged(nameof(WindowTitle));
        }

        /// <summary>
        /// Called on MainWindow Loaded event via Command
        /// </summary>
        /// <param name="obj"></param>
        private void MainWindowLoadedCommandExecuted(object obj)
        {
            //Welcome window
            if (loadDataService.IsShipProfileNotFound || Services.FirstStartService.IsTheFirstStart)
            {
                ShowWelcomeWindow();
            }
        }

        #endregion


        #region Working with files private methods

        //---------- Working with files

        /// <summary>
        /// Opens condition from the file.
        /// </summary>
        /// <param name="file">Readable condition file with full path</param>
        /// <param name="openOption">Option from enumeration weather to Open, Update or Import data</param>
        /// <param name="importOnlySelected">For import: Only selected for import items will be imported.</param>
        /// <param name="currentPort">For import: Only current port of loading items will be imported.</param>
        private async Task OpenNewFile(string file, OpenFile.OpenOption openOption = OpenFile.OpenOption.Open, bool importOnlySelected = false, string currentPort = null)
        {

            SetIsLoading(true);
            StatusBarControl.ChangeBarSet(25);
            if (!loadDataService.OpenNewFile(file, openOption, importOnlySelected, currentPort))
            {
                _messageDialogService.ShowOkDialog("File can not be opened", "Error");
                StatusBarControl.Cancel();
                SetIsLoading(false);
                return;
            }
            StatusBarControl.ChangeBarSet(80);


            await Task.Run(() => GetCargoData());
            StatusBarControl.ChangeBarSet(90);

            SetWindowTitle();
            StatusBarControl.ChangeBarSet(100);
            SetIsLoading(false);
        }


        /// <summary>
        /// If Working plan exists, offers options on how to open the file.
        /// Opening asyncronously.
        /// </summary>
        /// <param name="file">Any readable file</param>
        private async Task OpenFileWithOptionsChoiceAsync(string file)
        {
            if (CanExecuteForOptionalOpen(null))
            {
                StatusBarControl.StartProgressBar(10, "Opening...");
                var viewModel = new DialogWindowOptionsViewModel($"Choose how you wish to open the file {file}",
                    "Open as new condition", "Update condition", "Import Dg data");
                bool? dialogResult = mappedDialogWindowService.ShowDialog(viewModel);
                if (!dialogResult.HasValue || !dialogResult.Value)
                {
                    StatusBarControl.Cancel();
                    return;
                }

                if (dialogResult.Value)
                {
                    await Task.Run(() => OpenNewFile(file, (OpenFile.OpenOption)viewModel.ResultOption));
                }
            }
            else
            {
                StatusBarControl.StartProgressBar(10, "Opening...");
                await Task.Run(() => OpenNewFile(file));
            }
        }

        /// <summary>
        /// Common method for import Dg info.
        /// </summary>
        /// <param name="owner">Owner window for dialog window.</param>
        /// <param name="importOnlySelected">True: only selected for import items will be imported.</param>
        /// <param name="currentPort">If set, only selected items will be imported</param>
        private void ImportFileDgInfo(object owner, bool importOnlySelected = false, string currentPort = null)
        {
            if (!DialogOpenFile.OpenFileWithDialog(owner, out var file)) return;

            StatusBarControl.StartProgressBar(10, "Importing...");
            Task.Run(() => OpenNewFile(file, OpenFile.OpenOption.Import, importOnlySelected, currentPort));
        }

        /// <summary>
        /// Returns true if Reefer manifest info successfully imported from the file and Reefers properties updated
        /// </summary>
        /// <param name="file">Excel file containing manifest info</param>
        private void ImportReeferManifestInfo(string file, bool importOnlySelected = false, string currentPort = null)
        {
            SetIsLoading(true);
            StatusBarControl.StartProgressBar(10, "Importing...");
            if (!loadDataService.ImportReeferManifestInfo(file, importOnlySelected, currentPort))
            {
                _messageDialogService.ShowOkDialog("Manifest file can not be read", "Error");
                StatusBarControl.Cancel();
                SetIsLoading(false);
            }
            StatusBarControl.ChangeBarSet(90);
            DataMessenger.Default.Send<CargoDataUpdated>(new CargoDataUpdated(), "reeferinfoupdated");
            StatusBarControl.ChangeBarSet(100);
            SetIsLoading(false);
        }

        #endregion


        #region Private UI methods

        //----- Private UI methods ------------------------------

        /// <summary>
        /// Calls OnPropertyChange for main public properties
        /// </summary>
        private void RefreshView()
        {
            OnPropertyChanged(nameof(WorkingCargoPlan));
            OnPropertyChanged(nameof(Conflicts));
            OnPropertyChanged(nameof(VoyageInfo));
        }

        /// <summary>
        /// Raised when ShipProfile saved to update data
        /// </summary>
        /// <param name="obj"></param>
        private void OnShipProfileSaved(ShipProfileWrapperMessage obj)
        {
            WrapMethodWithIsLoading(GetCargoData);
        }

        /// <summary>
        /// Method changes SelectedItem to match with ConflictPanelItem object
        /// </summary>
        /// <param name="obj">Selected conflict</param>
        private void OnConflictSelectionChanged(ConflictPanelItemViewModel obj)
        {
            if (obj is null)
                return;

            switch (SelectedDataGridIndex)
            {
                case 0:
                    DgDataGridVM.SelectDg(obj.DgID);
                    break;
                case 1:
                    reefersDataGridVM.SelectReefer(obj.ContainerNumber);
                    break;
                case 2:
                    containersDataGridVM.SelectContainer(obj.ContainerNumber);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Spinner logic

        /// <summary>
        /// Sets IsLoading to true before execution of the method, then to false afterwards.
        /// </summary>
        /// <param name="method">Method to be executed</param>
        private void WrapMethodWithIsLoading(Action method)
        {
            SetIsLoading(true);
            method.Invoke();
            SetIsLoading(false);
        }

        /// <summary>
        /// Sets IsLoading to true or false and raises OnPropertyChanged.
        /// </summary>
        /// <param name="isLoading"></param>
        internal void SetIsLoading(bool isLoading)
        {
            IsLoading = isLoading;
            OnPropertyChanged(nameof(IsLoading));
        }

        #endregion

        #region Window dialog service
        /// <summary>
        /// Sets up dialog service and registers viewModels for it.
        /// </summary>
        private void SetupDialogService()
        {
            SetDialogServiceOwner(Application.Current.MainWindow);
            RegisterDialogServiceRelations();
        }

        /// <summary>
        /// Registers windows and their view models in dialog service
        /// </summary>
        private void RegisterDialogServiceRelations()
        {
            mappedDialogWindowService.Register<WelcomeWindowVM, WelcomeWindow>();
            mappedDialogWindowService.Register<WinLoginViewModel, winLogin>();
            mappedDialogWindowService.Register<DialogWindowOptionsViewModel, DialogWindowOptions>();
            mappedDialogWindowService.Register<CargoReportViewModel, CargoReport>();
        }

        /// <summary>
        /// Creates new dialog service.
        /// </summary>
        /// <param name="owner"></param>
        private void SetDialogServiceOwner(Window owner)
        {
            mappedDialogWindowService = new MappedDialogWindowService(owner);
        }

        #endregion

    }
}
