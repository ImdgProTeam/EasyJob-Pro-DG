using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    internal static class UserInformationMessageService
    {
        static UserInformatorCommunicatorService _informator => UserInformatorCommunicatorService.GetService();

        internal static void SendCannotReadMessage(MessageTypes messageType, string fileName = "")
        {

            _informator.RestoreReadingConditionMessages();
            switch (messageType)
            {
                case MessageTypes.None:
                default:
                    break;
                case MessageTypes.CannotOpenFile:
                    _informator.AddMessageReadingCondition("Cannot open file", $"Opening file '{fileName}' caused an error.");
                    break;
                case MessageTypes.CannotReadDgInfo:
                    _informator.AddMessageReadingCondition("Cannot read DG info", $"Reading DG information from file '{fileName}' caused an error.");
                    break;
                case MessageTypes.CannotReadReeferInfo:
                    _informator.AddMessageReadingCondition("Cannot read reefer manifest", $"Reading excel file '{fileName}' caused an error.");
                    break;
            }
            DataMessenger.Default.Send(new DisplayConflictsToBeRefreshedMessage(), DataMessengerContext.UPDATE_INFORMATION_MESSAGES);
        }

        internal static void SendMessage(MessageTypes messageType, string fileName = "")
        {
            switch (messageType)
            {
                case MessageTypes.None:
                default:
                    break;
                case MessageTypes.BlankConditionCreated:
                    _informator.ClearReadingConditionMessages();
                    _informator.AddMessageReadingCondition("Blank condition created", "Could not open the last working conditon. Blank condition created.");
                    break;
                case MessageTypes.CannotReadDgManifestFromExcel:
                    _informator.AddMessageReadingCondition("Could not read Dg manifest info from Excel", $"Reading Dg data from '{fileName}' caused error.");
                    break;
                case MessageTypes.CannotReadReeferManifestFromExcel:
                    _informator.AddMessageReadingCondition("Could not read reefer manifest info from Excel", $"Reading Reefer data from '{fileName}' caused error.");
                    break;

            }
        }

        internal enum MessageTypes
        {
            None = 0,
            CannotOpenFile = 1,
            CannotReadDgInfo = 2,
            CannotReadReeferInfo = 3,
            CannotReadDgManifestFromExcel = 4,
            CannotReadReeferManifestFromExcel = 5,

            BlankConditionCreated = 6,

        }
    }
}
