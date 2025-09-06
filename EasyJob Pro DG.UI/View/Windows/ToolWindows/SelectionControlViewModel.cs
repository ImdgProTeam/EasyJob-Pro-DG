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

        #region Container number
        public List<string> ContainerNumbers { get; private set; }
        public string SelectedNumber { get; set; }

        #endregion

        #region Location
        // Location
        public string Position
        {
            get => position;
            set
            {
                position = value;
                cellPosition.DisplayPosition = value;
                OnPropertyChanged(nameof(PositionDescription));
            }
        }
        private string position;
        private CellPositionWrapper cellPosition;
        public string PositionDescription => cellPosition.DisplayPosition;

        public string OldPosition
        {
            get => oldPosition;
            set
            {
                oldPosition = value;
                cellOldPosition.DisplayPosition = value;
                OnPropertyChanged(nameof(OldPositionDescription));
            }

        }
        private string oldPosition;
        private CellPositionWrapper cellOldPosition;
        public string OldPositionDescription => cellOldPosition.DisplayPosition;

        public bool IsUnderdeck
        {
            get => isUnderdeck;
            set
            {
                isUnderdeck = value;
                if (!isUnderdeck) return;

                isOnDeck = false;
                OnPropertyChanged(nameof(IsOnDeck));
            }
        }
        private bool isUnderdeck;
        public bool IsOnDeck
        {
            get => isOnDeck;
            set
            {
                isOnDeck = value;
                if (!isOnDeck) return;

                isUnderdeck = false;
                OnPropertyChanged(nameof(IsUnderdeck));
            }
        }
        private bool isOnDeck;

        #endregion

        #region Type of cargo
        // Type of cargo
        private bool isDg;

        public bool IsDG
        {
            get { return isDg; }
            set
            {
                isDg = value;
                if (!isDg) return;

                isNotDg = false;
                OnPropertyChanged(nameof(IsNotDg));
            }
        }

        private bool isNotDg;

        public bool IsNotDg
        {
            get { return isNotDg; }
            set
            {
                isNotDg = value;
                if (!value) return;

                isDg = false;
                OnPropertyChanged(nameof(IsDG));
            }
        }

        private bool isReefer;

        public bool IsReefer
        {
            get { return isReefer; }
            set
            {
                isReefer = value;
                if (!value) return;

                isNonReefer = false;
                OnPropertyChanged(nameof(IsNonReefer));
            }
        }

        private bool isNonReefer;

        public bool IsNonReefer
        {
            get { return isNonReefer; }
            set
            {
                isNonReefer = value;
                if (!value) return;

                isReefer = false;
                OnPropertyChanged(nameof(IsReefer));
            }
        }

        #endregion

        #region Other container properies
        // Container

        public List<string> POLs { get; private set; }
        public string SelectedPOL { get; set; }

        public List<string> PODs { get; private set; }
        public string SelectedPOD { get; set; }

        public List<string> FinalDestinations { get; private set; }
        public string SelectedFinalDestination { get; set; }

        public List<string> Operators { get; private set; }
        public string SelectedOperator { get; set; }

        public List<string> ContainerTypes { get; private set; }
        public string SelectedContainerType { get; set; }

        public bool IsOpenType { get; set; }

        private bool hasRemarks;

        public bool HasRemarks
        {
            get { return hasRemarks; }
            set
            {
                hasRemarks = value;
                if (!value) return;

                hasNoRemarks = false;
                OnPropertyChanged(nameof(HasNoRemarks));
            }
        }

        private bool hasNoRemarks;

        public bool HasNoRemarks
        {
            get { return hasNoRemarks; }
            set
            {
                hasNoRemarks = value;
                if (!value) return;

                hasRemarks = false;
                OnPropertyChanged(nameof(HasRemarks));
            }
        }

        #endregion

        #region Changes (updates)
        // Changes
        private bool isNewInPlan;

        public bool IsNewInPlan
        {
            get { return isNewInPlan; }
            set
            {
                isNewInPlan = value;
                if (!value) return;

                notIsNewInPlan = false;
                OnPropertyChanged(nameof(NotIsNewInPlan));
            }
        }

        private bool notIsNewInPlan;

        public bool NotIsNewInPlan
        {
            get { return notIsNewInPlan; }
            set
            {
                notIsNewInPlan = value;
                if (!value) return;

                isNewInPlan = false;
                OnPropertyChanged(nameof(IsNewInPlan));
            }
        }

        private bool hasPositionChanged;

        public bool HasPositionChanged
        {
            get { return hasPositionChanged; }
            set
            {
                hasPositionChanged = value;
                if (!value) return;

                notHasPositionChanged = false;
                OnPropertyChanged(nameof(NotHasPositionChanged));
            }
        }

        private bool notHasPositionChanged;

        public bool NotHasPositionChanged
        {
            get { return notHasPositionChanged; }
            set
            {
                notHasPositionChanged = value;
                if (!value) return;

                hasPositionChanged = false;
                OnPropertyChanged(nameof(HasPositionChanged));
            }
        }

        private bool hasUpdated;

        public bool HasUpdated
        {
            get { return hasUpdated; }
            set
            {
                hasUpdated = value;
                if (!value) return;

                notHasUpdated = false;
                OnPropertyChanged(nameof(NotHasUpdated));
            }
        }

        private bool notHasUpdated;

        public bool NotHasUpdated
        {
            get { return notHasUpdated; }
            set
            {
                notHasUpdated = value;
                if (!value) return;

                hasUpdated = false;
                OnPropertyChanged(nameof(HasUpdated));
            }
        }

        private bool isLocked;

        public bool IsLocked
        {
            get { return isLocked; }
            set
            {
                isLocked = value;
                if (!value) return;

                notIsLocked = false;
                OnPropertyChanged(nameof(NotIsLocked));
            }
        }

        private bool notIsLocked;

        public bool NotIsLocked
        {
            get { return notIsLocked; }
            set
            {
                notIsLocked = value;
                if (!value) return;

                isLocked = false;
                OnPropertyChanged(nameof(IsLocked));
            }
        }

        private bool isToImport;

        public bool IsToImport
        {
            get { return isToImport; }
            set
            {
                isToImport = value;
                if (!value) return;

                notIsToImport = false;
                OnPropertyChanged(nameof(NotIsToImport));
            }
        }

        private bool notIsToImport;

        public bool NotIsToImport
        {
            get { return notIsToImport; }
            set
            {
                notIsToImport = value;
                if (!value) return;

                isToImport = false;
                OnPropertyChanged(nameof(IsToImport));
            }
        }

        private bool hasPODchanged;

        public bool HasPODchanged
        {
            get { return hasPODchanged; }
            set
            {
                hasPODchanged = value;
                if (!value) return;

                notHasPODchanged = false;
                OnPropertyChanged(nameof(NotHasPODchanged));
            }
        }

        private bool notHasPODchanged;

        public bool NotHasPODchanged
        {
            get { return notHasPODchanged; }
            set
            {
                notHasPODchanged = value;
                if (!value) return;

                hasPODchanged = false;
                OnPropertyChanged(nameof(HasPODchanged));
            }
        }

        private bool hasTypeChanged;

        public bool HasTypeChanged
        {
            get { return hasTypeChanged; }
            set
            {
                hasTypeChanged = value;
                if (!value) return;

                notHasTypeChanged = false;
                OnPropertyChanged(nameof(NotHasTypeChanged));
            }
        }

        private bool notHasTypeChanged;

        public bool NotHasTypeChanged
        {
            get { return notHasTypeChanged; }
            set
            {
                notHasTypeChanged = value;
                if (!value) return;

                hasTypeChanged = false;
                OnPropertyChanged(nameof(HasTypeChanged));
            }
        }

        private bool isToBeKeptInPlan;

        public bool IsToBeKeptInPlan
        {
            get { return isToBeKeptInPlan; }
            set
            {
                isToBeKeptInPlan = value;
                if (!value) return;

                notIsToBeKeptInPlan = false;
                OnPropertyChanged(nameof(NotIsToBeKeptInPlan));
            }
        }

        private bool notIsToBeKeptInPlan;

        public bool NotIsToBeKeptInPlan
        {
            get { return notIsToBeKeptInPlan; }
            set
            {
                notIsToBeKeptInPlan = value;
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
        public int? SelectedUNNO { get; set; }
        public ObservableCollection<int> SelectedUNNOs { get; private set; }

        public ICommand AddSelectedUnnoCommand { get; private set; }
        public ICommand RemoveUnnoCommand { get; private set; }
        public ICommand ClearSelectedUnnosCommand { get; private set; }

        private void OnAddSelectedUnnoExecuted(object obj)
        {
            if (!SelectedUNNO.HasValue || SelectedUNNOs.Contains(SelectedUNNO.Value))
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
        }

        private void OnClearSelectedUnnosExecuted(object obj)
        {
            SelectedUNNOs.Clear();
            OnPropertyChanged(nameof(SelectedUNNOs));
        }

        private bool OnClearSelectedUnnosCanExecute(object obj)
        {
            return SelectedUNNOs.Count > 0;
        }


        // Dg class
        public List<string> DgClasses { get; private set; }
        public string SelectedDgClass { get; set; }
        public ObservableCollection<string> SelectedDgClasses { get; private set; }

        public ICommand AddSelectedDgClassCommand { get; private set; }
        public ICommand RemoveDgClassCommand { get; private set; }
        public ICommand ClearSelectedDgClassesCommand { get; private set; }

        private void OnAddSelectedDgClassExecuted(object obj)
        {
            if (string.IsNullOrWhiteSpace(SelectedDgClass) || SelectedDgClasses.Contains(SelectedDgClass))
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
        }
        private void OnClearSelectedDgClassesExecuted(object obj)
        {
            SelectedDgClasses.Clear();
            OnPropertyChanged(nameof(SelectedDgClasses));
        }

        private bool OnClearSelectedDgClassesCanExecute(object obj)
        {
            return SelectedDgClasses.Count > 0;
        }

        public decimal? FlashPoint { get; set; }
        private bool isFPlessthan;

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

        private bool isFPmorethan;

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

        private bool isConflicted;

        public bool IsConflicted
        {
            get { return isConflicted; }
            set
            {
                isConflicted = value;
                if (!value) return;

                notIsConflicted = false;
                OnPropertyChanged(nameof(NotIsConflicted));
            }
        }

        private bool notIsConflicted;

        public bool NotIsConflicted
        {
            get { return notIsConflicted; }
            set
            {
                notIsConflicted = value;
                if (!value) return;

                isConflicted = false;
                OnPropertyChanged(nameof(IsConflicted));
            }
        }

        private bool hasDgRemarks;

        public bool HasDgRemarks
        {
            get { return hasDgRemarks; }
            set
            {
                hasDgRemarks = value;
                if (!value) return;

                notHasDgRemarks = false;
                OnPropertyChanged(nameof(NotHasDgRemarks));
            }
        }

        private bool notHasDgRemarks;

        public bool NotHasDgRemarks
        {
            get { return notHasDgRemarks; }
            set
            {
                notHasDgRemarks = value;
                if (!value) return;

                hasDgRemarks = false;
                OnPropertyChanged(nameof(HasDgRemarks));
            }
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

        #region Reefer
        // Reefer
        private bool hasCommodity;

        public bool HasCommodity
        {
            get { return hasCommodity; }
            set
            {
                hasCommodity = value;
                if (!value) return;

                notHasCommodity = false;
                OnPropertyChanged(nameof(NotHasCommodity));
            }
        }

        private bool notHasCommodity;

        public bool NotHasCommodity
        {
            get { return notHasCommodity; }
            set
            {
                notHasCommodity = value;
                if (!value) return;

                hasCommodity = false;
                OnPropertyChanged(nameof(HasCommodity));
            }
        }

        private bool hasSpecial;

        public bool HasSpecial
        {
            get { return hasSpecial; }
            set
            {
                hasSpecial = value;
                if (!value) return;

                notHasSpecial = false;
                OnPropertyChanged(nameof(NotHasSpecial));
            }
        }


        private bool notHasSpecial;

        public bool NotHasSpecial
        {
            get { return notHasSpecial; }
            set
            {
                notHasSpecial = value;
                if (!value) return;

                hasSpecial = false;
                OnPropertyChanged(nameof(HasSpecial));
            }
        }

        private bool hasReeferRemarks;

        public bool HasReeferRemarks
        {
            get { return hasReeferRemarks; }
            set
            {
                hasReeferRemarks = value;
                if (!value) return;

                notHasReeferRemarks = false;
                OnPropertyChanged(nameof(NotHasReeferRemarks));
            }
        }

        private bool notHasReeferRemarks;

        public bool NotHasReeferRemarks
        {
            get { return notHasReeferRemarks; }
            set
            {
                notHasReeferRemarks = value;
                if (!value) return;

                hasReeferRemarks = false;
                OnPropertyChanged(nameof(HasReeferRemarks));
            }
        }

        public decimal? SetPoint { get; set; }
        private bool isSPlessthan;
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

        private bool isSPmorethan;
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
        public string FreeText { get; set; }

        #endregion

        // ----- Methods -----

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
            SelectedNumber = string.Empty;
            Position = string.Empty;
            OldPosition = string.Empty;
            isUnderdeck = false;
            isOnDeck = false;

            isDg = false;
            isNotDg = false;
            isReefer = false;
            isNonReefer = false;

            SelectedPOL = null;
            SelectedPOD = null;
            SelectedFinalDestination = null;
            SelectedOperator = null;
            SelectedContainerType = null;
            IsOpenType = false;
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

            SelectedUNNO = null;
            SelectedUNNOs.Clear();
            SelectedDgClass = null;
            SelectedDgClasses.Clear();
            FlashPoint = null;
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
            SetPoint = null;
            isSPlessthan = false;
            isSPmorethan = false;

            FreeText = string.Empty;

            OnPropertyChanged(null);
        }


        #region Constructor

        public SelectionControlViewModel()
        {
            SelectedUNNOs = new ObservableCollection<int>();
            SelectedDgClasses = new ObservableCollection<string>();
            cellPosition = new CellPositionWrapper();
            cellOldPosition = new CellPositionWrapper();

            AddSelectedUnnoCommand = new DelegateCommand(OnAddSelectedUnnoExecuted, OnAddSelectedUnnoCanExecute);
            ClearSelectedUnnosCommand = new DelegateCommand(OnClearSelectedUnnosExecuted, OnClearSelectedUnnosCanExecute);
            AddSelectedDgClassCommand = new DelegateCommand(OnAddSelectedDgClassExecuted, OnAddSelectedDgClassCanExecute);
            ClearSelectedDgClassesCommand = new DelegateCommand(OnClearSelectedDgClassesExecuted, OnClearSelectedDgClassesCanExecute);
            RemoveUnnoCommand = new DelegateCommand(OnRemoveUnnoExecuted);
            RemoveDgClassCommand = new DelegateCommand(OnRemoveDgClassExecuted);
        }

        #endregion
    }
}
