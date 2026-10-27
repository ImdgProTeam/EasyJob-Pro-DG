using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.ViewModel;
using System.Collections;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.View.Sort
{
    internal class ConflictGroupsComparer : IComparer
    {
        private readonly ObservableCollection<PanelConflictItemBaseViewModel> _originalList;

        /// <summary>
        /// Compares PanelConflictItems in accordance with <see cref="ConflictTypes"/> defined priority.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            var panelX = x as PanelConflictItemBaseViewModel;
            var panelY = y as PanelConflictItemBaseViewModel;

            if (panelX == null || panelY == null) return 0;

            int groupDifference = GetTypeSortPriority(panelY) - GetTypeSortPriority(panelX);
            if(_originalList is not null && groupDifference == 0)
            {
                int indexX = _originalList.IndexOf(panelX);
                int indexY = _originalList.IndexOf(panelY);
                return indexX.CompareTo(indexY);
            }
            return groupDifference;
        }

        private static int GetTypeSortPriority(PanelConflictItemBaseViewModel vm)
        {
            switch (vm.ConflictType)
            {
                case Data.ConflictTypes.Handling:
                    return 2;
                case Data.ConflictTypes.VentRequirement:
                    return 3;
                case Data.ConflictTypes.Stowage:
                    return 4;
                case Data.ConflictTypes.Segregation:
                    return 5;
                case Data.ConflictTypes.UserDefined:
                    return 6;
                case Data.ConflictTypes.Info:
                    return 7;
                case Data.ConflictTypes.Warning:
                    return 8;

                case Data.ConflictTypes.None:
                default:
                    return 0;
            }
        }

        public ConflictGroupsComparer()
        {
                
        }

        public ConflictGroupsComparer(ObservableCollection<PanelConflictItemBaseViewModel> collection)
        {
            _originalList = collection;    
        }
    }
}
