using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    internal class ConflictDataService : IConflictDataService
    {
        private static readonly ConflictDataService _instance = new ConflictDataService();

        public static ConflictDataService GetConflictDataService()
        {
            return _instance;
        }
        
        ICargoDataService _cargoDataService => CargoDataService.GetCargoDataService(); 

        public ConflictsList Conflicts { get; private set; }
        public VentilationRequirements Vents { get; private set; }

        /// <summary>
        /// Creates <see cref="Conflicts"/> and <see cref="Vents"/>
        /// </summary>
        /// <returns><see cref="Conflicts"/></returns>
        ConflictsList IConflictDataService.GetConflicts()
        {
            //Display info
            Conflicts.CreateConflictList(_cargoDataService.WorkingCargoPlan.DgList);
            Vents.Check();
            return Conflicts;
        }

        VentilationRequirements IConflictDataService.GetVentilationRequirements()
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