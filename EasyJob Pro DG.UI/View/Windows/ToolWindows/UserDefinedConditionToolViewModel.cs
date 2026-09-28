using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.Windows.ToolWindows;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows.ToolWindows
{
    internal class UserDefinedConditionToolViewModel : ToolWindowViewModelBase
    {
        // ----- Connected Services -----

        private IMessageDialogService _messageDialogService => Services.ServicesHandler.GetServicesAccess().MessageDialogServiceAccess;
        private IUserDefinedConditionService _userDefinedConditionService => Services.ServicesHandler.GetServicesAccess().UserDefindedConditionServiceAccess;

        public string DescriptionText =>
            $"User-defined conditions provide notifications (same like conflicts) when conditions met.";


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


        #region Conditions grid

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


        // ----- Command methods -----

        private bool RemoveSelectedConditionCommandCanExecute(object obj)
        {
            if (SelectedCondition != null)
                return true;
            return false;
        }

        private void OnRemoveSelectedConditionCommandExecuted(object obj)
        {
            ActiveConditions.Remove(SelectedCondition);
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
        }

        // ----- Private methods -----

        /// <summary>
        /// Re-set condition numer in list for each of existing conditions.
        /// </summary>
        private void ReviseConditonNumbersInList()
        {
            foreach (UserDefinedConditionWrapper condition in ActiveConditions)
            {
                condition.ConditionNumberInList = (uint)ActiveConditions.IndexOf(condition);
                condition.RefreshView();
            }
        }

        #endregion

        #region Add/edit conditions

        public UserDefinedConditionWrapper EditableCondition { get; set; }
        public ICommand AddNewCondition { get; set; }

        /// <summary>
        /// Text to be bound to 'Add' or 'Save' button content
        /// </summary>
        public string AddButtonText { get; private set; }

        /// <summary>
        /// List of ConditionTypes description to be used in Add new condition combo box.
        /// </summary>
        public List<string> ConditionTypesList { get; private set; }

        /// <summary>
        /// Index of selected item from <see cref="ConditionTypesList"/>
        /// </summary>
        public int SelectedConditionIndex
        {
            get => selectedConditionIndex;
            set
            {
                if (selectedConditionIndex == value) return;
                selectedConditionIndex = value;
                OnSelectedConditionTypeIndexChanged();
            }
        }

        /// <summary>
        /// Property bound to Add/edit condition Value text ('Enter value')
        /// </summary>
        public string EditableConditionValueText
        {
            get => editableConditionValueText;
            set
            {
                editableConditionValueText = value;
                OnEditableConditionValueTextChanged();
            }
        }

        /// <summary>
        /// Property bound to Add/edit condition Value text ('Enter value')
        /// </summary>
        public string EditableConditionCommentText{ get; set; }

        /// <summary>
        /// Text property bound to hint textbox below Value textbox to assist with correct entry.
        /// </summary>
        public string AddConditionHintText { get; private set; }

        private void OnEditableConditionValueTextChanged()
        {
            EditableCondition.ConditionType = (Model.UserDefinedConflicts.UserDefinedConditionType)selectedConditionIndex;
            EditableCondition.ConditionValue = editableConditionValueText;
            EditableCondition.ConditionComment = EditableConditionCommentText;
            EditableCondition.CellPosition = GetCellPosition();

            AddConditionHintText = editableConditionValueText;

            CheckIfEditableConditionIsValid();
            RefreshEditableConditionView();
        }

        private void CheckIfEditableConditionIsValid()
        {
            if(EditableCondition.CellPosition is not null && !string.IsNullOrEmpty(EditableCondition.ConditionValue))
                _isEditableConditionValid = true;
        }

        private CellPosition GetCellPosition()
        {
            return new CellPosition();
        }

        private void OnSelectedConditionTypeIndexChanged()
        {
            //throw new NotImplementedException();
        }

        private bool _isEditableConditionValid;
        private int selectedConditionIndex;
        private string editableConditionValueText;


        // ----- Command methods -----

        private bool AddNewConditionCommandCanExecute(object obj)
        {
            if (_isEditableConditionValid)
                return true;
            return false;
        }

        private void OnAddNewConditionCommandExecuted(object obj)
        {
            EditableCondition.ConditionDescription = $"{editableConditionValueText}: {selectedConditionIndex}";
            EditableCondition.ToolTipText = EditableConditionCommentText;
            EditableCondition.ConditionNumberInList = (uint)ActiveConditions.Count + 1;

            ActiveConditions.Add(EditableCondition);
            EditableCondition = new(new());
            ResetEditableFields();
            RefreshEditableConditionView();
        }

        private void ResetEditableFields()
        {
            editableConditionValueText = string.Empty;
            EditableConditionCommentText = string.Empty;
            AddConditionHintText = string.Empty;
        }

        private void RefreshEditableConditionView()
        {
            OnPropertyChanged(null);
        }

        #endregion


        #region Common command methods

        /// <summary>
        /// On 'Select' button pressed
        /// </summary>
        /// <param name="obj"></param>
        protected override void OnApplyExecuted(object obj)
        {
            throw new NotImplementedException();
        }

        protected override void OnClearCommandExecuted(object obj)
        {
            bool removed = _messageDialogService.ShowYesNoDialog($"Do you actually wish to remove all ({ActiveConditions.Count}) active conditions?", "Delete all conditions") == MessageDialogResult.Yes;
            if (!removed) return;

            ActiveConditions.Clear();
        }

        protected override bool OnClearCanExecute(object obj)
        {
            if (ActiveConditions.Count > 0)
                return true;
            return false;
        }

        #endregion





        private void SelectedConditionEdit()
        {
            throw new NotImplementedException();
        }


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

            AddNewCondition = new DelegateCommand(OnAddNewConditionCommandExecuted, AddNewConditionCommandCanExecute);
        }

        private void SetInitialTextValues()
        {  
            AddButtonText = "Test";
            AddConditionHintText = "Test hint text here";
        }

        private void LoadData()
        {
            ActiveConditions = _userDefinedConditionService.GetConditionWrappers;
            ReviseConditonNumbersInList();

            ConditionTypesList = conditionTypesText;

            EditableCondition = new UserDefinedConditionWrapper(new Model.UserDefinedConflicts.UserDefinedCondition());
        }



        private List<string> conditionTypesText => new()
        {
            //General = 0,
            "",
            //ContainerNumberLoaded, 1
            "Loaded container with number: ",
            //ContainerNumberDischarged, 2
            "Discharged container with number: ",

            //CellPositionLoaded, 3
            "Cell position is loaded:",
            //CellPositionEmpty, 4
            "Cell position is empty:",

            //CellPositionLoadedWithReefer, 5
            "Reefer is loaded in position:",
            //CellPositionLoadedWithDg, 6
            "Dg is loaded in position:",

            //ContainerLocationChanged, 7
            "Stowage position changed of container:",
            //ContainerPODchanged, 8
            "Port of discharging changed of container:",


            //DgNameContainsLoaded, 9
            "Any DG has in its Proper Shipping Name:",
            //DgTechnicalNameContains 10
            "DG has in its Technical Name:",
            //DgClassLoaded, 11
            "Loaded DG class:",
            //DgUnnoLoaded, 12
            "Loaded UNNO:",
            //DgFPLoaded 13
            "Loaded DG with flash point:",
            //ReeferContainingDgLoaded 14
            "DG under temperature control loaded:",

            //ReeferSetPointLoaded, 15
            "Loaded Reefer with set point:",
            //ReeferCommodityContains, 16
            "Loaded Reefer with commodity:",

            //NoNetWeightUnitsOnboard, 17
            "Loaded DG units without net weight.",
            //POLUnitsOnboard, 18
            "Loaded units with POL:",
            //PODUnitsOnboard, 19
            "Loaded units with POD:",

            //ContainerNumberNotInPosition, 20
            "Container number is not in position:",

            //FreeText = 255 21
            "Free text"
        };

        #endregion


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
    }
}
