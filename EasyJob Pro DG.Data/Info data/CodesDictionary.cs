using System.Collections.Generic;

namespace EasyJob_ProDG.Data.Info_data
{
    public partial class CodesDictionary
    {
        /// <summary>
        /// Contains all prefixes to existing conflict codes (other than special stowage or segregation codes)
        /// </summary>
        public static string[] ConflictCodesPrefixes = new [] { "SSC", "SGC", "EXC", "EXPL", "FF" };
        
        public static Dictionary<string, string> ConflictCodes = new Dictionary<string, string>
        {
            {"SSC1","Stowage under deck is not permitted by stowage category (7.1.3)." },
            {"SSC2", "Not permitted according to ship's DOC" },
            {"SSC3", "For stowage of FISHMEAL, UNSTABILIZED (UN 1374)}, FISHMEAL, STABILIZED (UN 2216) and KRILL MEAL (UN 3497) in containers, the provisions of 7.6.2.7.2.2 also apply." },
            {"SSC3a", "Temperature readings in the hold shall be taken once a day early in the morning during the voyage and recorded. (7.6.2.7.2.2)" },
            {"SSC3b", "The cargo shall be stowed protected from sources of heat. (7.6.2.7.2.2)" },
            {"SSC4", "For stowage of AMMONIUM NITRATE (UN 1942)}, AMMONIUM NITRATE BASED FERTILIZER (UN 2067 AND 2071) in containers, the applicable provisions of 7.6.2.8.4 and 7.6.2.11.1 also apply." },
            {"SSC5", "Marine pollutants and infectious substances: Where stowage on deck only is required, preference shall be given to stowage on well-protected decks or to stowage inboard in sheltered areas of exposed decks. (7.1.4.2)." },
            {"SSC5a", "Marine pollutants and infectious substances: Where stowage is permitted on deck or under deck, under deck stowage is preferred. (7.1.4.2)." },
            {"SSC6", "Goods of class 1 with the exception of division 1.4 shall be stowed not less than a horizontal distance of 12 m from living quarters, life-saving appliances* and areas where the ship’s passengers can access without any authorization or limitation. (7.1.4.4.2)." },
            {"SSC7", "Goods of class 1 with the exception of division 1.4 shall not be positioned closer to the ship’s side than a distance equal to one eighth of the beam or 2.4 m, whichever is the lesser. (7.1.4.4.3)." },
            {"SSC8", "Stowage of goods of class 7 might be violated. Segregation from the crew might be required. Refer to 7.1.4.5.18 and table 7.1.4.5.18." },
            {"SSC22", "For WASTE AEROSOLS or WASTE GAS CARTRIDGES: category C, clear of living quarters." },
            {"SGC1", "Two dg classes are in conflict according to Table 7.2.4" },
            {"SGC11", "Two dg classes are in conflict according to Table 7.2.4, taking into account additional requirement of 'segregation as for class'" },
            {"SGC2","Segregation is not required according to 7.2.6.3.2" },
            {"SGC202", "No segregation needs to be applied to substances within the table 7.2.6.3.4, except that due regard shall continue to be taken of the dangerous reactions specified in the provisions of 7.2.6.1.1 to 7.2.6.1.4. (*Except for substances with the technical name PEROXYACETIC ACID)" },
            {"SGC203", "UN 1325 FLAMMABLE SOLID, ORGANIC, N.O.S. with a technical name as listed in 2.5.3.2.4 under 'exempt' (CYCLOHEXANONE PEROXIDE(S)}, DIBENZOYL PEROXIDE, DI-(2-tert-BUTYLPEROXYISOPROPYL)BENZENE(S)}, DI-4-CHLOROBENZOYL PEROXIDE, DICUMYL PEROXIDE): No segregation needs to be applied to substances within the table 7.2.6.3.4, except that due regard shall continue to be taken of the dangerous reactions specified in the provisions of 7.2.6.1.1 to 7.2.6.1.4." },
            {"SGC21", "Notwithstanding 7.2.3.3 and 7.2.3.4, substances of the same class may be stowed together without regard to segregation required by secondary hazards (subsidiary hazard label(s))}, provided that the substances do not react dangerously with each other (7.2.6.1, 7.2.6.2)" },
            {"SGC201", "Substances of class 8, packing group II or III, that would otherwise be required to be segregated ... “away from” or “separated from” “acids” or ... “alkalis”, may be transported in the same cargo transport unit, provided [particular conditions are followed], the transport document must include the statement 'Transport in accordance with 7.2.6.5 of the IMDG Code' (7.2.6.5)" },
            {"SGC3", "Shall not be stowed underdeck together with a container under temperature control (7.4.2.3.3)}, unless that reefer is of a certified safe type" },
            {"SGC4", "Shall be stowed at least 2.4 m from any potential source of ignition (7.4.2.3.2)}, incl. reefer containers, unless that reefer is of a certified safe type" },
            {"SGC5", "Articles of division 1.4, compatibility group S in limited quantities shall not be stowed in the same compartment or hold, or cargo transport unit with dangerous goods of class 1 of compatibility groups A and L. (3.4.4.2)" },
            {"SGC6", "Articles of division 1.4, compatibility group S in limited quantities shall not be stowed in the same compartment or hold, or cargo transport unit with dangerous goods of class 1 of compatibility groups A and L. (Corresponding unit compatibility group was not defined, and has to be checked by the user) (3.4.4.2)" },
            {"SGC7", "Goods of class 1 shall not be stowed within a horizontal distance of 6 m from potential sources of ignition (including refrigerated or heated cargo transport units unless they are of certified safe type). (7.1.4.4.4)" },
            {"SGC8", "Segregation spacing of at least 6 m might be required. For segregation requirements between cargoes of class 7 refer to 7.1.4.5.15, 7.1.4.5.16, 7.1.4.5.17 and 7.2.6.3.1" },
            { "EXPL0", "Compatibility group is not provided, therefore it was not taken into account for segregation purpose." },
            { "EXPL1", "Explosive articles in compatibility group G (other than fireworks and those requiring special stowage) may be stowed with explosive articles of compatibility groups C, D and E provided no explosive substances are transported in the same compartment or hold, or closed cargo transport unit. (Table 7.2.7.1.4)"},
            { "EXPL2", "A consignment of one type in compatibility group L shall only be stowed with a consignment of the same type within compatibility group L. (Table 7.2.7.1.4)"},
            { "EXPL3", "Different types of articles of Division 1.6, compatibility group N, may only be transported together when it is proven that there is no additional risk of sympathetic detonation between the articles. Otherwise they shall be treated as division 1.1. (Table 7.2.7.1.4)"},
            { "EXPL4", "When articles of compatibility group N are transported with articles or substances of compatibility groups C, D or E, the goods of compatibility group N shall be treated as compatibility group D. (Table 7.2.7.1.4)"},
            { "EXPL5", "When articles of compatibility group N are transported together with articles or substances of compatibility group S, the entire load shall be treated as compatibility group N. (Table 7.2.7.1.4)"},
            { "EXPL6", "Any combination of articles in compatibility groups C, D and E shall be treated as compatibility group E. Any combination of substances in compatibility groups C and D shall be treated as the most appropriate compatibility group shown in 2.1.2.3, taking into account the predominant characteristics of the combined load. This overall classification code shall be displayed on any label or placard placed on a unit load or closed cargo transport unit as prescribed in 5.2.2.2.2. (Table 7.2.7.1.4)" },
            { "EXPLOTHERS", "Notwithstanding the segregation provisions of this chapter, AMMONIUM NITRATE (UN 1942)}, AMMONIUM NITRATE FERTILIZERS (UN 2067)}, alkali metal nitrates (e.g. UN 1486) and alkaline earth metal nitrates ... may be stowed together with blasting explosives (except TYPE C) provided the aggregate is treated as blasting explosives under class 1. (7.2.7.2.1)" },
            {"EXC9", "Cargo transport units containing substances used for cooling or conditioning purposes (other than fumigation) during transport are not subject to any provisions of this Code other than those of section 5.3. (5.5.3.2.1)" },
            {"FF1", "For fire-fighting purposes, dry lithium chloride powder, dry sodium chloride or graphite powder should be carried on board when this substance is transported. (Chapter 3.2 – Dangerous Goods List: Properties and observations column (17))" }


        };

        public static Dictionary<string, string> Segregation = new Dictionary<string, string>()
        {
            {"SG1", "For packages carrying a subsidiary hazard label of class 1, segregation as for class 1, division 1.3. However, in relation to goods of class 1, segregation as for the primary hazard."},
            {"SG2", "Segregation as for class 1.2G."},
            {"SG3", "Segregation as for class 1.3G."},
            {"SG4", "Segregation as for class 2.1."},
            {"SG5", "Segregation as for class 3."},
            {"SG6", "Segregation as for class 5.1."},
            {"SG7", "Stow “away from” class 3."},
            {"SG8", "Stow “away from” class 4.1."},
            {"SG9", "Stow “away from” class 4.3."},
            {"SG10", "Stow “away from” class 5.1."},
            {"SG11", "Stow “away from” class 6.2."},
            {"SG12", "Stow “away from” class 7."},
            {"SG13", "Stow “away from” class 8."},
            {"SG14", "Stow “separated from” class 1 except for division 1.4S."},
            {"SG15", "Stow “separated from” class 3."},
            {"SG16", "Stow “separated from” class 4.1."},
            {"SG17", "Stow “separated from” class 5.1."},
            {"SG18", "Stow “separated from” class 6.2."},
            {"SG19", "Stow “separated from” class 7."},
            {"SG20", "Stow “away from” SGG1 - acids."},
            {"SG21", "Stow “away from” SGG18 - alkalis."},
            {"SG22", "Stow “away from” ammonium salts."},
            {"SG23", "Stow “away from” animal or vegetable oils."},
            {"SG24", "Stow “away from” SGG17 - azides."},
            {"SG25", "Stow “separated from” goods of classes 2.1 and 3."},
            {"SG26", @"In addition: from goods of classes 2.1 and 3 when stowed on deck of a containership a minimum distance of two container spaces athwartship shall be maintained, when stowed on ro-ro ships a distance of 6 m athwartship shall be maintained."},
            {"SG27", "Stow “separated from” explosives containing chlorates or perchlorates."},
            {"SG28", @"Stow “separated from” SGG2 – ammonium compounds and explosives containing ammonium compounds or salts."},
            {"SG29", "Segregation from foodstuffs as in 7.3.4.2.2, 7.6.3.1.2 or 7.7.3.7."},
            {"SG30", "Stow “away from” SGG7 - heavy metals and their salts."},
            {"SG31", "Stow “away from” SGG9 - lead and its compounds."},
            {"SG32", "Stow “away from” SGG10 - liquid halogenated hydrocarbons."},
            {"SG33", "Stow “away from” SGG15 - powdered metals."},
            {"SG34", @"When containing ammonium compounds, “separated from” SGG4 – chlorates or SGG13 – perchlorates and explosives containing chlorates or perchlorates."},
            {"SG35", "Stow “separated from” SGG1 - acids."},
            {"SG36", "Stow “separated from” SGG18 - alkalis."},
            {"SG37", "Stow “separated from” ammonia."},
            {"SG38", "Stow “separated from” SGG2 - ammonium compounds."},
            {"SG39", "Stow “separated from” SGG2 - ammonium compounds other than AMMONIUM PERSULPHATE (UN 1444)."},
            {"SG40", @"Stow “separated from” SGG2 - ammonium compounds other than mixtures of ammonium persulphates and/or potassium persulphates and/or sodium persulphates."},
            {"SG41", "Stow “separated from” animal or vegetable oil."},
            {"SG42", "Stow “separated from” SGG3 - bromates."},
            {"SG43", "Stow “separated from” bromine."},
            {"SG44", "Stow “separated from” CARBON TETRACHLORIDE (UN 1846)."},
            {"SG45", "Stow “separated from” SGG4 - chlorates."},
            {"SG46", "Stow “separated from” chlorine."},
            {"SG47", "Stow “separated from” SGG5 - chlorites."},
            {"SG48", @"Stow “separated from” combustible material (particularly liquids)."},
            {"SG49", "Stow “separated from” SGG6 - cyanides."},
            {"SG50", "Segregation from foodstuffs as in 7.3.4.2.1, 7.6.3.1.2 or 7.7.3.6."},
            {"SG51", "Stow “separated from” SGG8 - hypochlorites."},
            {"SG52", "Stow “separated from” iron oxide."},
            {"SG53", "Shall not be stowed together with combustible material in the same cargo transport unit."},
            {"SG54", "Stow “separated from” SGG11 - mercury and mercury compounds."},
            {"SG55", "Stow “separated from” mercury salts."},
            {"SG56", "Stow “separated from” SGG12 - nitrites."},
            {"SG57", "Stow “separated from” odour-absorbing cargoes."},
            {"SG58", "Stow “separated from” SGG13 - perchlorates."},
            {"SG59", "Stow “separated from” SGG14 - permanganates."},
            {"SG60", "Stow “separated from” SGG16 - peroxides."},
            {"SG61", "Stow “separated from” SGG15 - powdered metals."},
            {"SG62", "Stow “separated from” sulphur."},
            {"SG63", "Stow “separated longitudinally by an intervening complete compartment or hold from” class 1."},
            {"SG64", "[Reserved]"},
            {"SG65", "Stow “separated by a complete compartment or hold from” class 1 except for division 1.4."},
            {"SG66", "[Reserved]"},
            {"SG67", @"Stow “separated from” division 1.4 and “separated longitudinally by an intervening complete compartment of hold from” divisions 1.1, 1.2, 1.3, 1.5 and 1.6 except from explosives of compatibility group J."},
            {"SG68", "If flashpoint 60°C c.c.or below, segregation as for class 3 but “away from” class 4.1."},
            {"SG69", @"For AEROSOLS with a maximum capacity of 1 L: segregation as for class 9.
                       Stow “separated from” class 1 except for division 1.4.
                       For AEROSOLS with a capacity above 1 L: segregation as for the appropriate subdivision of class 2.
                       For WASTE AEROSOLS: segregation as for the appropriate subdivision of class 2."},
            {"SG70", "For arsenic sulphides, “separated from” SGG1 - acids."},
            {"SG71", @"Within the appliance, to the extent that the dangerous goods are integral parts of the complete life-saving appliance, there is no need to apply the provisions on segregation of substances in chapter 7.2."},
            {"SG72", "See 7.2.6.3."},
            {"SG73", "[Reserved]"},
            {"SG74", "Segregation as for 1.4G."},
            {"SG76", "Segregation as for class 7." },
            {"SG77", "Segregation as for class 8. However, in relation to class 7, no segregation needs to be applied." },
            {"SG78", "Stow “separated longitudinally by an intervening complete compartment or hold from” division 1.1, 1.2, and 1.5."}
        };

        public static Dictionary<string, string> Stowage = new Dictionary<string, string>()
        {
            { "H1", "Keep as dry as reasonably practicable." },
            { "H2", "Keep as cool as reasonably practicable." },
            { "H3", "During transport, it should be stowed (or kept) in a cool ventilated place." },
            { "H4", @"If cleaning of cargo spaces has to be carried out at sea, the safety procedures followed and standard of equipment used shall be at least as effective as those em ployed as industry best practice in a port. Until such cleaning is undertaken, the cargo spaces in which the asbestos has been carried shall be closed and access to those spaces shall be prohibited."},
            { "H5", "Avoid handling the packaging or large packaging or keep handling to a minimum. Inform the appropriate public health authority or veterinary authority where persons or animals may have been exposed." },
            { "SW1", "Protected from sources of heat." },
            { "SW2", "Clear of living quarters."},
            {"SW3", "Shall be transported under temperature control."},
            {"SW4", "Surface ventilation is required to assist in removing any residual solvent vapour."},
            {"SW5", "If under deck, stow in a mechanically ventilated space."},
            {"SW6", @"When stowed under deck, mechanical ventilation shall be in accordance with SOLAS regulation II-2/19 (II-2/54) for flammable liquids with flashpoint below 23°C c.c."},
            {"SW7", "As approved by the competent authorities of the countries involved in the shipment."},
            {"SW8", @"Ventilation may be required. The possible need to open hatches in case of fire to provide maximum ventilation and to apply water in an emergency, and the consequent risk to the stability of the ship through flooding of the cargo spaces, shall be considered before loading."},
            {"SW9", @"Provide a good through ventilation for bagged cargo. Double strip stowage is recommended.
                    The illustration in 7.6.2.7.2.3 shows how this can be achieved. During the voyage regular temperature readings shall be taken at varying depths in the hold and recorded. If the temperature of the cargo exceeds the ambient temperature and continues to increase, ventilation shall be closed down."},
            {"SW10", @"Unless carried in closed cargo transport units, bales shall be properly covered by tarpaulins or the like. Cargo spaces shall be clean, dry and free from oil or grease. Ventilator cowls leading into the cargo space shall have sparking-preventing screens. All other openings entrances and hatches leading to the cargo space shall be securely closed. During temporary interruption of loading, when the hatch remains uncovered, a fire-watch shall be kept. During loading or discharge, smoking in the vicinity"},
            {"SW11", @"Cargo transport units shall be shaded from direct sunlight. Packages in cargo transport units shall be stowed so as to allow for adequate air circulation throughout the cargo."},
            {"SW12", "Taking account of any supplementary requirements specified in the transport documents."},
            {"SW13", @"Taking account of any supplementary requirements specified in the competent authority approval certificate(s)."},
            {"SW14", "Category A only if the special stowage provisions of 7.4.1.4 and 7.6.2.8.4 are complied with."},
            {"SW15", "For metal drums, stowage category B."},
            {"SW16", "For unit loads in open cargo transport units, stowage category B."},
            {"SW17", @"Category E, for closed cargo transport unit and pallet boxes only. Ventilation may be required. The possible need to open hatches in case of fire to provide maximum ventilation and to apply water in an emergency, and the consequent risk to the stability of the ship through flooding of the cargo space, shall be considered before loading."},
            {"SW18", "Category A, when transported in accordance with P650."},
            {"SW19", @"For batteries transported in accordance with special provisions 376 or 377, category C, unless transported on a short international voyage."},
            {"SW20", "For uranyl nitrate hexahydrate solution stowage, category D applies."},
            {"SW21", "For uranium metal pyrophoric and thorium metal pyrophoric stowage, category D applies."},
            {"SW22", @"For AEROSOLS with a maximum capacity of 1 L: category A.
                       For AEROSOLS with a capacity above 1 L: category B.
                       For WASTE AEROSOLS or WASTE GAS CARTRIDGES: category C, clear of living quarters."},
            {"SW23", "When transported in BK3 bulk container, see 7.6.2.12 and 7.7.3.9."},
            {"SW24", "For special stowage provisions, see 7.4.1.3 and 7.6.2.7.2."},
            {"SW25", "For special stowage provisions, see 7.6.2.7.3."},
            {"SW26", "For special stowage provisions, see 7.4.1.4 and 7.6.2.11.1.1."},
            {"SW27", "For special stowage provisions, see 7.6.2.7.2.1."},
            {"SW28", "As approved by the competent authority of the country of origin."},
            {"SW29", "For engines or machinery containing fuels with flashpoint equal or greater than 23°C, stowage Category A."},
            {"SW30", "For special stowage provisions, see 7.1.4.4.5" }
        };


        /// <summary>
        /// Returns prefix of a conflict code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetCodePrefix(string code)
        {
            string prefix = string.Empty;
            for(byte i =0; i < 4; i++)
            {
                if (char.IsDigit(code[i])) break;
                prefix += code[i];
            }
            return prefix;
        }
    }
}
