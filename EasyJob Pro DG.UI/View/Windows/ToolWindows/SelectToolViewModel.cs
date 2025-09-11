using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.Windows.ToolWindows;
using EasyJob_ProDG.UI.ViewModel;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows.ToolWindows
{
    internal class SelectToolViewModel : Observable
    {
        // Private fields
        private CargoPlanWrapper _cargoPlan => ServicesHandler.GetServicesAccess().CargoDataServiceAccess.WorkingCargoPlan;
        private MainWindowViewModel _mainWindowViewModel => ViewModelLocator.MainWindowViewModel;
        private int _selectedDataGridIndex => ViewModelLocator.MainWindowViewModel.SelectedDataGridIndex;
        private SelectionControlViewModel _selection => SelectionControlViewModel;

        // Public properties
        public SelectionControlViewModel SelectionControlViewModel { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand SelectCommand { get; private set; }

        #region Constructor

        public SelectToolViewModel()
        {
            SelectionControlViewModel = new SelectionControlViewModel();
            SelectionControlViewModel.CreateLists(_cargoPlan);

            ClearCommand = new DelegateCommand(OnClearCommandExecuted);
            SelectCommand = new DelegateCommand(OnSelectExecuted);
        }

        #endregion


        /// <summary>
        /// Assigns Selected units list to ItemsToSelect property of a selected DataGrid
        /// </summary>
        private void SelectSelectedUnits()
        {
            switch (_selectedDataGridIndex)
            {
                case 0:
                    {
                        List<string> listToSelect = CreateSelectDgListIds();
                        _mainWindowViewModel.DataGridDgViewModel.ItemsToSelect = listToSelect;
                        break;
                    }
                case 1:
                    {
                        List<string> listToSelect = CreateSelectContainerNumbersList(_cargoPlan.Reefers);
                        _mainWindowViewModel.DataGridReefersViewModel.ItemsToSelect = listToSelect;
                        break;
                    }
                case 2:
                    {
                        List<string> listToSelect = CreateSelectContainerNumbersList(_cargoPlan.Containers);
                        _mainWindowViewModel.DataGridContainersViewModel.ItemsToSelect = listToSelect;
                        break;
                    }
                case 3:
                    {
                        List<string> listToSelect = CreateSelectContainerNumbersList((ObservableCollection<ContainerWrapper>)_mainWindowViewModel.DataGridUpdatesViewModel.UnitsPlanView.SourceCollection);
                        _mainWindowViewModel.DataGridUpdatesViewModel.ItemsToSelect = listToSelect;
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates list of items IDs to assign to ItemsToSelect of DataGridDg.
        /// </summary>
        /// <returns></returns>
        private List<string> CreateSelectDgListIds()
        {
            var result = _cargoPlan.DgList.ToList();

            // Container number
            if (!string.IsNullOrWhiteSpace(_selection.SelectedNumber))
                result = result.Where(c => c.ContainerNumber.ContainsCaseAndSpaceInsensitive(_selection.SelectedNumber)).ToList();

            // Position
            if (!_selection.CellPosition.IsEmpty)
            {
                result = result.Where(c => _selection.CellPosition.Model.Equals(c)).ToList();
            }
            // Old position
            if (!_selection.CellOldPosition.IsEmpty)
            {
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.LocationBeforeRestow) && _selection.CellOldPosition.Model.Equals(c.LocationBeforeRestow.ConvertToLocationOnBoard())).ToList();
            }

            // Underdeck
            if (_selection.IsUnderdeck)
                result = result.Where(c => c.IsUnderdeck).ToList();
            else if (_selection.IsOnDeck)
                result = result.Where(c => !c.IsUnderdeck).ToList();

            // Type of cargo
            if (_selection.IsDG)
                result = result;
            else if (_selection.IsNotDg)
                return new List<string>();

            if (_selection.IsReefer)
                result = result.Where(c => c.IsRf).ToList();
            else if (_selection.IsNonReefer)
                result = result.Where(c => !c.IsRf).ToList();

            // Container properties
            if (!string.IsNullOrWhiteSpace(_selection.SelectedPOL))
                result = result.Where(c => c.POL.ContainsCaseAndSpaceInsensitive(_selection.SelectedPOL)).ToList();
            if (!string.IsNullOrWhiteSpace(_selection.SelectedPOD))
                result = result.Where(c => c.POD.ContainsCaseAndSpaceInsensitive(_selection.SelectedPOD)).ToList();
            if (!string.IsNullOrWhiteSpace(_selection.SelectedFinalDestination))
                result = result.Where(c => c.FinalDestination.ContainsCaseAndSpaceInsensitive(_selection.SelectedFinalDestination)).ToList();
            if (!string.IsNullOrWhiteSpace(_selection.SelectedContainerType))
                result = result.Where(c => c.ContainerType.ContainsCaseAndSpaceInsensitive(_selection.SelectedContainerType)).ToList();
            if (!string.IsNullOrWhiteSpace(_selection.SelectedOperator))
                result = result.Where(c => c.Carrier.ContainsCaseAndSpaceInsensitive(_selection.SelectedOperator)).ToList();
            if (_selection.IsOpenType)
                result = result.Where(c => c.IsOpen).ToList();

            if (_selection.HasRemarks || _selection.HasNoRemarks)
            {
                var containers = _cargoPlan.Containers.ToList();
                if (_selection.HasRemarks)
                    containers = containers.Where(c => !string.IsNullOrWhiteSpace(c.Remarks)).ToList();
                else
                    containers = containers.Where(c => string.IsNullOrWhiteSpace(c.Remarks)).ToList();
                result = result.Where(c => containers.Any(u => string.Equals(u.ContainerNumber, c.ContainerNumber))).ToList();
            }


            // Changes
            if (_selection.IsNewInPlan)
                result = result.Where(c => c.IsNewUnitInPlan).ToList();
            else if (_selection.NotIsNewInPlan)
                result = result.Where(c => !c.IsNewUnitInPlan).ToList();
            if (_selection.HasPositionChanged)
                result = result.Where(c => c.HasLocationChanged).ToList();
            else if (_selection.NotHasPositionChanged)
                result = result.Where(c => !c.HasLocationChanged).ToList();
            if (_selection.HasUpdated)
                result = result.Where(c => c.HasUpdated).ToList();
            else if (_selection.NotHasUpdated)
                result = result.Where(c => !c.HasUpdated).ToList();
            if (_selection.IsLocked)
                result = result.Where(c => c.IsPositionLockedForChange).ToList();
            else if (_selection.NotIsLocked)
                result = result.Where(c => !c.IsPositionLockedForChange).ToList();
            if (_selection.IsToImport)
                result = result.Where(c => c.IsToImport).ToList();
            else if (_selection.NotIsToImport)
                result = result.Where(c => !c.IsNotToImport).ToList();
            if (_selection.HasPODchanged)
                result = result.Where(c => c.HasPodChanged).ToList();
            else if (_selection.NotHasPODchanged)
                result = result.Where(c => !c.HasPodChanged).ToList();
            if (_selection.HasTypeChanged)
                result = result.Where(c => c.HasContainerTypeChanged).ToList();
            else if (_selection.NotHasTypeChanged)
                result = result.Where(c => !c.HasContainerTypeChanged).ToList();
            if (_selection.IsToBeKeptInPlan)
                result = result.Where(c => c.IsToBeKeptInPlan).ToList();
            else if (_selection.NotIsToBeKeptInPlan)
                result = result.Where(c => !c.IsToBeKeptInPlan).ToList();

            // Dg
            if (_selection.SelectedUNNO.HasValue || _selection.SelectedUNNOs.Count > 0)
            {
                var unnos = _selection.SelectedUNNOs;
                if (_selection.SelectedUNNO.HasValue) unnos.Add(_selection.SelectedUNNO.Value);

                result = result.Where(c => unnos.Contains(c.Unno)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(_selection.SelectedDgClass) || _selection.SelectedDgClasses.Count > 0)
            {
                var classes = _selection.SelectedDgClasses;
                if (!string.IsNullOrWhiteSpace(_selection.SelectedDgClass)) classes.Add(_selection.SelectedDgClass);

                result = result.Where(c => classes.Any(cl => c.AllDgClasses.ContainsCaseAndSpaceInsensitive(cl))).ToList();
            }

            // Flash point
            if (_selection.FlashPoint.HasValue)
            {
                if (_selection.IsFPlessthan)
                {
                    result = result.Where(dg => !dg.Model.FlashPointNotDefined && dg.Model.FlashPointAsDecimal < _selection.FlashPoint.Value).ToList();
                }
                else if (_selection.IsFPmorethan)
                {
                    result = result.Where(dg => !dg.Model.FlashPointNotDefined && dg.Model.FlashPointAsDecimal > _selection.FlashPoint.Value).ToList();
                }
                else
                {
                    result = result.Where(dg => dg.Model.FlashPointAsDecimal == _selection.FlashPoint.Value).ToList();
                }
            }

            // Conflicted and remarks
            if (_selection.IsConflicted)
                result = result.Where(c => c.IsConflicted).ToList();
            else if (_selection.NotIsConflicted)
                result = result.Where(c => !c.IsConflicted).ToList();
            if (_selection.HasDgRemarks)
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.Remarks)).ToList();
            else if (_selection.NotHasDgRemarks)
                result = result.Where(c => string.IsNullOrWhiteSpace(c.Remarks)).ToList();

            // Reefer
            if (_selection.HasCommodity || _selection.NotHasCommodity || _selection.HasSpecial || _selection.NotHasSpecial ||
                _selection.HasReeferRemarks || _selection.NotHasReeferRemarks || _selection.SetPoint.HasValue)
            {
                var reefers = _cargoPlan.Reefers.ToList();

                if (_selection.HasCommodity)
                    reefers = reefers.Where(c => !string.IsNullOrWhiteSpace(c.Commodity)).ToList();
                else if (_selection.NotHasCommodity)
                    reefers = reefers.Where(c => string.IsNullOrWhiteSpace(c.Commodity)).ToList();

                if (_selection.HasSpecial)
                    reefers = reefers.Where(c => !string.IsNullOrWhiteSpace(c.ReeferSpecial)).ToList();
                else if (_selection.NotHasSpecial)
                    reefers = reefers.Where(c => string.IsNullOrWhiteSpace(c.ReeferSpecial)).ToList();

                if (_selection.HasReeferRemarks)
                    reefers = reefers.Where(c => !string.IsNullOrWhiteSpace(c.ReeferRemark)).ToList();
                else if (_selection.NotHasReeferRemarks)
                    reefers = reefers.Where(c => string.IsNullOrWhiteSpace(c.ReeferRemark)).ToList();

                if (_selection.SetPoint.HasValue)
                {
                    if (_selection.IsSPlessthan)
                    {
                        reefers = reefers.Where(r => r.SetTemperature < _selection.SetPoint.Value).ToList();
                    }
                    else if (_selection.IsSPmorethan)
                    {
                        reefers = reefers.Where(r => r.SetTemperature > _selection.SetPoint.Value).ToList();
                    }
                    else
                    {
                        reefers = reefers.Where(r => r.SetTemperature == _selection.SetPoint.Value).ToList();
                    }
                }

                result = result.Where(c => reefers.Any(u => string.Equals(u.ContainerNumber, c.ContainerNumber))).ToList();
            }

            // Free text
            if (!string.IsNullOrWhiteSpace(_selection.FreeText))
            {
                var containers = _cargoPlan.Containers.Where(r => r.Remarks.ContainsCaseAndSpaceInsensitive(_selection.FreeText)).ToList();
                var reefers = _cargoPlan.Reefers.Where(r => r.ReeferRemark.ContainsCaseAndSpaceInsensitive(_selection.FreeText) ||
                                                        r.Commodity.ContainsCaseAndSpaceInsensitive(_selection.FreeText) ||
                                                        r.ReeferSpecial.ContainsCaseAndSpaceInsensitive(_selection.FreeText)).ToList();

                result = result.Where(c => (c.ContainerNumber.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.POL.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.POD.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.FinalDestination.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.ContainerType.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.Carrier.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.Remarks.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.AllDgClasses.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.Unno.ToString().ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.Name.Replace(" ", "").ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (!string.IsNullOrWhiteSpace(c.TechnicalName) && c.TechnicalName.Replace(" ", "").ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.Location.Replace(" ", "").ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (!string.IsNullOrWhiteSpace(c.LocationBeforeRestow) && c.LocationBeforeRestow.Replace(" ", "").ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
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
        private List<string> CreateSelectContainerNumbersList(ObservableCollection<ContainerWrapper> containersList)
        {
            var result = containersList.ToList();

            // Container number
            if (!string.IsNullOrWhiteSpace(_selection.SelectedNumber))
                result = result.Where(c => c.ContainerNumber.ContainsCaseAndSpaceInsensitive(_selection.SelectedNumber)).ToList();

            // Position
            if (!_selection.CellPosition.IsEmpty)
            {
                result = result.Where(c => _selection.CellPosition.Model.Equals(c)).ToList();
            }
            // Old position
            if (!_selection.CellOldPosition.IsEmpty)
            {
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.LocationBeforeRestow)
                                        && _selection.CellOldPosition.Model.Equals(c.LocationBeforeRestow.ConvertToLocationOnBoard())).ToList();
            }

            // Underdeck
            if (_selection.IsUnderdeck)
                result = result.Where(c => c.IsUnderdeck).ToList();
            else if (_selection.IsOnDeck)
                result = result.Where(c => !c.IsUnderdeck).ToList();

            // Type of cargo
            if (_selection.IsDG)
                result = result.Where(c => c.ContainsDgCargo).ToList();
            else if (_selection.IsNotDg)
                result = result.Where(c => !c.ContainsDgCargo).ToList();

            if (_selection.IsReefer)
                result = result.Where(c => c.IsRf).ToList();
            else if (_selection.IsNonReefer)
                result = result.Where(c => !c.IsRf).ToList();

            // Container properties
            if (!string.IsNullOrWhiteSpace(_selection.SelectedPOL))
                result = result.Where(c => c.POL.ContainsCaseAndSpaceInsensitive(_selection.SelectedPOL)).ToList();
            if (!string.IsNullOrWhiteSpace(_selection.SelectedPOD))
                result = result.Where(c => c.POD.ContainsCaseAndSpaceInsensitive(_selection.SelectedPOD)).ToList();
            if (!string.IsNullOrWhiteSpace(_selection.SelectedFinalDestination))
                result = result.Where(c => c.FinalDestination.ContainsCaseAndSpaceInsensitive(_selection.SelectedFinalDestination)).ToList();
            if (!string.IsNullOrWhiteSpace(_selection.SelectedContainerType))
                result = result.Where(c => c.ContainerType.ContainsCaseAndSpaceInsensitive(_selection.SelectedContainerType)).ToList();
            if (!string.IsNullOrWhiteSpace(_selection.SelectedOperator))
                result = result.Where(c => c.Carrier.ContainsCaseAndSpaceInsensitive(_selection.SelectedOperator)).ToList();
            if (_selection.IsOpenType)
                result = result.Where(c => c.IsOpen).ToList();

            if (_selection.HasRemarks || _selection.HasNoRemarks)
            {
                if (_selection.HasRemarks)
                    result = result.Where(c => !string.IsNullOrWhiteSpace(c.Remarks)).ToList();
                else
                    result = result.Where(c => string.IsNullOrWhiteSpace(c.Remarks)).ToList();
            }

            // Changes
            if (_selection.IsNewInPlan)
                result = result.Where(c => c.IsNewUnitInPlan).ToList();
            else if (_selection.NotIsNewInPlan)
                result = result.Where(c => !c.IsNewUnitInPlan).ToList();
            if (_selection.HasPositionChanged)
                result = result.Where(c => c.HasLocationChanged).ToList();
            else if (_selection.NotHasPositionChanged)
                result = result.Where(c => !c.HasLocationChanged).ToList();
            if (_selection.HasUpdated)
                result = result.Where(c => c.HasUpdated).ToList();
            else if (_selection.NotHasUpdated)
                result = result.Where(c => !c.HasUpdated).ToList();
            if (_selection.IsLocked)
                result = result.Where(c => c.IsPositionLockedForChange).ToList();
            else if (_selection.NotIsLocked)
                result = result.Where(c => !c.IsPositionLockedForChange).ToList();
            if (_selection.IsToImport)
                result = result.Where(c => c.IsToImport).ToList();
            else if (_selection.NotIsToImport)
                result = result.Where(c => !c.IsNotToImport).ToList();
            if (_selection.HasPODchanged)
                result = result.Where(c => c.HasPodChanged).ToList();
            else if (_selection.NotHasPODchanged)
                result = result.Where(c => !c.HasPodChanged).ToList();
            if (_selection.HasTypeChanged)
                result = result.Where(c => c.HasContainerTypeChanged).ToList();
            else if (_selection.NotHasTypeChanged)
                result = result.Where(c => !c.HasContainerTypeChanged).ToList();
            if (_selection.IsToBeKeptInPlan)
                result = result.Where(c => c.IsToBeKeptInPlan).ToList();
            else if (_selection.NotIsToBeKeptInPlan)
                result = result.Where(c => !c.IsToBeKeptInPlan).ToList();

            // Dg
            if (_selection.SelectedUNNO.HasValue || _selection.SelectedUNNOs.Count > 0)
            {
                var unnos = _selection.SelectedUNNOs.ToList();
                if (_selection.SelectedUNNO.HasValue) unnos.Add(_selection.SelectedUNNO.Value);

                var dgs = _cargoPlan.DgList.ToList();
                dgs = dgs.Where(c => unnos.Contains(c.Unno)).ToList();
                result = result.Where(c => dgs.Any(dg => string.Equals(dg.ContainerNumber, c.ContainerNumber))).ToList();
            }
            if (!string.IsNullOrWhiteSpace(_selection.SelectedDgClass) || _selection.SelectedDgClasses.Count > 0)
            {
                var classes = _selection.SelectedDgClasses.ToList();
                if (!string.IsNullOrWhiteSpace(_selection.SelectedDgClass)) classes.Add(_selection.SelectedDgClass);

                var dgs = _cargoPlan.DgList.ToList();
                dgs = dgs.Where(c => classes.Any(cl => c.AllDgClasses.ContainsCaseAndSpaceInsensitive(cl))).ToList();
                result = result.Where(c => dgs.Any(dg => string.Equals(dg.ContainerNumber, c.ContainerNumber))).ToList();
            }

            // Flash point
            if (_selection.FlashPoint.HasValue)
            {
                var dgs = _cargoPlan.DgList.ToList();
                if (_selection.IsFPlessthan)
                {
                    dgs = dgs.Where(dg => !dg.Model.FlashPointNotDefined && dg.Model.FlashPointAsDecimal < _selection.FlashPoint.Value).ToList();
                }
                else if (_selection.IsFPmorethan)
                {
                    dgs = dgs.Where(dg => !dg.Model.FlashPointNotDefined && dg.Model.FlashPointAsDecimal > _selection.FlashPoint.Value).ToList();
                }
                else
                {
                    dgs = dgs.Where(dg => dg.Model.FlashPointAsDecimal == _selection.FlashPoint.Value).ToList();
                }
                result = result.Where(c => dgs.Any(dg => string.Equals(dg.ContainerNumber, c.ContainerNumber))).ToList();
            }

            // Conflicted and remarks
            if (_selection.IsConflicted || _selection.NotIsConflicted || _selection.HasDgRemarks || _selection.NotHasDgRemarks)
            {
                var dgs = _cargoPlan.DgList.ToList();
                if (_selection.IsConflicted)
                    dgs = dgs.Where(c => c.IsConflicted).ToList();
                else if (_selection.NotIsConflicted)
                    dgs = dgs.Where(c => !c.IsConflicted).ToList();
                if (_selection.HasDgRemarks)
                    dgs = dgs.Where(c => !string.IsNullOrWhiteSpace(c.Remarks)).ToList();
                else if (_selection.NotHasDgRemarks)
                    dgs = dgs.Where(c => string.IsNullOrWhiteSpace(c.Remarks)).ToList();
                result = result.Where(c => dgs.Any(dg => string.Equals(dg.ContainerNumber, c.ContainerNumber))).ToList();
            }

            // Reefer

            if (_selection.HasCommodity)
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.Commodity)).ToList();
            else if (_selection.NotHasCommodity)
                result = result.Where(c => string.IsNullOrWhiteSpace(c.Commodity)).ToList();

            if (_selection.HasSpecial)
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.ReeferSpecial)).ToList();
            else if (_selection.NotHasSpecial)
                result = result.Where(c => string.IsNullOrWhiteSpace(c.ReeferSpecial)).ToList();

            if (_selection.HasReeferRemarks)
                result = result.Where(c => !string.IsNullOrWhiteSpace(c.ReeferRemark)).ToList();
            else if (_selection.NotHasReeferRemarks)
                result = result.Where(c => string.IsNullOrWhiteSpace(c.ReeferRemark)).ToList();

            if (_selection.SetPoint.HasValue)
            {
                if (_selection.IsSPlessthan)
                {
                    result = result.Where(r => r.SetTemperature < _selection.SetPoint.Value).ToList();
                }
                else if (_selection.IsSPmorethan)
                {
                    result = result.Where(r => r.SetTemperature > _selection.SetPoint.Value).ToList();
                }
                else
                {
                    result = result.Where(r => r.SetTemperature == _selection.SetPoint.Value).ToList();
                }
            }


            // Free text
            if (!string.IsNullOrWhiteSpace(_selection.FreeText))
            {
                var dgs = _cargoPlan.DgList.Where(dg => dg.Remarks.ContainsCaseAndSpaceInsensitive(_selection.FreeText) ||
                                                (dg.Name.Replace(" ", "").ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                                (!string.IsNullOrWhiteSpace(dg.TechnicalName) && dg.TechnicalName.Replace(" ", "").ContainsCaseAndSpaceInsensitive(_selection.FreeText))
                                                ).ToList();

                result = result.Where(c => (c.ContainerNumber.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.POL.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.POD.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.FinalDestination.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.ContainerType.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.Carrier.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (c.Remarks.ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            c.ReeferRemark.ContainsCaseAndSpaceInsensitive(_selection.FreeText) ||
                                            c.Commodity.ContainsCaseAndSpaceInsensitive(_selection.FreeText) ||
                                            c.ReeferSpecial.ContainsCaseAndSpaceInsensitive(_selection.FreeText) ||
                                            (c.Location.Replace(" ", "").ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (!string.IsNullOrWhiteSpace(c.LocationBeforeRestow) && c.LocationBeforeRestow.Replace(" ", "").ContainsCaseAndSpaceInsensitive(_selection.FreeText)) ||
                                            (dgs.Any(u => string.Equals(u.ContainerNumber, c.ContainerNumber)))
                ).ToList();
            }
            return result.Select(c => c.ContainerNumber).ToList();
        }

        #region Command methods

        /// <summary>
        /// On 'Select' button pressed
        /// </summary>
        /// <param name="obj"></param>
        private void OnSelectExecuted(object obj)
        {
            SelectSelectedUnits();
        }

        /// <summary>
        /// Clears all filter values
        /// </summary>
        /// <param name="obj"></param>
        private void OnClearCommandExecuted(object obj)
        {
            SelectionControlViewModel.Clear();
        }

        #endregion
    }
}
