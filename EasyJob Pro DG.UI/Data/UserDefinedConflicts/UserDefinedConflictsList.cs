using EasyJob_ProDG.UI.ViewModel.Conflicts;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.Data.UserDefinedConflicts
{
    /// <summary>
    /// Contains list of active user-defined conflicts in form of <see cref="PanelConflictItemUserDefinedViewModel"/>s and allows manipulations with that list.
    /// </summary>
    internal class UserDefinedConflictsList
    {
        List<PanelConflictItemUserDefinedViewModel> _conflicts;

        internal List<PanelConflictItemUserDefinedViewModel> GetConflicts => _conflicts;

        internal void AddConflict(PanelConflictItemUserDefinedViewModel conflict)
        {
            if (!_conflicts.Contains(conflict))
                _conflicts.Add(conflict);
        }

        internal void RemoveConflict(PanelConflictItemUserDefinedViewModel conflict)
        {
            if (_conflicts.Contains(conflict))
                _conflicts.Remove(conflict);
        }

        internal void Clear()
        {
            _conflicts.Clear();
        }

        #region Singleton constructor

        private static UserDefinedConflictsList _instance;
        internal static UserDefinedConflictsList GetAccess()
        {
            return _instance ??= new UserDefinedConflictsList();
        }

        private UserDefinedConflictsList()
        {
            _conflicts = new List<PanelConflictItemUserDefinedViewModel>();
        } 

        #endregion
    }
}
