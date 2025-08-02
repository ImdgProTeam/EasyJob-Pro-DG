using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.Model.IO.Excel;
using EasyJob_ProDG.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public static class HandleCargoPlan
    {
        /// <summary>
        /// Clears all dg conflicts in DgList
        /// </summary>
        public static void ClearConflicts(this CargoPlan cargoPlan)
        {
            foreach (var dg in cargoPlan.DgList)
                dg.ClearAllConflicts();
        }


        /// <summary>
        /// Creates CargoPlan from a readable file and updates it with IMDG info from database
        /// </summary>
        /// <param name="fileName">Full condition file name and path.</param>
        /// <param name="ownShip">Current ShipProfile.</param>
        /// <param name="dgDataBase">IMDG code Dg database in xml format.</param>
        /// <param name="openOption">Enumeration of possible option how to open the file.</param>
        /// <param name="existingCargoPlan">Present cargo plan, i.e. which will be updated.</param>
        /// <param name="importOnlySelected">In case of import: Sets if required to import only selected items.</param>
        /// <param name="currentPort">In case of import: POL for selecting Import.</param>
        public static CargoPlan CreateCargoPlan(string fileName, OpenFile.OpenOption openOption = OpenFile.OpenOption.Open,
                                        CargoPlan existingCargoPlan = null, bool importOnlySelected = false, string currentPort = null)
        {
            ShipProfile ownShip = ShipProfile.Instance;

            //creating cargo plan from file
            var cargoPlan = OpenFile.ReadCargoPlanFromFile(fileName);
            if (cargoPlan is null || cargoPlan.IsEmpty) return cargoPlan;

            // if need to import only dg info - then not required to update dg info
            if (openOption == OpenFile.OpenOption.Import)
            {
                existingCargoPlan.ImportDgData(cargoPlan, importOnlySelected, currentPort);
                return existingCargoPlan;
            }

            //Updating cargo plan from database
            HandleDgList.UpdateDgInfo(cargoPlan.DgList, OpenFile.DefineFileType(fileName));
            HandleDgList.CheckDgList(cargoPlan.DgList, (byte)OpenFile.FileTypes.Edi);

            //Choose what to do with new plan according to OpenOptions
            if (openOption == OpenFile.OpenOption.Update)
                cargoPlan = existingCargoPlan.UpdateCargoPlan(cargoPlan);

            //result
            return cargoPlan;
        }

        /// <summary>
        /// Updates Reefers with manifest info from excel file
        /// </summary>
        /// <param name="fileName">Excel file with manifest info - path.</param>
        /// <returns>True if imported and updated successfully.</returns>
        public static bool ImportReeferManifestInfoFromExcel(this CargoPlan cargoPlan, string fileName, bool importOnlySelected = false, string currentPort = null)
        {
            List<Container> tempList = new List<Container>();

            cargoPlan.ClearAllReefersHasUpdated();

            if (!tempList.ImportReeferInfoFromExcel(fileName)) return false;

            cargoPlan.Reefers.UpdateReeferManifestInfo(tempList, importOnlySelected, currentPort);

            return true;
        }


        // ----- Private methods -----

        /// <summary>
        /// Updates existing (this) CargoPlan Dg data from given CargoPlan
        /// </summary>
        /// <param name="cargoPlanToImportFrom">CargoPlan from which data to be taken for update.</param>
        /// <param name="importOnlySelected">Sets if required to import only selected items.</param>
        /// <param name="currentPol">If not null - only current POL units will be updated.</param>
        /// <returns>Returns new CargoPlan based on existing plan with updated info from given one.</returns>
        private static void ImportDgData(this CargoPlan existingCargoPlan, CargoPlan cargoPlanToImportFrom, bool importOnlySelected = false, string currentPol = null)
        {
            existingCargoPlan.ClearAllHasUpdated();

            string tempContainerNumber = "";
            bool containerNumberChanged;
            bool notToImport = false;

            //Equivalent container in existing plan
            Container container = null;
            //List of all Dg existing originally in a container
            List<Dg> allUpdatingDgTempList = new List<Dg>();
            //List of Dg existing in original plan with container number matching currently importing dg
            List<Dg> existingDgInContainer = new List<Dg>();
            List<string> containerNumbersToImportOnly = new List<string>();
            List<string> containerNumbersCurrentPOL = new List<string>();

            //creating list of selected items for import
            if (importOnlySelected)
            {
                containerNumbersToImportOnly = existingCargoPlan.Containers
                    .Where(c => c.IsToImport)
                    .Select(c => c.ContainerNumber).ToList();
            }
            //creating list of container numbers with current POL
            if (!string.IsNullOrEmpty(currentPol)) 
            {
                containerNumbersCurrentPOL = existingCargoPlan.Containers.
                        Where(c => c.POL == currentPol)
                        .Select(c => c.ContainerNumber).ToList();
            }

            //commence import from cargoPlanToImportFrom
            foreach (var dgToImport in cargoPlanToImportFrom.DgList)
            {
                //if only import for current POL
                if (!string.IsNullOrEmpty(currentPol))
                {
                    if (!containerNumbersCurrentPOL.Contains(dgToImport.ContainerNumber)) 
                        continue;
                }

                //if only import selected
                if (importOnlySelected)
                {
                    if (!containerNumbersToImportOnly.Contains(dgToImport.ContainerNumber))
                        continue;
                }

                //find out if dg is still in the same container with the previous dg imported
                containerNumberChanged = tempContainerNumber != dgToImport.ContainerNumber;
                tempContainerNumber = dgToImport.ContainerNumber;

                //creating list of all dg in the same new container
                if (containerNumberChanged)
                {
                    existingDgInContainer = existingCargoPlan.DgList.Where(d => d.ContainerNumber == tempContainerNumber).ToList();
                    allUpdatingDgTempList.AddRange(existingDgInContainer);
                    notToImport = false;

                    //find container in plan
                    container = existingCargoPlan.Containers.FirstOrDefault(c => c.ContainerNumber == tempContainerNumber);
                    if (container == null)
                        continue;
                    if (container.IsNotToImport)
                    {
                        notToImport = true;
                        continue;
                    }


                    //updating container info
                    if (!container.HasUpdated)
                    {
                        //check if POD changed
                        if (!UserSettings.DoNotImportPOD &&
                            !string.IsNullOrEmpty(dgToImport.POD) && 
                            !string.Equals(container.POD, dgToImport.POD))
                        {
                            container.POD = dgToImport.POD;
                            container.HasPodChanged = true;
                        }
                        else container.HasPodChanged = false;

                        //check if container type changed
                        if (!UserSettings.DoNotImportContainerType &&
                            !string.IsNullOrEmpty(dgToImport.ContainerType) && 
                            !string.Equals(container.ContainerType, dgToImport.ContainerType))
                        {
                            container.ContainerType = dgToImport.ContainerType;
                            container.HasContainerTypeChanged = true;
                        }
                        else container.HasContainerTypeChanged = false;

                        container.HasUpdated = true;
                    }
                }

                //if not to import -> next one
                if (notToImport || container == null) continue;

                dgToImport.CopyUpdatedTypeAndPODInfo(container);

                //from same container selecting dg with the same unno as dgToImport and not updated yet.
                List<Dg> sameUnnoTempDgList = existingDgInContainer.Where(d => d.Unno == dgToImport.Unno && !d.HasUpdated).ToList();
                int indexInDgList = 0;

                //decide which dg from the list to update
                switch (sameUnnoTempDgList.Count)
                {
                    case 0:
                        //create new dg
                        dgToImport.CopyContainerAbstractInfo(container);
                        dgToImport.IsNewUnitInPlan = true;
                        dgToImport.UpdateMissingDgInfo();
                        existingCargoPlan.DgList.Insert(
                            existingCargoPlan.DgList.FindLastIndex(d => d.ContainerNumber == dgToImport.ContainerNumber) + 1,
                            dgToImport);
                        container.DgCountInContainer++;
                        break;

                    case 1:
                        //import data to the only dg
                        allUpdatingDgTempList.Remove(sameUnnoTempDgList[0]); //clearing for further comparing of unupdated items
                        indexInDgList = existingCargoPlan.DgList.FindIndex(x => x == sameUnnoTempDgList[0]);
                        existingCargoPlan.DgList[indexInDgList].CompleteDgImport(dgToImport);
                        sameUnnoTempDgList[0].HasUpdated = true;
                        break;

                    default:
                        //select the most appropriate one
                        byte positionWithMaxMatch = GetMostMatchingDgFromList(dgToImport, sameUnnoTempDgList);

                        //import data to selected dg
                        allUpdatingDgTempList.Remove(sameUnnoTempDgList[positionWithMaxMatch]); //clearing for further comparing of unupdated items
                        indexInDgList = existingCargoPlan.DgList.FindIndex(x => x == sameUnnoTempDgList[positionWithMaxMatch]);
                        existingCargoPlan.DgList[indexInDgList].CompleteDgImport(dgToImport);
                        sameUnnoTempDgList[positionWithMaxMatch].HasUpdated = true;
                        break;
                }
            }

            //remove unupdated dg items
            foreach (var dg in allUpdatingDgTempList)
            {
                container = existingCargoPlan.Containers.SingleOrDefault(c => c.ContainerNumber == dg.ContainerNumber);
                if (container != null && container.IsNotToImport) continue;
                existingCargoPlan.DgList.Remove(dg);
                if (container != null) container.DgCountInContainer--;
            }
            Data.LogWriter.Write($"Dg data successfully imported to existing CargoPlan.");
        }


        /// <summary>
        /// When there are several Dgs with the same UNNo, to choose one which matches best the updating Dg
        /// </summary>
        /// <param name="dgToImport">Updating dg that defines parameters to choose.</param>
        /// <param name="sameUnnoTempDgList">List to search in.</param>
        /// <returns>Index of the most matching Dg in the sameUnnoTempDgList.</returns>
        private static byte GetMostMatchingDgFromList(Dg dgToImport, List<Dg> sameUnnoTempDgList)
        {
            byte positionWithMaxMatch = 0;
            byte lastMaxMatch = 0;
            for (byte i = 0; i < sameUnnoTempDgList.Count; i++)
            {
                byte count = 0;
                if (sameUnnoTempDgList[i].PackingGroup == dgToImport.PackingGroup)
                    count += 3;
                if (Math.Abs(sameUnnoTempDgList[i].DgNetWeight - dgToImport.DgNetWeight) < 0.001M)
                    count += 4;
                else
                {
                    if (Math.Abs(sameUnnoTempDgList[i].DgNetWeight) < 0.001M)
                        count += 2;
                }
                if (sameUnnoTempDgList[i].FlashPointAsDecimal == dgToImport.FlashPointAsDecimal)
                    count += 1;

                if (count > lastMaxMatch)
                {
                    lastMaxMatch = count;
                    positionWithMaxMatch = i;
                }
                if (lastMaxMatch == 8) break;
            }

            return positionWithMaxMatch;
        }

        /// <summary>
        /// Compares present and new CargoPlan.
        /// Items existing in both plans transferred from original to result without changes, but with changed location (if not locked).
        /// New items from the new plan (if not found in original) transferred from new plan to result.
        /// Items marked IsToBeKeptInPlan will be transferred with no change to result if not existing in newPlan.
        /// </summary>
        /// <param name="newPlan">CargoPlan with which the existing one will be compared to</param>
        /// <returns>New resulting CargoPlan</returns>
        private static CargoPlan UpdateCargoPlan(this CargoPlan cargoPlan, CargoPlan newPlan)
        {
            //TODO: To be tested properly!

            cargoPlan.ClearAllHasUpdated();
            cargoPlan.ClearUpdates();

            var resultingNewCargoPlan = new CargoPlan();
            var existingCargoPlan = cargoPlan.CopyCargoPlan();

            List<string> originalContainerNumberList = cargoPlan.Containers.Select(c => c.ContainerNumber).ToList();

            //Creating list and dealing with containers selected to be kept in plan
            List<Container> containersToBeKeptInPlan = existingCargoPlan.Containers.Where(c => c.IsToBeKeptInPlan && !c.HasNoNumber).ToList();

            //Main updating operation
            foreach (var newContainer in newPlan.Containers)
            {
                //check for existing unit (only units with proper number)
                bool unitFound = !newContainer.HasNoNumber && originalContainerNumberList.Contains(newContainer.ContainerNumber);
                bool unitLocationOccupied = false;

                //if unit is found
                if (unitFound)
                    for (int i = 0; i < existingCargoPlan.Containers.Count; i++)
                    {
                        var existingContainer = existingCargoPlan.Containers.ElementAt(i);

                        //check if units ContainerNumbers match
                        if (!string.Equals(existingContainer.ContainerNumber, newContainer.ContainerNumber)) continue;

                        ClearUnitUpdates(existingContainer);
                        if (existingContainer.IsToBeKeptInPlan)
                            containersToBeKeptInPlan.Remove(existingContainer);

                        //check for changed location
                        existingContainer.HasLocationChanged = !string.Equals(newContainer.Location, existingContainer.Location) &&
                                               !existingContainer.IsPositionLockedForChange;
                        if (existingContainer.HasLocationChanged)
                        {
                            existingContainer.LocationBeforeRestow = existingContainer.Location;
                            existingContainer.Location = newContainer.Location;
                        }

                        //check for other changes
                        if (!string.Equals(newContainer.POD, existingContainer.POD))
                        {
                            existingContainer.HasPodChanged = true;
                            existingContainer.POD = newContainer.POD;
                        }
                        if (!string.Equals(newContainer.ContainerType, existingContainer.ContainerType))
                        {
                            existingContainer.HasContainerTypeChanged = true;
                            existingContainer.ContainerType = newContainer.ContainerType;
                        }

                        //add unit to the plan
                        existingContainer.IsNewUnitInPlan = false;
                        ShiftContainerToResultingCargoPlan(existingCargoPlan, existingContainer, resultingNewCargoPlan, false, newPlan);
                        existingCargoPlan.Containers.Remove(existingContainer);
                        originalContainerNumberList.Remove(existingContainer.ContainerNumber);

                        break;
                    }

                // if no number
                else if (existingCargoPlan.HasNonamers && newContainer.HasNoNumber)
                {
                    for (int i = 0; i < existingCargoPlan.Containers.Count; i++)
                    {
                        var existingContainer = existingCargoPlan.Containers.ElementAt(i);

                        //check if units ContainerNumbers match
                        if (!string.Equals(existingContainer.Location, newContainer.Location)) continue;
                        unitLocationOccupied = true;

                        if (existingContainer.HasNoNumber)
                        {
                            //check for changes
                            if (!string.Equals(newContainer.POD, existingContainer.POD))
                            {
                                newContainer.HasPodChanged = true;
                            }
                            if (!string.Equals(newContainer.ContainerType, existingContainer.ContainerType))
                            {
                                newContainer.HasContainerTypeChanged = true;
                            }

                            //add unit to the plan
                            newContainer.IsNewUnitInPlan = false;
                            ShiftContainerToResultingCargoPlan(existingCargoPlan, newContainer, resultingNewCargoPlan, false, newPlan);
                            existingCargoPlan.Containers.Remove(existingContainer);
                            originalContainerNumberList.Remove(existingContainer.ContainerNumber);
                        }
                        else
                        {
                            // If existing container has a ContainerNumber it will remain in existingPlan until the end of cycle.
                            // Only the newContainer will be added in the same location.
                            newContainer.IsNewUnitInPlan = true;
                            ShiftContainerToResultingCargoPlan(newPlan, newContainer, resultingNewCargoPlan);
                        }
                        break;
                    }
                }

                //if not found - add new item
                if (!unitFound && !unitLocationOccupied)
                {
                    ShiftContainerToResultingCargoPlan(sourceCargoPlan: newPlan, container: newContainer, resultingCargoPlan: resultingNewCargoPlan);
                }
            }

            //Remaining containers to be kept in plan
            foreach (var container in containersToBeKeptInPlan)
            {
                ShiftContainerToResultingCargoPlan(existingCargoPlan, container, resultingNewCargoPlan, containerIsNew: false);
            }

            //Discharged containers
            foreach(var container in existingCargoPlan.Containers)
            {
                resultingNewCargoPlan.AddToDischarged(container);
            }

            //voyage
            resultingNewCargoPlan.VoyageInfo.VoyageNumber = newPlan.VoyageInfo.VoyageNumber;
            resultingNewCargoPlan.VoyageInfo.PortOfDeparture = newPlan.VoyageInfo.PortOfDeparture;
            resultingNewCargoPlan.VoyageInfo.PortOfDestination = newPlan.VoyageInfo.PortOfDestination;

            //updates
            resultingNewCargoPlan.Updates.HasPOLChanged = 
                newPlan.VoyageInfo.PortOfDeparture != cargoPlan.VoyageInfo.PortOfDeparture;

            Data.LogWriter.Write($"Cargo plan successfully updated.");
            return resultingNewCargoPlan;
        }



        // -------------- Supporting methods ----------------------------------------

        /// <summary>
        /// Sets HasUpdated property of all items in CargoPlan to false;
        /// </summary>
        private static void ClearAllHasUpdated(this CargoPlan cargoPlan)
        {
            foreach (var container in cargoPlan.Containers)
            {
                container.HasUpdated = false;
            }

            foreach (var dg in cargoPlan.DgList)
            {
                dg.HasUpdated = false;
            }
        }

        /// <summary>
        /// Sets HasUpdated property of all containers in <see cref="Reefers"/> to false.
        /// </summary>
        private static void ClearAllReefersHasUpdated(this CargoPlan cargoPlan)
        {
            foreach (var reefer in cargoPlan.Reefers)
            {
                reefer.HasUpdated = false;
            }
        }

        /// <summary>
        /// Resets all IUpdatable properties of the unit to false or null.
        /// </summary>
        /// <param name="unit">CargoPlan unit to be reset.</param>
        private static void ClearUnitUpdates(IUpdatable unit)
        {
            unit.HasUpdated = false;
            unit.HasLocationChanged = false;
            unit.LocationBeforeRestow = string.Empty;
            unit.HasPodChanged = false;
            unit.HasContainerTypeChanged = false;
        }

        /// <summary>
        /// Copies all required for the purpose of import information from dgToImport to dgToUpdate
        /// </summary>
        /// <param name="dgToUpdate"></param>
        /// <param name="dgToImport"></param>
        private static void CompleteDgImport(this Dg dgToUpdate, Dg dgToImport)
        {
            dgToUpdate.ImportDgInfo(dgToImport);
            dgToUpdate.CopyUpdatedTypeAndPODInfo(dgToImport);
        }

        /// <summary>
        /// Adds a container to CargoPlan. Adds to resulting and removes from source Reefers and Dg lists, if required.
        /// Does not remove container from sourceCargoPlan only. 
        /// </summary>
        /// <param name="sourceCargoPlan">From which plan container needs to be shifted</param>
        /// <param name="container">Container to be shifted</param>
        /// <param name="resultingCargoPlan">To which plan container needs to be added</param>
        /// <param name="containerIsNew">Shall container be marked as IsNewUnitInPlan</param>
        /// <param name="additionalCargoPlanToBeCleared"></param>
        private static void ShiftContainerToResultingCargoPlan(CargoPlan sourceCargoPlan, Container container, CargoPlan resultingCargoPlan,
                                                              bool containerIsNew = true, CargoPlan additionalCargoPlanToBeCleared = null)
        {
            //Add new Container
            container.IsNewUnitInPlan = containerIsNew;
            resultingCargoPlan.Containers.Add(container);
            if(containerIsNew)
                resultingCargoPlan.AddToLoaded(container);

            if (container.IsRf)
            {
                resultingCargoPlan.Reefers.Add(container);
                sourceCargoPlan.Reefers.Remove(container);

                additionalCargoPlanToBeCleared?.Reefers.Remove(additionalCargoPlanToBeCleared.Reefers.SingleOrDefault
                    (r => string.Equals(r.ContainerNumber, container.ContainerNumber)));
            }

            //Add new Dg
            if (container.DgCountInContainer == 0) return;
            List<Dg> newContainerDgList =
                sourceCargoPlan.DgList.Where(dg => dg.ContainerNumber == container.ContainerNumber).ToList();
            foreach (var dg in newContainerDgList)
            {
                dg.CopyIUpdatableToDg(container);
                dg.CopyUpdatedTypeAndPODInfo(container);
                resultingCargoPlan.DgList.Add(dg);
                sourceCargoPlan.DgList.Remove(dg);
                additionalCargoPlanToBeCleared?.DgList.Remove(dg);
            }
            //clear all possible dg remaining
            if (additionalCargoPlanToBeCleared == null) return;
            bool stillFound =
                additionalCargoPlanToBeCleared.DgList.Any(d => string.Equals(d.ContainerNumber, container.ContainerNumber));
            if (!stillFound) return;
            for (int i = 0; i < additionalCargoPlanToBeCleared.DgList.Count; i++)
            {
                var dg = additionalCargoPlanToBeCleared.DgList.ElementAt(i);
                if (!string.Equals(dg.ContainerNumber, container.ContainerNumber)) continue;
                additionalCargoPlanToBeCleared.DgList.Remove(dg);
                i--;
            }
        }

        /// <summary>
        /// Creates a new cargo plan and fills its lists with units from source cargo plan
        /// </summary>
        /// <returns>New CargoPlan with copied lists</returns>
        private static CargoPlan CopyCargoPlan(this CargoPlan cargoPlan)
        {
            var returnPlan = new CargoPlan();
            foreach (var dg in cargoPlan.DgList)
            {
                returnPlan.DgList.Add(dg);
            }

            foreach (var container in cargoPlan.Containers)
            {
                returnPlan.Containers.Add(container);
            }

            foreach (var reefer in cargoPlan.Reefers)
            {
                returnPlan.Reefers.Add(reefer);
            }

            returnPlan.VoyageInfo = cargoPlan.VoyageInfo;

            return returnPlan;
        }
    }
}
