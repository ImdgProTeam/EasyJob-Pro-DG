using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    internal class ConflictDataService : IConflictDataService
    {
        ICurrentProgramData currentProgramData => CurrentProgramData.GetCurrentProgramData();

        public ConflictDataService()
        {
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
