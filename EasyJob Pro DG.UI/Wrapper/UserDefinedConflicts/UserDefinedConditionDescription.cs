using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.UserDefinedConflicts;
using System;

namespace EasyJob_ProDG.UI.Wrapper.UserDefinedConflicts
{
    /// <summary>
    /// Generates description for <see cref="UserDefinedConditionWrapper"/>s
    /// </summary>
    internal static class UserDefinedConditionDescription
    {
        const string DEFAULT_CONDITION_DESCRIPTION = "Condition description";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        internal static void UpdateDescription(this UserDefinedConditionWrapper condition)
        {
            condition.ConditionDescription = condition.GetDescription();
            condition.ToolTipText = condition.ConditionDescription;
        }

        internal static string GetDescription(this UserDefinedConditionWrapper condition)
        {
            string resultDescription = string.Empty;

            try
            {
                switch (condition.ConditionType)
                {
                    case UserDefinedConditionType.General:
                    case UserDefinedConditionType.FreeText:
                        resultDescription = condition.ConditionValue;
                        break;

                    case UserDefinedConditionType.CellPositionLoaded:
                        resultDescription = $"Container is loaded in {condition.CellPosition}";
                        break;
                    case UserDefinedConditionType.CellPositionEmpty:
                        resultDescription = $"Container is discharged from or not loaded in {condition.CellPosition}";
                        break;
                    case UserDefinedConditionType.CellPositionLoadedWithReefer:
                        resultDescription = $"Reefer is loaded in {condition.CellPosition}";
                        break;
                    case UserDefinedConditionType.CellPositionLoadedWithDg:
                        resultDescription = $"Dangerous cargo is loaded in {condition.CellPosition}";
                        break;

                    case UserDefinedConditionType.ContainerNumberLoaded:
                        resultDescription = ContainerNumberContainingValue(condition)
                            + "is loaded"
                            + AddCellPositionIfNotEmpty(condition);
                        break;
                    case UserDefinedConditionType.ContainerNumberDischarged:

                        resultDescription = ContainerNumberContainingValue(condition)
                            + "is discharged or not loaded"
                            + AddCellPositionIfNotEmpty(condition);
                        break;

                    case UserDefinedConditionType.ContainerLocationChanged:
                        resultDescription = $"Position of {ContainerNumberContainingValue(condition)} has changed";
                        break;
                    case UserDefinedConditionType.ContainerPODchanged:
                        resultDescription = $"{ContainerNumberContainingValue(condition)} port of discharging has changed";
                        break;

                    case UserDefinedConditionType.DgNameContainsLoaded:
                        resultDescription = $"Dangerous cargo with '{condition.ConditionValue}' in its proper shipping name is loaded"
                                + AddCellPositionIfNotEmpty(condition);
                        break;
                    case UserDefinedConditionType.DgTechnicalNameContains:
                        resultDescription = $"Dangerous cargo with '{condition.ConditionValue}' in its technical name is loaded"
                                + AddCellPositionIfNotEmpty(condition);
                        break;
                    case UserDefinedConditionType.ReeferCommodityContains:
                        resultDescription = $"Reefer cargo with '{condition.ConditionValue}' in its commodity is loaded"
                                + AddCellPositionIfNotEmpty(condition);
                        break;


                    case UserDefinedConditionType.DgClassLoaded:
                        resultDescription = $"Dangerous cargo with class '{condition.ConditionValue}' is loaded"
                                + AddCellPositionIfNotEmpty(condition);
                        break;
                    case UserDefinedConditionType.DgUnnoLoaded:
                        resultDescription = $"Dangerous cargo with UNNO {ushort.Parse(condition.ConditionValue):0000} is loaded"
                                + AddCellPositionIfNotEmpty(condition);
                        break;


                    case UserDefinedConditionType.ReeferContainingDgLoaded:
                        resultDescription = $"Reefer containing dangerous goods is loaded"
                        + AddCellPositionIfNotEmpty(condition);
                        break;

                    case UserDefinedConditionType.DgFPLoaded:
                        resultDescription = $"Dangerous cargo with flash point {DefineConditionOption(condition.ConditionOption)}{condition.ConditionValue}"
                                + AddCellPositionIfNotEmpty(condition);
                        break;
                    case UserDefinedConditionType.ReeferSetPointLoaded:
                        resultDescription = $"Reefer with set point {DefineConditionOption(condition.ConditionOption)}{condition.ConditionValue}"
                                + AddCellPositionIfNotEmpty(condition);
                        break;

                    case UserDefinedConditionType.POLUnitsOnboard:
                        resultDescription = $"Container with port of loading '{condition.ConditionValue}' is loaded"
                                + AddCellPositionIfNotEmpty(condition);
                        break;
                    case UserDefinedConditionType.PODUnitsOnboard:
                        resultDescription = $"Container with port of discharging '{condition.ConditionValue}' is loaded"
                                + AddCellPositionIfNotEmpty(condition);
                        break;

                    case UserDefinedConditionType.ContainerNumberNotInPosition:
                        resultDescription = ContainerNumberContainingValue(condition)
                            + $" is not in {condition.CellPosition}";
                        break;

                    case UserDefinedConditionType.NoNetWeightUnitsOnboard:
                        resultDescription = $"Any dangerous cargo has no Net Weight value";
                        break;
                    default:
                        resultDescription = DEFAULT_CONDITION_DESCRIPTION;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteError($"Creating UD condition description for type {condition.ConditionType} with value {condition.ConditionValue} caused and error {ex.Message}");
                resultDescription = DEFAULT_CONDITION_DESCRIPTION;
            }
            return resultDescription;
        }

        internal static string AddCellPositionIfNotEmpty(UserDefinedConditionWrapper condition)
        {
            return (condition.CellPosition.IsEmpty ? "" : $" in {condition.CellPosition}");
        }

        internal static string AddCellPositionIfNotEmpty(CellPositionWrapper cellPosition)
        {
            return (cellPosition.IsEmpty ? "" : $" in {cellPosition}");
        }

        /// <summary>
        /// Depending on conditionValue returns text for container number or number containing
        /// </summary>
        /// <param name="conditionValue"></param>
        /// <returns>Container with number containing 'XXXX' or Container with number XXXX 1234567</returns>
        internal static string ContainerNumberContainingValue(UserDefinedConditionWrapper condition)
        {
            return ContainerNumberContainingValue(condition.ConditionValue);
        }

        /// <summary>
        /// Depending on container returns text for container number or number containing
        /// </summary>
        /// <param name="conditionValue"></param>
        /// <returns>Container with number containing 'XXXX' or Container with number XXXX 1234567</returns>
        internal static string ContainerNumberContainingValue(string containerNumber)
        {
            return containerNumber.Length < 11
                ? $"Container with number containing '{containerNumber}' "
                : $"Container with number {UserSettings.ContainerNumberToDisplay(containerNumber)} ";
        }

        /// <summary>
        /// Gives textual description of <see cref="UserDefinedConditionOption"/>
        /// </summary>
        /// <param name="conditionOption"></param>
        /// <returns></returns>
        internal static string DefineConditionOption(UserDefinedConditionOption conditionOption)
        {
            string result;

            switch (conditionOption)
            {
                case UserDefinedConditionOption.GreaterThanOrEqualTo:
                    result = "equal or greater than ";
                    break;
                case UserDefinedConditionOption.GreaterThan:
                    result = "greater than ";
                    break;
                case UserDefinedConditionOption.LessThanOrEqualTo:
                    result = "equal or less than ";
                    break;
                case UserDefinedConditionOption.LessThan:
                    result = "less than ";
                    break;
                case UserDefinedConditionOption.EqualTo:
                    result = "equal to ";
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
    }
}
