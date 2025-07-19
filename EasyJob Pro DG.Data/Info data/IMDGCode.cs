using System.Collections.Generic;

namespace EasyJob_ProDG.Data.Info_data
{
    public static partial class IMDGCode
    {
        public const ushort MaxUnnoNumber = 3560;
        public static List<string> AllValidDgClasses = new List<string>()
        {
            "1.1", "1.2", "1.3", "1.4", "1.5", "1.6",
            "2.1", "2.2", "2.3", "3", "4.1", "4.2", "4.3",
            "5.1", "5.2", "6.1", "6.2", "7", "8", "9"
        };

        public static List<char> AllValidCompatibilityGroupsOfClass1 = new List<char>()
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'N', 'S'
        };

        public static List<char> AllValidStowageCategories = new List<char>()
        {
            '0', 'A', 'B', 'C', 'D', 'E', '1', '2', '3', '4', '5'
        };

        public static readonly byte[,] SegregationTable ={
            {5, 5, 5, 4, 2, 2, 4, 4, 4, 4, 4, 4, 2, 4, 2, 4, 0 }, //Explosives 1.1, 1.2, 1.5
            {5, 5, 5, 4, 2, 2, 4, 3, 3, 4, 4, 4, 2, 4, 2, 2, 0 }, //Explosives 1.3, 1.6
            {5, 5, 5, 2, 1, 1, 2, 2, 2, 2, 2, 2, 0, 4, 2, 2, 0 }, //Explosives 1.4 
            {4, 4, 2, 0, 0, 0, 2, 1, 2, 2, 2, 2, 0, 4, 2, 1, 0 }, //Flammable gases 2.1   
            {2, 2, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 2, 1, 0, 0 }, //Non-toxic, non-flammable gases 2.2 
            {2, 2, 1, 0, 0, 0, 2, 0, 2, 0, 0, 2, 0, 2, 1, 0, 0 }, //Toxic gases 2.3 
            {4, 4, 2, 2, 1, 2, 0, 0, 2, 2, 2, 2, 0, 3, 2, 0, 0 }, //Flammable liquids 3
            {4, 3, 2, 1, 0, 0, 0, 0, 1, 0, 1, 2, 0, 3, 2, 1, 0 }, //Flammable solids 4.1
            {4, 3, 2, 2, 1, 2, 2, 1, 0, 1, 2, 2, 1, 3, 2, 1, 0 }, //Substances liable to spontaneous combustion 4.2
            {4, 4, 2, 2, 0, 0, 2, 0, 1, 0, 2, 2, 0, 2, 2, 1, 0 }, //Substances which, in contact with water, emit flammable gases 4.3
            {4, 4, 2, 2, 0, 0, 2, 1, 2, 2, 0, 2, 1, 3, 1, 2, 0 }, //Oxidizing substances(agents) 5.1 
            {4, 4, 2, 2, 1, 2, 2, 2, 2, 2, 2, 0, 1, 3, 2, 2, 0 }, //Organic peroxides 5.2 
            {2, 2, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0 }, //Toxic substances 6.1 
            {4, 4, 4, 4, 2, 2, 3, 3, 3, 2, 3, 3, 1, 0, 3, 3, 0 }, //Infectious substances 6.2
            {2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 1, 2, 0, 3, 0, 2, 0 }, //Radioactive material 7
            {4, 2, 2, 1, 0, 0, 0, 1, 1, 1, 2, 2, 0, 3, 2, 0, 0 }, //Corrosive substances 8 
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }  //Miscellaneous 9
            };

        /// <summary>
        /// Method to assign row number in segregation table.
        /// </summary>
        public static byte AssignSegregationTableRowNumber(string dgClass)
        {
            if (dgClass == null) return 16;

            byte tableRow;

            var _index = dgClass.Length > 3 ? dgClass.Substring(0, 3) : dgClass;
            switch (_index)
            {
                case "1.1":
                    tableRow = 0;
                    break;
                case "1.2":
                    tableRow = 0;
                    break;
                case "1.3":
                    tableRow = 1;
                    break;
                case "1.4":
                    tableRow = 2;
                    break;
                case "1.5":
                    tableRow = 0;
                    break;
                case "1.6":
                    tableRow = 1;
                    break;
                case "2.1":
                    tableRow = 3;
                    break;
                case "2.2":
                    tableRow = 4;
                    break;
                case "2.3":
                    tableRow = 5;
                    break;
                case "3":
                    tableRow = 6;
                    break;
                case "4.1":
                    tableRow = 7;
                    break;
                case "4.2":
                    tableRow = 8;
                    break;
                case "4.3":
                    tableRow = 9;
                    break;
                case "5.1":
                    tableRow = 10;
                    break;
                case "5.2":
                    tableRow = 11;
                    break;
                case "6.1":
                    tableRow = 12;
                    break;
                case "6.2":
                    tableRow = 13;
                    break;
                case "7":
                    tableRow = 14;
                    break;
                case "8":
                    tableRow = 15;
                    break;
                case "9":
                    tableRow = 16;
                    break;
                default:
                    tableRow = 0;
                    break;
            }
            return tableRow;
        }

        #region Class 1 compatibility table
        /// <summary>
        /// Permitted mixed stowage for goods of class 1 (Table 7.2.7.1.4)
        /// </summary>
        public static byte[,] ExplosivesPermittedMixedStowage =
{
                {9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   //A
                {0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9 },   //B
                {0, 0, 9, 6, 6, 0, 1, 0, 0, 0, 0, 4, 9 },   //C
                {0, 0, 6, 9, 6, 0, 1, 0, 0, 0, 0, 4, 9 },   //D
                {0, 0, 6, 6, 9, 0, 1, 0, 0, 0, 0, 4, 9 },   //E
                {0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 9 },   //F
                {0, 0, 1, 1, 1, 0, 9, 0, 0, 0, 0, 0, 9 },   //G
                {0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 9 },   //H
                {0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 0, 9 },   //J
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 0, 9 },   //K
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0 },   //L
                {0, 0, 4, 4, 4, 0, 0, 0, 0, 0, 0, 3, 5 },   //N
                {0, 9, 9, 9, 9, 9, 9, 9, 9, 9, 0, 5, 9 }    //S
            };

        public static char[] ExplosivesCompatibilityGroupCodes = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'N', 'S' };
        #endregion

        #region DOC classes
        public static List<string> DOCclasses = new List<string>()
        {
            "1.1-1.6 - Explosives",                                          //0
            "1.4(S) - Explosives, Division 1.4 Compatibiliyu group 'S'",     //1
            "2.1 - Flammable gases",                                         //2
            "2.2 - Non-flammable, Non-toxic gases",                          //3
            "2.3 - Toxic gases (flammable)",                                 //4
            "2.3 - Toxic gases (non-flammable)",                             //5
            "3 - Flammable liquids - low and intermediate flashpoint, <23°C",
            "3 - Flammable liquids - high flashpoint, ≥23°C but ≤60°C",
            "4.1 - Flammable solids, self-reactive substances and solid desensitized explosives",
            "4.2 - Solids liable to spontaneous combustion",
            "4.3 - Substances which, in contact with water, emit flammable gases (liquids)",
            "4.3 - Substances which, in contact with water, emit flammable gases (solids)",
            "5.1 - Oxidising substances (agents)",
            "5.2 - Organic peroxides",
            "6.1 - Toxic substances (liquids) - low and intermediate flashpoint, <23°C",
            "6.1 - Toxic substances (liquids) - high flashpoint, ≥23°C but ≤60°C",
            "6.1 - Toxic substances (liquids) - non flammable",
            "6.1 - Toxic substances (solids)",
            "8 - Corrosives (liquids) - low and intermediate flashpoint, <23°C",
            "8 - Corrosives (liquids) - high flashpoint, ≥23°C but ≤60°C",
            "8 - Corrosives (liquids) - non flammable",
            "8 - Corrosives (solids)",
            "9 - Miscellaneous Dangerous Substances and Articles"
        };

        public static Dictionary<string, string> DOCClassesDictionary = new Dictionary<string, string>()
        {
            {"class 1.1 - 1.6", "1.1-1.6 - Explosives" },                                          //0
            {"class 1.4S", "1.4(S) - Explosives, Division 1.4 Compatibiliyu group 'S'" },     //1
            {"class 2.1", "2.1 - Flammable gases" },                                         //2
            {"class 2.2", "2.2 - Non-flammable, Non-toxic gases" },                          //3
            {"class 2.3 flammable", "2.3 - Toxic gases (flammable)" },                                 //4
            {"class 2.3 non-flammable", "2.3 - Toxic gases (non-flammable)" },                             //5
            {"class 3 <23°C", "3 - Flammable liquids - low and intermediate flashpoint, <23°C" },
            {"class 3 ≥23°C ≤60°C", "3 - Flammable liquids - high flashpoint, ≥23°C but ≤60°C" },
            {"class 4.1", "4.1 - Flammable solids, self-reactive substances and solid desensitized explosives" },
            {"class 4.2", "4.2 - Solids liable to spontaneous combustion" },
            {"class 4.3 liquids", "4.3 - Substances which, in contact with water, emit flammable gases (liquids)" },
            {"class 4.3 solids", "4.3 - Substances which, in contact with water, emit flammable gases (solids)" },
            {"class 5.1", "5.1 - Oxidising substances (agents)" },
            {"class 5.2", "5.2 - Organic peroxides" },
            {"class 6.1 liquids <23°C", "6.1 - Toxic substances (liquids) - low and intermediate flashpoint, <23°C" },
            {"class 6.1 liquids ≥23°C ≤60°C", "6.1 - Toxic substances (liquids) - high flashpoint, ≥23°C but ≤60°C" },
            {"class 6.1 liquids", "6.1 - Toxic substances (liquids) - non flammable" },
            {"class 6.1 solids", "6.1 - Toxic substances (solids)" },
            {"class 8 liquids <23°C", "8 - Corrosives (liquids) - low and intermediate flashpoint, <23°C" },
            {"class 8 liquids ≥23°C ≤60°C", "8 - Corrosives (liquids) - high flashpoint, ≥23°C but ≤60°C" },
            {"class 8 liquids", "8 - Corrosives (liquids) - non flammable" },
            {"class 8 solids", "8 - Corrosives (solids)" },
            {"class 9", "9 - Miscellaneous Dangerous Substances and Articles" }
        };

        #endregion

        #region DOC additional requirement
        public static string[] DOCadditional =
        {
            "Goods of class 1 shall not be stowed within a horizontal distance of 6m from potential sources of fire, machinery exhausts, galley uptakes, lockers used for combustible stores or other potential sources of ignition and not less than a horizontal distance of 8 meters from the brige, living quarters and life-saving appliances.",
            "When dangerous goods are classes 2.1,2.3,3,4,5,6.1(B)},6.1(D)},8(B)},8(C)}, and 9 are carried under deck, they are to be carried out in closed freight containers only.",
            "For classes 2,3,4 liquids,5.1 liquids, 6.1, and 8 and 9 when carried in closed freight containers in purpose built dedicated container cargo spaces, the ventilation rate may be reduced to not less than two air exchanges per hour.",
            "Power ventilation is not required for class 4 (solid) and 5.1 (liquid) when carried out in closed freight containers",
            "Stowage of class 5.2 underdeck is prohibited",
            "Stowage of class 2.3 having subsidiary risk class 2.1 underdeck is prohibited",
            "Stowage of class 4.3 liquids having a flashpoint less than 23°C under deck is prohibited",
            "No special requirements for class 6.2/7 or for the goods in limited quantities",
            "Dangerous goods of class 9 in package form, which according to the IMDG code emmit flammable vapours, not to be carried in hold No 8"
        };
        #endregion

        #region Arrays of UNNOs
        /// <summary>
        /// List of unnos to which SW22 can be applied in part of Waste
        /// </summary>
        public static ushort[] SW22RelatedUnnos = new ushort[]
        {
            1950, 2037
        };

        public static ushort[] Table72631 = { 2014, 2984, 3105, 3107, 3109, 3149 };
        public static ushort[] Table72632 = { 1295, 1818, 2189 };
        public static ushort[] Table72633 = { 3391, 3392, 3393, 3394, 3395, 3396, 3397, 3398, 3399, 3400 };
        public static ushort[] Table72634 = { 1325, 3101, 3102, 3103, 3104, 3105, 3106, 3107, 3108, 3109, 3110, 3111, 3112, 3113, 3114, 3115, 3116, 3117, 3118, 3119, 3120 };
        public static ushort[] Classes72721 = { 1942, 2067, 1486, 1454, 1451, 2722, 1477, 1498, 1446, 2464, 1474, 1507 };
        public static ushort[] BlastingExplosives = { 81, 82, 84, 241, 331, 332 };

        /// <summary>
        /// Fish meal UNNOs in accordance with 7.4.1.3
        /// </summary>
        public static ushort[] Fishmeal = { 1374, 2216, 3497 };

        /// <summary>
        /// Ammonium Nitrate substances listed in 7.4.1.4
        /// </summary>
        public static ushort[] AmmoniumNitrate = { 1942, 2067, 2071 };
        #endregion

        #region Paragraphs quotes
        public static Dictionary<string, string> Paragraphs = new Dictionary<string, string>
        {
            { "1.1.1.7", "Dangerous goods, that are only asphyxiant (which dilute or replace the oxygen normally in the atmosphere)}, when used in cargo transport units for cooling or conditioning purposes are only subject to the provisions of section 5.5.3." },
            {"5.5.3.2.1", "Cargo transport units containing substances used for cooling or conditioning purposes (other than fumigation) during transport are not subject to any provisions of this Code other than those of this section." },
            { "7.1.4.4.5","Transport to or from offshore oil platforms, mobile offshore drilling units and other offshore installations \nNotwithstanding the stowage category indicated in column 16a of the Dangerous Goods List, UN 0124 JET PERFORATING GUNS, CHARGED, and UN 0494 JET PERFORATING GUNS, CHARGED, transported to or from offshore oil platforms, mobile offshore drilling units and other offshore installations may be stowed on deck in offshore well tool pallets, cradles or baskets provided that: \n.1 initiation devices shall be segregated from each other and from any jet perforating guns in accordance with the provisions of 7.2.7, and from any other dangerous goods in accordance with the provisions of 7.2.4 and 7.6.3.2, unless otherwise approved by the competent authority; \n.2 jet perforating guns shall be securely held in place during transport; \n.3 each shaped charge affixed to any gun shall not contain more than 112 g of explosives; \n.4 each shaped charge, if not completely enclosed in glass or metal, shall be fully protected by a metal cover following installation in the gun; \n.5 both ends of jet perforating guns shall be protected by means of steel end caps allowing for pressure release in the event of fire; \n.6 the total explosive content shall not exceed 95 kg per well tool pallet, cradle or basket; and \n.7 where more than one well tool pallet, cradle or basket is stowed \"on deck\", a minimum horizontal distance of 3 m shall be observed between them." },
            {"7.1.4.7", "Stowage of stabilized dangerous goods \nSubstances, for which the word \"STABILIZED\" is added as part of the proper shipping name of the substances in accordance with 3.1.2.6, Stowage Category D and SW1 shall apply." },
            {"7.4.1.4", @"For stowage of AMMONIUM NITRATE (UN 1942)}, AMMONIUM NITRATE BASED FERTILIZER (UN 2067 AND 2071) in containers, the applicable provisions of 7.6.2.8.4 and 7.6.2.11.1 also apply." },
            {"7.4.1.3", @"For stowage of FISHMEAL, UNSTABILIZED (UN 1374)}, FISHMEAL, STABILIZED (UN 2216) and KRILL MEAL (UN 3497) in containers, the provisions of 7.6.2.7.2.2 also apply." },
            { "7.6.2.8.4", @"UN 1942 AMMONIUM NITRATE and UN 2067 AMMONIUM NITRATE BASED FERTILIZER may be stowed under deck in a clean cargo space capable of being opened up in an emergency. The possible need to open hatches in case of fire to provide maximum ventilation and to apply water in an emergency and the consequent risk to the stability of the ship through flooding of cargo space shall be considered before loading." },
            {"7.6.2.11.1", @"Stowage provisions for AMMONIUM NITRATE BASED FERTILIZER, UN 2071
                \n7.6.2.11.1.1 AMMONIUM NITRATE BASED FERTILIZER, UN 2071 shall be stowed in a clean cargo space capable of being opened up in an emergency. In the case of bagged fertilizer or fertilizer in containers or in bulk containers, it is sufficient if, in the case of an emergency, the cargo is accessible through free approaches (hatch entries)}, and mechanical ventilation enables the master to exhaust any gases or fumes resulting from decomposition. The possible need to open hatches in case of fire to provide maximum ventilation and to apply water in an emergency, and the consequent risk to the stability of the ship through flooding of the cargo space, shall be considered before loading.
                \n7.6.2.11.1.2 If suppression of decomposition should prove impracticable (such as in bad weather)}, there would not necessarily be immediate danger to the structure of the ship. However, the residue left after decomposition may have only half the mass of the original cargo; this loss of mass may also affect the stability of the ship and shall be considered before loading.
                \n7.6.2.11.1.3 AMMONIUM NITRATE BASED FERTILIZER, UN 2071 shall be stowed out of direct contact with a metal engine-room bulkhead.In the case of bagged material, this may be done, for example, by using wooden boards to provide an air space between the bulkhead and the cargo.This requirement need not apply to short international voyages.
                \n7.6.2.11.1.4 In the case of ships not fitted with smoke-detecting or other suitable devices, arrangements shall be made during the voyage to inspect cargo spaces containing these fertilizers at intervals not exceeding 4 h (such as to sniff at the ventilators serving them) to ensure early detection of decomposition should that occur." },
            {"7.6.2.7.2", @"Stowage provisions for FISHMEAL, UNSTABILIZED (UN 1374)}, FISHMEAL, STABILIZED (UN 2216, class 9) and KRILL MEAL(UN 3497)
                \n7.6.2.7.2.2 For containers:
                \n.1 After packing, the doors and other openings shall be sealed to prevent the penetration of air into the unit.
                \n.2 Temperature readings in the hold shall be taken once a day early in the morning during the voyage and recorded.
                \n.3 If the temperature of the hold rises excessively above ambient and continues to increase, the possible need to apply copious quantities of water in an emergency and the consequent risk to the stability of the ship shall be considered.
                \n.4 The cargo shall be stowed protected from sources of heat." },
            {"7.6.2.7.3", @"Stowage provisions for SEED CAKE (UN 1386)
             \n7.6.2.7.3.1 Stowage provisions for SEED CAKE, containing vegetable oil (a) mechanically expelled seeds, containing more than 10% oil or more than 20% oil and moisture combined:
            \n\t.1 through and surface ventilation is required;
            \n\t.2 if the voyage exceeds 5 days, the ship shall be equipped with facilities for introducing carbon dioxide or inert gas into the cargo spaces;
            \n\t.3 bags shall always be stowed in double strip, as shown in 7.6.2.7.2.3 of this Code for fishmeal, unstabilized; and
            \n\t.4 regular temperature readings shall be taken at varying depths in the cargo space and recorded.If the temperature of the cargo exceeds 55°C and continues to increase, ventilation to the cargo spaces shall be restricted. If self-heating continues, then carbon dioxide or inert gas shall be introduced.
            \n7.6.2.7.3.2 Stowage provisions for SEED CAKE, containing vegetable oil (b) solvent extractions and expelled seeds containing not more than 10% of oil and, when the amount of moisture is higher than 10%, not more than 20% of oil and moisture combined:
            \n\t.1 surface ventilation is required to assist in removing any residual solvent vapour;
            \n\t.2 if bags are stowed without provision for ventilation to circulate throughout the stow and the voyage exceeds 5 days, regular temperature readings shall be taken at varying depths in the hold and recorded; and
            \n\t.3 if the voyage exceeds 5 days, the vessel shall be equipped with facilities for introducing carbon dioxide or inert gas into the cargo spaces."}
        };
        #endregion
    }
}
