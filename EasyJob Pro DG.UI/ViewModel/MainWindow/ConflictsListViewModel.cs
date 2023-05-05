using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EasyJob_ProDG.UI.Services.DataServices;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.UI.ViewModel
{

    public class ConflictListViewModel : Observable, IConflictListViewModel
    {
        //readonly fields
        readonly CargoDataService cargoDataService;
        readonly ConflictDataService conflictDataService;
        private List<ConflictPanelItemViewModel> deletedConflicts = new List<ConflictPanelItemViewModel>();


        //public properties
        public ConflictPanelItemViewModel SelectedConflict { get; set; }
        public ObservableCollection<ConflictPanelItemViewModel> Conflicts { get; set; }


        //Constructor
        public ConflictListViewModel()
        {
            DataMessenger.Default.Register<ConflictListToBeUpdatedMessage>(this, OnConflictListToBeUpdatedMessageReceived);

            DoubleClickOnSelectedItem = new DelegateCommand(NotifyOfSelectedConflict);
            RemoveConflictCommand = new DelegateCommand(RemoveConflict);
            RemoveSimilarConflictCommand = new DelegateCommand(RemoveSimilarConflicts);

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

            if (obj.FullListToBeUpdated)
            {
                deletedConflicts.Clear();
            }
            cargoDataService.ReCheckDgList();
            GetConflicts();
        }

        private void GetConflicts()
        {
            Conflicts = conflictDataService.GetConflicts();
            
            if(deletedConflicts.Count > 0)
            {
                for (int i = 0; i < Conflicts.Count; i++)
                {
                    var conflict = Conflicts[i];
                    if(deletedConflicts.Any(c => c.Equals(conflict)))
                    {
                        Conflicts.Remove(conflict);
                        i--;
                    }
                }
            }

            foreach (var conflict in Conflicts)
            {
                conflict.RefreshConflictText();
            }
        }

        public void NotifyOfSelectedConflict(object parameters)
        {
            DataMessenger.Default.Send(SelectedConflict, "conflict selection changed");
        }

        public void RemoveConflict(object parameter)
        {
            var conflict = parameter as ConflictPanelItemViewModel;
            if (conflict == null) return;

            deletedConflicts.Add(conflict);
            Conflicts.Remove(conflict);
        }

        public void RemoveSimilarConflicts(object parameter)
        {
            var conflict = parameter as ConflictPanelItemViewModel;
            if (conflict == null) return;

            var code = conflict.Code;
            for (int i = 0; i < Conflicts.Count; i++)
            {
                conflict = Conflicts[i];
                if (conflict.Code == code)
                {
                    deletedConflicts.Add(conflict);
                    Conflicts.RemoveAt(i);
                    i--;
                }
            }
        }

        internal void ClearDeletedConflictsList()
        {
            deletedConflicts.Clear();
        }


        //Commands
        public ICommand DoubleClickOnSelectedItem { get; set; }
        public ICommand RemoveConflictCommand { get; set; }
        public ICommand RemoveSimilarConflictCommand { get; set; }
    }
}
