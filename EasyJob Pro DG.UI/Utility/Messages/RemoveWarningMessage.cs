using EasyJob_ProDG.UI.ViewModel;

namespace EasyJob_ProDG.UI.Utility.Messages
{
    internal class RemoveWarningMessage
    {
        internal PanelConflictItemBaseViewModel Conflict;

        public RemoveWarningMessage(PanelConflictItemBaseViewModel conflict)
        {
            Conflict = conflict;
        }
    }
}
