using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel.Conflicts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class ConflictsSectionViewModel : Observable
    {
        // Private fields
        IConflictDataService conflictDataService;
        ConflictListViewModel conflictListViewModel = ViewModelLocator.ConflictListViewModel;
        ConflictFilterButtonVM clearAllButton;

        // Public members
        public ObservableCollection<ConflictFilterButtonVM> FilterButtons { get; private set; }

        internal bool IsFiltered => FilterButtons.Any(b => b.IsSelected);
        internal List<ConflictTypes> FilteredConflictTypes { get; private set; }

        public VentilationRequirements Vents { get; set; }


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
        /// Creates <see cref="FilterButtons"/> and fills it with <see cref="ConflictFilterButtonVM"/> to be used to filter conflicts.
        /// </summary>
        private void CreateFilterButtons()
        {
            FilterButtons = new ObservableCollection<ConflictFilterButtonVM>()
            {
                new ConflictFilterButtonVM(ConflictTypes.Stowage, "SW")
                {
                    Hint="Stowage conflicts",
                },
                new ConflictFilterButtonVM(ConflictTypes.Segregation, "SG")
                {
                    Hint="Segregarion conflicts",
                },
                new ConflictFilterButtonVM(ConflictTypes.VentRequirement, "Vent")
                {
                    Hint="Ventilation requirements",
                },
                new ConflictFilterButtonVM(ConflictTypes.Handling, "Hand")
                {
                    Hint="Handling instructions",
                },
                new ConflictFilterButtonVM(ConflictTypes.Info, "Info")
                {
                    Hint="Information messages",
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
        }

        private void EnableClearAllButton()
        {
            if (FilteredConflictTypes.Count > 0)
                clearAllButton.IsActive = true;
            else clearAllButton.IsActive = false;
            clearAllButton.RefreshView();

        }

        #region Commands
        public ICommand ReCheckCommand { get; private set; }

        private void LoadCommands()
        {
            ReCheckCommand = new DelegateCommand(OnReCheckRequested);
        }

        #endregion

        #region Constructor

        public ConflictsSectionViewModel()
        {
            conflictDataService = ConflictDataService.GetConflictDataService();
            LoadCommands();

            Vents = conflictDataService.GetVentilationRequirements();
            FilteredConflictTypes = new();
            CreateFilterButtons();
        }


        #endregion
    }
}
