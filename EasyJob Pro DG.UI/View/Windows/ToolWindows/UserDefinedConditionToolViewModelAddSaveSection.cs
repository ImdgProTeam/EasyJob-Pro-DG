using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.Model.UserDefinedConflicts;
using EasyJob_ProDG.UI.Wrapper;
using EasyJob_ProDG.UI.Wrapper.UserDefinedConflicts;
using System.Collections.Generic;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.Windows.ToolWindows
{
    internal partial class UserDefinedConditionToolViewModel
    {
        #region Text constants
        // ----- Text constants -----
        const string ENTER_VALUE = "Enter value";
        const string ENTER_TEXT = "Enter text";
        const string ENTER_CONTAINER_NUMBER = "Enter container number";
        const string ENTER_NO_VALUE_REQUIRED = "No value required";
        const string ENTER_PORT_CODE = "Enter port code";
        const string ENTER_POSITION = "Enter position";
        const string ENTER_PART_OF_NAME = "Enter part of name";
        const string ENTER_DG_CLASS = "Enter valid DG class";
        const string ENTER_UNNO = "Enter UNNO";

        const string ADD_BUTTON_TEXT = "Add";
        const string ADD_BUTTON_TOOLTIP = "Add condition";
        const string SAVE_BUTTON_TEXT = "Save";
        const string SAVE_BUTTON_TOOLTIP = "Save condition";

        private List<string> conditionTypesText => new()
        {
            //General = 0,
            "Select option",
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


        #region Private fields
        // ----- Private fields -----

        private string _correctEditableConditionValue;
        private string _editableConditionCommentText;
        private bool _editableConditionValueIsValid;
        private bool _isEditingExistingCondition = false;

        private UserDefinedConditionOption _correctEditableConditionOption;

        #endregion


        #region Public properties
        // ----- Public properties -----

        public UserDefinedConditionWrapper EditableCondition { get; set; }

        /// <summary>
        /// Text to be bound to 'Add' or 'Save' button content
        /// </summary>
        public string AddButtonText { get; private set; }

        /// <summary>
        /// Text to be bound to 'Add' or 'Save' button ToolTip
        /// </summary>
        public string AddButtonToolTip { get; private set; }

        /// <summary>
        /// Text property bound to hint textbox at the Value textbox to assist with correct entry.
        /// </summary>
        public string AddConditionHintText { get; private set; }

        /// <summary>
        /// Property bound to Add/edit condition Value text ('Enter value')
        /// </summary>
        public string EditableConditionCommentText
        {
            get => _editableConditionCommentText;
            set => _editableConditionCommentText = value.RemoveForbiddenSymbols();
        }

        public ICommand AddNewCondition { get; set; }
        public ICommand ClearConditionFieldsCommand { get; set; }

        #endregion

        #region Private methods
        // ----- Private methods -----

        /// <summary>
        /// Sets <see cref="AddConditionHintText"/> according to chosen type and inputs
        /// </summary>
        private void SetConditionHintText()
        {
            AddConditionHintText = UserDefinedConditionsToolViewModelHelpers.GenerateConditionHintText
                                                                                    (selectedConditionIndex,
                                                                                    _correctEditableConditionValue, EditableConditionCellPosition, _correctEditableConditionOption);
            OnPropertyChanged(nameof(AddConditionHintText));
        }

        /// <summary>
        /// Checks if all required fields of the <see cref="EditableCondition"/> fulfil to add the UDC.
        /// </summary>
        private bool CheckIfEditableConditionIsValid()
        {
            if (!_editableConditionValueIsValid)
            {
                return false;
            }

            switch (selectedConditionIndex)
            {
                //FreeText = 255 21
                case 21:
                case (byte)UserDefinedConditionType.General:
                case (byte)UserDefinedConditionType.ContainerNumberLoaded:
                case (byte)UserDefinedConditionType.ContainerNumberDischarged:
                case (byte)UserDefinedConditionType.ContainerLocationChanged:
                case (byte)UserDefinedConditionType.ContainerPODchanged:
                case (byte)UserDefinedConditionType.DgNameContainsLoaded:
                case (byte)UserDefinedConditionType.DgTechnicalNameContains:
                case (byte)UserDefinedConditionType.DgClassLoaded:
                case (byte)UserDefinedConditionType.DgUnnoLoaded:
                case (byte)UserDefinedConditionType.DgFPLoaded:
                case (byte)UserDefinedConditionType.ReeferContainingDgLoaded:
                case (byte)UserDefinedConditionType.ReeferSetPointLoaded:
                case (byte)UserDefinedConditionType.ReeferCommodityContains:
                case (byte)UserDefinedConditionType.NoNetWeightUnitsOnboard:
                case (byte)UserDefinedConditionType.POLUnitsOnboard:
                case (byte)UserDefinedConditionType.PODUnitsOnboard:
                    break;

                case (byte)UserDefinedConditionType.ContainerNumberNotInPosition:
                case (byte)UserDefinedConditionType.CellPositionLoaded:
                case (byte)UserDefinedConditionType.CellPositionEmpty:
                case (byte)UserDefinedConditionType.CellPositionLoadedWithReefer:
                case (byte)UserDefinedConditionType.CellPositionLoadedWithDg:
                    if (EditableConditionCellPosition.IsEmpty)
                        return false;
                    break;

                default:
                    break;
            }

            return true;
        }

        /// <summary>
        /// Sets visual properties on Add/Save button according to 'Add'
        /// </summary>
        private void SetAddButtonProperties()
        {
            AddButtonText = ADD_BUTTON_TEXT;
            AddButtonToolTip = ADD_BUTTON_TOOLTIP;
            OnPropertyChanged(nameof(AddButtonText));
            OnPropertyChanged(nameof(AddButtonToolTip));
        }

        /// <summary>
        /// Sets visual properties on Add/Save button according to 'Save'
        /// </summary>
        private void SetSaveButtonProperties()
        {
            AddButtonText = SAVE_BUTTON_TEXT;
            AddButtonToolTip = SAVE_BUTTON_TOOLTIP;
            OnPropertyChanged(nameof(AddButtonText));
            OnPropertyChanged(nameof(AddButtonToolTip));
        }

        /// <summary>
        /// Changes isEditing flag to false;
        /// </summary>
        private void StopEditing()
        {
            _isEditingExistingCondition = false;
        }

        /// <summary>
        /// Used to correctly display <>= symbols when edit conditons with numerical values.
        /// </summary>
        /// <param name="conditionValue"></param>
        /// <returns></returns>
        private string CreateEditableConditionValueTextFromConditionValue(string conditionValue)
        {
            return UserDefinedConditionsToolViewModelHelpers.CreateEnterValueFromConditionValue(conditionValue, selectedConditionIndex, _correctEditableConditionOption);
        }

        #endregion


        #region TextBox ConditionValue

        // ----- Value TextBox -----

        private string _editableConditionValueText = string.Empty;

        /// <summary>
        /// Property bound to Add/edit condition Value text ('Enter value')
        /// </summary>
        public string EditableConditionValueText
        {
            get => _editableConditionValueText;
            set
            {
                _editableConditionValueText = value.RemoveForbiddenSymbols();
                OnEditableConditionValueTextChanged();
            }
        }

        private void OnEditableConditionValueTextChanged()
        {
            CheckCorrectInputWithSelectedIndex();

            TryParseAndValidateValue();

            SetConditionHintText();
        }

        private void TryParseAndValidateValue()
        {
            _editableConditionValueIsValid =
                _editableConditionValueText.TryParseValue(selectedConditionIndex,
                                                     out _correctEditableConditionValue,
                                                     out _correctEditableConditionOption);
        }


        /// <summary>
        /// Handles individual cases of input, e.g. when required to change input field or selected index.
        /// </summary>
        private void CheckCorrectInputWithSelectedIndex()
        {
            // Switch to 'Free text' from 'Select option'
            if (selectedConditionIndex == 0)
                SelectedConditionIndex = 21;

            // Sync typing with 'CellPoisition' in case when value expects for position
            if (selectedConditionIndex >= 3 && selectedConditionIndex <= 6)
            {
                EditableConditionCellPositionText = _editableConditionValueText;
                OnPropertyChanged(nameof(EditableConditionCellPositionText));
            }
        }

        public bool IsEditableConditionValueTextBoxEnabled { get; private set; } = true;
        public string EditableConditionValueTextBoxTag { get; private set; }

        #endregion


        #region TextBox CellPosition
        // ----- CellPosition TextBox -----

        private string _editableConditionCellPositionText;
        public CellPositionWrapper EditableConditionCellPosition { get; set; }

        /// <summary>
        /// Property bound to Add/edit condition CellPosition text ('Enter value')
        /// </summary>
        public string EditableConditionCellPositionText
        {
            get => _editableConditionCellPositionText;
            set
            {
                _editableConditionCellPositionText = value;
                EditableConditionCellPosition.DisplayPosition = value;
                CellPositionHintText = EditableConditionCellPosition.DisplayPosition;
                OnPropertyChanged(nameof(CellPositionHintText));

                SetConditionHintText();
            }
        }

        public bool IsEditableConditionCellPositionTextBoxEnabled { get; private set; } = true;
        public string EditableConditionCellPositionTextBoxTag { get; private set; }

        /// <summary>
        /// Text property bound to hint textbox at the CellPosition textbox to assist with correct entry.
        /// </summary>
        public string CellPositionHintText { get; private set; }

        #endregion


        #region ComboBox Condition type
        // ----- ComboBox selection of ConditionType -----

        /// <summary>
        /// List of ConditionTypes description to be used in Add new condition combo box.
        /// </summary>
        public List<string> ConditionTypesList { get; private set; }

        private int selectedConditionIndex;

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
        /// Sets visual attributes and text to the panel controls depending on the selected Type.
        /// </summary>
        private void OnSelectedConditionTypeIndexChanged()
        {
            // Set enabled property of CellPosition TextBox and tag text of Value TextBox
            switch (selectedConditionIndex)
            {
                case (byte)UserDefinedConditionType.General:
                //FreeText = 255 21
                case 21:
                    EditableConditionValueTextBoxTag = ENTER_TEXT;
                    DisableCellPositionTextBox();
                    break;

                case (byte)UserDefinedConditionType.ContainerLocationChanged:
                case (byte)UserDefinedConditionType.ContainerPODchanged:
                    EditableConditionValueTextBoxTag = ENTER_CONTAINER_NUMBER;
                    DisableCellPositionTextBox();
                    break;

                case (byte)UserDefinedConditionType.NoNetWeightUnitsOnboard:
                    EditableConditionValueTextBoxTag = ENTER_NO_VALUE_REQUIRED;
                    DisableCellPositionTextBox();
                    break;

                case (byte)UserDefinedConditionType.POLUnitsOnboard:
                case (byte)UserDefinedConditionType.PODUnitsOnboard:
                    EditableConditionValueTextBoxTag = ENTER_PORT_CODE;
                    EnableCellPositionTextBox();
                    break;

                case (byte)UserDefinedConditionType.ContainerNumberLoaded:
                case (byte)UserDefinedConditionType.ContainerNumberDischarged:
                case (byte)UserDefinedConditionType.ContainerNumberNotInPosition:
                    EditableConditionValueTextBoxTag = ENTER_CONTAINER_NUMBER;
                    EnableCellPositionTextBox();
                    break;

                case (byte)UserDefinedConditionType.CellPositionLoaded:
                case (byte)UserDefinedConditionType.CellPositionEmpty:
                case (byte)UserDefinedConditionType.CellPositionLoadedWithReefer:
                case (byte)UserDefinedConditionType.CellPositionLoadedWithDg:
                    EditableConditionValueTextBoxTag = ENTER_POSITION;
                    EnableCellPositionTextBox();
                    if (string.IsNullOrWhiteSpace(EditableConditionCellPositionText)
                        && !string.IsNullOrWhiteSpace(EditableConditionValueText))
                        EditableConditionCellPositionText = EditableConditionValueText;
                    break;

                case (byte)UserDefinedConditionType.DgNameContainsLoaded:
                case (byte)UserDefinedConditionType.DgTechnicalNameContains:
                case (byte)UserDefinedConditionType.ReeferCommodityContains:
                    EditableConditionValueTextBoxTag = ENTER_PART_OF_NAME;
                    EnableCellPositionTextBox();
                    break;

                case (byte)UserDefinedConditionType.DgClassLoaded:
                    EditableConditionValueTextBoxTag = ENTER_DG_CLASS;
                    EnableCellPositionTextBox();
                    break;
                case (byte)UserDefinedConditionType.DgUnnoLoaded:
                    EditableConditionValueTextBoxTag = ENTER_UNNO;
                    EnableCellPositionTextBox();
                    break;
                case (byte)UserDefinedConditionType.DgFPLoaded:
                case (byte)UserDefinedConditionType.ReeferSetPointLoaded:
                    EditableConditionValueTextBoxTag = ENTER_VALUE;
                    EnableCellPositionTextBox();
                    break;
                case (byte)UserDefinedConditionType.ReeferContainingDgLoaded:
                    EditableConditionValueTextBoxTag = ENTER_NO_VALUE_REQUIRED;
                    EnableCellPositionTextBox();
                    break;

                default:
                    EditableConditionValueTextBoxTag = ENTER_VALUE;
                    EnableCellPositionTextBox();
                    break;

            }

            TryParseAndValidateValue();
            SetConditionHintText();
            RefreshEditableConditionView();
        }

        private void DisableCellPositionTextBox()
        {
            IsEditableConditionCellPositionTextBoxEnabled = false;
            EditableConditionCellPositionTextBoxTag = string.Empty;
            EditableConditionCellPositionText = string.Empty;
            CellPositionHintText = string.Empty;
        }

        private void EnableCellPositionTextBox()
        {
            IsEditableConditionCellPositionTextBoxEnabled = true;
            EditableConditionCellPositionTextBoxTag = ENTER_POSITION;
            if (string.IsNullOrWhiteSpace(EditableConditionCellPositionText))
                CellPositionHintText = CellPosition.ANY_POSITION;
        }

        #endregion


        #region Command methods
        // ----- Command methods -----

        private bool AddSaveNewConditionCommandCanExecute(object obj)
        {
            return CheckIfEditableConditionIsValid();
        }

        private void OnAddSaveNewConditionCommandExecuted(object obj)
        {
            EditableCondition.ConditionType = selectedConditionIndex.GetProperUserDefinedConditionType();
            EditableCondition.ConditionComment = EditableConditionCommentText;
            EditableCondition.CellPosition = EditableConditionCellPosition.Model.Clone();
            EditableCondition.ConditionValue = _correctEditableConditionValue;
            EditableCondition.ConditionOption = _correctEditableConditionOption;
            EditableCondition.UpdateDescription();


            if (!_isEditingExistingCondition)
            {
                EditableCondition.ConditionNumberInList = (uint)ActiveConditions.Count + 1;
                _userDefinedConditionService.AddCondition(EditableCondition.Clone(), out bool exists);
            }
            else
            {
                _userDefinedConditionService.SaveConditions();
                EditableCondition.RefreshView();
            }

            ClearAddSection();
            SetAddButtonProperties();
            StopEditing();
        }

        private void SelectedConditionEdit()
        {
            _isEditingExistingCondition = true;
            EditableCondition = SelectedCondition;

            selectedConditionIndex = (int)EditableCondition.ConditionType;

            _correctEditableConditionOption = EditableCondition.ConditionOption;
            _editableConditionValueText = CreateEditableConditionValueTextFromConditionValue(EditableCondition.ConditionValue);
            _correctEditableConditionValue = EditableCondition.ConditionValue;
            EditableConditionCellPosition = new CellPositionWrapper(EditableCondition.CellPosition.Clone());
            _editableConditionCellPositionText = EditableConditionCellPosition.DisplayPosition;
            CellPositionHintText = EditableConditionCellPosition.DisplayPosition;
            EditableConditionCommentText = EditableCondition.ConditionComment;

            OnSelectedConditionTypeIndexChanged();
            RefreshEditableConditionView();
            SetConditionHintText();
        }

        /// <summary>
        /// Clears all the 'Add' section
        /// </summary>
        private void ClearAddSection()
        {
            EditableCondition = new(new());
            ResetEditableFields();
            StopEditing();
            SetAddButtonProperties();
            RefreshEditableConditionView();
        }

        /// <summary>
        /// Clears all editable fields in add condition section (after add or new)
        /// </summary>
        private void ResetEditableFields()
        {
            EditableConditionValueText = string.Empty;
            EditableConditionCellPositionText = string.Empty;
            EditableConditionCommentText = string.Empty;

            AddConditionHintText = string.Empty;
            CellPositionHintText = string.Empty;
        }

        /// <summary>
        /// Refreshes all bindable properties
        /// </summary>
        private void RefreshEditableConditionView()
        {
            OnPropertyChanged(null);
        }

        #endregion

    }
}
