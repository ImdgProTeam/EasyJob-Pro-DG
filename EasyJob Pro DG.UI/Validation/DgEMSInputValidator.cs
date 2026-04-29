namespace EasyJob_ProDG.UI.Validation
{
    internal class DgEMSInputValidator
    {
        /// <summary>
        /// Validates EMS input and amends 'input' param to correct format value
        /// </summary>
        /// <param name="originalValue">Original value of EMS before change</param>
        /// <param name="input">User input that will be converted to correct format.</param>
        /// <returns>True if succesfully validated input value and amended to correct format.</returns>
        internal static bool Validate(string originalValue, ref string input)
        {
            //empty string - not ok
            if (string.IsNullOrEmpty(input))
                return false;

            //input according to display format - OK
            if (input.Length == 7 && input.StartsWith("F-") && input.Substring(4, 2) == "S-"
                && char.IsLetter(input[2]) && char.IsLetter(input[6]))
            {
                input = input.ToUpper();
                return true;
            }

            string value = input.Trim().Replace(" ", "").Replace("-", "").Replace(",", "").ToLower();

            //parsing 4 letter input
            if (value.Length == 4 && value[0] == 'f' && value[2] == 's'
                && char.IsLetter(value[1]) && char.IsLetter(value[3]))
            {
                input = GetFormattedEMS(value[1], value[3]);
                return true;
            }

            //parsing 2 letter input
            //fa -> F-A, sq -> S-Q
            if (value.Length == 2)
            {
                if (value[0] == 'f' && char.IsLetter(value[1]))
                {
                    input = originalValue.Remove(2, 1).Insert(2, value[1].ToString().ToUpper());
                    return true;
                }
                if (value[0] == 's' && char.IsLetter(value[1]))
                {
                    input = originalValue.Remove(6, 1).Insert(6, value[1].ToString().ToUpper());
                    return true;
                }
                if (char.IsLetter(value[0]) && char.IsLetter(value[1]))
                {
                    input = GetFormattedEMS(value[0], value[1]);
                    return true;
                }
            }

            //single letter
            if(value.Length == 1 && char.IsLetter(value[0]))
            {
                input = originalValue.Remove(2, 1).Insert(2, value[0].ToString().ToUpper());
                return true;
            }

            return false;
        }

        private static string GetFormattedEMS(char f, char s)
        {
            return GetFormattedEMS(f.ToString(), s.ToString());
        }

        private static string GetFormattedEMS(string f, string s)
        {
            return $"F-{f.ToUpper()} S-{s.ToUpper()}";
        }
    }
}
