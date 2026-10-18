using EasyJob_ProDG.Model.UserDefinedConflicts;
using EasyJob_ProDG.UI.Validation;
using EasyJob_ProDG.UI.Wrapper;
using EasyJob_ProDG.UI.Wrapper.UserDefinedConflicts;
using System.Linq;

namespace EasyJob_ProDG.UI.View.Windows.ToolWindows
{
    /// <summary>
    /// Class assists <see cref="UserDefinedConditionToolViewModel"/> to handle massive methods
    /// </summary>
    internal static class UserDefinedConditionsToolViewModelHelpers
    {
        #region Text constants
        // ----- Text constants -----

        const string ENTER_VALID_POSITION = "Enter valid position, e.g. 220182 or hold 1";
        const string ENTER_CONTAINER_NUMBER = $"Enter correct container number or a part of it";
        const string ENTER_PORT_CODE = $"Enter port code";
        const string ENTER_VALUE = $"Enter value";

        // ----- Default symbols or text to display with numeric values having options
        const string DEFAULT_LESS_THAN = "<";
        const string DEFAULT_LESS_THAN_OR_EQUAL = "<=";
        const string DEFAULT_MORE_THAN = ">";
        const string DEFAULT_MORE_THAN_OR_EQUAL = ">=";

        #endregion


        #region AddConditionHint
        // ----- Add condition hint -----

        internal static string GenerateConditionHintText(int selectedConditionIndex, string conditionValue,
            CellPositionWrapper cellPosition, UserDefinedConditionOption conditionOption = 0)
        {
            string resultHintText = string.Empty;

            switch (selectedConditionIndex)
            {
                case (byte)UserDefinedConditionType.General:
                case (byte)UserDefinedConditionType.FreeText:
                //FreeText = 255 21
                case 21:
                    resultHintText = "Enter any text to display it in Conflict section.";
                    break;

                case (byte)UserDefinedConditionType.CellPositionLoaded:
                    if (cellPosition.IsEmpty)
                        resultHintText = ENTER_VALID_POSITION;
                    else
                        resultHintText = $"Container is loaded in {cellPosition}";
                    break;
                case (byte)UserDefinedConditionType.CellPositionEmpty:
                    if (cellPosition.IsEmpty)
                        resultHintText = ENTER_VALID_POSITION;
                    else
                        resultHintText = $"Container is discharged or not loaded in {cellPosition}";
                    break;
                case (byte)UserDefinedConditionType.CellPositionLoadedWithReefer:
                    if (cellPosition.IsEmpty)
                        resultHintText = ENTER_VALID_POSITION;
                    else
                        resultHintText = $"Reefer is loaded in {cellPosition}";
                    break;
                case (byte)UserDefinedConditionType.CellPositionLoadedWithDg:
                    if (cellPosition.IsEmpty)
                        resultHintText = ENTER_VALID_POSITION;
                    else
                        resultHintText = $"Dangerous cargo is loaded in {cellPosition}";
                    break;

                case (byte)UserDefinedConditionType.ContainerNumberLoaded:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_CONTAINER_NUMBER;
                    else resultHintText = $"{UserDefinedConditionDescription.ContainerNumberContainingValue(conditionValue)} is loaded"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;
                case (byte)UserDefinedConditionType.ContainerNumberDischarged:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_CONTAINER_NUMBER;
                    else resultHintText = $"{UserDefinedConditionDescription.ContainerNumberContainingValue(conditionValue)} is discharged or not loaded"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;

                case (byte)UserDefinedConditionType.ContainerLocationChanged:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_CONTAINER_NUMBER;
                    else resultHintText = $"Position of {UserDefinedConditionDescription.ContainerNumberContainingValue(conditionValue)} has changed";
                    break;
                case (byte)UserDefinedConditionType.ContainerPODchanged:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_CONTAINER_NUMBER;
                    else resultHintText = $"{UserDefinedConditionDescription.ContainerNumberContainingValue(conditionValue)} port of discharging has changed";
                    break;

                case (byte)UserDefinedConditionType.DgNameContainsLoaded:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_VALUE;
                    else
                        resultHintText = $"Dangerous cargo with '{conditionValue}' in its proper shipping name is loaded"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;
                case (byte)UserDefinedConditionType.DgTechnicalNameContains:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_VALUE;
                    else
                        resultHintText = $"Dangerous cargo with '{conditionValue}' in its technical name is loaded"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;
                case (byte)UserDefinedConditionType.ReeferCommodityContains:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_VALUE;
                    else
                        resultHintText = $"Reefer cargo with '{conditionValue}' in its commodity is loaded"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;


                case (byte)UserDefinedConditionType.DgClassLoaded:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_VALUE;
                    else
                        resultHintText = $"Dangerous cargo with class '{conditionValue}' is loaded"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;
                case (byte)UserDefinedConditionType.DgUnnoLoaded:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_VALUE;
                    else
                        resultHintText = $"Dangerous cargo with UNNO {ushort.Parse(conditionValue): 0000} is loaded"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;


                case (byte)UserDefinedConditionType.ReeferContainingDgLoaded:
                    resultHintText = $"Reefer containing dangerous goods is loaded"
                    + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;

                case (byte)UserDefinedConditionType.DgFPLoaded:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_VALUE;
                    else
                        resultHintText = $"Dangerous cargo with flash point {UserDefinedConditionDescription.DefineConditionOption(conditionOption)}{conditionValue}"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;
                case (byte)UserDefinedConditionType.ReeferSetPointLoaded:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_VALUE;
                    else
                        resultHintText = $"Reefer with set point {UserDefinedConditionDescription.DefineConditionOption(conditionOption)}{conditionValue}"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;

                case (byte)UserDefinedConditionType.POLUnitsOnboard:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_PORT_CODE;
                    else
                        resultHintText = $"Container with port of loading '{conditionValue}' is loaded"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;
                case (byte)UserDefinedConditionType.PODUnitsOnboard:
                    if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_PORT_CODE;
                    else
                        resultHintText = $"Container with port of discharging '{conditionValue}' is loaded"
                            + UserDefinedConditionDescription.AddCellPositionIfNotEmpty(cellPosition);
                    break;

                case (byte)UserDefinedConditionType.ContainerNumberNotInPosition:
                    if (HasNoEntry(conditionValue) && cellPosition.IsEmpty)
                        resultHintText = ENTER_CONTAINER_NUMBER + " and position";
                    else if (HasNoEntry(conditionValue))
                        resultHintText = ENTER_CONTAINER_NUMBER;
                    else if (cellPosition.IsEmpty)
                        resultHintText = ENTER_VALID_POSITION;
                    else
                        resultHintText = $"{UserDefinedConditionDescription.ContainerNumberContainingValue(conditionValue)} is not in {cellPosition}";
                    break;

                case (byte)UserDefinedConditionType.NoNetWeightUnitsOnboard:
                    resultHintText = $"Any dangerous cargo has no Net Weight value";
                    break;
                default:
                    resultHintText = $"Select an option below and type values in the text boxes";
                    break;
            }

            return resultHintText;
        }



        /// <summary>
        /// Checks if no recognized entry made to any value or cellposition fields.
        /// </summary>
        /// <param name="conditionValue"></param>
        /// <param name="cellPosition"></param>
        /// <returns></returns>
        private static bool HasNoEntry(string conditionValue)
        {
            if (string.IsNullOrWhiteSpace(conditionValue))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region TryParseValue

        /// <summary>
        /// Parses <see cref="EditableConditionValueText"/> according to the <see cref="SelectedCondition"/>.
        /// </summary>
        /// <returns>False if not parsed.</returns>
        internal static bool TryParseValue(this string inputValue,
                                                    int selectedConditionIndex,
                                                    out string correctValue,
                                                    out UserDefinedConditionOption conditionOption)
        {
            bool result = false;
            conditionOption = UserDefinedConditionOption.EqualTo;

            switch (selectedConditionIndex)
            {
                case (byte)UserDefinedConditionType.General:
                case (byte)UserDefinedConditionType.FreeText:
                //FreeText = 255 21
                case 21:
                    correctValue = inputValue;
                    if (!string.IsNullOrEmpty(inputValue))
                        result = true;
                    break;

                case (byte)UserDefinedConditionType.CellPositionLoaded:
                case (byte)UserDefinedConditionType.CellPositionEmpty:
                case (byte)UserDefinedConditionType.CellPositionLoadedWithReefer:
                case (byte)UserDefinedConditionType.CellPositionLoadedWithDg:
                case (byte)UserDefinedConditionType.ReeferContainingDgLoaded:
                case (byte)UserDefinedConditionType.NoNetWeightUnitsOnboard:
                    correctValue = string.Empty;
                    result = true;
                    break;

                case (byte)UserDefinedConditionType.DgNameContainsLoaded:
                case (byte)UserDefinedConditionType.DgTechnicalNameContains:
                case (byte)UserDefinedConditionType.ReeferCommodityContains:
                    correctValue = inputValue.ToUpper();
                    if (!string.IsNullOrEmpty(inputValue))
                        result = true;
                    break;

                case (byte)UserDefinedConditionType.ContainerNumberLoaded:
                case (byte)UserDefinedConditionType.ContainerNumberDischarged:
                case (byte)UserDefinedConditionType.ContainerLocationChanged:
                case (byte)UserDefinedConditionType.ContainerPODchanged:
                case (byte)UserDefinedConditionType.ContainerNumberNotInPosition:
                    result = inputValue.TryParseContainerNumber(out correctValue);
                    break;


                case (byte)UserDefinedConditionType.DgClassLoaded:
                    result = inputValue.TryParseDgClass(out correctValue);
                    break;

                case (byte)UserDefinedConditionType.DgUnnoLoaded:
                    result = inputValue.TryParseUnno(out correctValue);
                    break;

                case (byte)UserDefinedConditionType.DgFPLoaded:
                case (byte)UserDefinedConditionType.ReeferSetPointLoaded:
                    result = inputValue.TryParseTempPoint
                        (out correctValue, out conditionOption);
                    break;

                case (byte)UserDefinedConditionType.POLUnitsOnboard:
                case (byte)UserDefinedConditionType.PODUnitsOnboard:
                    result = inputValue.TryParsePortCode(out correctValue);
                    break;

                default:
                    correctValue = string.Empty;
                    result = false;
                    break;
            }

            return result;
        }

        private static bool TryParseTempPoint(this string inputValue, out string correctValue, out UserDefinedConditionOption option)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                correctValue = string.Empty;
                option = UserDefinedConditionOption.EqualTo;
                return false;
            }

            string workingString = inputValue.Trim().ToLower().Replace(" ", "");

            string[] moreOrEqual = { ">=", "=>", "moreorequal", "equalormore", "greaterorequal", "equalorgreater", "equalorabove", "aboveorequal" };
            string[] lessOrEqual = { "<=", "=<", "lessorequal", "lesserorequal", "equalorless", "lowerorequal", "equalorlower", "beloworequal", "equalorbelow" };

            string[] above = { ">", "above", "morethan", "more", "over", "greaterthan", "greater" };
            string[] below = { "<", "below", "lessthan", "less", "under", "lowerthan", "lower" };

            // Find option
            if (moreOrEqual.Any(word => workingString.Contains(word)))
            {
                option = UserDefinedConditionOption.GreaterThanOrEqualTo;
            }
            else if (above.Any(word => workingString.Contains(word)))
            {
                option = UserDefinedConditionOption.GreaterThan;
            }
            else if (lessOrEqual.Any(word => workingString.Contains(word)))
            {
                option = UserDefinedConditionOption.LessThanOrEqualTo;
            }
            else if (below.Any(word => workingString.Contains(word)))
            {
                option = UserDefinedConditionOption.LessThan;
            }
            else
            {
                option = UserDefinedConditionOption.EqualTo;
            }

            bool lastCharIsDigit = false;
            bool negativeSign = false;
            string tempValue = string.Empty;
            char? scale = null;
            decimal numericValue;
            bool successfullParse;

            correctValue = string.Empty;

            foreach (char c in workingString)
            {
                if (char.IsDigit(c) || lastCharIsDigit && (char.Equals(c, '.') || char.Equals(c, ',')))
                {
                    if (!lastCharIsDigit)
                    {
                        if (negativeSign)
                            tempValue += "-";
                    }
                    tempValue += c;
                    lastCharIsDigit = true;
                }
                else if (char.Equals(c, '-'))
                {
                    negativeSign = true;
                }
                else
                {
                    negativeSign = false;
                    if (!lastCharIsDigit) continue;

                    if (char.Equals(c, 'c') || workingString.EndsWith("c"))
                        scale = 'C';
                    else if (char.Equals(c, 'f') || workingString.EndsWith("f"))
                        scale = 'F';
                    break;
                }
            }

            successfullParse = decimal.TryParse(tempValue, out numericValue);

            if (!successfullParse) return false;

            correctValue = (numericValue == 0 ? "0"
                : ((numericValue > 0 ? "+" : "") + $"{numericValue:0.###}"))
                + (scale.HasValue ? $" {scale.Value}" : "");
            return true;
        }

        private static bool TryParsePortCode(this string inputValue, out string correctValue)
        {
            correctValue = string.Empty;
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return false;
            }

            string workingText = inputValue.Trim().Replace(" ", "");
            if (workingText.Length > 5) return false;

            foreach (char c in workingText)
            {
                if (!char.IsLetterOrDigit(c)) return false;
            }

            correctValue = workingText.ToUpper();
            return true;
        }

        private static bool TryParseUnno(this string inputValue, out string correctValue)
        {
            correctValue = string.Empty;
            if (string.IsNullOrWhiteSpace(inputValue))
                return false;

            string workingText = inputValue.Trim().Replace(" ", "");
            if (workingText.Length > 4) return false;
            if (ushort.TryParse(workingText, out ushort value))
            {
                correctValue = value.ToString();
                return true;
            }
            return false;
        }

        private static bool TryParseDgClass(this string inputValue, out string correctValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                correctValue = string.Empty;
                return false;
            }
            return DgClassInputValidator.Validate(inputValue, out correctValue);
        }

        private static bool TryParseContainerNumber(this string inputValue, out string correctValue)
        {
            correctValue = string.Empty;

            string workingText = inputValue.Trim().Replace(" ", "");
            if (workingText.Length > 11) return false;

            int count = 0;
            foreach (char c in workingText)
            {
                if (!char.IsLetterOrDigit(c)) return false;
                if (count > 3 && !char.IsDigit(c)) return false;
                count++;
            }

            correctValue = workingText.ToUpper();
            return true;
        }

        #endregion

        #region Input and format helpers
        // ----- Input and format helpers -----

        /// <summary>
        /// Returns value with decorations or additional symbols if required by selected type and option
        /// </summary>
        /// <param name="conditionValue"></param>
        /// <param name="selectedConditionIndex"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        internal static string CreateEnterValueFromConditionValue(string conditionValue, int selectedConditionIndex, UserDefinedConditionOption option)
        {
            if (selectedConditionIndex == (byte)UserDefinedConditionType.DgFPLoaded
                || selectedConditionIndex == (byte)UserDefinedConditionType.ReeferSetPointLoaded)
            {
                string optionText;
                switch (option)
                {
                    case UserDefinedConditionOption.GreaterThan:
                        optionText = DEFAULT_MORE_THAN;
                        break;
                    case UserDefinedConditionOption.GreaterThanOrEqualTo:
                        optionText = DEFAULT_MORE_THAN_OR_EQUAL;
                        break;
                    case UserDefinedConditionOption.LessThan:
                        optionText = DEFAULT_LESS_THAN;
                        break;
                    case UserDefinedConditionOption.LessThanOrEqualTo:
                        optionText = DEFAULT_LESS_THAN_OR_EQUAL;
                        break;
                    case UserDefinedConditionOption.EqualTo:
                    default:
                        optionText = string.Empty;
                        break;
                }
                return $"{optionText}{(string.IsNullOrWhiteSpace(optionText) ? "" : " ")}{conditionValue}";
            }
            else return conditionValue;
        }

        /// <summary>
        /// Returns correct <see cref="UserDefinedConditionType"/> from selected index taking into account difference in numbers
        /// </summary>
        /// <param name="selectedIndex"></param>
        /// <returns></returns>
        internal static UserDefinedConditionType GetProperUserDefinedConditionType(this int selectedIndex)
        {
            if (selectedIndex == 21)
            {
                return UserDefinedConditionType.FreeText;
            }
            else return (UserDefinedConditionType)selectedIndex;
        }

        /// <summary>
        /// Removes or replaces any symbol from input text that may affect further work with conditions
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static string RemoveForbiddenSymbols(this string input)
        {
            return input.Replace("|", "/");
        }

        #endregion
    }
}
