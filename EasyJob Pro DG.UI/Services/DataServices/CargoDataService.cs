using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    public class CargoDataService : ICargoDataService
    {
        ICurrentProgramData currentProgramData = new CurrentProgramData();

        public CargoDataService()
        {
            //this.currentProgramData = currentProgramData;
        }

        public CargoPlanWrapper GetCargoPlan()
        {
            return currentProgramData.GetCargoPlan();
        }

        public void ReCheckDgList()
        {
            currentProgramData.ReCheckDgWrapperList();
        }

        public void ReCheckDgWrapperStowage(DgWrapper dgWrapper)
        {
            currentProgramData.ReCheckDgWrapperStowage(dgWrapper);
        }
    }
}
