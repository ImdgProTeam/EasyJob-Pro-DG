using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.Model.Cargo;
using System;
using static EasyJob_ProDG.UI.Settings.UserUISettings;

namespace EasyJob_ProDG.UI.View.Sort
{
    public class SortableLocation : LocationOnBoard, IComparable
    {
        static UserUISettings settings = new SettingsService().GetSettings();

        public SortableLocation(ILocationOnBoard wrapper)
        {
            Location = wrapper.Location;
        }

        public int CompareTo(object obj)
        {
            ILocationOnBoard other = obj as SortableLocation;
            if (obj == null) return 0;
            return Compare(this, other, settings.DgSortPattern);
        }

        public int Compare(ILocationOnBoard x, ILocationOnBoard y, DgSortOrderPattern pattern)
        {
            switch (pattern)
            {
                case DgSortOrderPattern.ABC:
                default:
                    return new LocationCompararABC().Compare(x, y);
                case DgSortOrderPattern.CBA:
                    return new LocationCompararABC().Compare(y, x);
                case DgSortOrderPattern.LR:
                    return new LocationCompararLR().Compare(x, y, settings.Combine2040BaysWhenSorting);
                case DgSortOrderPattern.RL:
                    return new LocationCompararRL().Compare(x, y, settings.Combine2040BaysWhenSorting);
                case DgSortOrderPattern.LRsnake:
                    return new LocationCompararLRSnake().Compare(x, y, settings.Combine2040BaysWhenSorting, settings.LowestTierOnDeck);
                case DgSortOrderPattern.RLsnake:
                    return new LocationCompararRLSnake().Compare(x, y, settings.Combine2040BaysWhenSorting, settings.LowestTierOnDeck);
            }
        }

        public override string ToString()
        {
            return $"{Bay:D3} {Row:D2} {Tier:D2}";
        }
    }
}
