using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.Windows.ToolWindows;
using EasyJob_ProDG.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.UI.View.DialogWindows.ToolWindows
{
    internal class SetToolViewModel : ToolWindowViewModelBase
    {
        #region Private fields

        private bool _isNoPropertySelected;

        private string selectedPOL;
        private string selectedPOD;
        private string selectedFinalDestination;
        private string selectedOperator;
        private string selectedContainerType;
        private bool isOpenType;
        private bool isClosedType;
        private bool isReefer;
        private bool isNonReefer;
        private string containerRemark;
        private bool isLocked;
        private bool notIsLocked;
        private bool isToBeKeptInPlan;
        private bool notIsToBeKeptInPlan;
        private bool isToImport;
        private bool notIsToImport;
        private bool isClearImport;
        private bool isClearAll;

        private int? selectedUNNO;
        private decimal? netWeight;
        private string packingGroup;
        private string stowageCategory;
        private bool isLQ;
        private bool notIsLQ;
        private bool isMP;
        private bool notIsMP;
        private bool isMax1L;
        private bool isNotMax1L;
        private bool isWaste;
        private bool notIsWaste;
        private bool isStabilized;
        private bool notIsStabilized;
        private bool isClearFlashPoint;
        private decimal? flashPoint;
        private string properShippingName;
        private string technicalName;
        private string dgRemark;

        private decimal? setPoint;
        private decimal? loadTemp;
        private string reeferRemark;
        private string reeferSpecial;
        private string reeferCommodity;
        private string reeferVent;

        #endregion

        internal bool IsNoPropertySelected => !IsReefer && !IsNonReefer
    && string.IsNullOrWhiteSpace(SelectedPOL) && string.IsNullOrWhiteSpace(SelectedPOD)
    && string.IsNullOrWhiteSpace(SelectedFinalDestination) && string.IsNullOrWhiteSpace(SelectedOperator)
    && string.IsNullOrWhiteSpace(SelectedContainerType) && !IsOpenType && !isClosedType
            && string.IsNullOrWhiteSpace(ContainerRemark)
    && !IsLocked && !NotIsLocked && !IsToImport && !NotIsToImport && !IsClearImport && !IsClearAll
    && !IsToBeKeptInPlan && !NotIsToBeKeptInPlan
    && !SelectedUNNO.HasValue && !NetWeight.HasValue
    && string.IsNullOrWhiteSpace(PackingGroup) && string.IsNullOrWhiteSpace(StowageCategory)
            && !IsLQ && !NotIsLQ && !IsMP && !NotIsMP && !IsMax1L && !NotIsMax1L
            && !IsWaste && !NotIsWaste && !IsStabilized && !NotIsStabilized
    && !FlashPoint.HasValue && !IsClearFlashPoint
    && string.IsNullOrWhiteSpace(ProperShippingName) && string.IsNullOrWhiteSpace(TechnicalName)
            && string.IsNullOrWhiteSpace(DgRemark)
    && !SetPoint.HasValue && !LoadTemp.HasValue
    && string.IsNullOrWhiteSpace(ReeferVent) && string.IsNullOrWhiteSpace(ReeferCommodity)
            && string.IsNullOrWhiteSpace(ReeferSpecial) && string.IsNullOrWhiteSpace(ReeferRemark);

        internal event EventHandler SelectionChanged;


        #region Container properties

        // CheckBoxes
        public bool cbPOLPOD { get; set; }
        public bool cbFinalDestination { get; set; }
        public bool cbContainerType { get; set; }
        public bool cbReefer { get; set; }
        public bool cbContainerRemark { get; set; }


        public List<string> POLs { get; private set; }
        public string SelectedPOL
        {
            get => selectedPOL;
            set
            {
                selectedPOL = value;
                ChangedSelection();

                if (string.IsNullOrWhiteSpace(SelectedPOL)) return;
                cbPOLPOD = true;
                OnPropertyChanged(nameof(cbPOLPOD));
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

                if (string.IsNullOrWhiteSpace(SelectedPOD)) return;
                cbPOLPOD = true;
                OnPropertyChanged(nameof(cbPOLPOD));
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

                if (string.IsNullOrWhiteSpace(selectedFinalDestination)) return;
                cbFinalDestination = true;
                OnPropertyChanged(nameof(cbFinalDestination));
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

                if (string.IsNullOrWhiteSpace(selectedOperator)) return;
                cbFinalDestination = true;
                OnPropertyChanged(nameof(cbFinalDestination));
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

                if (string.IsNullOrWhiteSpace(selectedContainerType)) return;
                cbContainerType = true;
                OnPropertyChanged(nameof(cbContainerType));
            }
        }

        public bool IsOpenType
        {
            get => isOpenType;
            set
            {
                isOpenType = value;
                ChangedSelection();

                if (!value) return;

                isClosedType = false;
                OnPropertyChanged(nameof(isClosedType));
                cbContainerType = true;
                OnPropertyChanged(nameof(cbContainerType));
            }
        }

        public bool IsClosedType
        {
            get => isClosedType;
            set
            {
                isClosedType = value;
                ChangedSelection();

                if (!value) return;
                isOpenType = false;
                OnPropertyChanged(nameof(isOpenType));
                cbContainerType = true;
                OnPropertyChanged(nameof(cbContainerType));
            }
        }

        public bool IsReefer
        {
            get => isReefer;
            set
            {
                isReefer = value;
                ChangedSelection();

                if (!value) return;

                isNonReefer = false;
                OnPropertyChanged(nameof(IsNonReefer));
                cbReefer = true;
                OnPropertyChanged(nameof(cbReefer));
            }
        }

        public bool IsNonReefer
        {
            get => isNonReefer;
            set
            {
                isNonReefer = value;
                ChangedSelection();

                if (!value) return;
                isReefer = false;
                OnPropertyChanged(nameof(IsReefer));
                cbReefer = true;
                OnPropertyChanged(nameof(cbReefer));
            }
        }

        public string ContainerRemark
        {
            get => containerRemark;
            set
            {
                containerRemark = value;
                ChangedSelection();

                if (string.IsNullOrWhiteSpace(containerRemark)) return;
                cbContainerRemark = true;
                OnPropertyChanged(nameof(cbContainerRemark));
            }
        }

        #endregion

        #region IUpdatable

        // CheckBoxes
        public bool cbUpdates1 { get; set; }
        public bool cbUpdates2 { get; set; }


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
                isClearAll = false;
                OnPropertyChanged(nameof(IsClearAll));
                cbUpdates1 = true;
                OnPropertyChanged(nameof(cbUpdates1));
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
                isClearAll = false;
                OnPropertyChanged(nameof(IsClearAll));
                cbUpdates1 = true;
                OnPropertyChanged(nameof(cbUpdates1));
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
                isClearImport = false;
                OnPropertyChanged(nameof(IsClearImport));
                isClearAll = false;
                OnPropertyChanged(nameof(IsClearAll));
                cbUpdates2 = true;
                OnPropertyChanged(nameof(cbUpdates2));
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
                isClearImport = false;
                OnPropertyChanged(nameof(IsClearImport));
                isClearAll = false;
                OnPropertyChanged(nameof(IsClearAll));
                cbUpdates2 = true;
                OnPropertyChanged(nameof(cbUpdates2));
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
                isClearAll = false;
                OnPropertyChanged(nameof(IsClearAll));
                cbUpdates1 = true;
                OnPropertyChanged(nameof(cbUpdates1));
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
                isClearAll = false;
                OnPropertyChanged(nameof(IsClearAll));
                cbUpdates1 = true;
                OnPropertyChanged(nameof(cbUpdates1));
            }
        }

        public bool IsClearImport
        {
            get { return isClearImport; }
            set
            {
                isClearImport = value;
                ChangedSelection();
                if (!value) return;

                isToImport = false;
                notIsToImport = false;
                OnPropertyChanged(nameof(IsToImport));
                OnPropertyChanged(nameof(NotIsToImport));
                isClearAll = false;
                OnPropertyChanged(nameof(IsClearAll));
                cbUpdates2 = true;
                OnPropertyChanged(nameof(cbUpdates2));
            }
        }

        public bool IsClearAll
        {
            get { return isClearAll; }
            set
            {
                isClearAll = value;
                ChangedSelection();
                if (!value) return;

                isLocked = false;
                OnPropertyChanged(nameof(IsLocked));
                notIsLocked = false;
                OnPropertyChanged(nameof(NotIsLocked));
                isToBeKeptInPlan = false;
                OnPropertyChanged(nameof(IsToBeKeptInPlan));
                notIsToBeKeptInPlan = false;
                OnPropertyChanged(nameof(NotIsToBeKeptInPlan));
                isToImport = false;
                OnPropertyChanged(nameof(IsToImport));
                notIsToImport = false;
                OnPropertyChanged(nameof(NotIsToImport));
                isClearImport = false;
                OnPropertyChanged(nameof(IsClearImport));
                cbUpdates2 = true;
                OnPropertyChanged(nameof(cbUpdates2));
            }
        }

        #endregion

        #region Dg

        // CheckBoxes
        public bool cbUnno { get; set; }
        public bool cbNetWeight { get; set; }
        public bool cbPackingGroup { get; set; }
        public bool cbLQ { get; set; }
        public bool cbMax1L { get; set; }
        public bool cbStabilized { get; set; }
        public bool cbFlashPoint { get; set; }
        public bool cbName { get; set; }
        public bool cbTechnicalName { get; set; }
        public bool cbDgRemark { get; set; }


        public List<int> UNNOs { get; private set; }
        public int? SelectedUNNO
        {
            get => selectedUNNO;
            set
            {
                selectedUNNO = value;
                ChangedSelection();

                if (!selectedUNNO.HasValue) return;
                cbUnno = true;
                OnPropertyChanged(nameof(cbUnno));
            }
        }

        public decimal? NetWeight
        {
            get { return netWeight; }
            set
            {
                netWeight = value;
                ChangedSelection();

                if (!netWeight.HasValue) return;
                cbNetWeight = true;
                OnPropertyChanged(nameof(cbNetWeight));
            }
        }

        public List<string> PackingGroups => new List<string>() { "", "I", "II", "III" };

        public string PackingGroup
        {
            get { return packingGroup; }
            set
            {
                packingGroup = value;
                ChangedSelection();

                if (string.IsNullOrEmpty(packingGroup)) return;
                cbPackingGroup = true;
                OnPropertyChanged(nameof(cbPackingGroup));
            }
        }

        public List<string> StowageCategories => DataGridDgViewModel.StowageCategories.ToList();

        public string StowageCategory
        {
            get { return stowageCategory; }
            set
            {
                stowageCategory = value;
                ChangedSelection();

                if (string.IsNullOrEmpty(stowageCategory)) return;
                cbPackingGroup = true;
                OnPropertyChanged(nameof(cbPackingGroup));
            }
        }

        public bool IsLQ
        {
            get { return isLQ; }
            set
            {
                isLQ = value;
                ChangedSelection();
                if (!value) return;

                notIsLQ = false;
                OnPropertyChanged(nameof(NotIsLQ));
                cbLQ = true;
                OnPropertyChanged(nameof(cbLQ));
            }
        }

        public bool NotIsLQ
        {
            get { return notIsLQ; }
            set
            {
                notIsLQ = value;
                ChangedSelection();
                if (!value) return;

                isLQ = false;
                OnPropertyChanged(nameof(IsLQ));
                cbLQ = true;
                OnPropertyChanged(nameof(cbLQ));
            }
        }

        public bool IsMP
        {
            get { return isMP; }
            set
            {
                isMP = value;
                ChangedSelection();
                if (!value) return;

                notIsMP = false;
                OnPropertyChanged(nameof(NotIsMP));
                cbLQ = true;
                OnPropertyChanged(nameof(cbLQ));
            }
        }

        public bool NotIsMP
        {
            get { return notIsMP; }
            set
            {
                notIsMP = value;
                ChangedSelection();
                if (!value) return;

                isMP = false;
                OnPropertyChanged(nameof(IsMP));
                cbLQ = true;
                OnPropertyChanged(nameof(cbLQ));
            }
        }

        public bool IsMax1L
        {
            get { return isMax1L; }
            set
            {
                isMax1L = value;
                ChangedSelection();
                if (!value) return;

                isNotMax1L = false;
                OnPropertyChanged(nameof(NotIsMax1L));
                cbMax1L = true;
                OnPropertyChanged(nameof(cbMax1L));
            }
        }

        public bool NotIsMax1L
        {
            get { return isNotMax1L; }
            set
            {
                isNotMax1L = value;
                ChangedSelection();
                if (!value) return;

                isMax1L = false;
                OnPropertyChanged(nameof(IsMax1L));
                cbMax1L = true;
                OnPropertyChanged(nameof(cbMax1L));
            }
        }

        public bool IsWaste
        {
            get { return isWaste; }
            set
            {
                isWaste = value;
                ChangedSelection();
                if (!value) return;

                notIsWaste = false;
                OnPropertyChanged(nameof(NotIsWaste));
                cbMax1L = true;
                OnPropertyChanged(nameof(cbMax1L));
            }
        }

        public bool NotIsWaste
        {
            get { return notIsWaste; }
            set
            {
                notIsWaste = value;
                ChangedSelection();
                if (!value) return;

                isWaste = false;
                OnPropertyChanged(nameof(IsWaste));
                cbMax1L = true;
                OnPropertyChanged(nameof(cbMax1L));
            }
        }

        public bool IsStabilized
        {
            get { return isStabilized; }
            set
            {
                isStabilized = value;
                ChangedSelection();
                if (!value) return;

                notIsStabilized = false;
                OnPropertyChanged(nameof(NotIsStabilized));
                cbStabilized = true;
                OnPropertyChanged(nameof(cbStabilized));
            }
        }

        public bool NotIsStabilized
        {
            get { return notIsStabilized; }
            set
            {
                notIsStabilized = value;
                ChangedSelection();
                if (!value) return;

                isStabilized = false;
                OnPropertyChanged(nameof(IsStabilized));
                cbStabilized = true;
                OnPropertyChanged(nameof(cbStabilized));
            }
        }

        public decimal? FlashPoint
        {
            get => flashPoint;
            set
            {
                flashPoint = value;
                ChangedSelection();

                if (!flashPoint.HasValue) return;
                isClearFlashPoint = false;
                OnPropertyChanged(nameof(IsClearFlashPoint));
                cbFlashPoint = true;
                OnPropertyChanged(nameof(cbFlashPoint));
            }
        }

        public bool IsClearFlashPoint
        {
            get => isClearFlashPoint;
            set
            {
                isClearFlashPoint = value;
                ChangedSelection();

                if (!value) return;
                flashPoint = null;
                OnPropertyChanged(nameof(FlashPoint));
                cbFlashPoint = true;
                OnPropertyChanged(nameof(cbFlashPoint));
            }
        }

        public List<string> ProperShippingNames { get; private set; }
        
        public string ProperShippingName
        {
            get { return properShippingName; }
            set
            {
                properShippingName = value;
                ChangedSelection();

                if (string.IsNullOrEmpty(properShippingName)) return;
                cbName = true;
                OnPropertyChanged(nameof(cbName));
            }
        }

        public List<string> TechnicalNames { get; private set; }

        public string TechnicalName
        {
            get { return technicalName; }
            set
            {
                technicalName = value;
                ChangedSelection();

                if (string.IsNullOrEmpty(technicalName)) return;
                cbTechnicalName = true;
                OnPropertyChanged(nameof(cbTechnicalName));
            }
        }

        public List<string> DgRemarks { get; private set; }
        public string DgRemark
        {
            get { return dgRemark; }
            set
            {
                dgRemark = value;
                ChangedSelection();

                if (string.IsNullOrEmpty(dgRemark)) return;
                cbDgRemark = true;
                OnPropertyChanged(nameof(cbDgRemark));
            }
        }


        #endregion

        #region Reefers

        // Check boxes
        public bool cbSetPoint { get; set; }
        public bool cbLoadTemp { get; set; }
        public bool cbVent { get; set; }
        public bool cbCommodity { get; set; }
        public bool cbReeferSpecial { get; set; }
        public bool cbReeferRemark { get; set; }


        public decimal? SetPoint
        {
            get => setPoint;
            set
            {
                setPoint = value;
                ChangedSelection();

                if (!setPoint.HasValue) return;
                cbSetPoint = true;
                OnPropertyChanged(nameof(cbSetPoint));
            }
        }

        public decimal? LoadTemp
        {
            get => loadTemp;
            set
            {
                loadTemp = value;
                ChangedSelection();

                if (!loadTemp.HasValue) return;
                cbLoadTemp = true;
                OnPropertyChanged(nameof(cbLoadTemp));
            }
        }

        public string ReeferVent
        {
            get { return reeferVent; }
            set
            {
                reeferVent = value;
                ChangedSelection();

                if (string.IsNullOrEmpty(reeferVent)) return;
                cbVent = true;
                OnPropertyChanged(nameof(cbVent));
            }
        }

        public List<string> ReeferCommodities { get; private set; }

        public string ReeferCommodity
        {
            get { return reeferCommodity; }
            set
            {
                reeferCommodity = value;
                ChangedSelection();

                if (string.IsNullOrEmpty(reeferCommodity)) return;
                cbCommodity = true;
                OnPropertyChanged(nameof(cbCommodity));
            }
        }

        public List<string> ReeferSpecials { get; private set; }

        public string ReeferSpecial
        {
            get { return reeferSpecial; }
            set
            {
                reeferSpecial = value;
                ChangedSelection();

                if (string.IsNullOrEmpty(reeferSpecial)) return;
                cbReeferSpecial = true;
                OnPropertyChanged(nameof(cbReeferSpecial));
            }
        }

        public List<string> ReeferRemarks { get; private set; }

        public string ReeferRemark
        {
            get { return reeferRemark; }
            set
            {
                reeferRemark = value;
                ChangedSelection();

                if (string.IsNullOrEmpty(reeferRemark)) return;
                cbReeferRemark = true;
                OnPropertyChanged(nameof(cbReeferRemark));
            }
        }

        #endregion

        #region Expanders properties

        public bool IsDgExpanded { get; set; } = false;
        public bool IsReeferExpanded { get; set; } = false;

        #endregion


        #region Command methods

        protected override void OnApplyExecuted(object obj)
        {
            throw new System.NotImplementedException();

            CreateLists();
        }

        protected override bool OnApplyCanExecute(object obj)
        {
            return !IsNoPropertySelected;
        }

        protected override void OnClearCommandExecuted(object obj)
        {
            ClearSelection();
        }
        protected override bool OnClearCanExecute(object obj)
        {
            return !IsNoPropertySelected;
        }
        #endregion


        #region Private methods

        private void CreateLists()
        {
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

            ProperShippingNames =
                [.. cargoPlan.DgList.Select(d => d.Name).Distinct().OrderBy(s => s)];
            TechnicalNames =
                [.. cargoPlan.DgList.Select(d => d.TechnicalName).Distinct().OrderBy(s => s)];
            DgRemarks =
                [.. cargoPlan.DgList.Select(d => d.Remarks).Distinct().OrderBy(s => s)];

            ReeferCommodities =
                [.. cargoPlan.Reefers.Select(r => r.Commodity).Distinct().OrderBy(s => s)];
            ReeferSpecials =
                [.. cargoPlan.Reefers.Select(r => r.ReeferSpecial).Distinct().OrderBy(s => s)];
            ReeferRemarks =
                [.. cargoPlan.Reefers.Select(r => r.ReeferRemark).Distinct().OrderBy(s => s)];

        }

        private void ChangedSelection()
        {
            OnPropertyChanged(nameof(IsNoPropertySelected));
            if (IsNoPropertySelected == _isNoPropertySelected) return;

            _isNoPropertySelected = IsNoPropertySelected;
            SelectionChanged?.Invoke(this, null);
        }

        private void SetExpanders()
        {
            IsDgExpanded = selectedDataGridIndex == 0;
            IsReeferExpanded = selectedDataGridIndex == 1;
        }

        private void ClearSelection()
        {
            selectedPOL = null;
            selectedPOD = null;
            selectedFinalDestination = null;
            selectedOperator = null;
            selectedContainerType = null;
            isOpenType = false;
            isClosedType = false;
            isReefer = false;
            isNonReefer = false;
            containerRemark = null;

            isLocked = false;
            notIsLocked = false;
            isToImport = false;
            notIsToImport = false;
            isToBeKeptInPlan = false;
            notIsToBeKeptInPlan = false;
            isClearImport = false;
            IsClearAll = false;

            selectedUNNO = null;
            netWeight = null;
            packingGroup = null;
            stowageCategory = null;
            isLQ = false;
            notIsLQ = false;
            isMP = false;
            notIsMP = false;
            isMax1L = false;
            isNotMax1L = false;
            isWaste = false;
            notIsWaste = false;
            isStabilized = false;
            notIsStabilized = false;
            isClearFlashPoint = false;
            flashPoint = null;
            properShippingName = null;
            technicalName = null;
            dgRemark = null;

            setPoint = null;
            loadTemp = null;
            reeferRemark = null;
            reeferSpecial = null;
            reeferCommodity = null;
            reeferVent = null;

            cbPOLPOD = false;
            cbFinalDestination = false;
            cbContainerType = false;
            cbReefer = false;
            cbContainerRemark = false;

            cbUpdates1 = false;
            cbUpdates2 = false;

            cbUnno = false;
            cbNetWeight = false;
            cbPackingGroup = false;
            cbLQ = false;
            cbMax1L = false;
            cbStabilized = false;
            cbFlashPoint = false;
            cbName = false;
            cbTechnicalName = false;
            cbDgRemark = false;

            cbSetPoint = false;
            cbLoadTemp = false;
            cbVent = false;
            cbCommodity = false;
            cbReeferSpecial = false;
            cbReeferRemark = false;

            OnPropertyChanged(null);
            ChangedSelection();
        } 

        #endregion


        #region Constructor

        public SetToolViewModel()
        {
            CreateLists();
            SetExpanders();

            ClearCommand = new DelegateCommand(OnClearCommandExecuted, OnClearCanExecute);
            ApplyCommand = new DelegateCommand(OnApplyExecuted, OnApplyCanExecute);
        }

        #endregion
    }
}
