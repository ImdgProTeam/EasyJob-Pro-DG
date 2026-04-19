using System.Collections.Generic;
using System.Windows.Threading;

namespace EasyJob_ProDG.UI.Data.Conflicts
{
    internal static class ConflictsListFilter
    {
        // ---------------- Public constructors -------------------------------------
        static ConflictsListFilter()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
        }
        private static Dispatcher dispatcher;

        //internal static void FilterByConflictType(this ConflictsList conflictsList, ICollection<ConflictTypes> conflictTypes)
        //{
        //    dispatcher.Invoke(() =>
        //    {
        //        foreach (var item in conflictsList)
        //        {
        //            item.IsVisible = conflictTypes.Contains(item.ConflictType);
        //        }
        //    });
        //}

        //internal static void ClearFilter(this ConflictsList conflictsList)
        //{
        //    dispatcher.Invoke(() =>
        //    {
        //        foreach (var item in conflictsList)
        //            item.IsVisible = true;
        //    });
        //}
    }
}
