using EasyJob_ProDG.UI.View.Windows.ToolWindows;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.View.DialogWindows.ToolWindows
{
    internal class SelectToolViewModel : ToolWindowViewModelBase
    {
        /// <summary>
        /// Assigns Selected units list to ItemsToSelect property of a selected DataGrid
        /// </summary>
        private void SelectSelectedUnits()
        {
            switch (selectedDataGridIndex)
            {
                case 0:
                    {
                        List<string> listToSelect = ToolWindowsCommonMethods.CreateSelectDgListIds(selection, cargoPlan);
                        mainWindowViewModel.DataGridDgViewModel.ItemsToSelect = listToSelect;
                        break;
                    }
                case 1:
                    {
                        List<string> listToSelect = ToolWindowsCommonMethods.CreateSelectContainerNumbersList(cargoPlan.Reefers, selection, cargoPlan);
                        mainWindowViewModel.DataGridReefersViewModel.ItemsToSelect = listToSelect;
                        break;
                    }
                case 2:
                    {
                        List<string> listToSelect = ToolWindowsCommonMethods.CreateSelectContainerNumbersList(cargoPlan.Containers, selection, cargoPlan);
                        mainWindowViewModel.DataGridContainersViewModel.ItemsToSelect = listToSelect;
                        break;
                    }
                case 3:
                    {
                        List<string> listToSelect = ToolWindowsCommonMethods.CreateSelectContainerNumbersList((ObservableCollection<ContainerWrapper>)mainWindowViewModel.DataGridUpdatesViewModel.UnitsPlanView.SourceCollection, selection, cargoPlan);
                        mainWindowViewModel.DataGridUpdatesViewModel.ItemsToSelect = listToSelect;
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
            SelectSelectedUnits();
        }

        #endregion
    }
}
