using EasyJob_ProDG.Model.UserDefinedConflicts;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.Data.UserDefinedConflicts
{
    internal class UserDefinedConditionRepository
    {
        private List<UserDefinedCondition> conditions;
        public List<UserDefinedCondition> GetConditions => conditions;
        public bool IsEmpty => conditions.Count == 0;

        #region Methods

        /// <summary>
        /// Adds a <see cref="UserDefinedCondition"/> to the repository.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="alreadyExists">Returns true if the condition already exists in the repository.</param>
        /// <returns>True if the condition has succesfully added to or already exitsts in the repository. 
        /// False if failed to add.</returns>
        internal bool AddCondition(UserDefinedCondition condition, out bool alreadyExists)
        {
            alreadyExists = false;

            if (condition is null) 
                return false;

            if (conditions.Contains(condition))
            {
                alreadyExists = true;
                return true;
            }

            conditions.Add(condition);
            if (conditions.Contains(condition)) return true;
            return false;
        }

        /// <summary>
        /// Removes <see cref="UserDefinedCondition"/> from the repository.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>True if succesfully removed.</returns>
        internal bool RemoveCondition(UserDefinedCondition condition)
        {
            if (!conditions.Contains(condition)) return false;

            conditions.Remove(condition);
            if (!conditions.Contains(condition)) return true;
            return false;
        } 

        /// <summary>
        /// Clears the repository.
        /// </summary>
        internal void Clear()
        {
            conditions.Clear();
        }

        /// <summary>
        /// Copies all conditions from sourceRepository.
        /// No checks performed.
        /// </summary>
        /// <param name="sourceRepository"></param>
        internal void CopyRepository(UserDefinedConditionRepository sourceRepository)
        {
            foreach (UserDefinedCondition condition in sourceRepository.GetConditions)
            {
                conditions.Add(condition);
            }
        }

        #endregion

        internal UserDefinedConditionRepository()
        {
            conditions = new List<UserDefinedCondition>();
        }
    }
}
