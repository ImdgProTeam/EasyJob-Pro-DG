using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using System;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    internal static class CargoPlanWrapperExtensions
    {
        internal static void AddNewContainer(this CargoPlanWrapper cargoPlan, Container container)
        {
            if (cargoPlan.Model.Containers.ContainsUnitWithSameContainerNumberInList(container)) return;
            if (cargoPlan.Containers.ContainsUnitWithSameContainerNumberInList(container)) return;

            container.HoldNr = ShipProfile.DefineCargoHoldNumber(container.Bay);

            //add to Model
            if (!cargoPlan.Model.AddContainer(container)) return;

            var containerWrapper = new ContainerWrapper(container);
            cargoPlan.Containers.Add(containerWrapper);
            if (container.IsRf)
                cargoPlan.Reefers.Add(containerWrapper);

            cargoPlan.UpdateCargoPlanValuesAndConflicts(containerWrapper);
        }

        /// <summary>
        /// Adds a Dg to WorkingCargoPlan and its Wrapper to CargoPlanWrapper
        /// </summary>
        /// <param name="dg"></param>
        internal static void AddDg(this CargoPlanWrapper cargoPlan, Dg dg)
        {
            if (dg == null) return;

            dg.HoldNr = ShipProfile.DefineCargoHoldNumber(dg.Bay);

            cargoPlan.Model.AddDg(dg);

            var containerWrapper = cargoPlan.Containers.FindContainerByContainerNumber(dg);
            if (containerWrapper is null)
            {
                var container = cargoPlan.Model.Containers.FindContainerByContainerNumber(dg);
                containerWrapper = new ContainerWrapper(container);
                cargoPlan.Containers.Add(containerWrapper);
                if (container.IsRf) cargoPlan.Reefers.Add(containerWrapper);
            }
            cargoPlan.DgList.Add(new DgWrapper(dg));

            cargoPlan.UpdateCargoPlanValuesAndConflicts(containerWrapper);
        }

        /// <summary>
        /// Adds a new reefer Container to WorkingCargoPlan
        /// </summary>
        /// <param name="unit">Container to be added</param>
        internal static void AddNewReefer(this CargoPlanWrapper cargoPlan, Container unit)
        {
            //if already exists -> no action
            if (cargoPlan.Model.Reefers.ContainsUnitWithSameContainerNumberInList(unit)) return;

            unit.HoldNr = ShipProfile.DefineCargoHoldNumber(unit.Bay);

            //add to Model
            if (!cargoPlan.Model.AddReefer(unit)) return;

            //add to WorkingCargoPlan
            ContainerWrapper containerWrapper;
            if (!cargoPlan.Containers.ContainsUnitWithSameContainerNumberInList(unit))
            {
                var container = cargoPlan.Model.Containers.FindContainerByContainerNumber(unit);
                containerWrapper = new ContainerWrapper(container);
                cargoPlan.Containers.Add(containerWrapper);
            }
            else
            {
                containerWrapper = cargoPlan.Containers.FindContainerByContainerNumber(unit);
                if (containerWrapper == null) throw new ArgumentException($"Container with ContainerNumber {unit.ContainerNumber} cannot be found in CargoPlan.Containers despite it is expected");
                containerWrapper.IsRf = true;
            }
            cargoPlan.Reefers.Add(containerWrapper);

            cargoPlan.UpdateCargoPlanValuesAndConflicts(containerWrapper);
        }

        /// <summary>
        /// Adds the Container to Reefers list
        /// </summary>
        /// <param name="unit">Container to be added</param>
        internal static void AddReefer(this CargoPlanWrapper cargoPlan, Container unit)
        {
            if (cargoPlan.Model.Reefers.Contains(unit)) return;
            cargoPlan.Model.Reefers.Add(unit);
            cargoPlan.Reefers.Add(new ContainerWrapper(unit));
            unit.IsRf = true;

            cargoPlan.UpdateCargoPlanValuesAndConflicts();
        }

        /// <summary>
        /// Adds a ContainerWrapper to Reefers list
        /// </summary>
        /// <param name="unit">ContainerWrapper to be added</param>
        internal static void AddReefer(this CargoPlanWrapper cargoPlan, ContainerWrapper unit)
        {
            cargoPlan.AddReefer(unit.Model);
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
            cargoPlan.Model.Containers.Remove(unit.Model);

            if (unit.IsRf)
            {
                var reefer = cargoPlan.Reefers.FindContainerByContainerNumber(containerNumber);
                cargoPlan.Reefers.Remove(reefer);
                cargoPlan.Model.Reefers.Remove(reefer.Model);
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

            cargoPlan.UpdateCargoPlanValuesAndConflicts();
        }

        /// <summary>
        /// Removes reefer unit from WorkingCargoPlan and Model Reefers
        /// </summary>
        /// <param name="unit">Reefer to be removed</param>
        /// <returns>Void</returns>
        internal static void RemoveReefer(this CargoPlanWrapper cargoPlan, ContainerWrapper unit)
        {
            cargoPlan.Reefers.Remove(cargoPlan.Reefers.FindContainerByContainerNumber(unit));
            cargoPlan.Model.Reefers.Remove(cargoPlan.Model.Reefers.FindContainerByContainerNumber(unit));
        }

        /// <summary>
        /// Removes reefer unit from WorkingCargoPlan and Model Reefers.
        /// </summary>
        /// <param name="unitNumber">Reefer to be removed ContainerNumber.</param>
        /// <param name="toUpdateInCargoPlan">If required to update IsRf property in Dg and Containers.</param>
        internal static void RemoveReefer(this CargoPlanWrapper cargoPlan, string unitNumber, bool toUpdateInCargoPlan = false)
        {
            cargoPlan.Reefers.Remove(cargoPlan.Reefers.FindContainerByContainerNumber(unitNumber));
            cargoPlan.Model.Reefers.Remove(cargoPlan.Model.Reefers.FindContainerByContainerNumber(unitNumber));

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

                cargoPlan.UpdateCargoPlanValuesAndConflicts();
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
            cargoPlan.Model.DgList.Remove(dg.Model);
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
        private static void UpdateCargoPlanValuesAndConflicts(this CargoPlanWrapper cargoPlan)
        {
            DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage());
            cargoPlan.RefreshCargoPlanValues();
        }

        /// <summary>
        /// Method refreshes the wrapper, updates summary values and sends a message to update conflicts list.
        /// </summary>
        /// <param name="wrapper">The ContainerWrapper which PropertyChange resulted in change of plan.</param>
        private static void UpdateCargoPlanValuesAndConflicts(this CargoPlanWrapper cargoPlan, ContainerWrapper wrapper)
        {
            wrapper.Refresh();
            if (wrapper.IsRf)
                cargoPlan.Reefers.FindContainerByContainerNumber(wrapper).Refresh();

            cargoPlan.UpdateCargoPlanValuesAndConflicts();
        }
    }
}
