using System.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public static class DgNameHelpers
    {
        private static string[] Wastes => ["waste", "iswaste"];
        private static string[] Stabilizeds => ["stabilized"];
        private static string[] Max1Ls => ["max1l", "maximumcapacityof1litre", "maximumcapacityof1liter", "maximum1litre"];
        private static string[] Conditioners => ["conditioner"];
        private static string[] Coolants => ["coolant"];

        internal static string AppendedFormattedText(this string text) => ", " + text;

        /// <summary>
        /// Appends <see cref="Dg.Name"/> name with additional text
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="addText"></param>
        internal static void ApendNameWith(this Dg dg, string addText)
        {
            dg.Name += AppendedFormattedText(addText);
        }

        /// <summary>
        /// Appends <see cref="Dg.Name"/> name with additional text
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="addText"></param>
        internal static void RemoveFromName(this Dg dg, string textToRemove)
        {
            if (string.IsNullOrWhiteSpace(dg.Name)) return;
                dg.Name = dg.Name.Replace(textToRemove.AppendedFormattedText(), "").Trim();
                dg.Name = dg.Name.Replace(textToRemove, "").Trim();
        }

        /// <summary>
        /// Checks if name contains one of variations of "Waste".
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the name contains a "waste".</returns>
        public static bool ContainsWaste(this string name)
        {
            if(Wastes.Any(w => name.ToLower().Replace(" ", "").Contains(w)))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if name contains one of variations of "Stabilized".
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the name contains a "stabilized".</returns>
        public static bool ContainsStabilized(this string name)
        {
            if(Stabilizeds.Any(s => name.ToLower().Replace(" ", "").Contains(s)))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if name contains one of variations of "Max 1 litre".
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the name contains a "Max 1 L".</returns>
        public static bool ContainsMax1Litre(this string name)
        {
            if (Max1Ls.Any(m => name.ToLower().Replace(" ", "").Contains(m)))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if name contains one of variations of "Conditioner".
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the name contains a "Conditioner".</returns>
        public static bool ContainsConditioner(this string name)
        {
            if (Conditioners.Any(c => name.ToLower().Replace(" ", "").Contains(c)))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if name contains one of variations of "Coolant".
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the name contains a "Coolant".</returns>
        public static bool ContainsCoolant(this string name)
        {
            if (Coolants.Any(c => name.ToLower().Replace(" ", "").Contains(c)))
                return true;
            return false;
        }

        /// <summary>
        /// Returns 'WASTE' appendix as defined in <see cref="Dg"/> class
        /// </summary>
        /// <returns>string ", WASTE"</returns>
        public static string GetWasteAppendixToName()
        {
            return Dg.PSN_WASTE.AppendedFormattedText();
        }

        /// <summary>
        /// Adds ', WASTE' to Name property of <see cref="Dg"/>
        /// </summary>
        /// <param name="dg"></param>
        public static void AddToNameWaste(this Dg dg)
        {
            dg.ApendNameWith(Dg.PSN_WASTE);
        }

        /// <summary>
        /// Adds ', WASTE' to Name property of <see cref="Dg"/>
        /// </summary>
        /// <param name="dg"></param>
        public static void AddToNameStabilized(this Dg dg)
        {
            dg.ApendNameWith(Dg.PSN_STABILIZED);
        }

        /// <summary>
        /// Adds ', WASTE' to Name property of <see cref="Dg"/>
        /// </summary>
        /// <param name="dg"></param>
        public static void AddToNameMax1l(this Dg dg)
        {
            dg.ApendNameWith(Dg.PSN_MAX1L);
        }
    }
}
