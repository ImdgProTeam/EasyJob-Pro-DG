using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.UI.Validation
{
    internal static class LocationInputValidator
    {
        internal static bool Validate(ILocationOnBoard location, ref string input)
        {
            string value = input.Trim().Replace(" ", "").ToLower();

            //if entry is digital value
            if (uint.TryParse(value, out uint intPosition)
                && intPosition >= 10000 && intPosition <= 2559999)
                return true;

            if (intPosition < 256 && intPosition > 0)
            {
                input = location.GetModifiedLocation((byte)intPosition);
                return true;
            }

            //b12, r00, t82
            if (byte.TryParse(value.Substring(1), out byte byteValue))
            {
                input = location.GetModifiedLocation(
                    value.StartsWith("b") ? byteValue : (byte)0,
                    value.StartsWith("r") ? byteValue : null,
                    value.StartsWith("t") ? byteValue : null
                    );
                return true;
            }

            //b+, r-
            if (value.Length > 1)
            {
                if (value[1] == '+')
                {
                    input = location.GetModifiedLocation(
                    value.StartsWith("b") ? (byte)(location.Bay + 1) : (byte)0,
                    value.StartsWith("r") ? (byte)(location.Row + 1) : null,
                    value.StartsWith("t") ? (byte)(location.Tier + 2) : null
                    );
                    return true;
                }
                if (value[1] == '-')
                {
                    input = location.GetModifiedLocation(
                    value.StartsWith("b") ? (byte)(location.Bay > 1 ? location.Bay - 1 : location.Bay) : (byte)0,
                    value.StartsWith("r") ? (byte)(location.Row > 0 ? location.Row - 1 : location.Row) : null,
                    value.StartsWith("t") ? (byte)(location.Tier > 2 ? location.Tier - 2 : location.Tier) : null
                    );
                    return true;
                }
            }

            return false;
        }
    }
}
