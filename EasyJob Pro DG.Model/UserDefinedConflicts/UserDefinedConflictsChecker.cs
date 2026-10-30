using EasyJob_ProDG.Model.Cargo;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    /// <summary>
    /// Creates <see cref="UserDefinedConflict"/>s from <see cref="CargoPlan"/> against active <see cref="UserDefinedCondition"/>s.
    /// </summary>
    public class UserDefinedConflictsChecker
    {
        /// <summary>
        /// Checks <see cref="CargoPlan"/> for <see cref="UserDefinedConflict"/>s against <see cref="UserDefinedCondition"/>s in parameter 'conditions' and returns the List<UserDefinedConflict> discovered.
        /// </summary>
        /// <param name="cargoPlan"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public List<UserDefinedConflict> CheckConflicts(CargoPlan cargoPlan, List<UserDefinedCondition> conditions)
        {
            List<UserDefinedConflict> conflictsToShow = new List<UserDefinedConflict>();
            foreach (var condition in conditions)
            {
                CheckAddConflicts(conflictsToShow, cargoPlan, condition);
            }
            return conflictsToShow;
        }


        // ----- Private methods -----

        /// <summary>
        /// Checks individual <see cref="UserDefinedCondition"/> if conflicts found in <see cref="CargoPlan"/>, adds <see cref="UserDefinedConflict"/> to the parameter 'conflictsToShow'.
        /// </summary>
        /// <param name="conflictsToShow"></param>
        /// <param name="cargoPlan"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private void CheckAddConflicts(List<UserDefinedConflict> conflictsToShow, CargoPlan cargoPlan, UserDefinedCondition condition)
        {
            if (CheckIfConflicted(cargoPlan, condition, out UserDefinedConflict conflict))
                conflictsToShow.Add(conflict);
        }

        private UserDefinedConflict CreateMultiUnitConflict(UserDefinedCondition condition, List<string> containerNumbers, List<int> dgIDs)
        {
            return new UserDefinedConflict(isMultiUnit: true)
            {
                ContainerNumbers = containerNumbers,
                DgIDs = dgIDs,
                ConditionReference = condition
            };
        }

        /// <summary>
        /// Checks if any <see cref="UserDefinedConflict"/> found in <see cref="CargoPlan"/> as per given <see cref="UserDefinedCondition"/>. 
        /// </summary>
        /// <param name="cargoPlan"></param>
        /// <param name="condition"></param>
        /// <param name="conflictedContainerNumbersList">List of conflicted containers numbers.</param>
        /// <returns>True if conflicts found. False if no confilicts found.</returns>
        private bool CheckIfConflicted(CargoPlan cargoPlan, UserDefinedCondition condition, out UserDefinedConflict conflict)
        {
            List<Dg> conflictedDgs = null;
            List<Container> conflictedContainers = null;
            conflict = null;

            if (CheckAccordingToConditionType(cargoPlan, condition, ref conflictedDgs, ref conflictedContainers))
            {
                conflict = CreateCorrectTypeOfConflict(condition, conflictedDgs, conflictedContainers);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns <see cref="UserDefinedConflict"/> created from input values of a correct type.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isSingleUnitConflict"></param>
        /// <param name="isMultiUnitConflict"></param>
        /// <param name="container"></param>
        /// <param name="conflictedDgs"></param>
        /// <param name="conflictedContainers"></param>
        private UserDefinedConflict CreateCorrectTypeOfConflict(UserDefinedCondition condition, List<Dg> conflictedDgs, List<Container> conflictedContainers)
        {
            List<string> conflictedContainerNumbers = null;
            List<int> conflictedDgIDs = null;

            if (IsConditionRequireContainerList(condition, conflictedContainers))
                GetContainerNumbersAndDgIds(conflictedContainers, conflictedDgs, out conflictedContainerNumbers, out conflictedDgIDs);

            UserDefinedConflict conflict = CreateMultiUnitConflict(condition, conflictedContainerNumbers, conflictedDgIDs);

            return conflict;
        }

        private bool IsConditionRequireContainerList(UserDefinedCondition condition, List<Container> conflictedContainers)
        {
            if ((condition.ConditionType == UserDefinedConditionType.ContainerNumberDischarged && conflictedContainers.Count > 0)
                || (condition.ConditionType == UserDefinedConditionType.ContainerNumberNotInPosition && conflictedContainers.Count > 0))
                return true;
            return (condition.ConditionType != UserDefinedConditionType.FreeText
            && condition.ConditionType != UserDefinedConditionType.General
            && condition.ConditionType != UserDefinedConditionType.ContainerNumberNotInPosition
            && condition.ConditionType != UserDefinedConditionType.ContainerNumberDischarged
            && condition.ConditionType != UserDefinedConditionType.CellPositionEmpty);
        }

        /// <summary>
        /// Switches between <see cref="UserDefinedConditionType"/> of the condition to check if there is a respective conflict according to the <see cref="UserDefinedCondition"/> and updates found conflicted lists of <see cref="Dg"/>s and <see cref="Container"/>s and other ref parameters.
        /// </summary>
        /// <param name="cargoPlan"></param>
        /// <param name="condition"></param>
        /// <param name="isSingleUnitConflict"></param>
        /// <param name="isMultiUnitConflict"></param>
        /// <param name="container"></param>
        /// <param name="conflictedDgs"></param>
        /// <param name="conflictedContainers"></param>
        private bool CheckAccordingToConditionType(CargoPlan cargoPlan, UserDefinedCondition condition, ref List<Dg> conflictedDgs, ref List<Container> conflictedContainers)
        {
            switch (condition.ConditionType)
            {
                case UserDefinedConditionType.ContainerNumberLoaded:
                    if (TryGetContainersByNumber(cargoPlan, condition, out conflictedContainers))
                    {
                        if (!condition.CellPosition.IsEmpty)
                        {
                            conflictedContainers = conflictedContainers.Where(c => condition.CellPosition == c).ToList();
                        }
                    }
                    break;

                case UserDefinedConditionType.ContainerNumberDischarged:
                case UserDefinedConditionType.ContainerNumberNotInPosition:
                    if (!TryGetContainersByNumber(cargoPlan, condition, out conflictedContainers))
                        return true;
                    if (condition.CellPosition.IsEmpty)
                        return false;
                    conflictedContainers = conflictedContainers.Where(c => condition.CellPosition != c).ToList();

                    break;

                case UserDefinedConditionType.ContainerLocationChanged:
                    if (TryGetContainersByNumber(cargoPlan, condition, out conflictedContainers))
                    {
                        conflictedContainers = conflictedContainers.Where(c => c.HasLocationChanged).ToList();
                    }
                    break;
                case UserDefinedConditionType.ContainerPODchanged:
                    if (TryGetContainersByNumber(cargoPlan, condition, out conflictedContainers))
                    {
                        conflictedContainers = conflictedContainers.Where(c => c.HasPodChanged).ToList();
                    }
                    break;

                case UserDefinedConditionType.CellPositionLoaded:
                    TryGetContainersByCellPosition(cargoPlan, condition, out conflictedContainers);
                    break;
                case UserDefinedConditionType.CellPositionEmpty:
                    if (TryGetContainersByCellPosition(cargoPlan, condition, out conflictedContainers))
                        return false;
                    else return true;

                case UserDefinedConditionType.CellPositionLoadedWithReefer:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers = cargoPlan.Reefers.Where(c => c == condition.CellPosition).ToList();
                    break;

                case UserDefinedConditionType.CellPositionLoadedWithDg:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange(cargoPlan.Containers.Where(d => d.ContainsDgCargo && (condition.CellPosition == d)));
                    break;

                case UserDefinedConditionType.DgNameContainsLoaded:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange([.. cargoPlan.DgList.Where(d => !string.IsNullOrWhiteSpace(d.Name)
                                            && d.Name.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim()))]);
                    break;


                case UserDefinedConditionType.DgTechnicalNameContains:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange([.. cargoPlan.DgList.Where(d => !string.IsNullOrWhiteSpace(d.TechnicalName)
                                                && d.TechnicalName.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim()))]);
                    break;

                case UserDefinedConditionType.DgClassLoaded:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange([.. cargoPlan.DgList.Where(d => d.AllDgClasses.Any(c => c.Contains(condition.ConditionValue.Trim())))]);
                    break;

                case UserDefinedConditionType.DgUnnoLoaded:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange([.. cargoPlan.DgList.Where(d => d.Unno == uint.Parse(condition.ConditionValue))]);
                    break;

                case UserDefinedConditionType.DgFPLoaded:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange
                        ([.. cargoPlan.DgList.Where(d => CheckConflictedDecimalCondition(d.FlashPointAsDecimal, condition))]);
                    break;

                case UserDefinedConditionType.ReeferContainingDgLoaded:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange([.. cargoPlan.Reefers.Where(r => r.ContainsDgCargo)]);
                    break;

                case UserDefinedConditionType.ReeferSetPointLoaded:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange([.. cargoPlan.Reefers.Where(r => CheckConflictedDecimalCondition(r.SetTemperature, condition))]);
                    break;
                case UserDefinedConditionType.ReeferCommodityContains:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange([.. cargoPlan.Reefers.Where(r => !string.IsNullOrWhiteSpace(r.Commodity)
                                                    && r.Commodity.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim()))]);
                    break;

                case UserDefinedConditionType.NoNetWeightUnitsOnboard:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange([.. cargoPlan.DgList.Where(d => d.DgNetWeight <= 0)]);
                    break;

                case UserDefinedConditionType.POLUnitsOnboard:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange([.. cargoPlan.Containers.Where(r => !string.IsNullOrWhiteSpace(r.POL)
                                                    && r.POL.ToUpper().Trim().Contains(condition.ConditionValue.ToUpper().Trim()))]);
                    break;

                case UserDefinedConditionType.PODUnitsOnboard:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange([.. cargoPlan.Containers.Where(r => !string.IsNullOrWhiteSpace(r.POD)
                                                    && r.POD.ToUpper().Trim().Contains(condition.ConditionValue.ToUpper().Trim()))]);
                    break;

                case UserDefinedConditionType.FreeText:
                case UserDefinedConditionType.General:
                    return true; // Free text condition is always conflicted as long as it is not empty, the actual conflict info will be shown in the comment of the conflict.

                default:
                    return false;
            }

            if ((conflictedContainers is null && conflictedDgs?.Count == 0)
                || conflictedContainers?.Count == 0)
                return false;

            if (conflictedDgs is null)
            {
                conflictedDgs = SearchForDgs(cargoPlan, conflictedContainers);
            }
            return true;
        }

        private List<Dg> SearchForDgs(CargoPlan cargoPlan, List<Container> containers)
        {
            var result = new List<Dg>();
            foreach (var dg in cargoPlan.DgList)
            {
                if (containers.Any(c => string.Equals(c.ContainerNumber, dg.ContainerNumber)))
                    result.Add(dg);
            }
            return result;
        }

        /// <summary>
        /// Checks if decimalValue is within the values stated in <see cref="UserDefinedCondition"/> taking into account <see cref="UserDefinedConditionOption"/>.
        /// </summary>
        /// <returns></returns>
        private bool CheckConflictedDecimalCondition(decimal decimalValue, UserDefinedCondition condition)
        {
            if (ProgramDefaultValues.DefaultFlashPointValue == decimalValue)
                return false;
            if (!decimal.TryParse(condition.ConditionValue, out decimal conditionValue))
                return false;

            switch (condition.ConditionOption)
            {
                case UserDefinedConditionOption.EqualTo:
                default:
                    return decimalValue < conditionValue + 0.1m && decimalValue > conditionValue - 0.1m;
                case UserDefinedConditionOption.GreaterThanOrEqualTo:
                    return decimalValue >= conditionValue - 0.01m;
                case UserDefinedConditionOption.GreaterThan:
                    return decimalValue > conditionValue;
                case UserDefinedConditionOption.LessThanOrEqualTo:
                    return decimalValue <= conditionValue + 0.001m;
                case UserDefinedConditionOption.LessThan:
                    return decimalValue < conditionValue;
            }
        }

        /// <summary>
        /// Returns in out parameters Lists of distinct ContainerNumbers and DgIDs from conflictedDgs.
        /// </summary>
        /// <param name="conflictedDgs"></param>
        /// <param name="conflictedContainerNumbers"></param>
        /// <param name="conflictedDgIDs"></param>
        private static void GetContainerNumbersAndDgIds(List<Dg> conflictedDgs, out List<string> conflictedContainerNumbers, out List<int> conflictedDgIDs)
        {
            conflictedContainerNumbers = conflictedDgs?.Select(dg => dg.ContainerNumber).Distinct().ToList();
            conflictedDgIDs = conflictedDgs.Select(dg => dg.ID).ToList();
        }

        /// <summary>
        /// Returns in out parameter List of distinct ContaierNumbers from conflictedContainers. 
        /// </summary>
        /// <param name="conflictedContainers"></param>
        /// <param name="conflictedContainerNumbers"></param>
        private static void GetContainerNumbersAndDgIds(List<Container> conflictedContainers, List<Dg> conflictedDgs, out List<string> conflictedContainerNumbers, out List<int> conflictedDgIDs)
        {
            if (conflictedContainers is null || conflictedContainers?.Count == 0)
            {
                GetContainerNumbersAndDgIds(conflictedDgs, out conflictedContainerNumbers, out conflictedDgIDs);
            }
            else
            {
                conflictedContainerNumbers = conflictedContainers.Select(c => c.ContainerNumber).Distinct().ToList();
                conflictedDgIDs = conflictedDgs.Select(dg => dg.ID).ToList();
            }
        }

        private static void InitiateConflictedDgs(out List<Dg> conflictedDgs)
        {
            conflictedDgs = new();
        }

        private static void InitiateConflictedContainers(out List<Container> conflictedContainers)
        {
            conflictedContainers = new();
        }

        private bool TryGetContainersByCellPosition(CargoPlan cargoPlan, UserDefinedCondition condition, out List<Container> conflictedContainers)
        {
            InitiateConflictedContainers(out conflictedContainers);
            conflictedContainers = cargoPlan.Containers.Where(c => c == condition.CellPosition).ToList();
            if (conflictedContainers.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Returns in out parameter a list of <see cref="Container"/>s that ContainerNumber matches or contains a part of value from condition.ConditionValue.
        /// True if the list is not empty.
        /// </summary>
        /// <param name="cargoPlan"></param>
        /// <param name="condition"></param>
        /// <param name="containers"></param>
        /// <returns>True if matching container numbers found.</returns>
        private bool TryGetContainersByNumber(CargoPlan cargoPlan, UserDefinedCondition condition, out List<Container> containers)
        {
            if (condition.ConditionValue.Length < 11)
                containers = cargoPlan.Containers.Where(c => c.ContainerNumber.Contains(condition.ConditionValue.ToUpper().Trim())).ToList();
            else
            {
                containers = cargoPlan.Containers.Where(c => string.Equals(c.ContainerNumber, condition.ConditionValue)).ToList();
            }
            if (containers.Count > 0)
                return true;
            return false;
        }
    }
}
