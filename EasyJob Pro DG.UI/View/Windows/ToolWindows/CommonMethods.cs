using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyJob_ProDG.UI.View.Windows.ToolWindows
{
    internal static class ToolWindowsCommonMethods
    {
        /// <summary>
        /// Creates list of items IDs to assign to ItemsToSelect of DataGridDg.
        /// </summary>
        /// <returns></returns>
        internal static List<string> CreateSelectDgListIds(SelectionControlViewModel selection, CargoPlanWrapper cargoPlan)
        {
            var result = cargoPlan.DgList.ToList();

            // Container number
            if (!string.IsNullOrWhiteSpace(selection.SelectedNumber))
                result = result.Where(c => c.ContainerNumber.ContainsCaseAndSpaceInsensitive(selection.SelectedNumber)).ToList();

            // Position
            if (!selection.CellPosition.IsEmpty)
            {
                result = result.Where(c => selection.CellPosition.Model.Equals(c)).ToList();
            }
            // Old position
            if (!selection.CellOldPosition.IsEmpty)
            {
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.LocationBeforeRestow) && selection.CellOldPosition.Model.Equals(c.LocationBeforeRestow.ConvertToLocationOnBoard())).ToList();
            }

            // Underdeck
            if (selection.IsUnderdeck)
                result = result.Where(c => c.IsUnderdeck).ToList();
            else if (selection.IsOnDeck)
                result = result.Where(c => !c.IsUnderdeck).ToList();

            // Type of cargo
            if (selection.IsDG)
                result = result;
            else if (selection.IsNotDg)
                return new List<string>();

            if (selection.IsReefer)
                result = result.Where(c => c.IsRf).ToList();
            else if (selection.IsNonReefer)
                result = result.Where(c => !c.IsRf).ToList();

            // Container properties
            if (!string.IsNullOrWhiteSpace(selection.SelectedPOL))
                result = result.Where(c => c.POL.ContainsCaseAndSpaceInsensitive(selection.SelectedPOL)).ToList();
            if (!string.IsNullOrWhiteSpace(selection.SelectedPOD))
                result = result.Where(c => c.POD.ContainsCaseAndSpaceInsensitive(selection.SelectedPOD)).ToList();
            if (!string.IsNullOrWhiteSpace(selection.SelectedFinalDestination))
                result = result.Where(c => c.FinalDestination.ContainsCaseAndSpaceInsensitive(selection.SelectedFinalDestination)).ToList();
            if (!string.IsNullOrWhiteSpace(selection.SelectedContainerType))
                result = result.Where(c => c.ContainerType.ContainsCaseAndSpaceInsensitive(selection.SelectedContainerType)).ToList();
            if (!string.IsNullOrWhiteSpace(selection.SelectedOperator))
                result = result.Where(c => c.Carrier.ContainsCaseAndSpaceInsensitive(selection.SelectedOperator)).ToList();
            if (selection.IsOpenType)
                result = result.Where(c => c.IsOpen).ToList();

            if (selection.HasRemarks || selection.HasNoRemarks)
            {
                var containers = cargoPlan.Containers.ToList();
                if (selection.HasRemarks)
                    containers = containers.Where(c => !string.IsNullOrWhiteSpace(c.Remarks)).ToList();
                else
                    containers = containers.Where(c => string.IsNullOrWhiteSpace(c.Remarks)).ToList();
                result = result.Where(c => containers.Any(u => string.Equals(u.ContainerNumber, c.ContainerNumber))).ToList();
            }


            // Changes
            if (selection.IsNewInPlan)
                result = result.Where(c => c.IsNewUnitInPlan).ToList();
            else if (selection.NotIsNewInPlan)
                result = result.Where(c => !c.IsNewUnitInPlan).ToList();
            if (selection.HasPositionChanged)
                result = result.Where(c => c.HasLocationChanged).ToList();
            else if (selection.NotHasPositionChanged)
                result = result.Where(c => !c.HasLocationChanged).ToList();
            if (selection.HasUpdated)
                result = result.Where(c => c.HasUpdated).ToList();
            else if (selection.NotHasUpdated)
                result = result.Where(c => !c.HasUpdated).ToList();
            if (selection.IsLocked)
                result = result.Where(c => c.IsPositionLockedForChange).ToList();
            else if (selection.NotIsLocked)
                result = result.Where(c => !c.IsPositionLockedForChange).ToList();
            if (selection.IsToImport)
                result = result.Where(c => c.IsToImport).ToList();
            else if (selection.NotIsToImport)
                result = result.Where(c => !c.IsNotToImport).ToList();
            if (selection.HasPODchanged)
                result = result.Where(c => c.HasPodChanged).ToList();
            else if (selection.NotHasPODchanged)
                result = result.Where(c => !c.HasPodChanged).ToList();
            if (selection.HasTypeChanged)
                result = result.Where(c => c.HasContainerTypeChanged).ToList();
            else if (selection.NotHasTypeChanged)
                result = result.Where(c => !c.HasContainerTypeChanged).ToList();
            if (selection.IsToBeKeptInPlan)
                result = result.Where(c => c.IsToBeKeptInPlan).ToList();
            else if (selection.NotIsToBeKeptInPlan)
                result = result.Where(c => !c.IsToBeKeptInPlan).ToList();

            // Dg
            if (selection.SelectedUNNO.HasValue || selection.SelectedUNNOs.Count > 0)
            {
                var unnos = selection.SelectedUNNOs;
                if (selection.SelectedUNNO.HasValue) unnos.Add(selection.SelectedUNNO.Value);

                result = result.Where(c => unnos.Contains(c.Unno)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(selection.SelectedDgClass) || selection.SelectedDgClasses.Count > 0)
            {
                var classes = selection.SelectedDgClasses;
                if (!string.IsNullOrWhiteSpace(selection.SelectedDgClass)) classes.Add(selection.SelectedDgClass);

                result = result.Where(c => classes.Any(cl => c.AllDgClasses.ContainsCaseAndSpaceInsensitive(cl))).ToList();
            }

            // Flash point
            if (selection.FlashPoint.HasValue)
            {
                if (selection.IsFPlessthan)
                {
                    result = result.Where(dg => !dg.Model.FlashPointNotDefined && dg.Model.FlashPointAsDecimal < selection.FlashPoint.Value).ToList();
                }
                else if (selection.IsFPmorethan)
                {
                    result = result.Where(dg => !dg.Model.FlashPointNotDefined && dg.Model.FlashPointAsDecimal > selection.FlashPoint.Value).ToList();
                }
                else
                {
                    result = result.Where(dg => dg.Model.FlashPointAsDecimal == selection.FlashPoint.Value).ToList();
                }
            }

            // Conflicted and remarks
            if (selection.IsConflicted)
                result = result.Where(c => c.IsConflicted).ToList();
            else if (selection.NotIsConflicted)
                result = result.Where(c => !c.IsConflicted).ToList();
            if (selection.HasDgRemarks)
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.Remarks)).ToList();
            else if (selection.NotHasDgRemarks)
                result = result.Where(c => string.IsNullOrWhiteSpace(c.Remarks)).ToList();

            // Reefer
            if (selection.HasCommodity || selection.NotHasCommodity || selection.HasSpecial || selection.NotHasSpecial ||
                selection.HasReeferRemarks || selection.NotHasReeferRemarks || selection.SetPoint.HasValue)
            {
                var reefers = cargoPlan.Reefers.ToList();

                if (selection.HasCommodity)
                    reefers = reefers.Where(c => !string.IsNullOrWhiteSpace(c.Commodity)).ToList();
                else if (selection.NotHasCommodity)
                    reefers = reefers.Where(c => string.IsNullOrWhiteSpace(c.Commodity)).ToList();

                if (selection.HasSpecial)
                    reefers = reefers.Where(c => !string.IsNullOrWhiteSpace(c.ReeferSpecial)).ToList();
                else if (selection.NotHasSpecial)
                    reefers = reefers.Where(c => string.IsNullOrWhiteSpace(c.ReeferSpecial)).ToList();

                if (selection.HasReeferRemarks)
                    reefers = reefers.Where(c => !string.IsNullOrWhiteSpace(c.ReeferRemark)).ToList();
                else if (selection.NotHasReeferRemarks)
                    reefers = reefers.Where(c => string.IsNullOrWhiteSpace(c.ReeferRemark)).ToList();

                if (selection.SetPoint.HasValue)
                {
                    if (selection.IsSPlessthan)
                    {
                        reefers = reefers.Where(r => r.SetTemperature < selection.SetPoint.Value).ToList();
                    }
                    else if (selection.IsSPmorethan)
                    {
                        reefers = reefers.Where(r => r.SetTemperature > selection.SetPoint.Value).ToList();
                    }
                    else
                    {
                        reefers = reefers.Where(r => r.SetTemperature == selection.SetPoint.Value).ToList();
                    }
                }

                result = result.Where(c => reefers.Any(u => string.Equals(u.ContainerNumber, c.ContainerNumber))).ToList();
            }

            // Free text
            if (!string.IsNullOrWhiteSpace(selection.FreeText))
            {
                var containers = cargoPlan.Containers.Where(r => r.Remarks.ContainsCaseAndSpaceInsensitive(selection.FreeText)).ToList();
                var reefers = cargoPlan.Reefers.Where(r => r.ReeferRemark.ContainsCaseAndSpaceInsensitive(selection.FreeText) ||
                                                        r.Commodity.ContainsCaseAndSpaceInsensitive(selection.FreeText) ||
                                                        r.ReeferSpecial.ContainsCaseAndSpaceInsensitive(selection.FreeText)).ToList();

                result = result.Where(c => (c.ContainerNumber.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.POL.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.POD.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.FinalDestination.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.ContainerType.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.Carrier.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.Remarks.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.AllDgClasses.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.Unno.ToString().ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.Name.Replace(" ", "").ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (!string.IsNullOrWhiteSpace(c.TechnicalName) && c.TechnicalName.Replace(" ", "").ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.Location.Replace(" ", "").ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (!string.IsNullOrWhiteSpace(c.LocationBeforeRestow) && c.LocationBeforeRestow.Replace(" ", "").ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (containers.Any(u => string.Equals(u.ContainerNumber, c.ContainerNumber))) ||
                                            (reefers.Any(u => string.Equals(u.ContainerNumber, c.ContainerNumber)))
                ).ToList();
            }

            return result.Select(c => c.Model.ID.ToString()).ToList();
        }

        /// <summary>
        /// Creates list of ContainerNumbers to assisgn to ItemsToSelect of DataGrids Reefers and Containers
        /// </summary>
        /// <param name="containersList"></param>
        /// <returns></returns>
        internal static List<string> CreateSelectContainerNumbersList(ObservableCollection<ContainerWrapper> containersList, SelectionControlViewModel selection, CargoPlanWrapper cargoPlan)
        {
            var result = containersList.ToList();

            // Container number
            if (!string.IsNullOrWhiteSpace(selection.SelectedNumber))
                result = result.Where(c => c.ContainerNumber.ContainsCaseAndSpaceInsensitive(selection.SelectedNumber)).ToList();

            // Position
            if (!selection.CellPosition.IsEmpty)
            {
                result = result.Where(c => selection.CellPosition.Model.Equals(c)).ToList();
            }
            // Old position
            if (!selection.CellOldPosition.IsEmpty)
            {
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.LocationBeforeRestow)
                                        && selection.CellOldPosition.Model.Equals(c.LocationBeforeRestow.ConvertToLocationOnBoard())).ToList();
            }

            // Underdeck
            if (selection.IsUnderdeck)
                result = result.Where(c => c.IsUnderdeck).ToList();
            else if (selection.IsOnDeck)
                result = result.Where(c => !c.IsUnderdeck).ToList();

            // Type of cargo
            if (selection.IsDG)
                result = result.Where(c => c.ContainsDgCargo).ToList();
            else if (selection.IsNotDg)
                result = result.Where(c => !c.ContainsDgCargo).ToList();

            if (selection.IsReefer)
                result = result.Where(c => c.IsRf).ToList();
            else if (selection.IsNonReefer)
                result = result.Where(c => !c.IsRf).ToList();

            // Container properties
            if (!string.IsNullOrWhiteSpace(selection.SelectedPOL))
                result = result.Where(c => c.POL.ContainsCaseAndSpaceInsensitive(selection.SelectedPOL)).ToList();
            if (!string.IsNullOrWhiteSpace(selection.SelectedPOD))
                result = result.Where(c => c.POD.ContainsCaseAndSpaceInsensitive(selection.SelectedPOD)).ToList();
            if (!string.IsNullOrWhiteSpace(selection.SelectedFinalDestination))
                result = result.Where(c => c.FinalDestination.ContainsCaseAndSpaceInsensitive(selection.SelectedFinalDestination)).ToList();
            if (!string.IsNullOrWhiteSpace(selection.SelectedContainerType))
                result = result.Where(c => c.ContainerType.ContainsCaseAndSpaceInsensitive(selection.SelectedContainerType)).ToList();
            if (!string.IsNullOrWhiteSpace(selection.SelectedOperator))
                result = result.Where(c => c.Carrier.ContainsCaseAndSpaceInsensitive(selection.SelectedOperator)).ToList();
            if (selection.IsOpenType)
                result = result.Where(c => c.IsOpen).ToList();

            if (selection.HasRemarks || selection.HasNoRemarks)
            {
                if (selection.HasRemarks)
                    result = result.Where(c => !string.IsNullOrWhiteSpace(c.Remarks)).ToList();
                else
                    result = result.Where(c => string.IsNullOrWhiteSpace(c.Remarks)).ToList();
            }

            // Changes
            if (selection.IsNewInPlan)
                result = result.Where(c => c.IsNewUnitInPlan).ToList();
            else if (selection.NotIsNewInPlan)
                result = result.Where(c => !c.IsNewUnitInPlan).ToList();
            if (selection.HasPositionChanged)
                result = result.Where(c => c.HasLocationChanged).ToList();
            else if (selection.NotHasPositionChanged)
                result = result.Where(c => !c.HasLocationChanged).ToList();
            if (selection.HasUpdated)
                result = result.Where(c => c.HasUpdated).ToList();
            else if (selection.NotHasUpdated)
                result = result.Where(c => !c.HasUpdated).ToList();
            if (selection.IsLocked)
                result = result.Where(c => c.IsPositionLockedForChange).ToList();
            else if (selection.NotIsLocked)
                result = result.Where(c => !c.IsPositionLockedForChange).ToList();
            if (selection.IsToImport)
                result = result.Where(c => c.IsToImport).ToList();
            else if (selection.NotIsToImport)
                result = result.Where(c => !c.IsNotToImport).ToList();
            if (selection.HasPODchanged)
                result = result.Where(c => c.HasPodChanged).ToList();
            else if (selection.NotHasPODchanged)
                result = result.Where(c => !c.HasPodChanged).ToList();
            if (selection.HasTypeChanged)
                result = result.Where(c => c.HasContainerTypeChanged).ToList();
            else if (selection.NotHasTypeChanged)
                result = result.Where(c => !c.HasContainerTypeChanged).ToList();
            if (selection.IsToBeKeptInPlan)
                result = result.Where(c => c.IsToBeKeptInPlan).ToList();
            else if (selection.NotIsToBeKeptInPlan)
                result = result.Where(c => !c.IsToBeKeptInPlan).ToList();

            // Dg
            if (selection.SelectedUNNO.HasValue || selection.SelectedUNNOs.Count > 0)
            {
                var unnos = selection.SelectedUNNOs.ToList();
                if (selection.SelectedUNNO.HasValue) unnos.Add(selection.SelectedUNNO.Value);

                var dgs = cargoPlan.DgList.ToList();
                dgs = dgs.Where(c => unnos.Contains(c.Unno)).ToList();
                result = result.Where(c => dgs.Any(dg => string.Equals(dg.ContainerNumber, c.ContainerNumber))).ToList();
            }
            if (!string.IsNullOrWhiteSpace(selection.SelectedDgClass) || selection.SelectedDgClasses.Count > 0)
            {
                var classes = selection.SelectedDgClasses.ToList();
                if (!string.IsNullOrWhiteSpace(selection.SelectedDgClass)) classes.Add(selection.SelectedDgClass);

                var dgs = cargoPlan.DgList.ToList();
                dgs = dgs.Where(c => classes.Any(cl => c.AllDgClasses.ContainsCaseAndSpaceInsensitive(cl))).ToList();
                result = result.Where(c => dgs.Any(dg => string.Equals(dg.ContainerNumber, c.ContainerNumber))).ToList();
            }

            // Flash point
            if (selection.FlashPoint.HasValue)
            {
                var dgs = cargoPlan.DgList.ToList();
                if (selection.IsFPlessthan)
                {
                    dgs = dgs.Where(dg => !dg.Model.FlashPointNotDefined && dg.Model.FlashPointAsDecimal < selection.FlashPoint.Value).ToList();
                }
                else if (selection.IsFPmorethan)
                {
                    dgs = dgs.Where(dg => !dg.Model.FlashPointNotDefined && dg.Model.FlashPointAsDecimal > selection.FlashPoint.Value).ToList();
                }
                else
                {
                    dgs = dgs.Where(dg => dg.Model.FlashPointAsDecimal == selection.FlashPoint.Value).ToList();
                }
                result = result.Where(c => dgs.Any(dg => string.Equals(dg.ContainerNumber, c.ContainerNumber))).ToList();
            }

            // Conflicted and remarks
            if (selection.IsConflicted || selection.NotIsConflicted || selection.HasDgRemarks || selection.NotHasDgRemarks)
            {
                var dgs = cargoPlan.DgList.ToList();
                if (selection.IsConflicted)
                    dgs = dgs.Where(c => c.IsConflicted).ToList();
                else if (selection.NotIsConflicted)
                    dgs = dgs.Where(c => !c.IsConflicted).ToList();
                if (selection.HasDgRemarks)
                    dgs = dgs.Where(c => !string.IsNullOrWhiteSpace(c.Remarks)).ToList();
                else if (selection.NotHasDgRemarks)
                    dgs = dgs.Where(c => string.IsNullOrWhiteSpace(c.Remarks)).ToList();
                result = result.Where(c => dgs.Any(dg => string.Equals(dg.ContainerNumber, c.ContainerNumber))).ToList();
            }

            // Reefer

            if (selection.HasCommodity)
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.Commodity)).ToList();
            else if (selection.NotHasCommodity)
                result = result.Where(c => string.IsNullOrWhiteSpace(c.Commodity)).ToList();

            if (selection.HasSpecial)
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.ReeferSpecial)).ToList();
            else if (selection.NotHasSpecial)
                result = result.Where(c => string.IsNullOrWhiteSpace(c.ReeferSpecial)).ToList();

            if (selection.HasReeferRemarks)
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.ReeferRemark)).ToList();
            else if (selection.NotHasReeferRemarks)
                result = result.Where(c => string.IsNullOrWhiteSpace(c.ReeferRemark)).ToList();

            if (selection.SetPoint.HasValue)
            {
                if (selection.IsSPlessthan)
                {
                    result = result.Where(r => r.SetTemperature < selection.SetPoint.Value).ToList();
                }
                else if (selection.IsSPmorethan)
                {
                    result = result.Where(r => r.SetTemperature > selection.SetPoint.Value).ToList();
                }
                else
                {
                    result = result.Where(r => r.SetTemperature == selection.SetPoint.Value).ToList();
                }
            }


            // Free text
            if (!string.IsNullOrWhiteSpace(selection.FreeText))
            {
                var dgs = cargoPlan.DgList.Where(dg => dg.Remarks.ContainsCaseAndSpaceInsensitive(selection.FreeText) ||
                                                (dg.Name.Replace(" ", "").ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                                (!string.IsNullOrWhiteSpace(dg.TechnicalName) && dg.TechnicalName.Replace(" ", "").ContainsCaseAndSpaceInsensitive(selection.FreeText))
                                                ).ToList();

                result = result.Where(c => (c.ContainerNumber.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.POL.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.POD.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.FinalDestination.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.ContainerType.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.Carrier.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (c.Remarks.ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            c.ReeferRemark.ContainsCaseAndSpaceInsensitive(selection.FreeText) ||
                                            c.Commodity.ContainsCaseAndSpaceInsensitive(selection.FreeText) ||
                                            c.ReeferSpecial.ContainsCaseAndSpaceInsensitive(selection.FreeText) ||
                                            (c.Location.Replace(" ", "").ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (!string.IsNullOrWhiteSpace(c.LocationBeforeRestow) && c.LocationBeforeRestow.Replace(" ", "").ContainsCaseAndSpaceInsensitive(selection.FreeText)) ||
                                            (dgs.Any(u => string.Equals(u.ContainerNumber, c.ContainerNumber)))
                ).ToList();
            }
            return result.Select(c => c.ContainerNumber).ToList();
        }
    }
}
