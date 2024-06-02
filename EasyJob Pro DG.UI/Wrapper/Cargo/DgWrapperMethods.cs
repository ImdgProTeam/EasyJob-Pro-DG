using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper.Cargo;
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
        }
        #endregion


        #region Methods affecting view changes
        // --------------- Methods affecting view changes ---------------------------

        /// <summary>
        /// Updates Dg info from DgListDataBase for the unit and updates presentation
        /// </summary>
        private void UpdateDgInfoAndUploadChanges()
        {
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
            NotifyOfChangedProperties();
        }

        /// <summary>
        /// Invokes event to update conflict list
        /// </summary>
        private void UpdateConflictList()
        {
            DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage());
        }

        /// <summary>
        /// Invokes sending of <see cref="DgListSelectedItemUpdatedMessage"/>
        /// Used by SelectionStatusBar in order to update status bar info
        /// </summary>
        private void NotifyOfChangedProperties()
        {
            DataMessenger.Default.Send(new DgListSelectedItemUpdatedMessage(), "selectionpropertyupdated");
        }

        private void UpdateDgStowageConflicts()
        {
            DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage(this));
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
        protected override void NotifyOfChangedProperty(object value, object oldValue = null, [CallerMemberName] string propertyName = null)
        {
            DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(this, value, oldValue, propertyName));
        }
        #endregion

    }
}
