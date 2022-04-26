//********************************************************//
//   Public class to make changes in current ShipProfile  //
//   * Used with ShipProfileWindow                        //
//                                                        //
//********************************************************//

using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using EasyJob_ProDG.UI.Wrapper.Dummies;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class ShipProfileWindowVM : Observable, IDataErrorInfo
    {
        readonly ShipProfileDataService _shipProfileDataService;
        private bool _isWindowLoaded;
        private bool _isChangeCancelled;

        public ShipProfileWrapper TempShip { get; set; }
        public OuterRowWrapper NewOuterRow { get; private set; } = new OuterRowWrapper();

        // ---- Constructors --------------------------------------------------------------

        public ShipProfileWindowVM()
        {
            _isWindowLoaded = false;
            _shipProfileDataService = new ShipProfileDataService();

            GetNewShipProfileVM();

            LoadCommands();

            RegisterMessages();
        }


        // ---- Methods --------------------------------------------------------------
        
        /// <summary>
        /// Creates TempShip from current ShipProfile
        /// </summary>
        private void GetNewShipProfileVM()
        {
            TempShip = _shipProfileDataService.CreateShipProfileWrapper();

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
            SaveChangesCommand = new DelegateCommand(SaveChanges);
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
            //DataMessenger.Default.Register<CancelChangesMessage>(this, OnCancelChangesMessageReceived, "ship profile");
            DataMessenger.Default.Register<ShipProfileWrapperMessage>(this, OnOuterRowChanged, "Outer row changed");
            DataMessenger.Default.Register<MessageFromDummy>(this, OnDummyAccommodationChanged, "Dummy Accommodation changed");
        }


        // ---- Sea sides methods --------------------------------------

        /// <summary>
        /// Checks if OuterRow completed and then adds it to SeaSidesObservable.
        /// </summary>
        /// <returns>True if completed</returns>
        private bool TryAddOuterRowToTempShip()
        {
            bool completed = NewOuterRow.Bay != 0 && NewOuterRow.PortMost != 0 && NewOuterRow.StarboardMost != 0;
            if (completed)
            {
                TempShip.SeaSidesObservable.Add(NewOuterRow);
                NewOuterRow = new OuterRowWrapper();
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
                else RemoveEmptyRowFromSeaSidesObservable();
                ReverseAllBaysBayChange();
            }
        }

        /// <summary>
        /// Turns first entry bay number to '0', if not yet changed.
        /// </summary>
        private void ReverseAllBaysBayChange()
        {
            if (TempShip.SeaSidesObservable[0].Bay != 0)
                TempShip.SeaSidesObservable[0].Bay = 0;
        }

        /// <summary>
        /// Removes an empty OuterRow from collection SeaSideObservable.
        /// </summary>
        private void RemoveEmptyRowFromSeaSidesObservable()
        {
            for (int i = 0; i < TempShip.SeaSidesObservable.Count; i++)
            {
                var entry = TempShip.SeaSidesObservable[i];
                bool empty = (entry.Bay == 0 && entry.PortMost == 0 && entry.StarboardMost == 0);
                if (empty)
                {
                    TempShip.SeaSidesObservable.Remove(entry);
                    return;
                }
            }
        }


        // ---- Accommodation methods -----------------------------------

        /// <summary>
        /// Adds bays from TempShip into AccommodationBaysObservableCollection
        /// </summary>
        private void CreateAccommodationDummyObservableCollection()
        {
            if (TempShip.AccommodationBaysObservable.Count > 0) return;

            int i = 1;
            foreach (byte bay in TempShip.AccommodationBays)
            {
                TempShip.AccommodationBaysObservable.Add(new DummyAccommodation(i, bay));
                i++;
            }
        }

        /// <summary>
        /// Blank method.
        /// Handles changes in DummyAccommodation.
        /// Called by Messenger when DummyChange message received.
        /// </summary>
        /// <param name="obj"></param>
        private void OnDummyAccommodationChanged(MessageFromDummy obj)
        {

        }


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
            TempShip.LivingQuartersObservable.Add(CellLivingQuarters);
            CellLivingQuarters = new CellPositionWrapper();
            _cellLivingQuartersNewEntry = string.Empty;
            ShipProfileWrapper.OnAddNewCell(TempShip.LivingQuartersObservable);
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
            TempShip.HeatedStructuresObservable.Add(CellHeatedStructures);
            CellHeatedStructures = new CellPositionWrapper();
            _cellHeatedStructuresNewEntry = string.Empty;
            ShipProfileWrapper.OnAddNewCell(TempShip.HeatedStructuresObservable);
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
            TempShip.LSAObservable.Add(CellLSA);
            CellLSA = new CellPositionWrapper();
            _cellLsaNewEntry = string.Empty;
            ShipProfileWrapper.OnAddNewCell(TempShip.LSAObservable);
            OnPropertyChanged("CellLSA");
            OnPropertyChanged("CellLSANewEntry");
        }
        private bool CanAddCellLSA(object obj) => !CellLSA.HasErrorOrEmpty;


        // ---- Events and their methods ---------------------------------------------------------------

        private void OnWindowLoaded(object obj)
        {
            _isWindowLoaded = true;
            _isChangeCancelled = false;
            CreateAccommodationDummyObservableCollection();
            //CreateHoldsObservableCollection();
        }

        private void OnWindowClosed(object obj)
        {
            //throw new NotImplementedException();
        }

        private void SaveChanges(object obj)
        {
            _shipProfileDataService.SaveShipProfile();
            GetNewShipProfileVM();

            _isWindowLoaded = false;
            _isChangeCancelled = true;
        }

        private void CancelChanges(object parameter)
        {
            TempShip = _shipProfileDataService.CreateShipProfileWrapper();
            _isWindowLoaded = false;
            _isChangeCancelled = true;
        }


        //-------------- Commands ----------------- //

        public ICommand WindowLoaded { get; set; }
        public ICommand WindowClosed { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand CancelChangesCommand { get; set; }
        public ICommand AddCellLivingQuartersCommand { get; set; }
        public ICommand AddCellHeatedStructuresCommand { get; set; }
        public ICommand AddCellLSACommand { get; set; }


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

    }
}
