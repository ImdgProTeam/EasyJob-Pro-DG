//********************************************************//
//   Public class to make changes in current ShipProfile  //
//   * Used with ShipProfileWindow                        //
//                                                        //
//********************************************************//

using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class ShipProfileWindowVM : Observable, IDataErrorInfo
    {
        private bool _isWindowLoaded;

        readonly ShipProfileDataService _shipProfileDataService;
        private MainWindowViewModel mainWindowViewModel => ViewModelLocator.MainWindowViewModel;

        public ShipProfileWrapper OwnShip { get; set; }
        public OuterRowWrapper NewOuterRow { get; private set; } = new OuterRowWrapper(new Model.Transport.OuterRow());


        #region Constructors

        // ---- Constructors --------------------------------------------------------------

        public ShipProfileWindowVM()
        {
            _isWindowLoaded = false;
            _shipProfileDataService = new ShipProfileDataService();

            GetNewShipProfileVM();

            LoadCommands();

            RegisterMessages();
            InvalidateCommands();
        }

        private void InvalidateCommands()
        {

        }

        #endregion


        #region Start up logic

        /// <summary>
        /// Creates OwnShip from current ShipProfile
        /// </summary>
        private void GetNewShipProfileVM()
        {
            OwnShip = _shipProfileDataService.CreateShipProfileWrapper();

            CellLivingQuarters = new CellPositionWrapper();
            _cellLivingQuartersNewEntry = string.Empty;
            CellHeatedStructures = new CellPositionWrapper();
            _cellHeatedStructuresNewEntry = string.Empty;
            CellLSA = new CellPositionWrapper();
            _cellLsaNewEntry = string.Empty;
        }

        /// <summary>
        /// Maps commands to their handler methods
        /// </summary>
        private void LoadCommands()
        {
            SaveChangesCommand = new DelegateCommand(SaveChanges, SaveChangesCanExecute);
            CancelChangesCommand = new DelegateCommand(CancelChanges);
            WindowLoaded = new DelegateCommand(OnWindowLoaded);
            WindowClosed = new DelegateCommand(OnWindowClosed);
            AddCellLivingQuartersCommand = new DelegateCommand(AddCellLivingQuarters, CanAddCellLivingQuarters);
            AddCellHeatedStructuresCommand = new DelegateCommand(AddCellHeatedStructures, CanAddCellHeatedStructures);
            AddCellLSACommand = new DelegateCommand(AddCellLSA, CanAddCellLSA);
        }

        /// <summary>
        /// Registers messages in DataMessenger
        /// </summary>
        private void RegisterMessages()
        {
            DataMessenger.Default.UnregisterAll(this);
            DataMessenger.Default.Register<ShipProfileWrapperMessage>(this, OnOuterRowChanged, "Outer row changed");
            DataMessenger.Default.Register<MessageFromDummy>(this, OnDummyAccommodationChanged, "Dummy Accommodation changed");
        }

        #endregion


        #region Sea-side methods

        // ---- Sea sides methods --------------------------------------

        /// <summary>
        /// Checks if OuterRow completed and then adds it to SeaSides.
        /// </summary>
        /// <returns>True if completed</returns>
        private bool TryAddOuterRowToTempShip()
        {
            bool completed = NewOuterRow.Bay != 0 && NewOuterRow.PortMost != 0 && NewOuterRow.StarboardMost != 0;
            if (completed)
            {
                OwnShip.SeaSides.Add(NewOuterRow);
                NewOuterRow = new OuterRowWrapper(new Model.Transport.OuterRow());
                OnPropertyChanged("NewOuterRow");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Handles changes in OuterRows.
        /// Called when received "Outer row changed" ShipProfileWrapperMessage.
        /// </summary>
        /// <param name="obj"></param>
        private void OnOuterRowChanged(ShipProfileWrapperMessage obj)
        {
            if (_isWindowLoaded)
            {
                if (TryAddOuterRowToTempShip()) return;
                else RemoveEmptyRowFromSeaSides();
                ReverseAllBaysBayChange();
            }
        }

        /// <summary>
        /// Turns first entry bay number to '0', if not yet changed.
        /// </summary>
        private void ReverseAllBaysBayChange()
        {
            if (OwnShip.SeaSides[0].Bay != 0)
                OwnShip.SeaSides[0].Bay = 0;
        }

        /// <summary>
        /// Removes an empty OuterRow from collection SeaSideObservable.
        /// </summary>
        private void RemoveEmptyRowFromSeaSides()
        {
            for (int i = 0; i < OwnShip.SeaSides.Count; i++)
            {
                var entry = OwnShip.SeaSides[i];
                bool empty = (entry.Bay == 0 && entry.PortMost == 0 && entry.StarboardMost == 0);
                if (empty)
                {
                    OwnShip.SeaSides.Remove(entry);
                    return;
                }
            }
        }

        #endregion

        #region Accommodation methods

        // ---- BaysSurroundingSuperstructure methods -----------------------------------

        /// <summary>
        /// Adds bays from OwnShip into AccommodationBaysObservableCollection
        /// </summary>
        private void CreateAccommodationDummyObservableCollection()
        {
            if (OwnShip.SuperstructuresBays.Count > 0) return;

            int i = 1;
            foreach (var bay in OwnShip.SuperstructuresBays)
            {
                OwnShip.SuperstructuresBays.Add(new DummySuperstructure(i, bay.Bay));
                i++;
            }
        }

        /// <summary>
        /// Blank method.
        /// Handles changes in DummySuperstructure.
        /// Called by Messenger when DummyChange message received.
        /// </summary>
        /// <param name="obj"></param>
        private void OnDummyAccommodationChanged(MessageFromDummy obj)
        {

        }

        #endregion

        #region Cell position Properties and mehtods

        // ---- CellPosition Properties and methods ------------------------------

        // Block of fields, properties and methods for LivingQuarters
        public CellPositionWrapper CellLivingQuarters { get; set; }
        private string _cellLivingQuartersNewEntry;
        public string CellLivingQuartersNewEntry
        {
            get { return _cellLivingQuartersNewEntry; }
            set
            {
                _cellLivingQuartersNewEntry = value;
                CellLivingQuarters.DisplayPosition = value;
                OnPropertyChanged("CellLivingQuarters");
            }
        }
        private void AddCellLivingQuarters(object obj)
        {
            OwnShip.LivingQuarters.Add(CellLivingQuarters);
            CellLivingQuarters = new CellPositionWrapper();
            _cellLivingQuartersNewEntry = string.Empty;
            ShipProfileWrapper.OnAddNewCell(OwnShip.LivingQuarters);
            OnPropertyChanged("CellLivingQuarters");
            OnPropertyChanged("CellLivingQuartersNewEntry");
        }
        private bool CanAddCellLivingQuarters(object obj) => !CellLivingQuarters.HasErrorOrEmpty;

        // Block of fields, properties and methods for HeatedStructures
        public CellPositionWrapper CellHeatedStructures { get; set; }
        private string _cellHeatedStructuresNewEntry;
        public string CellHeatedStructuresNewEntry
        {
            get { return _cellHeatedStructuresNewEntry; }
            set
            {
                _cellHeatedStructuresNewEntry = value;
                CellHeatedStructures.DisplayPosition = value;
                OnPropertyChanged("CellHeatedStructures");
            }
        }
        private void AddCellHeatedStructures(object obj)
        {
            OwnShip.HeatedStructures.Add(CellHeatedStructures);
            CellHeatedStructures = new CellPositionWrapper();
            _cellHeatedStructuresNewEntry = string.Empty;
            ShipProfileWrapper.OnAddNewCell(OwnShip.HeatedStructures);
            OnPropertyChanged("CellHeatedStructures");
            OnPropertyChanged("CellHeatedStructuresNewEntry");
        }
        private bool CanAddCellHeatedStructures(object obj) => !CellHeatedStructures.HasErrorOrEmpty;

        // Block of fields, properties and methods for LSA
        public CellPositionWrapper CellLSA { get; set; }
        private string _cellLsaNewEntry;
        public string CellLSANewEntry
        {
            get { return _cellLsaNewEntry; }
            set
            {
                _cellLsaNewEntry = value;
                CellLSA.DisplayPosition = value;
                OnPropertyChanged("CellLSA");
            }
        }
        private void AddCellLSA(object obj)
        {
            OwnShip.LSA.Add(CellLSA);
            CellLSA = new CellPositionWrapper();
            _cellLsaNewEntry = string.Empty;
            ShipProfileWrapper.OnAddNewCell(OwnShip.LSA);
            OnPropertyChanged("CellLSA");
            OnPropertyChanged("CellLSANewEntry");
        }
        private bool CanAddCellLSA(object obj) => !CellLSA.HasErrorOrEmpty;

        #endregion


        #region Events

        // ---- Events and their methods ---------------------------------------------------------------

        private void OnWindowLoaded(object obj)
        {
            _isWindowLoaded = true;
            CreateAccommodationDummyObservableCollection();
        }

        private void OnWindowClosed(object obj)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when 'Save' button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void SaveChanges(object obj)
        {
            Task.Run(DoSavingJob);
        }
        private bool SaveChangesCanExecute(object obj)
        {
            return OwnShip.IsChanged;
        }

        /// <summary>
        /// Saves changes to ShipProfile and updates the interface.
        /// </summary>
        private void DoSavingJob()
        {
            //Setting the window is loading mode and starting status bar
            mainWindowViewModel.SetIsLoading(true);
            mainWindowViewModel.StatusBarControl.StartProgressBar(10, "Saving ShipProfile...");

            //saving job
            _shipProfileDataService.SaveShipProfile();

            mainWindowViewModel.StatusBarControl.ProgressPercentage = 90;
            GetNewShipProfileVM();
            _isWindowLoaded = false;

            //Completing with visual loading effects
            mainWindowViewModel.StatusBarControl.ProgressPercentage = 100;
            mainWindowViewModel.SetIsLoading(false);
        }

        /// <summary>
        /// Called when 'Cancel' button is clicked.
        /// </summary>
        /// <param name="parameter"></param>
        private void CancelChanges(object parameter)
        {
            OwnShip = _shipProfileDataService.CreateShipProfileWrapper();
            _isWindowLoaded = false;
        }

        #endregion

        #region Commands

        //-------------- Commands ----------------- //

        public ICommand WindowLoaded { get; set; }
        public ICommand WindowClosed { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand CancelChangesCommand { get; set; }
        public ICommand AddCellLivingQuartersCommand { get; set; }
        public ICommand AddCellHeatedStructuresCommand { get; set; }
        public ICommand AddCellLSACommand { get; set; }

        #endregion

        #region IDataErrorInfo

        //------------- IDataErrorInfo -------------//

        public string Error => string.Empty;
        public string this[string columnName] => ValidateProperty(columnName);

        protected string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case "CellLivingQuartersNewEntry":
                    if (CellLivingQuarters.HasError)
                        return "Enter a valid position";
                    else
                        return string.Empty;

                case "CellHeatedStructuresNewEntry":
                    if (CellHeatedStructures.HasError)
                        return "Enter a valid position";
                    else
                        return string.Empty;

                case "CellLSANewEntry":
                    if (CellLSA.HasError)
                        return "Enter a valid position";
                    else
                        return string.Empty;

                default:
                    return string.Empty;
            }
        }

        #endregion

    }
}
