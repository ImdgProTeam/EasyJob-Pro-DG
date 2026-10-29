using EasyJob_ProDG.UI.ViewModel.Conflicts;

namespace EasyJob_ProDG.UI.Data.UserDefinedConflicts
{
    /// <summary>
    /// Extends <see cref="ConflictsList"/> to work with <see cref="UserDefinedConflict"/>s.
    /// </summary>
    internal static class UserDefinedConflictsListUpdater
    {
        private static UserDefinedConflictsList _conflictsRepository = UserDefinedConflictsList.GetAccess();

        /// <summary>
        /// Updates existing <see cref="ConflictsList"/>, handling <see cref="UserDefinedConflict"/>s only.
        /// Creates User-defined conflicts for new plan (isCollectionNew?).
        /// Updates list of existing user-defined conflicts for existing plan.
        /// </summary>
        /// <param name="conflicts"></param>
        /// <param name="isCollectionNew"></param>
        /// <returns></returns>
        internal static ConflictsList UpdateConflictsListWithUserDefinedConflicts(this ConflictsList conflicts, bool isCollectionNew)
        {
            if (isCollectionNew)
            {
                conflicts.RemoveAllUserDefinedConflicts();
                conflicts.CollectUserDefinedConflicts();
            }
            else
            {
                var tempConflicts = new ConflictsList();
                tempConflicts.CollectUserDefinedConflicts();
                conflicts.UpdateUserDefinedConflicts(tempConflicts);
            }
            return conflicts;
        }

        /// <summary>
        /// Collects <see cref="UserDefinedConflict"/>s from Repository.
        /// </summary>
        /// <param name="conflicts"></param>
        private static void CollectUserDefinedConflicts(this ConflictsList conflicts)
        {
            foreach (var conflict in _conflictsRepository.GetConflicts)
            {
                conflicts.AddNewConflict(conflict);
            }
        }

        /// <summary>
        /// Removes each conlict that is <see cref="PanelConflictItemUserDefinedViewModel"/>
        /// </summary>
        /// <param name="conflicts"></param>
        private static void RemoveAllUserDefinedConflicts(this ConflictsList conflicts)
        {
            for (int i = 0; i < conflicts.Count; i++)
            {
                if (conflicts[i] is PanelConflictItemUserDefinedViewModel)
                {
                    conflicts.RemoveAt(i);
                    i--;
                }
            }
        }

        private static void UpdateUserDefinedConflicts(this ConflictsList conflictsList, ConflictsList newConflictsList)
        {
            //Remove redundant conflicts
            for (int i = 0; i < conflictsList.Count; i++)
            {
                var conflict = conflictsList[i] as PanelConflictItemUserDefinedViewModel;

                //Check if reference unit removed from the list

                //General and Dg conflicts
                if (conflict is null)
                {
                    continue;
                }

                if (newConflictsList.Contains(conflict))
                    continue;

                //If not found - remove the conflict
                conflictsList.RemoveAt(i);
                i--;
            }

            //Add new conflicts
            foreach (PanelConflictItemUserDefinedViewModel conflict in newConflictsList)
            {
                conflictsList.AddNewConflict(conflict);
            }

        }
    }
}
