using System.Collections.Generic;

namespace EasyJob_ProDG.Model.Cargo
{
    internal static class HandleCargoPlanUpdates
    {
        /// <summary>
        /// Clears CargoPlan Updates - Loaded and Discharged containers lists.
        /// </summary>
        /// <param name="cargoPlan"></param>
        internal static void ClearUpdates(this CargoPlan cargoPlan)
        {
            cargoPlan.Updates.LoadedContainers = new List<Container>();
            cargoPlan.Updates.DischargedContainers = new List<Container>();
        }

        internal static void AddToLoaded(this CargoPlan cargoPlan, Container container)
        {
            cargoPlan.Updates.LoadedContainers.Add(container);
        }

        internal static void AddToDischarged(this CargoPlan cargoPlan, Container container)
        {
            cargoPlan.Updates.DischargedContainers.Add(container);
        }

    }
}
