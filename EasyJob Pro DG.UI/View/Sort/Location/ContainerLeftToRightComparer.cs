//Class was used for sorting of container position left to right.
//But another way of sorting was implemented
//Class not in use currently

using EasyJob_ProDG.Model.Cargo;
using System.Collections;

namespace EasyJob_ProDG.UI.View.Sort
{
    public class ContainerLeftToRightComparer : IComparer
    {
        readonly LeftToRightComparer leftToRight = new LeftToRightComparer();

        public int Compare(object x, object y)
        {
            LocationOnBoard a = x as LocationOnBoard;
            LocationOnBoard b = y as LocationOnBoard;

            if (a.Bay > b.Bay) return 1;
            if (a.Bay < b.Bay) return -1;

            return leftToRight.Compare(a.Row, b.Row);

        }
    }
}
