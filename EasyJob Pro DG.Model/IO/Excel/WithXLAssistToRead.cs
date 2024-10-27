using System;
using System.Linq;
using EasyJob_ProDG.Data;
using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.Model.IO.Excel
{
    /// <summary>
    /// Class created to assist in reading excel cell values to avoid exceptions and mistakes.
    /// </summary>
    static class WithXlAssistToRead
    {
        /// <summary>
        /// Method reads fp, corrects the errors/typos, translates F into C.
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        internal static decimal DgFp(string value)
        {
            decimal dgfp;
            string result = null;
            try
            {
                string units = "C";
                foreach (char c in value)
                {
                    if (char.IsDigit(c))
                    {
                        result += c;
                        continue;
                    }

                    if (char.IsPunctuation(c))
                    {
                        if (c == '.' || c == '-') result += c;
                        if (c == ',') result += '.';
                        continue;
                    }

                    if (char.IsLetter(c))
                    {
                        if (char.ToLower(c) == 'f')
                            units = "F";
                    }
                }

                dgfp = Math.Round(Convert.ToDecimal(result), 2);
                if (units == "F") dgfp = Math.Round(dgfp.ToCelcium(), 2);

            }
            catch (Exception)
            {
                dgfp = 9999;
            }
            return dgfp;
        }

        /// <summary>
        /// Method reads DG class and helps to avoid exceptions and also validates the result.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dgUnit"></param>
        /// <returns></returns>
        internal static string DgClass(string value, string containerNumber)
        {
            string result = "";

            DgClassValidator.ResetDgValidator();

            foreach (char c in value)
            {
                if (result.Length >= 4)
                    break;

                if (char.IsDigit(c))
                {
                    if (string.IsNullOrEmpty(result) || result.Length == 2 && result[1] == '.')
                        result += c;

                    //ignore all the digits beyond the valid length
                    else if (result.Length == 3) break;


                    //case if more then 2 digits - will ignore remaining
                    else if (result.Length == 1)
                    {
                        if (DgClassValidator.IsValidDivision(result[0], c))
                        {
                            result += "." + c;
                            DgClassValidator.AddChar('.'); DgClassValidator.AddChar(c);
                        }
                        continue;
                    }

                    if (!DgClassValidator.IsValidDgClass(c))
                        MessageInvalidClass(result, containerNumber);
                    continue;
                }

                if (char.IsPunctuation(c))
                {
                    if (string.IsNullOrEmpty(result)) continue;
                    if (result.Contains(".")) continue;
                    if (result.Length != 1) continue;

                    if (c == '.') result += c;
                    if (c == ',') result += '.';
                    if (!DgClassValidator.IsValidDgClass('.'))
                        MessageInvalidClass(result, containerNumber);
                    continue;
                }

                if (char.IsLetter(c))
                {
                    if (!result.StartsWith("1.") || result.Length != 3) continue;
                    result += char.ToUpper(c);
                    if (!DgClassValidator.IsValidDgClass(c))
                        MessageInvalidClass(result, containerNumber);
                }
            }

            var dgclass = result;
            return dgclass;
        }

        /// <summary>
        /// Method reads DG subclasses and helps to avoid exceptions and also validates the result.
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="dgUnit"></param>
        /// <param name="excelApp"></param>
        /// <returns></returns>
        internal static string[] DgSubClass (string value, string containerNumber)
        {
            if (string.IsNullOrEmpty(value)) return new string[0];
            string result = null;
            char lastsymbol = '&', prelastsymbol = '&', nextsymbol = '&';

            DgClassValidator.ResetDgValidator();


            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];

                //dealing with an empty result string
                if (string.IsNullOrEmpty(result))
                {
                    if (char.IsDigit(c))
                        result += c;
                    continue;

                }

                if (!string.IsNullOrEmpty(result))
                    lastsymbol = result[result.Length - 1];
                if (result.Length >= 2)
                    prelastsymbol = result[result.Length - 2];
                if (value.Length > i + 1)
                    nextsymbol = value[i + 1];

                #region Digits
                //digits
                if (char.IsDigit(c))
                {
                    if (char.IsDigit(lastsymbol))
                    {
                        //exclude multiple subclasses followed one by one, e.g. 2.2.2.2
                        if (prelastsymbol == '.')
                        {
                            result += " " + c;
                            continue;
                        }
                        if (DgClassValidator.IsValidDivision(lastsymbol, c))
                            result += "." + c;
                        else result += " " + c;
                        continue;
                    }
                    if (lastsymbol == '.' || lastsymbol == ' ')
                        result += c;
                    continue;
                }
                #endregion

                #region Punctuation

                //punctuation
                if (char.IsPunctuation(c))
                {
                    //exclude multiple subclasses followed one by one, e.g. 2.2.2.2
                    if (result.Length >= 2 && result.Substring(result.Length - 2, 1) == ".")
                        continue;

                    if (char.IsDigit(nextsymbol))
                    {
                        if ((c == '.' || c == ',') && DgClassValidator.IsValidDivision(lastsymbol, nextsymbol))
                            result += ".";
                        else result += " ";
                    }

                    continue;
                }
                #endregion

                #region Letters and seprators

                if (char.IsLetter(c))
                {
                    if (result.Length >= 3 && result.Substring(result.Length - 3, 2) == "1.")
                        result += char.ToUpper(c) + (char.IsDigit(nextsymbol) ? " " : "");
                    continue;
                }

                if (char.IsSeparator(c))
                {
                    if (!result.EndsWith(" "))
                        result += " ";
                }
                #endregion
            }



            if (string.IsNullOrEmpty(result)) return new string[0];
            var dgsubclasses = result.Split(' ');

            foreach (var dgclass in dgsubclasses)
                if (!DgClassValidator.IsValidDgClass(dgclass))
                    MessageInvalidClass(dgclass, containerNumber);
            return dgsubclasses;
        }


        /// <summary>
        /// Parsing string value in Remark column and updates the properties of dg unit and the 'closed' property of container.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <param name="cont"></param>
        internal static void ParseRemarkColumn(string value, Dg unit, Container cont)
        {
            //TODO: Method can be drastically improved
            bool matched = false;
            string tempValue = Convert.ToString(value).ToLower().Replace("\t", ",").Replace("\n", ",")
                .Replace(", ", ",").Replace("\\", ",").Replace("/", ",").Trim();
            string[] temparray = tempValue.Split(',');
            foreach (string x in temparray)
            {
                switch (x)
                {
                    case "flammable":
                        unit.IsFlammable = true;
                        matched = true;
                        break;
                    case "liquid":
                        unit.IsLiquid = true;
                        matched = true;
                        break;
                    case "lq":
                        unit.IsLq = true;
                        matched = true;
                        break;
                    case "limited quantity":
                        unit.IsLq = true;
                        matched = true;
                        break;
                    case "ltd qty":
                        unit.IsLq = true;
                        matched = true;
                        break;
                    case "emitting flammable vapours":
                        unit.IsEmitFlammableVapours = true;
                        matched = true;
                        break;
                    case "rf":
                        cont.IsRf = true;
                        break;
                    default:
                        break;
                }

                if (x.Contains("open"))
                {
                    cont.IsClosed = false;
                    matched = true;
                }

                if (IMDGCode.SegregationGroups.Contains(x))
                {
                    unit.SegregationGroup = x;
                    matched = true;
                }

                if (IMDGCode.SegregationGroups.Contains(x + 's'))
                {
                    unit.SegregationGroup = x + 's';
                    matched = true;
                }

                if (IMDGCode.SegregationGroups.Contains(x + "es"))
                {
                    unit.SegregationGroup = x + "es";
                    matched = true;
                }
            }

            if (matched) return;

            //Presumed separation with spaces
            temparray = tempValue.Split(' ');
            int i = 0;
            foreach (string x in temparray)
            {
                switch (x)
                {
                    case "flammable":
                        if (temparray[i].Contains("emit")) continue;
                        unit.IsFlammable = true;
                        continue;
                    case "liquid":
                        unit.IsLiquid = true;
                        continue;
                    case "lq":
                        unit.IsLq = true;
                        continue;
                    case "limited":
                        unit.IsLq = true;
                        continue;
                    case "ltd":
                        unit.IsLq = true;
                        continue;
                    case "emitting":
                        unit.IsEmitFlammableVapours = true;
                        continue;
                    case "emit":
                        unit.IsEmitFlammableVapours = true;
                        continue;
                    default:
                        break;
                }

                if (x.Contains("open"))
                {
                    cont.IsClosed = false;
                    continue;
                }
                if (IMDGCode.SegregationGroups.Contains(x))
                {
                    unit.SegregationGroup = x;
                    continue;
                }

                if (IMDGCode.SegregationGroups.Contains(x + 's'))
                {
                    unit.SegregationGroup = x + 's';
                    continue;
                }

                if (IMDGCode.SegregationGroups.Contains(x + "es"))
                {
                    unit.SegregationGroup = x + "es";
                    continue;
                }
                switch (x)
                {
                    case "ammonium":
                        unit.SegregationGroup = "ammonium compounds";
                        continue;
                    case "heavy":
                        unit.SegregationGroup = "heavy metals and their salts";
                        continue;
                    case "lead":
                        unit.SegregationGroup = "lead and its compounds";
                        continue;
                    case "halogenated":
                        unit.SegregationGroup = "liquid halogenated hydrocarbons";
                        continue;
                    case "mercury":
                        unit.SegregationGroup = "mercury and mercury compounds";
                        continue;
                    case "nitrites":
                        unit.SegregationGroup = "nitrites and their mixtures";
                        continue;
                    case "powdered":
                        unit.SegregationGroup = "powdered metals";
                        continue;
                    case "strong":
                        unit.SegregationGroup = "strong acids";
                        continue;
                    default:
                        break;
                }
                i++;
            }

            unit.Remarks = value;
        }

        /// <summary>
        /// Method displays message about wrong dg and give an option to continue or terminate.
        /// </summary>
        /// <param name="dgClass"></param>
        /// <param name="dgUnit"></param>
        /// <param name="cell"></param>
        /// <param name="excelApp"></param>
        private static void MessageInvalidClass
            (string dgClass, string containerNumber)
        {
            //TODO: To be implemented
            //IMessageDialogService message = new MessageDialogService();
            //if(message.ShowOKCancelDialog(@"While reading Excel file an error in dg class of unit {0} 
            //                            unno {1} has been detected. Please check your excel file, 
            //                            record at row {0} column {1}. Would you like to continue? y/n", "Caution!") == MessageDialogResult.Cancel)
            //{ _excelapp.Quit();
            //  //TO INPUT METHOD TO CLOSE THE APP
            //}
            Output.ThrowMessage("Method to deal with invalid class not implemented.");
        }
    }
}





