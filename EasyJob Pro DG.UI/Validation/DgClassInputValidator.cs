using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.UI.Validation
{
    internal static class DgClassInputValidator
    {
        /// <summary>
        /// Method validates <see cref="inputValue"/> to be a valid Dg class and returns <see langword="true"/> if valid.
        /// </summary>
        /// <param name="inputValue">Dg class to be validated.</param>
        /// <param name="outputValidValue">Corrected output valid Dg class.</param>
        /// <returns></returns>
        internal static bool Validate(string inputValue, out string outputValidValue)
        {
            outputValidValue = inputValue.Trim()
                .Replace(" ", "")
                .Replace(",", ".")
                .ToUpper();

            if (!char.IsDigit(outputValidValue[0])) return false;
            // no dg class length is more than 4 symbols
            if (outputValidValue.Length > 4) return false;
            //if length is 4 -> there should be a '.' and a letter at the end (1.4s)
            if (outputValidValue.Length > 3 && !char.IsLetter(outputValidValue[3]) && outputValidValue[1] != '.') return false;
            //if length is 3 -> there shall be a period or a letter at the end (5.1 or 14s)
            if (outputValidValue.Length > 2 && (outputValidValue[1] != '.' && !char.IsLetter(outputValidValue[2]))) return false;


            //if length is 2 -> period is skipped
            if (outputValidValue.Length > 1 && char.IsDigit(outputValidValue[1]))
                outputValidValue = outputValidValue.Insert(1, ".");

            //checking if compatibility group is valid
            if (outputValidValue.Length > 3
                && (!DgClassValidator.IsCompatibilityGoupContainedInIMDGCode(outputValidValue[3])
                || !outputValidValue.StartsWith("1")))
                outputValidValue = outputValidValue.Substring(0, 3);

            if (!DgClassValidator.IsClassContainedInIMDGCode(outputValidValue))
                return false;
            return true;
        }
    }
}
