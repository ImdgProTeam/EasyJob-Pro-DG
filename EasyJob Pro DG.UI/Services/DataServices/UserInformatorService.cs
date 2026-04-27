using EasyJob_ProDG.Model;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    internal class UserInformatorService : IUserInformatorService
    {
        private static UserInformatorService _instance = new UserInformatorService();
        internal static UserInformatorService GetService() => _instance;

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

        private UserInformatorService()
        {
            
        }
    }
}
