using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.Windows.ToolWindows;
using EasyJob_ProDG.UI.Wrapper;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows.ToolWindows
{
    internal class SelectToolViewModel : Observable
    {
        private CargoPlanWrapper _cargoPlan => ServicesHandler.GetServicesAccess().CargoDataServiceAccess.WorkingCargoPlan;
        public SelectionControlViewModel SelectionControlViewModel { get; private set; }
        public ICommand ClearCommand { get; private set; }


        public SelectToolViewModel()
        {
            SelectionControlViewModel = new SelectionControlViewModel();
            SelectionControlViewModel.CreateLists(_cargoPlan);

            ClearCommand = new DelegateCommand(OnClearCommandExecuted);
        }

        private void OnClearCommandExecuted(object obj)
        {
            SelectionControlViewModel.Clear();
        }
    }
}
