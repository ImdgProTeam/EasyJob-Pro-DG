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
            if (char.IsLetter(c))
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
                    if (_dgclass.Length != 2 || _dgclass[1] != '.') return false;
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

        /// <summary>
        /// Returns true if the <see cref="dgClass"/> and its compatibility group (in case of class 1) are found in IMDGCode dictionary 
        /// </summary>
        /// <param name="dgClass"></param>
        /// <returns></returns>
        public static bool IsClassAndCompatibilityGroupContainedInIMDGCode(string dgClass)
        {
            if(!RoughCheckValidation(dgClass)) return false;

            string _dgClass = dgClass.ParseDgClass();
            char _compatibilityGroup = dgClass.ParseCompatibilityGroup();

            return IMDGCode.AllValidDgClasses.Contains(_dgClass)
                && !_dgClass.StartsWith("1") || IsCompatibilityGoupContainedInIMDGCode(_compatibilityGroup);
        }

        private static string ParseDgClass(this string dgClass)
        {
            return dgClass.Length > 3
                ? dgClass.Substring(0, 3)
                : dgClass;
        }

        private static char ParseCompatibilityGroup(this string dgClass)
        {
            return dgClass.Length > 3 && dgClass.StartsWith("1")
                ? dgClass[3]
                : '0';
        }

        private static bool RoughCheckValidation(string dgClass)
        {
            if (dgClass.Length > 4) return false;
            if(dgClass.Length > 3 && !dgClass.StartsWith("1")) return false;
            if(dgClass.Length > 3 && !char.IsLetter(dgClass[3])) return false;
            return true;
        }

        /// <summary>
        /// Returns true if the <see cref="dgClass"/> and is found in IMDGCode dictionary.
        /// Compatibility group of class 1 will be ignored
        /// </summary>
        /// <param name="dgClass"></param>
        /// <returns></returns>
        public static bool IsClassContainedInIMDGCode(string dgClass)
        {
            string _dgClass = dgClass.ParseDgClass();
            return IMDGCode.AllValidDgClasses.Contains(_dgClass);
        }

        /// <summary>
        /// Returns true if <see cref="group"/> as compatibility group of class 1 is contained in IMDG code.
        /// </summary>
        /// <param name="group">Compatibility group of DG of class 1</param>
        /// <returns></returns>
        public static bool IsCompatibilityGoupContainedInIMDGCode(char group)
        {
            return IMDGCode.AllValidCompatibilityGroupsOfClass1.Contains(group);
        }
    }
}
