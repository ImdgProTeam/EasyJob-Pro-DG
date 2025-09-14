using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.Windows.ToolWindows
{
    internal abstract class ToolWindowViewModelBase : Observable
    {
        // Private fields
        protected CargoPlanWrapper cargoPlan => ServicesHandler.GetServicesAccess().CargoDataServiceAccess.WorkingCargoPlan;
        protected MainWindowViewModel mainWindowViewModel => ViewModelLocator.MainWindowViewModel;
        protected int selectedDataGridIndex => ViewModelLocator.MainWindowViewModel.SelectedDataGridIndex;
        protected SelectionControlViewModel selection => SelectionControlViewModel;

        // Public properties
        public SelectionControlViewModel SelectionControlViewModel { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }


        #region Command methods

        /// <summary>
        /// On 'Apply' button pressed
        /// </summary>
        /// <param name="obj"></param>
        protected abstract void OnApplyExecuted(object obj);
        protected virtual bool OnApplyCanExecute(object obj)
        {
            return true;
        }

        /// <summary>
        /// Clears all filter values
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnClearCommandExecuted(object obj)
        {
            SelectionControlViewModel.Clear();
        }

        #endregion


        #region Constructor

        public ToolWindowViewModelBase()
        {
            SelectionControlViewModel = new SelectionControlViewModel();
            SelectionControlViewModel.CreateLists(cargoPlan);

            ClearCommand = new DelegateCommand(OnClearCommandExecuted);
            ApplyCommand = new DelegateCommand(OnApplyExecuted, OnApplyCanExecute);
        }

        #endregion
    }
}
