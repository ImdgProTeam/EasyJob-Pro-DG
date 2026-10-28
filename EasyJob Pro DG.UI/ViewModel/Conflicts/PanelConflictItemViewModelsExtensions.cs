using System.Collections.Generic;

namespace EasyJob_ProDG.UI.ViewModel.Conflicts
{
    internal static class PanelConflictItemViewModelsExtensions
    {
        internal static List<string> CreateContainerNumbersListToSelect(this PanelConflictItemUserDefinedViewModel panelConflictItem)
        {
            var result = new List<string>();
            var conflict = panelConflictItem.GetConflict();
            if (conflict.IsMultiUnitConflict)
                result = conflict.ContainerNumbers;
            else
                result.Add(conflict.ContainerNumber);
            return result;
        }

        internal static List<string> CreateDgIdsListToSelect(this PanelConflictItemUserDefinedViewModel panelConflictItem)
        {
            var result = new List<string>();
            var conflict = panelConflictItem.GetConflict();
            if (conflict.IsMultiUnitConflict)
                conflict.DgIDs.ForEach(id => result.Add(id.ToString()));
            else
                result.Add(conflict.ContainerNumber);
            return result;
        }
    }
}
