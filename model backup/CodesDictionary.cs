using System;
using System.Collections.Generic;


namespace EasyJob_Pro_DG
{
    public class CodesDictionary
    {
        #region Conflict codes
        public static Dictionary<string, string> ConflictCodes = new Dictionary<string, string>
        {
            {"SSC1","Stowage under deck is not permitted by stowage category (7.1.3)." },
            {"SSC2", "Not permitted according to ship's DOC" },
            {"SSC3", "For stowage of FISHMEAL, UNSTABILIZED (UN 1374), FISHMEAL, STABILIZED (UN 2216) and KRILL MEAL (UN 3497) in containers, the provisions of 7.6.2.7.2.2 also apply." },
            {"SSC4", "For stowage of AMMONIUM NITRATE (UN 1942), AMMONIUM NITRATE BASED FERTILIZER (UN 2067 AND 2071) in containers, the applicable provisions of 7.6.2.8.4 and 7.6.2.11.1 also apply." },
            {"SSC5", "Marine pollutants: Where stowage on deck ... preference shall be given to stowage on well-protected decks or to stowage inboard in sheltered areas of exposed decks (7.1.4.2). (Not to be stowed on sea-sides)" },
            {"SSC6", "Goods of class 1 with the exception of division 1.4 shall be stowed not less than a horizontal distance of 12 m from living quarters, life-saving appliances and areas with public access. (7.1.4.4.2)." },
            {"SSC7", "Goods of class 1 with the exception of division 1.4 shall not be positioned closer to the ship’s side than a distance equal to one eighth of the beam or 2.4 m, whichever is the lesser. (7.1.4.4.3)." },
            {"SSC8", "Stowage of goods of class 7 might be violated. Segregation from the crew might be required. Refer to 7.1.4.5.18 and table 7.1.4.5.18." },
            {"SGC1", "Two dg classes are in conflict according to Table 7.2.4" },
            {"SGC11", "Two dg classes are in conflict according to Table 7.2.4, taking into account additional requirement of 'segregation as for class'" },
            {"SGC2","Segregation is not required according to 7.2.6.3.2" },
            {"SGC21", "Notwithstanding 7.2.3.3 and 7.2.3.4, substances of the same class may be stowed together without regard to segregation required by secondary hazards (subsidiary risk label(s)), provided that the substances do not react dangerously with each other (7.2.6.1, 7.2.6.2)" },
            {"SGC201", "Substances of class 8, packing group II or III, that would otherwise be required to be segregated ... “away from” or “separated from” “acids” or ... “alkalis”, may be transported in the same cargo transport unit, provided [particular conditions are followed], the transport document must include the statement 'Transport in accordance with 7.2.6.4 of the IMDG Code' (7.2.6.4)" },
            {"SGC3", "Shall not be stowed underdeck together with a container under temperature control (7.4.2.3.3), unless that reefer is of a certified safe type" },
            {"SGC4", "Shall be stowed at least 2.4 m from any potential source of ignition (7.4.2.3.2), incl. reefer containers, unless that reefer is of a certified safe type" },
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
            { "EXPLOTHERS", "Notwithstanding the segregation provisions of this chapter, AMMONIUM NITRATE (UN 1942), AMMONIUM NITRATE FERTILIZERS (UN 2067), alkali metal nitrates (e.g. UN 1486) and alkaline earth metal nitrates ... may be stowed together with blasting explosives (except TYPE C) provided the aggregate is treated as blasting explosives under class 1. (7.2.7.2.1)" }


        };
        #endregion

        #region IMDG quotes
        internal static Dictionary<string, string> Paragraphs = new Dictionary<string, string>
        {
            {"7.4.1.4", @"For stowage of AMMONIUM NITRATE (UN 1942), AMMONIUM NITRATE BASED FERTILIZER (UN 2067 AND 2071) in containers, the applicable provisions of 7.6.2.8.4 and 7.6.2.11.1 also apply." },
            {"7.4.1.3", @"For stowage of FISHMEAL, UNSTABILIZED (UN 1374), FISHMEAL, STABILIZED (UN 2216) and KRILL MEAL (UN 3497) in containers, the provisions of 7.6.2.7.2.2 also apply." },
            { "7.6.2.8.4", @"UN 1942 AMMONIUM NITRATE and UN 2067 AMMONIUM NITRATE BASED FERTILIZER may be stowed under deck in a clean cargo space capable of being opened up in an emergency. The possible need to open hatches in case of fire to provide maximum ventilation and to apply water in an emergency and the consequent risk to the stability of the ship through flooding of cargo space shall be considered before loading." },
            {"7.6.2.11.1", @"Stowage provisions for AMMONIUM NITRATE BASED FERTILIZER, UN 2071
                \n7.6.2.11.1.1 AMMONIUM NITRATE BASED FERTILIZER, UN 2071 shall be stowed in a clean cargo space capable of being opened up in an emergency. In the case of bagged fertilizer or fertilizer in containers or in bulk containers, it is sufficient if, in the case of an emergency, the cargo is accessible through free approaches (hatch entries), and mechanical ventilation enables the master to exhaust any gases or fumes resulting from decomposition. The possible need to open hatches in case of fire to provide maximum ventilation and to apply water in an emergency, and the consequent risk to the stability of the ship through flooding of the cargo space, shall be considered before loading.
                \n7.6.2.11.1.2 If suppression of decomposition should prove impracticable (such as in bad weather), there would not necessarily be immediate danger to the structure of the ship. However, the residue left after decomposition may have only half the mass of the original cargo; this loss of mass may also affect the stability of the ship and shall be considered before loading.
                \n7.6.2.11.1.3 AMMONIUM NITRATE BASED FERTILIZER, UN 2071 shall be stowed out of direct contact with a metal engine-room bulkhead.In the case of bagged material, this may be done, for example, by using wooden boards to provide an air space between the bulkhead and the cargo.This requirement need not apply to short international voyages.
                \n7.6.2.11.1.4 In the case of ships not fitted with smoke-detecting or other suitable devices, arrangements shall be made during the voyage to inspect cargo spaces containing these fertilizers at intervals not exceeding 4 h (such as to sniff at the ventilators serving them) to ensure early detection of decomposition should that occur." },
            {"7.6.2.7.2", @"Stowage provisions for FISHMEAL, UNSTABILIZED (UN 1374), FISHMEAL, STABILIZED (UN 2216, class 9) and KRILL MEAL(UN 3497)
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

        #region Container Types
        ////Closed cargo transport unit, with the exception of class 1, means a cargo transport unit which totally encloses
        ////the contents by permanent structures with complete and rigid surfaces.Cargo transport units with fabric
        ////sides or tops are not considered closed cargo transport units; for definition of closed cargo transport unit for
        ////class 1, see 7.1.2.
        public static List<Types> types = new List<Types>
        {
            new Types ("22B0", "Bulk",20,8.5,"22B0",true),
            new Types ("22B1,Dry Bulk,20,8.5,22B0,true"),
            new Types("22B3,Dry Bulk,20,8.5,22B0,true"),
            new Types("22B4,Dry Bulk,20,8.5,22B0,true"),
            new Types("22B5,Dry Bulk,20,8.5,22B0,true"),
            new Types("22B6,Dry Bulk,20,8.5,22B0,true"),
            new Types("22BK,Dry Bulk,20,8.5,22B0,true"),
            new Types("2080,Dry Bulk,20,8,22B0,true"),
            new Types("20B0,Dry Bulk,20,8,22B0,true"),
            new Types("20B1,Dry Bulk,20,8,22B0,true"),
            new Types("20B3,Dry Bulk,20,8,22B0,true"),
            new Types("20B4,Dry Bulk,20,8,22B0,true"),
            new Types("20B5,Dry Bulk,20,8,22B0,true"),
            new Types("20B6,Dry Bulk,20,8,22B0,true"),
            new Types("20BK,Dry Bulk,20,8,22B0,true"),
            new Types("20BU,Dry Bulk,20,8,22B0,true"),
            new Types("2280,Dry Bulk,20,8.5,22B0,true"),
            new Types("2281,Dry Bulk,20,8.5,22B0,true"),
            new Types("2299,Air/Surface,20,8.5,22B0,true"),
            new Types("22BU,Dry Bulk,20,8.5,22B0,true"),
            new Types("22G0,Standard Dry,20,8.5,22G0,true"),
            new Types("22G1,Standard Dry,20,8.5,22G0,true"),
            new Types("22G2,Standard Dry,20,8.5,22G0,true"),
            new Types("22G3,Standard Dry,20,8.5,22G0,true"),
            new Types("22V3,Standard Dry,20,8.5,22G0,true"),
            new Types("2300,Standard Dry,20,8.5,22G0,true"),
            new Types("2301,Standard Dry,20,8.5,22G0,true"),
            new Types("2302,Standard Dry,20,8.5,22G0,true"),
            new Types("2303,Standard Dry,20,8.5,22G0,true"),
            new Types("2304,Standard Dry,20,8.5,22G0,true"),
            new Types("2410,HIGH CUBE,20,9.5,22G0,true"),
            new Types("24G0,Standard Dry,20,9,22G0,true"),
            new Types("24G1,Standard Dry,20,9,22G0,true"),
            new Types("24G2,Standard Dry,20,9,22G0,true"),
            new Types("24G3,Standard Dry,20,9,22G0,true"),
            new Types("24GP,Standard Dry,20,9,22G0,true"),
            new Types("2500,Standard Dry,20,8.5,22G0,true"),
            new Types("25G0,Standard Dry High Cube,20,9,22G0,true"),
            new Types("2600,Standard Dry,20,4.25,22G0,true"),
            new Types("26G0,Standard Dry,20,9.5,22G0,true"),
            new Types("26G1,Standard Dry,20,9.5,22G0,true"),
            new Types("26G2,Standard Dry,20,9.5,22G0,true"),
            new Types("26G3,Standard Dry,20,9.5,22G0,true"),
            new Types("26GP,Standard Dry,20,9.5,22G0,true"),
            new Types("28G0,Standard Dry,20,4.25,22G0,true"),
            new Types("28GP,Standard Dry,20,4.25,22G0,true"),
            new Types("28U1,BIN HALF HEIGHT (OPEN TOP),20,4.25,22G0,false"),
            new Types("28U2,OPENING(S) AT ONE OR BOTH ENDS PLUS REMV TOP MEMB,20,8.5,22G0,false"),
            new Types("28UT,OPENING(S) AT ONE OR BOTH ENDS PLUS REMV TOP MEMB,20,8.5,22G0,false"),
            new Types("2994,Air/Surface,20,4,22G0,true"),
            new Types("2999,SLIDER CHASSIS,20,0,22G0,true"),
            new Types("3000,Standard Dry,30,8,22G0,true"),
            new Types("30G0,DRY CARGO/GENERAL PURPOSE,30,8,22G0,true"),
            new Types("3200,Standard Dry,30,8.5,22G0,true"),
            new Types("32G0,DRY CARGO/GENERAL PURPOSE,30,8.5,22G0,true"),
            new Types("3399,TRIAXLE CHASSIS,23,0,22G0,true"),
            new Types("7999,SLIDER CHASSIS,20,0,22G0,true"),
            new Types("B2G1,PASSIVE VENTS AT UPPER PART OF CARGO SPACE,24,8.5,22G0,true"),
            new Types("1000,Standard Dry,10,8,22G0,true"),
            new Types("10G0,DRY CARGO/GENERAL PURPOSE,10,8,22G0,true"),
            new Types("1200,Standard Dry,10,8.5,22G0,true"),
            new Types("12G0,DRY CARGO/GENERAL PURPOSE,10,8.5,22G0,true"),
            new Types("2000,Standard Dry,20,8,22G0,true"),
            new Types("2001,Standard Dry,20,8,22G0,true"),
            new Types("2002,Standard Dry,20,8,22G0,true"),
            new Types("2003,Standard Dry,20,8,22G0,true"),
            new Types("2004,Standard Dry,20,8,22G0,true"),
            new Types("2025,Livestock Carrier,20,8,22G0,false"),
            new Types("20G0,Standard Dry,20,8,22G0,true"),
            new Types("20G1,Standard Dry,20,8,22G0,true"),
            new Types("20G2,Standard Dry,20,8,22G0,true"),
            new Types("20G3,Standard Dry,20,8,22G0,true"),
            new Types("20GP,Standard Dry,20,8,22G0,true"),
            new Types("2101,Standard Dry,20,8,22G0,true"),
            new Types("2102,Standard Dry,20,8,22G0,true"),
            new Types("2103,Standard Dry,20,8,22G0,true"),
            new Types("2104,Standard Dry,20,8,22G0,true"),
            new Types("2125,Livestock Carrier,20,8,22G0,false"),
            new Types("2200,Standard Dry,20,8.5,22G0,true"),
            new Types("2201,Standard Dry,20,8.5,22G0,true"),
            new Types("2202,Standard Dry,20,8.5,22G0,true"),
            new Types("2204,Standard Dry,20,8.5,22G0,true"),
            new Types("2205,Standard Dry,20,8.5,22G0,true"),
            new Types("2210,Standard Dry,20,8.5,22G0,true"),
            new Types("2213,Standard Dry,20,8.5,22G0,true"),
            new Types("2225,Livestock Carrier,20,8.5,22G0,false"),
            new Types("22GP,Standard Dry,20,8.5,22G0,true"),
            new Types("2212,General Purpose (Hanging Garments),20,8.5,22G0,true"),
            new Types("25GP,High Cube,20,9.6,22G0,true"),
            new Types("22H0,Insulated (Conair),20,8.5,22H0,true"),
            new Types("22H1,Thermal Refrigerated/Heated,20,8.5,22H0,true"),
            new Types("22H2,Thermal Refrigerated/Heated,20,8.5,22H0,true"),
            new Types("22H5,Thermal Insulated,20,8.5,22H0,true"),
            new Types("22H6,Thermal Insulated,20,8.5,22H0,true"),
            new Types("22HI,Thermal Refrigerated/Heated,20,8.5,22H0,true"),
            new Types("24H5,Thermal Insulated,20,9,22H0,true"),
            new Types("24H6,Thermal Insulated,20,9,22H0,true"),
            new Types("2020,Thermal Insulated,20,8,22H0,true"),
            new Types("20H0,Thermal Refrigerated/Heated,20,8,22H0,true"),
            new Types("20H1,Thermal Refrigerated/Heated,20,8,22H0,true"),
            new Types("20H2,Thermal Refrigerated/Heated,20,8,22H0,true"),
            new Types("20H5,Thermal Insulated,20,8,22H0,true"),
            new Types("20H6,Thermal Insulated,20,8,22H0,true"),
            new Types("20HI,Thermal Refrigerated/Heated,20,8,22H0,true"),
            new Types("20HR,Thermal Refrigerated/Heated,20,8,22H0,true"),
            new Types("2220,Thermal Insulated,20,8.5,22H0,true"),
            new Types("2224,Insulated,20,8.5,22H0,true"),
            new Types("22HR,Thermal Refrigerated/Heated,20,8.5,22H0,true"),
            new Types("22P1,Flat Rack,20,8.5,22P1,false"),
            new Types("22P2,Platform,20,8.5,22P1,false"),
            new Types("22P4,Platform,20,8.5,22P1,false"),
            new Types("22P5,Platform,20,8.5,22P1,false"),
            new Types("22P7,PLATFORM FIXED,20,8.5,22P1,false"),
            new Types("22P8,Platform,20,8.5,22P1,false"),
            new Types("22P9,Platform,20,8.5,22P1,false"),
            new Types("22PL,Platform,20,8.5,22P1,false"),
            new Types("22PS,Platform,20,8.5,22P1,false"),
            new Types("2361,Platform,20,8.5,22P1,false"),
            new Types("2362,Platform,20,8.5,22P1,false"),
            new Types("2363,Platform,20,8.5,22P1,false"),
            new Types("2364,Platform,20,8.5,22P1,false"),
            new Types("2365,Platform,20,8.5,22P1,false"),
            new Types("2366,Platform,20,8.5,22P1,false"),
            new Types("2367,Platform,20,8.5,22P1,false"),
            new Types("2651,Open Top,20,4.25,22P1,false"),
            new Types("2661,Platform,20,4.25,22P1,false"),
            new Types("2761,Platform,20,4.25,22P1,false"),
            new Types("2063,Flat,20,8,22P1,false"),
            new Types("2066,Platform,20,8,22P1,false"),
            new Types("2067,Platform,20,8,22P1,false"),
            new Types("20P2,Platform,20,8,22P1,false"),
            new Types("20P4,Platform,20,8,22P1,false"),
            new Types("20P5,Platform,20,8,22P1,false"),
            new Types("20PC,Platform,20,8,22P1,false"),
            new Types("20PF,Platform,20,8,22P1,false"),
            new Types("20PL,Platform,20,8,22P1,false"),
            new Types("20PS,Platform,20,8,22P1,false"),
            new Types("2160,Flat,20,8,22P1,false"),
            new Types("2161,Platform,20,8,22P1,false"),
            new Types("2162,Platform,20,8,22P1,false"),
            new Types("2163,Platform,20,8,22P1,false"),
            new Types("2164,Platform,20,8,22P1,false"),
            new Types("2165,Platform,20,8,22P1,false"),
            new Types("2166,Platform,20,8,22P1,false"),
            new Types("2167,Platform,20,8,22P1,false"),
            new Types("2260,Flat,20,8.5,22P1,false"),
            new Types("2261,Flat,20,8.5,22P1,false"),
            new Types("2262,Platform,20,8.5,22P1,false"),
            new Types("2265,Platform,20,8.5,22P1,false"),
            new Types("2266,Platform,20,8.5,22P1,false"),
            new Types("2267,Platform,20,8.5,22P1,false"),
            new Types("22PF,Platform,20,8.5,22P1,false"),
            new Types("22PC,Platform,20,8.5,22P1,false"),
            new Types("8888,Uncontainerised,0,0,8888,false"),
            new Types("22P3,Collapsible Flat Rack,20,8.5,22P3,false"),
            new Types("20P3,Platform,20,8,22P3,false"),
            new Types("2263,Flat,20,8.5,22P3,false"),
            new Types("2264,Platform,20,8.5,22P3,false"),
            new Types("22R1,Reefer,20,8.5,22R1,true"),
            new Types("22R2,Thermal Refrigerated/Heated,20,8.5,22R1,true"),
            new Types("22R3,Thermal Refrigerated/Heated,20,8.5,22R1,true"),
            new Types("22R9,Thermal Refrigerated/Heated,20,8.5,22R1,true"),
            new Types("22RC,Thermal Refrigerated/Heated,20,8.5,22R1,true"),
            new Types("22RE,Thermal Refrigerated,20,8.5,22R1,true"),
            new Types("22RS,Thermal Refrigerated/Heated,20,8.5,22R1,true"),
            new Types("2330,Thermal Refrigerated,20,8.5,22R1,true"),
            new Types("2331,Thermal Refrigerated,20,8.5,22R1,true"),
            new Types("2332,Thermal Refrigerated/Heated,20,8.5,22R1,true"),
            new Types("2432,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24H0,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24H1,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24H2,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24HI,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24HR,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24R0,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24R1,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24R2,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24R3,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24RE,Thermal Refrigerated,20,9,22R1,true"),
            new Types("24RS,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("24RT,Thermal Refrigerated/Heated,20,9,22R1,true"),
            new Types("2030,Thermal Refrigerated,20,8,22R1,true"),
            new Types("2031,Thermal Refrigerated,20,8,22R1,true"),
            new Types("2032,Thermal Refrigerated/Heated,20,8,22R1,true"),
            new Types("2040,Thermal Refrigerated,20,8,22R1,true"),
            new Types("2041,Thermal Refrigerated,20,8,22R1,true"),
            new Types("2042,Thermal Refrigerated,20,8,22R1,true"),
            new Types("2043,Thermal Refrigerated,20,8,22R1,true"),
            new Types("20R0,Thermal Refrigerated,20,8,22R1,true"),
            new Types("20R1,Thermal Refrigerated/Heated,20,8,22R1,true"),
            new Types("20R2,Thermal Refrigerated/Heated,20,8,22R1,true"),
            new Types("20R3,Thermal Refrigerated/Heated,20,8,22R1,true"),
            new Types("20RE,Thermal Refrigerated,20,8,22R1,true"),
            new Types("20RS,Thermal Refrigerated/Heated,20,8,22R1,true"),
            new Types("20RT,Thermal Refrigerated/Heated,20,8,22R1,true"),
            new Types("2130,Thermal Refrigerated,20,8,22R1,true"),
            new Types("2131,Thermal Refrigerated,20,8,22R1,true"),
            new Types("2132,Thermal Refrigerated,20,8,22R1,true"),
            new Types("2230,Thermal Refrigerated,20,8.5,22R1,true"),
            new Types("2231,Thermal Refrigerated,20,8.5,22R1,true"),
            new Types("2232,Thermal Refrigerated/Heated,20,8.5,22R1,true"),
            new Types("2240,Thermal Refrigerated,20,8.5,22R1,true"),
            new Types("2242,Thermal Refrigerated,20,8.5,22R1,true"),
            new Types("22R0,Thermal Refrigerated/Heated,20,8.5,22R1,true"),
            new Types("22RT,Thermal Refrigerated/Heated,20,8.5,22R1,true"),
            new Types("2234,Thermal containers. Heated,20,8.5,22R1,true"),
            new Types("22T0,Tank,20,8.5,22T0,true"),
            new Types("22T1,Tank,20,8.5,22T0,true"),
            new Types("22T2,Tank,20,8.5,22T0,true"),
            new Types("22T3,Tank,20,8.5,22T0,true"),
            new Types("22T4,Tank,20,8.5,22T0,true"),
            new Types("22T5,Tank,20,8.5,22T0,true"),
            new Types("22T6,Tank,20,8.5,22T0,true"),
            new Types("22T7,Tank,20,8.5,22T0,true"),
            new Types("22T8,Tank,20,8.5,22T0,true"),
            new Types("22T9,Tank,20,8.5,22T0,true"),
            new Types("22TD,Tank,20,8.5,22T0,true"),
            new Types("22TG,Tank,20,8.5,22T0,true"),
            new Types("2670,Tank,20,4.25,22T0,true"),
            new Types("2671,Tank,20,4.25,22T0,true"),
            new Types("2870,HALF HEIGHT THERMAL TANK,20,0,22T0,true"),
            new Types("2070,Tank,20,8,22T0,true"),
            new Types("2071,Tank,20,8,22T0,true"),
            new Types("2072,Tank,20,8,22T0,true"),
            new Types("2073,Tank,20,8,22T0,true"),
            new Types("2074,Tank,20,8,22T0,true"),
            new Types("2075,Tank,20,8,22T0,true"),
            new Types("2076,Tank,20,8,22T0,true"),
            new Types("2077,Tank,20,8,22T0,true"),
            new Types("2078,Tank,20,8,22T0,true"),
            new Types("2079,Tank,20,8,22T0,true"),
            new Types("20T0,Tank,20,8,22T0,true"),
            new Types("20T1,Tank,20,8,22T0,true"),
            new Types("20T2,Tank,20,8,22T0,true"),
            new Types("20T3,Tank,20,8,22T0,true"),
            new Types("20T4,Tank,20,8,22T0,true"),
            new Types("20T5,Tank,20,8,22T0,true"),
            new Types("20T6,Tank,20,8,22T0,true"),
            new Types("20T7,Tank,20,8,22T0,true"),
            new Types("20T8,Tank,20,8,22T0,true"),
            new Types("20T9,Tank,20,8,22T0,true"),
            new Types("20TD,Tank,20,8,22T0,true"),
            new Types("20TG,Tank,20,8,22T0,true"),
            new Types("20TN,Tank,20,8,22T0,true"),
            new Types("2270,Tank,20,8.5,22T0,true"),
            new Types("2271,Tank,20,8.5,22T0,true"),
            new Types("2272,Tank,20,8.5,22T0,true"),
            new Types("2273,Tank,20,8.5,22T0,true"),
            new Types("2274,Tank,20,8.5,22T0,true"),
            new Types("2275,Tank,20,8.5,22T0,true"),
            new Types("2276,Tank,20,8.5,22T0,true"),
            new Types("2277,Tank,20,8.5,22T0,true"),
            new Types("2278,Tank,20,8.5,22T0,true"),
            new Types("2279,Tank,20,8.5,22T0,true"),
            new Types("22TN,Tank,20,8.5,22T0,true"),
            new Types("22U1,Open Top,20,8.5,22U1,false"),
            new Types("22U2,Open Top,20,8.5,22U1,false"),
            new Types("22U3,Open Top,20,8.5,22U1,false"),
            new Types("22U4,Open Top,20,8.5,22U1,false"),
            new Types("22U5,Open Top,20,8.5,22U1,false"),
            new Types("22U6,Standard Dry,20,8.5,22U1,true"),
            new Types("2650,Open Top,0,4.25,22U1,false"),
            new Types("2750,Open Top,20,4.25,22U1,false"),
            new Types("2770,Tank,20,4.25,22U1,true"),
            new Types("2771,Tank,20,4.25,22U1,true"),
            new Types("2851,HALF OPEN TOP,20,0,22U1,false"),
            new Types("2050,Open Top,20,8,22U1,false"),
            new Types("2051,Open Top,20,8,22U1,false"),
            new Types("2052,Open Top,20,8,22U1,false"),
            new Types("2053,Open Top,20,8,22U1,false"),
            new Types("20U0,Open Top,20,8,22U1,false"),
            new Types("20U1,Open Top,20,8,22U1,false"),
            new Types("20U2,Open Top,20,8,22U1,false"),
            new Types("20U3,Open Top,20,8,22U1,false"),
            new Types("20U4,Open Top,20,8,22U1,false"),
            new Types("20U5,Open Top,20,8,22U1,false"),
            new Types("20UT,Open Top,20,8,22U1,false"),
            new Types("2150,Open Top,20,8,22U1,false"),
            new Types("2203,Standard Dry,20,8.5,22U1,true"),
            new Types("2250,Open Top,20,8.5,22U1,false"),
            new Types("2251,Open Top,20,8.5,22U1,false"),
            new Types("2252,Open Top,20,8.5,22U1,false"),
            new Types("2253,Open Top,20,8.5,22U1,false"),
            new Types("2259,Open Top,20,8.5,22U1,false"),
            new Types("22U0,Open Top,20,8.5,22U1,false"),
            new Types("22UT,Open Top,20,8.5,22U1,false"),
            new Types("22UP,Hard Top,20,8.5,22UP,true"),
            new Types("22V0,Closed Vented,20,8.5,22VH,true"),
            new Types("22V2,Closed Vented,20,8.5,22VH,true"),
            new Types("22V4,Closed Vented,20,8.5,22VH,true"),
            new Types("22VH,Ventilated,20,8.5,22VH,true"),
            new Types("28VH,Vented,20,4.75,22VH,true"),
            new Types("28VO,Vented,20,4.75,22VH,true"),
            new Types("2010,Closed Vented,20,8,22VH,true"),
            new Types("2011,Closed Vented,20,8,22VH,true"),
            new Types("2013,Closed Ventilated,20,8,22VH,true"),
            new Types("2015,Closed Ventilated,20,8,22VH,true"),
            new Types("2017,Closed Ventilated,20,8,22VH,true"),
            new Types("20V0,Closed Vented,20,8,22VH,true"),
            new Types("20V2,Closed Vented,20,8,22VH,true"),
            new Types("20V4,Closed Vented,20,8,22VH,true"),
            new Types("20VH,Closed Vented,20,8,22VH,true"),
            new Types("2113,Closed Ventilated,20,8,22VH,true"),
            new Types("2117,Closed Ventilated,20,8,22VH,true"),
            new Types("2211,Closed Vented,20,8.5,22VH,true"),
            new Types("2215,Closed Ventilated,20,8.5,22VH,true"),
            new Types("2216,Closed Ventilated,20,8.5,22VH,true"),
            new Types("2217,Closed Ventilated,20,8.5,22VH,true"),
            new Types("29P0,Platform,20,1,29P0,false"),
            new Types("29P1,PLATFORM (CONTAINER),20,4,29P0,false"),
            new Types("2060,Platform,20,8,29P0,false"),
            new Types("2061,Platform,20,8,29P0,false"),
            new Types("2062,Platform,20,8,29P0,false"),
            new Types("2064,Platform,20,8,29P0,false"),
            new Types("2065,Platform,20,8,29P0,false"),
            new Types("20P0,Platform,20,8,29P0,false"),
            new Types("20P1,Platform,20,8,29P0,false"),
            new Types("22P0,Platform,20,8.5,29P0,false"),
            new Types("2760,Platform,20,4.25,29P0,false"),
            new Types("2960,Platform,20,4,29P0,false"),
            new Types("2969,Platform,20,4,29P0,false"),
            new Types("29PL,PLATFORM (CONTAINER),20,1,29P0,false"),
            new Types("42G0,Standard Dry,40,8.5,42G0,true"),
            new Types("42G1,Standard Dry,40,8.5,42G0,true"),
            new Types("42G2,Standard Dry,40,8.5,42G0,true"),
            new Types("42G3,Standard Dry,40,8.5,42G0,true"),
            new Types("4300,Standard Dry,40,8.5,42G0,true"),
            new Types("4301,Standard Dry,40,8.5,42G0,true"),
            new Types("4302,Standard Dry,40,8.5,42G0,true"),
            new Types("4303,Standard Dry,40,8.5,42G0,true"),
            new Types("4304,Standard Dry,40,8.5,42G0,true"),
            new Types("4305,Standard Dry,40,8.5,42G0,true"),
            new Types("4310,Standard Dry,40,8.5,42G0,true"),
            new Types("4311,Closed Vented,40,8.5,42G0,true"),
            new Types("4312,General Purpose (Hanging Garments),40,8.5,42G0,true"),
            new Types("4313,VENTILATED,40,0,42G0,true"),
            new Types("4315,Closed Ventilated,40,8.5,42G0,true"),
            new Types("4325,Livestock Carrier,40,8.5,42G0,false"),
            new Types("4326,Automobile Carrier,40,8.5,42G0,true"),
            new Types("4380,Dry Bulk,40,8.5,42G0,true"),
            new Types("44G0,Standard Dry,40,9,42G0,true"),
            new Types("44G1,Standard Dry,40,9,42G0,true"),
            new Types("44G2,Standard Dry,40,9,42G0,true"),
            new Types("44G3,Standard Dry,40,9,42G0,true"),
            new Types("44GP,Standard Dry,40,9,42G0,true"),
            new Types("4595,Air/Surface,40,8.5,42G0,true"),
            new Types("4599,Air/Surface,40,9,42G0,true"),
            new Types("4651,HALF HIGH,40,0,42G0,true"),
            new Types("4699,Air/Surface,40,4.25,42G0,true"),
            new Types("4886,Dry Bulk,40,4.25,42G0,true"),
            new Types("48UI,HALF HEIGHT (OPEN TOP),40,4.25,42G0,true"),
            new Types("4994,Air/Surface,40,8.5,42G0,true"),
            new Types("4999,GOOSENECK CHASSIS,40,0,42G0,true"),
            new Types("4CG0,OPENING(S) AT ONE OR BOTH ENDS,40,8.5,42G0,false"),
            new Types("4CGP,OPENING(S) AT ONE OR BOTH ENDS,40,8.5,42G0,false"),
            new Types("8500,Standard Dry,35,8.5,42G0,true"),
            new Types("8599,Air/Surface,35,8.5,42G0,true"),
            new Types("9995,Air/Surface,40,4,42G0,true"),
            new Types("9998,Air/Surface,40,4,42G0,true"),
            new Types("9999,Air/Surface,40,4,42G0,true"),
            new Types("M2G0,OPENING(S) AT ONE END OR BOTH ENDS,48,8.5,42G0,false"),
            new Types("P2G0,OPENING(S) AT ONE END OR BOTH ENDS,53,8.5,42G0,false"),
            new Types("4000,Standard Dry,40,8,42G0,true"),
            new Types("4001,Standard Dry,40,8,42G0,true"),
            new Types("4002,Standard Dry,40,8,42G0,true"),
            new Types("4003,Standard Dry,40,8,42G0,true"),
            new Types("4004,Standard Dry,40,8,42G0,true"),
            new Types("4020,Thermal Insulated,40,8,42G0,true"),
            new Types("4025,Livestock Carrier,40,8,42G0,false"),
            new Types("4026,Automobile Carrier,40,8,42G0,true"),
            new Types("4080,Dry Bulk,40,8,42G0,true"),
            new Types("4096,Air/Surface,40,8,42G0,true"),
            new Types("40B0,Dry Bulk,40,8,42G0,true"),
            new Types("40B1,Dry Bulk,40,8,42G0,true"),
            new Types("40B3,Dry Bulk,40,8,42G0,true"),
            new Types("40B4,Dry Bulk,40,8,42G0,true"),
            new Types("40B5,Dry Bulk,40,8,42G0,true"),
            new Types("40B6,Dry Bulk,40,8,42G0,true"),
            new Types("40BK,Dry Bulk,40,8,42G0,true"),
            new Types("40BU,Dry Bulk,40,8,42G0,true"),
            new Types("40G0,Standard Dry,40,8,42G0,true"),
            new Types("40G1,Standard Dry,40,8,42G0,true"),
            new Types("40G2,Standard Dry,40,8,42G0,true"),
            new Types("40G3,Standard Dry,40,8,42G0,true"),
            new Types("40GP,Standard Dry,40,8,42G0,true"),
            new Types("4101,Standard Dry,40,8,42G0,true"),
            new Types("4102,Standard Dry,40,8,42G0,true"),
            new Types("4103,Standard Dry,40,8,42G0,true"),
            new Types("4104,Standard Dry,40,8,42G0,true"),
            new Types("4126,Automobile Carrier,40,8,42G0,true"),
            new Types("4200,Standard Dry,40,8.5,42G0,true"),
            new Types("4201,Standard Dry,40,8.5,42G0,true"),
            new Types("4202,Standard Dry,40,8.5,42G0,true"),
            new Types("4203,Standard Dry,40,8.5,42G0,true"),
            new Types("4204,Standard Dry,40,8.5,42G0,true"),
            new Types("4225,Livestock Carrier,40,8.5,42G0,false"),
            new Types("4226,Automobile Carrier,40,8.5,42G0,true"),
            new Types("4280,Dry Bulk,40,8.5,42G0,true"),
            new Types("42B0,Dry Bulk,40,8.5,42G0,true"),
            new Types("42B1,Dry Bulk,40,8.5,42G0,true"),
            new Types("42B3,Dry Bulk,40,8.5,42G0,true"),
            new Types("42B4,Dry Bulk,40,8.5,42G0,true"),
            new Types("42B5,Dry Bulk,40,8.5,42G0,true"),
            new Types("42B6,Dry Bulk,40,8.5,42G0,true"),
            new Types("42BK,Dry Bulk,40,8.5,42G0,true"),
            new Types("42BU,Dry Bulk,40,8.5,42G0,true"),
            new Types("42GP,Standard Dry,40,8.5,42G0,true"),
            new Types("42G4,General Purose (Hanging Garments),40,8.5,42G0,true"),
            new Types("4CG1,PASSIVE VENTS AT UPPER PART OF CARGO SPACE,40,8.5,42G0,true"),
            new Types("42H5,Thermal Insulated,40,8.5,42H0,true"),
            new Types("42H6,Thermal Insulated,40,8.5,42H0,true"),
            new Types("44H5,Thermal Insulated,40,9,42H0,true"),
            new Types("44H6,Thermal Insulated,40,9,42H0,true"),
            new Types("45H5,Thermal Insulated,45,9.5,42H0,true"),
            new Types("45H6,Thermal Insulated,45,9.5,42H0,true"),
            new Types("L2H5,Thermal Insulated,45,8.5,42H0,true"),
            new Types("L2H6,Thermal Insulated,45,8.5,42H0,true"),
            new Types("L5H5,Thermal Insulated,45,9.5,42H0,true"),
            new Types("L5H6,Thermal Insulated,45,9.5,42H0,true"),
            new Types("42H0,Insulated (Conair),40,8.5,42H0,true"),
            new Types("42HI,Thermal Refrigerated/Heated,40,8.5,42H0,true"),
            new Types("42P1,Flat Rack,40,8.5,42P1,false"),
            new Types("42P2,Platform,40,8.5,42P1,false"),
            new Types("42P4,Platform,40,8.5,42P1,false"),
            new Types("42P5,Platform,40,8.5,42P1,false"),
            new Types("42P8,Platform,40,8.5,42P1,false"),
            new Types("42P9,Platform,40,8.5,42P1,false"),
            new Types("42PL,Platform,40,8.5,42P1,false"),
            new Types("42PS,Platform,40,8.5,42P1,false"),
            new Types("4361,Flat,40,8.5,42P1,false"),
            new Types("4362,Platform,40,8.5,42P1,false"),
            new Types("4364,Platform,40,8.5,42P1,false"),
            new Types("4365,Platform,40,8.5,42P1,false"),
            new Types("4366,Platform,40,8.5,42P1,false"),
            new Types("4367,Platform,40,8.5,42P1,false"),
            new Types("4560,Platform,40,8.5,42P1,false"),
            new Types("4561,Platform,40,8.5,42P1,false"),
            new Types("4661,Platform,40,4.25,42P1,false"),
            new Types("4761,Platform,40,4.25,42P1,false"),
            new Types("48P1,Platform,40,4.25,42P1,false"),
            new Types("48P5,Platform,40,4.25,42P1,false"),
            new Types("48PC,Platform,40,4.25,42P1,false"),
            new Types("48PF,Platform,40,4.25,42P1,false"),
            new Types("48PL,Platform,40,4.25,42P1,false"),
            new Types("4061,Platform,40,8,42P1,false"),
            new Types("4062,Platform,40,8,42P1,false"),
            new Types("4063,Platform,40,8,42P1,false"),
            new Types("4064,Platform,40,8,42P1,false"),
            new Types("4065,Platform,40,8,42P1,false"),
            new Types("4066,Platform,40,8,42P1,false"),
            new Types("4067,Platform,40,8,42P1,false"),
            new Types("40P1,Platform,40,8,42P1,false"),
            new Types("40P2,Platform,40,8,42P1,false"),
            new Types("40P4,Platform,40,8,42P1,false"),
            new Types("40P5,Platform,40,8,42P1,false"),
            new Types("40PC,Platform,40,8,42P1,false"),
            new Types("40PF,Platform,40,8,42P1,false"),
            new Types("40PL,Platform,40,8,42P1,false"),
            new Types("40PS,Platform,40,8,42P1,false"),
            new Types("4161,Platform,40,8,42P1,false"),
            new Types("4162,Platform,40,8,42P1,false"),
            new Types("4163,Platform,40,8,42P1,false"),
            new Types("4164,Platform,40,8,42P1,false"),
            new Types("4165,Platform,40,8,42P1,false"),
            new Types("4166,Platform,40,8,42P1,false"),
            new Types("4167,Platform,40,8,42P1,false"),
            new Types("4261,Platform,40,8.5,42P1,false"),
            new Types("4262,Platform,40,8.5,42P1,false"),
            new Types("4263,Flat,40,8.5,42P1,false"),
            new Types("4264,Platform,40,8.5,42P1,false"),
            new Types("4265,Platform,40,8.5,42P1,false"),
            new Types("4266,Platform,40,8.5,42P1,false"),
            new Types("4267,Platform,40,8.5,42P1,false"),
            new Types("42PC,Platform,40,8.5,42P1,false"),
            new Types("42PF,Platform,40,8.5,42P1,false"),
            new Types("42P3,Collapsible Flat Rack,40,8.5,42P3,false"),
            new Types("4363,Flat,40,8.5,42P3,false"),
            new Types("48P3,Platform,40,4.25,42P3,false"),
            new Types("40P3,Platform,40,8,42P3,false"),
            new Types("42R1,Reefer,40,8.5,42R1,true"),
            new Types("42R2,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("42R3,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("42R9,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("42RC,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("42RE,Thermal Refrigerated,40,8.5,42R1,true"),
            new Types("42RS,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("4320,Thermal Insulated,40,8.5,42R1,true"),
            new Types("4330,Thermal Refrigerated,40,8.5,42R1,true"),
            new Types("4332,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("4333,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("4340,Thermal Refrigerated,40,8.5,42R1,true"),
            new Types("44H0,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("44H1,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("44H2,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("44HI,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("44HR,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("44R0,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("44R1,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("44R2,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("44R3,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("44RE,Thermal Refrigerated,40,9,42R1,true"),
            new Types("44RS,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("44RT,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("8520,Thermal Insulated,35,8.5,42R1,true"),
            new Types("8532,Thermal Refrigerated/Heated,35,8.5,42R1,true"),
            new Types("4030,Thermal Refrigerated,40,8,42R1,true"),
            new Types("4031,Thermal Refrigerated,40,8,42R1,true"),
            new Types("4032,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("4040,Thermal Refrigerated,40,8,42R1,true"),
            new Types("40H0,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("40H1,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("40H2,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("40H5,Thermal Insulated,40,8,42R1,true"),
            new Types("40H6,Thermal Insulated,40,8,42R1,true"),
            new Types("40HI,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("40HR,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("40R0,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("40R1,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("40R2,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("40R3,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("40RE,Thermal Refrigerated,40,8,42R1,true"),
            new Types("40RS,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("40RT,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("4130,Thermal Refrigerated,40,8,42R1,true"),
            new Types("4131,Thermal Refrigerated,40,8,42R1,true"),
            new Types("4132,Thermal Refrigerated/Heated,40,8,42R1,true"),
            new Types("4224,Insulated,40,8.5,42R1,true"),
            new Types("4230,Thermal Refrigerated,40,8.5,42R1,true"),
            new Types("4231,Thermal Refrigerated,40,8.5,42R1,true"),
            new Types("4232,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("4240,Thermal Refrigerated,40,8.5,42R1,true"),
            new Types("4243,Thermal Refrigerated,40,8.5,42R1,true"),
            new Types("42H1,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("42H2,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("42HR,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("42R0,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("4432,Thermal Refrigerated/Heated,40,9,42R1,true"),
            new Types("42RT,Thermal Refrigerated/Heated,40,8.5,42R1,true"),
            new Types("42T0,Tank,40,8.5,42T0,true"),
            new Types("42T1,Tank,40,8.5,42T0,true"),
            new Types("42T2,Tank,40,8.5,42T0,true"),
            new Types("42T3,Tank,40,8.5,42T0,true"),
            new Types("42T4,Tank,40,8.5,42T0,true"),
            new Types("42T5,Tank,40,8.5,42T0,true"),
            new Types("42T6,Tank,40,8.5,42T0,true"),
            new Types("42T7,Tank,40,8.5,42T0,true"),
            new Types("42T8,Tank,40,8.5,42T0,true"),
            new Types("42T9,Tank,40,8.5,42T0,true"),
            new Types("42TD,Tank,40,8.5,42T0,true"),
            new Types("42TG,Tank,40,8.5,42T0,true"),
            new Types("4370,Tank,40,8.5,42T0,true"),
            new Types("8770,Tank,35,4.25,42T0,true"),
            new Types("4070,Tank,40,8,42T0,true"),
            new Types("4071,Tank,40,8,42T0,true"),
            new Types("40T0,Tank,40,8,42T0,true"),
            new Types("40T1,Tank,40,8,42T0,true"),
            new Types("40T2,Tank,40,8,42T0,true"),
            new Types("40T3,Tank,40,8,42T0,true"),
            new Types("40T4,Tank,40,8,42T0,true"),
            new Types("40T5,Tank,40,8,42T0,true"),
            new Types("40T6,Tank,40,8,42T0,true"),
            new Types("40T7,Tank,40,8,42T0,true"),
            new Types("40T8,Tank,40,8,42T0,true"),
            new Types("40T9,Tank,40,8,42T0,true"),
            new Types("40TD,Tank,40,8,42T0,true"),
            new Types("40TG,Tank,40,8,42T0,true"),
            new Types("40TN,Tank,40,8,42T0,true"),
            new Types("4170,Tank,40,8,42T0,true"),
            new Types("4270,Tank,40,8.5,42T0,true"),
            new Types("4271,Tank,40,8.5,42T0,true"),
            new Types("42TN,Tank,40,8.5,42T0,true"),
            new Types("42U1,Open Top,40,8.5,42U1,false"),
            new Types("42U2,Open Top,40,8.5,42U1,false"),
            new Types("42U3,Open Top,40,8.5,42U1,false"),
            new Types("42U4,Open Top,40,8.5,42U1,false"),
            new Types("42U5,Open Top,40,8.5,42U1,false"),
            new Types("42U6,Standard Dry,40,8.5,42U1,true"),
            new Types("4350,Open Top,40,8.5,42U1,false"),
            new Types("4351,Open Top,40,8.5,42U1,false"),
            new Types("4650,Open Top,40,4.25,42U1,false"),
            new Types("4750,Open Top,40,4.25,42U1,false"),
            new Types("4751,Open Top,40,4.25,42U1,false"),
            new Types("48U0,Open top,40,4.25,42U1,false"),
            new Types("48UT,Open top,40,4.25,42U1,false"),
            new Types("8550,Open top,35,8.5,42U1,false"),
            new Types("4050,Open Top,40,8,42U1,false"),
            new Types("4051,Open Top,40,8,42U1,false"),
            new Types("4052,Open Top,40,8,42U1,false"),
            new Types("4053,Open Top,40,8,42U1,false"),
            new Types("40U0,Open Top,40,8,42U1,false"),
            new Types("40U1,Open Top,40,8,42U1,false"),
            new Types("40U2,Open Top,40,8,42U1,false"),
            new Types("40U3,Open Top,40,8,42U1,false"),
            new Types("40U4,Open Top,40,8,42U1,false"),
            new Types("40U5,Open Top,40,8,42U1,false"),
            new Types("40UT,Open Top,40,8,42U1,false"),
            new Types("4250,Open Top,40,8.5,42U1,false"),
            new Types("4251,Open Top,40,8.5,42U1,false"),
            new Types("4252,Open Top,40,8.5,42U1,false"),
            new Types("4253,Open Top,40,8.5,42U1,false"),
            new Types("42P6,Open Top,40,8.5,42U1,false"),
            new Types("42U0,Open Top,40,8.5,42U1,false"),
            new Types("42UT,Open Top,40,8.5,42U1,false"),
            new Types("4551,OPEN TOP HIGHCUBE,40,9.5,42U1,false"),
            new Types("42UP,Hard Top,40,8.5,42UP,true"),
            new Types("42V0,Closed Vented,40,8.5,42VH,true"),
            new Types("42V2,Closed Vented,40,8.5,42VH,true"),
            new Types("42V4,Closed Vented,40,8.5,42VH,true"),
            new Types("42VH,Ventilated,40,8.5,42VH,true"),
            new Types("4010,Closed Vented,40,8,42VH,true"),
            new Types("4011,Closed Vented,40,8,42VH,true"),
            new Types("4015,Closed Ventilated,40,8,42VH,true"),
            new Types("40V0,Closed Vented,40,8,42VH,true"),
            new Types("40V2,Closed Vented,40,8,42VH,true"),
            new Types("40V4,Closed Vented,40,8,42VH,true"),
            new Types("40VH,Closed Vented,40,8,42VH,true"),
            new Types("4210,Closed Vented,40,8.5,42VH,true"),
            new Types("4211,Closed Vented,40,8.5,42VH,true"),
            new Types("4215,Closed Ventilated,40,8.5,42VH,true"),
            new Types("45G0,High Cube,40,9.5,45G0,true"),
            new Types("45G1,High Cube,40,9.5,45G0,true"),
            new Types("45G2,Standard Dry,40,9.5,45G0,true"),
            new Types("45G3,Standard Dry,40,9.5,45G0,true"),
            new Types("45G4,Unrecognized container type,0,0,45G0,true"),
            new Types("9200,Standard Dry,45,8.5,45G0,true"),
            new Types("9400,Standard Dry,45,9.5,45G0,true"),
            new Types("4400,Standard Dry,40,9,45G0,true"),
            new Types("4410,HIGH CUBE,40,9.5,45G0,true"),
            new Types("4420,Thermal Insulated,40,9,45G0,true"),
            new Types("4426,Automobile Carrier,40,9,45G0,true"),
            new Types("4500,Standard Dry,40,8.5,45G0,true"),
            new Types("4505,Standard Dry,40,8.5,45G0,true"),
            new Types("4510,Standard Dry,40,9.5,45G0,true"),
            new Types("4511,Standard Dry,40,9.5,45G0,true"),
            new Types("4514,HIGH CUBE,40,9.5,45G0,true"),
            new Types("45GP,Standard Dry,40,9.5,45G0,true"),
            new Types("45R0,Thermal Refrigerated/Heated,45,9.5,45R1,true"),
            new Types("45R1,Reefer High Cube,40,9.5,45R1,true"),
            new Types("45R2,Thermal Refrigerated/Heated,45,9.5,45R1,true"),
            new Types("45R3,Thermal Refrigerated/Heated,45,9.5,45R1,true"),
            new Types("45R9,Thermal Refrigerated,40,9.5,45R1,true"),
            new Types("45RC,Thermal Refrigerated/Heated,40,9.5,45R1,true"),
            new Types("45RE,Thermal Refrigerated,45,9.5,45R1,true"),
            new Types("45RS,Thermal Refrigerated/Heated,45,9.5,45R1,true"),
            new Types("4530,Thermal Refrigerated,40,8.5,45R1,true"),
            new Types("4531,Thermal Refrigerated,40,8.5,45R1,true"),
            new Types("4532,Thermal Refrigerated/Heated,40,8.5,45R1,true"),
            new Types("4533,Thermal Refrigerated/Heated,40,8.5,45R1,true"),
            new Types("45H2,Thermal Refrigerated/Heated,45,9.5,45R1,true"),
            new Types("45RT,Thermal Refrigerated/Heated,45,9.5,45R1,true"),
            new Types("4534,HIGHCUBE INTEGRATED REEFER,40,9.5,45R1,true"),
            new Types("45U6,High Cube Hard Top,40,9.5,45UP,true"),
            new Types("45UP,High Cube Hard Top,40,9.5,45UP,true"),
            new Types("49P0,Platform,40,4,49P0,false"),
            new Types("49P1,Platform,40,4,49P0,false"),
            new Types("49P3,Platform,40,4,49P0,false"),
            new Types("49P5,Platform,40,4,49P0,false"),
            new Types("49PC,Platform,40,4,49P0,false"),
            new Types("49PF,Platform,40,4,49P0,false"),
            new Types("4060,Flat,40,8,49P0,false"),
            new Types("40P0,Platform,40,8,49P0,false"),
            new Types("4260,Flat,40,8.5,49P0,false"),
            new Types("42P0,Platform,40,8.5,49P0,false"),
            new Types("4360,Flat,40,8.5,49P0,false"),
            new Types("45P3,FOLDING COMPLETE END STRUCTURE (PLATFORM),40,9.5,49P0,false"),
            new Types("45P8,Platform,40,9.5,49P0,false"),
            new Types("45PC,FOLDING COMPLETE END STRUCTURE (PLATFORM),40,9.5,49P0,false"),
            new Types("48P0,Platform,40,4.25,49P0,false"),
            new Types("4960,Platform,40,4,49P0,false"),
            new Types("49PL,Platform,40,4,49P0,false"),
            new Types("9500,Standard Dry,45,9.5,L5G1,true"),
            new Types("9510,Standard Dry,45,9.5,L5G1,true"),
            new Types("L5G1,45 High Cube,45,9,L5G1,true"),
            new Types("L5G2,Standard Dry,45,9.5,L5G1,true"),
            new Types("L5G3,Standard Dry,45,9.5,L5G1,true"),
            new Types("L5G9,Standard Dry,45,9.5,L5G1,true"),
            new Types("L0G9,Standard Dry,45,8,L5G1,true"),
            new Types("L0GP,HL: OPENING(S) AT ONE END OR BOTH ENDS,45,8,L5G1,false"),
            new Types("L2G0,Standard Dry,45,8.5,L5G1,true"),
            new Types("L2G1,Standard Dry,45,8.5,L5G1,true"),
            new Types("L2G2,Standard Dry,45,8.5,L5G1,true"),
            new Types("L2G3,Standard Dry,45,8.5,L5G1,true"),
            new Types("L2G9,Standard Dry,45,8.5,L5G1,true"),
            new Types("L2GP,Standard Dry,45,8.5,L5G1,true"),
            new Types("L5G0,Standard Dry,45,9,L5G1,true"),
            new Types("L5GP,Standard Dry,45,9.5,L5G1,true"),
            new Types("L5R1,45 Reefer High Cube,45,9.5,L5R1,true"),
            new Types("L5R2,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("L5R3,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("L5RE,Thermal Refrigerated,45,9.5,L5R1,true"),
            new Types("L5RS,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("45H0,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("45H1,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("45HI,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("45HR,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("9532,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("L2H0,Thermal Refrigerated/Heated,45,8.5,L5R1,true"),
            new Types("L2H1,Thermal Refrigerated/Heated,45,8.5,L5R1,true"),
            new Types("L2H2,Thermal Refrigerated/Heated,45,8.5,L5R1,true"),
            new Types("L2HI,Thermal Refrigerated/Heated,45,8.5,L5R1,true"),
            new Types("L2HR,Thermal Refrigerated/Heated,45,8.5,L5R1,true"),
            new Types("L2R0,Thermal Refrigerated,45,8.5,L5R1,true"),
            new Types("L2R1,Thermal Refrigerated/Heated,45,8.5,L5R1,true"),
            new Types("L2R2,Thermal Refrigerated/Heated,45,8.5,L5R1,true"),
            new Types("L2R3,Thermal Refrigerated/Heated,45,8.5,L5R1,true"),
            new Types("L2RE,Thermal Refrigerated,45,8.5,L5R1,true"),
            new Types("L2RS,Thermal Refrigerated/Heated,45,8.5,L5R1,true"),
            new Types("L2RT,Thermal Refrigerated/Heated,45,8.5,L5R1,true"),
            new Types("L5H0,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("L5H1,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("L5H2,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("L5HI,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("L5HR,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("L5R0,Thermal Refrigerated,45,9.5,L5R1,true"),
            new Types("L5RT,Thermal Refrigerated/Heated,45,9.5,L5R1,true"),
            new Types("12TR,Flatbed,42,8,12TR,false")
        };
#endregion

        public class Types
        {
            public string code;
            public string description;
            public int length;
            public double height;
            public string group;
            public bool closed;

            public Types (string _code, string _descr, int _length, double _height, string _group, bool _closed)
            {
                code = _code;
                description = _descr;
                length = _length;
                height = _height;
                group = _group;
                closed = _closed;
            }

            public Types (string line)
            {
                string[] temp = line.Split(',');
                code = temp[0];
                description = temp[1];
                length = Convert.ToInt16( temp[2]);

                height = Double.Parse(temp[3].Replace('.',','));
                group = temp[4];
                closed = Convert.ToBoolean(temp[5]);
            }
        }


        #region Special Stowage codes
        public static Dictionary<string, string> stowage = new Dictionary<string, string>()
        {
            { "H1", "Keep as dry as reasonably practicable." },
            { "H2", "Keep as cool as reasonably practicable." },
            { "H3", "During transport, it should be stowed (or kept) in a cool ventilated place." },
            { "H4", @"If cleaning of cargo spaces has to be carried out at sea, the safety procedures 
                    followed and standard of equipment used shall be at least as effective as those em
                    ployed as industry best practice in a port. Until such cleaning is undertaken, the 
                    cargo spaces in which the asbestos has been carried shall be closed and access to 
                    those spaces shall be prohibited."},
            { "SW1", "Protected from sources of heat." },
            { "SW2", "Clear of living quarters."},
            {"SW3", "Shall be transported under temperature control."},
            {"SW4", "Surface ventilation is required to assist in removing any residual solvent vapour."},
            {"SW5", "If under deck, stow in a mechanically ventilated space."},
            {"SW6", @"When stowed under deck, mechanical ventilation shall be in accordance with SOLAS
                    regulation II-2/19 (II-2/54) for flammable liquids with flashpoint below 23°C c.c."},
            {"SW7", "As approved by the competent authorities of the countries involved in the shipment."},
            {"SW8", @"Ventilation may be required. The possible need to open hatches in case of fire to provide
                    maximum ventilation and to apply water in an emergency, and the consequent risk to the
                    stability of the ship through flooding of the cargo spaces, shall be considered before loading."},
            {"SW9", @"Provide a good through ventilation for bagged cargo. Double strip stowage is recommended.
                    The illustration in 7.6.2.7.2.3 shows how this can be achieved. During the voyage regular
                    temperature readings shall be taken at varying depths in the hold and recorded. If the
                    temperature of the cargo exceeds the ambient temperature and continues to increase,
                    ventilation shall be closed down."},
            {"SW10", @"Unless carried in closed cargo transport units, bales shall be properly covered by tarpaulins
                    or the like. Cargo spaces shall be clean, dry and free from oil or grease. Ventilator cowls
                    leading into the cargo space shall have sparking-preventing screens. All other openings,
                    entrances and hatches leading to the cargo space shall be securely closed. During temporary
                    interruption of loading, when the hatch remains uncovered, a fire-watch shall be kept. During
                    loading or discharge, smoking in the vicinity"},
            {"SW11", @"Cargo transport units shall be shaded from direct sunlight. Packages in cargo transport units
                    shall be stowed so as to allow for adequate air circulation throughout the cargo."},
            {"SW12", "Taking account of any supplementary requirements specified in the transport documents."},
            {"SW13", @"Taking account of any supplementary requirements specified in the competent authority
                    approval certificate(s)."},
            {"SW14", "Category A only if the special stowage provisions of 7.4.1.4 and 7.6.2.8.4 are complied with."},
            {"SW15", "For metal drums, stowage category B."},
            {"SW16", "For unit loads in open cargo transport units, stowage category B."},
            {"SW17", @"Category E, for closed cargo transport unit and pallet boxes only. Ventilation may be
                    required. The possible need to open hatches in case of fire to provide maximum ventilation
                    and to apply water in an emergency, and the consequent risk to the stability of the ship
                    through flooding of the cargo space, shall be considered before loading."},
            {"SW18", "Category A, when transported in accordance with P650."},
            {"SW19", @"For batteries transported in accordance with special provisions 376 or 377, category C,
                    unless transported on a short international voyage."},
            {"SW20", "For uranyl nitrate hexahydrate solution stowage, category D applies."},
            {"SW21", "For uranium metal pyrophoric and thorium metal pyrophoric stowage, category D applies."},
            {"SW22", @"For AEROSOLS with a maximum capacity of 1 L: category A.
                       For AEROSOLS with a capacity above 1 L: category B.
                       For WASTE AEROSOLS: category C, clear of living quarters."},
            {"SW23", "When transported in BK3 bulk container, see 7.6.2.12 and 7.7.3.9."},
            {"SW24", "For special stowage provisions, see 7.4.1.3 and 7.6.2.7.2."},
            {"SW25", "For special stowage provisions, see 7.6.2.7.3."},
            {"SW26", "For special stowage provisions, see 7.4.1.4 and 7.6.2.11.1.1."},
            {"SW27", "For special stowage provisions, see 7.6.2.7.2.1."},
            {"SW28", "As approved by the competent authority of the country of origin."},
            {"SW29", "For engines or machinery containing fuels with flashpoint equal or greater than 23°C, stowage Category A."}
        };
        #endregion

        #region Special Segregation codes
        public static Dictionary<string, string> segregation = new Dictionary<string, string>()
        {
            {"SG1", @"For packages carrying a subsidiary risk label of class 1, segregation as for class 1, division 1.3."},
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
            {"SG20", "Stow “away from” acids."},
            {"SG21", "Stow “away from” alkalis."},
            {"SG22", "Stow “away from” ammonium salts."},
            {"SG23", "Stow “away from” animal or vegetable oils."},
            {"SG24", "Stow “away from” azides."},
            {"SG25", "Stow “separated from” goods of classes 2.1 and 3."},
            {"SG26", @"In addition: from goods of classes 2.1 and 3 when stowed on deck of a containership a
                    minimum distance of two container spaces athwartship shall be maintained, when stowed 
                    on ro-ro ships a distance of 6 m athwartship shall be maintained."},
            {"SG27", "Stow “away from” explosives containing chlorates or perchlorates."},
            {"SG28", @"Stow “away from” ammonium compounds and explosives containing ammonium compounds or salts."},
            {"SG29", "Segregation from foodstuffs as in 7.3.4.2.2, 7.6.3.1.2 or 7.7.3.7."},
            {"SG30", "Stow “away from” heavy metals and their salts."},
            {"SG31", "Stow “away from” lead and its compounds."},
            {"SG32", "Stow “away from” liquid halogenated hydrocarbons."},
            {"SG33", "Stow “away from” powdered metals."},
            {"SG34", @"When containing ammonium compounds, “away from” chlorates or perchlorates and 
                    explosives containing chlorates or perchlorates."},
            {"SG35", "Stow “separated from” acids."},
            {"SG36", "Stow “separated from” alkalis."},
            {"SG37", "Stow “separated from” ammonia."},
            {"SG38", "Stow “separated from” ammonium compounds."},
            {"SG39", "Stow “separated from” ammonium compounds other than AMMONIUM PERSULPHATE (UN 1444)."},
            {"SG40", @"Stow “separated from” ammonium compounds other than mixtures of ammonium 
                    persulphates and/or potassium persulphates and/or sodium persulphates."},
            {"SG41", "Stow “separated from” animal or vegetable oil."},
            {"SG42", "Stow “separated from” bromates."},
            {"SG43", "Stow “separated from” bromine."},
            {"SG44", "Stow “separated from” CARBON TETRACHLORIDE (UN 1846)."},
            {"SG45", "Stow “separated from” chlorates."},
            {"SG46", "Stow “separated from” chlorine."},
            {"SG47", "Stow “separated from” chlorites."},
            {"SG48", @"Stow “separated from” combustible material(particularly liquids). Combustible material
                     does not include packing materials or dunnage."},
            {"SG49", "Stow “separated from” cyanides."},
            {"SG50", "Segregation from foodstuffs as in 7.3.4.2.1, 7.6.3.1.2 or 7.7.3.6."},
            {"SG51", "Stow “separated from” hypochlorites."},
            {"SG52", "Stow “separated from” iron oxide."},
            {"SG53", "Stow “separated from” liquid organic substances."},
            {"SG54", "Stow “separated from” mercury and mercury compounds."},
            {"SG55", "Stow “separated from” mercury salts."},
            {"SG56", "Stow “separated from” nitrites."},
            {"SG57", "Stow “separated from” odour-absorbing cargoes."},
            {"SG58", "Stow “separated from” perchlorates."},
            {"SG59", "Stow “separated from” permanganates."},
            {"SG60", "Stow “separated from” peroxides."},
            {"SG61", "Stow “separated from” powdered metals."},
            {"SG62", "Stow “separated from” sulphur."},
            {"SG63", "Stow “separated longitudinally by an intervening complete compartment or hold from” class 1."},
            {"SG64", "[Reserved]"},
            {"SG65", "Stow “separated by a complete compartment or hold from” class 1 except for division 1.4."},
            {"SG66", "[Reserved]"},
            {"SG67", @"Stow “separated from” division 1.4 and “separated longitudinally by an intervening
                    complete compartment of hold from” divisions 1.1, 1.2, 1.3, 1.5 and 1.6 except from
                    explosives of compatibility group J."},
            {"SG68", "If flashpoint 60°C c.c.or below, segregation as for class 3 but “away from” class 4.1."},
            {"SG69", @"For AEROSOLS with a maximum capacity of 1 L: segregation as for class 9.
                       Stow “separated from” class 1 except for division 1.4.
                       For AEROSOLS with a capacity above 1 L: segregation as for the appropriate subdivision of class 2.
                       For WASTE AEROSOLS: segregation as for the appropriate subdivision of class 2."},
            {"SG70", "For arsenic sulphides, “separated from” acids."},
            {"SG71", @"Within the appliance, to the extent that the dangerous goods are integral parts of the
                    complete life-saving appliance, there is no need to apply the provisions on segregation of
                    substances in chapter 7.2."},
            {"SG72", "See 7.2.6.3."},
            {"SG73", "[Reserved]"},
            {"SG 74", "Segregation as for 1.4G."},
            {"SG 75", "Stow “separated from” strong acids."}
        };
        #endregion
        
        #region Special provisions
        internal Dictionary<int, string> special = new Dictionary<int, string>()
        {
            {16, @"Samples of new or existing explosive substances or articles may be transported as directed by the competent authority for purposes including: testing, classification, research and development, quality control, or as a commercial sample. Explosive samples which are not wetted or desensitized shall be limited to 10 kg in small packages as specified by the competent authority. Explosive samples which are wetted or desensitized shall be limited to 25 kg."},
            {23, @"Even though this substance has a flammability hazard, it only exhibits such hazard under extreme fire conditions in confined areas."},
            {26, @"This substance is not permitted for transport in portable tanks, or intermediate bulk containers with a capacity exceeding 450 L, due to the potential initiation of an explosion when transported in large volumes."},
            {28, @"This substance may be transported under the provisions of class 4.1 only if it is so packaged that the percentage of diluent will not fall below that stated, at any time during transport(see 2.4.2.4)."},
            {29, @"The packages, including bales, are exempt from labelling provided that they are marked with the appropriate class (e.g. “class 4.2”). Packages, with the exception of bales, shall also display the proper shipping name and the UN number of the substance that they contain in accordance with 5.2.1. In any case, the packages, including bales, are exempt from class marking provided that they are loaded in a cargo transport unit and that they contain goods to which only one UN number has been assigned.The cargo transport units in which the packages, including bales, are loaded shall display any relevant labels, placards and marks in accordance with chapter 5.3."},
            {32, @"When in any other form, this substance is not subject to the provisions of this Code."},
            {37, @"When coated, this substance is not subject to the provisions of this Code."},
            {38, @"This substance, when it contains not more than 0.1% calcium carbide, is not subject to the provisions of this Code."},
            {39, @"This substance, when it contains less than 30% or not less than 90% silicon, is not subject to the provisions of this Code."},
            {43, @"When offered for transport as pesticides, these substances shall be transported under the relevant pesticide entry and in accordance with the relevant pesticide provisions (see 2.6.2.3 and 2.6.2.4)."},
            {45, @"Antimony sulphides and oxides which contain not more than 0.5% of arsenic, calculated on the total mass, are not subject to the provisions of this Code."},
            {47, @"Ferricyanides and ferrocyanides are not subject to the provisions of this Code."},
            {59, @"These substances, when they contain not more than 50% magnesium, are not subject to the provisions of this Code."},
            {61, @"The technical name, which shall supplement the proper shipping name, shall be the ISO common name, or other name listed in The WHO Recommended Classification of Pesticides by Hazard and Guidelines to Classification or the name of the active substance (see also 3.1.2.8.1.1)."},
            {62, @"This substance, when it contains not more than 4% sodium hydroxide, is not subject to the provisions of this Code."},
            {63, @"The division of class 2 and the subsidiary risks depend on the nature of the contents of the aerosol dispenser.The following provisions shall apply:
              .1 Class 2.1 applies if the contents include 85% by mass or more flammable components and the chemical heat of combustion is 30 kJ/g or more;
              .2 Class 2.2 applies if the contents contain 1% by mass or less flammable components and the heat of combustion is less than 20 kJ/g.
              .3 Otherwise the product shall be classified as tested by the tests described in the Manual of Tests and Criteria, part III, section 31. Extremely flammable and flammable aerosols shall be classified in class 2.1; non-flammable in class 2.2;
              .4 Gases of class 2.3 shall not be used as a propellant in an aerosol dispenser;
              .5 Where the contents other than the propellant of aerosol dispensers to be ejected are classified as class 6.1 packing groups II or III or class 8 packing groups II or III, the aerosol shall have a subsidiary risk of class 6.1 or class 8;
              .6 Aerosols with contents meeting the criteria for packing group I for toxicity or corrosivity shall be prohibited from transport;
              .7 Except for consignments transported in limited quantities(see chapter 3.4), packages containing aerosols shall bear labels for the primary risk and for the subsidiary risk(s), if any.
              Flammable components are flammable liquids, flammable solids or flammable gases and gas mixtures as defined in notes 1 to 3 of subsection 31.1.3 of part III of the Manual of Tests and Criteria.  This designation does not cover pyrophoric, self-heating or water-reactive substances. The chemical heat of combustion shall be determined by one of the following methods: ASTM D 240, ISO/FDIS 13943:1999 (E/F) 86.1 to 86.3 or NFPA 30B."},
            {65, @"Hydrogen peroxide aqueous solutions with less than 8% hydrogen peroxide are not subject to the  provisions of this Code."},
            {66, @"Cinnabar is not subject to the provisions of this Code."},
            {76, @"The transport of this substance shall be prohibited except with special authorization granted by the competent authority of the country concerned."},
            {105, @"Nitrocellulose meeting the descriptions of UN 2556 or UN 2557 may be classified in class 4.1."},
            {113, @"The transport of chemically unstable mixtures is prohibited."},
            {117, @"Only regulated when transported by sea."},
            {119, @"Refrigerating machines and refrigerating-machinery components including machines or other appliances which have been designed for the specific purpose of keeping food or other items at a low temperature in an internal compartment, and air-conditioning units.Refrigerating machines and refrigerating-machine components are not subject to the provisions of this Code if they contain less than 12 kg of gas in class 2.2 or less than 12 L of ammonia solution(UN 2672)."},
            {122, @"The subsidiary risk(s), the control and emergency temperatures, if any, and the generic entry number for each of the currently assigned organic peroxide formulations are given in 2.5.3.2.4, 4.1.4.2 packing instruction IBC520 and 4.2.5.2.6 portable tank instruction T23."},
            {127, @"Other inert material or inert material mixture may be used at the discretion of the competent authority, provided this inert material has identical phlegmatizing properties."},
            { 131, @"The phlegmatized substance shall be significantly less sensitive than dry PETN."},
            { 133, @"If over-confined in packagings, this substance may exhibit explosive behaviour.Packagings authorized under packing instruction P409 are intended to prevent over-confinement.When a packaging other than those prescribed under packing instruction P409 is authorized by the competent authority of the country of origin in accordance with 4.1.3.7, the package shall bear an “EXPLOSIVE” subsidiary risk label (Model No. 1, see 5.2.2.2.2) unless the competent authority of the country of origin has permitted this label to be dispensed with for the specific packaging employed because test data have proved that the substance in this packaging does not exhibit explosive behaviour (see 5.4.1.5.5.1). The provisions of 7.2.3.3, 7.1.3.1 and 7.1.4.4 shall also be considered."},
            {135, @"The dihydrated sodium salt of dichloroisocyanuric acid does not meet the criteria for inclusion in class 5.1 and is not subject to the provisions of this Code unless meeting the criteria for inclusion in another class or division."},
            {138, @"p-Bromobenzyl cyanide is not subject to the provisions of this Code."},
            {141, @"Products which have undergone sufficient heat treatment so that they present no hazard during transport are not subject to the provisions of this Code."},
            {142, @"Solvent-extracted soya bean meal containing not more than 1.5% oil and 11% moisture, being substantially free from flammable solvents, which is accompanied by a certificate from the shipper stating that the substance, as offered for shipment, meets this requirement is not subject to the provisions of this Code."},
            {144, @"An aqueous solution containing not more than 24% alcohol by volume is not subject to the provisions of this Code."},
            {145, @"Alcoholic beverages of packing group III, when transported in receptacles of 250 L or less, are not subject to the provisions of this Code."},
            {152, @"The classification of this substance will vary with particle size and packaging, but borderlines have 3
            not been experimentally determined. Appropriate classifications shall be made as required by 2.1.3."},
            {153, @"This entry applies only if it is demonstrated, on the basis of tests, that the substance, when in
            contact with water, is not combustible nor shows a tendency to auto-ignition and that the mixture of
            gases evolved is not flammable."},
            {163, @"A substance specifically listed by name in the Dangerous Goods List shall not be transported under
            this entry.Materials transported under this entry may contain 20% or less nitrocellulose provided
            the nitrocellulose contains not more than 12.6% nitrogen (by dry mass)."},
            { 168, @"Asbestos which is immersed or fixed in a natural or artificial binder(such as cement, plastics, asphalt,
            resins or mineral ore) in such a way that no escape of hazardous quantities of respirable asbestos
            fibres can occur during transport is not subject to the provisions of this Code.Manufactured articles
            containing asbestos and not meeting this provision are nevertheless not subject to the provisions of
            this Code when packaged so that no escape of hazardous quantities of respirable asbestos fibres
            can occur during transport."},
            {
            169, @"Phthalic anhydride in the solid state and tetrahydrophthalic anhydride, with not more than 0.05 %
            maleic anhydride, are not subject to the provisions of this Code.Phthalic anhydride molten at a
            temperature above its flashpoint, with not more than 0.05 % maleic anhydride, shall be classified
            under UN 3256."},
            {
            172, @"Where a radioactive material has(a) subsidiary risk(s):
            .1 The substance shall be allocated to packing group I, II or III, if appropriate, by application of
            the packing group criteria provided in part 2 corresponding to the nature of the predominant
            subsidiary risk;
            .2 Packages shall be labelled with subsidiary risk labels corresponding to each subsidiary risk
            exhibited by the material; corresponding placards shall be affixed to cargo transport units in
            accordance with the relevant provisions of 5.3.1;
            .3 For the purposes of documentation and package marking, the proper shipping name shall be
            supplemented with the name of the constituents which most predominantly contribute to this
            (these) subsidiary risk(s) and which shall be enclosed in parenthesis;
            .4 The dangerous goods transport document shall indicate the subsidiary class or division and,
            where assigned, the packing group as required by 5.4.1.4.1.4 and 5.4.1.4.1.5.
            For packing, see also 4.1.9.1.5."},
            {177, @"Barium sulphate is not subject to the provisions of this Code."},
            {178, @"This entry shall be used only when no other appropriate entry exists in the list, and only with the
            approval of the competent authority of the country of origin."},
            {181, @"Packages containing this type of substance shall bear the “EXPLOSIVE” subsidiary risk label(Model
            No. 1, see 5.2.2.2.2) unless the competent authority of the country of origin has permitted this label
            to be dispensed with for the specific packaging employed because test data have proved that the
            substance in this packaging does not exhibit explosive behaviour (see 5.4.1.5.5.1). The provisions of
            7.2.3.3 shall also be considered."},
            { 182, @"The group of alkali metals includes lithium, sodium, potassium, rubidium and caesium."},
            { 183, @"The group of alkaline earth metals includes magnesium, calcium, strontium and barium."},
            { 186, @"In determining the ammonium nitrate content, all nitrate ions for which a molecular equivalent of
            ammonium ions is present in the mixture shall be calculated as ammonium nitrate."},
            {188, @"Cells and batteries offered for transport are not subject to other provisions of this Code if they meet
            the following:
            .1 For a lithium metal or lithium alloy cell, the lithium content is not more than 1 g, and for a lithiumion
            cell, the watt-hour rating is not more than 20 Wh;
            .2 For a lithium metal or lithium alloy battery, the aggregate lithium content is not more than 2 g,
            and for a lithium-ion battery, the watt-hour rating is not more than 100 Wh. Lithium-ion batteries
            subject to this provision shall be marked with the watt-hour rating on the outside case, except
            those manufactured before 1 January 2009;
            .3 Each cell or battery meets the provisions of 2.9.4.1 and 2.9.4.5;
            .4 Cells and batteries, except when installed in equipment, shall be packed in inner packagings
            that completely enclose the cell or battery. Cells and batteries shall be protected so as to
            prevent short circuits.This includes protection against contact with conductive materials within
            the same packaging that could lead to a short circuit. The inner packagings shall be packed in
            strong outer packagings which conform to the provisions of 4.1.1.1, 4.1.1.2, and 4.1.1.5.
            .5 Cells and batteries when installed in equipment shall be protected from damage and short
            circuit, and the equipment shall be equipped with an effective means of preventing accidental
            activation.This requirement does not apply to devices which are intentionally active in transport
            (radio frequency identification (RFID) transmitters, watches, sensors, etc.) and which are not
            capable of generating a dangerous evolution of heat.When batteries are installed in equipment,
            the equipment shall be packed in strong outer packagings constructed of suitable material
            of adequate strength and design in relation to the packaging’s capacity and its intended use
            unless the battery is afforded equivalent protection by the equipment in which it is contained.
            .6 Each package shall be marked with the appropriate lithium battery mark, as illustrated in
            5.2.1.10;
            Note: The provisions concerning marking in special provision 188 of amendment 37-14 of the
            Code may continue to be applied until 31 December 2018.
            This requirement does not apply to:
            .1 packages containing only button cell batteries installed in equipment(including circuit
            boards); and
            .2 packages containing no more than four cells or two batteries installed in equipment, where
            there are not more than two packages in the consignment.
            .7 Except when batteries are installed in equipment, each package shall be capable of withstanding
            a 1.2 m drop test in any orientation without damage to cells or batteries contained therein,
            without shifting of the contents so as to allow battery to battery (or cell to cell) contact and
            without release of contents; and
            .8 Except when batteries are installed in or packed with equipment, packages shall not exceed
            30 kg gross mass.
            As used above and elsewhere in this Code, “lithium content” means the mass of lithium in the anode
            of a lithium metal or lithium alloy cell.
            Separate entries exist for lithium metal batteries and lithium ion batteries to facilitate the transport of
            these batteries for specific modes of transport and to enable the application of different emergency
            response actions.
            A single cell battery as defined in part III, subsection 38.3.2.3 of the Manual of Tests and Criteria
            is considered a “cell” and shall be transported according to the requirements for “cells” for the
            purpose of this special provision."},
            { 190, @"Aerosol dispensers shall be provided with protection against inadvertent discharge. Aerosols
            with a capacity not exceeding 50 mL containing only non-toxic constituents are not subject to the
            provisions of this Code."},
            { 191, @"Receptacles with a capacity not exceeding 50 mL containing only non-toxic constituents are not
            subject to the provisions of this Code."},
            { 193, @"This entry may only be used for uniform ammonium nitrate based fertilizer mixtures of the nitrogen,
            phosphate or potash type, containing not more than 70% ammonium nitrate and not more than
            0.4% total combustible/organic material calculated as carbon or with not more than 45% ammonium
            nitrate and unrestricted combustible material. Fertilizers within these composition limits are not
            subject to the provisions of this Code when shown by a Trough Test (see Manual of Tests and
            Criteria, part III, subsection 38.2) that they are not liable to self-sustaining decomposition."},
            {
            194, @"The control and emergency temperatures, if any, and the generic entry number for each of the
            currently assigned self-reactive substances are given in 2.4.2.3.2.3."},
            { 195, @"For certain organic peroxides types B or C, a smaller packaging than that allowed by packing
            methods OP5 or OP6 respectively has to be used (see 4.1.7 and 2.5.3.2.4)."},
            {
            196, @"Formulations which, in laboratory testing, neither detonate in the cavitated state nor deflagrate,
            which show no effect when heated under confinement and which exhibit no explosive power may be
            transported under this entry.The formulation must also be thermally stable(i.e.the SADT is 60°C or
            higher for a 50 kg package). Formulations not meeting these criteria shall be transported under the
            provisions of class 5.2 (see 2.5.3.2.4)."},
            {198, @"Nitrocellulose solutions containing not more than 20% nitrocellulose may be transported as paint,
            perfumery products or printing ink, as applicable.See UN Nos. 1210, 1263, 1266, 3066, 3469 and
            3470."},
            {199, @"Lead compounds which, when mixed in a ratio of 1:1000 with 0.07M hydrochloric acid and stirred
            for one hour at a temperature of 23°C ± 2°C, exhibit a solubility of 5% or less(see ISO 3711:1990,
            Lead chromate pigments and lead chromate-molybdate pigments – Specifications and methods of
            test) are considered insoluble and are not subject to the provisions of this Code unless they meet
            the criteria for inclusion in another hazard class."},
            {201, @"Lighters and lighter refills shall comply with the provisions of the country in which they were filled. 3
            They shall be provided with protection against inadvertent discharge.The liquid portion of the gas
            shall not exceed 85% of the capacity of the receptacle at 15°C.The receptacles, including the
            closures, shall be capable of withstanding an internal pressure of twice the pressure of the liquefied
            petroleum gas at 55°C.The valve mechanisms and ignition devices shall be securely sealed, taped
            or otherwise fastened or designed to prevent operation or leakage of the contents during transport.
            Lighters shall not contain more than 10 g of liquefied petroleum gas.Lighter refills shall not contain
            more than 65 g of liquefied petroleum gas."},
            {203, @"This entry shall not be used for polychlorinated biphenyls, UN 2315."},
            {204, @"Articles containing smoke-producing substance(s) corrosive according to the criteria for class 8 shall be labelled with a “CORROSIVE” subsidiary risk label(Model No. 8, see 5.2.2.2.2).\nArticles containing smoke-producing substance(s) toxic by inhalation according to the criteria for class 6.1 shall be labelled with a “TOXIC” subsidiary risk label(Model No. 6.1, see 5.2.2.2.2), except that those manufactured before 31 December 2016 may be transported until 1 January 2019 without a “TOXIC” subsidiary label."},
            {205, @"This entry shall not be used for PENTACHLOROPHENOL, UN 3155."},
            {207, @"Plastics moulding compounds may be made from polystyrene, poly(methyl methacrylate) or other polymeric material."},
            {208, @"The commercial grade of calcium nitrate fertilizer, when consisting mainly of a double salt (calcium nitrate and ammonium nitrate) containing not more than 10% ammonium nitrate and at least 12% water of crystallization, is not subject to the provisions of this Code."},
            {209, @"The gas shall be at a pressure corresponding to ambient atmospheric pressure at the time the containment system is closed and this shall not exceed 105 kPa absolute."},
            {210, @"Toxins from plant, animal or bacterial sources which contain infectious substances, or toxins that are contained in infectious substances, shall be classified under class 6.2."},
            {215, @"This entry only applies to the technically pure substance or to formulations derived from it, having an SADT higher than 75°C, and, therefore, does not apply to formulations which are self-reactive substances(for self-reactive substances, see 2.4.2.3.2.3). Homogeneous mixtures containing not more than 35% by mass of azodicarbonamide and at least 65% of inert substance are not subject to this Code unless criteria of other classes are met."},
            {216, @"Mixtures of solids which are not subject to the provisions of this Code and flammable liquids may be transported under this entry without first applying the classification criteria of class 4.1, provided there is no free liquid visible at the time the substance is loaded or at the time the packaging or cargo transport unit is closed.Each cargo transport unit shall be leakproof when used as a bulk container. Sealed packets and articles containing less than 10 mL of a packing group II or III flammable liquid absorbed into a solid material are not subject to the provisions of this Code provided there is no free liquid in the packet or article."},
            {217, @"This entry shall only be used for mixtures of solids which are not subject to the provisions of this Code and toxic liquids may be transported under this entry without first applying the classification criteria of class 6.1, provided there is no free liquid visible at the time the substance is loaded or at the time the packaging or cargo transport unit is closed.Each cargo transport unit shall be leakproof when used as a bulk container.This entry shall not be used for solids containing a packing group I liquid."},
            {218, @"This entry shall only be used for mixtures of solids which are not subject to the provisions of this Code and corrosive liquids may be transported under this entry without first applying the classification criteria of class 8, provided there is no free liquid visible at the time the substance is loaded or at the time the packaging or cargo transport unit is closed.Each cargo transport unit shall be leakproof when used as a bulk container.This entry shall not be used for solids containing a packing group I liquid."},
            {219, @"Genetically modified microorganisms(GMMOs) and genetically modified organisms(GMOs) packed and marked in accordance with packing instruction P904 are not subject to any other provisions of this Code.\nIf GMMOs or GMOs meet the definition in chapter 2.6 of a toxic substance or an infectious substance and the criteria for inclusion in class 6.1 or 6.2, the provisions of this Code for transporting toxic substances or infectious substances apply."},
            {220, @"The technical name of the flammable liquid component only of this solution or mixture shall be shown in parentheses immediately following the proper shipping name."},
            {221, @"Substances included under this entry shall not be of packing group I."},
            {223, @"If the chemical or physical properties of a substance covered by this description are such that, when tested, it does not meet the established defining criteria for the class or division listed in column 3, or any other class or division, it is not subject to the provisions of this Code except in the case of a marine pollutant where 2.10.3 applies."},
            {224, @"Unless it can be demonstrated by testing that the sensitivity of the substance in its frozen state is no greater than in its liquid state, the substance shall remain liquid during normal transport conditions. It shall not freeze at temperatures above -15°C."},
            {225, @"Fire extinguishers under this entry may include installed actuating cartridges (cartridges, power device of division 1.4C or 1.4S) without changing the classification of class 2.2 provided the total quantity of deflagrating(propellant) explosives does not exceed 3.2 g per extinguishing unit.Fire extinguishers shall be manufactured, tested, approved and labelled according to the provisions applied in the country of manufacture.\nNote: “Provisions applied in the country of manufacture” means the provisions applicable in the country of manufacture or those applicable in the country of use.
                Fire extinguishers under this entry include:
            \n.1 portable fire extinguishers for manual handling and operation;
            \n.2 fire extinguishers for installation in aircraft;
            \n.3 fire extinguishers mounted on wheels for manual handling;
            \n.4 fire extinguishing equipment or machinery mounted on wheels or wheeled platforms or units transported similar to(small) trailers; and
            \n.5 fire extinguishers composed of a non-rollable pressure drum and equipment, and handled, e.g. by fork lift or crane when loaded or unloaded.
            \nNote: Pressure receptacles which contain gases for use in the above-mentioned extinguishers or for use in stationary fire-fighting installations shall meet the requirements in chapter 6.2 and all requirements applicable to the relevant dangerous goods when these pressure receptacles are transported separately."},
            {226, @"Formulations of these substances containing not less than 30% non-volatile, non-flammable phlegmatizer are not subject to the provisions of this Code."},
            {227, @"When phlegmatized with water and inorganic inert material, the content of urea nitrate may not exceed 75% by mass and the mixture shall not be capable of being detonated by the series 1, type (a) test in the Manual of Tests and Criteria, part I."},
            {228, @"Mixtures not meeting the criteria for flammable gases (class 2.1) shall be transported under UN 3163."},
            {230, @"Lithium cells and batteries may be transported under this entry if they meet the provisions of 2.9.4."},
            {232, @"This entry shall only be used when the substance does not meet the criteria of any other class.
            Transport in cargo transport units other than in tanks shall be in accordance with standards specified
            by the competent authority of the country of origin."},
            {235, @"This entry applies to articles which contain class 1 explosive substances and which may also contain
            dangerous goods of other classes.These articles are used to enhance safety in vehicles, vessels or
            aircraft, e.g.air bag inflators, air bag modules, seat-belt pretensioners, and pyromechanical devices."},
            { 236, @"Polyester resin kits consist of two components: a base material (either class 3 or class 4.1, packing
            group II or III) and an activator(organic peroxide). The organic peroxide shall be type D, E, or F,
            not requiring temperature control.The packing group shall be II or III, according to the criteria of
            either class 3 or class 4.1, as appropriate, applied to the base material.The quantity limit shown in
            column 7a of the Dangerous Goods List of chapter 3.2 applies to the base material."},
            {237, @"The membrane filters, including paper separators, coating or backing materials, etc., that are present
            in transport, shall not be liable to propagate a detonation as tested by one of the tests described in
            the Manual of Tests and Criteria, part I, test series 1(a).
            In addition, the competent authority may determine, on the basis of the results of suitable burning
            rate tests taking account of the standard tests in the Manual of Tests and Criteria, part III, 33.2.1, that
            nitrocellulose membrane filters in the form in which they are to be transported are not subject to the
            provisions of this Code applicable to flammable solids in class 4.1."},
            {238, @".1 Batteries can be considered as non-spillable provided that they are capable of withstanding the
            vibration and pressure differential tests given below, without leakage of battery fluid:
            Vibration test: The battery is rigidly clamped to the platform of a vibration machine and a simple
            harmonic motion having an amplitude of 0.8 mm(1.6 mm maximum total excursion) is applied.
            The frequency is varied at the rate of 1 Hz/min between the limits of 10 Hz and 55 Hz.The
            entire range of frequencies and return is traversed in 95 ± 5 minutes for each mounting position
            (direction of vibration) of the battery.The battery is tested in three mutually perpendicular
            positions(to include testing with fill openings and vents, if any, in an inverted position) for equal
            time periods.
            Pressure differential test: Following the vibration test, the battery is stored for six hours at 24°C 3
            ± 4°C while subjected to a pressure differential of at least 88 kPa.The battery is tested in three
            mutually perpendicular positions(to include testing with fill openings and vents, if any, in an
            inverted position) for at least six hours in each position.
            Non-spillable type batteries which are an integral part of and necessary for the operation of
            mechanical or electronic equipment shall be securely fastened in the battery holder on the
            equipment and protected in such a manner as to prevent damage and short circuits.
            .2 Non-spillable batteries are not subject to the provisions of this Code if, at a temperature of 55°C,
            the electrolyte will not flow from a ruptured or cracked case and there is no free liquid to flow
            and if, when packaged for transport, the terminals are protected from short circuit."},
            {
            239, @"Batteries or cells shall not contain dangerous goods other than sodium, sulphur or sodium
            compounds (e.g.sodium polysulphides and sodium tetrachloroaluminate). Batteries or cells shall
            not be offered for transport at a temperature such that liquid elemental sodium is present in the
            battery or cell, unless approved and under the conditions established by the competent authority.
            Cells shall consist of hermetically sealed metal casings which fully enclose the dangerous goods
            and which are so constructed and closed as to prevent the release of the dangerous goods under
            normal conditions of transport.
            Batteries shall consist of cells secured within and fully enclosed by a metal casing so constructed
            and closed as to prevent the release of the dangerous goods under normal conditions of transport.
            Batteries installed in vehicles are not subject to the provisions of this Code."},
            {240, @"This entry only applies to vehicles powered by wet batteries, sodium batteries, lithium metal batteries
            or lithium ion batteries and equipment powered by wet batteries or sodium batteries transported
            with these batteries installed.
            For the purpose of this special provision, vehicles are self-propelled apparatus designed to carry one
            or more persons or goods.Examples of such vehicles are electrically-powered cars, motorcycles,
            scooters, three- and four-wheeled vehicles or motorcycles, trucks, locomotives, bicycles (pedal
            cycles with an electric motor) and other vehicles of this type(e.g.self-balancing vehicles or vehicles
            not equipped with at least one seating position), wheel chairs, lawn tractors, self-propelled farming
            and construction equipment, boats and aircraft.This includes vehicles transported in a packaging.
            In this case some parts of the vehicle may be detached from its frame to fit into the packaging.
            Examples of equipment are lawnmowers, cleaning machines or model boats and model aircraft.
            Equipment powered by lithium metal batteries or lithium ion batteries shall be consigned under the
            entries UN 3091 LITHIUM METAL BATTERIES CONTAINED IN EQUIPMENT or UN 3091 LITHIUM
            METAL BATTERIES PACKED WITH EQUIPMENT or UN 3481 LITHIUM ION BATTERIES CONTAINED
            IN EQUIPMENT or UN 3481 LITHIUM ION BATTERIES PACKED WITH EQUIPMENT, as appropriate.
            Hybrid electric vehicles powered by both an internal combustion engine and wet batteries, sodium
            batteries, lithium metal batteries or lithium ion batteries, transported with the battery(ies) installed
            shall be consigned under the entries UN 3166 VEHICLE, FLAMMABLE GAS POWERED or UN 3166
            VEHICLE, FLAMMABLE LIQUID POWERED, as appropriate.Vehicles which contain a fuel cell shall
            be consigned under the entries UN 3166 VEHICLE, FUEL CELL, FLAMMABLE GAS POWERED or
            UN 3166 VEHICLE, FUEL CELL, FLAMMABLE LIQUID POWERED, as appropriate.
            Vehicles may contain other dangerous goods than batteries(e.g.fire extinguishers, compressed
            gas accumulators or safety devices) required for their functioning or safe operation without being
            subject to any additional requirements for these other dangerous goods, unless otherwise specified
            in this Code."},
            {241, @"The formulation shall be prepared so that it remains homogeneous and does not separate during transport.Formulations with low nitrocellulose contents and not showing dangerous properties when tested for their liability to detonate, deflagrate or explode when heated under defined confinement by tests of test series 1(a), 2(b) and 2(c) respectively in the Manual of Tests and Criteria, part I and not being a flammable solid when tested in accordance with test No. 1 in the Manual of Tests and Criteria, part III, paragraph 33.2.1.4 (chips, if necessary, crushed and sieved to a particle size of less than 1.25 mm) are not subject to the provisions of this Code."},
            {242, @"Sulphur is not subject to the provisions of this Code when it has been formed to a specific shape (such as prills, granules, pellets, pastilles or flakes)."},
            {243, @"Gasoline, motor spirit and petrol for use in spark-ignition engines (e.g. in automobiles, stationary engines and other engines) shall be assigned to this entry regardless of variations in volatility."},
            {244, @"This entry includes materials and substances such as aluminium dross, aluminium skimmings, spent cathodes, spent potliner and aluminium salt slags.
            \nBefore loading, these by-products shall be cooled to ambient temperature, unless they have been calcined to remove moisture. Cargo transport units containing bulk loads shall be adequately ventilated and protected against ingress of water throughout the journey."},
            {247, @"Alcoholic beverages containing more than 24% alcohol but not more than 70% by volume, when transported as part of the manufacturing process, may be transported in wooden barrels with a capacity of more than 250 L and not more than 500 L meeting the general requirements of 4.1.1, as appropriate, on the following conditions:
            \n.1 the wooden barrels shall be checked and tightened before filling;
            \n.2 sufficient ullage(not less than 3%) shall be left to allow for the expansion of the liquid;
            \n.3 the wooden barrels shall be transported with the bungholes pointing upwards;
            \n.4 the wooden barrels shall be transported in containers meeting the provisions of the International
            \nConvention for Safe Containers(CSC 1972), as amended, and each wooden barrel shall be secured in custom-made cradles and be wedged by appropriate means to prevent it from being displaced in any way during transport; and
            \n.5 when carried on board ships, the containers shall be stowed in open cargo spaces or in enclosed cargo spaces complying with the requirements for class 3 flammable liquids with a flashpoint of 23°C c.c.or less in regulation II-2/19 of SOLAS, 74, as amended or regulation II-2/54 of SOLAS 74, as amended by the resolutions indicated in II-2/1.2.1, as applicable."},
            {249, @"Ferrocerium, stabilized against corrosion, with a minimum iron content of 10% is not subject to the provisions of this Code."},
            {250, @"This entry may only be used for samples of chemicals taken for analysis in connection with the implementation of the Convention on the Prohibition of the Development, Production, Stockpiling and Use of Chemical Weapons and on their Destruction. The transport of substances under this entry shall be in accordance with the chain of custody and security procedures specified by the Organization for the Prohibition of Chemical Weapons.
            \nThe chemical sample may only be transported provided prior approval has been granted by the competent authority or the Director General of the Organization for the Prohibition of Chemical Weapons and providing the sample complies with the following conditions:
            \n.1 it shall be packaged according to packing instruction 623 in the International Civil Aviation Organization’s Technical Instructions for the Safe Transport of Dangerous Goods by Air; and
            \n.2 during transport, it shall be accompanied by a copy of the document of approval for transport, showing the quantity limitations and the packing provisions."},
            {251, @"The entry CHEMICAL KIT or FIRST AID KIT is intended to apply to boxes, cases, etc., containing small quantities of various dangerous goods which are used, for example, for medical, analytical, testing or repair purposes. Such kits may not contain dangerous goods for which the quantity “0” has been indicated in column 7a of the Dangerous Goods List.
            \nComponents shall not react dangerously (see 4.1.1.6). The total quantity of dangerous goods in any one kit shall not exceed either 1 L or 1 kg.The packing group assigned to the kit as a whole shall be the most stringent packing group assigned to any individual substance in the kit.
            \nWhere the kit contains only dangerous goods to which no packing group is assigned, no packin group need be indicated on the dangerous goods transport document.
            \nKits which are carried on board vehicles for first-aid or operating purposes are not subject to the provisions of this Code.
            \nChemical kits and first aid kits containing dangerous goods in inner packagings which do not exceed the quantity limits for limited quantities applicable to individual substances as specified in column 7a of the Dangerous Goods List may be transported in accordance with chapter 3.4."},
            {252, @"Provided the ammonium nitrate remains in solution under all conditions of transport, aqueous solutions of ammonium nitrate, with not more than 0.2% combustible material, in a concentration not exceeding 80%, are not subject to the provisions of this Code."},
            {266, @"This substance, when containing less alcohol, water or phlegmatizer than specified, shall not be transported, unless specifically authorized by the competent authority."},
            {267, @"Any explosives, blasting, type C containing chlorates shall be segregated from explosives containing ammonium nitrate or other ammonium salts."},
            {270, @"Aqueous solutions of class 5.1 inorganic solid nitrate substances are considered as not meeting the criteria of class 5.1 if the concentration of the substances in solution at the minimum temperature encountered in transport is not greater than 80% of the saturation limit."},
            {271, @"Lactose or glucose or similar materials may be used as a phlegmatizer provided that the substance contains not less than 90%, by mass, of phlegmatizer. The competent authority may authorize these mixtures to be classified under class 4.1 on the basis of series 6(c) tests of part I of the Manual of Tests and Criteria on at least three packages as prepared for transport.Mixtures containing at least 98%, by mass, of phlegmatizer are not subject to the provisions of this Code.Packages containing mixtures with not less than 90%, by mass, of phlegmatizer need not bear a “TOXIC” subsidiary risk label."},
            {272, @"This substance shall not be transported under the provisions of class 4.1 unless specifically
            authorized by the competent authority(see UN 0143 or UN 0150 as appropriate)."},
            {273, @"Maneb and maneb preparations stabilized against self-heating need not be classified in class 4.2
            when it can be demonstrated by testing that a cubic volume of 1 m3 of substance does not self-ignite
            and that the temperature at the centre of the sample does not exceed 200°C when the sample is
            maintained at a temperature of not less than 75°C ± 2°C for a period of 24 hours."},
            {274, @"For the purposes of documentation and package marking, the proper shipping name shall be
            supplemented with the technical name(see 3.1.2.8.1)."},
            {277, @"For aerosols or receptacles containing toxic substances, the limited quantity value is 120 mL.For all
            other aerosols or receptacles, the limited quantity value is 1,000 mL."},
            {278, @"These substances shall not be classified and transported unless authorized by the competent
            authority on the basis of results from series 2 tests and series 6(c) tests of part I of the Manual of
            Tests and Criteria on packages as prepared for transport(see 2.1.3.1). The competent authority shall
            assign the packing group on the basis of the chapter 2.3 criteria and the package type used for the
            series 6(c) tests."},
            {279, @"The substance is assigned to this classification or packing group based on human experience rather
            than the strict application of classification criteria set out in this Code."},
            {280, @"This entry applies to safety devices for vehicles, vessels or aircraft, e.g.air bag inflators, air bag
            modules, seat-belt pretensioners, and pyromechanical devices, which contain dangerous goods
            of class 1 or of other classes, when transported as component parts and if these articles as
            presented for transport have been tested in accordance with test series 6(c) of part I of the Manual
            of Tests and Criteria, with no explosion of the device, no fragmentation of device casing or pressure
            receptacle, and no projection hazard nor thermal effect which would significantly hinder fire-fighting
            or emergency response efforts in the immediate vicinity.This entry does not apply to life-saving
            appliances described in special provision 296 (UN Nos. 2990 and 3072)."},
            {281, @"Transport of hay, straw or bhusa when wet, damp or contaminated with oil is prohibited and when
            not wet or contaminated with oil is subject to the provisions of this Code."},
            {283, @"Articles, containing gas, intended to function as shock absorbers, including impact-energyabsorbing
            devices or pneumatic springs, are not subject to the provisions of this Code provided:
            .1 each article has a gas space capacity not exceeding 1.6 L and a charge pressure not exceeding
            280 bar where the product of the capacity (litres) and charge pressure (bar) does not exceed 80
            (i.e. 0.5 L gas space and 160 bar charge pressure, 1 L gas space and 80 bar charge pressure,
            1.6 L gas space and 50 bar charge pressure, 0.28 L gas space and 280 bar charge pressure);
            .2 each article has a minimum burst pressure of 4 times the charge pressure at 20°C for products
            not exceeding 0.5 L gas space capacity and 5 times charge pressure for products greater than
            0.5 L gas space capacity;
            .3 each article is manufactured from material which will not fragment upon rupture;
            .4 each article is manufactured in accordance with a quality-assurance standard acceptable to
            the competent authority; and
            .5 the design type has been subjected to a fire test demonstrating that pressure in the article is
            relieved by means of a fire-degradable seal or other pressure relief device, such that the article
            will not fragment and that the article does not rocket."},
            { 284, @"An oxygen generator, chemical, containing oxidizing substances shall meet the following conditions:
            .1 the generator, when containing an explosive device, shall only be transported under this entry
            when excluded from class 1 in accordance with 2.1.3 of this Code;
            .2 the generator, without its packaging, shall be capable of withstanding a 1.8 m drop test onto
            a rigid, non-resilient, flat and horizontal surface, in the position most likely to cause damage,
            without loss of its contents and without actuation; and
            .3 when the generator is equipped with an actuating device, it shall have at least two positive
            means of preventing unintentional actuation."},
            {286, @"Nitrocellulose membrane filters covered by this entry, each with a mass not exceeding 0.5 g, are not
            subject to the provisions of this Code when contained individually in an article or a sealed packet."},
            {288, @"These substances shall not be classified and transported unless authorized by the competent
            authority on the basis of results from series 2 tests and series 6(c) tests of part I of the Manual of
            Tests and Criteria on packages as prepared for transport(see 2.1.3)."},
            {289, @"Safety devices, electrically initiated and safety devices, pyrotechnic installed in vehicles, vessels
            or aircraft or in completed components such as steering columns, door panels, seats, etc., are not
            subject to the provisions of this Code."},
            {290, @"When this radioactive material meets the definitions and criteria of other classes or divisions as
            defined in part 2, it shall be classified in accordance with the following:
            .1 where the substance meets the criteria for dangerous goods in excepted quantities as set out in
            chapter 3.5, the packagings shall be in accordance with 3.5.2 and meet the testing requirements
            of 3.5.3. All other requirements applicable to radioactive material, excepted packages as set out
            in 1.5.1.5 shall apply without reference to the other class or division;
            .2 where the quantity exceeds the limits specified in 3.5.1.2, the substance shall be classified in
            accordance with the predominant subsidiary risk.The dangerous goods transport document
            shall describe the substance with the UN number and proper shipping name applicable to
            the other class supplemented with the name applicable to the radioactive excepted package
            according to column 2 in the Dangerous Goods List of chapter 3.2, and the substance shall be
            transported in accordance with the provisions applicable to that UN number.An example of the
            information shown on the dangerous goods transport document is:
            UN 1993, Flammable liquid, N.O.S. (ethanol and toluene mixture), Radioactive material, excepted
            package – limited quantity of material, class 3, PG II.
            In addition, the provisions of 2.7.2.4.1 shall apply;
            .3 the provisions of chapter 3.4 for the transport of dangerous goods packed in limited quantities
            shall not apply to substances classified in accordance with subparagraph .2;
            .4 when the substance meets a special provision that exempts this substance from all dangerous
            goods provisions of the other classes, it shall be classified in accordance with the applicable
            UN number of class 7 and all requirements specified in 1.5.1.5 shall apply."},
            {
            291, @"Flammable liquefied gases shall be contained within refrigerating-machine components. These
            components shall be designed and tested to at least three times the working pressure of the
            machinery. The refrigerating machines and refrigerating-machine components shall be designed
            and constructed to contain the liquefied gas and preclude the risk of bursting or cracking of the
            pressure-retaining components during normal conditions of transport.Refrigerating machines and
            refrigerating-machine components are not subject to the provisions of this Code if they contain less
            than 12 kg of gas."},
            { 293, @"The following definitions apply to matches:
            .1 Fusee matches are matches the heads of which are prepared with a friction-sensitive igniter
            composition and a pyrotechnic composition which burns with little or no flame, but with intense heat;
            .2 Safety matches are combined with or attached to the box, book or card that can be ignited by
            friction only on a prepared surface;
            .3 “Strike anywhere” matches are matches that can be ignited by friction on a solid surface;
            .4 Wax ‘Vesta’ matches are matches that can be ignited by friction either on a prepared surface or
            on a solid surface."},
            { 294, @"Safety matches and wax ‘Vesta’ matches in an outer packaging not exceeding 25 kg net mass are
            not subject to any other provision(except marking) of this Code when packaged in accordance with
            packing instruction P407."},
            {295, @"Batteries need not be individually marked and labelled if the pallet bears the appropriate mark and
            label."},
            {296, @"These entries apply to life-saving appliances such as liferafts, personal flotation devices and
            self-inflating slides. UN 2990 applies to self-inflating appliances. UN 3072 applies to life-saving
            appliances that are not self-inflating.Life-saving appliances may contain:
            .1 signal devices (class 1) which may include smoke and illumination signal flares packed in
            packagings that prevent them from being inadvertently activated;
            .2 for UN 2990 only, cartridges, power device of division 1.4, compatibility group S, may be
            contained for purposes of the self-inflating mechanism and provided that the quantity of
            explosives per appliance does not exceed 3.2 g;
            .3 class 2.2 compressed or liquefied gases;
            .4 electric storage batteries(class 8) and lithium batteries(class 9);
            .5 first aid kits or repair kits containing small quantities of dangerous goods(e.g.classes 3, 4.1,
            5.2, 8 or 9 substances); or
            .6 “Strike anywhere” matches packed in packagings that prevent them from being inadvertently
            activated.
            Life-saving appliances packed in strong rigid outer packagings with a total maximum gross mass of
            40 kg, containing no dangerous goods other than class 2.2 compressed or liquefied gases with no
            subsidiary risk in receptacles with a capacity not exceeding 120 mL, installed solely for the purpose
            of the activation of the appliance, are not subject to the provision of this Code."},
            {299, @"Consignments of:
            .1 Cotton, dry having a density not less than 360 kg/m3;
            .2 Flax, dry having a density not less than 400 kg/m3; 3
            .3 Sisal, dry having a density not less than 360 kg/m3; and
            .4 Tampico fibre, dry having a density not less than 360 kg/m3,
            according to ISO 8115:1986, are not subject to the provisions of this Code when transported in
            closed cargo transport units."},
            {300, @"Fish meal, fish scrap and krill meal shall not be transported if the temperature at the time of loading
            exceeds 35°C or 5°C above the ambient temperature, whichever is higher."},
            {301, @"This entry only applies to machinery or apparatus containing dangerous substances as a residue or
            an integral element of the machinery or apparatus.It shall not be used for machinery or apparatus
            for which a proper shipping name already exists in the Dangerous Goods List. Machinery and
            apparatus transported under this entry shall only contain dangerous goods which are authorized to
            be transported in accordance with the provisions in chapter 3.4 (Limited quantities). The quantity of
            dangerous goods in machinery or apparatus shall not exceed the quantity specified in column 7a of
            the Dangerous Goods List for each item of dangerous goods contained. If the machinery or apparatus
            contains more than one item of dangerous goods, the individual substances shall not be capable of
            reacting dangerously with one another (see 4.1.1.6). When it is required to ensure liquid dangerous
            goods remain in their intended orientation, package orientation labels meeting the specifications of
            ISO 780:1985 shall be affixed on at least two opposite vertical sides with the arrows pointing in the
            correct direction. The transport of dangerous goods in machinery or apparatus where the quantity
            of dangerous goods exceeds the quantity specified in column 7a of the Dangerous Goods List is
            authorized when approved by the competent authority, except where special provision 363 applies."},
            { 302, @"Fumigated cargo transport units containing no other dangerous goods are only subject to the
            provisions of 5.5.2."},
            { 303, @"Receptacles shall be assigned to the class and, if any, subsidiary hazard of the gas or mixture of
            gases contained therein determined in accordance with the provisions of chapter 2.2."},
            {304, @"This entry may only be used for the transport of non-activated batteries which contain dry potassium
            hydroxide and which are intended to be activated prior to use by the addition of an appropriate
            amount of water to the individual cells."},
            {305, @"These substances are not subject to the provisions of this Code when in concentrations of not more
            than 50 mg/kg."},
            { 306, @"This entry may only be used for substances that are too insensitive for acceptance into class 1 when
            tested in accordance with test series 2 (see Manual of Tests and Criteria, part I)."},
            {307, @"This entry shall be used for uniform mixtures containing ammonium nitrate as the main ingredient
            within the following composition limits:
            .1 not less than 90% ammonium nitrate with not more than 0.2% total combustible/organic
            material calculated as carbon and with added matter, if any, which is inorganic and inert towards
            ammonium nitrate; or
            .2 less than 90% but more than 70% ammonium nitrate with other inorganic materials or more
            than 80% but less than 90% ammonium nitrate mixed with calcium carbonate and/or dolomite
            and/or mineral calcium sulphate and not more than 0.4% total combustible/organic material
            calculated as carbon; or
            .3 nitrogen type ammonium nitrate based fertilizers containing mixtures of ammonium nitrate and
            ammonium sulphate with more than 45% but less than 70% ammonium nitrate and not more
            than 0.4% total combustible/organic material calculated as carbon such that the sum of the
            percentage compositions of ammonium nitrate and ammonium sulphate exceeds 70%."},
            {308, @"Fish scrap or fish meal shall contain at least 100 ppm of antioxidant(ethoxyquin) at the time of
            consignment."},
            {309, @"This entry applies to non-sensitized emulsions, suspensions and gels consisting primarily of a
            mixture of ammonium nitrate and fuel, intended to produce a Type E blasting explosive only after
            further processing prior to use.
            The mixture for emulsions typically has the following composition: 60–85% ammonium nitrate,
            5–30% water, 2–8% fuel, 0.5–4% emulsifier agent, 0–10% soluble flame suppressants, and trace
            additives.Other inorganic nitrate salts may replace part of the ammonium nitrate.
            The mixture for suspensions and gels typically has the following composition: 60–85% ammonium
            nitrate, 0–5% sodium or potassium perchlorate, 0–17% hexamine nitrate or monomethylamine
            nitrate, 5–30% water, 2–15% fuel, 0.5–4% thickening agent, 0–10% soluble flame suppressants,
            and trace additives.Other inorganic nitrate salts may replace part of the ammonium nitrate.
            Substances shall satisfactorily pass tests 8(a), (b) and(c) of test series 8 of the Manual of Tests and
            Criteria, part I, section 18 and be approved by the competent authority."},
            {310, @"The testing requirements in the Manual of Tests and Criteria, part III, subsection 38.3 do not apply
            to production runs, consisting of not more than 100 cells and batteries, or to pre-production
            prototypes of cells and batteries when these prototypes are transported for testing when packaged
            in accordance with packing instruction P910 of 4.1.4.1.
            The transport document shall include the following statement: “Transport in accordance with special
            provision 310”.
            Damaged or defective cells, batteries, or cells and batteries contained in equipment shall be
            transported in accordance with special provision 376 and packaged in accordance with packing
            instructions P908 of 4.1.4.1 or LP904 of 4.1.4.3, as applicable.
            Cells, batteries or cells and batteries contained in equipment transported for disposal or recycling
            may be packaged in accordance with special provision 377 and packing instruction P909 of 4.1.4.1."},
            {311, @"Substances shall not be transported under this entry unless approved by the competent authority
            on the basis of the results of appropriate tests according to part I of the Manual of Tests and Criteria.
            Packaging shall ensure that the percentage of diluent does not fall below that stated in the competent
            authority approval at any time during transport."},
            {312, @"Vehicles powered by a fuel cell engine shall be consigned under the entries UN No. 3166 VEHICLE,
            FUEL CELL, FLAMMABLE GAS POWERED or UN No. 3166 VEHICLE, FUEL CELL, FLAMMABLE
            LIQUID POWERED, as appropriate.These entries include hybrid electric vehicles powered by both
            a fuel cell and an internal combustion engine with wet batteries, sodium batteries, lithium metal
            batteries or lithium ion batteries, transported with the battery(ies) installed.
            Other vehicles which contain an internal combustion engine shall be consigned under the entries
            UN 3166 VEHICLE, FLAMMABLE GAS POWERED or UN 3166 VEHICLE, FLAMMABLE LIQUID
            POWERED, as appropriate.These entries include hybrid electric vehicles powered by both an
            internal combustion engine and wet batteries, sodium batteries, lithium metal batteries or lithium ion
            batteries, transported with the batteries installed."},
            {
            314, @".1 These substances are liable to exothermic decomposition at elevated temperatures.
            Decomposition can be initiated by heat or by impurities (e.g.powdered metals (iron, manganese,
            cobalt, magnesium) and their compounds).
            .2 During the course of transport, these substances shall be shaded from direct sunlight and all
            sources of heat and be placed in adequately ventilated areas."},
            { 315, @"This entry shall not be used for class 6.1 substances which meet the inhalation toxicity criteria for
            packing group I described in 2.6.2.2.4.3."},
            {316, @"This entry applies only to calcium hypochlorite, dry, when transported in non-friable tablet form."},
            {317, @"“Fissile-excepted” applies only to those fissile materials and packages containing fissile material
            which are excepted in accordance with 2.7.2.3.5."},
            {318, @"For the purposes of documentation, the proper shipping name shall be supplemented with the
            technical name(see 3.1.2.8). Technical names need not be shown on the package.When the
            infectious substances to be transported are unknown, but suspected of meeting the criteria for
            inclusion in category A and assignment to UN 2814 or UN 2900, the words “suspected category A
            infectious substance” shall be shown, in parentheses, following the proper shipping name on the
            transport document, but not on the outer packagings."},
            {
            319, @"Substances packed and packages marked in accordance with packing instruction P650 are not
            subject to any other provisions of this Code."},
            { 321, @"These storage systems shall always be considered as containing hydrogen."},
            { 322, @"When transported in non-friable tablet form, these goods are assigned to packing group III."},
            { 324, @"This substance needs to be stabilized when in concentrations of not more than 99%."},
            { 325, @"In the case of non-fissile or fissile-excepted uranium hexafluoride, the material shall be classified
            under UN 2978."},
            { 326, @"In the case of fissile uranium hexafluoride, the material shall be classified under UN 2977."},
            { 327, @"Waste aerosols consigned in accordance with 5.4.1.4.3.3 may be transported under this entry for the
            purposes of reprocessing or disposal. They need not be protected against movement and inadvertent
            discharge provided that measures to prevent dangerous build-up of pressure and dangerous
            atmospheres are addressed.Waste aerosols, other than those leaking or severely deformed, shall
            be packed in accordance with packing instruction P207 and special provision PP87, or packing
            instruction LP200 and special packing provision L2.Leaking or severely deformed aerosols shall
            be transported in salvage packagings provided appropriate measures are taken to ensure there
            is no dangerous build-up of pressure.Waste aerosols shall not be transported in closed freight
            containers."},
            { 328, @"This entry applies to fuel cell cartridges, including when contained in equipment or packed with 3
            equipment.Fuel cell cartridges installed in or integral to a fuel cell system are regarded as contained
            in equipment. “Fuel cell cartridge” means an article that stores fuel for discharge into the fuel cell
            through a valve(s) that controls the discharge of fuel into the fuel cell.Fuel cell cartridges, including
            when contained in equipment, shall be designed and constructed to prevent fuel leakage under
            normal conditions of transport.
            Fuel cell cartridge design types using liquids as fuels shall pass an internal pressure test at a
            pressure of 100 kPa(gauge) without leakage.
            Except for fuel cell cartridges containing hydrogen in metal hydride, which shall be in compliance
            with special provision 339, each fuel cell cartridge design type shall be shown to pass a 1.2 m drop
            test onto an unyielding surface, in the orientation most likely to result in failure of the containment
            system, with no loss of contents.
            When lithium metal or lithium ion batteries are contained in the fuel cell system, the consignment
            shall be consigned under this entry and under the appropriate entries for UN 3091 LITHIUM METAL
            BATTERIES CONTAINED IN EQUIPMENT or UN 3481 LITHIUM ION BATTERIES CONTAINED IN
            EQUIPMENT."},
            { 332, @"Magnesium nitrate hexahydrate is not subject to the provisions of this Code."},
            { 333, @"Ethanol and gasoline, motor spirit or petrol mixtures for use in spark-ignition engines (e.g. in
            automobiles, stationary engines and other engines) shall be assigned to this entry regardless of
            variations in volatility."},
            {
            334, @"A fuel cell cartridge may contain an activator provided it is fitted with two independent means of
            preventing unintended mixing with the fuel during transport."},
            {
            335, @"Mixtures of solids which are not subject to the provisions of this Code and environmentally hazardous
            liquids assigned to UN 3082 may be classified and transported as UN 3077, provided there is no free
            liquid visible at the time the substance is loaded or at the time the packaging or cargo transport unit
            is closed.If free liquid is visible at the time the mixture is loaded or at the time the packaging or cargo
            transport unit is closed, the mixture shall be classified as UN 3082. Each cargo transport unit shall
            be leakproof when used as a bulk container.Sealed packets and articles containing less than 10 mL
            of an environmentally hazardous liquid assigned to UN 3082, absorbed into a solid material but with
            no free liquid in the packet or article, or containing less than 10 g of an environmentally hazardous
            solid assigned to UN 3077, are not subject to the provisions of this Code."},
            {
            338, @"Each fuel cell cartridge transported under this entry and designed to contain a liquefied flammable
            gas shall:
            .1 be capable of withstanding, without leakage or bursting, a pressure of at least two times the
            equilibrium pressure of the contents at 55°C;
            .2 not contain more than 200 mL liquefied flammable gas, the vapour pressure of which shall not
            exceed 1 000 kPa at 55°C; and
            .3 pass the hot water bath test prescribed in 6.2.4.1 of chapter 6.2."},
            {
            339, @"Fuel cell cartridges containing hydrogen in a metal hydride transported under this entry shall have a
            water capacity less than or equal to 120 mL.The pressure in the fuel cell cartridge shall not exceed
            5 MPa at 55°C.The design type shall withstand, without leaking or bursting, a pressure of two (2)
            times the design pressure of the cartridge at 55°C or 200 kPa more than the design pressure of the
            cartridge at 55°C, whichever is greater. The pressure at which this test is conducted is referred to in
            the Drop Test and the Hydrogen Cycling Test as the “minimum shell burst pressure”.
            Fuel cell cartridges shall be filled in accordance with procedures provided by the manufacturer. The
            manufacturer shall provide the following information with each fuel cell cartridge:
            .1 Inspection procedures to be carried out before initial filling and before refilling of the fuel cell
            cartridge;
            .2 Safety precautions and potential hazards to be aware of;
            .3 Method for determining when the rated capacity has been achieved;
            .4 Minimum and maximum pressure range;
            .5 Minimum and maximum temperature range; and
            .6 Any other requirements to be met for initial filling and refilling, including the type of equipment
            to be used for initial filling and refilling.
            The fuel cell cartridges shall be designed and constructed to prevent fuel leakage under normal
            conditions of transport. Each cartridge design type, including cartridges integral to a fuel cell, shall
            be subjected to and shall pass the following tests:
            Drop test
            A 1.8 m drop test onto an unyielding surface in four different orientations:
            .1 Vertically, on the end containing the shut-off valve assembly;
            .2 Vertically, on the end opposite to the shut-off valve assembly;
            .3 Horizontally, onto a steel apex with a diameter of 38 mm, with the steel apex in the upward
            position; and
            .4 At a 45° angle on the end containing the shut-off valve assembly.
            There shall be no leakage, determined by using a soap bubble solution or other equivalent
            means on all possible leak locations, when the cartridge is charged to its rated charging
            pressure.The fuel cell cartridge shall then be hydrostatically pressurized to destruction. The
            recorded burst pressure shall exceed 85% of the minimum shell burst pressure.
            Fire test
            A fuel cell cartridge filled to rated capacity with hydrogen shall be subjected to a fire engulfment test.
            The cartridge design, which may include a vent feature integral to it, is deemed to have passed the
            fire test if:
            .1 The internal pressure vents to zero gauge pressure without rupture of the cartridge; or
            .2 The cartridge withstands the fire for a minimum of 20 minutes without rupture.
            Hydrogen cycling test
            This test is intended to ensure that a fuel cell cartridge design stress limits are not exceeded during
            use.
            The fuel cell cartridge shall be cycled from not more than 5% rated hydrogen capacity to not less
            than 95% rated hydrogen capacity and back to not more than 5% rated hydrogen capacity.The rated
            charging pressure shall be used for charging and temperatures shall be held within the operating
            temperature range.The cycling shall be continued for at least 100 cycles.
            Following the cycling test, the fuel cell cartridge shall be charged and the water volume displaced
            by the cartridge shall be measured. The cartridge design is deemed to have passed the hydrogen
            cycling test if the water volume displaced by the cycled cartridge does not exceed the water volume
            displaced by an uncycled cartridge charged to 95% rated capacity and pressurized to 75% of its
            minimum shell burst pressure.
            Production leak test
            Each fuel cell cartridge shall be tested for leaks at 15°C ± 5°C, while pressurized to its rated charging
            pressure. There shall be no leakage, determined by using a soap bubble solution or other equivalent
            means on all possible leak locations.
            Each fuel cell cartridge shall be permanently marked with the following information:
            .1 The rated charging pressure in megapascals (MPa);
            .2 The manufacturer’s serial number of the fuel cell cartridges or unique identification number; and
            .3 The date of expiry based on the maximum service life(year in four digits; month in two digits)."},
            {340, @"Chemical kits, first aid kits and polyester resin kits containing dangerous substances in inner
            packagings which do not exceed the quantity limits for excepted quantities applicable to individual
            substances as specified in column 7b of the Dangerous Goods List may be transported in accordance
            with chapter 3.5. Class 5.2 substances, although not individually authorized as excepted quantities
            in the Dangerous Goods List, are authorized in such kits and are assigned code E2(see 3.5.1.2)."},
            {341, @"Bulk transport of infectious substances in BK2 bulk containers is only permitted for infectious
            substances contained in animal material as defined in 1.2.1 (see 4.3.2.4.1)."},
            {342, @"Glass inner receptacles(such as ampoules or capsules) intended only for use in sterilization
            devices, when containing less than 30 mL of ethylene oxide per inner packaging with not more than
            300 mL per outer packaging, may be transported in accordance with the provisions in chapter 3.5,
            irrespective of the indication of “E0” in column 7b of the Dangerous Goods List provided that:
            .1 After filling, each glass inner receptacle has been determined to be leak tight by placing the
            glass inner receptacle in a hot water bath at a temperature, and for a period of time, sufficient
            to ensure that an internal pressure equal to the vapour pressure of ethylene oxide at 55°C is
            achieved.Any glass inner receptacle showing evidence of leakage, distortion or other defect
            under this test shall not be transported under the terms of this special provision;
            .2 In addition to the packaging required by 3.5.2, each glass inner receptacle is placed in a sealed
            plastics bag compatible with ethylene oxide and capable of containing the contents in the event
            of breakage or leakage of the glass inner receptacle; and
            .3 Each glass inner receptacle is protected by a means of preventing puncture of the plastics bag
            (e.g.sleeves or cushioning) in the event of damage to the packaging(e.g.by crushing)."},
            {343, @"This entry applies to crude oil containing hydrogen sulphide in sufficient concentration that vapours 3
            evolved from the crude oil can present an inhalation hazard.The packing group assigned shall be
            determined by the flammability hazard and inhalation hazard, in accordance with the degree of
            danger presented."},
            { 344, @"The provisions of 6.2.4 shall be met."},
            { 345, @"This gas contained in open cryogenic receptacles with a maximum capacity of one litre constructed
            with glass double walls having the space between the inner and outer wall evacuated (vacuum
            insulated) is not subject to the provisions of this Code provided each receptacle is transported in an
            outer packaging with suitable cushioning or absorbent materials to protect it from impact damage."},
            {
            346, @"Open cryogenic receptacles conforming to the requirements of packing instruction P203 and
            containing no dangerous goods except for UN 1977, nitrogen, refrigerated liquid, which is fully
            absorbed in a porous material, are not subject to any other provisions of this Code."},
            {
            347, @"This entry shall only be used if the results of test series 6(d) of part I of the Manual of Tests and
            Criteria have demonstrated that any hazardous effects arising from functioning are confined within
            the package."},
            { 348, @"Batteries manufactured after 31 December 2011 shall be marked with the Watt hour rating on the
            outside case."},
            { 349, @"Mixtures of a hypochlorite with an ammonium salt are not to be accepted for transport. UN 1791
            hypochlorite solution is a substance of class 8."},
            {350, @"Ammonium bromate and its aqueous solutions and mixtures of a bromate with an ammonium salt
            are not to be accepted for transport."},
            {351, @"Ammonium chlorate and its aqueous solutions and mixtures of a chlorate with an ammonium salt are
            not to be accepted for transport."},
            {352, @"Ammonium chlorite and its aqueous solutions and mixtures of a chlorite with an ammonium salt are
            not to be accepted for transport."},
            {353, @"Ammonium permanganate and its aqueous solutions and mixtures of a permanganate with an
            ammonium salt are not to be accepted for transport."},
            {354, @"This substance is toxic by inhalation."},
            {355, @"Oxygen cylinders for emergency use transported under this entry may include installed actuating
            cartridges (cartridges, power device of class 1.4, compatibility group C or S), without changing the
            classification of class 2.2 provided the total quantity of deflagrating(propellant) explosives does not
            exceed 3.2 g per oxygen cylinder.The cylinders with the installed actuating cartridges as prepared
            for transport shall have an effective means of preventing inadvertent activation."},
            { 356 , @"Metal hydride storage systems installed in vehicles, vessels or aircrafts or in completed components
            or intended to be installed in vehicles, vessels or aircrafts shall be approved by the competent
            authority before acceptance for transport.The transport document shall include an indication that
            the package was approved by the competent authority or a copy of the competent authority approval
            shall accompany each consignment."},
            { 357, @"Petroleum crude oil containing hydrogen sulphide in sufficient concentration that vapours evolved
            from the crude oil can present an inhalation hazard shall be consigned under the entry UN 3494
            PETROLEUM SOUR CRUDE OIL, FLAMMABLE, TOXIC."},
            { 358, @"Nitroglycerin solution in alcohol with more than 1% but not more than 5% nitroglycerin may be
            classified in class 3 and assigned to UN 3064 provided all the requirements of packing instruction
            P300 are complied with."},
            {359, @"Nitroglycerin solution in alcohol with more than 1% but not more than 5% nitroglycerin shall be
            classified in class 1 and assigned to UN 0144 if not all the requirements of packing instruction P300
            are complied with."},
            {360, @"Vehicles only powered by lithium metal batteries or lithium ion batteries shall be consigned under the
            entry UN 3171 BATTERY POWERED VEHICLE."},
            {361, @"This entry applies to electric double layer capacitors with an energy storage capacity greater than
            0.3 Wh.Capacitors with an energy storage capacity of 0.3 Wh or less are not subject to the provisions
            of this Code.Energy storage capacity means the energy held by a capacitor, as calculated using
            the nominal voltage and capacitance.All capacitors to which this entry applies, including capacitors
            containing an electrolyte that does not meet the classification criteria of any class or division of
            dangerous goods, shall meet the following conditions:
            .1 Capacitors not installed in equipment shall be transported in an uncharged state.Capacitors
            installed in equipment shall be transported either in an uncharged state or protected against
            short circuit;
            .2 Each capacitor shall be protected against a potential short circuit hazard in transport as follows:
            .1 when a capacitor’s energy storage capacity is less than or equal to 10 Wh or when the
            energy storage capacity of each capacitor in a module is less than or equal to 10 Wh , the
            capacitor or module shall be protected against short circuit or be fitted with a metal strap
            connecting the terminals; and
            .2 when the energy storage capacity of a capacitor or a capacitor in a module is more than
            10 Wh, the capacitor or module shall be fitted with a metal strap connecting the terminals;
            .3 Capacitors containing dangerous goods shall be designed to withstand a 95 kPa pressure
            differential;
            .4 Capacitors shall be designed and constructed to safely relieve pressure that may build up
            in use, through a vent or a weak point in the capacitor casing.Any liquid which is released
            upon venting shall be contained by the packaging or by the equipment in which a capacitor is
            installed; and
            .5 Capacitors manufactured after 31 December 2013 shall be marked with the energy storage
            capacity in Wh.
            Capacitors containing an electrolyte not meeting the classification criteria of any class or division of
            dangerous goods, including when installed in equipment, are not subject to other provisions of this
            Code.
            Capacitors containing an electrolyte meeting the classification criteria of any class or division
            of dangerous goods, with an energy storage capacity of 10 Wh or less are not subject to other
            provisions of this Code when they are capable of withstanding a 1.2 m drop test unpackaged on an
            unyielding surface without loss of contents.
            Capacitors containing an electrolyte meeting the classification criteria of any class or division of
            dangerous goods that are not installed in equipment and with an energy storage capacity of more
            than 10 Wh are subject to the provisions of this Code.
            Capacitors installed in the equipment and containing an electrolyte meeting the classification
            criteria of any class or division of dangerous goods, are not subject to other provisions of this Code
            provided the equipment is packaged in a strong outer packaging constructed of suitable material
            and of adequate strength and design, in relation to the packaging’s intended use and in such a
            manner as to prevent accidental functioning of capacitors during transport. Large robust equipment
            containing capacitors may be offered for transport unpackaged or on pallets when capacitors are
            afforded equivalent protection by the equipment in which they are contained.
            Note: Capacitors which by design maintain a terminal voltage (e.g.asymmetrical capacitors) do not
            belong to this entry."},
            {362, @"This entry applies to liquids, pastes or powders, pressurized with a propellant which meets the definition of a gas in 2.2.1.2.1 or 2.2.1.2.2.
            \nNote: A chemical under pressure in an aerosol dispenser shall be transported under UN 1950.
            \nThe following provisions shall apply:
            \n.1\tthe chemical under pressure shall be classified based on the hazard characteristics of the components in the different states:
            \n\t– the propellant;\n\t– the liquid; or\n\t– the solid.
            \n\tIf one of these components, which can be a pure substance or a mixture, needs to be classified as flammable, the chemical under pressure shall be classified as flammable in class 2.1. Flammable components are flammable liquids and liquid mixtures, flammable solids and solid mixtures or flammable gases and gas mixtures meeting the following criteria:
            \n\t.1 a flammable liquid is a liquid having a flashpoint of not more than 93°C;
            \n\t.2 a flammable solid is a solid which meets the criteria in 2.4.2.2 of this Code;
            \n\t.3 a flammable gas is a gas which meets the criteria in 2.2.2.1 of this Code;
            \n.2 gases of class 2.3 and gases with a subsidiary risk of 5.1 shall not be used as a propellant in a chemical under pressure;
            \n.3 where the liquid or solid components are classified as dangerous goods of class 6.1, packing groups II or III, or class 8, packing groups II or III, the chemical under pressure shall be assigned a subsidiary risk of class 6.1 or class 8 and the appropriate UN number shall be assigned. Components classified in class 6.1, packing group I, or class 8, packing group I, shall not be used for transport under this proper shipping name;
            \n.4 in addition, chemicals under pressure with components meeting the properties of: class 1, explosives; class 3, liquid desensitized explosives; class 4.1, self-reactive substances and solid desensitized explosives; class 4.2, substances liable to spontaneous combustion; class 4.3, substances which, in contact with water, emit flammable gases; class 5.1, oxidizing substances; class 5.2, organic peroxides; class 6.2, Infectious substances or class 7, Radioactive material, shall not be used for transport under this proper shipping name; 
            \n.5 substances to which PP86 or TP7 are assigned in column 9 and column 14 of the Dangerous Goods List in chapter 3.2 and therefore require air to be eliminated from the vapour space, shall not be used for transport under this UN number but shall be transported under their respective UN numbers as listed in the Dangerous Goods List of chapter 3.2." },
            {363, @".1 This entry applies to engines or machinery, powered by fuels classified as dangerous goods via internal combustion systems or fuel cells(e.g.combustion engines, generators, compressors, turbines, heating units, etc.), except those which are assigned under UN 3166 or UN 3363;
            \n.2 Engines or machinery which are empty of liquid or gaseous fuels and which do not contain other dangerous goods, are not subject to this Code.
            \nNote 1: An engine or machinery is considered to be empty of liquid fuel when the liquid fuel tank has been drained and the engine or machinery cannot be operated due to a lack of fuel.Engine or machinery components such as fuel lines, fuel filters and injectors do not need to be cleaned, drained or purged to be considered empty of liquid fuels. In addition, the liquid fuel tank does not need to be cleaned or purged.
            \nNote 2: An engine or machinery is considered to be empty of gaseous fuels when the gaseous fuel tanks are empty of liquid (for liquefied gases), the positive pressure in the tanks does not exceed 2 bar and the fuel shut-off or isolation valve is closed and secured.
            \n.3 Engines and machinery containing fuels meeting the classification criteria of class 3, shall be consigned under the entries UN No. 3528 ENGINE, INTERNAL COMBUSTION, FLAMMABLE LIQUID POWERED or UN 3528 ENGINE, FUEL CELL, FLAMMABLE LIQUID POWERED or UN 3528 MACHINERY, INTERNAL COMBUSTION, FLAMMABLE LIQUID POWERED or UN 3528 MACHINERY, FUEL CELL, FLAMMABLE LIQUID POWERED, as appropriate.
            \n.4 Engines and machinery containing fuels meeting the classification criteria of class 2.1, shall be consigned under the entries UN 3529 ENGINE, INTERNAL COMBUSTION, FLAMMABLE GAS POWERED or UN 3529 ENGINE, FUEL CELL, FLAMMABLE GAS POWERED or
            UN 3529 MACHINERY, INTERNAL COMBUSTION, FLAMMABLE GAS POWERED or UN 3529 MACHINERY, FUEL CELL, FLAMMABLE GAS POWERED, as appropriate.\nEngines and machinery powered by both a flammable gas and a flammable liquid shall be consigned under the appropriate UN 3529 entry.
            \n.5 Engines and machinery containing liquid fuels meeting the classification criteria of 2.9.3 for environmentally hazardous substances and not meeting the classification criteria of any other class or division, shall be consigned under the entries UN 3530 ENGINE, INTERNAL COMBUSTION or UN 3530 MACHINERY, INTERNAL COMBUSTION, as appropriate.
            \n.6 Engines or machinery may contain other dangerous goods than fuels(e.g.batteries, fire extinguishers, compressed gas accumulators or safety devices) required for their functioning or safe operation without being subject to any additional requirements for these other dangerous goods, unless otherwise specified in this Code.
            \n.7 The engines or machinery are not subject to any other provisions of this Code, except for special provision 972, part 7 and column 16a and 16b in the dangerous goods list, if the following conditions are met:
            \n\t.1 the engine or machinery, including the means of containment containing dangerous goods, shall be in compliance with the construction requirements specified by the competent authority;
            \n\t.2 any valves or openings (e.g.venting devices) shall be closed during transport;
            \n\t.3 the engines or machinery shall be oriented to prevent inadvertent leakage of dangerous goods and secured by means capable of restraining the engines or machinery to prevent any movement during transport which would change the orientation or cause them to be damaged;
            \n\t.4 for UN 3528 and UN 3530:
            \n\t\t– where the engine or machinery contains more than 60 L of liquid fuel and has a capacity of not more than 450 L, the labelling requirements of 5.2.2 shall apply;
            \n\t\t– where the engine or machinery contains more than 60 L of liquid fuel and has a capacity of more than 450 L but not more than 3,000 L, it shall be labelled on two opposing sides in accordance with 5.2.2;
            \n\t\t– where the engine or machinery contains more than 60 L of liquid fuel and has a capacity of more than 3,000 L, it shall be placarded on two opposing sides in accordance with 5.3.1.1.2;
            \n\t\t– for UN 3530, in addition the marking requirements of 5.2.1.6 apply.
            \n\t.5 for UN 3529:
            \n\t\t– where the fuel tank of the engine or machinery has a water capacity of not more than 450 L, the labelling requirements of 5.2.2 shall apply;
            \n\t\t– where the fuel tank of the engine or machinery has a water capacity of more than 450 L but not more than 1,000 L, it shall be labelled on two opposing sides in accordance with 5.2.2;
            \n\t\t– where the fuel tank of the engine or machinery has a water capacity of more than 1,000 L, it shall be placarded on two opposing sides in accordance with 5.3.1.1.2;
            \n\t.6 a transport document in accordance with 5.4 is required and shall contain the following additional statement “Transport in accordance with special provision 363”."},
            {364, @"This article may only be transported under the provisions of chapter 3.4 if, as presented for transport,the package is capable of passing the test in accordance with test series 6(d) of part I of the Manualof Tests and Criteria as determined by the competent authority."},
            {365, @"For manufactured instruments and articles containing mercury, see UN 3506."},
            {366, @"Manufactured instruments and articles containing not more than 1 kg of mercury are not subject tothe provisions of this Code."},
            {367, @"For the purposes of documentation and package marking:
            \nThe proper shipping name “PAINT RELATED MATERIAL” may be used for consignments of packages containing “PAINT” and “PAINT RELATED MATERIAL” in the same package;
            \nThe proper shipping name “PAINT RELATED MATERIAL, CORROSIVE, FLAMMABLE” may be used for consignments of packages containing “PAINT, CORROSIVE, FLAMMABLE” and “PAINT RELATED MATERIAL, CORROSIVE, FLAMMABLE” in the same package;
            \nThe proper shipping name “PAINT RELATED MATERIAL, FLAMMABLE, CORROSIVE” may be used for consignments of packages containing “PAINT, FLAMMABLE, CORROSIVE” and “PAINT RELATED MATERIAL, FLAMMABLE, CORROSIVE” in the same package; and
            \nThe proper shipping name “PRINTING INK RELATED MATERIAL” may be used for consignments of packages containing “PRINTING INK” and “PRINTING INK RELATED MATERIAL” in the same package."},
            {368, @"In the case of non-fissile or fissile-excepted uranium hexafluoride, the material shall be classified under UN 3507 or UN 2978."},
            {369, @"In accordance with 2.0.3.5, this radioactive material in an excepted package possessing toxic and corrosive properties is classified in class 6.1 with radioactivity and corrosivity subsidiary risks.
            \nUranium hexafluoride may be classified under this entry only if the conditions of 2.7.2.4.1.2, 2.7.2.4.1.5, 2.7.2.4.5.2 and, for fissile-excepted material, of 2.7.2.3.5 are met.
            \nIn addition to the provisions applicable to the transport of class 6.1 substances with a corrosivity subsidiary risk, the provisions of 5.1.3.2, 5.1.5.2.2, 5.1.5.4.1.2, 7.1.4.5.9, 7.1.4.5.10, 7.1.4.5.12, and 7.8.4.1 to 7.8.4.6 shall apply.
            \nNo class 7 label is required to be displayed."},
            {370, @"This entry applies to:
            \n– ammonium nitrate with more than 0.2% combustible substances, including any organic substance calculated as carbon, to the exclusion of any added substance; and
            \n– ammonium nitrate with not more than 0.2% combustible substances, including any organic substance calculated as carbon, to the exclusion of any added substance, that gives a positive result when tested in accordance with test series 2 (see Manual of Tests and Criteria, part I). See also UN 1942."},
            {371, @".1 This entry also applies to articles, containing a small pressure receptacle with a release device. Such articles shall comply with the following requirements:
            \n\t.1 the water capacity of the pressure receptacle shall not exceed 0.5 L and the working pressure shall not exceed 25 bar at 15°C;
            \n\t.2 the minimum burst pressure of the pressure receptacle shall be at least four times the pressure of the gas at 15°C;
            \n\t.3 each article shall be manufactured in such a way that unintentional firing or release is avoided under normal conditions of handling, packing, transport and use.This may be fulfilled by an additional locking device linked to the activator;
            \n\t.4 each article shall be manufactured in such a way as to prevent hazardous projections of the pressure receptacle or parts of the pressure receptacle;
            \n\t.5 each pressure receptacle shall be manufactured from material which will not fragment upon rupture;
            \n\t.6 the design type of the article shall be subjected to a fire test.For this test, the provisions of paragraphs 16.6.1.2 except subparagraph (g), 16.6.1.3.1 to 16.6.1.3.6, 16.6.1.3.7 (b) and 16.6.1.3.8 of the Manual of Tests and Criteria shall be applied. It shall be demonstrated that the article relieves its pressure by means of a fire degradable seal or other pressure relief device, in such a way that the pressure receptacle will not fragment and that the article or fragments of the article do not rocket more than 10 m; and
            \n\t.7 the design type of the article shall be subjected to the following test. A stimulating mechanism shall be used to initiate one article in the middle of the packaging. There shall be no hazardous effects outside the package such as disruption of the package, metal fragments or a receptacle which passes through the packaging.
            \n.2 The manufacturer shall produce technical documentation of the design type, manufacture as well as the tests and their results. The manufacturer shall apply procedures to ensure that articles produced in series are made of good quality, conform to the design type and are able to meet the requirements in .1. The manufacturer shall provide such information to the competent authority on request."},
            {372, @"This entry applies to asymmetric capacitors with an energy storage capacity greater than 0.3 Wh. Capacitors with an energy storage capacity of 0.3 Wh or less are not subject to the provisions of this Code.
            \nEnergy storage capacity means the energy stored in a capacitor, as calculated according to the following equation:
             \nWh = 1/2Cn(Ur^2-Ul^2) / 3,600
            \nusing the nominal capacitance(CN), rated voltage(UR) and rated lower limit voltage(UL).
            \nAll asymmetric capacitors to which this entry applies shall meet the following conditions:
            \n.1 capacitors or modules shall be protected against short circuit;
            \n.2 capacitors shall be designed and constructed to safely relieve pressure that may build up in use, through a vent or a weak point in the capacitor casing.Any liquid which is released upon venting shall be contained by packaging or by equipment in which a capacitor is installed;
            \n.3 capacitors manufactured after 31 December 2015 shall be marked with the energy storage capacity in Wh;
            \n.4 capacitors containing an electrolyte meeting the classification criteria of any class or division of dangerous goods shall be designed to withstand a 95 kPa pressure differential;
            \nCapacitors containing an electrolyte not meeting the classification criteria of any class or division of dangerous goods, including when configured in a module or when installed in equipment are not subject to other provisions of this Code.Capacitors containing an electrolyte meeting the classification criteria of any class or division of dangerous goods, with an energy storage capacity of 20 Wh or less, including when configured in a module, are not subject to other provisions of this Code when the capacitors are capable of withstanding a 1.2 m drop test unpackaged on an unyielding surface without loss of contents.
            \nCapacitors containing an electrolyte meeting the classification criteria of any class or division of dangerous goods that are not installed in equipment and with an energy storage capacity of more than 20 Wh are subject to this Code.
            \nCapacitors installed in equipment and containing an electrolyte meeting the classification criteria of any class or division of dangerous goods, are not subject to other provisions of these regulations provided that the equipment is packaged in a strong outer packaging constructed of suitable material, and of adequate strength and design, in relation to the packaging’s intended use and in such a manner as to prevent accidental functioning of capacitors during transport.Large robust equipment containing capacitors may be offered for transport unpackaged or on pallets when capacitors are afforded equivalent protection by the equipment in which they are contained.
            \nNote: Notwithstanding the provisions of this special provision, nickel-carbon asymmetric capacitors containing class 8 alkaline electrolytes shall be transported as UN 2795, BATTERIES, WET, FILLED WITH ALKALI electric storage."},
            {373, @"Neutron radiation detectors containing non-pressurized boron trifluoride gas may be transported under this entry provided that the following conditions are met:
            \n.1 Each radiation detector shall meet the following conditions:
            \n\t.1 the pressure in each detector shall not exceed 105 kPa absolute at 20°C;
            \n\t.2 the amount of gas shall not exceed 13 g per detector;
            \n\t.3 each detector shall be manufactured under a registered quality assurance programme;
            \n\tNote: The application of ISO 9001:2008 may be considered acceptable for this purpose.
            \n\t.4 each neutron radiation detector shall be of welded metal construction with brazed metal to ceramic feed through assemblies. These detectors shall have a minimum burst pressure of 1800 kPa as demonstrated by design type qualification testing; and
            \n\t.5 each detector shall be tested to a 1 × 10–10 cm3/s leak tightness standard before filling.
            \n.2 Radiation detectors transported as individual components shall be transported as follows:
            \n\t.1 detectors shall be packed in a sealed intermediate plastics liner with sufficient absorbent or adsorbent material to absorb or adsorb the entire gas contents;
            \n\t.2 they shall be packed in strong outer packaging.The completed package shall be capable of withstanding a 1.8 m drop test without leakage of gas contents from detectors; and
            \n\t.3 the total amount of gas from all detectors per outer packaging shall not exceed 52 g.
            \n.3 Completed neutron radiation detection systems containing detectors meeting the conditions of .1 shall be transported as follows:
            \n\t.1 the detectors shall be contained in a strong sealed outer casing;
            \n\t.2 the casing shall contain sufficient absorbent or adsorbent material to absorb or adsorb the entire gas contents; and
            \n\t.3 the completed systems shall be packed in strong outer packagings capable of withstanding a 1.8 m drop test without leakage unless a system’s outer casing affords equivalent protection.
            \nPacking instruction P200 of 4.1.4.1 is not applicable.
            \nThe transport document shall include the statement “Transport in accordance with special provision 373”.
            \nNeutron radiation detectors containing not more than 1 g of boron trifluoride, including those with solder glass joints, are not subject to this Code provided they meet the requirements in paragraph .1 and are packed in accordance with paragraph .2. Radiation detection systems containing such detectors are not subject to this Code provided they are packed in accordance with paragraph .3.
            \nNeutron radiation detectors shall be stowed in accordance with stowage Category A."},
            {376, @"Lithium ion cells or batteries and lithium metal cells or batteries identified as being damaged or defective such that they do not conform to the type tested according to the applicable provisions of the Manual of Tests and Criteria shall comply with the requirements of this special provision.
            \nFor the purposes of this special provision, these may include, but are not limited to:
            \n– Cells or batteries identified as being defective for safety reasons;
            \n– Cells or batteries that have leaked or vented;
            \n– Cells or batteries that cannot be diagnosed prior to transport; or
            \n– Cells or batteries that have sustained physical or mechanical damage.
            \nNote: In assessing a battery as damaged or defective, the type of battery and its previous use and misuse shall be taken into account.
            \nCells and batteries shall be transported according to the provisions applicable to UN 3090, UN 3091, UN 3480 and UN 3481, except special provision 230 and as otherwise stated in this special provision.
            \nPackages shall be marked “DAMAGED/DEFECTIVE LITHIUM-ION BATTERIES” or “DAMAGED/ DEFECTIVE LITHIUM METAL BATTERIES”, as applicable.
            \nCells and batteries shall be packed in accordance with packing instructions P908 of 4.1.4.1 or LP904 of 4.1.4.3, as applicable.
            \nCells and batteries liable to rapidly disassemble, dangerously react, produce a flame or a dangerous evolution of heat or a dangerous emission of toxic, corrosive or flammable gases or vapours under normal conditions of transport shall not be transported except under conditions specified by the competent authority."},
            {377, @"Lithium ion and lithium metal cells and batteries and equipment containing such cells and batteries transported for disposal or recycling, either packed together with or packed without non-lithium    batteries, may be packaged in accordance with packing instruction P909 of 4.1.4.1.
            \nThese cells and batteries are not subject to the requirements of section 2.9.4.
            \nPackages shall be marked “LITHIUM BATTERIES FOR DISPOSAL” or “LITHIUM BATTERIES FOR RECYCLING”.
            \nIdentified damaged or defective batteries shall be transported in accordance with special provision 376 and packaged in accordance with P908 of 4.1.4.1 or LP904 of 4.1.4.3, as applicable."},
            { 378, @"Radiation detectors containing this gas in non-refillable pressure receptacles not meeting the
            requirements of chapter 6.2 and packing instruction P200 of 4.1.4.1 may be transported under this
            entry provided:
            .1 The working pressure in each receptacle does not exceed 50 bar;
            .2 The receptacle capacity does not exceed 12 litres; 3
            .3 Each receptacle has a minimum burst pressure of at least 3 times the working pressure when a
            relief device is fitted and at least 4 times the working pressure when no relief device is fitted;
            .4 Each receptacle is manufactured from material which will not fragment upon rupture;
            .5 Each detector is manufactured under a registered quality assurance programme;
            Note: ISO 9001:2008 may be used for this purpose.
            .6 Detectors are transported in strong outer packagings.The complete package shall be capable
            of withstanding a 1.2 metre drop test without breakage of the detector or rupture of the outer
            packaging. Equipment that includes a detector shall be packed in a strong outer packaging
            unless the detector is afforded equivalent protection by the equipment in which it is contained;
            and
            .7 The transport document includes the following statement “Transport in accordance with special
            provision 378”.
            Radiation detectors, including detectors in radiation detection systems, are not subject to any other
            requirements of this Code if the detectors meet the requirements in .1 to .6 above and the capacity
            of detector receptacles does not exceed 50 ml."},
            {
            379, @"Anhydrous ammonia adsorbed on a solid or absorbed in a solid contained in ammonia dispensing
            systems or receptacles intended to form part of such systems are not subject to the other provisions
            of this Code if the following conditions are observed:
            .1 The adsorption or absorption presents the following properties:
            .1 the pressure at a temperature of 20°C in the receptacle is less than 0.6 bar;
            .2 the pressure at a temperature of 35°C in the receptacle is less than 1 bar;
            .3 the pressure at a temperature of 85°C in the receptacle is less than 12 bar;
            .2 The adsorbent or absorbent material shall not have dangerous properties listed in classes 1 to
            8;
            .3 The maximum contents of a receptacle shall be 10 kg of ammonia; and
            .4 Receptacles containing adsorbed or absorbed ammonia shall meet the following conditions:
            .1 receptacles shall be made of a material compatible with ammonia as specified in
            ISO 11114-1:2012;
            .2 receptacles and their means of closure shall be hermetically sealed and able to contain the
            generated ammonia;
            .3 each receptacle shall be able to withstand the pressure generated at 85°C with a volumetric
            expansion no greater than 0.1%;
            .4 each receptacle shall be fitted with a device that allows for gas evacuation once pressure
            exceeds 15 bar without violent rupture, explosion or projection; and
            .5 each receptacle shall be able to withstand a pressure of 20 bar without leakage when the
            pressure relief device is deactivated.
            When transported in an ammonia dispenser, the receptacles shall be connected to the dispenser in
            such a way that the assembly is guaranteed to have the same strength as a single receptacle.
            The properties of mechanical strength mentioned in this special provision shall be tested using a
            prototype of a receptacle and/or dispenser filled to nominal capacity, by increasing the temperature
            until the specified pressures are reached.
            The test results shall be documented, shall be traceable and shall be communicated to the relevant
            authorities upon request."},
            { 380, @"If a vehicle is powered by a flammable liquid and a flammable gas internal combustion engine, it shall
            be assigned to UN 3166 VEHICLE, FLAMMABLE GAS POWERED."},
            {381, @"Large packagings conforming to the packing group III performance level used in accordance with
            packing instruction LP02 of 4.1.4.3, as prescribed in the IMDG Code(amendment 37-14), may be
            used until 31 December 2022."},
            {382, @"Polymeric beads may be made from polystyrene, poly(methyl methacrylate) or other polymeric
            material.When it can be demonstrated that no flammable vapour, resulting in a flammable atmosphere,
            is evolved according to test U1 (Test method for substances liable to evolve flammable vapours) of
            part III, subsection 38.4.4 of the Manual of Tests and Criteria, polymeric beads, expandable, need
            not be classified under this UN number. This test should only be performed when declassification of
            a substance is considered."},
            {383, @"Table tennis balls manufactured from celluloid are not subject to this Code where the net mass of
            each table tennis ball does not exceed 3.0 g and the total net mass of table tennis balls does not
            exceed 500 g per package."},
            {384, @"The label to be used is Model No. 9A, see 5.2.2.2.2. However, for placarding of cargo transport units, the placard shall correspond to Model No. 9.
            Note: The class 9 label(Model No. 9) may continue to be used until 31 December 2018."},
            {385, @"This entry applies to vehicles powered by flammable liquid or gas internal combustion engines or
            fuel cells.
            Hybrid electric vehicles powered by both an internal combustion engine and wet batteries, sodium
            batteries, lithium metal batteries or lithium ion batteries, transported with the batteries installed shall
            be consigned under this entry.Vehicles powered by wet batteries, sodium batteries, lithium metal
            batteries or lithium ion batteries, transported with the batteries installed, shall be consigned under
            the entry UN No. 3171 BATTERY-POWERED VEHICLE (see special provision 240).
            For the purpose of this special provision, vehicles are self-propelled apparatus designed to carry one
            or more persons or goods. Examples of such vehicles are cars, motorcycles, trucks, locomotives,
            scooters, three- and four-wheeled vehicles or motorcycles, lawn tractors, self-propelled farming
            and construction equipment, boats and aircraft.
            Dangerous goods such as batteries, air bags, fire extinguishers, compressed gas accumulators,
            safety devices and other integral components of the vehicle that are necessary for the operation of
            the vehicle or for the safety of its operator or passengers, shall be securely installed in the vehicle
            and are not otherwise subject to this Code.
            386 When substances are stabilized by temperature control, the provisions of 7.3.7 apply. When chemical
            stabilization is employed, the person offering the packaging, IBC or tank for transport shall ensure
            that the level of stabilization is sufficient to prevent the substance in the packaging, IBC or tank from
            dangerous polymerization at a bulk mean temperature of 50°C, or, in the case of a portable tank,
            45°C. Where chemical stabilization becomes ineffective at lower temperatures within the anticipated
            duration of transport, temperature control is required. In making this determination factors to be taken
            into consideration include, but are not limited to, the capacity and geometry of the packaging, IBC
            or tank and the effect of any insulation present, the temperature of the substance when offered for
            transport, the duration of the journey and the ambient temperature conditions typically encountered
            in the journey (considering also the season of year), the effectiveness and other properties of the
            stabilizer employed, applicable operational controls imposed by regulation (e.g. requirements to
            protect from sources of heat, including other cargo transported at a temperature above ambient)
            and any other relevant factors.”"},

            { 900, @"The transport of the following substances is prohibited:
            \n\tAMMONIUM HYPOCHLORITE
            \n\tAMMONIUM NITRATE liable to self-heating sufficient to initiate decomposition
            \n\tAMMONIUM NITRITES and mixtures of an inorganic nitrite with an ammonium salt
            \n\tCHLORIC ACID, AQUEOUS SOLUTION with more than 10% chloric acid
            \n\tETHYL NITRITE pure
            \n\tHYDROCYANIC ACID, AQUEOUS SOLUTION (HYDROGEN CYANIDE, AQUEOUS SOLUTION)
            with more than 20% hydrogen cyanide
            \n\tHYDROGEN CHLORIDE, REFRIGERATED LIQUID
            \n\tHYDROGEN CYANIDE SOLUTION, IN ALCOHOL with more than 45% hydrogen cyanide
            \n\tMERCURY OXYCYANIDE pure
            \n\tMETHYL NITRITE
            \n\tPERCHLORIC ACID with more than 72% acid, by mass
            \n\tSILVER PICRATE, dry or wetted with less than 30% water by mass
            \n\tZINC AMMONIUM NITRITE
            \nSee also special provisions 349, 350, 351, 352 and 353." },
            {903, @"HYPOCHLORITE MIXTURES with 10% or less available CHLORINE are not subject to the provisions
            of this Code." },
            {904, @"The provisions of this Code, except for the marine pollution aspects, do not apply to these substances
            if they are completely miscible with water, except when transported in receptacles with a capacity
            greater than 250 L and in tanks." },
            {905, @"May only be shipped as an 80% solution in TOLUENE. The pure product is shock-sensitive
            and decomposes with explosive violence and the possibility of detonation when heated under
            confinement. Can be ignited by impact." },
            {907, @"The consignment shall be accompanied by a certificate from a recognized authority stating:
            \n– moisture content;
            \n– fat content;
            \n– details of anti-oxidant treatment for meals older than 6 months(for UN 2216 only);
            \n– anti-oxidant concentration at the time of shipment, which must exceed 100 mg/kg(for UN 2216 only);
            \n– packing, number of bags and total mass of the consignment;
            \n– temperature of fish meal at the time of despatch from the factory;
            \n– date of production.
            \nNo weathering/curing is required prior to loading. Fish meal under UN 1374 shall have been
            weathered for not less than 28 days before shipment.
            \nWhen fish meal is packed into containers, the containers shall be packed in such a way that the
            free air space has been restricted to the minimum." },
            {912, @"This entry also covers solutions in water with concentrations above 70%." },
            {916, @"The provisions of this Code do not apply to this substance when:
            – mechanically produced, with a particle size of 53 microns or greater; or
            – chemically produced, with a particle size of 840 microns or greater." },
            {917, @"Scrap with rubber content below 45% or exceeding 840 microns and fully vulcanized hard rubber
            are not subject to the provisions of this Code." },
            {920, @"Bars, ingots or sticks are not subject to the provisions of this Code." },
            {921, @"Zirconium, dry, 254 microns or thicker is not subject to the provisions of this Code." },
            {922, @"LEAD PHOSPHITE, DIBASIC which is accompanied by the certificate from the shipper stating that
            the substance, as offered for shipment, has been stabilized in such a way that it does not possess
            the properties of class 4.1 is not subject to the provisions of this Code." },
            {923, @"The temperature shall be checked regularly." },
            {925, @"The provisions of this Code do not apply to:
            \n– non-activated carbon blacks of mineral origin;
            \n– a consignment of carbon if it passes the tests for self-heating substances as reflected in
            the Manual of Tests and Criteria (see 33.3.1.3.3), and is accompanied by a certificate from a
            laboratory accredited by the competent authority, stating that the product to be loaded has
            been correctly sampled by trained staff from that laboratory and that the sample was correctly
            tested and has passed the test; and
            \n– carbons made by a steam activation process." },
            {926, @"This substance shall preferably have been weathered for not less than one month before shipment
            unless a certificate from a person recognized by the competent authority of the country of shipment
            states a maximum moisture content of 5%." },
            {927, @"p -Nitrosodimethylaniline, wetted with more than 50% water is not subject to the provisions of this
            Code." },
            {928, @"The provisions of this Code shall not apply to:
            \n– fish meal when acidified and wetted with more than 40% water, by mass, irrespective of other factors;
            \n– consignments of fish meal which are accompanied by a certificate issued by a recognized
            competent authority of the country of shipment or other recognized authority stating that the
            product has no self-heating properties when transported in packaged form; or
            \n– fish meal manufactured from “white” fish with a moisture content of not more than 12% and a fat
            content of not more than 5% by mass." },
            {929, @"If satisfied, as a result of tests, that such relaxation is justified, the competent authority may permit:
            \n– the seed cakes described as “SEED CAKE, containing vegetable oil (a) mechanically expelled
            seeds, containing more than 10% of oil or more than 20% of oil and moisture combined” to
            be transported under conditions governing “SEED CAKE, containing vegetable oil (b) solvent
            extractions and expelled seeds, containing not more than 10% of oil and, when the amount of
            moisture is higher than 10% not more than 20% of oil and moisture combined”, and
            \n– the seed cakes described as “SEED CAKE, containing vegetable oil (b) solvent extractions and
            expelled seeds, containing not more than 10% of oil and, when the amount of moisture is higher
            than 10% not more than 20% of oil and moisture combined” to be transported under conditions
            governing SEED CAKE, UN 2217.
            Certificates from the shipper shall state oil content and moisture content and shall accompany the
            shipment."},
            {930, @"All pesticides can only be carried under the provisions of this class if accompanied by a certificate
            supplied by the shipper stating that, when in contact with water, it is not combustible and does not
            show tendency to autoignition, and that the mixture of gases evolved is not flammable.Otherwise,
            the provisions of class 4.3 shall be applicable."},
            {931, @"A consignment of this substance which is accompanied by a declaration from the shipper stating
            that it has no self-heating properties is not subject to the provisions of this Code." },
            {932, @"Requires a certificate from the maker or shipper, stating that the shipment was stored under cover,
            but in the open air, in the size in which it was packaged, for not less than 3 days prior to shipment." },
            {934, @"Requires the percentage range of calcium carbide impurity to be shown on the shipping documents." },
            {935, @"Substances which do not evolve flammable gases when wet, which are accompanied by a certificate
            from the shipper stating that the substance, as offered for shipment, does not evolve flammable
            gases when wet, are not subject to the provisions of this Code." },
            {937, @"The solid hydrated form of this substance is not subject to the provisions of this Code." },
            {939, @"A consignment of this substance that is accompanied by a shipper’s certificate stating that it does
            not contain more than 0.05% maleic anhydride is not subject to the provisions of this Code." },
            {942, @"The concentration and temperature of the solution at the time of loading, its percentage of
            combustible material and of chlorides as well as the contents of free acid shall be certified." },
            {943, @"Water-activated articles shall bear a subsidiary risk label of class 4.3." },
            {945, @"Stabilization of fish meal shall be achieved to prevent spontaneous combustion by effective application
            of between 400 and 1000 mg/kg(ppm) ethoxyquin, or liquid BHT(butylated hydroxytoluene) or
            between 1000 and 4000 mg/kg(ppm) BHT in powder form at the time of production.The said
            application shall occur no longer than twelve months prior to shipment." },
            {946, @"Requires certification from the shipper that the substance is not of class 4.2." },
            {948, @"These substances may be transported in bulk in cargo transport units only if their melting point is
            75°C or above." },
            {951, @"Bulk container shall be hermetically sealed and under a nitrogen blanket." },
            {952, @"UN 1942 may be transported in bulk container if approved by the competent authority." },
            {954, @"The provisions of this Code shall not apply to consignments of compressed baled hay with a
            moisture content of less than 14% shipped in closed cargo transport units and accompanied by a
            certificate from the shipper stating that the product does not present any class 4.1, UN 1327, hazard
            in transport and that its moisture content is less than 14%." },
            {955, @"If a viscous substance and its packaging fulfils the provisions of 2.3.2.5, the packing provisions of
            chapter 4.1, the marking and labelling provisions of chapter 5.2 and the package testing provisions
            of chapter 6.1 are not applicable." },
            {958, @"This entry also covers articles, such as rags, cotton waste, clothing or sawdust, containing
            polychlorinated biphenyls, polyhalogenated biphenyls or polyhalogenated terphenyls where no free
            visible liquid is present." },
            {959, @"Waste aerosols authorized for transport under special provision 327 shall only be transported on
            short international voyages.Long international voyages are authorized only with the approval of
            the competent authority.Packagings shall be marked and labelled and cargo transport units shall
            be marked and placarded for appropriate sub-division of class 2 and, if applicable, the subsidiary
            risk(s)." },
            {960, @"Not subject to the provisions of this Code but may be subject to provisions governing the transport
            of dangerous goods by other modes." },
            {961, @"Vehicles are not subject to the provisions of this Code if any of the following conditions are met:
            \n.1 vehicles are stowed on the vehicle, special category and ro-ro spaces or on the weather deck of
            a ro-ro ship or a cargo space designated by the Administration (flag State) in accordance with
            SOLAS 74, chapter II-2, regulation 20 as specifically designed and approved for the carriage of
            vehicles, and there are no signs of leakage from the battery, engine, fuel cell, compressed gas
            cylinder or accumulator, or fuel tank when applicable.When packed in a cargo transport unit
            the exception does not apply to container cargo spaces of a ro-ro ship.
            In addition, for vehicles powered solely by lithium batteries and hybrid electric vehicles powered
            by both an internal combustion engine and lithium metal or ion batteries, the lithium batteries shall
            meet the provisions of 2.9.4, except that 2.9.4.1 does not apply when pre-production prototype
            batteries or batteries of a small production run, consisting of not more than 100 batteries,
            are installed in the vehicle and the vehicle is manufactured and approved according to the
            provisions applied in the country of manufacture or country of use.Where a lithium battery
            installed in a vehicle is damaged or defective, the battery shall be removed.
            \n.2 vehicles powered by a flammable liquid fuel with a flashpoint of 38°C or above, there are no 3
            leaks in any portion of the fuel system, the fuel tank(s) contains 450 L of fuel or less and installed
            batteries are protected from short-circuit;
            \n.3 vehicles powered by a flammable liquid fuel with a flashpoint less than 38°C, the fuel tank(s) are
            empty and installed batteries are protected from short circuit.Vehicles are considered to be
            empty of flammable liquid fuel when the fuel tank has been drained and the vehicles cannot be
            operated due to a lack of fuel. Engine components such as fuel lines, fuel filters and injectors
            do not need to be cleaned, drained or purged to be considered empty.The fuel tank does not
            need to be cleaned or purged;
            \n.4 vehicles powered by a flammable gas(liquefied or compressed), the fuel tank(s) are empty and
            the positive pressure in the tank does not exceed 2 bar, the fuel shut-off or isolation valve is
            closed and secured, and installed batteries are protected from short circuit;
            \n.5 vehicles solely powered by a wet or dry electric storage battery or a sodium battery, and the
            battery is protected from short circuit." },
            {962, @"Vehicles, not meeting the conditions of special provision 961 shall be assigned to class 9 and shall
            meet the following requirements:
            \n.1 vehicles shall not show signs of leakage from batteries, engines, fuel cells, compressed gas
            cylinders or accumulators, or fuel tank(s) when applicable;
            \n.2 for flammable liquid powered vehicles the fuel tank(s) containing the flammable liquid shall not
            be more than one fourth full and in any case the flammable liquid shall not exceed 250 L unless
            otherwise approved by the competent authority;
            \n.3 for flammable gas powered vehicles, the fuel shut-off valve of the fuel tank(s) shall be securely
            closed;
            \n.4 installed batteries shall be protected from damage, short circuit, and accidental activation
            during transport.Lithium batteries shall meet the provisions of 2.9.4, except that 2.9.4.1
            does not apply when pre-production prototype batteries or batteries of a small production
            run, consisting of not more than 100 batteries, are installed in the vehicle and the vehicle is
            manufactured and approved according to the provisions applied in the country of manufacture
            or country of use. Where a lithium battery installed in a vehicle is damaged or defective, the
            battery shall be removed and transported according to SP 376, unless otherwise approved by
            the competent Authority.
            The provisions of this Code relevant to marking, labelling, placarding and marine pollutants shall not
            apply." },
            {963, @"Nickel-metal hydride button cells or nickel-metal hydride cells or batteries packed with or contained
            in equipment are not subject to the provisions of this Code.
            All other nickel-metal hydride cells or batteries shall be securely packed and protected from short
            circuit.They are not subject to other provisions of this Code provided that they are loaded in a cargo
            transport unit in a total quantity of less than 100 kg gross mass.When loaded in a cargo transport unit
            in a total quantity of 100 kg gross mass or more, they are not subject to other provisions of this Code
            except those of 5.4.1, 5.4.3 and columns 16a and 16b of the Dangerous Goods List in chapter 3.2." },
            { 964, @"This substance is not subject to the provisions of this Code when transported in non-friable prills or
            granules form and if it passes the test for oxidizing solid substances as reflected in the Manual of
            Tests and Criteria (see 34.4.1) and is accompanied by a certificate from a laboratory accredited by a
            competent authority, stating that the product has been correctly sampled by trained staff from the
            laboratory and that the sample was correctly tested and has passed the test." },
            {965, @".1 When transported in cargo transport units, the cargo transport units shall provide an adequate
            exchange of air in the unit(e.g.by using a ventilated container, open-top container or container
            in one door off operation) to prevent the build-up of an explosive atmosphere.Alternatively,
            these entries shall be transported under temperature control in refrigerated cargo transport
            units that comply with the provisions of 7.3.7.6. When cargo transport units with venting devices
            are used, these devices shall be kept clear and operable.When mechanical devices are used
            for ventilation, they shall be explosion-proof to prevent ignition of flammable vapours from the
            substances.
            \n.2 The provisions of .1 do not apply if:
            \n.1 the substance is packed in hermetically sealed packagings or IBCs, which conform to
            packing group II performance level for liquid dangerous goods according to the provisions
            of 6.1 or 6.5, respectively; and
            \n.2 the marked hydraulic test pressure exceeds 1.5 times the total gauge pressure in the
            packagings or IBCs determined at 55°C for the respective filling goods according to
            4.1.1.10.1.
            \n.3 Where the substance is loaded in closed cargo transport units, the provisions of 7.3.6.1 shall be
            met.
            \n.4 Cargo transport units shall be marked with a warning mark including the words “CAUTION –
            MAY CONTAIN FLAMMABLE VAPOUR” with lettering not less than 25 mm high.This mark shall
            be affixed at each access point in a location where it will be easily seen by persons prior to
            opening or entering the cargo transport unit and shall remain on the cargo transport unit until
            the following provisions are met:
            \n.1 the cargo transport unit has been completely ventilated to remove any hazardous
            concentration of vapour or gas;
            \n.2 the immediate vicinity of the cargo transport unit is clear of any source of ignition; and
            \n.3 the goods have been unloaded." },
            {966, @"Sheeted bulk containers(BK1) are only permitted in accordance with 4.3.3." },
            {967, @"Flexible bulk containers(BK3) are only permitted in accordance with 4.3.4." },
            {968, @"This entry shall not be used for sea transport.Discarded packaging shall meet the requirements of
            4.1.1.11." },
            {969, @"Substances classified in accordance to 2.9.3 are subject to the provisions for marine pollutants.
            Substances which are transported under UN 3077 and UN 3082 but which do not meet the criteria
            of 2.9.3 (see 2.9.2.2) are not subject to the provisions for marine pollutants.However for substances
            that are identified as marine pollutants in this Code (see Index) but which no longer meet the criteria
            of 2.9.3, the provisions of 2.10.2.6 apply." },
            { 971, @"Battery powered equipment may only be transported provided that the battery shows no sign of
            leakage and is protected from short-circuit.In this case, no other provisions of this Code apply." },
            {972, @"Lithium batteries shall meet the provisions of 2.9.4, except that 2.9.4.1 does not apply when pre-production prototype batteries or batteries of a small production run, consisting of not more than 100 batteries, are installed in the engine or machinery. Where a lithium battery installed in an engine or machinery is damaged or defective, the battery shall be removed."}
            };
#endregion

    }
}
