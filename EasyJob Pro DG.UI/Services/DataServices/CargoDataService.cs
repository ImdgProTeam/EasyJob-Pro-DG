using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Provides WorkingCargoPlan.
    /// </summary>
    public class CargoDataService : ICargoDataService
    {
        ICurrentProgramData _currentProgramData => CurrentProgramData.GetCurrentProgramData();

        #region Singleton

        /// <summary>
        /// Provides access to the service.
        /// </summary>
        /// <returns>A reference to the service instance.</returns>
        public static CargoDataService GetCargoDataService()
        {
            return _instance;
        }
        
        private static CargoDataService _instance = new CargoDataService();
        #endregion


        public string ConditionFileName => _currentProgramData.ConditionFileName;
        public CargoPlanWrapper WorkingCargoPlan { get; private set; }


        /// <summary>
        /// Creates new <see cref="CargoPlanWrapper"/> from <see cref="CurrentProgramData"/> <see cref="CargoPlan"/>
        /// </summary>
        public void GetCargoPlan()
        {
            WorkingCargoPlan?.Dispose();
            WorkingCargoPlan = new CargoPlanWrapper(_currentProgramData.CargoPlan);
        }

        #region Constructor

        private CargoDataService()
        {
        }

        #endregion
    }
}
