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
        /// Copies container info from one <see cref="ContainerAbstract"/> to another.
        /// Present method calls respective method from <see cref="Model.IO"/>
        /// </summary>
        /// <param name="copyTo"></param>
        /// <param name="copyFrom"></param>
        public static void CopyContainerInfo(this ContainerAbstract copyTo, ContainerAbstract copyFrom)
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
                if (!HandleUnitWithSameNumberInList(cargoPlan, container))
                {
                    return false;
                }
            }
            #endregion

            cargoPlan.Containers.Add(container);
            if (container.IsRf) cargoPlan.Reefers.Add(container);
            return true;
        }


        /// <summary>
        /// For unit with container number that already exists in the plan, if <see cref="Location"/> is different => append container number with alpha-numerical appendix and do respective change for Dg in <see cref="CargoPlan.DgList"/>
        /// </summary>
        /// <param name="cargoPlan"></param>
        /// <param name="container"></param>
        /// <returns>True if successfully changed and the unit has unique <see cref="ContainerNumber"/>. False if unit with the same number exists in the same position.</returns>
        private static bool HandleUnitWithSameNumberInList(CargoPlan cargoPlan, Container container)
        {
            var sameNumberUnit = cargoPlan.Containers.FindContainerByContainerNumber(container.ContainerNumber);
            if (sameNumberUnit is null) return true;

            if (sameNumberUnit.Location == container.Location) return false;

            container.ContainerNumber = AppendDoublerContainerNumber(container, cargoPlan);
            Data.LogWriter.Write($"Container number {sameNumberUnit.ContainerNumber} appended as {container.ContainerNumber}");
            AppendDgContainerNumber(sameNumberUnit.ContainerNumber, container, cargoPlan);

            return true;
        }

        /// <summary>
        /// Searches if <see cref="Dg"/> with initialContainerNumber exists in Location of appendedNumberContainer in <see cref="CargoPlan"/> and appends ContainerNumebr of such <see cref="Dg"/>
        /// </summary>
        /// <param name="initialContainerNumber"></param>
        /// <param name="appendedNumberContainer"></param>
        /// <param name="cargoPlan"></param>
        private static void AppendDgContainerNumber(string initialContainerNumber, Container appendedNumberContainer, CargoPlan cargoPlan)
        {
            foreach (var unit in cargoPlan.DgList.FindAll(dg => dg.ContainerNumber == initialContainerNumber && dg.Location == appendedNumberContainer.Location))
            {
                if (unit is null) continue;
                unit.ContainerNumber = appendedNumberContainer.ContainerNumber;
            }
        }

        /// <summary>
        /// Adds unique alpha-numerical appendix to container number.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="cargoPlan"></param>
        /// <returns>New appended container number.</returns>
        private static string AppendDoublerContainerNumber(Container container, CargoPlan cargoPlan)
        {
            int i = 0;
            string newNumber = string.Empty;
            while (i < 1000)
            {
                newNumber = $"{container.ContainerNumber}-{i:000}";
                if (cargoPlan.Containers.FindContainerByContainerNumber(newNumber) is null)
                {
                    return newNumber;
                }
                i++;
            }
            string appendix = string.Empty;
            for (int x = 65; x < 91; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    appendix = $"{(char)x}{y:00}";
                    newNumber = $"{container.ContainerNumber}-{appendix}";
                    if (cargoPlan.Containers.FindContainerByContainerNumber(newNumber) is null)
                    {
                        return newNumber;
                    }
                }
            }
            for (int x = 65; x < 91; x++)
            {
                for (int y = 65; y < 91; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        appendix = $"{(char)x}{(char)y}{z}";
                        newNumber = $"{container.ContainerNumber}-{appendix}";
                        if (cargoPlan.Containers.FindContainerByContainerNumber(newNumber) is null)
                        {
                            return newNumber;
                        }
                    }
                }
            }
            return "error";
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
