using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System.Linq;
using System.Reflection;

namespace EasyJob_ProDG.UI.Data
{
    /// <summary>
    /// Class is used to receive messages and make necessary changes related to respective unit in all three Container, Dg and Reefer lists.
    /// </summary>
    internal class CargoPlanUnitPropertyChanger
    {
        private static CargoPlanUnitPropertyChanger _changer = null;
        private MessageDialogService _messageDialogService => MessageDialogService.Connect();
        private CargoDataService _cargoDataService => CargoDataService.GetCargoDataService();
        private CargoPlanWrapper _workingCargoPlan => _cargoDataService.WorkingCargoPlan;

        private bool ChangeInProgress;

        #region Constructor
        // ----- Constructor -----

        private CargoPlanUnitPropertyChanger()
        {
            RegisterInMessenger();
            ChangeInProgress = false;
        }

        #endregion

        #region SetUp
        /// <summary>
        /// Initializes fields of the Changer
        /// </summary>
        internal static void Launch()
        {
            _changer ??= new CargoPlanUnitPropertyChanger();
        }

        private void RegisterInMessenger()
        {
            DataMessenger.Default.Unregister(_changer);
            DataMessenger.Default.Register<CargoPlanUnitPropertyChanged>(_changer, OnCargoPlanUnitPropertyChanged);
        }

        #endregion

        /// <summary>
        /// Handles change of any unit property changed message received
        /// </summary>
        /// <param name="obj"></param>
        private void OnCargoPlanUnitPropertyChanged(CargoPlanUnitPropertyChanged obj)
        {
            if (ChangeInProgress) return;
            ChangeInProgress = true;

            switch (obj.PropertyName)
            {
                case nameof(ILocationOnBoard.Location):
                case nameof(IContainer.ContainerType):
                case nameof(IContainer.IsClosed):
                    SetNewCargoPlanUnitPropertyValue(obj.ContainerNumber, obj.Value, obj.PropertyName);
                    break;

                case nameof(IContainer.POL):
                case nameof(IContainer.POD):
                case nameof(IContainer.FinalDestination):
                case nameof(IContainer.Carrier):
                case nameof(Model.IO.IUpdatable.IsPositionLockedForChange):
                case nameof(Model.IO.IUpdatable.IsToBeKeptInPlan):
                case nameof(Model.IO.IUpdatable.IsToImport):
                case nameof(Model.IO.IUpdatable.IsNotToImport):
                    SetNewCargoPlanUnitPropertyValue(obj.ContainerNumber, obj.Value, obj.PropertyName);
                    break;

                case nameof(IContainer.ContainerNumber):
                    if (AskSetContainerNumber(obj))
                    {
                        ChangeInProgress = false;
                        return;
                    }
                    break;

                case nameof(IContainer.IsRf):
                    SetIsReeferProperty(obj);
                    break;

                default:
                    break;
            }

            ChangeInProgress = false;
        }

        /// <summary>
        /// Invokes OnConflictListToBeChanged event
        /// </summary>
        private void RefreshConflictList()
        {
            DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage());
        }

        /// <summary>
        /// Handles the change of containerNumber property
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if change cancelled by user</returns>
        private bool AskSetContainerNumber(CargoPlanUnitPropertyChanged obj)
        {
            if (obj.OldValue == null) return true;

            var answer = _messageDialogService.ShowOkCancelDialog(
                "Container number will be changed for all units with the same number in the plan?",
                null);

            if (answer == MessageDialogResult.Cancel)
            {
                SetNewContainerNumber(obj.OldValue.ToString(), obj.Value.ToString());
                return true;
            }

            SetNewContainerNumber(obj.Value.ToString(), obj.OldValue.ToString());
            RefreshConflictList();
            return false;
        }

        /// <summary>
        /// Sets value to chosen property name to all units in WorkingCargoPlan with the same containerNumber
        /// </summary>
        /// <param name="containerNumber">ContainerNumber of units to be updated</param>
        /// <param name="value">New value</param>
        /// <param name="propertyName">Property to be changed</param>
        private void SetNewCargoPlanUnitPropertyValue(string containerNumber, object value, string propertyName)
        {
            var _dgs = _workingCargoPlan.DgList.Where(x => x.ContainerNumber == containerNumber);
            foreach (var dg in _dgs)
            {
                if (dg.ContainerNumber != containerNumber)
                    continue;

                PropertyInfo dgInfo = dg.GetType().GetProperty(propertyName);
                if (dgInfo == null) continue;

                if (dgInfo.GetValue(dg).ToString() != value.ToString())
                    dgInfo.SetValue(dg, value);
            }

            var container = _workingCargoPlan.Containers.FirstOrDefault(x => x.ContainerNumber == containerNumber);
            if (container != null)
            {
                PropertyInfo containerInfo = container.GetType().GetProperty(propertyName);
                if (containerInfo == null) 
                    throw new System.ArgumentNullException(propertyName, $"Unable to locate property for container number {containerNumber} in CargoPlan.Containers");

                if (containerInfo.GetValue(container).ToString() != value.ToString())
                    containerInfo.SetValue(container, value);
                container.Refresh();
            }

            var reefer = _workingCargoPlan.Reefers.FirstOrDefault(x => x.ContainerNumber == containerNumber);
            if (reefer != null)
            {
                PropertyInfo reeferInfo = reefer.GetType().GetProperty(propertyName);
                if (reeferInfo == null) 
                    throw new System.ArgumentNullException(propertyName, $"Unable to locate property for container number {containerNumber} in CargoPlan.Reefers");

                if (reeferInfo.GetValue(reefer).ToString() != value.ToString())
                    reeferInfo.SetValue(reefer, value);
                reefer.Refresh();
            }

            _workingCargoPlan.RefreshCargoPlanValues();
        }

        /// <summary>
        /// Changes container number to all items with matching number in all lists 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue"></param>
        private void SetNewContainerNumber(string value, string oldValue)
        {
            var _dgs = _workingCargoPlan.DgList.Where(x => x.ContainerNumber == oldValue);
            foreach (var dg in _dgs)
            {
                if (dg.ContainerNumber != oldValue) continue;
                if (dg.ContainerNumber == value) continue;
                dg.ContainerNumber = value;
            }

            var container = _workingCargoPlan.Containers.FirstOrDefault(x => x.ContainerNumber == oldValue);
            if (container?.ContainerNumber == oldValue)
            {
                if (container.ContainerNumber != value)
                    container.ContainerNumber = value;
                container.Refresh();
            }

            var reefer = _workingCargoPlan.Reefers.FirstOrDefault(x => x.ContainerNumber == oldValue);
            if(reefer is null) return;
            if (reefer.ContainerNumber != oldValue) return;
            if (reefer.ContainerNumber != value)
                reefer.ContainerNumber = value;
            reefer.Refresh();
        }

        /// <summary>
        /// Handles change in IsRf property
        /// </summary>
        /// <param name="obj"></param>
        private void SetIsReeferProperty(CargoPlanUnitPropertyChanged obj)
        {
            ContainerWrapper wrapper = obj.IsDgWrapper
                ? _workingCargoPlan.Containers.FindContainerByContainerNumber((IContainer)obj.Unit)
                : (ContainerWrapper)obj.Unit;
            if (wrapper == null) return;

            //remove reefer
            if (!(bool)obj.Value)
            {
                bool removed = _messageDialogService.ShowYesNoDialog(
                                   $"Do you want to delete unit {obj.ContainerNumber} from Reefer list", "")
                               == MessageDialogResult.Yes;
                if (removed) _workingCargoPlan.RemoveReefer(wrapper);
                SetNewCargoPlanUnitPropertyValue(obj.ContainerNumber, !removed, obj.PropertyName);
            }

            //add reefer
            else
            {
                _workingCargoPlan.AddReefer(wrapper);
                SetNewCargoPlanUnitPropertyValue(obj.ContainerNumber, obj.Value, obj.PropertyName);
            }

            //to all
            RefreshConflictList();
        }

    }
}
