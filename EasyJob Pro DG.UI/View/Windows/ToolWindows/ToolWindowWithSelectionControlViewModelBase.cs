using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.Windows.ToolWindows
{
    internal class ToolWindowWithSelectionControlViewModelBase : ToolWindowViewModelBase
    {
        // Private fields
        protected SelectionControlViewModel selection => SelectionControlViewModel;

        // Public properties
        public SelectionControlViewModel SelectionControlViewModel { get; private set; }

        #region Command methods

        /// <summary>
        /// Clears all filter values
        /// </summary>
        /// <param name="obj"></param>
        protected override void OnClearCommandExecuted(object obj)
        {
            SelectionControlViewModel.Clear();
        }

        protected override bool OnClearCanExecute(object obj)
        {
            return !SelectionControlViewModel.IsNoPropertySelected;
        }

        #endregion

        #region Event methods

        private void ExecuteApply(object sender, EventArgs e)
        {
            OnApplyExecuted(null);
        }

        #endregion

        #region Constructor

        public ToolWindowWithSelectionControlViewModelBase() : base()
        {
            SelectionControlViewModel = new SelectionControlViewModel();
            SelectionControlViewModel.CreateLists(cargoPlan);

            SelectionControlViewModel.CallApply -= ExecuteApply;
            SelectionControlViewModel.CallApply += ExecuteApply;
        }

        #endregion
    }
}
