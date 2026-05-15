using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.UI.Wrapper.Cargo
{
    internal static class DgWrapperHelpers
    {
        internal static string NormalizeNameForComparison(this string name)
        {
            return name.ToLower().Replace(" ", "").Replace(".", "");
        }

        internal static string GetWasteAppendix()
        {
            return DgNameHelpers.GetWasteAppendixToName();
        }
    }
}
