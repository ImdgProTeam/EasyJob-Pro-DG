using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Provides Conflicts and VentilationRequirements.
    /// </summary>
    internal interface IConflictDataService
    {
        ConflictsList GetConflicts();
        VentilationRequirements GetVentilationRequirements();
    }
}
