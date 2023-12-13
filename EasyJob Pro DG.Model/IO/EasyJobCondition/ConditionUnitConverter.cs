using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.Model.IO.EasyJobCondition
{
    internal static class ConditionUnitConverter
    {
        /// <summary>
        /// Converts <see cref="Container"/> into <see cref="ConditionUnit"/>
        /// </summary>
        /// <param name="container">Container to be converted.</param>
        /// <returns>new ConditionUnit.</returns>
        public static ConditionUnit ConvertToConditionUnit(this Container container)
        {
            return new ConditionUnit()
            {
                ContainerNumber = container.ContainerNumber,
                Location = container.Location,
                POL = container.POL,
                POD = container.POD,
                FinalDestination = container.FinalDestination,
                Carrier = container.Carrier,
                ContainerType = container.ContainerType,
                IsClosed = container.IsClosed,
                Remarks = container.Remarks,
                LocationBeforeRestow = container.LocationBeforeRestow,

                ReeferRemark = container.ReeferRemark,
                SetTemperature = container.SetTemperature,
                VentSetting = container.VentSetting,
                Commodity = container.Commodity,
                ReeferSpecial = container.ReeferSpecial,
                LoadTemperature = container.LoadTemperature,

                IsPositionLockedForChange = container.IsPositionLockedForChange,
                IsToBeKeptInPlan = container.IsToBeKeptInPlan,
                IsToImport = container.IsToImport,
                IsNotToImport = container.IsNotToImport,
                IsNewUnitInPlan = container.IsNewUnitInPlan,
                HasLocationChanged = container.HasLocationChanged,
                HasUpdated = container.HasUpdated,
                HasPodChanged = container.HasPodChanged,
                HasContainerTypeChanged = container.HasContainerTypeChanged,

                DgCountInContainer = container.DgCountInContainer,
                IsRf = container.IsRf,
                ContainerTypeRecognized = container.ContainerTypeRecognized,
            };
        }

        /// <summary>
        /// Converts <see cref="Dg"/> into <see cref="ConditionUnit"/> (without Dg properties).
        /// </summary>
        /// <param name="dg">Dg to be converted.</param>
        /// <returns>New ConditionUnit.</returns>
        public static ConditionUnit ConvertToConditionUnit(this Dg dg)
        {
            return new ConditionUnit()
            {
                ContainerNumber = dg.ContainerNumber,
                Location = dg.Location,
                POL = dg.POL,
                POD = dg.POD,
                FinalDestination = dg.FinalDestination,
                Carrier = dg.Carrier,
                ContainerType = dg.ContainerType,
                IsClosed = dg.IsClosed,
                LocationBeforeRestow = dg.LocationBeforeRestow,

                IsPositionLockedForChange = dg.IsPositionLockedForChange,
                IsToBeKeptInPlan = dg.IsToBeKeptInPlan,
                IsToImport = dg.IsToImport,
                IsNotToImport = dg.IsNotToImport,
                IsNewUnitInPlan = dg.IsNewUnitInPlan,
                HasLocationChanged = dg.HasLocationChanged,
                HasUpdated = dg.HasUpdated,
                HasPodChanged = dg.HasPodChanged,
                HasContainerTypeChanged = dg.HasContainerTypeChanged,

                IsRf = dg.IsRf,
                ContainerTypeRecognized = dg.ContainerTypeRecognized
            };
        }

        /// <summary>
        /// Fully converts <see cref="ConditionUnit"/> into <see cref="Dg"/> by given index in DgCargo list of the unit
        /// </summary>
        /// <param name="index">Index of Dg in DgCargo list of ConditionUnit</param>
        /// <returns>New dg</returns>
        internal static Dg ConvertToDg(this ConditionUnit conditionUnit, int index = 1)
        {
            Dg dg = conditionUnit.ConvertToDgOnly(index);
            dg.CopyContainerAbstractInfo(conditionUnit);
            return dg;
        }

        /// <summary>
        /// Converts <see cref="ConditionUnit"/> into <see cref="Dg"/> by given index in DgCargo list of the unit and updates container info from given <see cref="Container"/>
        /// </summary>
        /// <param name="container">Container to update container info</param>
        /// <param name="index"Index of Dg in DgCargo list of ConditionUnit></param>
        /// <returns></returns>
        internal static Dg ConvertToDg(this ConditionUnit conditionUnit, Container container, int index = 1)
        {
            Dg dg = conditionUnit.ConvertToDgOnly(index);
            dg.CopyContainerAbstractInfo(container);
            return dg;
        }

        /// <summary>
        /// Converts <see cref="ConditionUnit"/> into <see cref="Dg"/> contained in DgCargo by selected index
        /// </summary>
        /// <param name="index">Index of Dg in DgCargo list of ConditionUnit</param>
        /// <returns>New Dg</returns>
        private static Dg ConvertToDgOnly(this ConditionUnit conditionUnit, int index)
        {
            Dg dg = new Dg();

            dg.Unno = conditionUnit.DgCargoInContainer[index].Unno;
            dg.DgClass = conditionUnit.DgCargoInContainer[index].DgClass;
            dg.DgSubClassArray = conditionUnit.DgCargoInContainer[index].DgSubClassArray;
            dg.DgNetWeight = conditionUnit.DgCargoInContainer[index].DgNetWeight;
            dg.PackingGroupAsByte = conditionUnit.DgCargoInContainer[index].PackingGroupAsByte;
            dg.FlashPoint = conditionUnit.DgCargoInContainer[index].FlashPoint;
            dg.IsMp = conditionUnit.DgCargoInContainer[index].IsMp;
            dg.IsLq = conditionUnit.DgCargoInContainer[index].IsLq;
            dg.Name = conditionUnit.DgCargoInContainer[index].Name;
            dg.TechnicalName = conditionUnit.DgCargoInContainer[index].TechnicalName;
            dg.IsNameChanged = conditionUnit.DgCargoInContainer[index].IsNameChanged;
            dg.IsTechnicalNameIncluded = conditionUnit.DgCargoInContainer[index].IsTechnicalNameIncluded;
            dg.StowageCat = conditionUnit.DgCargoInContainer[index].StowageCat;
            dg.IsMax1L = conditionUnit.DgCargoInContainer[index].IsMax1L;
            dg.IsWaste = conditionUnit.DgCargoInContainer[index].IsWaste;
            dg.EmergencyContacts = conditionUnit.DgCargoInContainer[index].DgEMS;
            dg.SegregatorClass = conditionUnit.DgCargoInContainer[index].SegregationGroup;
            dg.NumberAndTypeOfPackages = conditionUnit.DgCargoInContainer[index].NumberAndTypeOfPackages;
            dg.EmergencyContacts = conditionUnit.DgCargoInContainer[index].EmergencyContacts;
            dg.Remarks = conditionUnit.DgCargoInContainer[index].Remarks;

            dg.IsPositionLockedForChange = conditionUnit.DgCargoInContainer[index].IsPositionLockedForChange;
            dg.IsToBeKeptInPlan = conditionUnit.DgCargoInContainer[index].IsToBeKeptInPlan;
            dg.IsNewUnitInPlan = conditionUnit.DgCargoInContainer[index].IsNewUnitInPlan;
            dg.HasLocationChanged = conditionUnit.DgCargoInContainer[index].HasLocationChanged;
            dg.HasUpdated = conditionUnit.DgCargoInContainer[index].HasUpdated;
            dg.HasPodChanged = conditionUnit.DgCargoInContainer[index].HasPodChanged;
            dg.HasContainerTypeChanged = conditionUnit.DgCargoInContainer[index].HasContainerTypeChanged;

            return dg;
        }
    }
}
