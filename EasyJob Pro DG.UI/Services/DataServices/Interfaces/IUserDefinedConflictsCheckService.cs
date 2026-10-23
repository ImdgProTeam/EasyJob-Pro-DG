namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Checks conflicts and updates active conflicts repository.
    /// </summary>
    internal interface IUserDefinedConflictsCheckService
    {
        /// <summary>
        /// Checks User-defined conflicts againt loaded user-defined conditions and updates active conflicts repository.
        /// </summary>
        void CheckUserDefinedConflicts();
    }
}