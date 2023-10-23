using System.Collections.Generic;
using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.Model.IO.EasyJobCondition
{
    internal class ConditionUnit : Container
    {
        protected internal List<Dg> DgCargoInContainer;

        /// <summary>
        /// Converts Container into ConditionUnit.
        /// </summary>
        /// <param name="container">Container to be converted.</param>
        /// <returns>new ConditionUnit.</returns>
        public ConditionUnit ToConditionUnit(Container container)
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
        /// Converts Dg into ConditionUnit (without Dg properties).
        /// </summary>
        /// <param name="dg">Dg to be converted.</param>
        /// <returns>New ConditionUnit.</returns>
        public ConditionUnit ToConditionUnit(Dg dg)
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
                //IsUnderdeck = dg.IsUnderdeck,
                ContainerTypeRecognized = dg.ContainerTypeRecognized
            };
        }

        /// <summary>
        /// Converts ConditionUnit to Container with no Dg info
        /// </summary>
        /// <returns>New Container</returns>
        private Container ConvertToContainer()
        {
            Container container = new Container()
            {
                ContainerNumber = ContainerNumber,
                Location = Location,
                POL = POL,
                POD = POD,
                FinalDestination = FinalDestination,
                Carrier = Carrier,
                ContainerType = ContainerType,
                IsClosed = IsClosed,
                Remarks = Remarks,

                IsPositionLockedForChange = IsPositionLockedForChange,
                IsToBeKeptInPlan = IsToBeKeptInPlan,
                IsNewUnitInPlan = IsNewUnitInPlan,
                HasLocationChanged = HasLocationChanged,
                HasUpdated = HasUpdated,
                HasPodChanged = HasPodChanged,
                HasContainerTypeChanged = HasContainerTypeChanged,

                IsRf = IsRf,
                ContainerTypeRecognized = ContainerTypeRecognized
            };

            return container;
        }

        /// <summary>
        /// Fully converts ConditionUnit into Dg by given index in DgCargo list of the unit
        /// </summary>
        /// <param name="index">Index of Dg in DgCargo list of ConditionUnit</param>
        /// <returns>New dg</returns>
        internal Dg ConvertToDg(int index = 1)
        {
            Dg dg = this.ConvertToDgOnly(index);
            dg.CopyContainerInfo(this.ConvertToContainer());
            return dg;
        }

        /// <summary>
        /// Converts ConditionUnit into Dg by given index in DgCargo list of the unit and updates container info from given container
        /// </summary>
        /// <param name="container">Container to update container info</param>
        /// <param name="index"Index of Dg in DgCargo list of ConditionUnit></param>
        /// <returns></returns>
        internal Dg ConvertToDg(Container container, int index = 1)
        {
            Dg dg = ConvertToDgOnly(index);
            dg.CopyContainerInfo(container);
            return dg;
        }

        /// <summary>
        /// Converts ConditionUnit into Dg contained in DgCargo by selected index
        /// </summary>
        /// <param name="index">Index of Dg in DgCargo list of ConditionUnit</param>
        /// <returns>New Dg</returns>
        private Dg ConvertToDgOnly(int index)
        {
            Dg dg = new Dg();

            dg.Unno = DgCargoInContainer[index].Unno;
            dg.DgClass = DgCargoInContainer[index].DgClass;
            dg.DgSubclass = DgCargoInContainer[index].DgSubclass;
            dg.DgNetWeight = DgCargoInContainer[index].DgNetWeight;
            dg.PackingGroupByte = DgCargoInContainer[index].PackingGroupByte;
            dg.FlashPoint = DgCargoInContainer[index].FlashPoint;
            dg.IsMp = DgCargoInContainer[index].IsMp;
            dg.IsLq = DgCargoInContainer[index].IsLq;
            dg.Name = DgCargoInContainer[index].Name;
            dg.TechnicalName = DgCargoInContainer[index].TechnicalName;
            dg.IsNameChanged = DgCargoInContainer[index].IsNameChanged;
            dg.IsTechnicalNameIncluded = DgCargoInContainer[index].IsTechnicalNameIncluded;
            dg.StowageCat = DgCargoInContainer[index].StowageCat;
            dg.IsMax1L = DgCargoInContainer[index].IsMax1L;
            dg.IsWaste = DgCargoInContainer[index].IsWaste;
            dg.EmergencyContacts = DgCargoInContainer[index].DgEMS;
            dg.SegregatorClass = DgCargoInContainer[index].SegregationGroup;
            dg.NumberAndTypeOfPackages = DgCargoInContainer[index].NumberAndTypeOfPackages;
            dg.EmergencyContacts = DgCargoInContainer[index].EmergencyContacts;
            dg.Remarks = DgCargoInContainer[index].Remarks;

            dg.IsPositionLockedForChange = DgCargoInContainer[index].IsPositionLockedForChange;
            dg.IsToBeKeptInPlan = DgCargoInContainer[index].IsToBeKeptInPlan;
            dg.IsNewUnitInPlan = DgCargoInContainer[index].IsNewUnitInPlan;
            dg.HasLocationChanged = DgCargoInContainer[index].HasLocationChanged;
            dg.HasUpdated = DgCargoInContainer[index].HasUpdated;
            dg.HasPodChanged = DgCargoInContainer[index].HasPodChanged;
            dg.HasContainerTypeChanged = DgCargoInContainer[index].HasContainerTypeChanged;

            return dg;
        }

        #region Constructor

        internal ConditionUnit() : base()
        {

        } 

        #endregion
    }
}
