using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel;
using EasyJob_ProDG.UI.ViewModel.Conflicts;
using System.Linq;

namespace EasyJob_ProDG.UI.Data
{
    public class ConflictsList : AsyncObservableCollection<PanelConflictItemBaseViewModel>
    {

        /// <summary>
        /// Method add a new ConflictPanelItem to ConflictList, if it does not already exist
        /// </summary>
        /// <param name="conf"></param>
        public void AddNewConflict(PanelConflictItemDgViewModel conflict)
        {
            if (Contains(conflict)) return;
            Add(conflict);
        }

        public void AddNewConflict(PanelConflictItemBaseViewModel conflict)
        {
            if (conflict is PanelConflictItemDgViewModel)
                AddNewConflict(conflict as PanelConflictItemDgViewModel);
            else
            {
                if (!Contains(conflict))
                    Add(conflict);
            }
        }

        /// <summary>
        /// Sets the rule to check weather a conflict exists in the list.
        /// </summary>
        /// <param name="conflict">ConflictPanelItem to be checked</param>
        /// <returns></returns>
        public bool Contains(PanelConflictItemDgViewModel conflict)
        {
            return this.Any(c => c is PanelConflictItemDgViewModel && c.Equals(conflict));
            //foreach (PanelConflictItemBaseViewModel confl in this)
            //{
            //    var con = confl as PanelConflictItemDgViewModel;
            //    if (con is null) continue;

            //    if (con.DgID == conflict.DgID
            //        && con.Location == conflict.Location
            //        && con.Code == conflict.Code
            //        && con.Unno == conflict.Unno
            //        && con.IsStowageConflict == conflict.IsStowageConflict)
            //    {
            //        if (con.IsSegregationConflict)
            //        {
            //            if (conflict.IsSegregationConflict
            //                && con.ConflictingDgNumber == conflict.ConflictingDgNumber
            //                && con.ConflictingDgLocation == conflict.ConflictingDgLocation
            //                && con.ConflictingDgUnno == conflict.ConflictingDgUnno)
            //                return true;
            //            continue;
            //        }
            //        return true;
            //    }
            //}
            //return false;
        }

        public bool Contains(PanelConflictItemBaseViewModel conflict)
        {
            if (conflict is PanelConflictItemDgViewModel)
                return Contains(conflict as PanelConflictItemDgViewModel);

            return this.Any(c => c.Equals(conflict));
        }
    }
}
