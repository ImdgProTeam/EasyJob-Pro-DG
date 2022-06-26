using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.Model.Validators;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using System;
using System.Runtime.CompilerServices;

namespace EasyJob_ProDG.UI.Wrapper
{
    public partial class DgWrapper : ModelWrapper<Dg>, ILocationOnBoard, IContainer, IUpdatable
    {
        #region Public methods
        // --------------- Methods ---------------------------------------

        /// <summary>
        /// Clears and registrations and subscriptions, e.g. before deleting
        /// </summary>
        internal void ClearSubscriptions()
        {
            DataMessenger.Default.Unregister(this);
        }

        /// <summary>
        /// Compiles location of input of bay, row or tier
        /// </summary>
        /// <param name="value">Bay, row or tier</param>
        /// <param name="propertyName">Specified property</param>
        /// <returns></returns>
        internal string CompileLocation(byte value, [CallerMemberName] string propertyName = null)
        {
            switch (propertyName.ToLower())
            {
                case "bay":
                    return AddZeroIfRequired(value, 2) + AddZeroIfRequired(Row) + AddZeroIfRequired(Tier);
                case "row":
                    return AddZeroIfRequired(Bay, 2) + AddZeroIfRequired(value) + AddZeroIfRequired(Tier);
                case "tier":
                    return AddZeroIfRequired(Bay, 2) + AddZeroIfRequired(Row) + AddZeroIfRequired(value);
                default:
                    return Location;
            }
        }

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


        #region Private methods
        // --------------- Private methods ---------------------------------------

        /// <summary>
        /// Checks if input value is less then '10' and adds '0' before the digit if the case
        /// </summary>
        /// <param name="value"></param>
        /// <param name="numberOfZeros"></param>
        /// <returns></returns>
        private string AddZeroIfRequired(byte value, byte numberOfZeros = 1)
        {
            string result = "";
            if (value < 10) result += "0";
            if (value < 100 && numberOfZeros == 2) result += "0";
            result += value.ToString();
            return result;
        }

        /// <summary>
        /// Supporting method to parse multiple classes into array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string[] ParseMultipleClasses(string value)
        {
            char[] separator = { ' ', ',' };
            string[] array = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return array;
        }

        /// <summary>
        /// Checks if UN no exists in database and prompts user to confirm weather to continue if UN does not exist.
        /// </summary>
        /// <param name="unno">UN no being checked</param>
        /// <returns>If UN no is valid or user acknowledge</returns>
        internal static bool CheckForExistingUnno(ushort unno)
        {
            if (UnnoValidator.Validate(unno)) return true;
            if (_messageDialogService.ShowYesNoDialog(
                    $"UN no {unno:0000} does not exist in the DataBase. \nDo you wish to proceed?", "Attention!") ==
                MessageDialogResult.Yes) return true;
            return false;
        }
        #endregion


        #region Methods affecting view changes
        // --------------- Methods affecting view changes ---------------------------

        /// <summary>
        /// Updates Dg info from DgListDataBase for the unit and updates presentation
        /// </summary>
        private void UpdateDgInfoAndUploadChanges()
        {
            Model.UpdateDgInfo(_dgDataBase);
            UpdateDgDataPresentation();
        }

        /// <summary>
        /// Called when packing group is updated. Contains logic to process the PKG change.
        /// </summary>
        private void OnUpdatePackingGroup()
        {
            Model.AssignFromDgList(_dgDataBase, false, true);
            OnDgPackingGroupChangedEventHandler.Invoke(this);
            UpdateDgDataPresentation();
        }

        /// <summary>
        /// Notifies CargoPlan of change in DgNetWeight.
        /// </summary>
        private void OnNetWeightChanged()
        {
            DataMessenger.Default.Send<UpdateCargoPlan>(new UpdateCargoPlan(), "Net weight changed");
        }

        /// <summary>
        /// Invokes event to update conflict list
        /// </summary>
        private void UpdateConflictList()
        {
            OnConflictListToBeChangedEventHandler.Invoke(this);
        }

        private void UpdateDgStowageConflicts()
        {
            DataMessenger.Default.Send(new ConflictListToBeUpdatedMessage(this));
        }

        /// <summary>
        /// Calls OnPropertyChanged for most of Dg properties
        /// </summary>
        private void UpdateDgDataPresentation()
        {
            OnPropertyChanged("Unno");
            OnPropertyChanged("DgClass");
            OnPropertyChanged("DgSubclass");
            OnPropertyChanged("Name");
            OnPropertyChanged("PackingGroup");
            OnPropertyChanged("AllDgClasses");
            OnPropertyChanged("Liquid");
            OnPropertyChanged("Flammable");
            OnPropertyChanged("EmitFlammableVapours");
            OnPropertyChanged("StowageCat");
            OnPropertyChanged("StowageSW");
            OnPropertyChanged("SegregationSG");
            OnPropertyChanged("SegregationGroup");
            OnPropertyChanged("Special");
            OnPropertyChanged("IsMax1L");
            OnPropertyChanged("IsWaste");
            OnPropertyChanged("Properties");
        }

        /// <summary>
        /// Calls OnPropertyChanged for all location related properties
        /// </summary>
        private void UpdateLocationPresentation()
        {
            OnPropertyChanged("Location");
            OnPropertyChanged("Bay");
            OnPropertyChanged("Row");
            OnPropertyChanged("Tier");
            OnPropertyChanged("HoldNr");
            OnPropertyChanged("IsUnderdeck");
            OnPropertyChanged("Size");
            OnPropertyChanged("LocationSortable");
        }

        /// <summary>
        /// Sends message to synchronise changes with CargoPlan
        /// </summary>
        /// <param name="value">new value set</param>
        /// <param name="oldValue">old value</param>
        /// <param name="propertyName">property that is changed</param>
        private void SetToAllContainersInPlan(object value, object oldValue = null, [CallerMemberName] string propertyName = null)
        {
            DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(this, value, oldValue, propertyName));
        }
        #endregion

    }
}
