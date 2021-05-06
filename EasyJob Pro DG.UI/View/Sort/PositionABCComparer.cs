//Class was used for sorting of container position alphabethically.
//But another way of sorting was implemented
//Class not in use currently


using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.View.Sort
{
    public class PositionABCComparer : IComparer<DgWrapper>
    {
        int result;

        public int Compare(DgWrapper x, DgWrapper y)
        {
            result = int.Parse(x.Location) - int.Parse(y.Location);
            if (result == 0)
                result = x.Unno - y.Unno;
            if (result == 0)
                result = (int)(x.DgNetWeight - y.DgNetWeight);
            return result;
        }


    }
}
