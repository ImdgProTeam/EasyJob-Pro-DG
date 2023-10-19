using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.Model.IO.Excel;
using EasyJob_ProDG.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static EasyJob_ProDG.Model.IO.Excel.WithXlReefers;

namespace EasyJob_ProDG.Model.Cargo
{
    public class CargoPlan : ICargoPlan
    {
        public List<Dg> DgList { get; set; }
        public ICollection<Container> Containers { get; set; }
        public ICollection<Container> Reefers { get; set; }
        public Voyage VoyageInfo { get; set; }

        public decimal TotalDgNetWeight
        {
            get
            {
                decimal sum = 0M;
                foreach (var dg in DgList)
                {
                    sum += dg.DgNetWeight;
                }
                return sum;
            }
        }
        public decimal TotalMPNetWeight
        {
            get
            {
                decimal sum = 0M;
                foreach (var dg in DgList)
                {
                    if (dg.IsMp)
                        sum += dg.DgNetWeight;
                }
                return sum;
            }
        }
        public bool IsEmpty => Containers.Count <= 0 && DgList.Count <= 0 && Reefers.Count <= 0;

        /// <summary>
        /// True if <see cref="CargoPlan"/> contains <see cref="Container"/>s without numbers.
        /// </summary>
        internal bool HasNonamers => Containers.Any(c => c.HasNoNumber);
        internal int NextNonamerNumber;


        // -------------- Main methods ----------------------------------------------

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
        public CargoPlan CreateCargoPlan(string fileName, Transport.ShipProfile ownShip,
            XDocument dgDataBase, OpenFile.OpenOption openOption = OpenFile.OpenOption.Open, CargoPlan existingCargoPlan = null,
            bool importOnlySelected = false, string currentPort = null)
        {
            //creating cargo plan from file
            var cargoPlan = OpenFile.ReadCargoPlanFromFile(fileName, ownShip);
            if (cargoPlan is null || cargoPlan.IsEmpty) return cargoPlan;

            //Updating cargo plan from database
            HandleDg.UpdateDgInfo(cargoPlan.DgList, dgDataBase);
            HandleDgList.CheckDgList(cargoPlan.DgList, (byte)OpenFile.FileTypes.Edi);

            //Choose what to do with new plan according to OpenOptions
            if (openOption == OpenFile.OpenOption.Update)
                cargoPlan = existingCargoPlan.UpdateCargoPlan(cargoPlan);
            else if (openOption == OpenFile.OpenOption.Import)
            {
                existingCargoPlan.ImportDgData(cargoPlan, importOnlySelected, currentPort);
                cargoPlan = existingCargoPlan;
            }

            //result
            return cargoPlan;
        }

        /// <summary>
        /// Updates Reefers with manifest info from excel file
        /// </summary>
        /// <param name="fileName">Excel file with manifest info - path.</param>
        /// <returns>True if imported and updated successfully.</returns>
        public bool ImportReeferManifestInfoFromExcel(string fileName, bool importOnlySelected = false, string currentPort = null)
        {
            List<Container> tempList = new List<Container>();

            if (!tempList.ImportReeferInfoFromExcel(fileName)) return false;

            Reefers.UpdateReeferManifestInfo(tempList, importOnlySelected, currentPort);

            return true;
        }

        /// <summary>
        /// Updates existing (this) CargoPlan Dg data from given CargoPlan
        /// </summary>
        /// <param name="cargoPlan">CargoPlan from which data to be taken for update.</param>
        /// <param name="importOnlySelected">Sets if required to import only selected items.</param>
        /// <param name="currentPol">If not null - only current POL units will be updated.</param>
        /// <returns>Returns new CargoPlan based on existing plan with updated info from given one.</returns>
        private void ImportDgData(CargoPlan cargoPlan, bool importOnlySelected = false, string currentPol = null)
        {
            //TODO: To be tested properly, especially with not iftdgn files.

            ClearAllHasUpdated();

            string tempContainerNumber = "";
            bool containerNumberChanged;
            bool notToImport = false;

            Container container = null;
            //List of all Dg existing in original plan
            List<Dg> allUpdatingDgTempList = new List<Dg>();
            //List of Dg existing in original plan with container number matching currently importing dg
            List<Dg> existingDgInContainer = new List<Dg>();
            List<string> containerNumbersToImportOnly = new List<string>();

            //creating list of selected items for import
            if (importOnlySelected)
            {
                containerNumbersToImportOnly = (Containers.Where(c => c.IsToImport)).Select(c => c.ContainerNumber).ToList();
            }

            //commence import from cargoPlan
            foreach (var dgToImport in cargoPlan.DgList)
            {
                //if only import for current POL
                if (!string.IsNullOrEmpty(currentPol))
                {
                    if (dgToImport.POL != currentPol) continue;
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
                    existingDgInContainer = DgList.Where(d => d.ContainerNumber == tempContainerNumber).ToList();
                    allUpdatingDgTempList.AddRange(existingDgInContainer);
                    notToImport = false;

                    //find container in plan
                    container = Containers.FirstOrDefault(c => c.ContainerNumber == tempContainerNumber);
                    if (container == null) continue;
                    if (container.IsNotToImport)
                    {
                        notToImport = true;
                        continue;
                    }

                    //updating container info
                    if (!container.HasUpdated)
                    {
                        //check if POD changed
                        if (!string.IsNullOrEmpty(dgToImport.POD) && !string.Equals(container.POD, dgToImport.POD))
                        {
                            container.POD = dgToImport.POD;
                            container.HasPodChanged = true;
                        }
                        else container.HasPodChanged = false;

                        //TODO: Optional import of container type from IFTDGN
                        //check if container type changed
                        if (!string.IsNullOrEmpty(dgToImport.ContainerType) && !string.Equals(container.ContainerType, dgToImport.ContainerType))
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

                //copy container info to importing dg
                dgToImport.CopyContainerInfo(container);

                //from same container selecting dg with the same unno as dgToImport and not updated yet.
                List<Dg> sameUnnoTempDgList = existingDgInContainer.Where(d => d.Unno == dgToImport.Unno && !d.HasUpdated).ToList();
                int indexInDgList = 0;

                //decide which dg from the list to update
                switch (sameUnnoTempDgList.Count)
                {
                    case 0:
                        //create new dg
                        dgToImport.CopyContainerInfo(container);
                        dgToImport.IsNewUnitInPlan = true;
                        DgList.Add(dgToImport);
                        container.DgCountInContainer++;
                        break;

                    case 1:
                        //import data to the only dg
                        allUpdatingDgTempList.Remove(sameUnnoTempDgList[0]); //clearing for further comparing of unupdated items
                        dgToImport.CopyNonImportableInfo(sameUnnoTempDgList[0]);
                        indexInDgList = DgList.FindIndex(x => x == sameUnnoTempDgList[0]);
                        DgList[indexInDgList] = dgToImport;
                        sameUnnoTempDgList[0].HasUpdated = true;
                        break;

                    default:
                        //select the most appropriate one
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
                            if (Math.Abs(sameUnnoTempDgList[i].FlashPointDouble - dgToImport.FlashPointDouble) < 0.1)
                                count += 1;

                            if (count > lastMaxMatch)
                            {
                                lastMaxMatch = count;
                                positionWithMaxMatch = i;
                            }
                            if (lastMaxMatch == 8) break;
                        }

                        //import data to selected dg
                        allUpdatingDgTempList.Remove(sameUnnoTempDgList[positionWithMaxMatch]); //clearing for further comparing of unupdated items
                        dgToImport.CopyNonImportableInfo(sameUnnoTempDgList[positionWithMaxMatch]);
                        indexInDgList = DgList.FindIndex(x => x == sameUnnoTempDgList[positionWithMaxMatch]);
                        DgList[indexInDgList] = dgToImport;
                        sameUnnoTempDgList[positionWithMaxMatch].HasUpdated = true;
                        break;
                }
            }

            //remove unupdated dg items
            foreach (var dg in allUpdatingDgTempList)
            {
                container = Containers.SingleOrDefault(c => c.ContainerNumber == dg.ContainerNumber);
                if (container != null && container.IsNotToImport) continue;
                DgList.Remove(dg);
                if (container != null) container.DgCountInContainer--;
            }
            Data.LogWriter.Write($"Dg data successfully imported to existing CargoPlan.");
        }

        /// <summary>
        /// Compares present and new CargoPlan.
        /// Items existing in both plans transferred from original to result without changes, but with changed location (if not locked).
        /// New items from the new plan (if not found in original) transferred from new plan to result.
        /// Items marked IsToBeKeptInPlan will be transferred with no change to result if not existing in newPlan.
        /// </summary>
        /// <param name="newPlan">CargoPlan with which the existing one will be compared to</param>
        /// <returns>New resulting CargoPlan</returns>
        private CargoPlan UpdateCargoPlan(CargoPlan newPlan)
        {
            //TODO: To be tested properly!

            ClearAllHasUpdated();

            var resultingNewCargoPlan = new CargoPlan();
            var existingCargoPlan = CopyCargoPlan();

            List<string> originalContainerNumberList = Containers.Select(c => c.ContainerNumber).ToList();

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
                                newContainer.POD = newContainer.POD;
                            }
                            if (!string.Equals(newContainer.ContainerType, existingContainer.ContainerType))
                            {
                                newContainer.HasContainerTypeChanged = true;
                                newContainer.ContainerType = newContainer.ContainerType;
                            }

                            //add unit to the plan
                            newContainer.IsNewUnitInPlan = false;
                            ShiftContainerToResultingCargoPlan(existingCargoPlan, newContainer, resultingNewCargoPlan, false, newPlan);
                            existingCargoPlan.Containers.Remove(existingContainer);
                            originalContainerNumberList.Remove(existingContainer.ContainerNumber);
                            i--;
                        }
                        else
                        {
                            // If existing container has a ContainerNumber it will remain in existingPlan until the end of cycle.
                            // Only the newContainer will be added in the same location.
                            ShiftContainerToResultingCargoPlan(newPlan, newContainer, resultingNewCargoPlan);
                        }
                        break;
                    }
                }

                //if not found - add new item
                if (!unitFound && !unitLocationOccupied)
                { 
                    ShiftContainerToResultingCargoPlan(newPlan, newContainer, resultingNewCargoPlan);
                }
            }

            //Remaining containers to be kept in plan
            foreach (var container in containersToBeKeptInPlan)
            {
                ShiftContainerToResultingCargoPlan(existingCargoPlan, container, resultingNewCargoPlan, false);
            }

            //voyage
            resultingNewCargoPlan.VoyageInfo.VoyageNumber = newPlan.VoyageInfo.VoyageNumber;
            resultingNewCargoPlan.VoyageInfo.PortOfDeparture = newPlan.VoyageInfo.PortOfDeparture;
            resultingNewCargoPlan.VoyageInfo.PortOfDestination = newPlan.VoyageInfo.PortOfDestination;

            Data.LogWriter.Write($"Cargo plan successfully updated.");
            return resultingNewCargoPlan;
        }

        #region Add/Remove methods
        //----------- Add/Remove methods -------------------------------------------------------------------

        /// <summary>
        /// Adds new Dg to CargoPlan
        /// </summary>
        /// <param name="dg">New dg to be added to plan</param>
        public void AddDg(Dg dg, XDocument dgDataBase)
        {
            if (dg == null || string.IsNullOrEmpty(dg.ContainerNumber)) return;

            var container = Containers.FindContainerByContainerNumber(dg);
            if (container is null)
            {
                container = (Container)dg;
                Containers.Add(container);
                if (dg.IsRf) Reefers.Add(container);
            }
            else
            {
                dg.CopyContainerInfo(container);
            }
            dg.UpdateDgInfo(dgDataBase);
            DgList.Add(dg);
            container.DgCountInContainer++;
        }

        /// <summary>
        /// Adds new container to CargoPlan
        /// </summary>
        /// <param name="container">Container to add to the plan. Container number shall be unique.</param>
        /// <returns>True if container succesfully added to CargoPlan</returns>
        public bool AddContainer(Container container)
        {
            #region Safety checks
            if (container is null) return false;
            if (string.IsNullOrEmpty(container.ContainerNumber))
            {
                Data.LogWriter.Write($"Attempt to add a container with no container number");
                return false;
            }
            if (Containers.ContainsUnitWithSameContainerNumberInList(container))
            {
                Data.LogWriter.Write($"Attempt to add a container with container number which is already in list");
                return false;
            }
            #endregion

            Containers.Add(container);
            if (container.IsRf) Reefers.Add(container);
            return true;
        }

        /// <summary>
        /// Adds new reefer to CargoPlan
        /// </summary>
        /// <param name="container">Reefer container to be added. Container number shall be unique.</param>
        public bool AddReefer(Container reefer)
        {
            #region Safety checks
            if (reefer is null) return false;
            if (string.IsNullOrEmpty(reefer.ContainerNumber))
            {
                Data.LogWriter.Write($"Attempt to add a reefer with no container number");
                return false;
            }
            #endregion

            if (Containers.ContainsUnitWithSameContainerNumberInList(reefer))
            {
                Data.LogWriter.Write($"Attempt to add a reefer with container number which is already in list");
                var container = Containers.FindContainerByContainerNumber(reefer) ?? throw new Exception($"Container with ContainerNumber {reefer.ContainerNumber} cannot be found in CargoPlan despite it was expected.");
                container.IsRf = true;
                reefer.CopyContainerInfo(container);
            }
            else
            {
                reefer.IsRf = true;
                Containers.Add(reefer);
            }
            Reefers.Add(reefer);
            return true;
        }

        #endregion


        // -------------- Supporting methods ----------------------------------------

        /// <summary>
        /// Sets HasUpdated property of all items in CargoPlan to false;
        /// </summary>
        private void ClearAllHasUpdated()
        {
            foreach (var container in Containers)
            {
                container.HasUpdated = false;
            }

            foreach (var dg in DgList)
            {
                dg.HasUpdated = false;
            }
        }

        /// <summary>
        /// Resets all IUpdatable properties of the unit to false or null.
        /// </summary>
        /// <param name="unit">CargoPlan unit to be reset.</param>
        private void ClearUnitUpdates(IUpdatable unit)
        {
            unit.HasUpdated = false;
            unit.HasLocationChanged = false;
            unit.LocationBeforeRestow = string.Empty;
            unit.HasPodChanged = false;
            unit.HasContainerTypeChanged = false;
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
                CopyIUpdatableToDg(container, dg);
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
        /// Copies IUpdatable properties to selected dg
        /// </summary>
        /// <param name="fromContainer">From a Container</param>
        /// <param name="toDg">To Dg</param>
        private static void CopyIUpdatableToDg(Container fromContainer, Dg toDg)
        {
            toDg.IsNewUnitInPlan = fromContainer.IsNewUnitInPlan;
            toDg.HasLocationChanged = fromContainer.HasLocationChanged;
            if (toDg.HasLocationChanged)
            {
                toDg.Location = fromContainer.Location;
            }
            toDg.LocationBeforeRestow = fromContainer.LocationBeforeRestow;
        }

        /// <summary>
        /// Creates a new cargo plan and fills its lists with units from source cargo plan
        /// </summary>
        /// <returns>New CargoPlan with copied lists</returns>
        private CargoPlan CopyCargoPlan()
        {
            var returnPlan = new CargoPlan();
            foreach (var dg in DgList)
            {
                returnPlan.DgList.Add(dg);
            }

            foreach (var container in Containers)
            {
                returnPlan.Containers.Add(container);
            }

            foreach (var reefer in Reefers)
            {
                returnPlan.Reefers.Add(reefer);
            }

            returnPlan.VoyageInfo = VoyageInfo;
            //returnPlan.HasNonamers = HasNonamers;

            return returnPlan;
        }

        /// <summary>
        /// Clears all dg conflicts in DgList
        /// </summary>
        public void ClearConflicts()
        {
            foreach (var dg in DgList)
                dg.ClearAllConflicts();
        }



        // -------------- Constructors ----------------------------------------------

        /// <summary>
        /// Creates CargoPlan with initiated blank Lists.
        /// </summary>
        public CargoPlan()
        {
            DgList = new List<Dg>();
            Reefers = new List<Container>();
            Containers = new List<Container>();
            VoyageInfo = new Voyage();
            //HasNonamers = false;
        }
    }
}
