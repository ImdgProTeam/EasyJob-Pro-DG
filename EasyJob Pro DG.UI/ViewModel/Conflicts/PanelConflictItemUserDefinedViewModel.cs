using EasyJob_ProDG.Model.UserDefinedConflicts;
using System;

namespace EasyJob_ProDG.UI.ViewModel.Conflicts
{
    public class PanelConflictItemUserDefinedViewModel : PanelConflictItemBaseViewModel
    {
        const string GROUP_TITLE_USER_DEFINED = "User-defined conflicts";

        public override string ConflictGroupTitle => GROUP_TITLE_USER_DEFINED;

        protected override string CreateDisplayText()
        {
            throw new NotImplementedException();
        }

        internal PanelConflictItemUserDefinedViewModel CreatePanelConflictItemFrom(UserDefinedConflict conflict)
        {
            return this;
        }
    }
}
