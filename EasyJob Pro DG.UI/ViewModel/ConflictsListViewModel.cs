using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EasyJob_ProDG.UI.Services.DataServices;

namespace EasyJob_ProDG.UI.ViewModel
{

    public class ConflictListViewModel : Observable, IConflictListViewModel
    {
        //readonly fields
        readonly CargoDataService cargoDataService;
        readonly ConflictDataService conflictDataService;


        //public properties
        public ConflictPanelItemViewModel SelectedConflict { get; set; }
        public ObservableCollection<ConflictPanelItemViewModel> Conflicts { get; set; }


        //Constructor
        public ConflictListViewModel()
        {
            DataMessenger.Default.Register<ConflictListToBeUpdatedMessage>(this, OnConflictListToBeUpdatedMessageReceived);

            DoubleClickOnSelectedItem = new DelegateCommand(NotifyOfSelectedConflict);
            //DataMessenger.Default.Register<ConflictsList>(this, OnConflictListReceived);
            cargoDataService = new CargoDataService();
            conflictDataService = new ConflictDataService();
            GetConflicts();
        }


        //Methods

        /// <summary>
        /// Initiates ReCheck of DgList and Updates ConflictList
        /// Called by changed property of a DgWrapper.
        /// </summary>
        /// <param name="obj"></param>
        private void OnConflictListToBeUpdatedMessageReceived(ConflictListToBeUpdatedMessage obj)
        {
            if (obj.OnlyUnitStowageToBeUpdated)
            {
                cargoDataService.ReCheckDgWrapperStowage(obj.dgWrapper);
                return;
            }
            
            cargoDataService.ReCheckDgList();
            GetConflicts();
        }

        private void GetConflicts()
        {
            Conflicts = conflictDataService.GetConflicts();
        }

        public void NotifyOfSelectedConflict(object parameters)
        {
            DataMessenger.Default.Send(SelectedConflict, "conflict selection changed");
        }


        //Commands
        public ICommand DoubleClickOnSelectedItem { get; set; }
    }
}
