using EasyJob_ProDG.UI.ViewModel.Conflicts;

namespace EasyJob_ProDG.UI.Utility.Messages
{
    internal class UserDefinedConditionRemoveMessage
    {
        internal PanelConflictItemUserDefinedViewModel conflictPanelItem;

        public UserDefinedConditionRemoveMessage(object obj)
        {
            conflictPanelItem = obj as PanelConflictItemUserDefinedViewModel;
        }
    }
}
