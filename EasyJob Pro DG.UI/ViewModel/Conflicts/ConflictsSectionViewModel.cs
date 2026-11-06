using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Utility.Messages;
using EasyJob_ProDG.UI.ViewModel.Conflicts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class ConflictsSectionViewModel : Observable
    {

        #region Text
        const string DEFAULT_HEADER_TEXT = "List of conflicts";
        const string DEFAULT_NO_CONFLICTS_HEADER_TEXT = "No conflicts found";


        private Dictionary<ConflictTypes, string> ConflictTypeDisplayTextDictionary = new Dictionary<ConflictTypes, string>()
        {
            { ConflictTypes.None, "None" },
            { ConflictTypes.Segregation, "Segregation conflicts" },
            { ConflictTypes.Stowage, "Stowage conflicts" },
            { ConflictTypes.Info, "Information messages" },
            { ConflictTypes.Warning, "Warnings" },
            { ConflictTypes.UserDefined, "User-defined conflicts" },
            { ConflictTypes.VentRequirement, "Ventilation requirements" },
            { ConflictTypes.Handling, "Handling instructions" },
        };

        #endregion

        // Private fields
        ConflictListViewModel conflictListViewModel = ViewModelLocator.ConflictListViewModel;
        ConflictFilterButtonVM clearAllButton;

        // Public and internal members
        public string HeaderText { get; private set; } = DEFAULT_HEADER_TEXT;
        public ObservableCollection<ConflictFilterButtonVM> FilterButtons { get; private set; }
        public VentilationRequirements Vents { get; set; }

        internal bool IsFiltered => FilterButtons.Any(b => b.IsSelected);
        internal List<ConflictTypes> FilteredConflictTypes { get; private set; }



        #region Private methods
        // Private methods

        /// <summary>
        /// Calls Re-check of condition conflicts
        /// </summary>
        /// <param name="obj"></param>
        private void OnReCheckRequested(object obj)
        {
            DataMessenger.Default.Send(new ConflictsToBeCheckedAndUpdatedMessage(true));
        }

        /// <summary>
        /// Called when 'settings' button in the top right corner is pressed.
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ShowUserDefinedConditionWindowExecuted(object obj)
        {
            DataMessenger.Default.Send(new OpenWindowMessage(), DataMessengerContext.OPEN_USER_DEFINED_CONDITION_TOOL_WINDOW);
        }

        /// <summary>
        /// Creates <see cref="FilterButtons"/> and fills it with <see cref="ConflictFilterButtonVM"/> to be used to filter conflicts.
        /// </summary>
        private void CreateFilterButtons()
        {
            FilterButtons = new ObservableCollection<ConflictFilterButtonVM>()
            {
                new ConflictFilterButtonVM(ConflictTypes.Stowage, "SW")
                {
                    Hint=ConflictTypeDisplayTextDictionary[ConflictTypes.Stowage],
                },
                new ConflictFilterButtonVM(ConflictTypes.Segregation, "SG")
                {
                    Hint=ConflictTypeDisplayTextDictionary[ConflictTypes.Segregation],
                },
                new ConflictFilterButtonVM(ConflictTypes.VentRequirement, "Vent")
                {
                    Hint=ConflictTypeDisplayTextDictionary[ConflictTypes.VentRequirement],
                },
                new ConflictFilterButtonVM(ConflictTypes.Handling, "Hand")
                {
                    Hint=ConflictTypeDisplayTextDictionary[ConflictTypes.Handling],
                },
                new ConflictFilterButtonVM(ConflictTypes.Info, "Info")
                {
                    Hint=ConflictTypeDisplayTextDictionary[ConflictTypes.Info],
                },
                new ConflictFilterButtonVM(ConflictTypes.UserDefined, "User")
                {
                    Hint=ConflictTypeDisplayTextDictionary[ConflictTypes.UserDefined],
                },
            };
            foreach (var filterButton in FilterButtons)
            {
                filterButton.AssignedCommand = new DelegateCommand(OnFilterButtonPressed);
            }

            clearAllButton = new ConflictFilterButtonVM(ConflictTypes.None, "Clear", "Show all conflicts")
            {
                AssignedCommand = new DelegateCommand(ClearFilters),
                IsActive = false
            };
            FilterButtons.Add(clearAllButton);

            SetIsAlertBadgeOnFilterButtons(null, null);
        }

        private void ClearFilters(object obj)
        {
            foreach (var button in FilterButtons)
            {
                button.IsSelected = false;
                button.RefreshView();
            }
            FilteredConflictTypes.Clear();
            conflictListViewModel.SetConflictsFilter();

            EnableClearAllButton();
            SetIsFilteredTextOnHeader();
        }

        private void OnFilterButtonPressed(object obj)
        {
            var conflict = obj as ConflictFilterButtonVM;
            if (conflict is null) return;

            if (conflict.IsSelected)
                FilteredConflictTypes.Add(conflict.ConflictType);
            else
                FilteredConflictTypes.Remove(conflict.ConflictType);

            EnableClearAllButton();

            conflictListViewModel.SetConflictsFilter(FilteredConflictTypes);
            SetIsFilteredTextOnHeader();
        }

        /// <summary>
        /// Makes 'Clear all' button enabled or disabled depending on if any other button IsSelected.
        /// </summary>
        private void EnableClearAllButton()
        {
            if (FilteredConflictTypes.Count > 0)
                clearAllButton.IsActive = true;
            else clearAllButton.IsActive = false;
            clearAllButton.RefreshView();

        }

        /// <summary>
        /// Sets alert badges on top of buttons if conflict type exists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetIsAlertBadgeOnFilterButtons(object sender, EventArgs e)
        {
            foreach (var button in FilterButtons)
            {
                if (conflictListViewModel.ExistingConflictTypes.Contains(button.ConflictType))
                    button.IsAlert = true;
                else button.IsAlert = false;
                button.RefreshView();
            }
        }

        private void SetIsFilteredTextOnHeader()
        {
            if (FilteredConflictTypes.Count == 0)
            {
                HeaderText = conflictListViewModel.HasNoConflicts
                    ? DEFAULT_NO_CONFLICTS_HEADER_TEXT
                 : DEFAULT_HEADER_TEXT;
            }
            else if (FilteredConflictTypes.Count == 1)
            {
                HeaderText = $"Filtered: {ConflictTypeDisplayTextDictionary[FilteredConflictTypes[0]]}";
            }
            else { HeaderText = $"{DEFAULT_HEADER_TEXT} (filtered)"; }
            OnPropertyChanged(nameof(HeaderText));
        }

        private void OnDisplayConflictsToBeRefreshedMessageReceived(DisplayConflictsToBeRefreshedMessage message)
        {
            SetIsFilteredTextOnHeader();
        }

        #endregion

        #region Commands
        public ICommand ReCheckCommand { get; private set; }
        public ICommand ShowUserDefinedConditionWindowCommand { get; private set; }

        private void LoadCommands()
        {
            ReCheckCommand = new DelegateCommand(OnReCheckRequested);
            ShowUserDefinedConditionWindowCommand = new DelegateCommand(ShowUserDefinedConditionWindowExecuted);
        }

        #endregion

        #region Constructor

        public ConflictsSectionViewModel()
        {
            LoadCommands();
            RegisterInMessenger();

            FilteredConflictTypes = new();

            conflictListViewModel.DisplayConflictsSet -= SetIsAlertBadgeOnFilterButtons;
            conflictListViewModel.DisplayConflictsSet += SetIsAlertBadgeOnFilterButtons;

            CreateFilterButtons();
        }

        private void RegisterInMessenger()
        {
            DataMessenger.Default.Register<DisplayConflictsToBeRefreshedMessage>(this, OnDisplayConflictsToBeRefreshedMessageReceived, DataMessengerContext.UPDATE_CONFLICTS);
        }

        #endregion
    }
}
