using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyJob_ProDG.UI.Wrapper.Cargo
{
    internal static class DgWrapperPropertiesHelpers
    {
        #region Proper shipping name constants

        const string PSN_MAX1L = "Max 1L";
        const string PSN_STABILIZED = "STABILIZED";
        const string PSN_WASTE = "WASTE";

        #endregion

        internal static string NormalizeNameForComparison(this string name)
        {
            return name.ToLower().Replace(" ", "").Replace(".", "");
        }

    }
}
