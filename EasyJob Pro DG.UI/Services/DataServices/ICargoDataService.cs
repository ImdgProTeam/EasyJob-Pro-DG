using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    public interface ICargoDataService
    {
        CargoPlanWrapper GetCargoPlan();
        CargoPlanWrapper WorkingCargoPlan { get; }
        string ConditionFileName { get; }
    }
}
