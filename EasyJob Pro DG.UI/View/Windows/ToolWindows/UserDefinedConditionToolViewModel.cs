using EasyJob_ProDG.UI.View.Windows.ToolWindows;

namespace EasyJob_ProDG.UI.View.DialogWindows.ToolWindows
{
    internal class UserDefinedConditionToolViewModel : ToolWindowViewModelBase
    {

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


        #region Command methods

        /// <summary>
        /// On 'Select' button pressed
        /// </summary>
        /// <param name="obj"></param>
        protected override void OnApplyExecuted(object obj)
        {
            SelectSelectedUnits();
        }

        protected override void OnClearCommandExecuted(object obj)
        {
            throw new System.NotImplementedException();
        }

        protected override bool OnClearCanExecute(object obj)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
