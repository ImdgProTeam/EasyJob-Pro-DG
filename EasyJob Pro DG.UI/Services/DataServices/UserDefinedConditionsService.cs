using EasyJob_ProDG.Model.UserDefinedConflicts;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Provides access to <see cref="UserDefinedConditionRepository"/> via <see cref="UserDefinedConditionRepositoryHandler"/>
    /// </summary>
    public class UserDefinedConditionsService : IUserDefinedConditionService
    {
        private UserDefinedConditionRepositoryHandler _repositoryHandler;
        private ObservableCollection<UserDefinedConditionWrapper> _conditions;

        public ObservableCollection<UserDefinedConditionWrapper> GetConditionWrappers
        {
            get
            {
                CreateConditions();
                return _conditions;
            }
        }


        public bool AddCondition(UserDefinedConditionWrapper conditionWrapper, out bool alreadyExists)
        {
            if (_repositoryHandler.AddCondition(conditionWrapper.Model, out alreadyExists))
            {
                _conditions.Add(conditionWrapper);
                return true;
            }
            return false;
        }

        public bool RemoveCondition(UserDefinedConditionWrapper conditionWrapper)
        {
            if (_repositoryHandler.RemoveCondition(conditionWrapper.Model))
            {
                _conditions.Remove(conditionWrapper);
                return true;
            }
            return false;
        }

        public void ClearConditions()
        {
            _repositoryHandler.ClearRepository();
            _conditions.Clear();
        }

        public void SaveConditions()
        {
            _repositoryHandler.SaveRepository();
        }

        /// <summary>
        /// Provides access to the service.
        /// </summary>
        /// <returns></returns>
        public static UserDefinedConditionsService GetService()
        {
            return _instance ??= new UserDefinedConditionsService();
        }

        private void CreateConditions()
        {
            if (_conditions == null)
            {
                _conditions = new ObservableCollection<UserDefinedConditionWrapper>();
                foreach (var condition in _repositoryHandler.GetConditions)
                {
                    _conditions.Add(new UserDefinedConditionWrapper(condition));
                }
            }
        }

        #region Singleton constructor

        static UserDefinedConditionsService _instance;

        private UserDefinedConditionsService()
        {
            _repositoryHandler = UserDefinedConditionRepositoryHandler.GetService();
        }

        #endregion
    }
}
