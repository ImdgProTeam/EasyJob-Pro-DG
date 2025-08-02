using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.DialogWindows;
using EasyJob_ProDG.UI.Wrapper;
using System;

namespace EasyJob_ProDG.UI.Services
{
    /// <summary>
    /// Class to assist in handling various services complex sequences.
    /// </summary>
    public class ServicesHandler
    {
        #region Private fields
        /// <summary>
        /// Instance of the class for Singleton use
        /// </summary>
        private static ServicesHandler _instance = new ServicesHandler();

        // services references
        ILoadDataService _loadDataService;
        ICargoDataService _cargoDataService;
        ICargoPlanCheckService _cargoPlanCheckService;
        IConflictDataService _conflictDataService;
        ISettingsService _uiSettingsService;
        IWindowDialogService _windowDialogService;
        IMappedDialogWindowService _mappedDialogWindowService;
        IMessageDialogService _messageDialogService;
        ITitleService _titleService;
        IFileNameService _fileNameService;

        #endregion


        #region Public Properties (Accessors to the services)

        internal ILoadDataService LoadDataServiceAccess => _loadDataService;
        internal ICargoDataService CargoDataServiceAccess => _cargoDataService;
        internal ICargoPlanCheckService CargoPlanCheckServiceAccess => _cargoPlanCheckService;
        internal IConflictDataService ConflictDataServiceAccess => _conflictDataService;
        internal ISettingsService SettingsServiceAccess => _uiSettingsService;
        internal IWindowDialogService WindowDialogServiceAccess => _windowDialogService;
        internal IMappedDialogWindowService MappedDialogWindowServiceAccess => _mappedDialogWindowService;
        internal IMessageDialogService MessageDialogServiceAccess => _messageDialogService;
        internal ITitleService TitleServiceAccess => _titleService;
        internal IFileNameService FileNameServiceAccess => _fileNameService;

        #endregion


        /// <summary>
        /// Provides access to the <see cref="ServicesHandler"/>.
        /// </summary>
        /// <returns>A reference to the service instance.</returns>
        public static ServicesHandler GetServicesAccess()
        {
            return _instance;
        }

        /// <summary>
        /// Loads settings and data and connects program files at program start
        /// </summary>
        internal void LoadData()
        {
            _uiSettingsService.LoadSettings();

            //Connecting Program files
            if (!_loadDataService.ConnectProgramFiles())
            {
                _messageDialogService.ShowOkDialog("The program is unable to connect DataBase file.\nProDG will be stopped and closed.", "Error");
                Environment.Exit(0);
            }

            string openPath = ((View.UI.MainWindow)System.Windows.Application.Current.MainWindow)?.StartupFilePath;
            _loadDataService.LoadCargoData(openPath);
        }

        /// <summary>
        /// Receives message to check CargoPlan and update ConflictsList.
        /// Uses messages to send respective notifications.
        /// </summary>
        /// <param name="obj"></param>
        private void OnConflictsToBeCheckedAndUpdatedMessageReceived(ConflictsToBeCheckedAndUpdatedMessage obj)
        {
            DgWrapper wrapper = obj.dgWrapper;
            if (wrapper is null)
            {
                _cargoPlanCheckService.CheckCargoPlan();
                DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage(obj.FullListToBeUpdated), "update conflicts");
            }
            else // if wrapper is passed, it means that only the unit stowage needs to be checked
            {
                _cargoPlanCheckService.CheckDgWrapperStowage(wrapper);
                DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage(wrapper), "update conflicts");
            }
            DataMessenger.Default.Send(new DgListSelectedItemUpdatedMessage());
        }


        #region Initializing setup private methods

        private void ConnectServices()
        {
            _loadDataService = new LoadDataService();
            _cargoDataService = CargoDataService.GetCargoDataService();
            _cargoPlanCheckService = CargoPlanCheckService.GetCargoPlanCheckService();
            _conflictDataService = ConflictDataService.GetConflictDataService();
            _uiSettingsService = new SettingsService();
            _mappedDialogWindowService = new MappedDialogWindowService(System.Windows.Application.Current.MainWindow);
            _windowDialogService = new WindowDialogService();
            _messageDialogService = MessageDialogService.Connect();
            _titleService = new TitleService();
            _fileNameService = new FileNameService();
        }

        private void RegisterInMessenger()
        {
            DataMessenger.Default.Unregister(_instance);
            DataMessenger.Default.Register<ConflictsToBeCheckedAndUpdatedMessage>(this, OnConflictsToBeCheckedAndUpdatedMessageReceived);
        }

        #endregion

        #region Window dialog service

        /// <summary>
        /// Sets up dialog service and registers viewModels for it.
        /// </summary>
        private void SetupDialogService()
        {
            SetDialogServiceOwner(System.Windows.Application.Current.MainWindow);
            RegisterMappedDialogServiceRelations();
        }

        /// <summary>
        /// Registers windows and their view models in dialog service
        /// </summary>
        private void RegisterMappedDialogServiceRelations()
        {
            _mappedDialogWindowService.Register<WelcomeWindowVM, WelcomeWindow>();
            _mappedDialogWindowService.Register<WinLoginViewModel, winLogin>();
            _mappedDialogWindowService.Register<DialogWindowOptionsViewModel, DialogWindowOptions>();
            _mappedDialogWindowService.Register<CargoReportViewModel, CargoReport>();
        }

        /// <summary>
        /// Creates new dialog service.
        /// </summary>
        /// <param name="owner"></param>
        private void SetDialogServiceOwner(System.Windows.Window owner)
        {
            _mappedDialogWindowService = new MappedDialogWindowService(owner);
        }

        #endregion

        #region Constructor

        private ServicesHandler()
        {
            if (_instance is null)
            {
                ConnectServices();
                RegisterInMessenger();
                SetupDialogService();
            }
        }

        #endregion
    }
}
