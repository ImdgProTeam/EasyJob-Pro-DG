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

        // Public properties
        public ICommand ClearCommand { get; protected set; }
        public ICommand ApplyCommand { get; protected set; }


        #region Command methods

        /// <summary>
        /// On 'Apply' button pressed
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnApplyExecuted(object obj)
        {
            throw new NotImplementedException();

        }
        protected virtual bool OnApplyCanExecute(object obj)
        {
            return true;
        }

        /// <summary>
        /// Clears all filter values
        /// </summary>
        /// <param name="obj"></param>
        protected abstract void OnClearCommandExecuted(object obj);

        protected abstract bool OnClearCanExecute(object obj);


        #endregion

        #region Event methods

        private void ExecuteApply(object sender, EventArgs e)
        {
            OnApplyExecuted(null);
        }

        #endregion

        #region Constructor

        public ToolWindowViewModelBase()
        {
            ClearCommand = new DelegateCommand(OnClearCommandExecuted, OnClearCanExecute);
            ApplyCommand = new DelegateCommand(OnApplyExecuted, OnApplyCanExecute);
        }

        #endregion
    }
}
