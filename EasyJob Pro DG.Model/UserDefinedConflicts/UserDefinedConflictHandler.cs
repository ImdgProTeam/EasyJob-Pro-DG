namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    /// <summary>
    /// Extends functionality of <see cref="UserDefinedConflict"/>
    /// </summary>
    internal static class UserDefinedConflictHandler
    {
        /// <summary>
        /// Adds containerNumber to the <see cref="UserDefinedConflict"/> ContainerNumbers, if not already listed
        /// </summary>
        /// <param name="conflict"></param>
        /// <param name="containerNumber"></param>
        internal static void AddMultiUnitConflictContainerNumber(this UserDefinedConflict conflict, string containerNumber)
        {
            if (!conflict.ContainerNumbers.Contains(containerNumber))
                conflict.ContainerNumbers.Add(containerNumber);
        }

        /// <summary>
        /// Adds dgID to the <see cref="UserDefinedConflict"/> DgIDs, if not already listed
        /// </summary>
        /// <param name="conflict"></param>
        /// <param name="dgID"></param>
        internal static void AddMultiUnitConflictDgID(this UserDefinedConflict conflict, string dgID)
        {
            if (!conflict.DgIDs.Contains(dgID))
                conflict.DgIDs.Add(dgID);
        }
    }
}
