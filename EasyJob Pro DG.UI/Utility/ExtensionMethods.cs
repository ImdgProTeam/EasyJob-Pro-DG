using System;
using System.Runtime.CompilerServices;

namespace EasyJob_ProDG.UI.Utility
{
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Takes an input location and checks and converts it if necessary to a safe location form.
        /// </summary>
        /// <param name="inputLocation">Input location. If null -> 000000 will be returned.</param>
        /// <returns>Safe format location string.</returns>
        internal static string CorrectFormatContainerLocation(this string inputLocation)
        {
            string outputLocation;

            if (string.IsNullOrEmpty(inputLocation))
                outputLocation = "000000";
            else if (inputLocation.Length < 5)
                outputLocation = "0000" + inputLocation;
            else outputLocation = inputLocation;

            return outputLocation;
        }

        /// <summary>
        /// Limits the maximum input of ContainerLocation with 2559999 (max bay 255) and positive.
        /// </summary>
        /// <param name="inputLocation">Input string for checking and correction.</param>
        /// <returns>Same as input, if input location is safe. 
        /// Null if inputLocation is null or not a positive number.
        /// Shortened string if the inputLocation more than 2559999.</returns>
        internal static string LimitMaxContainerLocationInput(this string inputLocation)
        {
            if (inputLocation == null) return null;

            string newValue = inputLocation?.Replace(" ", "");
            if (newValue.Length > 6)
            {
                if (!uint.TryParse(newValue, out uint newIntValue)) return null;
                else if (newIntValue > 2559999)
                {
                    return newValue.Substring(1);
                }
            }
            return inputLocation.Trim();
        }


        /// <summary>
        /// Amends Location with specified property value (bay, row or tier). 
        /// Ignores full location input (returns as it is).
        /// </summary>
        /// <param name="value">Bay, row or tier number.</param>
        /// <param name="propertyName">Specified property (Bay, row or tier)</param>
        /// <returns>Location with meaningless zeroes in case of change. Location as it is if no property was specified.</returns>
        internal static string ReturnLocationPropertyInDisplayFormat(this Model.Cargo.ILocationOnBoard unit, byte value, [CallerMemberName] string propertyName = null)
        {
            switch (propertyName.ToLower())
            {
                case "bay":
                    return AddZeroIfRequired(value, 2) + AddZeroIfRequired(unit.Row) + AddZeroIfRequired(unit.Tier);
                case "row":
                    return AddZeroIfRequired(unit.Bay, 2) + AddZeroIfRequired(value) + AddZeroIfRequired(unit.Tier);
                case "tier":
                    return AddZeroIfRequired(unit.Bay, 2) + AddZeroIfRequired(unit.Row) + AddZeroIfRequired(value);
                default:
                    return unit.Location;
            }
        }

        /// <summary>
        /// Checks if input value is less then '10' and adds '0' before the digit if the case
        /// </summary>
        /// <param name="value"></param>
        /// <param name="numberOfZeros"></param>
        /// <returns></returns>
        private static string AddZeroIfRequired(byte value, byte numberOfZeros = 1)
        {
            string result = "";
            if (value < 10) result += "0";
            if (value < 100 && numberOfZeros == 2) result += "0";
            result += value.ToString();
            return result;
        }

        /// <summary>
        /// Parses multiple values separated by ',' in string into array.
        /// Ignores empty values.
        /// </summary>
        /// <param name="value">String of values separated by ','.</param>
        /// <returns>Array of meaningful values.</returns>
        static string[] ParseMultipleClasses(string value)
        {
            char[] separator = { ' ', ',' };
            string[] array = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return array;
        }

    }
}
