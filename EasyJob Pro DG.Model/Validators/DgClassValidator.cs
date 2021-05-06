using EasyJob_ProDG.Data.Info_data;
using System.Collections.Generic;

namespace EasyJob_ProDG.Model.Cargo
{
    public static class DgClassValidator
    {
        private static string _dgclass;

        /// <summary>
        /// Checks if next input character will remain dg class valid.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsValidDgClass(char c)
        {
            //check for absence of any letters except comptibility groups
            if(char.IsLetter(c))
                if (_dgclass != null && !_dgclass.StartsWith("1.") && _dgclass.Length != 3)
                    return false;
                else
                    return true;

            //checking for the first digit to be one of possible classes
            if (char.IsDigit(c) && c != '0' && string.IsNullOrEmpty(_dgclass))
            {
                _dgclass += c;
                return true;
            }

            //work with subdivisions
            switch (c)
            {
                case '.':
                    if (_dgclass.Length > 1) return false;
                    if (_dgclass[0] != '1' && _dgclass[0] != '2' && _dgclass[0] != '4' && _dgclass[0] != '5' &&
                        _dgclass[0] != '6')
                        return false;
                    _dgclass += c;
                    return true;

                case '1':
                case '2':
                case '3':
                    if ( _dgclass.Length != 2 || _dgclass[1] != '.') return false;
                    _dgclass += c;
                    return IsValidDgClass(_dgclass);
                case '4':
                case '5':
                case '6':
                    if (_dgclass != "1.") return false;
                    _dgclass += c;
                    return IsValidDgClass(_dgclass);
            }

            return false;
        }

        /// <summary>
        /// Checks if dg class exists in IMDG code.
        /// Compatibility group will be ignored.
        /// </summary>
        /// <param name="dgClass"></param>
        /// <returns></returns>
        public static bool IsValidDgClass(string dgClass)
        {
            //removing letters
            List<char> letters = new List<char>();
            foreach (var c in dgClass)
                if (char.IsLetter(c))
                {
                    //checking as letters only applicable to class 1
                    if (!dgClass.StartsWith("1.")) return false;
                    letters.Add(c);
                }
            foreach (var c in letters)
                dgClass = dgClass.Replace(c.ToString(), "");

            return IMDGCode.AllValidDgClasses.Contains(dgClass);
        }

        /// <summary>
        /// Checks if a subdivision is valid for class
        /// </summary>
        /// <param name="dgPrimarClass">Main class</param>
        /// <param name="dgSubDivision">Subdivision being checked</param>
        /// <returns>True if subdivision is valid</returns>
        private static bool IsValidDivision(string dgPrimarClass, string dgSubDivision)
        {
            List<string> subgroups;
            switch (dgPrimarClass)
            {
                case "1":
                    subgroups = new List<string>()
                    {
                        "1", "2", "3", "4", "5", "6"
                    };
                    if (subgroups.Contains(dgSubDivision))
                        return true;
                    break;

                case "2":
                case "4":
                    subgroups = new List<string>()
                    {
                        "1", "2", "3"
                    };
                    if (subgroups.Contains(dgSubDivision))
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
                    if (subgroups.Contains(dgSubDivision))
                        return true;
                    break;

            }

            return false;
        }

        /// <summary>
        /// Checks if a subdivision is valid for class
        /// </summary>
        /// <param name="dgPrimarClass">Main class</param>
        /// <param name="dgSubDivision">Subdivision being checked</param>
        /// <returns>True if subdivision is valid</returns>
        public static bool IsValidDivision(char dgPrimarClass, char dgSubDivision)
        {
            return IsValidDivision(dgPrimarClass.ToString(), dgSubDivision.ToString());
        }

        /// <summary>
        /// Resets validator to enable next validation from the beginning
        /// (erases all symbols from validator)
        /// </summary>
        public static void ResetDgValidator()
        {
            _dgclass = null;
        }

        /// <summary>
        /// Adds a symbol to validator memory
        /// </summary>
        /// <param name="c"></param>
        public static void AddChar(char c)
        {
            _dgclass += c;
        }
    }
}
