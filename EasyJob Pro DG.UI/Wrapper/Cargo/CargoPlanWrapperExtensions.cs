using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;
using System;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    /// <summary>
    /// Responsible for add/remove logic for units in <see cref="CargoPlanWrapper"/>
    /// Extends <see cref="CargoPlanWrapper"/>.
    /// Does NOT update confilcts!
    /// </summary>
    internal static class CargoPlanWrapperExtensions
    {
        internal static bool AddNewContainer(this CargoPlanWrapper cargoPlan, ContainerWrapper container)
        {
            if (container is null) return false;
            if (string.IsNullOrEmpty(container.ContainerNumber)) return false;

            if (cargoPlan.Model.Containers.ContainsUnitWithSameContainerNumberInList(container)) return false;
            if (cargoPlan.Containers.ContainsUnitWithSameContainerNumberInList(container)) return false;

            container.Model.HoldNr = ShipProfile.DefineCargoHoldNumber(container.Bay);

            cargoPlan.Containers.Add(container);
            if (container.IsRf)
                cargoPlan.Reefers.Add(container);

            cargoPlan.RefreshCargoPlanValues(container);

            return true;
        }

        /// <summary>
        /// Adds a Dg to WorkingCargoPlan and its Wrapper to CargoPlanWrapper
        /// </summary>
        /// <param name="dg"></param>
        internal static bool AddDg(this CargoPlanWrapper cargoPlan, DgWrapper dg)
        {
            if (dg == null || string.IsNullOrEmpty(dg.ContainerNumber)) return false;


            var container = cargoPlan.Containers.FindContainerByContainerNumber(dg);

            // if new container - create and add to the plan
            if (container is null)
            {
                dg.Model.HoldNr = ShipProfile.DefineCargoHoldNumber(dg.Bay);
                container = (ContainerWrapper)dg;
                cargoPlan.Containers.Add(container);
                if (dg.IsRf) cargoPlan.Reefers.Add(container);
            }
            // for existing container - only copy info
            else
            {
                dg.Model.CopyContainerInfo(container.Model);
            }

            dg.Model.UpdateDgInfo();
            cargoPlan.DgList.Add(dg);
            container.Model.DgCountInContainer++;

            cargoPlan.RefreshCargoPlanValues(container);
            return true;
        }

        /// <summary>
        /// Adds a new reefer Container to WorkingCargoPlan
        /// </summary>
        /// <param name="unit">Container to be added</param>
        internal static bool AddNewReefer(this CargoPlanWrapper cargoPlan, ContainerWrapper unit)
        {
            if (unit is null) return false;
            if (string.IsNullOrEmpty(unit.ContainerNumber)) return false;

            //if already exists -> no action
            if (cargoPlan.Model.Reefers.ContainsUnitWithSameContainerNumberInList(unit)) return false;

            unit.Model.IsRf = true;
            unit.Model.HoldNr = ShipProfile.DefineCargoHoldNumber(unit.Bay);

            //add to WorkingCargoPlan
            ContainerWrapper containerWrapper;
            if (!cargoPlan.Containers.ContainsUnitWithSameContainerNumberInList(unit))
            {
                containerWrapper = unit;
                cargoPlan.Containers.Add(containerWrapper);
            }
            else
            {
                containerWrapper = cargoPlan.Containers.FindContainerByContainerNumber(unit);
                if (containerWrapper == null) throw new ArgumentException($"Container with ContainerNumber {unit.ContainerNumber} cannot be found in CargoPlan.Containers despite it is expected");
                containerWrapper.IsRf = true;
            }

            cargoPlan.Reefers.Add(containerWrapper);

            cargoPlan.RefreshCargoPlanValues(containerWrapper);
            return true;
        }

        /// <summary>
        /// Adds a ContainerWrapper to Reefers list
        /// </summary>
        /// <param name="unit">ContainerWrapper to be added</param>
        internal static void AddReefer(this CargoPlanWrapper cargoPlan, ContainerWrapper unit)
        {
            if (cargoPlan.Model.Reefers.Contains(unit.Model)) return;
            cargoPlan.Reefers.Add(unit);
            unit.IsRf = true;

            RefreshCargoPlanValues(cargoPlan);
        }

        /// <summary>
        /// Removes container from WorkingCargoPlan (also from Reefers and DgList).
        /// </summary>
        /// <param name="containerNumber">ContainerNumber of a container to be deleted.</param>
        internal static void RemoveContainer(this CargoPlanWrapper cargoPlan, string containerNumber)
        {
            var unit = cargoPlan.Containers.FindContainerByContainerNumber(containerNumber);

            unit.ClearSubscriptions();
            cargoPlan.Containers.Remove(unit);

            if (unit.IsRf)
            {
                var reefer = cargoPlan.Reefers.FindContainerByContainerNumber(containerNumber);
                cargoPlan.Reefers.Remove(reefer);
            }

            for (int i = 0; i < cargoPlan.DgList.Count; i++)
            {
                var d = cargoPlan.DgList[i];

                if (d.ContainerNumber != containerNumber) continue;

                d.ClearSubscriptions();
                cargoPlan.DgList.Remove(d);
                cargoPlan.Model.DgList.Remove(d.Model);
                i--;
            }

            RefreshCargoPlanValues(cargoPlan);
        }

        /// <summary>
        /// Removes reefer unit from WorkingCargoPlan and Model Reefers
        /// </summary>
        /// <param name="unit">Reefer to be removed</param>
        /// <returns>Void</returns>
        internal static void RemoveReefer(this CargoPlanWrapper cargoPlan, ContainerWrapper unit)
        {
            cargoPlan.Reefers.Remove(cargoPlan.Reefers.FindContainerByContainerNumber(unit));
        }

        /// <summary>
        /// Removes reefer unit from WorkingCargoPlan and Model Reefers.
        /// </summary>
        /// <param name="unitNumber">Reefer to be removed ContainerNumber.</param>
        /// <param name="toUpdateInCargoPlan">If required to update IsRf property in Dg and Containers.</param>
        internal static void RemoveReefer(this CargoPlanWrapper cargoPlan, string unitNumber, bool toUpdateInCargoPlan = false)
        {
            cargoPlan.Reefers.Remove(cargoPlan.Reefers.FindContainerByContainerNumber(unitNumber));

            if (toUpdateInCargoPlan)
            {
                var container = cargoPlan.Containers.FindContainerByContainerNumber(unitNumber);
                container.Model.IsRf = false;
                container.ResetReefer();
                container.Refresh();

                foreach (var dg in cargoPlan.DgList.Where(x => x.ContainerNumber == unitNumber))
                {
                    dg.Model.IsRf = false;
                    dg.RefreshIsRfProperty();
                }

                RefreshCargoPlanValues(cargoPlan);
            }
        }

        /// <summary>
        /// Method removes dg from DgWrapperList and from model as well
        /// </summary>
        /// <param name="dg"></param>
        internal static void RemoveDg(this CargoPlanWrapper cargoPlan, DgWrapper dg)
        {
            dg.ClearSubscriptions();
            cargoPlan.DgList.Remove(dg);
        }

        /// <summary>
        /// Reduces number of DgInContainer property of all Containers in WorkingCargoPlan
        /// </summary>
        /// <param name="containerNumber"></param>
        internal static void RemoveRemovedDgFromCargoPlan(this CargoPlanWrapper cargoPlan, IContainer unit)
        {
            var container = cargoPlan.Containers.FindContainerByContainerNumber(unit);
            if (container is null) return;

            container?.Model.RemoveDgFromContainer();
            container?.Refresh();

            if (container.IsRf)
            {
                var reefer = cargoPlan.Reefers.FindContainerByContainerNumber(unit);
                reefer?.Refresh();
            }
        }


        /// <summary>
        /// Method updates summary and sends a message to update the conflicts list.
        /// </summary>
        private static void RefreshCargoPlanValues(this CargoPlanWrapper cargoPlan)
        {
            cargoPlan.RefreshCargoPlanValues();
        }

        /// <summary>
        /// Method refreshes the wrapper, updates summary values and sends a message to update conflicts list.
        /// </summary>
        /// <param name="wrapper">The ContainerWrapper which PropertyChange resulted in change of plan.</param>
        private static void RefreshCargoPlanValues(this CargoPlanWrapper cargoPlan, ContainerWrapper wrapper)
        {
            wrapper.Refresh();
            if (wrapper.IsRf)
                cargoPlan.Reefers.FindContainerByContainerNumber(wrapper).Refresh();

            RefreshCargoPlanValues(cargoPlan);
        }
    }
}
