using EasyJob_ProDG.Model.UserDefinedConflicts;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        private void CreateConditions()
        {
            if (_conditions == null)
            {
                _conditions = new ObservableCollection<UserDefinedConditionWrapper>();
                foreach (var condition in _repositoryHandler.GetConditions)
                {
                    _conditions.Add(new UserDefinedConditionWrapper(condition));
                }
                RegisterCollection<UserDefinedConditionWrapper, UserDefinedCondition>(_conditions, _repositoryHandler.GetConditions);
            }
        }


        public bool AddCondition(UserDefinedConditionWrapper conditionWrapper, out bool alreadyExists)
        {
            return _repositoryHandler.AddCondition(conditionWrapper.Model, out alreadyExists);
        }

        public bool RemoveCondition(UserDefinedConditionWrapper conditionWrapper)
        {
            return _repositoryHandler.RemoveCondition(conditionWrapper.Model);
        }

        /// <summary>
        /// Provides access to the service.
        /// </summary>
        /// <returns></returns>
        public static UserDefinedConditionsService GetService()
        {
            return _instance ??= new UserDefinedConditionsService();
        }


        protected virtual void RegisterCollection<TWrapper, TModel>(ObservableCollection<TWrapper> wrapperCollection,
    List<TModel> modelCollection) where TWrapper : ModelWrapper<TModel>
        {
            wrapperCollection.CollectionChanged += (s, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems.Cast<TWrapper>())
                    {
                        modelCollection.Remove(item.Model);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems.Cast<TWrapper>())
                    {
                        modelCollection.Add(item.Model);
                    }
                }
            };
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
