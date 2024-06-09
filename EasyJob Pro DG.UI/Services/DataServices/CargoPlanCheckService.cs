using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Service to check stowage and segregation of <see cref="CargoPlanWrapper"/>
    /// </summary>
    internal class CargoPlanCheckService : ICargoPlanCheckService
    {
        ICurrentProgramData _currentProgramData => CurrentProgramData.GetCurrentProgramData();
        CargoPlan _cargoPlan => _currentProgramData.CargoPlan;

        /// <summary>
        /// Clears existing conflicts in cargoPlan, carries out Stowage and Segregation check.
        /// </summary>
        /// <param name="cargoPlan">Plain cargo plan (model)</param>
        public void CheckCargoPlan()
        {
            ReCheckDgList();
        }

        /// <summary>
        /// Clears existing conflicts in cargoPlan, carries out Stowage and Segregation check.
        /// When start program, when change dg properties, delete cargo, open new condition
        /// </summary>
        /// <param name="cargoPlan">Plain cargo plan (model)</param>
        private void ReCheckDgList()
        {
            //Clear all conflicts
            _cargoPlan.ClearConflicts();

            //Check stowage and segregation
            Stowage.CheckStowage(_cargoPlan);
            HandleDgList.CheckSegregation(_cargoPlan);
        }

        /// <summary>
        /// ReChecks and updates stowage conflicts in the selected unit
        /// </summary>
        /// <param name="unit"></param>
        public void CheckDgWrapperStowage(DgWrapper unit)
        {
            if (unit == null) return;
            ReCheckDgStowage(unit.Model, _currentProgramData.CargoPlan);
        }

        /// <summary>
        /// Checks stowage of a selected dg and updates its StowageConflictList
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="cargoPlan"></param>
        private void ReCheckDgStowage(Dg dg, CargoPlan cargoPlan)
        {
            dg.Conflicts?.ClearStowageConflicts();
            Stowage.CheckUnitStowage(dg, cargoPlan.Containers);
        }

    }
}
