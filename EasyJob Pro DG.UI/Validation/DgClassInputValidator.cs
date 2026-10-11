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
            string tempOutputValue;
            outputValidValue = string.Empty;

            tempOutputValue = inputValue.Trim()
                .Replace(" ", "")
                .Replace(",", ".")
                .ToUpper();

            if (!char.IsDigit(tempOutputValue[0])) return false;
            // no dg class length is more than 4 symbols
            if (tempOutputValue.Length > 4) return false;
            //if length is 4 -> there should be a '.' and a letter at the end (1.4s)
            if (tempOutputValue.Length > 3 && !char.IsLetter(tempOutputValue[3]) && tempOutputValue[1] != '.') return false;
            //if length is 3 -> there shall be a period or a letter at the end (5.1 or 14s)
            if (tempOutputValue.Length > 2 && (tempOutputValue[1] != '.' && !char.IsLetter(tempOutputValue[2]))) return false;


            //if length is 2 -> period is skipped
            if (tempOutputValue.Length > 1 && char.IsDigit(tempOutputValue[1]))
                tempOutputValue = tempOutputValue.Insert(1, ".");

            //checking if compatibility group is valid
            if (tempOutputValue.Length > 3
                && (!DgClassValidator.IsCompatibilityGoupContainedInIMDGCode(tempOutputValue[3])
                || !tempOutputValue.StartsWith("1")))
                tempOutputValue = tempOutputValue.Substring(0, 3);

            if (!DgClassValidator.IsClassContainedInIMDGCode(tempOutputValue))
                return false;

            outputValidValue = tempOutputValue;
            return true;
        }
    }
}
