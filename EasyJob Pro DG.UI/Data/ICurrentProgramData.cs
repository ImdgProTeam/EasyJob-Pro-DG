using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.UI.Data
{
    /// <summary>
    /// Provides program data access to current <see cref="CargoPlan"/>, <see cref="ShipProfile"/> and condition file name in use.
    /// </summary>
    internal interface ICurrentProgramData
    {
        string ConditionFileName { get; }
        CargoPlan CargoPlan { get; }

        CargoPlan GetCargoPlan();
        ShipProfile GetShipProfile();

        void SetConditionFileName(string name);
        void ApendConditionFileNameWithImported();
        void SetCargoPlan(CargoPlan cargoPlan);
    }
}
