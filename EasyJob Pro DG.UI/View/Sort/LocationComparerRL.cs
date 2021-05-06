using EasyJob_ProDG.Model.Cargo;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.View.Sort
{
    class LocationCompararRL : IComparer<ILocationOnBoard>
    {
        public int Compare(ILocationOnBoard x, ILocationOnBoard y)
        {
            return Compare(x, y, false);
        }

        public int Compare(ILocationOnBoard x, ILocationOnBoard y, bool combine4020)
        {
            bool sameBay = combine4020
                ? (x.Bay <= y.Bay + 1 && x.Bay >= y.Bay - 1)
                : x.Bay == y.Bay;

            if (sameBay)
            {
                if (x.Tier == y.Tier)
                {
                    if (x.Row % 2 == 0)
                    {
                        if (y.Row % 2 == 0)
                        {
                            return x.Row - y.Row;
                        }
                        else return 1;
                    }
                    else
                    if (y.Row % 2 == 0)
                    {
                        return -1;
                    }
                    else return y.Row - x.Row;
                }
                else return x.Tier - y.Tier;

            }
            else return x.Bay - y.Bay;
        }


    }
}
