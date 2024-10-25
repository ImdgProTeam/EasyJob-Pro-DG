using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel;

namespace EasyJob_ProDG.UI.Data
{
    public class ConflictsList : AsyncObservableCollection<ConflictPanelItemViewModel>
    {

        /// <summary>
        /// Method add a new ConflictPanelItem to ConflictList, if it does not already exist
        /// </summary>
        /// <param name="conf"></param>
        public void AddNewConflict(ConflictPanelItemViewModel conf)
        {
            if (Contains(conf)) return;
            Add(conf);
        }

        /// <summary>
        /// Sets the rule to check weather a conflict exists in the list.
        /// </summary>
        /// <param name="conflict">ConflictPanelItem to be checked</param>
        /// <returns></returns>
        public new bool Contains(ConflictPanelItemViewModel conflict)
        {
            foreach (var con in this)
            {
                if (con.DgID == conflict.DgID
                    && con.Location == conflict.Location
                    && con.Code == conflict.Code
                    && con.Unno == conflict.Unno
                    && con.IsStowageConflict == conflict.IsStowageConflict)
                {
                    if (con.IsSegregationConflict)
                    {
                        if (conflict.IsSegregationConflict
                            && con.ConflictingDgNumber == conflict.ConflictingDgNumber
                            && con.ConflictingDgLocation == conflict.ConflictingDgLocation
                            && con.ConflictingDgUnno == conflict.ConflictingDgUnno)
                            return true;
                        continue;
                    }
                    return true;
                }
            }
            return false;
        }

    }
}
