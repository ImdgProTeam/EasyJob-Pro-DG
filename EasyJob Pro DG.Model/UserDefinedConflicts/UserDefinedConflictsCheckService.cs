using EasyJob_ProDG.Model.Cargo;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    internal class UserDefinedConflictsCheckService
    {
        internal List<UserDefinedConflict> CheckConflicts(CargoPlan cargoPlan, List<UserDefinedCondition> conditions)
        {
            List<UserDefinedConflict> conflictsToShow = new List<UserDefinedConflict>();
            foreach (var condition in conditions)
            {
                CheckAddConflicts(conflictsToShow, cargoPlan, condition);
            }
            return conflictsToShow;
        }

        /// <summary>
        /// Checks individual <see cref="UserDefinedCondition"/> if conflicts found in <see cref="CargoPlan"/>, adds <see cref="UserDefinedConflict"/> to the parameter 'conflictsToShow'.
        /// </summary>
        /// <param name="conflictsToShow"></param>
        /// <param name="cargoPlan"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static List<UserDefinedConflict> CheckAddConflicts(List<UserDefinedConflict> conflictsToShow, CargoPlan cargoPlan, UserDefinedCondition condition)
        {
            CheckIfConflicted(cargoPlan, condition, out List<string> containerNumbersList);
            UserDefinedConflict conflict = new UserDefinedConflict("title", "description", condition);
            conflictsToShow.Add(conflict);
            return conflictsToShow;
        }

        /// <summary>
        /// Checks if any <see cref="UserDefinedConflict"/> found in <see cref="CargoPlan"/> as per given <see cref="UserDefinedCondition"/>. 
        /// </summary>
        /// <param name="cargoPlan"></param>
        /// <param name="condition"></param>
        /// <param name="conflictedContainerNumbersList">List of conflicted containers numbers.</param>
        /// <returns>True if conflicts found. False if no confilicts found.</returns>
        private static bool CheckIfConflicted(CargoPlan cargoPlan, UserDefinedCondition condition, out List<string> conflictedContainerNumbersList)
        {
            bool isConflicted = false;
            conflictedContainerNumbersList = new();

            switch (condition.ConditionType)
            {
                case UserDefinedConditionType.ContainerNumberLoaded:
                    if (cargoPlan.Containers.Any(c => string.Equals(c.ContainerNumber, condition.ConditionValue)))
                        conflictedContainerNumbersList.Add(condition.ConditionValue);
                    break;
                case UserDefinedConditionType.ContainerNumberDischarged:
                    isConflicted = !cargoPlan.Containers.Any(c => string.Equals(c.ContainerNumber, condition.ConditionValue));
                    break;
                case UserDefinedConditionType.CellPositionLoaded:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.Containers.Where(c => (ILocationOnBoard)c == condition.CellPosition).Select(c => c.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.CellPositionEmpty:
                    isConflicted = !cargoPlan.Containers.Any(c => (ILocationOnBoard)c == condition.CellPosition);
                    break;
                case UserDefinedConditionType.ContainerLocationChanged:
                    isConflicted = cargoPlan.Containers.Any(c => c.ContainerNumber == condition.ConditionValue && c.HasLocationChanged);
                    conflictedContainerNumbersList.Add(condition.ConditionValue);
                    break;
                case UserDefinedConditionType.ContainerPODchanged:
                    isConflicted = cargoPlan.Containers.Any(c => c.ContainerNumber == condition.ConditionValue && c.HasPodChanged);
                    conflictedContainerNumbersList.Add(condition.ConditionValue);
                    break;
                case UserDefinedConditionType.ContainerPropertyLoaded:
                    //conflictedContainerNumbersList.AddRange([...cargoPlan.Containers])
                    break;
                case UserDefinedConditionType.DgPropertyLoaded:
                    break;
                case UserDefinedConditionType.ReeferPropertyLoaded:
                    break;
                case UserDefinedConditionType.DgNameContainsLoaded:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.DgList.Where(d => d.Name != null && d.Name.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim())).Select(dg => dg.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.DgClassLoaded:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.DgList.Where(d => d.AllDgClasses.Any(c => c.ToLower().Contains(condition.ConditionValue.ToLower().Trim()))).Select(dg => dg.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.DgUnnoLoaded:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.DgList.Where(d => d.Unno == int.Parse(condition.ConditionValue)).Select(dg => dg.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.ReeferSetPointLoaded:
                    if (!decimal.TryParse(condition.ConditionValue, out decimal setPoint))
                        break;
                    if (condition.ConditionOption == UserDefinedConditionOption.GreaterThan)
                    {
                        conflictedContainerNumbersList.AddRange([.. cargoPlan.Reefers.Where(r => r.SetTemperature > setPoint).Select(r => r.ContainerNumber)]);
                    }
                    else if (condition.ConditionOption == UserDefinedConditionOption.LessThan)
                    {
                        conflictedContainerNumbersList.AddRange([.. cargoPlan.Reefers.Where(r => r.SetTemperature < setPoint).Select(r => r.ContainerNumber)]);
                    }
                    else // Equal
                        conflictedContainerNumbersList.AddRange([.. cargoPlan.Reefers.Where(r => r.SetTemperature - setPoint < 0.1m).Select(r => r.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.ReeferCommodityContains:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.Reefers.Where(r => r.Commodity.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim())).Select(r => r.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.NoNetWeightUnitsOnboard:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.DgList.Where(dg => dg.DgNetWeight <= 0).Select(dg => dg.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.POLUnitsOnboard:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.Containers.Where(c => c.POL.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim())).Select(c => c.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.PODUnitsOnboard:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.Containers.Where(c => c.POD.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim())).Select(c => c.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.CellPositionLoadedWithReefer:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.Containers.Where(c => c.IsRf && (c as ILocationOnBoard) == condition.CellPosition).Select(c => c.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.CellPositionLoadedWithDg:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.Containers.Where(c => c.ContainsDgCargo && (c as ILocationOnBoard) == condition.CellPosition).Select(c => c.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.DgNameContainsLoadedInCellPosition:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.DgList.Where(d => d.Name.ToLower().Trim().Contains(condition.ConditionValue.ToLower().Trim()) && (d as ILocationOnBoard) == condition.CellPosition).Select(dg => dg.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.DgClassLoadedInCellPosition:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.DgList.Where(d => d.AllDgClasses.Contains(condition.ConditionValue) && (d as ILocationOnBoard) == condition.CellPosition).Select(dg => dg.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.DgUnnoLoadedInCellPosition:
                    conflictedContainerNumbersList.AddRange([.. cargoPlan.DgList.Where(d => d.Unno == int.Parse(condition.ConditionValue) && (d as ILocationOnBoard) == condition.CellPosition).Select(dg => dg.ContainerNumber)]);
                    break;
                case UserDefinedConditionType.ContainerNumberNotInPosition:
                    if (cargoPlan.Containers.Any(c => string.Equals(c.ContainerNumber, condition.ConditionValue) 
                    && (c as ILocationOnBoard) != condition.CellPosition))
                    {
                        isConflicted = true;
                        conflictedContainerNumbersList.Add(condition.ConditionValue);
                    }
                    break;
                case UserDefinedConditionType.FreeText:
                    isConflicted = true; // Free text condition is always conflicted as long as it is not empty, the actual conflict info will be shown in the comment of the conflict.
                    break;
                case UserDefinedConditionType.General:
                default:
                    break;
            }
            if (conflictedContainerNumbersList.Count > 0)
                isConflicted = true;
            return isConflicted;
        }
    }
}
