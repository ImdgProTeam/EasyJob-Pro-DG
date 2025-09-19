using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.Windows.ToolWindows
{
    internal class SelectionControlViewModel : Observable
    {
        #region Internal members

        internal bool IsNoPropertySelected => string.IsNullOrWhiteSpace(SelectedNumber)
    && string.IsNullOrWhiteSpace(Position) && string.IsNullOrWhiteSpace(OldPosition)
    && !IsUnderdeck && !isOnDeck && !IsDG && !IsNotDg && !IsReefer && !isNonReefer
    && string.IsNullOrWhiteSpace(SelectedPOL) && string.IsNullOrWhiteSpace(SelectedPOD) 
            && string.IsNullOrWhiteSpace(SelectedOperator)
    && string.IsNullOrWhiteSpace(SelectedFinalDestination) && string.IsNullOrWhiteSpace(SelectedContainerType)
    && !IsOpenType && !HasRemarks && !HasNoRemarks && !IsNewInPlan && !NotIsNewInPlan
    && !HasPositionChanged && !NotHasPositionChanged && !HasUpdated && !NotHasUpdated
    && !IsLocked && !NotIsLocked && !IsToImport && !NotIsToImport && !HasPODchanged && !NotHasPODchanged
    && !HasTypeChanged && !NotHasTypeChanged && !IsToBeKeptInPlan && !NotIsToBeKeptInPlan
    && !SelectedUNNO.HasValue && SelectedUNNOs.Count == 0
    && string.IsNullOrWhiteSpace(SelectedDgClass) && SelectedDgClasses.Count == 0
    && !FlashPoint.HasValue && !IsConflicted && !NotIsConflicted && !HasDgRemarks && !NotHasDgRemarks
    && !HasCommodity && !NotHasCommodity && !HasSpecial && !NotHasSpecial
    && !HasReeferRemarks && !NotHasReeferRemarks && !SetPoint.HasValue
    && string.IsNullOrWhiteSpace(FreeText);
        private bool _isNoPropertySelected = true;

        internal event EventHandler SelectionChanged; 
        internal event EventHandler CallApply; 

        #endregion

        // ----- Bindable properties -----
        #region Container number
        public List<string> ContainerNumbers { get; private set; }
        public string SelectedNumber
        {
            get => selectedNumber;
            set
            {
                selectedNumber = value;
                ChangedSelection();
            }
        }

        #endregion

        #region Location
        // Location
        public string Position
        {
            get => position;
            set
            {
                position = value;
                CellPosition.DisplayPosition = value;
                OnPropertyChanged(nameof(PositionDescription));
                ChangedSelection();
            }
        }

        internal CellPositionWrapper CellPosition;
        public string PositionDescription => CellPosition.DisplayPosition;

        public string OldPosition
        {
            get => oldPosition;
            set
            {
                oldPosition = value;
                CellOldPosition.DisplayPosition = value;
                OnPropertyChanged(nameof(OldPositionDescription));
                ChangedSelection();
            }

        }

        internal CellPositionWrapper CellOldPosition;
        public string OldPositionDescription => CellOldPosition.DisplayPosition;

        public bool IsUnderdeck
        {
            get => isUnderdeck;
            set
            {
                isUnderdeck = value;
                ChangedSelection();
                if (!isUnderdeck) return;

                isOnDeck = false;
                OnPropertyChanged(nameof(IsOnDeck));
            }
        }

        public bool IsOnDeck
        {
            get => isOnDeck;
            set
            {
                isOnDeck = value;
                ChangedSelection();
                if (!isOnDeck) return;

                isUnderdeck = false;
                OnPropertyChanged(nameof(IsUnderdeck));
            }
        }

        #endregion

        #region Type of cargo
        // Type of cargo

        public bool IsDG
        {
            get { return isDg; }
            set
            {
                isDg = value;
                ChangedSelection();
                if (!isDg) return;

                isNotDg = false;
                OnPropertyChanged(nameof(IsNotDg));
            }
        }

        public bool IsNotDg
        {
            get { return isNotDg; }
            set
            {
                isNotDg = value;
                ChangedSelection();
                if (!value) return;

                isDg = false;
                OnPropertyChanged(nameof(IsDG));
            }
        }

        public bool IsReefer
        {
            get { return isReefer; }
            set
            {
                isReefer = value;
                ChangedSelection();
                if (!value) return;

                isNonReefer = false;
                OnPropertyChanged(nameof(IsNonReefer));
            }
        }

        public bool IsNonReefer
        {
            get { return isNonReefer; }
            set
            {
                isNonReefer = value;
                ChangedSelection();
                if (!value) return;

                isReefer = false;
                OnPropertyChanged(nameof(IsReefer));
            }
        }

        #endregion

        #region Other container properies
        // Container

        public List<string> POLs { get; private set; }
        public string SelectedPOL
        {
            get => selectedPOL;
            set
            {
                selectedPOL = value;
                ChangedSelection();
            }
        }

        public List<string> PODs { get; private set; }
        public string SelectedPOD
        {
            get => selectedPOD;
            set
            {
                selectedPOD = value;
                ChangedSelection();
            }
        }

        public List<string> FinalDestinations { get; private set; }
        public string SelectedFinalDestination
        {
            get => selectedFinalDestination;
            set
            {
                selectedFinalDestination = value;
                ChangedSelection();
            }
        }

        public List<string> Operators { get; private set; }
        public string SelectedOperator
        {
            get => selectedOperator;
            set
            {
                selectedOperator = value;
                ChangedSelection();
            }
        }

        public List<string> ContainerTypes { get; private set; }
        public string SelectedContainerType
        {
            get => selectedContainerType;
            set
            {
                selectedContainerType = value;
                ChangedSelection();
            }
        }

        public bool IsOpenType
        {
            get => isOpenType;
            set
            {
                isOpenType = value;
                ChangedSelection();
            }
        }

        public bool HasRemarks
        {
            get { return hasRemarks; }
            set
            {
                hasRemarks = value;
                ChangedSelection();
                if (!value) return;

                hasNoRemarks = false;
                OnPropertyChanged(nameof(HasNoRemarks));
            }
        }

        public bool HasNoRemarks
        {
            get { return hasNoRemarks; }
            set
            {
                hasNoRemarks = value;
                ChangedSelection();
                if (!value) return;

                hasRemarks = false;
                OnPropertyChanged(nameof(HasRemarks));
            }
        }

        #endregion

        #region Changes (updates)
        // Changes

        public bool IsNewInPlan
        {
            get { return isNewInPlan; }
            set
            {
                isNewInPlan = value;
                ChangedSelection();
                if (!value) return;

                notIsNewInPlan = false;
                OnPropertyChanged(nameof(NotIsNewInPlan));
            }
        }

        public bool NotIsNewInPlan
        {
            get { return notIsNewInPlan; }
            set
            {
                notIsNewInPlan = value;
                ChangedSelection();
                if (!value) return;

                isNewInPlan = false;
                OnPropertyChanged(nameof(IsNewInPlan));
            }
        }

        public bool HasPositionChanged
        {
            get { return hasPositionChanged; }
            set
            {
                hasPositionChanged = value;
                ChangedSelection();
                if (!value) return;

                notHasPositionChanged = false;
                OnPropertyChanged(nameof(NotHasPositionChanged));
            }
        }

        public bool NotHasPositionChanged
        {
            get { return notHasPositionChanged; }
            set
            {
                notHasPositionChanged = value;
                ChangedSelection();
                if (!value) return;

                hasPositionChanged = false;
                OnPropertyChanged(nameof(HasPositionChanged));
            }
        }

        public bool HasUpdated
        {
            get { return hasUpdated; }
            set
            {
                hasUpdated = value;
                ChangedSelection();
                if (!value) return;

                notHasUpdated = false;
                OnPropertyChanged(nameof(NotHasUpdated));
            }
        }

        public bool NotHasUpdated
        {
            get { return notHasUpdated; }
            set
            {
                notHasUpdated = value;
                ChangedSelection();
                if (!value) return;

                hasUpdated = false;
                OnPropertyChanged(nameof(HasUpdated));
            }
        }

        public bool IsLocked
        {
            get { return isLocked; }
            set
            {
                isLocked = value;
                ChangedSelection();
                if (!value) return;

                notIsLocked = false;
                OnPropertyChanged(nameof(NotIsLocked));
            }
        }

        public bool NotIsLocked
        {
            get { return notIsLocked; }
            set
            {
                notIsLocked = value;
                ChangedSelection();
                if (!value) return;

                isLocked = false;
                OnPropertyChanged(nameof(IsLocked));
            }
        }

        public bool IsToImport
        {
            get { return isToImport; }
            set
            {
                isToImport = value;
                ChangedSelection();
                if (!value) return;

                notIsToImport = false;
                OnPropertyChanged(nameof(NotIsToImport));
            }
        }

        public bool NotIsToImport
        {
            get { return notIsToImport; }
            set
            {
                notIsToImport = value;
                ChangedSelection();
                if (!value) return;

                isToImport = false;
                OnPropertyChanged(nameof(IsToImport));
            }
        }

        public bool HasPODchanged
        {
            get { return hasPODchanged; }
            set
            {
                hasPODchanged = value;
                ChangedSelection();
                if (!value) return;

                notHasPODchanged = false;
                OnPropertyChanged(nameof(NotHasPODchanged));
            }
        }

        public bool NotHasPODchanged
        {
            get { return notHasPODchanged; }
            set
            {
                notHasPODchanged = value;
                ChangedSelection();
                if (!value) return;

                hasPODchanged = false;
                OnPropertyChanged(nameof(HasPODchanged));
            }
        }

        public bool HasTypeChanged
        {
            get { return hasTypeChanged; }
            set
            {
                hasTypeChanged = value;
                ChangedSelection();
                if (!value) return;

                notHasTypeChanged = false;
                OnPropertyChanged(nameof(NotHasTypeChanged));
            }
        }

        public bool NotHasTypeChanged
        {
            get { return notHasTypeChanged; }
            set
            {
                notHasTypeChanged = value;
                ChangedSelection();
                if (!value) return;

                hasTypeChanged = false;
                OnPropertyChanged(nameof(HasTypeChanged));
            }
        }

        public bool IsToBeKeptInPlan
        {
            get { return isToBeKeptInPlan; }
            set
            {
                isToBeKeptInPlan = value;
                ChangedSelection();
                if (!value) return;

                notIsToBeKeptInPlan = false;
                OnPropertyChanged(nameof(NotIsToBeKeptInPlan));
            }
        }

        public bool NotIsToBeKeptInPlan
        {
            get { return notIsToBeKeptInPlan; }
            set
            {
                notIsToBeKeptInPlan = value;
                ChangedSelection();
                if (!value) return;

                isToBeKeptInPlan = false;
                OnPropertyChanged(nameof(IsToBeKeptInPlan));
            }
        }

        #endregion

        #region Dg
        // Dg

        //UNNO
        public List<int> UNNOs { get; private set; }
        public int? SelectedUNNO
        {
            get => selectedUNNO;
            set
            {
                selectedUNNO = value;
                ChangedSelection();
            }
        }
        public ObservableCollection<int> SelectedUNNOs { get; private set; }

        public ICommand AddSelectedUnnoCommand { get; private set; }
        public ICommand AddSelectedUnnoOrApplyCommand { get; private set; }
        public ICommand RemoveUnnoCommand { get; private set; }
        public ICommand ClearSelectedUnnosCommand { get; private set; }

        private void OnAddSelectedUnnoExecuted(object obj)
        {
            if (!SelectedUNNO.HasValue && SelectedUNNOs.Count > 0)
            {
                CallApply.Invoke(this, new EventArgs());
                return;
            }

            if (SelectedUNNOs.Contains(SelectedUNNO.Value))
                return;

            InsertValueSorted(SelectedUNNO.Value, SelectedUNNOs);
            SelectedUNNO = null;
            OnPropertyChanged(nameof(SelectedUNNO));
            OnPropertyChanged(nameof(SelectedUNNOs));
        }
        private bool OnAddSelectedUnnoCanExecute(object obj)
        {
            return SelectedUNNO.HasValue;
        }

        private void OnRemoveUnnoExecuted(object obj)
        {
            SelectedUNNOs.Remove((int)obj);
            OnPropertyChanged(nameof(SelectedUNNOs));
            ChangedSelection();
        }

        private void OnClearSelectedUnnosExecuted(object obj)
        {
            SelectedUNNOs.Clear();
            OnPropertyChanged(nameof(SelectedUNNOs));
            ChangedSelection();
        }

        private bool OnClearSelectedUnnosCanExecute(object obj)
        {
            return SelectedUNNOs.Count > 0;
        }


        // Dg class
        public List<string> DgClasses { get; private set; }
        public string SelectedDgClass
        {
            get => selectedDgClass;
            set
            {
                selectedDgClass = value;
                ChangedSelection();
            }
        }
        public ObservableCollection<string> SelectedDgClasses { get; private set; }

        public ICommand AddSelectedDgClassCommand { get; private set; }
        public ICommand AddSelectedDgClassOrApplyCommand { get; private set; }
        public ICommand RemoveDgClassCommand { get; private set; }
        public ICommand ClearSelectedDgClassesCommand { get; private set; }

        private void OnAddSelectedDgClassExecuted(object obj)
        {
            if (string.IsNullOrWhiteSpace(SelectedDgClass) && SelectedDgClasses.Count > 0)
            {
                CallApply.Invoke(this, new EventArgs());
                return;
            }

            if (SelectedDgClasses.Contains(SelectedDgClass))
                return;

            InsertValueSorted(SelectedDgClass, SelectedDgClasses);
            SelectedDgClass = null;
            OnPropertyChanged(nameof(SelectedDgClass));
            OnPropertyChanged(nameof(SelectedDgClasses));
        }
        private bool OnAddSelectedDgClassCanExecute(object obj)
        {
            return !string.IsNullOrWhiteSpace(SelectedDgClass);
        }

        private void OnRemoveDgClassExecuted(object obj)
        {
            SelectedDgClasses.Remove((string)obj);
            OnPropertyChanged(nameof(SelectedDgClasses));
            ChangedSelection();
        }
        private void OnClearSelectedDgClassesExecuted(object obj)
        {
            SelectedDgClasses.Clear();
            OnPropertyChanged(nameof(SelectedDgClasses));
            ChangedSelection();
        }

        private bool OnClearSelectedDgClassesCanExecute(object obj)
        {
            return SelectedDgClasses.Count > 0;
        }

        public decimal? FlashPoint
        {
            get => flashPoint;
            set
            {
                flashPoint = value;
                ChangedSelection();
            }
        }

        public bool IsFPlessthan
        {
            get { return isFPlessthan; }
            set
            {
                isFPlessthan = value;
                if (!value) return;

                isFPmorethan = false;
                OnPropertyChanged(nameof(IsFPmorethan));
            }
        }

        public bool IsFPmorethan
        {
            get { return isFPmorethan; }
            set
            {
                isFPmorethan = value;
                if (!value) return;

                isFPlessthan = false;
                OnPropertyChanged(nameof(IsFPlessthan));
            }
        }

        public bool IsConflicted
        {
            get { return isConflicted; }
            set
            {
                isConflicted = value;
                ChangedSelection();
                if (!value) return;

                notIsConflicted = false;
                OnPropertyChanged(nameof(NotIsConflicted));
            }
        }

        public bool NotIsConflicted
        {
            get { return notIsConflicted; }
            set
            {
                notIsConflicted = value;
                ChangedSelection();
                if (!value) return;

                isConflicted = false;
                OnPropertyChanged(nameof(IsConflicted));
            }
        }

        public bool HasDgRemarks
        {
            get { return hasDgRemarks; }
            set
            {
                hasDgRemarks = value;
                ChangedSelection();
                if (!value) return;

                notHasDgRemarks = false;
                OnPropertyChanged(nameof(NotHasDgRemarks));
            }
        }


        public bool NotHasDgRemarks
        {
            get { return notHasDgRemarks; }
            set
            {
                notHasDgRemarks = value;
                ChangedSelection();
                if (!value) return;

                hasDgRemarks = false;
                OnPropertyChanged(nameof(HasDgRemarks));
            }
        }

        #endregion

        #region Reefer
        // Reefer

        public bool HasCommodity
        {
            get { return hasCommodity; }
            set
            {
                hasCommodity = value;
                ChangedSelection();
                if (!value) return;

                notHasCommodity = false;
                OnPropertyChanged(nameof(NotHasCommodity));
            }
        }

        public bool NotHasCommodity
        {
            get { return notHasCommodity; }
            set
            {
                notHasCommodity = value;
                ChangedSelection();
                if (!value) return;

                hasCommodity = false;
                OnPropertyChanged(nameof(HasCommodity));
            }
        }

        public bool HasSpecial
        {
            get { return hasSpecial; }
            set
            {
                hasSpecial = value;
                ChangedSelection();
                if (!value) return;

                notHasSpecial = false;
                OnPropertyChanged(nameof(NotHasSpecial));
            }
        }

        public bool NotHasSpecial
        {
            get { return notHasSpecial; }
            set
            {
                notHasSpecial = value;
                ChangedSelection();
                if (!value) return;

                hasSpecial = false;
                OnPropertyChanged(nameof(HasSpecial));
            }
        }

        public bool HasReeferRemarks
        {
            get { return hasReeferRemarks; }
            set
            {
                hasReeferRemarks = value;
                ChangedSelection();
                if (!value) return;

                notHasReeferRemarks = false;
                OnPropertyChanged(nameof(NotHasReeferRemarks));
            }
        }

        public bool NotHasReeferRemarks
        {
            get { return notHasReeferRemarks; }
            set
            {
                notHasReeferRemarks = value;
                ChangedSelection();
                if (!value) return;

                hasReeferRemarks = false;
                OnPropertyChanged(nameof(HasReeferRemarks));
            }
        }

        public decimal? SetPoint
        {
            get => setPoint;
            set
            {
                setPoint = value;
                ChangedSelection();
            }
        }

        public bool IsSPlessthan
        {
            get { return isSPlessthan; }
            set
            {
                isSPlessthan = value;
                if (!value) return;

                isSPmorethan = false;
                OnPropertyChanged(nameof(IsSPmorethan));
            }
        }

        public bool IsSPmorethan
        {
            get { return isSPmorethan; }
            set
            {
                isSPmorethan = value;
                if (!value) return;

                isSPlessthan = false;
                OnPropertyChanged(nameof(IsSPlessthan));
            }
        }
        #endregion

        #region Other properties
        // Free text
        public string FreeText
        {
            get => freeText;
            set
            {
                freeText = value;
                ChangedSelection();
            }
        }

        #endregion


        #region Properties fields

        private string position;
        private string oldPosition;
        private bool isUnderdeck;
        private bool isOnDeck;

        private bool isDg;
        private bool isNotDg;
        private bool isReefer;
        private bool isNonReefer;

         private bool hasRemarks;
        private bool hasNoRemarks;       

        private bool isNewInPlan;
        private bool notIsNewInPlan;
        private bool hasPositionChanged;
        private bool notHasPositionChanged;
        private bool hasUpdated;
        private bool notHasUpdated;
        private bool isLocked;
        private bool notIsLocked;
        private bool isToImport;
        private bool notIsToImport;
        private bool hasPODchanged;
        private bool notHasPODchanged;
        private bool hasTypeChanged;
        private bool notHasTypeChanged;
        private bool isToBeKeptInPlan;
        private bool notIsToBeKeptInPlan;

        private bool isConflicted;
        private bool notIsConflicted;
        private bool hasDgRemarks;
        private bool notHasDgRemarks;

        private bool hasCommodity;
        private bool notHasCommodity;
        private bool hasSpecial;
        private bool notHasSpecial;
        private bool hasReeferRemarks;
        private bool notHasReeferRemarks;
        private decimal? setPoint;
        private bool isSPlessthan;
        private bool isSPmorethan;
        private string selectedNumber;
        private string selectedPOL;
        private string selectedPOD;
        private string selectedFinalDestination;
        private string selectedOperator;
        private string selectedContainerType;
        private bool isOpenType;
        private int? selectedUNNO;
        private string selectedDgClass;
        private decimal? flashPoint;        
        private bool isFPlessthan;
        private bool isFPmorethan;
        private string freeText;

        #endregion

        // ----- Methods -----
        #region Public methods

        public void CreateLists(CargoPlanWrapper cargoPlan)
        {
            ContainerNumbers =
                [.. cargoPlan.Containers.Select(c => c.ContainerNumber).Distinct().OrderBy(s => s)];

            POLs =
                [.. cargoPlan.Containers.Select(c => c.POL).Distinct().OrderBy(s => s)];

            PODs =
                [.. cargoPlan.Containers.Select(c => c.POD).Distinct().OrderBy(s => s)];

            FinalDestinations =
                [.. cargoPlan.Containers.Select(c => c.FinalDestination).Distinct().OrderBy(s => s)];

            Operators =
                [.. cargoPlan.Containers.Select(c => c.Carrier).Distinct().OrderBy(s => s)];

            ContainerTypes =
                [.. cargoPlan.Containers.Select(c => c.ContainerType).Distinct().OrderBy(s => s)];

            UNNOs =
                [.. cargoPlan.DgList.Select(d => d.Unno).Distinct().OrderBy(s => s)];

            var dgclasses = new List<string>();
            foreach (var dg in cargoPlan.DgList)
            {
                dgclasses.AddRange(dg.Model.AllDgClasses);
            }
            DgClasses =
                [.. dgclasses.Distinct().OrderBy(s => s)];
        }

        /// <summary>
        /// Resets all bindable properties to default/clear values
        /// </summary>
        public void Clear()
        {
            selectedNumber = string.Empty;
            position = string.Empty;
            oldPosition = string.Empty;
            isUnderdeck = false;
            isOnDeck = false;

            isDg = false;
            isNotDg = false;
            isReefer = false;
            isNonReefer = false;

            selectedPOL = null;
            selectedPOD = null;
            selectedFinalDestination = null;
            selectedOperator = null;
            selectedContainerType = null;
            isOpenType = false;
            hasRemarks = false;
            hasNoRemarks = false;

            isNewInPlan = false;
            notIsNewInPlan = false;
            hasPositionChanged = false;
            notHasPositionChanged = false;
            hasUpdated = false;
            notHasUpdated = false;
            isLocked = false;
            notIsLocked = false;
            isToImport = false;
            notIsToImport = false;
            hasPODchanged = false;
            notHasPODchanged = false;
            hasTypeChanged = false;
            notHasTypeChanged = false;
            isToBeKeptInPlan = false;
            notIsToBeKeptInPlan = false;

            selectedUNNO = null;
            SelectedUNNOs.Clear();
            selectedDgClass = null;
            SelectedDgClasses.Clear();
            flashPoint = null;
            isFPlessthan = false;
            isFPmorethan = false;
            isConflicted = false;
            notIsConflicted = false;
            hasDgRemarks = false;
            notHasDgRemarks = false;

            hasCommodity = false;
            notHasCommodity = false;
            hasSpecial = false;
            notHasSpecial = false;
            hasReeferRemarks = false;
            notHasReeferRemarks = false;
            setPoint = null;
            isSPlessthan = false;
            isSPmorethan = false;

            freeText = string.Empty;

            OnPropertyChanged(null);
            ChangedSelection();
        } 

        #endregion

        #region Private methods

        private void ChangedSelection()
        {
            OnPropertyChanged(nameof(IsNoPropertySelected));
            if (IsNoPropertySelected == _isNoPropertySelected) return;

            _isNoPropertySelected = IsNoPropertySelected;
            SelectionChanged?.Invoke(this, null);
        }

        /// <summary>
        /// Method inserts value to ObservableCollection in ascending order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="collection"></param>
        private void InsertValueSorted<T>(T value, ObservableCollection<T> collection)
        {
            int position = 0;
            if (collection.Count > 0)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    if (Comparer<T>.Default.Compare(collection[i], value) > 0)
                    {
                        break;
                    }
                    position++;
                }
            }
            collection.Insert(position, value);
        } 

        #endregion


        #region Constructor

        public SelectionControlViewModel()
        {
            SelectedUNNOs = new ObservableCollection<int>();
            SelectedDgClasses = new ObservableCollection<string>();
            CellPosition = new CellPositionWrapper();
            CellOldPosition = new CellPositionWrapper();

            AddSelectedUnnoCommand = new DelegateCommand(OnAddSelectedUnnoExecuted, OnAddSelectedUnnoCanExecute);
            AddSelectedUnnoOrApplyCommand = new DelegateCommand(OnAddSelectedUnnoExecuted);
            ClearSelectedUnnosCommand = new DelegateCommand(OnClearSelectedUnnosExecuted, OnClearSelectedUnnosCanExecute);
            AddSelectedDgClassCommand = new DelegateCommand(OnAddSelectedDgClassExecuted, OnAddSelectedDgClassCanExecute);
            AddSelectedDgClassOrApplyCommand = new DelegateCommand(OnAddSelectedDgClassExecuted);
            ClearSelectedDgClassesCommand = new DelegateCommand(OnClearSelectedDgClassesExecuted, OnClearSelectedDgClassesCanExecute);
            RemoveUnnoCommand = new DelegateCommand(OnRemoveUnnoExecuted);
            RemoveDgClassCommand = new DelegateCommand(OnRemoveDgClassExecuted);
        }

        #endregion
    }
}
