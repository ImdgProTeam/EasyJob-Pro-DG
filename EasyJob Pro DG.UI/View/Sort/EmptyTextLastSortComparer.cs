using EasyJob_ProDG.UI.View.AttachedProperties;
using System.ComponentModel;
using System.Reflection;

namespace EasyJob_ProDG.UI.View.Sort
{
    public class EmptyTextLastSortComparer : ICustomSorter
    {
        private PropertyInfo _propertyInfo;
        private bool isAcsending => SortDirection == ListSortDirection.Ascending;
        public ListSortDirection SortDirection { get; set; }
        public string SortMemberPath { get ; set ; }

        public int Compare(object x, object y)
        {
            PropertyInfo property = this._propertyInfo == null || !string.Equals(_propertyInfo.Name, SortMemberPath)  
                ? (this._propertyInfo = x.GetType().GetProperty(SortMemberPath))
                : _propertyInfo;
            if(property == null) return 0;

            string value1 = property.GetValue(x) as string;
            string value2 = property.GetValue(y) as string;


            if(string.Equals(value1, value2)) return 0;
            if (string.IsNullOrEmpty(value1)) return isAcsending ? 1 : -1;
            if (string.IsNullOrEmpty(value2)) return isAcsending ? -1 : 1;
            return string.Compare(value1, value2);

        }
    }
}
