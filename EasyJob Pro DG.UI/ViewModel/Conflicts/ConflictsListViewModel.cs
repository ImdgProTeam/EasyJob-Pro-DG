using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{

    public class ConflictListViewModel : Observable
    {
        #region Private fields

        //readonly fields
        readonly IConflictDataService conflictDataService;
        private List<ConflictPanelItemViewModel> deletedConflicts = new List<ConflictPanelItemViewModel>();
        private ObservableCollection<ConflictPanelItemViewModel> allConflicts;
        private List<ConflictTypes> conflictsFilter;

        #endregion

        #region Public properties
        //public properties

        /// <summary>
        /// Confilicts from this list are displayed in ConflictList
        /// </summary>
        public ObservableCollection<ConflictPanelItemViewModel> DisplayConflicts { get; private set; }
        public ConflictPanelItemViewModel SelectedConflict { get; set; }


        #endregion

        internal void SetConflictsFilter(ICollection<ConflictTypes> conflictsTypes = null)
        {
            if (conflictsTypes == null)
                conflictsFilter.Clear();
            else
            {
                conflictsFilter = (List<ConflictTypes>)conflictsTypes;
            }
            SetDisplayConflicts();
        }

        #region Private methods

        /// <summary>
        /// Creates <see cref="DisplayConflicts"/> from the conflicts as received from <see cref="ConflictDataService"/>.
        /// The items that have been deleted will be removed from <see cref="DisplayConflicts"/>
        /// </summary>
        private void SetDisplayConflicts()
        {
            allConflicts = conflictDataService.GetConflicts();
            SetFilteredDisplayConflicts();

            // Cleaning conflicts that has already been ordered to delete
            if (deletedConflicts.Count > 0)
            {
                for (int i = 0; i < DisplayConflicts.Count; i++)
                {
                    var conflict = DisplayConflicts[i];
                    if (deletedConflicts.Any(c => c.Equals(conflict)))
                    {
                        DisplayConflicts.Remove(conflict);
                        i--;
                    }
                }
            }

            foreach (var conflict in DisplayConflicts)
            {
                conflict.RefreshConflictText();
            }
        }

        private void SetFilteredDisplayConflicts()
        {
            foreach (var conflict in allConflicts)
            {
                if (conflictsFilter.Count == 0)
                {
                    if (!DisplayConflicts.Any(c => c.Equals(conflict)))
                        DisplayConflicts.Add(conflict);
                }
                else
                {
                    if (conflictsFilter.Contains(conflict.ConflictType))
                    {
                        if (!DisplayConflicts.Any(c => c.Equals(conflict)))
                            DisplayConflicts.Add(conflict);
                    }
                    else 
                        DisplayConflicts.Remove(DisplayConflicts.FirstOrDefault(c => c.Equals(conflict)));
                }
            }

        }

        /// <summary>
        /// Refreshes <see cref="DisplayConflicts"/> depending on <see cref="DisplayConflictsToBeRefreshedMessage"/> parameters.
        /// </summary>
        /// <param name="obj">Contains parameters.</param>
        private void OnDisplayConflictsToBeRefreshedMessageReceived(DisplayConflictsToBeRefreshedMessage obj)
        {
            // TODO: Implement logic to update only selected unit stowage conflicts
            //if (obj.OnlyUnitStowageToBeUpdated)
            //{
            //    if (!obj.dgWrapper.IsConflicted && DisplayConflicts.Any(w => w.DgID == obj.dgWrapper.)
            //    {

            //        DisplayConflicts.Remove()
            //    }
            //}

            if (obj.FullListToBeUpdated)
            {
                deletedConflicts.Clear();
                DisplayConflicts.Clear();
            }
            SetDisplayConflicts();
        }

        #endregion

        #region Command methods

        /// <summary>
        /// Removes parameter conflict from the view (<see cref="DisplayConflicts"/>)
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveConflict(object parameter)
        {
            if (parameter is not ConflictPanelItemViewModel conflict) return;

            deletedConflicts.Add(conflict);
            DisplayConflicts.Remove(conflict);
        }

        /// <summary>
        /// Removes all <see cref="ConflictPanelItemViewModel"/>s from 
        /// <see cref="DisplayConflicts"/> with the same conflict code as the parameter conflict.
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveSimilarConflicts(object parameter)
        {
            var conflict = parameter as ConflictPanelItemViewModel;
            if (conflict == null) return;

            var code = conflict.Code;
            for (int i = 0; i < DisplayConflicts.Count; i++)
            {
                conflict = DisplayConflicts[i];
                if (conflict.Code == code)
                {
                    deletedConflicts.Add(conflict);
                    DisplayConflicts.RemoveAt(i);
                    i--;
                }
            }
        }

        private void NotifyOfSelectedConflict(object parameters)
        {
            DataMessenger.Default.Send(SelectedConflict, "conflict selection changed");
        }

        #endregion

        #region Constructor
        //Constructor
        public ConflictListViewModel()
        {
            DataMessenger.Default.Register<DisplayConflictsToBeRefreshedMessage>(this, OnDisplayConflictsToBeRefreshedMessageReceived, "update conflicts");

            DoubleClickOnSelectedItem = new DelegateCommand(NotifyOfSelectedConflict);
            RemoveConflictCommand = new DelegateCommand(RemoveConflict);
            RemoveSimilarConflictCommand = new DelegateCommand(RemoveSimilarConflicts);

            conflictsFilter = new();
            DisplayConflicts = new();
            conflictDataService = ConflictDataService.GetConflictDataService();

            SetDisplayConflicts();
        }

        #endregion

        #region Commands
        //Commands
        public ICommand DoubleClickOnSelectedItem { get; private set; }
        public ICommand RemoveConflictCommand { get; private set; }
        public ICommand RemoveSimilarConflictCommand { get; private set; }

        #endregion
    }
}
