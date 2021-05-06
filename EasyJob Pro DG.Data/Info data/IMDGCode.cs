using System.Collections.Generic;

namespace EasyJob_ProDG.Data.Info_data
{
    public static class IMDGCode
    {

        public static List<string> AllValidDgClasses = new List<string>()
        {
            "1.1", "1.2", "1.3", "1.4", "1.5", "1.6",
            "2.1", "2.2", "2.3", "3", "4.1", "4.2", "4.3",
            "5.1", "5.2", "6.1", "6.2", "7", "8", "9"
        };

        #region Segregation groups
        public enum SegregationGroup : byte
        {
            acids = 1,
            ammonium_compounds,
            bromates,
            chlorates,
            chlorites,
            cyanides,
            heavy_metals_and_their_salts,
            hypochlorites,
            lead_and_its_compounds,
            liquid_halogenated_hydrocarbons,
            mercury_and_mercury_compounds,
            nitrites_and_their_mixtures,
            perchlorates,
            permanganates,
            powdered_metals,
            peroxides,
            azides,
            alkalis,
            strong_acids
        }

        public static string[] SegregationGroups =
        {
            "none",
            "SGG1 - acids",
            "SGG2 - ammonium compounds",
            "SGG3 - bromates",
            "SGG4 - chlorates",
            "SGG5 - chlorites",
            "SGG6 - cyanides",
            "SGG7 - heavy metals and their salts",
            "SGG8 - hypochlorites",
            "SGG9 - lead and its compounds",
            "SGG10 - liquid halogenated hydrocarbons",
            "SGG11 - mercury and mercury compounds",
            "SGG12 - nitrites and their mixtures",
            "SGG13 - perchlorates",
            "SGG14 - permanganates",
            "SGG15 - powdered metals",
            "SGG16 - peroxides",
            "SGG17 - azides",
            "SGG18 - alkalis",
            "SGG1a - strong acids"
        };

        public static string[] SegregationGroupsCodes =
        {
            "none",
            "SGG1",
            "SGG2",
            "SGG3",
            "SGG4",
            "SGG5",
            "SGG6",
            "SGG7",
            "SGG8",
            "SGG9",
            "SGG10",
            "SGG11",
            "SGG12",
            "SGG13",
            "SGG14",
            "SGG15",
            "SGG16",
            "SGG17",
            "SGG18",
            "SGG1a"
        };

        public static Dictionary<string, string> SegregationGroupMatch = new Dictionary<string, string>()
        {
            {"acids", "SGG1 - acids"},
            {"ammoniumcompounds", "SGG2 - ammonium compounds"},
            {"bromates", "SGG3 - bromates"},
            {"chlorates", "SGG4 - chlorates"},
            {"chlorites", "SGG5 - chlorites"},
            {"cyanides", "SGG6 - cyanides"},
            {"heavymetals", "SGG7 - heavy metals and their salts"},
            {"heavymetalsandtheirsalts", "SGG7 - heavy metals and their salts"},
            {"hypochlorites", "SGG8 - hypochlorites"},
            {"lead", "SGG9 - lead and its compounds"},
            {"leadcompounds", "SGG9 - lead and its compounds"},
            {"leadanditscompounds", "SGG9 - lead and its compounds"},
            {"liquidhalogenatedhydrocarbons", "SGG10 - liquid halogenated hydrocarbons"},
            {"mercury", "SGG11 - mercury and mercury compounds"},
            {"mercurycompounds", "SGG11 - mercury and mercury compounds"},
            {"mercuryandmercurycompounds", "SGG11 - mercury and mercury compounds"},
            {"nitrites", "SGG12 - nitrites and their mixtures"},
            {"nitritesmixtures", "SGG12 - nitrites and their mixtures"},
            {"nitrites and their mixtures", "SGG12 - nitrites and their mixtures"},
            {"perchlorates", "SGG13 - perchlorates"},
            {"permanganates", "SGG14 - permanganates"},
            {"powderedmetals", "SGG15 - powdered metals"},
            {"peroxides", "SGG16 - peroxides"},
            {"azides", "SGG17 - azides"},
            {"alkalis", "SGG18 - alkalis"},
            {"strongacids", "SGG1a - strong acids"}

        };
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
            "When dangerous goods are classes 2.1,2.3,3,4,5,6.1(B),6.1(D),8(B),8(C), and 9 are carried under deck, they are to be carried out in closed freight containers only.",
            "For classes 2,3,4 liquids,5.1 liquids, 6.1, and 8 and 9 when carried in closed freight containers in purpose built dedicated container cargo spaces, the ventilation rate may be reduced to not less than two air exchanges per hour.",
            "Power ventilation is not required for class 4 (solid) and 5.1 (liquid) when carried out in closed freight containers",
            "Stowage of class 5.2 underdeck is prohibited",
            "Stowage of class 2.3 having subsidiary risk class 2.1 underdeck is prohibited",
            "Stowage of class 4.3 liquids having a flashpoint less than 23°C under deck is prohibited",
            "No special requirements for class 6.2/7 or for the goods in limited quantities",
            "Dangerous goods of class 9 in package form, which according to the IMDG code emmit flammable vapours, not to be carried in hold No 8"
        };
        #endregion
    }
}
