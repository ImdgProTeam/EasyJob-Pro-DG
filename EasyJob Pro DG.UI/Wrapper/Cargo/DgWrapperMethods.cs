using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper.Cargo;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EasyJob_ProDG.UI.Wrapper
{
    public partial class DgWrapper : AbstractContainerWrapper<Dg>
    {
        #region Public methods
        // --------------- Methods ---------------------------------------

        /// <summary>
        /// Adds TechnicalName, if any, to ProperShippingName and changes status of IsTechnicalNameIncluded.
        /// </summary>
        internal void IncludeTechnicalName()
        {
            if (IsTechnicalNameIncluded) return;
            if (string.IsNullOrEmpty(TechnicalName)) return;
            Name += "\n" + TechnicalName;
            IsTechnicalNameIncluded = true;
            OnPropertyChanged(nameof(IsTechnicalNameIncluded));
        }

        /// <summary>
        /// Removes TechnicalName, if any, to ProperShippingName and changes status of IsTechnicalNameIncluded.
        /// </summary>
        public void RemoveTechnicalName()
        {
            if (!IsTechnicalNameIncluded) return;
            if (string.IsNullOrEmpty(TechnicalName)) return;
            Name = Name.Replace(TechnicalName, "");
            if (Name.EndsWith("\n"))
                Name = Name.Remove(Name.Length - 1);
            IsTechnicalNameIncluded = false;
            OnPropertyChanged(nameof(IsTechnicalNameIncluded));
        }

        /// <summary>
        /// Restores proper shipping name as it is recorded in IMDG Code.
        /// </summary>
        public void RestoreProperShippingName()
        {
            Model.Name = OriginalNameFromCode;
            IsNameChanged = false;
            IsTechnicalNameIncluded = false;
            if (IsMax1L && !Name.ContainsMax1Litre())
                Model.AddToNameMax1l();
            if (IsWaste && !Name.ContainsWaste())
                Model.AddToNameWaste();
            if (IsStabilized && !Name.ContainsStabilized())
                Model.AddToNameStabilized();

            OnPropertyChanged(nameof(Name));
        }
        #endregion

        #region SegregationGroups methods

        /// <summary>
        /// Creates <see cref="SegregationGroups"/> to be used as a selectable dropdown in <see cref="SegregationGroup"/> column.
        /// Called only once a cell selected for edit.
        /// </summary>
        private void CreateSegregationGroups()
        {
            if (_segregationGroups != null) return;
            _segregationGroups = new ObservableCollection<SegregationGroupWrapper>();
            for (byte i = 0; i <= IMDGCode.SegregationGroupsNumber; i++)
            {
                var item = new SegregationGroupWrapper()
                {
                    Code = IMDGCode.SegregationGroupsCodes[i],
                    Name = IMDGCode.SegregationGroups[i],
                    Number = i,
                    IsSelected = Model.SegregationGroupList.Contains(i)
                };
                item.IsAsPerCode = Model.SegregationSGList.Contains(item.Code);
                item.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(SegregationGroupWrapper.IsSelected))
                    {
                        HandleSegregationGroupsSelection();
                    }
                };
                _segregationGroups.Add(item);
            }
        }

        /// <summary>
        /// Called when selected items in SegregationGroups dropdown menu
        /// </summary>
        private void HandleSegregationGroupsSelection()
        {
            if (_segregationGroups.First(g => g.Number == 0).IsSelected)
            {
                foreach (var item in _segregationGroups)
                {
                    item.IsSelected = false;
                }
            }
        }

        /// <summary>
        /// Sets SegregationGroups to Model from selected <see cref="_segregationGroups"/>.
        /// Calls <see cref="UpdateConflictList"/> in the end.
        /// </summary>
        internal void SetSegregationGroups()
        {
            Model.ClearSegregationGroups();
            foreach (var item in _segregationGroups)
            {
                if (item.IsSelected)
                    Model.SegregationGroupList.Add(item.Number);
            }
            OnPropertyChanged(nameof(SegregationGroup));
            UpdateConflictList();
        }

        #endregion


        #region Methods affecting view changes
        // --------------- Methods affecting view changes ---------------------------

        /// <summary>
        /// Updates Dg info from DgListDataBase for the unit and updates presentation
        /// </summary>
        private void UpdateDgInfoAndUploadChanges()
        {
            Model.Clear(Unno);
            Model.UpdateDgInfo();
            RefreshDgDataPresentation();
        }

        /// <summary>
        /// Called when packing group is updated. Contains logic to process the PKG change.
        /// </summary>
        private void OnUpdatePackingGroup()
        {
            Model.AssignFromDgList(false, true);
            RefreshDgDataPresentation();
        }

        /// <summary>
        /// Notifies WorkingCargoPlan of change in DgNetWeight.
        /// </summary>
        private void OnNetWeightChanged()
        {
            DataMessenger.Default.Send<UpdateCargoPlan>(new UpdateCargoPlan(), "Net weight changed");
            NotifyStatusBar();
        }

        /// <summary>
        /// Invokes event to update conflict list
        /// </summary>
        private void UpdateConflictList()
        {
            DataMessenger.Default.Send(new ConflictsToBeCheckedAndUpdatedMessage());
        }

        /// <summary>
        /// Invokes sending of <see cref="DgListSelectedItemUpdatedMessage"/>
        /// Used by SelectionStatusBar in order to update status bar info
        /// </summary>
        private void NotifyStatusBar()
        {
            DataMessenger.Default.Send(new DgListSelectedItemUpdatedMessage(), "selectionpropertyupdated");
        }

        private void UpdateDgStowageConflicts()
        {
            DataMessenger.Default.Send(new ConflictsToBeCheckedAndUpdatedMessage(this));
        }

        /// <summary>
        /// Calls OnPropertyChanged for most of Dg properties
        /// </summary>
        private void RefreshDgDataPresentation()
        {
            OnPropertyChanged(null);
        }

        /// <summary>
        /// Sends message to synchronise changes with WorkingCargoPlan
        /// </summary>
        /// <param name="value">new value set</param>
        /// <param name="oldValue">old value</param>
        /// <param name="propertyName">property that is changed</param>
        protected override void NotifyOfChangedContainerProperty(object value, object oldValue = null, [CallerMemberName] string propertyName = null)
        {
            DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(this, value, oldValue, propertyName));
        }
        #endregion

    }
}
