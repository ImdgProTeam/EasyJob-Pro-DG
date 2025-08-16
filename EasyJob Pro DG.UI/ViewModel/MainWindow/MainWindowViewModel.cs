using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.IO;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.DialogWindows;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;



namespace EasyJob_ProDG.UI.ViewModel
{
    [MarkupExtensionReturnType(typeof(MainWindowViewModel))]
    public partial class MainWindowViewModel : Observable
    {
        #region Private fields

        /// <summary>
        /// Access to all services
        /// </summary>
        ServicesHandler Services;

        #endregion


        #region Public properties to be used in View
        public string WindowTitle { get; private set; }
        public CargoPlanWrapper WorkingCargoPlan => Services.CargoDataServiceAccess.WorkingCargoPlan;
        public Voyage VoyageInfo => WorkingCargoPlan.VoyageInfo ?? null;
        public StatusBarViewModel StatusBarControl { get; set; }

        // Data Grids bindable properties
        public DataGridDgViewModel DataGridDgViewModel => ViewModelLocator.DataGridDgViewModel;
        public DataGridReefersViewModel DataGridReefersViewModel => ViewModelLocator.DataGridReefersViewModel;
        public DataGridContainersViewModel DataGridContainersViewModel => ViewModelLocator.DataGridContainersViewModel;
        public DataGridUpdatesViewModel DataGridUpdatesViewModel => ViewModelLocator.DataGridUpdatesViewModel;
        public int SelectedDataGridIndex { get; set; }

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

            LoadData();

            SetWindowTitle();

            IsLoading = false;

        }
        #endregion

        //-----------------------------------------------------------------------------------------------------------------------------

        #region StartUp logic
        //----- StartUp logic -----

        private void LoadServices()
        {
            Services = ServicesHandler.GetServicesAccess();
        }

        private void LoadData()
        {
            Services.LoadData();

            GetCargoData();

            CargoPlanWrapperHandler.Launch();
            CargoPlanUnitPropertyChanger.Launch();
        }

        private void SubscribeToMessenger()
        {
            DataMessenger.Default.Register<UpdateCargoPlan>(this, OnNeedToUpdateCargoPlanMessageReceived, "Need to update cargo plan");
            DataMessenger.Default.Register<ShipProfileSavedMessage>(this, OnShipProfileSaved, "ship profile saved");
            DataMessenger.Default.Register<ConflictPanelItemViewModel>(this, OnConflictSelectionChanged,
                "conflict selection changed");
        }


        /// <summary>
        /// Updates MainWindow title
        /// </summary>
        private void SetWindowTitle()
        {
            WindowTitle = Services.TitleServiceAccess.GetTitle();
            OnPropertyChanged(nameof(WindowTitle));
        }

        /// <summary>
        /// Called on MainWindow Loaded event via Command
        /// </summary>
        /// <param name="obj"></param>
        private void MainWindowLoadedCommandExecuted(object obj)
        {
            //Welcome window
            if (Services.LoadDataServiceAccess.IsShipProfileNotFound || UI.Services.FirstStartService.IsTheFirstStart)
            {
                ShowWelcomeWindow();
            }
        }

        #endregion


        /// <summary>
        /// Gets public properties values from cargo and conflict data services
        /// </summary>
        private void GetCargoData()
        {
            //Get data from cargoDataService
            Services.CargoDataServiceAccess.GetCargoPlan();

            //Run WorkingCargoPlan check
            Services.CargoPlanCheckServiceAccess.CheckCargoPlan();

            // Refreshes conflicts list
            DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage(), "update conflicts");

            //OnPropertyChange
            RefreshView();

            //Notify DgDataGrid of change
            DataMessenger.Default.Send(new CargoDataUpdated(), "cargodataupdated");
        }


        #region Working with files private methods

        //---------- Working with files

        /// <summary>
        /// Opens condition from the file.
        /// </summary>
        /// <param name="file">Readable condition file name with full path</param>
        /// <param name="openOption">Option from enumeration weather to Open, Update or Import data</param>
        /// <param name="importOnlySelected">For import: Only selected for import items will be imported.</param>
        /// <param name="currentPort">For import: Only current port of loading items will be imported.</param>
        private async Task OpenNewFile(string file, OpenFile.OpenOption openOption = OpenFile.OpenOption.Open, bool importOnlySelected = false, string currentPort = null)
        {

            SetIsLoading(true);
            StatusBarControl.ChangeBarSet(25);
            if (!Services.LoadDataServiceAccess.OpenCargoPlanFromFile(file, openOption, importOnlySelected, currentPort))
            {
                Services.MessageDialogServiceAccess.ShowOkDialog("File can not be opened", "Error");
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

            // Show update summary
            SetConditionUpdateSummaryCreatedStatus(false);
            if (openOption == OpenFile.OpenOption.Update && Services.SettingsServiceAccess.ShowSummaryOnUpdateCondition)
                await Application.Current.Dispatcher.InvokeAsync(() => ShowConditionUpdateSummary());
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
                bool? dialogResult = Services.MappedDialogWindowServiceAccess.ShowDialog(viewModel);
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
            if (!Services.LoadDataServiceAccess.ImportReeferManifestInfo(file, importOnlySelected, currentPort))
            {
                Services.MessageDialogServiceAccess.ShowOkDialog("Manifest file can not be read", "Error");
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
            OnPropertyChanged(nameof(VoyageInfo));
        }

        /// <summary>
        /// Raised when ShipProfile saved to update data
        /// </summary>
        /// <param name="obj"></param>
        private void OnShipProfileSaved(ShipProfileSavedMessage obj)
        {
            WrapMethodWithIsLoading(() =>
            {
                WorkingCargoPlan.UpdateCargoHoldNumbers();
                GetCargoData();
            });
        }

        /// <summary>
        /// Raised when it is required to call <see cref="GetCargoData"/> method via received message.
        /// </summary>
        private void OnNeedToUpdateCargoPlanMessageReceived(UpdateCargoPlan message)
        {
            GetCargoData();
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
                    DataGridDgViewModel.SelectDg(obj.DgID);
                    break;
                case 1:
                    DataGridReefersViewModel.SelectUnit(obj.ContainerNumber);
                    break;
                case 2:
                    DataGridContainersViewModel.SelectUnit(obj.ContainerNumber);
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

    }
}
