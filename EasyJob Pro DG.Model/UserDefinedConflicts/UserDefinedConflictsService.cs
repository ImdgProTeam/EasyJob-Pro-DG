using System.Collections.Generic;

namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    public class UserDefinedConflictsService
    {
        List<UserDefinedConflict> conflicts;
        List<UserDefinedCondition> conditions;
        UserDefinedConflictsCheckService checkService;


        public List<UserDefinedConflict> GetConflicts => conflicts;
        public List<UserDefinedCondition> GetConditions => conditions;

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
        }
    }
}
