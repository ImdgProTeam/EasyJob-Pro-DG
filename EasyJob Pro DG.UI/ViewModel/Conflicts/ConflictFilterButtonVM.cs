using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Utility;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel.Conflicts
{
    public class ConflictFilterButtonVM : Observable
    {
        public string Title { get; set; }

        public string Hint { get; set; }
        public bool IsActive { get; set; }
        public bool IsSelected { get; set; }


        //For icon highlight
        public bool IsAlert { get; set; }
        public int Count { get; set; }

        //Command parameter
        internal ConflictTypes ConflictType { get; set; }
        public ICommand AssignedCommand { get; set; }

        // Methods
        internal void RefreshView()
        {
            OnPropertyChanged(nameof(IsSelected));
            OnPropertyChanged(nameof(IsActive));
        }

        public ConflictFilterButtonVM(ConflictTypes conflictType, string title, string hint = "", bool isActive = true)
        {
            Title = title;
            Hint = hint;
            ConflictType = conflictType;
            IsActive = isActive;
            IsSelected = false;
            IsAlert = false;            
        }
    }
}
