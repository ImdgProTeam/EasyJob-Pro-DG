using EasyJob_ProDG.Model.UserDefinedConflicts;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Provides access to <see cref="UserDefinedConditionRepository"/> via <see cref="UserDefinedConditionRepositoryHandler"/>
    /// </summary>
    public class UserDefinedConditionsService : IUserDefinedConditionService
    {
        private UserDefinedConditionRepositoryHandler _repositoryHandler;

        public List<UserDefinedCondition> GetConditions =>
            _repositoryHandler.GetConditions;

        public bool AddCondition(UserDefinedCondition condition, out bool alreadyExists)
        {
            return _repositoryHandler.AddCondition(condition, out alreadyExists);
        }

        public bool RemoveCondition(UserDefinedCondition condition)
        {
            return _repositoryHandler.RemoveCondition(condition);
        }

        /// <summary>
        /// Provides access to the service.
        /// </summary>
        /// <returns></returns>
        public static UserDefinedConditionsService GetService()
        {
            return _instance ??= new UserDefinedConditionsService();
        }

        #region Sindleton constructor

        static UserDefinedConditionsService _instance;

        private UserDefinedConditionsService()
        {
            _repositoryHandler = UserDefinedConditionRepositoryHandler.GetService();
        }

        #endregion
    }
}
