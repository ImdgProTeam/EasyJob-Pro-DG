using EasyJob_ProDG.Model.UserDefinedConflicts;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Provides interface to <see cref="UserDefinedCondition"/>s repository using wrappers.
    /// </summary>
    public interface IUserDefinedConditionService
    {
        /// <summary>
        /// Provides list of actual <see cref="UserDefinedCondition"/>s
        /// </summary>
        public ObservableCollection<UserDefinedConditionWrapper> GetConditionWrappers { get; }

        /// <summary>
        /// Adds condition to the repository
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="alreadyExists"></param>
        /// <returns></returns>
        bool AddCondition(UserDefinedConditionWrapper conditionWrapper, out bool alreadyExists);

        /// <summary>
        /// Removes condition from the repository
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool RemoveCondition(UserDefinedConditionWrapper conditionWrapper);

    }
}