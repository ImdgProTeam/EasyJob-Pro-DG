﻿using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper.Cargo;
using System;
using System.Runtime.CompilerServices;

namespace EasyJob_ProDG.UI.Wrapper
{
    public partial class DgWrapper : AbstractContainerWrapper<Dg>
    {
        #region Public methods
        // --------------- Methods ---------------------------------------

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
            DataMessenger.Default.Send(new ConflictListToBeUpdatedMessage());
        }     
        
        /// <summary>
        /// Invokes sending of <see cref="DgListSelectedItemUpdatedMessage"/>
        /// Used by SelectionStatusBar
        /// </summary>
        private void NotifyOfChangedProperties()
        {
            DataMessenger.Default.Send(new DgListSelectedItemUpdatedMessage(), "selectionpropertyupdated");
        }

        private void UpdateDgStowageConflicts()
        {
            DataMessenger.Default.Send(new ConflictListToBeUpdatedMessage(this));
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
        protected override void SetToAllContainersInPlan(object value, object oldValue = null, [CallerMemberName] string propertyName = null)
        {
            DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(this, value, oldValue, propertyName));
        }
        #endregion

    }
}
