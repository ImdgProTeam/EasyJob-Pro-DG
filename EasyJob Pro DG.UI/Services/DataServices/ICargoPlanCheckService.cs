using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    internal interface ICargoPlanCheckService
    {
        void CheckCargoPlan();
        void CheckDgWrapperStowage(DgWrapper unit);
    }
}