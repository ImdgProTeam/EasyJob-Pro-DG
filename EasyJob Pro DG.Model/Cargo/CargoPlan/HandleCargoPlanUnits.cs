using EasyJob_ProDG.Model.Transport;
using System;

namespace EasyJob_ProDG.Model.Cargo
{
    /// <summary>
    /// Class to handle add / remove and shifting of <see cref="CargoPlan"/> units within the plan
    /// </summary>
    public static class HandleCargoPlanUnits
    {
        //----------- Add/Remove methods -------------------------------------------------------------------

        /// <summary>
        /// Adds new Dg to CargoPlan
        /// </summary>
        /// <param name="dg">New dg to be added to plan</param>
        public static void AddDg(this CargoPlan cargoPlan, Dg dg)
        {
            if (dg == null || string.IsNullOrEmpty(dg.ContainerNumber)) return;

            var container = cargoPlan.Containers.FindContainerByContainerNumber(dg);
            if (container is null)
            {
                container = (Container)dg;
                cargoPlan.Containers.Add(container);
                if (dg.IsRf) cargoPlan.Reefers.Add(container);
            }
            else
            {
                dg.CopyContainerAbstractInfo(container);
            }
            dg.UpdateDgInfo();
            cargoPlan.DgList.Add(dg);
            container.DgCountInContainer++;
        }

        /// <summary>
        /// Copies container info from one <see cref="ContainerAbstract"/> to another.
        /// Present method calls respective method from <see cref="Model.IO"/>
        /// </summary>
        /// <param name="copyTo"></param>
        /// <param name="copyFrom"></param>
        public static void CopyContainerInfo (this ContainerAbstract copyTo, ContainerAbstract copyFrom)
        {
            copyTo.CopyContainerAbstractInfo(copyFrom);
        } 

        /// <summary>
        /// Adds new container to CargoPlan
        /// </summary>
        /// <param name="container">Container to add to the plan. Container number shall be unique.</param>
        /// <returns>True if container succesfully added to CargoPlan</returns>
        public static bool AddContainer(this CargoPlan cargoPlan, Container container)
        {
            #region Safety checks
            if (container is null) return false;
            if (string.IsNullOrEmpty(container.ContainerNumber))
            {
                Data.LogWriter.Write($"Attempt to add a container with no container number");
                return false;
            }
            if (cargoPlan.Containers.ContainsUnitWithSameContainerNumberInList(container))
            {
                Data.LogWriter.Write($"Attempt to add a container with container number which is already in list");
                return false;
            }
            #endregion

            cargoPlan.Containers.Add(container);
            if (container.IsRf) cargoPlan.Reefers.Add(container);
            return true;
        }

        /// <summary>
        /// Adds new reefer to CargoPlan
        /// </summary>
        /// <param name="container">Reefer container to be added. Container number shall be unique.</param>
        public static bool AddReefer(this CargoPlan cargoPlan, Container reefer)
        {
            #region Safety checks
            if (reefer is null) return false;
            if (string.IsNullOrEmpty(reefer.ContainerNumber))
            {
                Data.LogWriter.Write($"Attempt to add a reefer with no container number");
                return false;
            }
            #endregion

            if (cargoPlan.Containers.ContainsUnitWithSameContainerNumberInList(reefer))
            {
                Data.LogWriter.Write($"Attempt to add a reefer with container number which is already in list");
                var container = cargoPlan.Containers.FindContainerByContainerNumber(reefer) ?? throw new Exception($"Container with ContainerNumber {reefer.ContainerNumber} cannot be found in CargoPlan despite it was expected.");
                container.IsRf = true;
                reefer.CopyContainerAbstractInfo(container);
            }
            else
            {
                reefer.IsRf = true;
                cargoPlan.Containers.Add(reefer);
            }
            cargoPlan.Reefers.Add(reefer);
            return true;
        }


        /// <summary>
        /// Updates all <see cref="HoldNr"/> properties for all items in <see cref="CargoPlan"/>
        /// </summary>
        public static void OnCargoHoldsUpdated(this CargoPlan cargoPlan)
        {
            foreach (var unit in cargoPlan.DgList)
            {
                unit.HoldNr = ShipProfile.DefineCargoHoldNumber(unit.Bay);
            }
            foreach (var container in cargoPlan.Containers)
            {
                container.HoldNr = ShipProfile.DefineCargoHoldNumber(container.Bay);
            }
        }
    }
}
