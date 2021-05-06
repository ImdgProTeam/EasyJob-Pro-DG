using System.Collections.Generic;

namespace EasyJob_Pro_DG
{
    public static class Validator
    {
        private static string dgclass;

        /// <summary>
        /// Checks if next input character will remain dg class valid.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsValidDgClass(char c)
        {
            //check for absence of any letters except comptibility groups
            if(char.IsLetter(c))
                if (dgclass != null && !dgclass.StartsWith("1.") && dgclass.Length != 3)
                    return false;
                else
                    return true;

            //checking for the first digit to be one of possible classes
            if (char.IsDigit(c) && c != '0' && string.IsNullOrEmpty(dgclass))
            {
                dgclass += c;
                return true;
            }

            //work with subdivisions
            switch (c)
            {
                case '.':
                    if (dgclass.Length > 1) return false;
                    if (dgclass[0] != '1' && dgclass[0] != '2' && dgclass[0] != '4' && dgclass[0] != '5' &&
                        dgclass[0] != '6')
                        return false;
                    dgclass += c;
                    return true;

                case '1':
                case '2':
                case '3':
                    if ( dgclass.Length != 2 || dgclass[1] != '.') return false;
                    dgclass += c;
                    return IsValidDgClass(dgclass);
                case '4':
                case '5':
                case '6':
                    if (dgclass != "1.") return false;
                    dgclass += c;
                    return IsValidDgClass(dgclass);
            }

            return false;
        }

        /// <summary>
        /// Checks if dg class exists in IMDG code.
        /// Compatibility group will be ignorred.
        /// </summary>
        /// <param name="dg"></param>
        /// <returns></returns>
        public static bool IsValidDgClass(string dg)
        {
            //removing letters
            List<char> letters = new List<char>();
            foreach (var c in dg)
                if (char.IsLetter(c))
                {
                    //checking as letters only applicable to class 1
                    if (!dg.StartsWith("1.")) return false;
                    letters.Add(c);
                }
            foreach (var c in letters)
                dg = dg.Replace(c.ToString(), "");

            return IMDGCode.AllValidDgClasses.Contains(dg);
        }

        public static bool IsValidSubclass(string dgprimarclass, string dgsubclass)
        {
            List<string> subgroups;
            switch (dgprimarclass)
            {
                case "1":
                    subgroups = new List<string>()
                    {
                        "1", "2", "3", "4", "5", "6"
                    };
                    if (subgroups.Contains(dgsubclass))
                        return true;
                    break;

                case "2":
                case "4":
                    subgroups = new List<string>()
                    {
                        "1", "2", "3"
                    };
                    if (subgroups.Contains(dgsubclass))
                        return true;
                    break;
                case "3":
                case "7":
                case "8":
                case "9":
                    return false;
                case "5":
                case "6":
                    subgroups = new List<string>()
                    {
                        "1", "2"
                    };
                    if (subgroups.Contains(dgsubclass))
                        return true;
                    break;

            }

            return false;
        }

        public static bool IsValidSubclass(char dgprimarclass, char dgsubclss)
        {
            return IsValidSubclass(dgprimarclass.ToString(), dgsubclss.ToString());
        }

        public static void ResetDgValidator()
        {
            dgclass = null;
        }

        public static void AddChar(char c)
        {
            dgclass += c;
        }
    }
}
