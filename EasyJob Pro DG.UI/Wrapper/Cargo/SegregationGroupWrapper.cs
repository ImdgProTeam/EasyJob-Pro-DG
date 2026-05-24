using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.Wrapper.Cargo
{
    /// <summary>
    /// Wrapper for one SegregationGroup in the list with option to select (IsSelected property).
    /// </summary>
    public class SegregationGroupWrapper : Observable
    {
        private bool isSelected;

        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                if (IsAsPerCode) isSelected = true;
                OnPropertyChanged();
            }
        }

        internal bool IsAsPerCode;
        internal byte Number { get; set; }
    }
}
