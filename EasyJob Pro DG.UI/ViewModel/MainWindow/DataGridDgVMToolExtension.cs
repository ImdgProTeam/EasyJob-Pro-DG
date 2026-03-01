using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyJob_ProDG.UI.ViewModel
{
    internal static class DataGridDgVMToolExtension
    {
        internal static void ChangeSelectedDgPropertyValues(this DataGridDgViewModel viewModel, Dictionary<string, string> changes)
        {
            if (viewModel == null || viewModel.GetSelectionObjectList().Count < 1 || changes.Count < 1) return;

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
                foreach (DgWrapper unit in viewModel.GetSelectionObjectList())
                {
                    unit.ChangeDgWrapperPropertiesValues(changes);
                }
            }

            // special case to remove reefers
            if (removeReefers)
            {
                HandleRemoveReefers(viewModel);
            }
        }

        private static void HandleRemoveReefers(DataGridDgViewModel viewModel)
        {
            var reefersToRemove = new List<ContainerWrapper>();
            foreach (DgWrapper unit in viewModel.GetSelectionObjectList())
            {
                var reefer = Services.ServicesHandler.GetServicesAccess().CargoDataServiceAccess.WorkingCargoPlan.Reefers.
                                                        FirstOrDefault(x => x.ContainerNumber == unit.ContainerNumber);
                if (unit.IsRf && reefer != null)
                    reefersToRemove.Add(reefer);
            }
            CargoPlanWrapperHandler.Launch().RemoveSeveralReefers(reefersToRemove);
        }

        /// <summary>
        /// Sets values to <see cref="DgWrapper"/> properties according to changes Dictionary.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="changes"></param>
        private static void ChangeDgWrapperPropertiesValues(this DgWrapper dg, Dictionary<string, string> changes)
        {
            foreach (string key in changes.Keys)
            {
                dg.SetDgWrapperPropertyValue(key, changes[key]);
            }
        }

        private static void SetDgWrapperPropertyValue(this DgWrapper dg, string propertyName, string value)
        {
            PropertyInfo dgInfo = dg.GetType().GetProperty(propertyName);

            switch (propertyName)
            {
                //Container properties
                case nameof(IContainer.POL):
                case nameof(IContainer.POD):
                case nameof(IContainer.FinalDestination):
                case nameof(IContainer.Carrier):
                case nameof(IContainer.ContainerType):
                    DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(dg, value, null, propertyName));
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
                        DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(dg, properValue, null, propertyName));
                    break;

                case "ContainerRemarks":
                    SetContainerRemark(dg.ContainerNumber, value);
                    break;

                //Reefer
                case nameof(IReefer.SetTemperature):
                case nameof(IReefer.LoadTemperature):
                case nameof(IReefer.VentSetting):
                case nameof(IReefer.Commodity):
                case nameof(IReefer.ReeferSpecial):
                case nameof(IReefer.ReeferRemark):
                    SetReeferProperty(dg.ContainerNumber, propertyName, value);
                    break;

                //Dg
                case nameof(DgWrapper.Unno):
                    if (ushort.TryParse(value, out ushort ushortValue))
                        dg.Unno = ushortValue;
                    break;

                //special cases
                case "PackingGroupAsByte":
                    dg.PackingGroup = value;
                    break;

                case nameof(DgWrapper.StowageCat):
                    if (value.Length > 1)
                        value = value.Substring(value.Length - 1);
                    dg.StowageCat = value[0];
                    break;

                case "FlashPointAsDecimal":
                    if (value == "clear")
                    {
                        dg.FlashPoint = string.Empty;
                    }
                    else
                        dg.FlashPoint = value;
                    break;

                //bool
                case nameof(DgWrapper.IsLq):
                case nameof(DgWrapper.IsMp):
                case nameof(DgWrapper.IsMax1L):
                case nameof(DgWrapper.IsWaste):
                case nameof(DgWrapper.IsStabilized):
                    if (!bool.TryParse(value, out bool boolValue))
                        return;

                    if (dgInfo == null)
                        throw new System.ArgumentNullException(propertyName, $"Unable to locate property for dg number {dg.ContainerNumber} in CargoPlan.DgList");

                    if (dgInfo.GetValue(dg)?.ToString() != value.ToString())
                        dgInfo.SetValue(dg, boolValue);
                    break;

                //decimal
                case nameof(DgWrapper.DgNetWeight):
                    if (decimal.TryParse(value, out decimal decimalValue))
                        dg.DgNetWeight = decimalValue;
                    break;

                //strings
                case nameof(DgWrapper.Name):
                case nameof(DgWrapper.TechnicalName):
                    if (dgInfo == null)
                        throw new System.ArgumentNullException(propertyName, $"Unable to locate property for dg number {dg.ContainerNumber} in CargoPlan.DgList");

                    if (dgInfo.GetValue(dg)?.ToString() != value.ToString())
                        dgInfo.SetValue(dg, value);
                    break;

                case "DgRemarks":
                    dg.Remarks = value;
                    break;

                default:
                    break;

            }
        }

        private static void SetReeferProperty(string containerNumber, string propertyName, string value)
        {
            var reefer = Services.ServicesHandler.GetServicesAccess().CargoDataServiceAccess.WorkingCargoPlan.Reefers.
                FirstOrDefault(x => x.ContainerNumber == containerNumber);
            if (reefer == null || !reefer.IsRf) return;

            switch (propertyName)
            {
                case nameof(IReefer.SetTemperature):
                    if (!decimal.TryParse(value, out decimal decimalValue)) return;
                    reefer.SetTemperature = decimalValue;
                    break;

                case nameof(IReefer.LoadTemperature):
                    if (!decimal.TryParse(value, out decimal decimalValue2)) return;
                    reefer.LoadTemperature = decimalValue2;
                    break;

                case nameof(IReefer.VentSetting):
                case nameof(IReefer.Commodity):
                case nameof(IReefer.ReeferSpecial):
                case nameof(IReefer.ReeferRemark):
                    PropertyInfo reeferInfo = reefer.GetType().GetProperty(propertyName);
                    if (reeferInfo == null)
                        throw new System.ArgumentNullException(propertyName, $"Unable to locate property for container number {reefer.ContainerNumber} in CargoPlan.Reefers");

                    if (reeferInfo.GetValue(reefer)?.ToString() != value.ToString())
                        reeferInfo.SetValue(reefer, value);
                    reefer.Refresh();
                    break;

                default:
                    break;
            }
            reefer.RefreshIReefer();
        }

        private static void SetContainerRemark(string containerNumber, string value)
        {
            var container = Services.ServicesHandler.GetServicesAccess().CargoDataServiceAccess.WorkingCargoPlan.Containers.
                FirstOrDefault(x => x.ContainerNumber == containerNumber);
            if (container != null)
                container.Remarks = value;
        }
    }
}
