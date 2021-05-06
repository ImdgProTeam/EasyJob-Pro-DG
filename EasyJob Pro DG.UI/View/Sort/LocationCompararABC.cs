using EasyJob_ProDG.Model.Cargo;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.View.Sort
{
    class LocationCompararABC : IComparer<ILocationOnBoard>
    {
        public int Compare(ILocationOnBoard x, ILocationOnBoard y)
        {
            return int.Parse(x.Location) - int.Parse(y.Location);
        }
    }
}
