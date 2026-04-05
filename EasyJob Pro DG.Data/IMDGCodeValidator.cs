using EasyJob_ProDG.Data.Info_data;

namespace EasyJob_ProDG.Data
{
    /// <summary>
    /// Class contains public methods to validate that values being checked are contained in IMDG Code.
    /// </summary>
    public static class IMDGCodeValidator
    {
        /// <summary>
        /// Checks if dgClass is a valid DG class as per IMDG Code.
        /// </summary>
        /// <param name="dgClass">Value of dg class to be checked</param>
        /// <returns>True if IMDG Code contains dgClass.</returns>
        public static bool IsValidDgClass(this string dgClass)
        {
            if (!char.IsDigit(dgClass[0])) return false;
            if (dgClass.Length == 1 || dgClass.Length == 3)
            {
                if (IMDGCode.AllValidDgClasses.Contains(dgClass))
                    return true;
                else return false;
            }
            if (dgClass.Length == 4 && dgClass[0] == '1')
            {
                if (!char.IsLetter(dgClass[3])) return false;
                if (!IMDGCode.AllValidCompatibilityGroupsOfClass1.Contains(dgClass.ToUpper()[3])) return false;
                if (IMDGCode.AllValidDgClasses.Contains(dgClass.Substring(0, 3)))
                    return true;
            }
            return false;
        }
    }
}
