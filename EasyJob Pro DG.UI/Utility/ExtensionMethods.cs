using EasyJob_ProDG.Model.Cargo;
using System.Collections.Generic;
using System.Linq;

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
    }
}
