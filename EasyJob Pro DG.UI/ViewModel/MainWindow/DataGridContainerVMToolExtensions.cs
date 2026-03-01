using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyJob_ProDG.UI.ViewModel
{
    internal static class DataGridContainerVMToolExtensions
    {
        private static DataGridContainerViewModelBase _viewModel;

        /// <summary>
        /// Sets changes to properties values of Selected <see cref="ContainerWrapper"/>s according to <see cref="changes"/> Dictionary.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="changes"></param>
        internal static void ChangeSelectedContainerWrapperPropertiesValue(this DataGridContainerViewModelBase viewModel, Dictionary<string, string> changes)
        {
            if (viewModel == null || viewModel.GetSelectionObjectList().Count < 1 || changes.Count < 1) return;
            _viewModel = viewModel;

            var selection = viewModel.GetSelectionObjectList();

            //delay removing reefers in order to set other properties
            bool removeReefers = changes.ContainsKey(nameof(IContainer.IsRf)) && changes[nameof(IContainer.IsRf)] == "false";
            if (removeReefers)
            {
                changes.Remove(nameof(IContainer.IsRf));
            }

            //set all properties
            if (changes.Count > 0)
            {
                foreach (ContainerWrapper unit in viewModel.GetSelectionObjectList())
                {
                    unit.ChangeContainerWrapperPropertiesValues(changes);
                }
            }

            // special case to remove reefers
            if (removeReefers)
            {
                HandleRemoveReefers(viewModel);
            }
        }

        private static void HandleRemoveReefers(DataGridContainerViewModelBase viewModel)
        {
            var reefersToRemove = new List<ContainerWrapper>();
            foreach (ContainerWrapper unit in viewModel.GetSelectionObjectList())
            {
                if (unit.IsRf)
                    reefersToRemove.Add(unit);
            }
            CargoPlanWrapperHandler.Launch().RemoveSeveralReefers(reefersToRemove);
        }

        private static void RefreshReeferProperties(string containerNumber)
        {
            if (_viewModel is DataGridReefersViewModel) return;

            var reefers = Services.ServicesHandler.GetServicesAccess().CargoDataServiceAccess.WorkingCargoPlan.Reefers;
            if (reefers.Count < 1) return;

            var reefer = reefers.FirstOrDefault(r => r.ContainerNumber == containerNumber);
            if (reefer != null)
                reefer.RefreshIReefer();
        }

        /// <summary>
        /// Sets values to <see cref="ContainerWrapper"/> properties according to changes Dictionary.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="changes"></param>
        private static void ChangeContainerWrapperPropertiesValues(this ContainerWrapper container, Dictionary<string, string> changes)
        {
            foreach (string key in changes.Keys)
            {
                container.SetContainerWrapperPropertyValue(key, changes[key]);
            }
        }

        private static void SetContainerWrapperPropertyValue(this ContainerWrapper container, string propertyName, string value)
        {
            switch (propertyName)
            {
                //Container properties
                case nameof(IContainer.POL):
                case nameof(IContainer.POD):
                case nameof(IContainer.FinalDestination):
                case nameof(IContainer.Carrier):
                case nameof(IContainer.ContainerType):
                    DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(container, value, null, propertyName));
                    break;

                //Booleans container and IUpdatable
                case nameof(IContainer.IsClosed):
                case nameof(IContainer.IsRf):
                //IUpdatable
                case nameof(Model.IO.IUpdatable.IsPositionLockedForChange):
                case nameof(Model.IO.IUpdatable.IsToBeKeptInPlan):
                case nameof(Model.IO.IUpdatable.IsToImport):
                case nameof(Model.IO.IUpdatable.IsNotToImport):
                    if (bool.TryParse(value, out bool properValue))
                        DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(container, properValue, null, propertyName));
                    break;

                case "ContainerRemarks":
                    container.Remarks = value;
                    break;

                //Reefer
                case nameof(IReefer.SetTemperature):
                    if (!decimal.TryParse(value, out decimal decimalValue)) return;
                    if (container.IsRf)
                        container.SetTemperature = decimalValue;
                    RefreshReeferProperties(container.ContainerNumber);
                    break;
                case nameof(IReefer.LoadTemperature):
                    if (!decimal.TryParse(value, out decimal decimalValue2)) return;
                    if (container.IsRf)
                        container.LoadTemperature = decimalValue2;
                    RefreshReeferProperties(container.ContainerNumber);
                    break;

                case nameof(IReefer.VentSetting):
                case nameof(IReefer.Commodity):
                case nameof(IReefer.ReeferSpecial):
                case nameof(IReefer.ReeferRemark):
                    if (!container.IsRf) break;
                    PropertyInfo reeferInfo = container.GetType().GetProperty(propertyName);
                    if (reeferInfo == null)
                        throw new System.ArgumentNullException(propertyName, $"Unable to locate property for container number {container.ContainerNumber} in CargoPlan.Reefers");

                    if (reeferInfo.GetValue(container)?.ToString() != value.ToString())
                        reeferInfo.SetValue(container, value);
                    container.Refresh();
                    RefreshReeferProperties(container.ContainerNumber);
                    break;

                default:
                    break;

            }
        }
    }
}
