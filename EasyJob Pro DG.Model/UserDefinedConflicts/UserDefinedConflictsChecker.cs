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
        const string DEFAULT_EMPTY_VALUE = "EMPTY";

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

        private UserDefinedConflict CreateSingleUnitConflict(UserDefinedCondition condition, string containerNumber)
        {
            return new UserDefinedConflict(isMultiUnit: false)
            {
                ContainerNumber = containerNumber,
                ConditionReference = condition
            };
        }

        private UserDefinedConflict CreateMultiUnitConflict(UserDefinedCondition condition, List<string> containerNumbers, List<int> dgIDs = null)
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
            bool isSingleUnitConflict = false;
            bool isMultiUnitConflict = false;

            Container container = null;
            List<Dg> conflictedDgs = null;
            List<Container> conflictedContainers = null;

            CheckAccordingToConditionType(cargoPlan, condition, ref isSingleUnitConflict, ref isMultiUnitConflict, ref container, ref conflictedDgs, ref conflictedContainers);

            conflict = CreateCorrectTypeOfConflict(condition, isSingleUnitConflict, isMultiUnitConflict, container, conflictedDgs, conflictedContainers);

            return isSingleUnitConflict || isMultiUnitConflict;
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
        private UserDefinedConflict CreateCorrectTypeOfConflict(UserDefinedCondition condition, bool isSingleUnitConflict, bool isMultiUnitConflict, Container container, List<Dg> conflictedDgs, List<Container> conflictedContainers)
        {
            List<string> conflictedContainerNumbers;
            UserDefinedConflict conflict = null;

            if (isSingleUnitConflict)
            {
                conflict = CreateSingleUnitConflict(condition, container?.ContainerNumber ?? DEFAULT_EMPTY_VALUE);
            }
            else if (isMultiUnitConflict)
            {
                List<int> conflictedDgIDs = null;

                if (conflictedDgs is not null)
                    GetContainerNumbersAndDgIds(conflictedDgs, out conflictedContainerNumbers, out conflictedDgIDs);
                else
                    GetContainerNumbers(conflictedContainers, out conflictedContainerNumbers);

                conflict = CreateMultiUnitConflict(condition, conflictedContainerNumbers, conflictedDgIDs);
            }
            return conflict;
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
        private void CheckAccordingToConditionType(CargoPlan cargoPlan, UserDefinedCondition condition, ref bool isSingleUnitConflict, ref bool isMultiUnitConflict, ref Container container, ref List<Dg> conflictedDgs, ref List<Container> conflictedContainers)
        {
            switch (condition.ConditionType)
            {
                // Single unit conditions

                case UserDefinedConditionType.ContainerNumberLoaded:
                    if (TryGetContainerByNumber(cargoPlan, condition, out container))
                    {
                        if (condition.CellPosition.IsEmpty || condition.CellPosition == container)
                        {
                            isSingleUnitConflict = true;
                        }
                    }
                    break;

                case UserDefinedConditionType.ContainerNumberDischarged:
                    if (!TryGetContainerByNumber(cargoPlan, condition, out container)
                        || (!condition.CellPosition.IsEmpty && condition.CellPosition != container))
                    {
                        isSingleUnitConflict = true;
                    }
                    break;

                case UserDefinedConditionType.CellPositionLoaded:
                    if (TryGetContainerByCellPosition(cargoPlan, condition, out container))
                    {
                        isSingleUnitConflict = true;
                    }
                    break;
                case UserDefinedConditionType.CellPositionEmpty:
                    if (!TryGetContainerByCellPosition(cargoPlan, condition, out container))
                    {
                        isSingleUnitConflict = true;
                    }
                    break;

                case UserDefinedConditionType.CellPositionLoadedWithReefer:
                    if (TryGetContainerByCellPosition(cargoPlan, condition, out container))
                    {
                        isSingleUnitConflict = true;
                    }
                    break;
                case UserDefinedConditionType.CellPositionLoadedWithDg:
                    if (TryGetContainerByCellPosition(cargoPlan, condition, out container))
                    {
                        isSingleUnitConflict = true;
                    }
                    break;

                case UserDefinedConditionType.ContainerLocationChanged:
                    if (TryGetContainerByNumber(cargoPlan, condition, out container) && container.HasLocationChanged)
                    {
                        isSingleUnitConflict = true;
                    }
                    break;
                case UserDefinedConditionType.ContainerPODchanged:
                    if (TryGetContainerByNumber(cargoPlan, condition, out container) && container.HasPodChanged)
                    {
                        isSingleUnitConflict = true;
                    }
                    break;

                case UserDefinedConditionType.ContainerNumberNotInPosition:
                    if (!TryGetContainerByNumber(cargoPlan, condition, out container)
                        || (!condition.CellPosition.IsEmpty && condition.CellPosition != container))
                    {
                        isSingleUnitConflict = true;
                    }
                    break;


                // Multi-unit conditions

                case UserDefinedConditionType.DgNameContainsLoaded:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange([.. cargoPlan.DgList.Where(d => !string.IsNullOrWhiteSpace(d.Name)
                                            && d.Name.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim()))]);

                    isMultiUnitConflict = conflictedDgs.Count >= 1;
                    break;


                case UserDefinedConditionType.DgTechnicalNameContains:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange([.. cargoPlan.DgList.Where(d => !string.IsNullOrWhiteSpace(d.TechnicalName)
                                                && d.TechnicalName.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim()))]);

                    isMultiUnitConflict = conflictedDgs.Count >= 1;
                    break;

                case UserDefinedConditionType.DgClassLoaded:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange([.. cargoPlan.DgList.Where(d => d.AllDgClasses.Any(c => c.Contains(condition.ConditionValue.Trim())))]);

                    isMultiUnitConflict = conflictedDgs.Count >= 1;
                    break;

                case UserDefinedConditionType.DgUnnoLoaded:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange([.. cargoPlan.DgList.Where(d => d.Unno == uint.Parse(condition.ConditionValue))]);

                    isMultiUnitConflict = conflictedDgs.Count >= 1;
                    break;

                case UserDefinedConditionType.DgFPLoaded:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange
                        ([.. cargoPlan.DgList.Where(d => CheckConflictedDecimalCondition(d.FlashPointAsDecimal, condition))]);

                    isMultiUnitConflict = conflictedDgs.Count >= 1;
                    break;

                case UserDefinedConditionType.ReeferContainingDgLoaded:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange([.. cargoPlan.Reefers.Where(r => r.ContainsDgCargo)]);

                    isMultiUnitConflict = conflictedContainers.Count >= 1;
                    break;

                case UserDefinedConditionType.ReeferSetPointLoaded:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange([.. cargoPlan.Reefers.Where(r => CheckConflictedDecimalCondition(r.SetTemperature, condition))]);

                    isMultiUnitConflict = conflictedContainers.Count >= 1;
                    break;
                case UserDefinedConditionType.ReeferCommodityContains:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange([.. cargoPlan.Reefers.Where(r => !string.IsNullOrWhiteSpace(r.Commodity)
                                                    && r.Commodity.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim()))]);

                    isMultiUnitConflict = conflictedContainers.Count >= 1;
                    break;

                case UserDefinedConditionType.NoNetWeightUnitsOnboard:
                    InitiateConflictedDgs(out conflictedDgs);
                    conflictedDgs.AddRange([.. cargoPlan.DgList.Where(d => d.DgNetWeight <= 0)]);

                    isMultiUnitConflict = conflictedDgs.Count >= 1;
                    break;

                case UserDefinedConditionType.POLUnitsOnboard:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange([.. cargoPlan.Containers.Where(r => !string.IsNullOrWhiteSpace(r.POL)
                                                    && r.POL.ToUpper().Trim().Contains(condition.ConditionValue.ToUpper().Trim()))]);

                    isMultiUnitConflict = conflictedContainers.Count >= 1;
                    break;

                case UserDefinedConditionType.PODUnitsOnboard:
                    InitiateConflictedContainers(out conflictedContainers);
                    conflictedContainers.AddRange([.. cargoPlan.Containers.Where(r => !string.IsNullOrWhiteSpace(r.POD)
                                                    && r.POD.ToUpper().Trim().Contains(condition.ConditionValue.ToUpper().Trim()))]);

                    isMultiUnitConflict = conflictedContainers.Count >= 1;
                    break;

                case UserDefinedConditionType.FreeText:
                case UserDefinedConditionType.General:
                    isSingleUnitConflict = true; // Free text condition is always conflicted as long as it is not empty, the actual conflict info will be shown in the comment of the conflict.
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Checks if decimalValue is within the values stated in <see cref="UserDefinedCondition"/> taking into account <see cref="UserDefinedConditionOption"/>.
        /// </summary>
        /// <returns></returns>
        private bool CheckConflictedDecimalCondition(decimal decimalValue, UserDefinedCondition condition)
        {
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
            conflictedContainerNumbers = new();
            conflictedDgIDs = new();
            conflictedContainerNumbers = conflictedDgs.Select(dg => dg.ContainerNumber).Distinct().ToList();
            conflictedDgIDs = conflictedDgs.Select(dg => dg.ID).ToList();
        }

        /// <summary>
        /// Returns in out parameter List of distinct ContaierNumbers from conflictedContainers. 
        /// </summary>
        /// <param name="conflictedContainers"></param>
        /// <param name="conflictedContainerNumbers"></param>
        private static void GetContainerNumbers(List<Container> conflictedContainers, out List<string> conflictedContainerNumbers)
        {
            conflictedContainerNumbers = new();
            conflictedContainerNumbers = conflictedContainers.Select(c => c.ContainerNumber).Distinct().ToList();
        }

        private static void InitiateConflictedDgs(out List<Dg> conflictedDgs)
        {
            conflictedDgs = new();
        }

        private static void InitiateConflictedContainers(out List<Container> conflictedContainers)
        {
            conflictedContainers = new();
        }

        private bool TryGetContainerByCellPosition(CargoPlan cargoPlan, UserDefinedCondition condition, out Container container)
        {
            container = cargoPlan.Containers.FirstOrDefault(c => (ILocationOnBoard)c == condition.CellPosition);
            if (container is not null)
                return true;
            return false;
        }

        private bool TryGetContainerByNumber(CargoPlan cargoPlan, UserDefinedCondition condition, out Container container)
        {
            container = cargoPlan.Containers.FirstOrDefault(c => string.Equals(c.ContainerNumber, condition.ConditionValue));
            if (container is not null)
                return true;
            return false;
        }
    }
}
