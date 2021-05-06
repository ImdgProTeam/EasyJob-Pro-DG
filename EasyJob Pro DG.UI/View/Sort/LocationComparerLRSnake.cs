using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.Cargo;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.View.Sort
{
    class LocationCompararLRSnake : IComparer<ILocationOnBoard>
    {
        public int Compare(ILocationOnBoard x, ILocationOnBoard y)
        {
            return Compare(x, y, true, ProgramDefaultSettingValues.lowestTier);
        }

        public int Compare(ILocationOnBoard x, ILocationOnBoard y, bool combine4020, byte lowestTier)
        {
            bool sameBay = combine4020
                ? (x.Bay <= y.Bay + 1 && x.Bay >= y.Bay - 1)
                : x.Bay == y.Bay;

            bool leftToRightTier = x.Tier == 02
                || (x.Tier < lowestTier && (x.Tier - 2) % 4 == 0)
                || x.Tier == lowestTier
                || (x.Tier > lowestTier && (x.Tier - lowestTier) % 4 == 0);

            if (sameBay)
            {
                if (x.Tier == y.Tier)
                    if (leftToRightTier)
                    {
                        return new LocationCompararLR().Compare(x, y, combine4020);
                    }
                    else
                    {
                        return new LocationCompararRL().Compare(x, y, combine4020);
                    }
                else return x.Tier - y.Tier;

            }
            else return x.Bay - y.Bay;
        }
    }
}
