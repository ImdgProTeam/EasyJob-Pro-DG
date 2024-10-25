using EasyJob_ProDG.Model.IO;

namespace EasyJob_ProDG.Model.Cargo
{
    internal static class ContainerCopier
    {
        /// <summary>
        /// Copies <see cref="IContainer"/>, <see cref="LocationOnBoard"/>, <see cref="IUpdatable"/> info from one <see cref="ContainerAbstract"/> to another.
        /// </summary>
        /// <param name="containerCopyTo"></param>
        /// <param name="containerCopyFrom"></param>
        public static void CopyContainerAbstractInfo(this ContainerAbstract containerCopyTo, ContainerAbstract containerCopyFrom)
        {
            containerCopyTo.ContainerNumber = containerCopyFrom.ContainerNumber;
            containerCopyTo.ContainerType = containerCopyFrom.ContainerType;
            containerCopyTo.Location = containerCopyFrom.Location;
            containerCopyTo.LocationBeforeRestow = containerCopyFrom.LocationBeforeRestow;
            containerCopyTo.HoldNr = containerCopyFrom.HoldNr;
            containerCopyTo.IsClosed = containerCopyFrom.IsClosed;
            containerCopyTo.POD = containerCopyFrom.POD;
            containerCopyTo.POL = containerCopyFrom.POL;
            containerCopyTo.FinalDestination = containerCopyFrom.FinalDestination;
            containerCopyTo.IsRf = containerCopyFrom.IsRf;
            containerCopyTo.Carrier = containerCopyFrom.Carrier;

            containerCopyTo.IsPositionLockedForChange = containerCopyFrom.IsPositionLockedForChange;
            containerCopyTo.IsToBeKeptInPlan = containerCopyFrom.IsToBeKeptInPlan;
            containerCopyTo.IsToImport = containerCopyFrom.IsToImport;
            containerCopyTo.IsNotToImport = containerCopyFrom.IsNotToImport;
            containerCopyTo.IsNewUnitInPlan = containerCopyFrom.IsNewUnitInPlan;
            containerCopyTo.HasLocationChanged = containerCopyFrom.HasLocationChanged;
            containerCopyTo.HasUpdated = containerCopyFrom.HasUpdated;
            containerCopyTo.HasContainerTypeChanged = containerCopyFrom.HasContainerTypeChanged;
            containerCopyTo.HasPodChanged = containerCopyFrom.HasPodChanged;
        }

        /// <summary>
        /// Copies basic <see cref="Container"/> properties from another <see cref="Container"/>
        /// </summary>
        /// <param name="container"></param>
        public static void CopyContainerOnlyInfo(this Container containerCopyTo, Container containerCopyFrom)
        {
            containerCopyTo.Location = containerCopyFrom.Location;
            containerCopyTo.DgCountInContainer = containerCopyFrom.DgCountInContainer;
            containerCopyTo.IsRf = containerCopyFrom.IsRf;

            containerCopyTo.ContainerType = containerCopyFrom.ContainerType;
            containerCopyTo.ContainerTypeRecognized = containerCopyFrom.ContainerTypeRecognized;
            containerCopyTo.IsClosed = containerCopyFrom.IsClosed;

            containerCopyTo.Carrier = containerCopyFrom.Carrier;
            containerCopyTo.FinalDestination = containerCopyFrom.FinalDestination;
            containerCopyTo.POD = containerCopyFrom.POD;
            containerCopyTo.POL = containerCopyFrom.POL;
        }

        /// <summary>
        /// Copies all non-importable information from source Dg.
        /// </summary>
        /// <param name="dgCopyFrom">Dg from which info shall be copied.</param>
        internal static void CopyNonImportableDgInfo(this Dg dgCopyTo, Dg dgCopyFrom)
        {
            dgCopyTo.Remarks = dgCopyFrom.Remarks;
            dgCopyTo.EmergencyContacts = dgCopyFrom.EmergencyContacts;
        }


        /// <summary>
        /// Copies only <see cref="Type"/> and <see cref="POD"/> if Changed respectively from one <see cref="ContainerAbstract"/> to another.
        /// </summary>
        /// <param name="toUnit"></param>
        /// <param name="fromUnit"></param>
        internal static void CopyUpdatedTypeAndPODInfo(this ContainerAbstract toUnit, ContainerAbstract fromUnit)
        {
            if (fromUnit.HasContainerTypeChanged)
                toUnit.ContainerType = fromUnit.ContainerType;
            if (fromUnit.HasPodChanged)
                toUnit.POD = fromUnit.POD;
        }

        /// <summary>
        /// Copies <see cref="IUpdatable"/> properties from one unit to another
        /// </summary>
        /// <param name="fromUnit">From unit</param>
        /// <param name="toUnit">To unit</param>
        internal static void CopyIUpdatableToDg(this IUpdatable toUnit, IUpdatable fromUnit)
        {
            toUnit.IsNewUnitInPlan = fromUnit.IsNewUnitInPlan;
            toUnit.HasLocationChanged = fromUnit.HasLocationChanged;
            if (toUnit.HasLocationChanged)
            {
                toUnit.Location = fromUnit.Location;
            }
            toUnit.LocationBeforeRestow = fromUnit.LocationBeforeRestow;
            toUnit.HasPodChanged = fromUnit.HasPodChanged;
            toUnit.HasContainerTypeChanged = fromUnit.HasContainerTypeChanged;
        }
    }
}
