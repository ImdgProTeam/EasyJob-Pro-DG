using System.Collections.Generic;

namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    /// <summary>
    /// Contains Lists of <see cref="UserDefinedConflict"/> and <see cref="UserDefinedCondition"/> to be manipulated by users.
    /// </summary>
    public class UserDefinedConflictsService
    {
        List<UserDefinedConflict> conflicts;
        List<UserDefinedCondition> conditions;
        UserDefinedConflictsCheckService checkService;


        public List<UserDefinedConflict> GetConflicts => conflicts;
        public List<UserDefinedCondition> GetConditions => conditions;


        private void CheckConflicts()
        {
            conflicts = checkService.CheckConflicts(null, conditions);
        }

        #region Add/remove methods

        public void AddCondition(UserDefinedCondition condition)
        {
            if (!conditions.Contains(condition))
                conditions.Add(condition);
        }

        public void RemoveCondition(UserDefinedCondition condition)
        {
            if (conditions.Contains(condition))
                conditions.Remove(condition);
        }

        public void AddConflict(UserDefinedConflict conflict)
        {
            if (!conflicts.Contains(conflict))
                conflicts.Add(conflict);
        }

        public void RemoveConflict(UserDefinedConflict conflict)
        {
            if (conflicts.Contains(conflict))
                conflicts.Remove(conflict);
        } 

        #endregion

        public UserDefinedConflictsService()
        {
            conditions = new List<UserDefinedCondition>();
            conflicts = new List<UserDefinedConflict>();
            checkService = new UserDefinedConflictsCheckService();
        }
    }
}
