using EasyJob_ProDG.Model;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Utility.Messages;
using System.Linq;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    internal class UserInformatorCommunicatorService : IUserInformatorCommunicatorService
    {
        private static UserInformatorCommunicatorService _instance = new UserInformatorCommunicatorService();
        internal static UserInformatorCommunicatorService GetService() => _instance;

        public void ClearGeneralMessages()
        {
            UserInformator.ClearMessages(InformationMessageType.General);
        }

        public void ClearReadingConditionMessages()
        {
            UserInformator.ClearMessages(InformationMessageType.ReadingCondition);
        }

        public void ClearShipProfileMessages()
        {
            UserInformator.ClearMessages(InformationMessageType.ShipProfile);
        }

        public void AddMessageReadingCondition(string header, string message)
        {
            UserInformator.AddMessage(new InformationMessage(header, message, InformationMessageType.ReadingCondition));
        }

        public void PreserveReadingConditionMessages()
        {
            UserInformator.PreserveReadingMessages();
        }

        public void RestoreReadingConditionMessages()
        {
            UserInformator.RestoreReadingMessages();
        }

        private UserInformatorCommunicatorService()
        {
            RegisterInMessenger();
        }

        private void RegisterInMessenger()
        {
            DataMessenger.Default.Register<RemoveWarningMessage>(this, OnRemoveWarningMessageReceived, DataMessengerContext.REMOVE_INFO_MESSAGE);
        }

        private void OnRemoveWarningMessageReceived(RemoveWarningMessage message)
        {
            var conflict = message.Conflict;
            var warning = UserInformator.GetMessages().FirstOrDefault(
                w => string.Equals(w.Title, conflict.DisplayConflictHeader)
                && string.Equals(w.Message, conflict.DisplayText));
            if (warning is null) return;
            UserInformator.RemoveMessage(warning);
        }
    }
}
