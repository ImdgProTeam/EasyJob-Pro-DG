using System;
using EasyJob_Pro_DG.View.Services;
using Excel = Microsoft.Office.Interop.Excel;

namespace EasyJob_Pro_DG
{
    /// <summary>
    /// Class created to assist in reading excel cell values avoiding exceptions and mistakes
    /// </summary>
    static class WithXLAssistToRead
    {
        /// <summary>
        /// Method reads fp, corrects the errors/typoes, translate F into C
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        internal static float DGPF(Excel.Range cells)
        {
            float _dgfp;
            string result = null;
            try
            {
                string units = "C";
                string value = Convert.ToString(cells.Value2);
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

                _dgfp = (float)Math.Round(Convert.ToDouble(result), 2);
                if (units == "F") _dgfp = (float)Math.Round(AdditionalFunctions.ToCelcium(_dgfp), 2);

            }
            catch (Exception)
            {
                _dgfp = 9999;
            }

            return _dgfp;

        }

        /// <summary>
        /// Metho reads DG class and helps to avoid exceptions and also validates the result
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="dgunit"></param>
        /// <param name="excelapp"></param>
        /// <returns></returns>
        internal static string DGclass(Excel.Range cells, Dg dgunit, Excel.Application excelapp)
        {
            string value = Convert.ToString(cells.Value2);
            string result = "";
                    
            Validator.ResetDgValidator();

            foreach (char c in value)
            {
                if (result.Length >= 4)
                    break;

                if (char.IsDigit(c))
                {
                    if(string.IsNullOrEmpty(result) || result.Length == 2 && result[1] == '.')
                            result += c;

                    //ignore all the digits beyond the valid length
                    else if (result.Length == 3) break;


                    //case if more then 2 digits - will ignore remaining
                    else if (result.Length == 1)
                    {
                        if (Validator.IsValidSubclass(result[0], c))
                        {
                            result += "." + c;
                            Validator.AddChar('.'); Validator.AddChar(c);
                        }
                        continue;
                    }

                    if(!Validator.IsValidDgClass(c))
                        MessageInvalidClass(result, dgunit, cells, excelapp);

                    continue;

                }

                if (char.IsPunctuation(c))
                {
                    if (string.IsNullOrEmpty(result)) continue;
                    if (result.Contains(".")) continue;
                    if (result.Length != 1) continue;

                    if (c == '.') result += c;
                    if (c == ',') result += '.';
                    if(!Validator.IsValidDgClass('.'))
                        MessageInvalidClass(result, dgunit, cells, excelapp);
                    continue;
                }

                if (char.IsLetter(c))
                {
                    if (!result.StartsWith("1.") || result.Length != 3) continue;
                    result += char.ToUpper(c);
                    if(!Validator.IsValidDgClass(c))
                        MessageInvalidClass(result, dgunit, cells, excelapp);
                }
            }

            var _dgclass = result;
            return _dgclass;
        }

        /// <summary>
        /// Metho reads DG subclasses and helps to avoid exceptions and also validates the result
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="dgunit"></param>
        /// <param name="excelapp"></param>
        /// <returns></returns>
        internal static string[] DGsubclass(Excel.Range cells, Dg dgunit, Excel.Application excelapp)
        {
            string value = Convert.ToString(cells.Value2);
            if (string.IsNullOrEmpty(value)) return new string[0];
            string result = null;
            char lastsymbol = '&', prelastsymbol = '&', nextsymbol = '&';

            Validator.ResetDgValidator();

            
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
                { if (char.IsDigit(lastsymbol))
                    {
                        //exclude multiple subclasses followed one by one, e.g. 2.2.2.2
                        if (prelastsymbol == '.')
                        {
                            result += " " + c;
                            continue;
                        }
                        if (Validator.IsValidSubclass(lastsymbol, c))
                            result += "." + c;
                        else result += " " + c;
                        continue;
                    }
                    else if (lastsymbol == '.' || lastsymbol == ' ')
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
                        if ((c == '.' || c == ',') && Validator.IsValidSubclass(lastsymbol, nextsymbol))
                            result += ".";
                        else result += " ";
                    }

                    continue;
                }
                #endregion

#region Letters and seprators

                 if (char.IsLetter(c))
                {
                    if (result.Length>=3 && result.Substring(result.Length-3,2) == "1.")
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
            var _dgsubclasses = result.Split(' ');

            foreach (var dgclass in _dgsubclasses)
                if(!Validator.IsValidDgClass(dgclass))
                    MessageInvalidClass(dgclass, dgunit, cells, excelapp);
            return _dgsubclasses;
        }

        /// <summary>
        /// Method displays message about wrong dg and give an option to continue or terminate
        /// </summary>
        /// <param name="_dgclass"></param>
        /// <param name="_dgunit"></param>
        /// <param name="_cell"></param>
        /// <param name="_excelapp"></param>
        private static void MessageInvalidClass(string _dgclass, Dg _dgunit, Excel.Range _cell, Excel.Application _excelapp)
        {
            IMessageDialogService message = new MessageDialogService();
            if(message.ShowOKCancelDialog(@"While reading Excel file an error in dg class of unit {0} 
                                        unno {1} has been detected. Please check your excel file, 
                                        record at row {0} column {1}. Would you like to continue? y/n", "Caution!") == MessageDialogResult.Cancel)
            { _excelapp.Quit();
              //TO INPUT METHOD TO CLOSE THE APP
            }
        }


    }
    }





