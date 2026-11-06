using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Utility.Messages;
using EasyJob_ProDG.UI.View.Sort;
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
        private IConflictDataService conflictDataService;
        private List<PanelConflictItemBaseViewModel> deletedConflicts = new List<PanelConflictItemBaseViewModel>();
        private ObservableCollection<PanelConflictItemBaseViewModel> allConflicts;
        private ListCollectionView _displayConflictsCollectionView;
        private List<ConflictTypes> _conflictsFilter;
        private ConflictGroupsComparer _conflictsComparer;

        private readonly object _collectionLock = new object();

        #endregion

        #region Public properties
        //public properties

        /// <summary>
        /// Confilicts from this list are displayed in ConflictList
        /// </summary>
        public AsyncObservableCollection<PanelConflictItemBaseViewModel> DisplayConflicts
        {
            get => _conflictsToDispaly;
            private set;
        }
        private AsyncObservableCollection<PanelConflictItemBaseViewModel> _conflictsToDispaly { get; set; }

        public PanelConflictItemBaseViewModel SelectedConflict { get; set; }

        internal List<ConflictTypes> ExistingConflictTypes => allConflicts.Select(c => c.ConflictType).Distinct().ToList();

        public bool HasNoConflicts => allConflicts?.Count == 0;
        public bool HasNothingToDisplay => _conflictsToDispaly?.Count == 0;

        #endregion

        #region Public and internal methods

        /// <summary>
        /// Sets <see cref="ConflictsList"/> filter made of collection of <see cref="ConflictTypes"/>
        /// </summary>
        /// <param name="conflictsTypes"></param>
        internal void SetConflictsFilter(ICollection<ConflictTypes> conflictsTypes = null)
        {
            if (conflictsTypes == null)
                _conflictsFilter.Clear();
            else
            {
                _conflictsFilter = (List<ConflictTypes>)conflictsTypes;
            }
            SetDisplayConflicts();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates <see cref="_conflictsToDispaly"/> from the conflicts as received from <see cref="ConflictDataService"/>.
        /// The items that have been deleted will be removed from <see cref="_conflictsToDispaly"/>
        /// </summary>
        private void SetDisplayConflicts()
        {
            GatherAllConflicts();
            SetFilteredDisplayConflicts();

            foreach (var conflict in _conflictsToDispaly)
            {
                conflict.RefreshConflictText();
            }

            RaiseDisplayConflictsSetNotificationEvent();
            OnPropertyChanged(nameof(HasNothingToDisplay));
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
        /// Filters from <see cref="_conflictsToDispaly"/> only those that pass <see cref="_conflictsFilter"/> and not deleted.
        /// </summary>
        private void SetFilteredDisplayConflicts()
        {
            foreach (var conflict in allConflicts)
            {
                if (_conflictsFilter.Count == 0)
                {
                    if (!_conflictsToDispaly.Any(c => c.Equals(conflict)))
                        if (!deletedConflicts.Any(c => c.Equals(conflict)))
                            _conflictsToDispaly.Add(conflict);
                }
                else
                {
                    if (_conflictsFilter.Contains(conflict.ConflictType))
                    {
                        if (!_conflictsToDispaly.Any(c => c.Equals(conflict)))
                            if (!deletedConflicts.Any(c => c.Equals(conflict)))
                                _conflictsToDispaly.Add(conflict);
                    }
                    else
                        _conflictsToDispaly.Remove(_conflictsToDispaly.FirstOrDefault(c => c.Equals(conflict)));
                }
            }

            var conflictsToRemove = _conflictsToDispaly.Where(c => !allConflicts.Any(conf => conf.Equals(c))).ToList();
            foreach (var conflict in conflictsToRemove)
                _conflictsToDispaly.Remove(conflict);

        }

        /// <summary>
        /// Refreshes <see cref="_conflictsToDispaly"/> depending on <see cref="DisplayConflictsToBeRefreshedMessage"/> parameters.
        /// </summary>
        /// <param name="obj">Contains parameters.</param>
        private void OnDisplayConflictsToBeRefreshedMessageReceived(DisplayConflictsToBeRefreshedMessage obj)
        {
            //called by Re-check button or new condition after open or update
            if (obj.FullListToBeUpdated)
            {
                deletedConflicts.Clear();
                _conflictsToDispaly?.Clear();
                OnPropertyChanged(nameof(_conflictsToDispaly));
            }

            SetDisplayConflicts();
        }

        #endregion

        #region Command methods

        /// <summary>
        /// Removes parameter conflict from the view (<see cref="_conflictsToDispaly"/>)
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveConflict(object parameter)
        {
            if (parameter is not PanelConflictItemBaseViewModel conflict) return;

            deletedConflicts.Add(conflict);
            _conflictsToDispaly.Remove(conflict);
        }

        /// <summary>
        /// Removes all <see cref="PanelConflictItemBaseViewModel"/>s from 
        /// <see cref="_conflictsToDispaly"/> with the same conflict code as the parameter conflict.
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveSimilarConflicts(object parameter)
        {
            var conflict = parameter as PanelConflictItemBaseViewModel;
            if (conflict == null) return;

            var code = conflict.Code;
            for (int i = 0; i < _conflictsToDispaly.Count; i++)
            {
                conflict = _conflictsToDispaly[i];
                if (conflict.Code == code)
                {
                    deletedConflicts.Add(conflict);
                    _conflictsToDispaly.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Removes UserDefinedCondition associated with selected conflict from repository. 
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveUserDefinedCondition(object parameter)
        {
            DataMessenger.Default.Send(new UserDefinedConditionRemoveMessage(parameter), DataMessengerContext.USER_DEFINED_CONDITION_REMOVE);
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
        public ICommand RemoveUserDefinedConditionCommand { get; private set; }

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
            RegisterInMessenger();

            RegisterCommands();

            _conflictsFilter = new();
            SetCollectionView();

            BindingOperations.EnableCollectionSynchronization(_conflictsToDispaly, _collectionLock);
            BindingOperations.EnableCollectionSynchronization(DisplayConflicts, _collectionLock);

            LoadServices();

            SetDisplayConflicts();
        }

        private void LoadServices()
        {
            conflictDataService = ConflictDataService.GetConflictDataService();
        }

        private void SetCollectionView()
        {
            _conflictsToDispaly = new();

            _displayConflictsCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(_conflictsToDispaly);
            _displayConflictsCollectionView.GroupDescriptions.Clear();
            _displayConflictsCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("ConflictGroupTitle"));
            _conflictsComparer = new ConflictGroupsComparer(_conflictsToDispaly);
            _displayConflictsCollectionView.CustomSort = _conflictsComparer;
        }

        private void RegisterInMessenger()
        {
            DataMessenger.Default.Register<DisplayConflictsToBeRefreshedMessage>(this, OnDisplayConflictsToBeRefreshedMessageReceived, DataMessengerContext.UPDATE_CONFLICTS);
        }

        private void RegisterCommands()
        {
            DoubleClickOnSelectedItem = new DelegateCommand(NotifyOfSelectedConflict);
            RemoveConflictCommand = new DelegateCommand(RemoveConflict);
            RemoveSimilarConflictCommand = new DelegateCommand(RemoveSimilarConflicts);
            RemoveUserDefinedConditionCommand = new DelegateCommand(RemoveUserDefinedCondition);
        }

        #endregion
    }
}
