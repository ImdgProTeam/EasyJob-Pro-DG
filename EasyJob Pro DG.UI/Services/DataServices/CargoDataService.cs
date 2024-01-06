using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    public class CargoDataService : ICargoDataService
    {
        private static CargoDataService _instance = new CargoDataService();
        public static CargoDataService GetCargoDataService()
        { 
            return _instance;
        }

        ICurrentProgramData _currentProgramData => CurrentProgramData.GetCurrentProgramData();

        public string ConditionFileName => _currentProgramData.ConditionFileName;
        public CargoPlanWrapper WorkingCargoPlan { get; private set; }


        /// <summary>
        /// Creates and returns new <see cref="CargoPlanWrapper"/> created from <see cref="CurrentProgramData"/> <see cref="CargoPlan"/>
        /// </summary>
        /// <returns><see cref="WorkingCargoPlan"/></returns>
        public CargoPlanWrapper GetCargoPlan()
        {
            WorkingCargoPlan?.Dispose();
            WorkingCargoPlan = new CargoPlanWrapper(_currentProgramData.CargoPlan);
            return WorkingCargoPlan;
        }

        #region Constructor

        private CargoDataService()
        {
        }

        #endregion
    }
}
