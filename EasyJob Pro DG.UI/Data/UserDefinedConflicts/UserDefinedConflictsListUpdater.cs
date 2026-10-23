using System;

namespace EasyJob_ProDG.UI.Data.UserDefinedConflicts
{
    /// <summary>
    /// Extends <see cref="ConflictsList"/> to work with <see cref="UserDefinedConflict"/>s.
    /// </summary>
    internal static class UserDefinedConflictsListUpdater
    {
        private static UserDefinedConflictsList _conflictsRepository = UserDefinedConflictsList.GetAccess();

        /// <summary>
        /// Creates User-defined conflicts for new plan (isCollectionNew?).
        /// Updates list of existing user-defined conflicts for existing plan.
        /// </summary>
        /// <param name="conflicts"></param>
        /// <param name="isCollectionNew"></param>
        /// <returns></returns>
        internal static ConflictsList CheckUserDefinedConflicts(this ConflictsList conflicts, bool isCollectionNew)
        {
            if (isCollectionNew)
            {
                conflicts.RemoveAllUserDefinedConflicts();
                conflicts.CreateUserDefinedConflicts();
            }
            else
            {
                var tempConflicts = new ConflictsList();
                tempConflicts.CreateUserDefinedConflicts();
                conflicts.UpdateUserDefinedConflicts();
            }
            return conflicts;
        }


        private static ConflictsList CreateUserDefinedConflicts(this ConflictsList conflicts)
        {
            throw new NotImplementedException();
        }

        private static ConflictsList RemoveAllUserDefinedConflicts(this ConflictsList conflicts)
        {
            throw new NotImplementedException();
        }

        private static ConflictsList UpdateUserDefinedConflicts(this ConflictsList conflicts)
        {
            throw new NotImplementedException();
        }
    }
}
