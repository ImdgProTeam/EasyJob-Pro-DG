namespace EasyJob_ProDG.Model.Cargo
{
    internal static class DgConflictMethods
    {
        #region Conflict methods
        /// <summary>
        /// Method creates dg.Conflict if not yet created
        /// </summary>
        private static void AddConflict(this Dg dg)
        {
            dg.Conflicts ??= new Conflicts();
        }

        /// <summary>
        /// Adds Stowage or Segregation conflict to Dg.
        /// </summary>
        /// <param name="stowOrSegr">"stowage" or "segregation"</param>
        /// <param name="code">string conflict code</param>
        /// <param name="b">conflicting Dg in case of segregatin conflict/></param>
        public static void AddConflict(this Dg dg, string stowOrSegr, string code, Dg b = null)
        {
            dg.AddConflict(true, stowOrSegr, code, b);
        }

        /// <summary>
        /// Method adds conflict to Dg if 'add' is 'true'. Stowage or segregation conflict will be added as defined. Method combines other conflict related methods in Dg class to reduce nesting.
        /// </summary>
        /// <param name="add"></param>
        /// <param name="stoworsegr"></param>
        /// <param name="code"></param>
        /// <param name="b"></param>
        internal static void AddConflict(this Dg dg, bool add, string stoworsegr, string code, Dg b = null)
        {
            if (!add) return;
            dg.AddConflict();
            if (stoworsegr == "stowage") dg.Conflicts.AddStowConflict(code);
            else dg.Conflicts.AddSegrConflict(code, b);
        }

        /// <summary>
        /// Method clears all conflicts in dg unit and changes 'conflicted' status to false
        /// </summary>
        public static void ClearAllConflicts(this Dg dg)
        {
            if (!dg.IsConflicted) return;
            dg.Conflicts.SegregationConflictsList.Clear();
            dg.Conflicts.StowageConflictsList.Clear();
        }
        #endregion
    }
}
