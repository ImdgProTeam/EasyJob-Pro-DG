using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.Windows.ToolWindows;
using EasyJob_ProDG.UI.ViewModel;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows.ToolWindows
{
    internal class FilterToolViewModel : ToolWindowViewModelBase
    {
        /// <summary>
        /// Assigns Filtered units list to Filter property of a selected DataGrid
        /// </summary>
        private void FilterSelectedUnits()
        {
            switch (selectedDataGridIndex)
            {
                case 0:
                    {
                        List<string> listToFilter = ToolWindowsCommonMethods.CreateSelectDgListIds(selection, cargoPlan);
                        mainWindowViewModel.DataGridDgViewModel.SetAdditionalFilter(listToFilter);
                        break;
                    }
                case 1:
                    {
                        List<string> listToFilter = ToolWindowsCommonMethods.CreateSelectContainerNumbersList(cargoPlan.Reefers, selection, cargoPlan);
                        mainWindowViewModel.DataGridReefersViewModel.SetAdditionalFilter(listToFilter);
                        break;
                    }
                case 2:
                    {
                        List<string> listToFilter = ToolWindowsCommonMethods.CreateSelectContainerNumbersList(cargoPlan.Containers, selection, cargoPlan);
                        mainWindowViewModel.DataGridContainersViewModel.SetAdditionalFilter(listToFilter);
                        break;
                    }
                case 3:
                    {
                        List<string> listToFilter = ToolWindowsCommonMethods.CreateSelectContainerNumbersList((ObservableCollection<ContainerWrapper>)mainWindowViewModel.DataGridUpdatesViewModel.UnitsPlanView.SourceCollection, selection, cargoPlan);
                        mainWindowViewModel.DataGridUpdatesViewModel.SetAdditionalFilter(listToFilter);
                        break;
                    }
                default:
                    break;
            }
        }


        #region Command methods

        /// <summary>
        /// On 'Select' button pressed
        /// </summary>
        /// <param name="obj"></param>
        protected override void OnApplyExecuted(object obj)
        {
            FilterSelectedUnits();
        }

        #endregion
    }
}
