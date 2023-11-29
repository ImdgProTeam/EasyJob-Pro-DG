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
        #region Private fields
        // ----- Private fields -----

        private readonly ShipProfileDataService _shipProfileDataService;
        private MainWindowViewModel mainWindowViewModel => ViewModelLocator.MainWindowViewModel;

        #endregion

        #region Public properties
        // ----- Public properties -----

        public ShipProfileWrapper OwnShip { get; private set; }
        public OuterRowWrapper NewOuterRow { get; private set; } 

        #endregion


        #region Constructor
        // ----- Constructor -----

        public ShipProfileWindowVM()
        {
            _shipProfileDataService = new ShipProfileDataService();

            InitiateProperties();
            GetNewShipProfileVM();
            LoadCommands();
        }

        #endregion


        #region Start up logic

        private void InitiateProperties()
        {
            NewOuterRow = new OuterRowWrapper(new Model.Transport.OuterRow());
            NewOuterRow.PropertyChanged -= OnOuterRowChanged;
            NewOuterRow.PropertyChanged += OnOuterRowChanged;

            CellLivingQuarters = new CellPositionWrapper();
            CellHeatedStructures = new CellPositionWrapper();
            CellLSA = new CellPositionWrapper();
            _cellLivingQuartersNewEntry = string.Empty;
            _cellHeatedStructuresNewEntry = string.Empty;
            _cellLsaNewEntry = string.Empty;
        }

        /// <summary>
        /// Creates OwnShip from current ShipProfile
        /// </summary>
        private void GetNewShipProfileVM()
        {
            OwnShip = _shipProfileDataService.CreateShipProfileWrapper();
            OwnShip.IsLoaded = true;
        }

        /// <summary>
        /// Maps commands to their handler methods
        /// </summary>
        private void LoadCommands()
        {
            SaveChangesCommand = new DelegateCommand(SaveChanges, SaveChangesCanExecute);
            CancelChangesCommand = new DelegateCommand(CancelChanges);
            AddCellLivingQuartersCommand = new DelegateCommand(AddCellLivingQuarters, CanAddCellLivingQuarters);
            AddCellHeatedStructuresCommand = new DelegateCommand(AddCellHeatedStructures, CanAddCellHeatedStructures);
            AddCellLSACommand = new DelegateCommand(AddCellLSA, CanAddCellLSA);
        }

        #endregion


        #region Sea-side methods

        // ---- Sea sides methods --------------------------------------

        /// <summary>
        /// Handles changes in <see cref="NewOuterRow"/>.
        /// </summary>
        /// <param name="obj"></param>
        private void OnOuterRowChanged(object sender, PropertyChangedEventArgs e)
        {
            if (TryAddOuterRowToSeaSides()) return;
        }

        /// <summary>
        /// Checks if OuterRow completed and then adds it to SeaSides.
        /// </summary>
        /// <returns>True if completed</returns>
        private bool TryAddOuterRowToSeaSides()
        {
            bool completed = NewOuterRow.Bay != 0 && NewOuterRow.PortMost != 0 && NewOuterRow.StarboardMost != 0;
            if (completed)
            {
                NewOuterRow.PropertyChanged -= OnOuterRowChanged;
                NewOuterRow.PropertyChanged += OwnShip.OnSeaSidePropertyChanged;
                OwnShip.SeaSides.Add(NewOuterRow);

                NewOuterRow = new OuterRowWrapper(new Model.Transport.OuterRow());
                NewOuterRow.PropertyChanged -= OnOuterRowChanged;
                NewOuterRow.PropertyChanged += OnOuterRowChanged;

                OnPropertyChanged(nameof(NewOuterRow));
                return true;
            }
            return false;
        }


        #endregion

        #region Cell position Properties and mehtods

        // ---- CellPosition Properties and methods ------------------------------

        // Block of fields, properties and methods for LivingQuarters

        /// <summary>
        /// Bount to text block to display what is read from <see cref="CellLivingQuartersNewEntry"/>
        /// </summary>
        public CellPositionWrapper CellLivingQuarters { get; set; }
        private string _cellLivingQuartersNewEntry;

        /// <summary>
        /// Bound to input text to create new <see cref="CellLivingQuarters"/> entry.
        /// </summary>
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
            CellLivingQuarters.RemoveCellRequested -= OwnShip.RemoveRowFromLivingQuarters;
            CellLivingQuarters.RemoveCellRequested += OwnShip.RemoveRowFromLivingQuarters;
            OwnShip.LivingQuarters.Add(CellLivingQuarters);

            CellLivingQuarters = new CellPositionWrapper();
            _cellLivingQuartersNewEntry = string.Empty;

            ShipProfileWrapper.ReNumberCellPositionWrapperList(OwnShip.LivingQuarters);
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
            CellHeatedStructures.RemoveCellRequested -= OwnShip.RemoveRowFromHeatedStructures;
            CellHeatedStructures.RemoveCellRequested += OwnShip.RemoveRowFromHeatedStructures;
            OwnShip.HeatedStructures.Add(CellHeatedStructures);

            CellHeatedStructures = new CellPositionWrapper();
            _cellHeatedStructuresNewEntry = string.Empty;

            ShipProfileWrapper.ReNumberCellPositionWrapperList(OwnShip.HeatedStructures);
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
            CellLSA.RemoveCellRequested -= OwnShip.RemoveRowFromLSA;
            CellLSA.RemoveCellRequested += OwnShip.RemoveRowFromLSA;
            OwnShip.LSA.Add(CellLSA);

            CellLSA = new CellPositionWrapper();
            _cellLsaNewEntry = string.Empty;

            ShipProfileWrapper.ReNumberCellPositionWrapperList(OwnShip.LSA);
            OnPropertyChanged("CellLSA");
            OnPropertyChanged("CellLSANewEntry");
        }
        private bool CanAddCellLSA(object obj) => !CellLSA.HasErrorOrEmpty;

        #endregion


        #region Save / Cancel logic

        /// <summary>
        /// Called when 'Save' button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void SaveChanges(object obj)
        {
            OwnShip.AcceptChanges();
            OwnShip.AcceptSpecialPropertiesChanges();
            OwnShip.Model.UpdatePrivateProperties();
            Task.Run(DoSavingJob);
            OwnShip.IsLoaded = false;
        }

        private bool SaveChangesCanExecute(object obj)
        {
            return OwnShip != null && OwnShip.IsChanged || OwnShip.IsSpecialPropertiesChanged;
        }

        /// <summary>
        /// Saves changes to ShipProfile and updates the interface.
        /// </summary>
        private void DoSavingJob()
        {
            //Setting the window is loading mode and starting status bar
            mainWindowViewModel.SetIsLoading(true);
            mainWindowViewModel.StatusBarControl.StartProgressBar(10, "Saving ShipProfile...");
            EasyJob_ProDG.Data.ProgressBarReporter.ReportPercentage = 20;

            //saving job
            _shipProfileDataService.SaveShipProfile();
            EasyJob_ProDG.Data.ProgressBarReporter.ReportPercentage = 50;

            //Sending notification message to DataMessenger
            DataMessenger.Default.Send(new ShipProfileWrapperMessage(), "ship profile saved");

            mainWindowViewModel.StatusBarControl.ProgressPercentage = 90;
            //_isWindowLoaded = false;

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
            OwnShip.RejectChanges();
            OwnShip.RejectSpecialPropertiesChanges();
            OwnShip.IsLoaded = false;
        }

        #endregion

        #region Commands

        //-------------- Commands ----------------- //

        public ICommand SaveChangesCommand { get; private set; }
        public ICommand CancelChangesCommand { get; private set; }
        public ICommand AddCellLivingQuartersCommand { get; private set; }
        public ICommand AddCellHeatedStructuresCommand { get; private set; }
        public ICommand AddCellLSACommand { get; private set; }

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
