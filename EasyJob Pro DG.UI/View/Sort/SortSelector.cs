using EasyJob_ProDG.UI.Wrapper;
using EasyJob_ProDG.Model.Cargo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//Class was used for sorting of container position.
//But another way of sorting was implemented
//Class not in use currently

using System.Text;
using System.Threading.Tasks;
using static EasyJob_ProDG.UI.Settings.UserUISettings;

namespace EasyJob_ProDG.UI.View.Sort
{
    public class SortSelector : IComparer
    {
        DgSortOrderPattern _direction;
        public SortSelector(DgSortOrderPattern direction)
        {
            _direction = direction;
        }


        public int Compare(object x, object y)
        {
            DgWrapper valueX = x as DgWrapper;
            DgWrapper valueY = y as DgWrapper;

            switch (_direction)
            {
                case DgSortOrderPattern.ABC:
                default:
                    return new PositionABCComparer().Compare(valueX, valueY);
                case DgSortOrderPattern.CBA:
                    return 1;
                case DgSortOrderPattern.LR:
                    return new ContainerLeftToRightComparer().Compare(x, y);
            }
        }


    }
}
