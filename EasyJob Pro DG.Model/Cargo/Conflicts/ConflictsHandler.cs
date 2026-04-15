using static EasyJob_ProDG.Model.Cargo.Conflicts;

namespace EasyJob_ProDG.Model.Cargo
{
    internal static class ConflictsHandler
    {

        #region Add/Replace conflict logic
        //----------- Add/Remove methods -------------------------------------------------------------------

        // --- Methods to add/remove/replace conflicts in the list ---
        public static void AddStowConflict(this Conflicts conflict, string code)
        {
            if (!conflict.StowageConflictsList.Contains(code)) conflict.StowageConflictsList.Add(code);
        }
        public static void AddSegrConflict(this Conflicts conflict, string code, Dg unit)
        {
            if (!conflict.Contains(code, unit)) conflict.SegregationConflictsList.Add(new SegregationConflict(code, unit));
        }

        /// <summary>
        /// Clears all stowage conflicts
        /// </summary>
        /// <param name="dg"></param>
        public static void ClearStowageConflicts(this Conflicts conflict)
        {
            conflict.StowageConflictsList.Clear();
        }

        /// <summary>
        /// Emties mutual segregation conflicts from both dg SegregationConflictLists
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void RemoveSegregationConflict(Dg a, Dg b)
        {
            foreach (Conflicts.SegregationConflict conf in a.Conflicts.SegregationConflictsList)
            {
                if (conf.ConflictContainerNr != b.ContainerNumber) continue;
                //remove conflict from a
                a.Conflicts.SegregationConflictsList.Remove(conf);

                //remove conflict from b
                if (b.IsConflicted && b.Conflicts.Contains(a)) foreach (Conflicts.SegregationConflict bconf in b.Conflicts.SegregationConflictsList)
                    if (bconf.ConflictContainerNr == a.ContainerNumber)
                        b.Conflicts.SegregationConflictsList.Remove(bconf);
            }
        }

        /// <summary>
        /// Renews mutual segregation confilcts in both dg SegregationConflictLists with new code
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="newCode"></param>
        public static void ReplaceConflict(Dg a, Dg b, string newCode)
        {
            if (a.Conflicts == null) return;
            foreach (Conflicts.SegregationConflict conf in a.Conflicts.SegregationConflictsList)
                if (conf.ConflictContainerNr == b.ContainerNumber)
                {
                    conf.Code = newCode;
                    if (b.IsConflicted && b.Conflicts.Contains(a))
                        foreach (var bconf in b.Conflicts.SegregationConflictsList)
                            if (bconf.ConflictContainerNr == a.ContainerNumber) bconf.Code = newCode;
                }
        }

        /// <summary>
        /// Finds all segregation conflicts and replaces their codes with the newCode on both conflicted units.
        /// </summary>
        /// <param name="dg">Dg to search segregation conflicts in.</param>
        /// <param name="newCode">New code to be set for all conflicts.</param>
        internal static void ReplaceAllSegregationConflicts(Dg dg, string newCode)
        {
            if (dg.Conflicts == null) return;

            foreach (Conflicts.SegregationConflict conf in dg.Conflicts.SegregationConflictsList)
            {
                conf.Code = newCode;
                foreach (Conflicts.SegregationConflict mutualConflict in conf.DgInConflict.Conflicts.SegregationConflictsList)
                {
                    if (mutualConflict.ConflictContainerNr == dg.ContainerNumber)
                        mutualConflict.Code = newCode;
                }
            }
        }

        #endregion
    }
}