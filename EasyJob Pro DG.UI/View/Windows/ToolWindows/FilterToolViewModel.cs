using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Utility.Messages;
using EasyJob_ProDG.UI.View.Windows.ToolWindows;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.View.DialogWindows.ToolWindows
{
    internal class FilterToolViewModel : ToolWindowWithSelectionControlViewModelBase
    {
        #region Private fields
        private bool isToClear => SelectionControlViewModel.IsNoPropertySelected;
        bool hasFilterCleared = true; 

        #endregion

        #region Bindable properties

        /// <summary>
        /// Text on 'Apply' button
        /// </summary>
        public string FilterButtonText { get; private set; }

        /// <summary>
        /// Text on Status bar
        /// </summary>
        public string StatusBarText { get; private set; } = $"No filter applied"; 

        #endregion


        #region Public methods

        /// <summary>
        /// Method bound to Closed event of FilterWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnWindowClosed(object sender, System.EventArgs e)
        {
            DataMessenger.Default.UnregisterAll(this);
        } 

        #endregion

        #region Override methods

        /// <summary>
        /// On 'Select' button pressed
        /// </summary>
        /// <param name="obj"></param>
        protected override void OnApplyExecuted(object obj)
        {
            FilterSelectedUnits();
        }

        protected override bool OnApplyCanExecute(object obj)
        {
            if (!isToClear) return true;
            if (hasFilterCleared) return false;
            return base.OnApplyCanExecute(obj);
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Assigns Filtered units list to Filter property of a selected DataGrid
        /// </summary>
        private void FilterSelectedUnits()
        {
            switch (selectedDataGridIndex)
            {
                case 0:
                    {
                        if (!isToClear)
                        {
                            List<string> listToFilter = ToolWindowsCommonMethods.CreateSelectDgListIds(selection, cargoPlan);
                            mainWindowViewModel.DataGridDgViewModel.SetAdditionalFilter(listToFilter);
                            OnFilterApplied();
                        }
                        else
                        {
                            mainWindowViewModel.DataGridDgViewModel.ClearAdditionalFilter();
                            OnFilterCleared();
                        }
                        break;
                    }
                case 1:
                    {
                        if (!isToClear)
                        {
                            List<string> listToFilter = ToolWindowsCommonMethods.CreateSelectContainerNumbersList(cargoPlan.Reefers, selection, cargoPlan);
                            mainWindowViewModel.DataGridReefersViewModel.SetAdditionalFilter(listToFilter);
                            OnFilterApplied();
                        }
                        else
                        {
                            mainWindowViewModel.DataGridReefersViewModel.ClearAdditionalFilter();
                            OnFilterCleared();
                        }
                        break;
                    }
                case 2:
                    {
                        if (!isToClear)
                        {
                            List<string> listToFilter = ToolWindowsCommonMethods.CreateSelectContainerNumbersList(cargoPlan.Containers, selection, cargoPlan);
                            mainWindowViewModel.DataGridContainersViewModel.SetAdditionalFilter(listToFilter);
                            OnFilterApplied();
                        }
                        else
                        {
                            mainWindowViewModel.DataGridContainersViewModel.ClearAdditionalFilter();
                            OnFilterCleared();
                        }
                        break;
                    }
                case 3:
                    {
                        if (!isToClear)
                        {
                            List<string> listToFilter = ToolWindowsCommonMethods.CreateSelectContainerNumbersList((ObservableCollection<ContainerWrapper>)mainWindowViewModel.DataGridUpdatesViewModel?.UnitsPlanView.SourceCollection, selection, cargoPlan);
                            mainWindowViewModel.DataGridUpdatesViewModel?.SetAdditionalFilter(listToFilter);
                            OnFilterApplied();
                        }
                        else
                        {
                            mainWindowViewModel.DataGridUpdatesViewModel?.ClearAdditionalFilter();
                            OnFilterCleared();
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// On receipt of ChangedSelection message from MainWindow on change of selected DataGrid
        /// </summary>
        /// <param name="t"></param>
        private void OnSelectedDataGridChanged(object t)
        {
            switch (selectedDataGridIndex)
            {
                case 0:
                    if (mainWindowViewModel.DataGridDgViewModel.AdvancedFilterApplied)
                        OnFilterApplied();
                    else OnFilterCleared();
                    break;
                case 1:
                    if (mainWindowViewModel.DataGridReefersViewModel.AdvancedFilterApplied)
                        OnFilterApplied();
                    else OnFilterCleared();
                    break;
                case 2:
                    if (mainWindowViewModel.DataGridContainersViewModel.AdvancedFilterApplied)
                        OnFilterApplied();
                    else OnFilterCleared();
                    break;
                case 3:
                    if (mainWindowViewModel.DataGridUpdatesViewModel.AdvancedFilterApplied)
                        OnFilterApplied();
                    else OnFilterCleared();
                    break;
            }
        }

        private void OnFilterCleared()
        {
            hasFilterCleared = true;
            StatusBarText = $"No filter applied";
            OnPropertyChanged(nameof(StatusBarText));
        }

        private void OnFilterApplied()
        {
            hasFilterCleared = false;
            StatusBarText = $"Filter applied";
            OnPropertyChanged(nameof(StatusBarText));
        }

        /// <summary>
        /// Method subscribed to SelectionChanged event of SelectionControlViewModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionChanged(object sender, EventArgs e)
        {
            if (SelectionControlViewModel.IsNoPropertySelected)
                FilterButtonText = $"Clear filter";
            else FilterButtonText = $"Filter";
            OnPropertyChanged(nameof(FilterButtonText));
        }

        #endregion

        #region Constructor
        public FilterToolViewModel() : base()
        {
            FilterButtonText = $"Filter";
            SelectionControlViewModel.SelectionChanged += SelectionChanged;
            DataMessenger.Default.Register<ChangeSelectionMessage>(this, OnSelectedDataGridChanged, "selected data grid changed");
        } 

        #endregion
    }
}
