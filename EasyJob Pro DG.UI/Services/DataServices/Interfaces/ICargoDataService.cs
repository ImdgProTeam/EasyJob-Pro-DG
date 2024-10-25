using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Provides WorkingCargoPlan.
    /// </summary>
    public interface ICargoDataService
    {
        void GetCargoPlan();
        CargoPlanWrapper WorkingCargoPlan { get; }
        string ConditionFileName { get; }
    }
}
