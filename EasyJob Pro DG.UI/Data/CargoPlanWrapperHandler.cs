using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    /// <summary>
    /// Receives messages and accordingly manipulates loaded <see cref="CargoPlanWrapper"/>.
    /// </summary>
    internal class CargoPlanWrapperHandler
    {
        // ----------------- Private fields ------------------------------------
        private static CargoPlanWrapperHandler _handleCargoPlanWrapper;
        private MessageDialogService _messageDialogService => MessageDialogService.Connect();
        private ICargoDataService _cargoDataService;
        private CargoPlanWrapper _cargoPlan => _cargoDataService.WorkingCargoPlan;


        // ----- Access to this handler -----
        internal static CargoPlanWrapperHandler Launch() => _handleCargoPlanWrapper ??= new CargoPlanWrapperHandler();


        // -------------- Add/Remove/Modify methods ---------------------------------

        /// <summary>
        /// Removes selected dg from WorkingCargoPlan
        /// </summary>
        /// <param name="obj"></param>
        private void OnDgRemoveRequested(UpdateCargoPlan obj)
        {
            foreach (var dgWrapper in obj.DgWrappersChanged)
            {
                _cargoPlan.RemoveDg(dgWrapper);
                _cargoPlan.RemoveRemovedDgFromCargoPlan(dgWrapper);
            }

            DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage());
            DataMessenger.Default.Send(new UpdateCargoPlan(), "Need to update cargo plan");
            _cargoPlan.RefreshCargoPlanValues();
        }

        /// <summary>
        /// Method works with SetTool to remove reefers from CargoPlan in one action.
        /// </summary>
        /// <param name="reefersToRemove"></param>
        internal void RemoveSeveralReefers(List<ContainerWrapper> reefersToRemove)
        {
            bool removed = _messageDialogService.ShowYesNoDialog(
                       $"Do you want to remove {reefersToRemove.Count} reefers from list", "")
                   == MessageDialogResult.Yes;
            if (removed)
            {
                foreach (var reefer in reefersToRemove)
                {
                    _cargoPlan.RemoveReefer(reefer);

                    var _dgs = _cargoPlan.DgList.Where(x => x.ContainerNumber == reefer.ContainerNumber);
                    foreach (var dg in _dgs)
                    {
                        dg.Model.IsRf = false;
                        dg.RefreshIsRfProperty();
                    }

                    var container = _cargoPlan.Containers.FirstOrDefault(x => x.ContainerNumber == reefer.ContainerNumber);
                    if (container != null)
                    {
                        container.Model.IsRf = false;
                        container.RefreshIsRfProperty();
                    }

                }
                _cargoPlan.RefreshCargoPlanValues();
            }
        }

        private void SubscribeToMessenger()
        {
            DataMessenger.Default.Register<UpdateCargoPlan>(this, OnDgRemoveRequested, "Remove dg");
        }

        private CargoPlanWrapperHandler()
        {
            _cargoDataService = CargoDataService.GetCargoDataService();
            SubscribeToMessenger();
        }
    }
}
