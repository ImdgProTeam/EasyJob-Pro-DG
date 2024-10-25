using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Checks stowage and segregation of <see cref="CargoPlanWrapper"/> or stowage of a <see cref="DgWrapper"/> unit.
    /// </summary>
    internal interface ICargoPlanCheckService
    {
        void CheckCargoPlan();
        void CheckDgWrapperStowage(DgWrapper unit);
    }
}