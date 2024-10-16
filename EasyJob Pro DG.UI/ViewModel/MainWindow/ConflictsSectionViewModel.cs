using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class ConflictsSectionViewModel : Observable
    {

        // Private fields
        IConflictDataService conflictDataService;
        
        // Public members
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
        }

        #endregion
    }
}
