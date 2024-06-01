using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.IO;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.DialogWindows;
using EasyJob_ProDG.UI.View.UI;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public partial class MainWindowViewModel
    {
        #region Commands

        // ----- Main Window controls commands -----
        public ICommand AddNewDgCommand { get; private set; }
        public ICommand ReCheckCommand { get; private set; }

        // ----- Settings commands -----
        public ICommand OpenShipProfileWindowCommand { get; private set; }
        public ICommand OpenUserSettingsWindowCommand { get; private set; }
        public ICommand SaveSettingsCommand { get; private set; }
        public ICommand RestoreSettingsCommand { get; private set; }

        // ----- Display utility windows commands -----
        public ICommand ShowAboutCommand { get; private set; }
        public ICommand ShowLicenseDialogCommand { get; private set; }
        public ICommand ShowLoginWindowCommand { get; private set; }

        // ----- Summary commands -----
        public ICommand ShowCargoSummaryCommand { get; private set; }
        public ICommand ShowPortToPortReportCommand { get; private set; }
        public ICommand ShowDgCargoSummaryCommand { get; private set; }

        // ----- Files commands -----
        public ICommand NewCargoPlanCommand { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ICommand SaveFileCommand { get; private set; }
        public ICommand UpdateConditionCommand { get; private set; }

        // ----- Import Dg commands -----
        public ICommand ImportDataCommand { get; private set; }
        public ICommand ImportDataOnlyPolCommand { get; private set; }
        public ICommand ImportDataOnlySelectedCommand { get; private set; }

        // ----- Import Reefer manifests commands -----
        public ICommand ImportReeferManifestInfoCommand { get; private set; }
        public ICommand ImportReeferManifestInfoOnlySelectedCommand { get; private set; }
        public ICommand ImportReeferManifestInfoOnlyPolCommand { get; private set; }

        // ----- Export to excel command -----
        public ICommand ExportToExcelCommand { get; private set; }

        // ----- Various commands -----
        public ICommand SelectionChangedCommand { get; private set; }
        public ICommand ApplicationClosingCommand { get; private set; }
        public ICommand MainWindowLoadedCommand { get; private set; }


        // ----------- Registered commands ------------------------------------------
        public DelegateCommand CloseApplicationCommand
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    CloseApplication();
                });
            }
        }

        // -------------- 
        #endregion

        private void LoadCommands()
        {
            MainWindowLoadedCommand = new DelegateCommand(MainWindowLoadedCommandExecuted);
            AddNewDgCommand = new DelegateCommand(OnAddNewDg, CanAddNewDg);
            ReCheckCommand = new DelegateCommand(OnReCheckRequested);
            OpenShipProfileWindowCommand = new DelegateCommand(OpenShipProfileWindowExecuted);
            OpenUserSettingsWindowCommand = new DelegateCommand(OpenUserSettingsWindowExecuted);
            SaveSettingsCommand = new DelegateCommand(SaveSettingsToFileExecuted);
            RestoreSettingsCommand = new DelegateCommand(RestoreSettingsFromFileExecuted);
            ShowAboutCommand = new DelegateCommand(ShowAboutExecuted);
            ShowLicenseDialogCommand = new DelegateCommand(ShowLicenseDialogExecuted);
            ShowLoginWindowCommand = new DelegateCommand(ShowLoginWindowOnExecuted);
            ShowCargoSummaryCommand = new DelegateCommand(ShowCargoSummaryCommandOnExecuted);
            ShowPortToPortReportCommand = new DelegateCommand(ShowPortToPortReportCommandOnExecuted);
            ShowDgCargoSummaryCommand = new DelegateCommand(ShowDgCargoSummaryCommandOnExecuted);
            NewCargoPlanCommand = new DelegateCommand(NewCargoPlanCommandOnExecuted);
            OpenFileCommand = new DelegateCommand(OpenOnExecuted);
            SaveFileCommand = new DelegateCommand(SaveOnExecuted);
            UpdateConditionCommand = new DelegateCommand(UpdateConditionOnExecuted, CanExecuteForOptionalOpen);
            ImportDataCommand = new DelegateCommand(ImportInfoOnExecuted, CanImportDgInfo);
            ImportDataOnlyPolCommand = new DelegateCommand(ImportInfoOnlyPolOnExecuted, CanImportDgInfo);
            ImportDataOnlySelectedCommand = new DelegateCommand(ImportInfoOnlySelectedOnExecuted, CanImportDgInfoOnlySelected);
            ImportReeferManifestInfoCommand = new DelegateCommand(ImportReeferManifestInfoOnExecuted, CanAddReeferManifestInfo);
            ImportReeferManifestInfoOnlySelectedCommand = new DelegateCommand(ImportReeferManifestInfoOnlySelectedOnExecuted, CanAddReeferManifestInfoOnlySelected);
            ImportReeferManifestInfoOnlyPolCommand = new DelegateCommand(ImportReeferManifestInfoOnlyPolOnExecuted, CanAddReeferManifestInfo);

            ExportToExcelCommand = new DelegateCommand(ExportToExcelOnExecuted);
            SelectionChangedCommand = new DelegateCommand(OnApplicationClosing);
            ApplicationClosingCommand = new DelegateCommand(OnApplicationClosing);

        }

        #region Command methods

        // ----- Close Application -----
        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        // ----- Export to excel -----

        /// <summary>
        /// Calls export to excel method
        /// </summary>
        /// <param name="obj"></param>
        private void ExportToExcelOnExecuted(object obj)
        {
            StatusBarControl.StartProgressBar(10, "Exporting to excel...");
            Action d = loadDataService.ExportDgListToExcel;
            Task.Run(() => WrapMethodWithIsLoading(d)).ConfigureAwait(false);
        }


        // ----- New WorkingCargoPlan -----

        /// <summary>
        /// Creates a new blank cargo plan with no cargo in it.
        /// </summary>
        /// <param name="obj"></param>
        private void NewCargoPlanCommandOnExecuted(object obj)
        {
            if (_messageDialogService.ShowYesNoDialog($"Are you sure that you want to create a new blank cargo plan?\nAll unsaved changes will be lost."
                , "Create new cargo plan") == MessageDialogResult.No) return;
            loadDataService.LoadBlankCargoPlan();
            GetCargoData();
        }


        // ----- Open file -----

        /// <summary>
        /// Method will call dialog service to choose a file to open and open it
        /// </summary>
        /// <param name="obj">Owner window</param>
        private void OpenOnExecuted(object obj)
        {
            if (!DialogOpenFile.OpenFileWithDialog(obj, out var file))
            {
                StatusBarControl.Cancel();
                return;
            }
            _ = OpenFileWithOptionsChoiceAsync(file);
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
                _ = OpenFileWithOptionsChoiceAsync(filePathArray[0]);
            }
        }


        // ----- Import Dg info

        /// <summary>
        /// Imports Dg and reefer data to update existing cargo plan
        /// </summary>
        /// <param name="obj"></param>
        private void ImportInfoOnExecuted(object obj)
        {
            ImportFileDgInfo(obj);
        }

        /// <summary>
        /// Import Dg data for current port of loading only
        /// </summary>
        /// <param name="obj"></param>
        private void ImportInfoOnlyPolOnExecuted(object obj)
        {
            ImportFileDgInfo(obj, false, VoyageInfo.PortOfDeparture);
        }

        /// <summary>
        /// Import Dg data for selected for import items only.
        /// </summary>
        /// <param name="obj"></param>
        private void ImportInfoOnlySelectedOnExecuted(object obj)
        {
            ImportFileDgInfo(obj, true);
        }


        // ----- Import manifest info -----

        /// <summary>
        /// Opens dialog to choose excel file with reefer manifests and imports reefer info
        /// </summary>
        /// <param name="obj">Owner window</param>
        private void ImportReeferManifestInfoOnExecuted(object obj)
        {
            if (!DialogOpenFile.OpenExcelFileWithDialog(obj, out var file)) return;
            Task.Run(() => ImportReeferManifestInfo(file));
        }

        /// <summary>
        /// Opens dialog to choose excel file with reefer manifests and imports reefer info of reefers loaded in the current port
        /// </summary>
        /// <param name="obj">Owner window</param>
        private void ImportReeferManifestInfoOnlyPolOnExecuted(object obj)
        {
            if (!DialogOpenFile.OpenExcelFileWithDialog(obj, out var file)) return;
            Task.Run(() => ImportReeferManifestInfo(file, false, VoyageInfo.PortOfDeparture));
        }

        /// <summary>
        /// Opens dialog to choose excel file with reefer manifests and imports reefer info of selected reefers only
        /// </summary>
        /// <param name="obj">Owner window</param>
        private void ImportReeferManifestInfoOnlySelectedOnExecuted(object obj)
        {
            if (!DialogOpenFile.OpenExcelFileWithDialog(obj, out var file)) return;
            Task.Run(() => ImportReeferManifestInfo(file, true));
        }


        // ----- Update condition -----

        /// <summary>
        /// Updates existing cargo plan with new plan
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateConditionOnExecuted(object obj)
        {
            if (!DialogOpenFile.OpenFileWithDialog(obj, out var file)) return;
            StatusBarControl.StartProgressBar(10, "Updating...");
            Task.Run(() => OpenNewFile(file, OpenFile.OpenOption.Update));
        }


        // ----- Save condition -----

        /// <summary>
        /// Method calls dialog to choose file name and location to save the condition.
        /// </summary>
        /// <param name="obj"></param>
        private void SaveOnExecuted(object obj)
        {
            //suggested file name
            string fileName = GetSuggestedFileName();

            if (DialogSaveFile.SaveFileWithDialog(ref fileName))
            {
                Action d = delegate ()
                {
                    loadDataService.SaveConditionToFile(fileName);
                    SetWindowTitle();
                };
                Task.Run(() => WrapMethodWithIsLoading(d));
            }
        }

        /// <summary>
        /// Suggests the fileName when saving condition based on voyage info.
        /// </summary>
        /// <returns></returns>
        private string GetSuggestedFileName()
        {
            string suggestion = string.Empty;
            var conditionName = cargoDataService.ConditionFileName;
            if (conditionName.EndsWith(ProgramDefaultSettingValues.ConditionFileExtension) && !string.Equals(conditionName, Properties.Settings.Default.WorkingCargoPlanFile))
                return conditionName;

            conditionName = conditionName.ToUpper().Replace(" ", "").Replace("-", "").Replace("_", "");
            if (!string.IsNullOrEmpty(conditionName))
            {
                if (conditionName.Contains("PREFINAL"))
                {
                    suggestion = "Pre-Final";
                }
                else if (conditionName.Contains("FINAL"))
                {
                    suggestion = "Final";
                }
                else if (conditionName.Contains("PRESTOW"))
                {
                    suggestion = "Prestow";
                }
                else if (conditionName.Contains("STOWAGE"))
                {
                    suggestion = "Stowage";
                }
                else if (conditionName.Contains("LOAD"))
                {
                    suggestion = "Load";
                }
                else if (conditionName.Contains("PRE"))
                {
                    suggestion = "Prestow";
                }
                if (conditionName.Contains("UPDATED"))
                {
                    suggestion += "Updated";
                }
                else if (conditionName.Contains("UPDATE"))
                {
                    suggestion += "Update";
                }
                else if (conditionName.Contains("CORRECTED"))
                {
                    suggestion += "Corrected";
                }
                else if (conditionName.Contains("CORRECT"))
                {
                    suggestion += "Correcte";
                }
            }

            return VoyageInfo.VoyageNumber + " "
            + VoyageInfo.PortOfDeparture + " "
            + suggestion;
        }


        // ----- Add items -----

        /// <summary>
        /// Shifts view to DgDataGrid and calls DisplayAddMenu from DataGridDgViewModel
        /// </summary>
        /// <param name="parameter">none</param>
        private void OnAddNewDg(object parameter)
        {
            var container = GetSelectedContainer();

            SelectedDataGridIndex = 0;
            OnPropertyChanged(nameof(SelectedDataGridIndex));

            DataGridDgViewModel.OnDisplayAddDgMenu(container);
        }

        /// <summary>
        /// Gets SelectedUnit as Container from SelectedDataGrid.
        /// </summary>
        /// <returns>Container from selection.</returns>
        private Container GetSelectedContainer()
        {
            switch (SelectedDataGridIndex)
            {
                case 0:
                    return (Container)DataGridDgViewModel.SelectedDg?.Model;
                case 1:
                    return DataGridReefersViewModel.SelectedUnit?.Model;
                case 2:
                    return DataGridContainersViewModel.SelectedUnit?.Model;
                default:
                    return null;
            }
        }


        // ----- Re-check condition

        /// <summary>
        /// Calls Re-check of condition conflicts
        /// </summary>
        /// <param name="obj"></param>
        private void OnReCheckRequested(object obj)
        {
            DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage(true));
        }

        // ----- Settings save - restore -----
        private void SaveSettingsToFileExecuted(object obj)
        {
            uiSettingsService.SaveSettingsToFile();
        }

        private void RestoreSettingsFromFileExecuted(object obj)
        {
            uiSettingsService.RestoreSettingsFromFile(obj);
        }

        #endregion

        #region Command CanExecute methods

        //----- Add Dg -----
        private bool CanAddNewDg(object obj) => true;

        //----- Import Dg -----
        private bool CanImportDgInfo(object obj)
        {
            return CanExecuteForOptionalOpen(obj);
        }
        private bool CanImportDgInfoOnlySelected(object obj)
        {
            if (CanImportDgInfo(obj))
                return WorkingCargoPlan.DgList.Any(x => x.IsToImport == true);
            return false;
        }

        //----- Import Reefer manifest -----
        private bool CanAddReeferManifestInfo(object obj)
        {
            return CanExecuteForOptionalOpen(obj) && WorkingCargoPlan.ReeferCount > 0;
        }
        private bool CanAddReeferManifestInfoOnlySelected(object obj)
        {
            if (CanAddReeferManifestInfo(obj))
                return WorkingCargoPlan.Reefers.Any(x => x.IsToImport == true);
            return false;
        }

        #endregion


        #region Event methods
        private void OnApplicationClosing(object parameter)
        {
            SaveWorkingCondition();
            LogWriter.CloseLog();
        }

        private void SaveWorkingCondition()
        {
            //todo: Implement file name saving and restoring on startup
            loadDataService.SaveConditionToFile(ProgramDefaultSettingValues.ProgramDirectory + Properties.Settings.Default.WorkingCargoPlanFile);
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
            windowDialogService.ShowDialog(new ShipProfileWindow(), new ShipProfileWindowVM());
            SetWindowTitle();
        }
        private void OpenUserSettingsWindowExecuted(object parameters)
        {
            int selectedTab;
            switch ((string)parameters)
            {
                case "ExcelDg":
                    selectedTab = 0;
                    break;
                case "ExcelReefers":
                    selectedTab = 1;
                    break;

                case null:
                default:
                    selectedTab = 0;
                    break;
            }

            windowDialogService.ShowDialog(new SettingsWindow(selectedTab));
        }
        private void ShowAboutExecuted(object parameters)
        {
            windowDialogService.ShowDialog(new winAbout());
        }
        private void ShowLicenseDialogExecuted(object parameter)
        {
            windowDialogService.ShowDialog(new winLicence());
        }
        private void ShowLoginWindowOnExecuted(object obj)
        {
            var viewModel = new WinLoginViewModel();
            var result = mappedDialogWindowService.ShowDialog(viewModel);

            if (result.HasValue)
            {
                if (result.Value)
                {

                }
            }
        }
        private void ShowWelcomeWindow()
        {
            var viewModel = new WelcomeWindowVM();
            bool? dialogResult = mappedDialogWindowService.ShowDialog(viewModel);

            if (dialogResult.HasValue)
            {
                if (dialogResult.Value)
                {
                    OpenShipProfileWindowExecuted(null);
                    return;
                }
                return;
            }
        }

        internal void ShowLicenceAgreement(bool isFirstStart = false)
        {
            var viewModel = new LicenceAgreementViewModel(isFirstStart);
            windowDialogService.ShowDialog(new LicenceAgreement(), viewModel);
        }


        // ----- Summary -----

        private void ShowDgCargoSummaryCommandOnExecuted(object obj)
        {
            var dgSummaryReport = new DgSummaryReportViewModel(VoyageInfo);
            dgSummaryReport.CreateReport(WorkingCargoPlan.Model);
            windowDialogService.ShowDialog(new DgSummaryReport(), dgSummaryReport);
        }

        private void ShowCargoSummaryCommandOnExecuted(object obj)
        {
            var cargoReportViewModel = new CargoReportViewModel(VoyageInfo);
            cargoReportViewModel.CreateReport(WorkingCargoPlan.Model);
            mappedDialogWindowService.ShowDialog(cargoReportViewModel);
        }

        private void ShowPortToPortReportCommandOnExecuted(object obj)
        {
            var portToPortReportViewModel = new PortToPortReportViewModel();
            portToPortReportViewModel.CreateReport(WorkingCargoPlan.Model);
            windowDialogService.ShowDialog(new PortToPortReport(), portToPortReportViewModel);
        }

        #endregion
    }
}
