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
            DataMessenger.Default.Register<ConflictListToBeUpdatedMessage>(this, OnConflictListReceived);
            DoubleClickOnSelectedItem = new DelegateCommand(NotifyOfSelectedConflict);
            //DataMessenger.Default.Register<ConflictsList>(this, OnConflictListReceived);
            cargoDataService = new CargoDataService();
            conflictDataService = new ConflictDataService();
            OnConflictListReceived();

        }


        //Methods
        private void OnConflictListReceived(ConflictListToBeUpdatedMessage obj)
        {
            cargoDataService.ReCheckDgList();
            OnConflictListReceived();
        }

        private void OnConflictListReceived()
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
