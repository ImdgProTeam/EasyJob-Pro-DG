using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Provides Conflicts and VentilationRequirements.
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
        public VentilationRequirements Vents { get; private set; }

        /// <summary>
        /// Creates <see cref="Conflicts"/> and <see cref="Vents"/>
        /// </summary>
        /// <returns><see cref="Conflicts"/></returns>
        public ConflictsList GetConflicts()
        {
            //Display info
            Conflicts.CreateConflictList(_cargoDataService.WorkingCargoPlan.DgList);
            Vents.Check();
            return Conflicts;
        }

        public VentilationRequirements GetVentilationRequirements()
        {
            return Vents;
        }


        #region Constructor

        private ConflictDataService()
        {
            Conflicts = new();
            Vents = new();
        } 

        #endregion
    }
}