using System.Linq;

namespace EasyJob_ProDG.Model.Validators
{
    /// <summary>
    /// Validates UNNo to be existing in DgDataBase
    /// </summary>
    public static class UnnoValidator
    {
        /// <summary>
        /// Will check if <see cref="unno"/> exists in DgDataBase.
        /// </summary>
        /// <param name="unno">UNNo to be checked</param>
        /// <returns>True if UNNo is valid</returns>
        public static bool Validate(int unno)
        {
            var entries = (from entry in ProgramFiles.DgDataBase.Descendants("DG")
                           where (int)entry.Attribute("unno") == unno
                           select entry);

            return entries.Any();
        }
    }
}
