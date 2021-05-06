using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Utility;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EasyJob_ProDG.UI.Services.DataServices;

namespace EasyJob_ProDG.UI.ViewModel
{

    public class ConflictListViewModel : Observable, IConflictListViewModel
    {
        //readonly fields
        readonly CargoDataService cargoDataService;
        readonly ConflictDataService conflictDataService;

        //public CargoPlanWrapper cargoPlan;
        //public DgWrapper selectedDg;


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

            //Conflicts.GroupDescriptions.Add(new PropertyGroupDescription("ConflictGroupTitle"));
            //Conflicts.GroupDescriptions.Add(new PropertyGroupDescription("ContainerNumber"));
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
            //Conflicts = CollectionViewSource.GetDefaultView(conflictDataService.GetConflicts());
            //OnPropertyChanged("Conflicts");
            //Conflicts.Refresh();


        }
        private void OnConflictListReceived(ConflictsList conflicts)
        {
            //Conflicts = CollectionViewSource.GetDefaultView(conflicts);
            //OnPropertyChanged("Conflicts");


        }

        public void Load()
        {

        }

        public void NotifyOfSelectedConflict(object parameters)
        {
            DataMessenger.Default.Send(SelectedConflict, "conflict selection changed");
        }

        private void ConflictBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var s = sender as ListBox;
            ConflictPanelItemViewModel txb = s?.SelectedItems[0] as ConflictPanelItemViewModel;
            MessageBox.Show(txb?.Text);
        }

        //Commands
        public ICommand DoubleClickOnSelectedItem { get; set; }
    }
}
