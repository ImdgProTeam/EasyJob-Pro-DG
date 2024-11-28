using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Validation
{
    internal static class UnnoInputValidator
    {
        internal static bool Validate(ushort input)
        {
            if (!DataHelper.CheckForExistingUnno(input))
                return false;
            return true;
        }
    }
}
