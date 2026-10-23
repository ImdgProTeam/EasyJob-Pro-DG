using EasyJob_ProDG.Model.UserDefinedConflicts;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Data.UserDefinedConflicts;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    internal class UserDefinedConflictsCheckService : IUserDefinedConflictsCheckService
    {
        // ----- Services and repositories references ----

        UserDefinedConflictsList _activeConflicts;
        UserDefinedConflictsChecker _checker;
        ICurrentProgramData _currentProgramData => CurrentProgramData.GetCurrentProgramData();
        UserDefinedConditionRepositoryHandler _conditionsRepositoryHandler;


        // ----- Public methods -----

        public void CheckUserDefinedConflicts()
        {
            var conflicts = _checker.CheckConflicts(_currentProgramData.CargoPlan, _conditionsRepositoryHandler.GetConditions);
            _activeConflicts.Clear();
            conflicts.ForEach(conflict => 
                            _activeConflicts.AddConflict(
                                new ViewModel.Conflicts.PanelConflictItemUserDefinedViewModel().CreatePanelConflictItemFrom(conflict)));
        }

        /// <summary>
        /// Provides access to the service.
        /// </summary>
        /// <returns></returns>
        public static UserDefinedConflictsCheckService GetService()
        {
            return _instance ??= new UserDefinedConflictsCheckService();
        }


        #region Singleton constructor
        // ----- Singleton constructor -----

        static UserDefinedConflictsCheckService _instance;

        private UserDefinedConflictsCheckService()
        {
            _activeConflicts = UserDefinedConflictsList.GetAccess();
            _checker = new UserDefinedConflictsChecker();
            _conditionsRepositoryHandler = UserDefinedConditionRepositoryHandler.GetService();
        }

        #endregion
    }
}
