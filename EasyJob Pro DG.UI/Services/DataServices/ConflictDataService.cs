using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    internal class ConflictDataService : IConflictDataService
    {
        ICurrentProgramData currentProgramData = new CurrentProgramData();

        public ConflictDataService()
        {
            this.currentProgramData = currentProgramData;
        }

        public ConflictsList GetConflicts()
        {
            return currentProgramData.GetConflictsList();
        }

        public VentilationRequirements GetVentilationRequirements()
        {
            return currentProgramData.GetVentilationRequirements();
        }

        public void ReCheckConflicts()
        {
            currentProgramData.ReCheckDgWrapperList();
        }
    }
}
