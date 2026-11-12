using EasyJob_ProDG.Model;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel.Conflicts;

namespace EasyJob_ProDG.UI.Data
{
    /// <summary>
    /// Extension class for <see cref="ConflictsList"/> to handle <see cref="InformationMessage"/>s from <see cref="UserInformator"/> and other warnings.
    /// </summary>
    internal static class ConflictsListWarningsHandler
    {
        /// <summary>
        /// Creates conflicts from <see cref="UserInformator"/> and adds to <see cref="ConflictsList"/>
        /// </summary>
        /// <param name="conflictsList"></param>
        internal static void CreateWarnings(this ConflictsList conflictsList)
        {
            foreach (var message in UserInformator.GetMessages())
            {
                PanelConflictItemGeneralViewModel conflict =
                    new PanelConflictItemGeneralViewModel(message.Title, message.Message, "", ConflictTypes.Info);
                conflict.AssignAction(message);
                if (conflict != null)
                    conflictsList.Add(conflict);
            }
        }

        /// <summary>
        /// Assigns Actions if required to particular <see cref="PanelConflictItemGeneralViewModel"/>
        /// </summary>
        /// <param name="conflict"></param>
        /// <param name="message"></param>
        private static void AssignAction(this PanelConflictItemGeneralViewModel conflict, InformationMessage message)
        {
            switch (message.MessageType)
            {
                case InformationMessageType.General:
                    break;
                case InformationMessageType.ShipProfile:
                    conflict.AssignedAction = ViewModelLocator.MainWindowViewModel.OpenShipProfileWindow;
                    break;
                case InformationMessageType.ReadingCondition:
                    break;
                case InformationMessageType.All:
                    break;
            }
        }

    }
}
