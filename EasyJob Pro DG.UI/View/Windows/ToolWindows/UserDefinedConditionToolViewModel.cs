using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using EasyJob_ProDG.UI.Wrapper.UserDefinedConflicts;
using System;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.View.Windows.ToolWindows
{
    internal partial class UserDefinedConditionToolViewModel : ToolWindowViewModelBase
    {
        #region Text constants
        // ----- Text constants -----

        public string DescriptionText =>
            $"User-defined conditions alert you with notifications in conflicts section. The alerts will show up when:";

        #endregion


        #region Connected services
        // ----- Connected Services -----

        private IMessageDialogService _messageDialogService => Services.ServicesHandler.GetServicesAccess().MessageDialogServiceAccess;
        private IUserDefinedConditionService _userDefinedConditionService => Services.ServicesHandler.GetServicesAccess().UserDefindedConditionServiceAccess;

        #endregion



        /// <summary>
        /// Assigns Selected units list to ItemsToSelect property of a selected DataGrid
        /// </summary>
        private void SelectSelectedUnits()
        {
            switch (selectedDataGridIndex)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        break;
                    }
                case 2:
                    {
                        break;
                    }
                case 3:
                    {
                        break;
                    }
                default:
                    break;
            }
        }


        #region Common command methods

        protected override void OnApplyExecuted(object obj)
        {
            throw new NotImplementedException();
        }

        protected override void OnClearCommandExecuted(object obj)
        {
            bool removed = _messageDialogService.ShowYesNoDialog($"Do you actually wish to remove all ({ActiveConditions.Count}) active conditions?", "Delete all conditions") == MessageDialogResult.Yes;
            if (!removed) return;

            _userDefinedConditionService.ClearConditions();
        }

        protected override bool OnClearCanExecute(object obj)
        {
            if (ActiveConditions.Count > 0)
                return true;
            return false;
        }

        private void OnClearConditionFieldsCommandExecuted(object obj)
        {
            ClearAddSection();
        }

        #endregion


        #region Constructor

        public UserDefinedConditionToolViewModel() : base()
        {
            LoadCommands();
            LoadData();
            //LoadTestData();

            SetInitialTextValues();
        }

        private void LoadCommands()
        {
            RemoveSelectedConditonCommand = new DelegateCommand(OnRemoveSelectedConditionCommandExecuted, RemoveSelectedConditionCommandCanExecute);
            EditSelectedConditonCommand = new DelegateCommand(OnEditSelectedConditionCommandExecuted, EditSelectedConditionCommandCanExecute);

            AddNewCondition = new DelegateCommand(OnAddSaveNewConditionCommandExecuted, AddSaveNewConditionCommandCanExecute);
            ClearConditionFieldsCommand = new DelegateCommand(OnClearConditionFieldsCommandExecuted);
        }

        private void SetInitialTextValues()
        {
            SetAddButtonProperties();
            AddConditionHintText = "Choose options below";
            EditableConditionValueTextBoxTag = "Enter text";
            OnSelectedConditionTypeIndexChanged();
        }

        private void LoadData()
        {
            ActiveConditions = _userDefinedConditionService.GetConditionWrappers;
            SetActiveConditionsDescription();
            ReviseConditonNumbersInList();

            ConditionTypesList = conditionTypesText;

            EditableCondition = new UserDefinedConditionWrapper(new Model.UserDefinedConflicts.UserDefinedCondition());
            EditableConditionCellPosition = new CellPositionWrapper(new CellPosition());
            EditableConditionCellPosition.AcceptEmpty = true;
        }

        private void SetActiveConditionsDescription()
        {
            if (ActiveConditions.Count == 0) return;
            foreach (var condition in ActiveConditions)
            {
                condition.UpdateDescription();
            }
        }

        #endregion

        #region Test data

        /// <summary>
        /// Method can be called in constructor to get test data if connection to services is missing.
        /// </summary>
        private void LoadTestData()
        {
            ActiveConditions = new ObservableCollection<UserDefinedConditionWrapper>()
            {
                new UserDefinedConditionWrapper(new Model.UserDefinedConflicts.UserDefinedCondition())
                {
                    ConditionDescription = "This is a test condition no 1"
                },
                new UserDefinedConditionWrapper(new Model.UserDefinedConflicts.UserDefinedCondition())
                {
                    ConditionDescription = "This is a test condition no 2"
                },
                new UserDefinedConditionWrapper(new Model.UserDefinedConflicts.UserDefinedCondition())
                {
                    ConditionDescription = "This is a test condition no 3"
                },
                new UserDefinedConditionWrapper(new Model.UserDefinedConflicts.UserDefinedCondition())
                {
                    ConditionDescription = "This is a test condition no 4"
                },
            };

            ConditionTypesList = conditionTypesText;

            AddButtonText = "Test";
            AddConditionHintText = "Test hint text here";
        }

        #endregion
    }
}
