using EasyJob_ProDG.Model.UserDefinedConflicts;
using EasyJob_ProDG.UI.Wrapper.UserDefinedConflicts;
using System;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.ViewModel.Conflicts
{
    public class PanelConflictItemUserDefinedViewModel : PanelConflictItemBaseViewModel
    {
        // ----- Constants -----

        const string GROUP_TITLE_USER_DEFINED = "User-defined conflicts";
        const int MAX_NUMBER_OF_CONTAINER_NUMBERS_IN_DESCRIPTION = 10;


        // ----- Private fields -----
        
        private string _title;
        private string _description;


        #region Display properties
        // ----- Display properties -----

        public override string ConflictGroupTitle => GROUP_TITLE_USER_DEFINED;
        public override string DisplayConflictHeader => _title;

        #endregion


        // ----- Inhereted override methods -----

        protected override string CreateDisplayText()
        {
            return _description;
        }


        // ----- Private methods -----

        /// <summary>
        /// Creates PanelConflictItem title and description as out parameters, depending on <see cref="UserDefinedConditionType"/> of the <see cref="UserDefinedConflict"/>
        /// </summary>
        /// <param name="conflict"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        private bool ParseUserDefinedConflict(UserDefinedConflict conflict, out string _title, out string _description)
        {
            var condition = conflict.ConditionReference;

            string conditionDescription = condition.GetDescription();
            _title = string.Empty;
            _description = string.Empty;

            try
            {
                _title = GetTitle(conflict);

                switch (conflict.ConditionReference.ConditionType)
                {
                    // Single unit conflicts

                    case UserDefinedConditionType.ContainerNumberLoaded:
                    case UserDefinedConditionType.ContainerNumberDischarged:
                        _description += conditionDescription;
                        break;

                    case UserDefinedConditionType.CellPositionLoaded:
                    case UserDefinedConditionType.CellPositionEmpty:
                    case UserDefinedConditionType.CellPositionLoadedWithReefer:
                    case UserDefinedConditionType.CellPositionLoadedWithDg:
                    case UserDefinedConditionType.ContainerLocationChanged:
                    case UserDefinedConditionType.ContainerPODchanged:
                    case UserDefinedConditionType.ContainerNumberNotInPosition:
                        _description = $"{conflict.ContainerNumber}\n {conditionDescription}";
                        break;

                    // Multi-unit conflicts
                    case UserDefinedConditionType.DgNameContainsLoaded:
                    case UserDefinedConditionType.DgTechnicalNameContains:
                    case UserDefinedConditionType.DgClassLoaded:
                    case UserDefinedConditionType.DgUnnoLoaded:
                    case UserDefinedConditionType.DgFPLoaded:
                    case UserDefinedConditionType.ReeferContainingDgLoaded:
                    case UserDefinedConditionType.ReeferCommodityContains:
                    case UserDefinedConditionType.ReeferSetPointLoaded:
                    case UserDefinedConditionType.NoNetWeightUnitsOnboard:
                    case UserDefinedConditionType.POLUnitsOnboard:
                    case UserDefinedConditionType.PODUnitsOnboard:
                        _description = $"{conditionDescription}:{FollowContainerNumberList(conflict.ContainerNumbers)}";
                        break;

                    case UserDefinedConditionType.General:
                    case UserDefinedConditionType.FreeText:
                        _description = condition.ConditionValue;
                        break;
                }
            }
            catch (Exception ex)
            {
                EasyJob_ProDG.Data.LogWriter.WriteError($"Creating description for user-defined condition caused error: {ex.Message}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Creates list of container numbers to be used in the description in required format depending on the count of such numbers. 
        /// </summary>
        /// <param name="containerNumbers"></param>
        /// <returns></returns>
        private string FollowContainerNumberList(List<string> containerNumbers)
        {
            string result = string.Empty;
            switch (containerNumbers.Count)
            {
                case 0:
                    result = "NIL";
                    break;
                case 1:
                    result = $" {containerNumbers[0]}";
                    break;
                case <= MAX_NUMBER_OF_CONTAINER_NUMBERS_IN_DESCRIPTION:
                    foreach (var number in containerNumbers)
                    {
                        result += $"\n{number}";
                    }
                    break;
                default:
                    for (int i = 0; i < MAX_NUMBER_OF_CONTAINER_NUMBERS_IN_DESCRIPTION; i++)
                    {
                        result += $"\n{containerNumbers[i]}";
                    }
                    result += "\n...";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Generates title text depending on conflict <see cref="UserDefinedConditionType"/>
        /// </summary>
        /// <param name="conflict"></param>
        /// <returns></returns>
        private string GetTitle(UserDefinedConflict conflict)
        {
            string result = string.Empty;
            var condition = conflict.ConditionReference;

            switch (condition.ConditionType)
            {
                case UserDefinedConditionType.ContainerNumberLoaded:
                case UserDefinedConditionType.ContainerNumberDischarged:
                    result = $"{conflict.ContainerNumber}!";
                    break;
                case UserDefinedConditionType.CellPositionLoaded:
                    result = $"Cell is loaded!";
                    break;
                case UserDefinedConditionType.CellPositionEmpty:
                    result = $"Cell is empty!";
                    break;
                case UserDefinedConditionType.CellPositionLoadedWithReefer:
                    result = $"Reefer is loaded!";
                    break;
                case UserDefinedConditionType.CellPositionLoadedWithDg:
                    result = $"DG is loaded!";
                    break;
                case UserDefinedConditionType.ContainerLocationChanged:
                    result = $"Position changed!";
                    break;
                case UserDefinedConditionType.ContainerPODchanged:
                    result = $"POD changed!";
                    break;
                case UserDefinedConditionType.DgNameContainsLoaded:
                    result = $"Proper shipping name!";
                    break;
                case UserDefinedConditionType.DgTechnicalNameContains:
                    result = $"Technical name!";
                    break;
                case UserDefinedConditionType.DgClassLoaded:
                    result = $"DG class!";
                    break;
                case UserDefinedConditionType.DgUnnoLoaded:
                    result = $"UNNO!";
                    break;
                case UserDefinedConditionType.DgFPLoaded:
                    result = $"Flash point!";
                    break;
                case UserDefinedConditionType.ReeferContainingDgLoaded:
                    result = $"Reefer with DG!";
                    break;
                case UserDefinedConditionType.ReeferSetPointLoaded:
                    result = $"Reefer set point!";
                    break;
                case UserDefinedConditionType.ReeferCommodityContains:
                    result = $"Reefer commodity!";
                    break;
                case UserDefinedConditionType.NoNetWeightUnitsOnboard:
                    result = $"No net weight!";
                    break;
                case UserDefinedConditionType.POLUnitsOnboard:
                    result = $"Port of loading!";
                    break;
                case UserDefinedConditionType.PODUnitsOnboard:
                    result = $"Port of discharging!";
                    break;
                case UserDefinedConditionType.ContainerNumberNotInPosition:
                    result = $"Container not in position!";
                    break;

                case UserDefinedConditionType.General:
                case UserDefinedConditionType.FreeText:
                    result = (condition.ConditionValue.Length > 12)
                           ? $"{condition.ConditionValue.Substring(0, 10)}..."
                           : condition.ConditionValue;
                    break;
            }
            return result;
        }

        #region Constructor
        // ----- Public constructor -----

        public PanelConflictItemUserDefinedViewModel(UserDefinedConflict conflict) : base()
        {
            ParseUserDefinedConflict(conflict, out _title, out _description);
        }

        #endregion
    }
}
