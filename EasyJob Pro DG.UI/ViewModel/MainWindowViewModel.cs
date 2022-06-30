using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.Cargo;
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
using EasyJob_ProDG.UI.View.UI;
using EasyJob_ProDG.UI.Wrapper;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Application = System.Windows.Application;
using Window = System.Windows.Window;

namespace EasyJob_ProDG.UI.ViewModel
{
    [MarkupExtensionReturnType(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : Observable
    {
        #region Private fields
        LoadDataService loadDataService;
        WindowDialogService windowDialogService;
        CargoDataService cargoDataService;
        ConflictDataService conflictDataService;
        SettingsService uiSettingsService;
        IDialogWindowService dialogWindowService;
        IMessageDialogService _messageDialogService;
        ITitleService _titleService;

        private DataGridDgViewModel dgDataGridVM => ViewModelLocator.DataGridDgViewModel;
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
        public DgWrapper SelectedDg { get; set; }
        public StatusBarViewModel StatusBarControl { get; set; }
        public GridLength ConflictColumnWidth { get; set; }
        public int SelectedDataGridIndex { get; set; }

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
            dialogWindowService = new DialogWindowService(Application.Current.MainWindow);
            windowDialogService = new WindowDialogService();
            _messageDialogService = MessageDialogService.Connect();
            _titleService = new TitleService();

        }
        private void LoadData()
        {
            uiSettingsService.LoadSettigs();
            loadDataService.ConnectProgramFiles();
            loadDataService.LoadData();

            UISettings = uiSettingsService.GetSettings();

            GetCargoData();

        }

        /// <summary>
        /// Gets public properties values from cargo and conflict data services
        /// </summary>
        private void GetCargoData()
        {
            //Get data from cargoDataService
            WorkingCargoPlan = cargoDataService.GetCargoPlan();
            Conflicts = conflictDataService.GetConflicts();
            Vents = conflictDataService.GetVentilationRequirements();

            //OnPropertyChange
            RefreshView();

            //Notify DgDataGrid of change
            DataMessenger.Default.Send<CargoDataUpdated>(new CargoDataUpdated(), "cargodataupdated");
        }
        private void LoadCommands()
        {
            AddNewDgCommand = new DelegateCommand(OnAddNewDg, CanAddNewDg);
            ReCheckCommand = new DelegateCommand(OnReCheckRequested);
            OpenShipProfileWindowCommand = new DelegateCommand(OpenShipProfileWindowExecuted);
            OpenUserSettingsWindowCommand = new DelegateCommand(OpenUserSettingsWindowExecuted);
            ShowAboutCommand = new DelegateCommand(ShowAboutExecuted);
            ShowLicenseDialogCommand = new DelegateCommand(ShowLicenseDialogExecuted);
            OpenFileCommand = new DelegateCommand(OpenOnExecuted);
            SaveFileCommand = new DelegateCommand(SaveOnExecuted);
            UpdateConditionCommand = new DelegateCommand(UpdateConditionOnExecuted, CanExecuteForOptionalOpen);
            ImportDataCommand = new DelegateCommand(ImportInfoOnExecuted, CanExecuteForOptionalOpen);
            ImportDataOnlyPolCommand = new DelegateCommand(ImportInfoOnlyPolOnExecuted, CanExecuteForOptionalOpen);
            ImportDataOnlySelectedCommand = new DelegateCommand(ImportInfoOnlySelectedOnExecuted, CanExecuteForOptionalOpen);
            ImportReeferManifestInfoCommand = new DelegateCommand(ImportReeferManifestInfoOnExecuted, CanAddReeferManifestInfo);
            ImportReeferManifestInfoOnlySelectedCommand = new DelegateCommand(ImportReeferManifestInfoOnlySelectedOnExecuted, CanAddReeferManifestInfoOnlySelected);
            ImportReeferManifestInfoOnlyPolCommand = new DelegateCommand(ImportReeferManifestInfoOnlyPolOnExecuted, CanAddReeferManifestInfo);

            ExportToExcelCommand = new DelegateCommand(ExportToExcelOnExecuted);
            SelectionChangedCommand = new DelegateCommand(OnApplicationClosing);
            ApplicationClosingCommand = new DelegateCommand(OnApplicationClosing);

        }

        /// <summary>
        /// Updates MainWindow title
        /// </summary>
        private void SetWindowTitle()
        {
            WindowTitle = _titleService.GetTitle();
            OnPropertyChanged("WindowTitle");
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
        private void OpenNewFile(string file, OpenFile.OpenOption openOption = OpenFile.OpenOption.Open, bool importOnlySelected = false, string currentPort = null)
        {
            StatusBarControl.ChangeBarSet(25);
            if (!loadDataService.OpenNewFile(file, openOption, importOnlySelected, currentPort))
            {
                _messageDialogService.ShowOkDialog("File can not be opened", "Error");
                StatusBarControl.Cancel();
            }
            StatusBarControl.ChangeBarSet(70);
            GetCargoData();

            SetWindowTitle();
            StatusBarControl.ChangeBarSet(90);
            DataMessenger.Default.Send<CargoDataUpdated>(new CargoDataUpdated(), "cargodataupdated");
            StatusBarControl.ChangeBarSet(100);
        }

        /// <summary>
        /// If Working plan exists, offers options on how to open the file
        /// </summary>
        /// <param name="file">Any readable file</param>
        private void OpenFileWithOptionsChoice(string file)
        {
            if (CanExecuteForOptionalOpen(null))
            {
                StatusBarControl.StartProgressBar(10, "Opening...");
                var viewModel = new DialogWindowOptionsViewModel($"Choose how you wish to open the file {file}",
                    "Open as new condition", "Update condition", "Import Dg data");
                bool? dialogResult = dialogWindowService.ShowDialog(viewModel);
                if (!dialogResult.HasValue || !dialogResult.Value)
                {
                    StatusBarControl.Cancel();
                    return;
                }

                if (dialogResult.Value)
                {
                    //StatusBarControl.ChangeBarSet(15);
                    OpenNewFile(file, (OpenFile.OpenOption)viewModel.ResultOption);
                }
            }
            else
            {
                OpenNewFile(file);
            }
        }

        /// <summary>
        /// Returns true if Reefer manifest info successfully imported from the file and Reefers properties updated
        /// </summary>
        /// <param name="file">Excel file containing manifest info</param>
        private void ImportReeferManifestInfo(string file, bool importOnlySelected = false, string currentPort = null)
        {
            StatusBarControl.StartProgressBar(10, "Importing...");
            if (!loadDataService.ImportReeferManifestInfo(file, importOnlySelected, currentPort))
            {
                _messageDialogService.ShowOkDialog("Manifest file can not be read", "Error");
                StatusBarControl.Cancel();
            }
            StatusBarControl.ChangeBarSet(90);
            DataMessenger.Default.Send<CargoDataUpdated>(new CargoDataUpdated(), "reeferinfoupdated");
            StatusBarControl.ChangeBarSet(100);
        }
        #endregion


        #region Private methods
        //---------- Private methods ------------------------------
        /// <summary>
        /// Calls OnPropertyChange for main public properties
        /// </summary>
        private void RefreshView()
        {
            OnPropertyChanged("WorkingCargoPlan");
            OnPropertyChanged("Conflicts");
            OnPropertyChanged("VoyageInfo");
        }

        /// <summary>
        /// Raised when ShipProfile saved to update data
        /// </summary>
        /// <param name="obj"></param>
        private void OnShipProfileSaved(ShipProfileWrapperMessage obj)
        {
            GetCargoData();
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
            dialogWindowService.Register<WinLoginViewModel, winLogin>();
            dialogWindowService.Register<DialogWindowOptionsViewModel, DialogWindowOptions>();
        }

        /// <summary>
        /// Creates new dialog service.
        /// </summary>
        /// <param name="owner"></param>
        private void SetDialogServiceOwner(Window owner)
        {
            dialogWindowService = new DialogWindowService(owner);

        }
        #endregion


        #region Messenger commands

        private void SubscribeToMessenger()
        {
            DataMessenger.Default.Register<ShipProfileWrapperMessage>(this, OnShipProfileSaved, "ship profile saved");
        }
        #endregion


        #region Command methods

        /// <summary>
        /// Calls export to excel method
        /// </summary>
        /// <param name="obj"></param>
        private void ExportToExcelOnExecuted(object obj)
        {
            loadDataService.ExportToExcel(WorkingCargoPlan);
        }

        /// <summary>
        /// Method will call dialog service to choose a file to open and open it
        /// </summary>
        /// <param name="obj">Owner window</param>
        private void OpenOnExecuted(object obj)
        {
            //StatusBarControl.StartProgressBar(0, "Opening...");
            if (!DialogOpenFile.OpenFileWithDialog(obj, out var file))
            {
                StatusBarControl.Cancel();
                return;
            }
            OpenFileWithOptionsChoice(file);
        }

        /// <summary>
        /// Imports Dg and reefer data to update existing cargo plan
        /// </summary>
        /// <param name="obj"></param>
        private void ImportInfoOnExecuted(object obj)
        {
            ImportFileInfoOnExecuted(obj);
        }

        /// <summary>
        /// Import Dg data for current port of loading only
        /// </summary>
        /// <param name="obj"></param>
        private void ImportInfoOnlyPolOnExecuted(object obj)
        {
            ImportFileInfoOnExecuted(obj, false, VoyageInfo.PortOfDeparture);
        }

        /// <summary>
        /// Import Dg data for selected for import items only.
        /// </summary>
        /// <param name="obj"></param>
        private void ImportInfoOnlySelectedOnExecuted(object obj)
        {
            ImportFileInfoOnExecuted(obj, true);
        }

        /// <summary>
        /// Common method for import Dg info.
        /// </summary>
        /// <param name="owner">Owner window for dialog window.</param>
        /// <param name="importOnlySelected">True: only selected for import items will be imported.</param>
        /// <param name="currentPort">If set, only selected items will be imported</param>
        private void ImportFileInfoOnExecuted(object owner, bool importOnlySelected = false, string currentPort = null)
        {
            if (!DialogOpenFile.OpenFileWithDialog(owner, out var file)) return;
            OpenNewFile(file, OpenFile.OpenOption.Import, importOnlySelected, currentPort);
        }

        private bool CanAddReeferManifestInfo(object obj)
        {
            return WorkingCargoPlan.ReeferCount > 0;
        }
        private bool CanAddReeferManifestInfoOnlySelected(object obj)
        {
            if (CanAddReeferManifestInfo(obj))
                return WorkingCargoPlan.Reefers.Any(x => x.IsToImport == true);
            return false;
        }

        /// <summary>
        /// Opens dialog to choose excel file with reefer manifests and imports reefer info
        /// </summary>
        /// <param name="obj">Owner window</param>
        private void ImportReeferManifestInfoOnExecuted(object obj)
        {
            if (!DialogOpenFile.OpenExcelFileWithDialog(obj, out var file)) return;
            ImportReeferManifestInfo(file);
        }

        /// <summary>
        /// Opens dialog to choose excel file with reefer manifests and imports reefer info of reefers loaded in the current port
        /// </summary>
        /// <param name="obj">Owner window</param>
        private void ImportReeferManifestInfoOnlyPolOnExecuted(object obj)
        {
            if (!DialogOpenFile.OpenExcelFileWithDialog(obj, out var file)) return;
            ImportReeferManifestInfo(file, false, VoyageInfo.PortOfDeparture);
        }

        /// <summary>
        /// Opens dialog to choose excel file with reefer manifests and imports reefer info of selected reefers only
        /// </summary>
        /// <param name="obj">Owner window</param>
        private void ImportReeferManifestInfoOnlySelectedOnExecuted(object obj)
        {
            if (!DialogOpenFile.OpenExcelFileWithDialog(obj, out var file)) return;
            ImportReeferManifestInfo(file, true);
        }

        /// <summary>
        /// Updates existing cargo plan with new plan
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateConditionOnExecuted(object obj)
        {
            if (!DialogOpenFile.OpenFileWithDialog(obj, out var file)) return;
            OpenNewFile(file, OpenFile.OpenOption.Update);
        }

        /// <summary>
        /// Method calls dialog to choose file name and location to save the condition.
        /// </summary>
        /// <param name="obj"></param>
        private void SaveOnExecuted(object obj)
        {
            if (DialogSaveFile.SaveFileWithDialog(out var fileName))
            {
                loadDataService.SaveFile(fileName);
            }

            SetWindowTitle();
        }

        /// <summary>
        /// Method initiates OpenFile when a file dropped onto the MainWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnFileDrop(object sender, DragEventArgs e)
        {
            var filePathArray = (string[])e.Data.GetData(DataFormats.FileDrop);
            string file = filePathArray?[0];

            if (filePathArray != null && File.Exists(file) && DialogOpenFile.ConfirmFileType(file))
            {
                OpenFileWithOptionsChoice(filePathArray[0]);
            }
        }

        /// <summary>
        /// Shifts view to DgDataGrid and calls DisplayAddMenu from DgDataGridVM
        /// </summary>
        /// <param name="parameter">none</param>
        private void OnAddNewDg(object parameter)
        {
            var container = GetSelectedContainer();

            SelectedDataGridIndex = 0;
            OnPropertyChanged(nameof(SelectedDataGridIndex));

            dgDataGridVM.OnDisplayAddDgMenu(container);
        }
        private bool CanAddNewDg(object obj) => true;

        /// <summary>
        /// Gets SelectedUnit as Container from SelectedDataGrid.
        /// </summary>
        /// <returns>Container from selection.</returns>
        private Container GetSelectedContainer()
        {
            switch (SelectedDataGridIndex)
            {
                case 0:
                    return (Container)dgDataGridVM.SelectedDg?.Model;
                case 1:
                    return reefersDataGridVM.SelectedReefer?.Model;
                case 2:
                    return containersDataGridVM.SelectedContainer?.Model;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Calls Re-check of condition conflicts
        /// </summary>
        /// <param name="obj"></param>
        private void OnReCheckRequested(object obj)
        {
            DataMessenger.Default.Send(new ConflictListToBeUpdatedMessage());
        }
        #endregion


        #region Event methods
        private void OnApplicationClosing(object parameter)
        {
            SaveWorkingCondition();
        }

        private void SaveWorkingCondition()
        {
            //todo: Implement file name saving and restoring on startup
            loadDataService.SaveFile(ProgramDefaultSettingValues.ProgramDirectory + "Working cargo plan.ejc");
        }

        private bool CanExecuteForOptionalOpen(object obj)
        {
            bool canExecute = WorkingCargoPlan != null && !WorkingCargoPlan.IsEmpty;
            return canExecute;
        }
        #endregion


        #region Methods calling toolbox windows
        private void OpenShipProfileWindowExecuted(object parameters)
        {
            windowDialogService.ShowDialog(new ShipProfileWindow());
            SetWindowTitle();
        }
        private void OpenUserSettingsWindowExecuted(object parameters)
        {
            windowDialogService.ShowDialog(new SettingsWindow());
        }
        private void ShowAboutExecuted(object parameters)
        {
            winAbout winAb = new winAbout();
            winAb.Show();
        }
        private void ShowLicenseDialogExecuted(object parameter)
        {
            var viewModel = new WinLoginViewModel();
            var result = dialogWindowService.ShowDialog(viewModel);

            if (result.HasValue)
            {
                if (result.Value)
                {

                }
            }


        }
        #endregion


        #region Commands
        public ICommand AddNewDgCommand { get; set; }
        public ICommand ReCheckCommand { get; set; }
        public ICommand OpenShipProfileWindowCommand { get; private set; }
        public ICommand OpenUserSettingsWindowCommand { get; private set; }
        public ICommand ShowAboutCommand { get; private set; }
        public ICommand ShowLicenseDialogCommand { get; private set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }
        public ICommand UpdateConditionCommand { get; set; }
        public ICommand ImportDataCommand { get; set; }
        public ICommand ImportDataOnlyPolCommand { get; set; }
        public ICommand ImportDataOnlySelectedCommand { get; set; }
        public ICommand ImportReeferManifestInfoCommand { get; set; }
        public ICommand ImportReeferManifestInfoOnlySelectedCommand { get; set; }
        public ICommand ImportReeferManifestInfoOnlyPolCommand { get; set; }

        public ICommand ExportToExcelCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand ApplicationClosingCommand { get; set; }


        // ----------- Registered commands ------------------------------------------
        public DelegateCommand CloseApplicationCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    Application.Current.Shutdown();
                });
            }
        }

        // -------------- 
        #endregion
    }
}
