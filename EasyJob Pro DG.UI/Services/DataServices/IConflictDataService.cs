using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    internal interface IConflictDataService
    {
        ConflictsList GetConflicts();
        VentilationRequirements GetVentilationRequirements();
    }
}
