using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services
{
    /// <summary>
    /// Class to assist in handling various services complex sequences.
    /// </summary>
    public class ServicesHandler
    {
        private static ServicesHandler _instance;
        private ICargoPlanCheckService _cargoPlanCheckService;

        public static void Initiate()
        {
            _instance = new ServicesHandler();

        }

        /// <summary>
        /// Receives message to check CargoPlan and update ConflictsList.
        /// Uses messages to send respective notifications.
        /// </summary>
        /// <param name="obj"></param>
        private void OnConflictsToBeCheckedAndUpdatedMessageReceived(ConflictsToBeCheckedAndUpdatedMessage obj)
        {
            DgWrapper wrapper = obj.dgWrapper;
            if (wrapper is null)
            {
                _cargoPlanCheckService.CheckCargoPlan();
                DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage(obj.FullListToBeUpdated), "update conflicts");
            }
            else // if wrapper is passed, it means that only the unit stowage needs to be checked
            {
                _cargoPlanCheckService.CheckDgWrapperStowage(wrapper);
                DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage(wrapper), "update conflicts");
            }
            DataMessenger.Default.Send(new DgListSelectedItemUpdatedMessage());
        }


        #region Initializing setup private methods

        private void ConnectServices()
        {
            _cargoPlanCheckService = new CargoPlanCheckService();
        }

        private void RegisterInMessenger()
        {
            DataMessenger.Default.Unregister(_instance);
            DataMessenger.Default.Register<ConflictsToBeCheckedAndUpdatedMessage>(this, OnConflictsToBeCheckedAndUpdatedMessageReceived);
        } 
        #endregion

        #region Constructor

        private ServicesHandler()
        {
            if (_instance is null)
            {
                ConnectServices();
                RegisterInMessenger();
            }
        }


        #endregion
    }
}
