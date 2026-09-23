using EasyJob_ProDG.Model.IO.UserDefinedConditions;
using EasyJob_ProDG.UI.Data.UserDefinedConflicts;
using System.Collections.Generic;

namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    /// <summary>
    /// Responsible for handling of <see cref="UserDefinedConditionRepository"/>
    /// </summary>
    public class UserDefinedConditionRepositoryHandler
    {
        private UserDefinedConditionRepository _repository;

        public List<UserDefinedCondition> GetConditions => _repository.GetConditions;

        /// <summary>
        /// Adds <see cref="UserDefinedCondition"/> to the repository.
        /// Causes repository to be saved in UDC file.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="alreadyExists">True if the condition already exists in the repository.</param>
        /// <returns>True if the conditions has been succesfully added to or already exists in the repository.</returns>
        public bool AddCondition(UserDefinedCondition condition, out bool alreadyExists)
        {
            if (_repository.AddCondition(condition, out alreadyExists))
            {
                UDC_IO.CreateUDCFile(_repository);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the <see cref="UserDefinedCondition"/> from the repository.
        /// Causes repository to be saved in UDC file.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool RemoveCondition(UserDefinedCondition condition)
        {
            if (_repository.RemoveCondition(condition))
            {
                UDC_IO.CreateUDCFile(_repository);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clears all conditions from the repository.
        /// </summary>
        public void ClearRepository()
        {
            _repository.Clear();
            UDC_IO.CreateUDCFile(_repository);
        }

        #region Singleton

        private static UserDefinedConditionRepositoryHandler _instance;

        /// <summary>
        /// Provides access to the service.
        /// </summary>
        /// <returns></returns>
        public static UserDefinedConditionRepositoryHandler GetService()
        {
            _instance ??= new UserDefinedConditionRepositoryHandler();
            return _instance;
        }

        private UserDefinedConditionRepositoryHandler()
        {
            InitiateRepository();
        }

        private void InitiateRepository()
        {
            _repository = new UserDefinedConditionRepository();
            var repositoryFromFile = UDC_IO.ReadUDCFile();
            if (repositoryFromFile != null && !repositoryFromFile.IsEmpty)
            {
                _repository.CopyRepository(repositoryFromFile);
            }
        }

        #endregion
    }
}
