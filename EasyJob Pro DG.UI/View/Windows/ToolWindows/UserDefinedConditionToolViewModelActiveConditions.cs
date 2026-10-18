using EasyJob_ProDG.UI.Wrapper;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.Windows.ToolWindows
{
    internal partial class UserDefinedConditionToolViewModel
    {
        #region Conditions grid

        #region Bindable properties
        // ----- Bindable properties -----

        /// <summary>
        /// Selected condition from ActiveConditions grid.
        /// </summary>
        public UserDefinedConditionWrapper SelectedCondition { get; set; }

        /// <summary>
        /// Property used to bind conditions
        /// </summary>
        public ObservableCollection<UserDefinedConditionWrapper> ActiveConditions { get; set; }

        /// <summary>
        /// Command to remove selected UDCondition from ActiveConditions
        /// </summary>
        public ICommand RemoveSelectedConditonCommand { get; set; }

        /// <summary>
        /// Command to edit selected UDCondition from ActiveConditions.
        /// </summary>
        public ICommand EditSelectedConditonCommand { get; set; }

        #endregion

        #region Command methods
        // ----- Command methods -----

        private bool RemoveSelectedConditionCommandCanExecute(object obj)
        {
            if (SelectedCondition != null)
                return true;
            return false;
        }

        private void OnRemoveSelectedConditionCommandExecuted(object obj)
        {
            _userDefinedConditionService.RemoveCondition(SelectedCondition);
            ReviseConditonNumbersInList();
        }

        private bool EditSelectedConditionCommandCanExecute(object obj)
        {
            if (SelectedCondition != null)
                return true;
            return false;
        }

        private void OnEditSelectedConditionCommandExecuted(object obj)
        {
            SelectedConditionEdit();
            SetSaveButtonProperties();
        }

        #endregion

        #region Private methods
        // ----- Private methods -----

        /// <summary>
        /// Re-set condition numer in list for each of existing conditions.
        /// </summary>
        private void ReviseConditonNumbersInList()
        {
            foreach (UserDefinedConditionWrapper condition in ActiveConditions)
            {
                condition.ConditionNumberInList = (uint)ActiveConditions.IndexOf(condition) + 1;
                condition.RefreshView();
            }
        }

        #endregion

        #endregion
    }
}
