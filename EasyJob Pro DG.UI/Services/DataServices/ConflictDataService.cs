using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Provides Conflicts
    /// </summary>
    internal class ConflictDataService : IConflictDataService
    {
        ICargoDataService _cargoDataService => CargoDataService.GetCargoDataService();

        #region Singleton

        /// <summary>
        /// Provides access to the service.
        /// </summary>
        /// <returns></returns>
        public static ConflictDataService GetConflictDataService()
        {
            return _instance;
        }

        private static readonly ConflictDataService _instance = new ConflictDataService();

        #endregion

        public ConflictsList Conflicts { get; private set; }

        /// <summary>
        /// Creates <see cref="Conflicts"/> and <see cref="Vents"/>
        /// </summary>
        /// <returns><see cref="Conflicts"/></returns>
        public ConflictsList GetConflicts()
        {
            Conflicts.CreateConflictList(_cargoDataService.WorkingCargoPlan.DgList);
            return Conflicts;
        }


        #region Constructor

        private ConflictDataService()
        {
            Conflicts = new();
        }

        #endregion
    }
}