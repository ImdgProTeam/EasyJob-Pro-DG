using System.Linq;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Messages;
using System.Reflection;
using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.UI.Wrapper
{
    /// <summary>
    /// Class is used to receive messages and make necessary changes related to respective unit in all three Container, Dg and Reefer lists.
    /// </summary>
    internal class CargoPlanUnitPropertyChanger
    {
        private static CargoPlanUnitPropertyChanger _changer = null;
        private readonly MessageDialogService _messageDialogService = new MessageDialogService();
        private CargoPlanWrapper _cargoPlanWrapper;

        internal static bool ChangeInProgress;

        //Empty constructor
        internal CargoPlanUnitPropertyChanger()
        {

        }

        /// <summary>
        /// Initializes fields of the Changer
        /// </summary>
        /// <param name="cargoPlanWrapper">Working CargoPlanWrapper</param>
        internal void SetUp(CargoPlanWrapper cargoPlanWrapper)
        {
            _changer ??= new CargoPlanUnitPropertyChanger();

            _cargoPlanWrapper = cargoPlanWrapper;
            ChangeInProgress = false;

            DataMessenger.Default.Unregister(_changer);
            DataMessenger.Default.Register<CargoPlanUnitPropertyChanged>(_changer, OnCargoPlanUnitPropertyChanged);
        }

        /// <summary>
        /// Handles change of any unit property changed message received
        /// </summary>
        /// <param name="obj"></param>
        public void OnCargoPlanUnitPropertyChanged(CargoPlanUnitPropertyChanged obj)
        {
            if (ChangeInProgress) return;
            ChangeInProgress = true;

            switch (obj.PropertyName)
            {
                case "Location":
                case "ContainerType":
                case "IsClosed":
                    SetNewCargoPlanUnitPropertyValue(obj.ContainerNumber, obj.Value, obj.PropertyName);
                    UpdateConflictList();
                    break;

                case "POL":
                case "POD":
                case "FinalDestination":
                case "Carrier":
                case "IsPositionLockedForChange":
                case "IsToBeKeptInPlan":
                case "IsToImport":
                case "IsNotToImport":
                    SetNewCargoPlanUnitPropertyValue(obj.ContainerNumber, obj.Value, obj.PropertyName);
                    break;

                case "ContainerNumber":
                    if (AskSetContainerNumber(obj))
                    {
                        ChangeInProgress = false;
                        return;
                    }
                    break;

                case "IsRf":
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
        private void UpdateConflictList()
        {
            OnConflictListToBeChangedEventHandler.Invoke(this);
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
            UpdateConflictList();
            return false;
        }

        /// <summary>
        /// Sets value to chosen property name to all units in CargoPlan with the same containerNumber
        /// </summary>
        /// <param name="containerNumber">ContainerNumber of units to be updated</param>
        /// <param name="value">New value</param>
        /// <param name="propertyName">Property to be changed</param>
        internal void SetNewCargoPlanUnitPropertyValue(string containerNumber, object value, string propertyName)
        {
            foreach (var dg in _cargoPlanWrapper.DgList)
            {
                PropertyInfo dgInfo = dg.GetType().GetProperty(propertyName);
                if (dgInfo == null) continue;

                if (dg.ContainerNumber == containerNumber)
                    if (dgInfo.GetValue(dg).ToString() != value.ToString())
                        dgInfo.SetValue(dg, value);
            }

            foreach (var container in _cargoPlanWrapper.Containers)
            {
                PropertyInfo containerInfo = container.GetType().GetProperty(propertyName);
                if (containerInfo == null) continue;

                if (container.ContainerNumber == containerNumber)
                {
                    if (containerInfo.GetValue(container).ToString() != value.ToString())
                        containerInfo.SetValue(container, value);
                    container.Refresh();
                }
            }

            foreach (var reefer in _cargoPlanWrapper.Reefers)
            {
                PropertyInfo reeferInfo = reefer.GetType().GetProperty(propertyName);
                if (reeferInfo == null) continue;

                if (reefer.ContainerNumber == containerNumber)
                {
                    if (reeferInfo.GetValue(reefer).ToString() != value.ToString())
                        reeferInfo.SetValue(reefer, value);
                    reefer.Refresh();
                }
            }
        }

        /// <summary>
        /// Changes container number to all items with matching number in all lists 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue"></param>
        internal void SetNewContainerNumber(string value, string oldValue)
        {
            foreach (var dg in _cargoPlanWrapper.DgList)
            {
                if (dg.ContainerNumber != oldValue) continue;
                if (dg.ContainerNumber == value) continue;
                dg.ContainerNumber = value;
            }

            foreach (var container in _cargoPlanWrapper.Containers)
            {
                if (container.ContainerNumber != oldValue) continue;
                if (container.ContainerNumber != value)
                    container.ContainerNumber = value;
                else container.Refresh();
            }

            foreach (var reefer in _cargoPlanWrapper.Reefers)
            {
                if (reefer.ContainerNumber != oldValue) continue;
                if (reefer.ContainerNumber != value)
                    reefer.ContainerNumber = value;
            }
        }

        /// <summary>
        /// Handles change in IsRf property
        /// </summary>
        /// <param name="obj"></param>
        private void SetIsReeferProperty(CargoPlanUnitPropertyChanged obj)
        {
            ContainerWrapper wrapper = (bool)obj.IsDgWrapper
                ? _cargoPlanWrapper.Containers.FirstOrDefault(c => c.ContainerNumber == ((IContainer)obj.Unit).ContainerNumber)
                : (ContainerWrapper)obj.Unit;
            if (wrapper == null) return;

            //remove reefer
            if (!(bool)obj.Value)
            {
                bool removed = _messageDialogService.ShowYesNoDialog(
                                   $"Do you want to delete unit {obj.ContainerNumber} from Reefer list", "")
                               == MessageDialogResult.Yes;
                _cargoPlanWrapper.RemoveReefer(wrapper);
                SetNewCargoPlanUnitPropertyValue(obj.ContainerNumber, !removed, obj.PropertyName);
            }

            //add reefer
            else
            {
                _cargoPlanWrapper.AddReefer(wrapper);
                SetNewCargoPlanUnitPropertyValue(obj.ContainerNumber, obj.Value, obj.PropertyName);
            }

            //to all
            UpdateConflictList();
        }


        public delegate void ConflictListToBeChangedEventHandler(object sender);
        public static event ConflictListToBeChangedEventHandler OnConflictListToBeChangedEventHandler = null;
    }
}
