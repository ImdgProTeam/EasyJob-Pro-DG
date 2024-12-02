using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.Wrapper
{
    /// <summary>
    /// Receives messages and accordingly manipulates loaded <see cref="CargoPlanWrapper"/>.
    /// </summary>
    internal class CargoPlanWrapperHandler
    {
        // ----------------- Private fields ------------------------------------
        private static CargoPlanWrapperHandler _handleCargoPlanWrapper;
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
