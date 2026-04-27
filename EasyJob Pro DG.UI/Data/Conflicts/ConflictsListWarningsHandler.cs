using EasyJob_ProDG.Model;
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
                GeneralConflictPanelItemViewModel conflict =
                    new GeneralConflictPanelItemViewModel(message.Title, message.Message, "", ConflictTypes.Info);
                if (conflict != null)
                    conflictsList.Add(conflict);
            }
        }
    }
}
