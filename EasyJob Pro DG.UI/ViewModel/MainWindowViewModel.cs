﻿using EasyJob_ProDG.Data;
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
using EasyJob_ProDG.UI.Utility.Messages;
using EasyJob_ProDG.UI.View.DialogWindows;
using EasyJob_ProDG.UI.View.UI;
using EasyJob_ProDG.UI.Wrapper;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Application = System.Windows.Application;
using Window = System.Windows.Window;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class MainWindowViewModel : Observable
    {
        // -------------- Private fields --------------------------------------------

        LoadDataService loadDataService;
        WindowDialogService windowDialogService;
        CargoDataService cargoDataService;
        ConflictDataService conflictDataService;
        SettingsService uiSettingsService;
        IDialogWindowService dialogWindowService;
        IMessageDialogService _messageDialogService;
        ITitleService _titleService;


        // -------------- Public properties to be used in view ----------------------

        public string WindowTitle { get; private set; }
        public ConflictsList Conflicts { get; set; }
        public VentilationRequirements Vents { get; set; }
        public CargoPlanWrapper WorkingCargoPlan { get; set; }
        public Voyage VoyageInfo => WorkingCargoPlan.VoyageInfo ?? null;
        public UserUISettings UISettings { get; set; }
        public DgWrapper SelectedDg { get; set; }
        public StatusBarViewModel StatusBarControl { get; set; }


        // -------------- Constructor -----------------------------------------------

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
            RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChanged()
        {

        }


        // ---------------- Methods -------------------------------------------------

        private void LoadServices()
        {
            loadDataService = new LoadDataService();
            cargoDataService = new CargoDataService();
            conflictDataService = new ConflictDataService();
            uiSettingsService = new SettingsService();
            dialogWindowService = new DialogWindowService(Application.Current.MainWindow);
            windowDialogService = new WindowDialogService();
            _messageDialogService = new MessageDialogService();
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
        private void LoadCommands()
        {
            AddNewDg = new DelegateCommand(OnAddNewDg, CanAddNewDg);
            ReCheckCommand = new DelegateCommand(OnReCheckRequested);
            OpenShipProfileWindow = new DelegateCommand(OpenShipProfileWindowExecuted);
            OpenUserSettingsWindow = new DelegateCommand(OpenUserSettingsWindowExecuted);
            ShowAbout = new DelegateCommand(ShowAboutExecuted);
            ShowLicenseDialog = new DelegateCommand(ShowLicenseDialogExecuted);
            OpenFileCommand = new DelegateCommand(OpenOnExecuted);
            SaveFileCommand = new DelegateCommand(SaveOnExecuted);
            UpdateConditionCommand = new DelegateCommand(UpdateConditionOnExecuted, CanExecuteForOptionalOpen);
            ImportDataCommand = new DelegateCommand(ImportInfoOnExecuted, CanExecuteForOptionalOpen);
            ImportDataOnlyPolCommand = new DelegateCommand(ImportInfoOnlyPolOnExecuted, CanExecuteForOptionalOpen);
            ImportDataOnlySelectedCommand = new DelegateCommand(ImportInfoOnlySelectedOnExecuted, CanExecuteForOptionalOpen);
            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
            SelectionChangedCommand = new DelegateCommand(OnApplicationClosing);
            ApplicationClosing = new DelegateCommand(OnApplicationClosing);

            //Dummy command added for testing purpose
            DummyCommand = new DelegateCommand(DummyMethod);

        }

        private static bool dummyBool;
        /// <summary>
        /// Dummy method added for testing purpose and does not affect the program run
        /// </summary>
        /// <param name="obj"></param>
        private void DummyMethod(object obj)
        {
            //var dummy = CargoPlanWrapper.ContainedInList(new Container(){ContainerNumber = "TRHU3998761", Location="0050384"}, WorkingCargoPlan.DgList);
            //MessageBox.Show(dummy.ToString());

            //var viewModel = new DialogWindowOptionsViewModel("This is a dummy dialogWindow with a very very very long long long text for testing purpose only. To see how good it is displayed in the window", "Button 1 Will also become a very long text in here which has no meaning at all", "Button 2", "Button one more to test how it looks", "This is a test header, which is not compulsory at all");
            //bool? result = dialogWindowService.ShowDialog(viewModel);
            //var choise = viewModel.ResultOption;
            if (dummyBool == false)
            {
                StatusBarControl.StartProgressBar(50);
                dummyBool = true;
            }
            else StatusBarControl.ChangeBarSet(70);
        }

        /// <summary>
        /// Updates MainWindow title
        /// </summary>
        private void SetWindowTitle()
        {
            WindowTitle = _titleService.GetTitle();
            OnPropertyChanged("WindowTitle");
        }

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
                if (!dialogResult.HasValue)
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

        /// <summary>
        /// Calls OnPropertyChange for main public properties
        /// </summary>
        private void RefreshView()
        {
            OnPropertyChanged("WorkingCargoPlan");
            OnPropertyChanged("Conflicts");
            OnPropertyChanged("VoyageInfo");
        }

        // --------- Window dialog service ------------------------------------------

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

        // --------- Messenger commands ---------------------------------------------

        private void SubscribeToMessenger()
        {
            DataMessenger.Default.Register<ShipProfileWrapperMessage>(this, OnShipProfileSaved, "ship profile saved");
        }









        ///// <summary>
        ///// Changes location to all items with matching number in all lists 
        ///// </summary>
        ///// <param name="containerNumber"></param>
        ///// <param name="value">New location</param>
        //private void SetNewContainerLocation(string containerNumber, string value)
        //{
        //    foreach (var dg in WorkingCargoPlan.DgList)
        //    {
        //        if (dg.ContainerNumber == containerNumber)
        //            if (dg.Location != value)
        //                dg.Location = value;
        //    }

        //    foreach (var container in WorkingCargoPlan.Containers)
        //    {
        //        if (container.ContainerNumber == containerNumber)
        //            if (container.Location != value)
        //                container.Location = value;
        //    }

        //    foreach (var reefer in WorkingCargoPlan.Reefers)
        //    {
        //        if (reefer.ContainerNumber == containerNumber)
        //            if (reefer.Location != value)
        //                reefer.Location = value;
        //    }
        //}



        /// <summary>
        /// Raised when ShipProfile saved to update data
        /// </summary>
        /// <param name="obj"></param>
        private void OnShipProfileSaved(ShipProfileWrapperMessage obj)
        {
            GetCargoData();
        }

        //---------- Command methods ------------------------------------------------

        /// <summary>
        /// Calls export to excel method
        /// </summary>
        /// <param name="obj"></param>
        private void ExportToExcel(object obj)
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
                //StatusBarControl.Cancel();
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
            ImportFileInfo(obj);
        }

        /// <summary>
        /// Import Dg data for current port of loading only
        /// </summary>
        /// <param name="obj"></param>
        private void ImportInfoOnlyPolOnExecuted(object obj)
        {
            ImportFileInfo(obj, false, VoyageInfo.PortOfDeparture);
        }

        /// <summary>
        /// Import Dg data for selected for import items only.
        /// </summary>
        /// <param name="obj"></param>
        private void ImportInfoOnlySelectedOnExecuted(object obj)
        {
            ImportFileInfo(obj, true);
        }

        /// <summary>
        /// Common method for import Dg info.
        /// </summary>
        /// <param name="owner">Owner window for dialog window.</param>
        /// <param name="importOnlySelected">True: only selected for import items will be imported.</param>
        /// <param name="currentPort">If set, only selected items will be imported</param>
        private void ImportFileInfo(object owner, bool importOnlySelected = false, string currentPort = null)
        {
            if (!DialogOpenFile.OpenFileWithDialog(owner, out var file)) return;
            OpenNewFile(file, OpenFile.OpenOption.Import, importOnlySelected, currentPort);
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
        /// Adds new default Dg to WorkingCargoPlan.
        /// Updates DgContainerCount.
        /// </summary>
        /// <param name="parameter">none</param>
        private void OnAddNewDg(object parameter)
        {
            WorkingCargoPlan.AddDg(new DgWrapper()
            {
                DgClass = "1.3",
                Unno = 3225,
                Location = "020008",
                ContainerNumber = "GHGA9871235",
                POD = "CNSHA",
                POL = "USOAK",
                IsInList = true
            });
            OnPropertyChanged("DgContainerCount");
        }
        private bool CanAddNewDg(object obj)
        {
            return false;
        }

        /// <summary>
        /// Calls Re-check of condition conflicts
        /// </summary>
        /// <param name="obj"></param>
        private void OnReCheckRequested(object obj)
        {
            DataMessenger.Default.Send(new ConflictListToBeUpdatedMessage());
            //conflictDataService.ReCheckConflicts();
        }

        //--------- Event methods ---------------------------------------------------

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

        //--------- Methods without use and references ------------------------------

        /// <summary>
        /// Clears all class properties
        /// </summary>
        public void NewTable()
        {
            WorkingCargoPlan.Clear();
            Conflicts.Clear();
            Stowage.SWgroups.Clear();
        }


        // ---------- Toolbox windows calling methods

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


        // ---------------- Commands ------------------------------------------------

        public ICommand AddNewDg { get; set; }
        public ICommand ReCheckCommand { get; set; }
        public ICommand OpenShipProfileWindow { get; private set; }
        public ICommand OpenUserSettingsWindow { get; private set; }
        public ICommand ShowAbout { get; private set; }
        public ICommand ShowLicenseDialog { get; private set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }
        public ICommand UpdateConditionCommand { get; set; }
        public ICommand ImportDataCommand { get; set; }
        public ICommand ImportDataOnlyPolCommand { get; set; }
        public ICommand ImportDataOnlySelectedCommand { get; set; }
        public ICommand ExportToExcelCommand { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand ApplicationClosing { get; set; }

        //Dummy command added for testing purpose only.
        //Remember to delete dummy button when removing the command.
        public ICommand DummyCommand { get; set; }


        // ----------- Registered commands ------------------------------------------

        public DelegateCommand CloseApplication
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

    }
}
