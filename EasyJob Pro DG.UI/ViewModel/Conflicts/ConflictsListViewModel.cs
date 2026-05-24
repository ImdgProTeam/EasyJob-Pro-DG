using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
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

        private readonly object _collectionLock = new object();

        #endregion

        #region Public properties
        //public properties

        /// <summary>
        /// Confilicts from this list are displayed in ConflictList
        /// </summary>
        public ObservableCollection<ConflictPanelItemViewModel> DisplayConflicts { get; private set; }
        public ConflictPanelItemViewModel SelectedConflict { get; set; }

        internal List<ConflictTypes> ExistingConflictTypes => allConflicts.Select(c => c.ConflictType).Distinct().ToList();
        #endregion

        #region Public and internal methods

        /// <summary>
        /// Sets <see cref="ConflictsList"/> filter made of collection of <see cref="ConflictTypes"/>
        /// </summary>
        /// <param name="conflictsTypes"></param>
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

        #endregion

        #region Private methods

        /// <summary>
        /// Creates <see cref="DisplayConflicts"/> from the conflicts as received from <see cref="ConflictDataService"/>.
        /// The items that have been deleted will be removed from <see cref="DisplayConflicts"/>
        /// </summary>
        private void SetDisplayConflicts()
        {
            GatherAllConflicts();
            SetFilteredDisplayConflicts();

            foreach (var conflict in DisplayConflicts)
            {
                conflict.RefreshConflictText();
            }

            RaiseDisplayConflictsSetNotificationEvent();
        }

        /// <summary>
        /// Receives all conflicts from <see cref="ConflictDataService"/>
        /// </summary>
        private void GatherAllConflicts()
        {
            allConflicts = conflictDataService.GetConflicts();
        }

        private void RaiseDisplayConflictsSetNotificationEvent()
        {
            DisplayConflictsSet?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Filters from <see cref="DisplayConflicts"/> only those that pass <see cref="conflictsFilter"/> and not deleted.
        /// </summary>
        private void SetFilteredDisplayConflicts()
        {
            foreach (var conflict in allConflicts)
            {
                if (conflictsFilter.Count == 0)
                {
                    if (!DisplayConflicts.Any(c => c.Equals(conflict)))
                        if (!deletedConflicts.Any(c => c.Equals(conflict)))
                            DisplayConflicts.Add(conflict);
                }
                else
                {
                    if (conflictsFilter.Contains(conflict.ConflictType))
                    {
                        if (!DisplayConflicts.Any(c => c.Equals(conflict)))
                            if (!deletedConflicts.Any(c => c.Equals(conflict)))
                                DisplayConflicts.Add(conflict);
                    }
                    else
                        DisplayConflicts.Remove(DisplayConflicts.FirstOrDefault(c => c.Equals(conflict)));
                }
            }

            var conflictsToRemove = DisplayConflicts.Where(c => !allConflicts.Any(conf => conf.Equals(c))).ToList();
            foreach (var conflict in conflictsToRemove)
                DisplayConflicts.Remove(conflict);
        }

        /// <summary>
        /// Refreshes <see cref="DisplayConflicts"/> depending on <see cref="DisplayConflictsToBeRefreshedMessage"/> parameters.
        /// </summary>
        /// <param name="obj">Contains parameters.</param>
        private void OnDisplayConflictsToBeRefreshedMessageReceived(DisplayConflictsToBeRefreshedMessage obj)
        {
            //called by Re-check button or new condition after open or update
            if (obj.FullListToBeUpdated)
            {
                deletedConflicts.Clear();
                DisplayConflicts?.Clear();
                OnPropertyChanged(nameof(DisplayConflicts));
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

        #region Commands
        //Commands
        public ICommand DoubleClickOnSelectedItem { get; private set; }
        public ICommand RemoveConflictCommand { get; private set; }
        public ICommand RemoveSimilarConflictCommand { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Raised when DisplayConflicts are set.
        /// </summary>
        internal event EventHandler DisplayConflictsSet;

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
            BindingOperations.EnableCollectionSynchronization(DisplayConflicts, _collectionLock);
            conflictDataService = ConflictDataService.GetConflictDataService();

            SetDisplayConflicts();
        }

        #endregion
    }
}
