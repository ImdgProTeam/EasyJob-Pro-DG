using EasyJob_ProDG.Model.UserDefinedConflicts;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Provides interface to <see cref="UserDefinedCondition"/>s repository
    /// </summary>
    public interface IUserDefinedConditionService
    {
        /// <summary>
        /// Provides list of actual <see cref="UserDefinedCondition"/>s
        /// </summary>
        public List<UserDefinedCondition> GetConditions { get; }

        bool AddCondition(UserDefinedCondition condition, out bool alreadyExists);

        bool RemoveCondition(UserDefinedCondition condition);

    }
}