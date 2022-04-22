using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EasyJob_ProDG.Data.Info_data;

namespace EasyJob_ProDG.Model.Cargo
{
    partial class Segregation
    {
        public static void AssignSegregatorClassesAndGroups(IEnumerable<Dg> dgList)
        {
            foreach (var dg in dgList)
            {
                foreach (var sscode in dg.SegregationSGList)
                {
                    switch (sscode)
                    {
                        case "SG1": //For packages carrying a subsidiary risk label of class 1, segregation as for class 1, division 1.3.
                            foreach (string s in dg.DgSubclassArray)
                            {
                                if (s.StartsWith("1"))
                                {
                                    dg.SegregatorClass = "1.3";
                                    dg.SegregatorException = new SegregatorException("1", 5);
                                }
                            }
                            break;
                        case "SG2": //Segregation as for class 1.2G.
                            dg.SegregatorClass = "1.2G";
                            break;
                        case "SG3": //Segregation as for class 1.3G.
                            dg.SegregatorClass = "1.3G";
                            break;
                        case "SG4": //Segregation as for class 2.1.
                            dg.SegregatorClass = "2.1";
                            break;
                        case "SG5": // Segregation as for class 3.
                            dg.SegregatorClass = "3";
                            break;
                        case "SG6": //Segregation as for class 5.1.
                            dg.SegregatorClass = "5.1";
                            break;
                        case "SG68": //If flashpoint 60°C c.c.or below, segregation as for class 3 but “away from” class 4.1.
                            if (dg.FlashPointDouble <= 60 || Math.Abs(dg.FlashPointDouble - 9999) < 10)
                            {
                                dg.SegregatorClass = "3";
                                dg.SegregatorException = new SegregatorException("4.1", 1);
                            }
                            break;
                        case "SG69": //For AEROSOLS with a maximum capacity of 1 L: segregation as for class 9.
                            //Stow “separated from” class 1 except for division 1.4.
                            //For AEROSOLS with a capacity above 1 L: segregation as for the appropriate subdivision of class 2.
                            //For WASTE AEROSOLS: segregation as for the appropriate subdivision of class 2.
                            if (dg.IsMax1L)
                            {
                                dg.SegregatorClass = "9";
                                dg.SegregatorException = new SegregatorException("1", 2);
                            }
                            else
                            {
                                dg.SegregatorClass = null;
                                dg.SegregatorException = null;
                            }
                            break;
                        case "SG74": //Segregation as for 1.4G.
                            dg.SegregatorClass = "1.4G";
                            break;
                        case "SG76": //Segregation as for class 7.
                            dg.SegregatorClass = "7";
                            break;
                        case "SG77": //Segregation as for class 8. However, in relation to class 7, no segregation needs to be applied.
                            dg.SegregatorClass = "8";
                            dg.SegregatorException = new SegregatorException("7", 0);
                            break;

                        #region Segregation Groups

                        case "SGG1":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[1];
                            break;
                        case "SGG1a":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[19];
                            break;
                        case "SGG2":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[2];
                            break;
                        case "SGG3":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[3];
                            break;
                        case "SGG4":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[4];
                            break;
                        case "SGG5":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[5];
                            break;
                        case "SGG6":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[6];
                            break;
                        case "SGG7":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[7];
                            break;
                        case "SGG8":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[8];
                            break;
                        case "SGG9":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[9];
                            break;
                        case "SGG10":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[10];
                            break;
                        case "SGG11":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[11];
                            break;
                        case "SGG12":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[12];
                            break;
                        case "SGG13":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[13];
                            break;
                        case "SGG14":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[14];
                            break;
                        case "SGG15":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[15];
                            break;
                        case "SGG16":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[16];
                            break;
                        case "SGG17":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[17];
                            break;
                        case "SGG18":
                            dg.SegregationGroup = IMDGCode.SegregationGroups[18];
                            break;
                            #endregion
                    }
                }
            }
        }




        /// <summary>
        /// Checks according to Special segregation codes acc to col 16b of DG List. 
        /// For each individual dg check is carried out against the whole dg list, where required, and Conflict is added to the unit.
        /// Otherwise segregator (class to be used for segregation instead of original class) is appointed.
        /// </summary>
        /// <param name="a">Dg which special codes are being checked</param>
        /// <param name="dglist">DgList in which Dg is to be checked</param>
        /// <param name="ship">Ship profile</param>
        public static void SpecialSegregationCheck(Dg a, IEnumerable<Dg> dglist, Transport.ShipProfile ship)
        {
            foreach (string sscode in a.SegregationSGList)
            {
                bool conf = false;
                switch (sscode)
                {
                    case "SG7": //Stow “away from” class 3.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "3", SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG8": //Stow “away from” class 4.1.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "4.1", SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG9": //Stow “away from” class 4.3.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "4.3", SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG10": //Stow “away from” class 5.1.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "5.1", SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG11": //Stow “away from” class 6.2.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "6.2", SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG12": //Stow “away from” class 7.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "7", SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG13": //Stow “away from” class 8.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "8", SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG14": //Stow “separated from” class 1 except for division 1.4S.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "1", SegregationCase.SeparatedFrom, sscode, ship, exceptFromClass: "1.4S");
                        }
                        break;

                    case "SG15": // Stow “separated from” class 3.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "3", SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG16": //Stow “separated from” class 4.1.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "4.1", SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG17": //Stow “separated from” class 5.1.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "5.1", SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG18": //Stow “separated from” class 6.2.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "6.2", SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG19": //Stow “separated from” class 7.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "7", SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG20": //Stow “away from” acids.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.acids, SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG21": //Stow “away from” alkalis.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.alkalis, SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG22": //Stow “away from” ammonium salts.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.ammonium_compounds, SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG23": //Stow “away from” animal or vegetable oils.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSubstance(a, b, "oil", SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG24": //Stow “away from” azides.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.azides, SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG25": //Stow “separated from” goods of classes 2.1 and 3.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClasses(a, b, new [] { "2.1", "3" }, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG26": //In addition: from goods of classes 2.1 and 3 when stowed on deck of a container ship a minimum distance of two container spaces athwartship shall be maintained, when stowed on ro-ro ships a distance of 6 m athwartship shall be maintained.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.IsLq) continue;
                            if ((b.AllDgClassesList.Contains("2.1") || b.AllDgClassesList.Contains("3")) && !a.IsUnderdeck && !b.IsUnderdeck)
                            {
                                conf = Athwartship(a, b, 2, ship.Row00Exists) && ForeAndAft(a, b, 1);
                                a.AddConflict(conf, segr, sscode, b);
                            }
                        }
                        break;

                    case "SG27": //Stow “away from” explosives containing chlorates or perchlorates.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.IsLq) continue;
                            foreach (int s in b.SegregationGroupList)
                            {
                                if (s != (int)IMDGCode.SegregationGroup.chlorates && s != (int)IMDGCode.SegregationGroup.perchlorates)
                                    continue;
                                foreach (string cl in b.AllDgClassesList)
                                {
                                    if (cl.StartsWith("1"))
                                    {
                                        conf = SegregationCase1(a, b, ship);
                                        a.AddConflict(conf, segr, sscode, b);
                                    }
                                }
                                break;
                            }
                        }
                        break;

                    case "SG28": //Stow “away from” ammonium compounds and explosives containing ammonium compounds or salts.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.ammonium_compounds, SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG29": //Segregation from foodstuffs as in 7.3.4.2.2, 7.6.3.1.2 or 7.7.3.7.
                                 //not in same cargo transport unit
                        break;

                    case "SG30": //Stow “away from” heavy metals and their salts.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.heavy_metals_and_their_salts, SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG31": //Stow “away from” lead and its compounds.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.lead_and_its_compounds, SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG32": //Stow “away from” liquid halogenated hydrocarbons.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.liquid_halogenated_hydrocarbons, SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG33": //Stow “away from” powdered metals.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.powdered_metals, SegregationCase.AwayFrom, sscode, ship);
                        }
                        break;

                    case "SG34": //When containing ammonium compounds, “separated from” SGG4 – chlorates or SGG13 – perchlorates and explosives containing chlorates or perchlorates.
                        if (!a.SegregationGroupList.Contains((byte)IMDGCode.SegregationGroup.ammonium_compounds)) break;
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.chlorates, SegregationCase.SeparatedFrom, sscode, ship);
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.perchlorates, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG35": //Stow “separated from” acids.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.acids, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG36": //Stow “separated from” alkalis.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.alkalis, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG37": //Stow “separated from” ammonia.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSubstance(a, b, "ammonia", SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG38": //Stow “separated from” ammonium compounds.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.ammonium_compounds, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG39": //Stow “separated from” ammonium compounds other than AMMONIUM PERSULPHATE (UN 1444).
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.IsLq || b.Unno == 1444) continue;
                            foreach (int s in b.SegregationGroupList)
                            {
                                if (s != (byte)IMDGCode.SegregationGroup.ammonium_compounds) continue;
                                conf = SegregationCase2(a, b, ship);
                                a.AddConflict(conf, segr, sscode, b);
                                break;
                            }
                        }
                        break;

                    case "SG40": //Stow “separated from” ammonium compounds other than mixtures of ammonium persulphates and/or potassium persulphates and/or sodium persulphates.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.IsLq) continue;
                            foreach (byte s in b.SegregationGroupList)
                            {
                                if (s != (byte)IMDGCode.SegregationGroup.ammonium_compounds) continue;
                                if (b.Name.ToUpper().Contains("POTASSIUM PERSULPHATE") || b.Name.ToUpper().Contains("AMMONIUM PERSULPHATE") || b.Name.ToUpper().Contains("SODIUM PERSULPHATE")) break;
                                conf = SegregationCase2(a, b, ship);
                                a.AddConflict(conf, segr, sscode, b);
                                break;
                            }
                        }
                        break;

                    case "SG41": //Stow “separated from” animal or vegetable oil.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSubstance(a, b, "oil", SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG42": //Stow “separated from” bromates.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.bromates, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG43": //Stow “separated from” bromine.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSubstance(a, b, "bromine", SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG44": //Stow “separated from” CARBON TETRACHLORIDE (UN 1846).
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithUnno(a, b, 1846, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG45": //Stow “separated from” chlorates.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.chlorates, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG46": //Stow “separated from” chlorine.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSubstance(a, b, "chlorine", SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG47": //Stow “separated from” chlorites.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.chlorites, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG48": //Stow “separated from” combustible material(particularly liquids). Combustible material does not include packing materials or dunnage.
                        a.AddConflict(conf, segr, sscode, a);
                        break;

                    case "SG49": //Stow “separated from” cyanides.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.cyanides, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG50": //Segregation from foodstuffs as in 7.3.4.2.1, 7.6.3.1.2 or 7.7.3.6.
                        break;

                    case "SG51": //Stow “separated from” hypochlorites.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.hypochlorites, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG52": //Stow “separated from” iron oxide.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSubstance(a, b, new[] { "iron", "oxide" }, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG53": //Stow “separated from” liquid organic substances.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSubstance(a, b, new[] { "liquid", "organic" }, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG54": //Stow “separated from” mercury and mercury compounds.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.mercury_and_mercury_compounds, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG55": //Stow “separated from” mercury salts.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.mercury_and_mercury_compounds, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG56": //Stow “separated from” nitrites.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.nitrites_and_their_mixtures, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG57": //Stow “separated from” odour-absorbing cargoes.
                        a.AddConflict(true, segr, sscode, a);
                        break;

                    case "SG58": //Stow “separated from” perchlorates.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.perchlorates, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG59": //Stow “separated from” permanganates.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.perchlorates, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG60": //Stow “separated from” peroxides.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.peroxides, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG61": //Stow “separated from” powdered metals.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.powdered_metals, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG62": //Stow “separated from” sulphur.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSubstance(a, b, "sulphur", SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG63": //Stow “separated longitudinally by an intervening complete compartment or hold from” class 1.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "1", SegregationCase.SeparatedLongitudinallyByCompleteCompartmentFrom, sscode, ship);
                        }
                        break;

                    case "SG64": //[Reserved]
                        break;

                    case "SG65": //Stow “separated by a complete compartment or hold from” class 1 except for division 1.4.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClass(a, b, "1", SegregationCase.SeparatedLongitudinallyByCompleteCompartmentFrom,
                                sscode, ship, exceptFromClass: "1.4");
                        }
                        break;

                    case "SG66": //[Reserved]
                        break;

                    case "SG67": //Stow “separated from” division 1.4 and “separated longitudinally by an intervening complete compartment of hold from” divisions 1.1, 1.2, 1.3, 1.5 and 1.6 except from explosives of compatibility group J.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.IsLq) continue;
                            foreach (string s in b.AllDgClassesList)
                                if (s.StartsWith("1.4"))
                                {
                                    conf = SegregationCase2(a, b, ship);
                                    a.AddConflict(conf, segr, sscode, b);
                                }
                                else if (s.StartsWith("1") && b.CompatibilityGroup != 'J')
                                {
                                    conf = SegregationCase4(a, b, ship);
                                    a.AddConflict(conf, segr, sscode, b);
                                }
                        }
                        break;

                    case "SG70": //For arsenic sulphides, “separated from” acids.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.acids, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG71": //Within the appliance, to the extent that the dangerous goods are integral parts of the complete life-saving appliance, there is no need to apply the provisions on segregation of substances in chapter 7.2.
                        break;

                    case "SG72": //See 7.2.6.3.2.
                        break;

                    case "SG73": //[Reserved]
                        break;

                    case "SG75": //Stow “separated from” strong acids.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.strong_acids, SegregationCase.SeparatedFrom, sscode, ship);
                        }
                        break;

                    case "SG78": //Stow “separated longitudinally by an intervening complete compartment or hold from” division 1.1, 1.2, and 1.5.
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithClasses(a, b, new[] { "1.1", "1.2", "1.5" }
                            , SegregationCase.SeparatedLongitudinallyByCompleteCompartmentFrom, sscode, ship);
                        }
                        break;

                }
            }
        }


        private static void CheckSegregationWithSpecialGroup(Dg a, Dg b, IMDGCode.SegregationGroup segregationGroup,
                                        SegregationCase segregationCase, string sscode, Transport.ShipProfile ship)
        {
            bool conf;

            if (a == b || b.IsLq) return;
            foreach (byte s in b.SegregationGroupList)
            {
                if (s != (byte)segregationGroup &&
                    s != (segregationGroup == IMDGCode.SegregationGroup.acids ? (byte)IMDGCode.SegregationGroup.strong_acids : 0)) continue;
                conf = SegregationConflictCheck(a, b, (byte)segregationCase, ship);
                a.AddConflict(conf, segr, sscode, b);

                //For acids & alkalis cases of class 8 - 7.2.6.5
                if (conf &&
                    (segregationGroup == IMDGCode.SegregationGroup.acids
                    || segregationGroup == IMDGCode.SegregationGroup.strong_acids
                    || segregationGroup == IMDGCode.SegregationGroup.alkalis))
                    CheckClass8CanBeStowedTogether(a, b);
                break;
            }
        }

        private static void CheckSegregationWithClass(Dg a, Dg b, string dgClass, SegregationCase segregationCase,
                                                    string sscode, Transport.ShipProfile ship, string exceptFromClass = null)
        {
            bool conf;
            if (a == b || b.IsLq) return;
            foreach (string s in b.AllDgClassesList)
            {
                if (!s.StartsWith(dgClass)) continue;
                if (!string.IsNullOrEmpty(exceptFromClass) && s.StartsWith(exceptFromClass)) continue;
                conf = SegregationConflictCheck(a, b, (byte)segregationCase, ship);
                a.AddConflict(conf, segr, sscode, b);
                break;
            }
        }

        private static void CheckSegregationWithClasses(Dg a, Dg b, string[] dgClasses, SegregationCase segregationCase,
                                            string sscode, Transport.ShipProfile ship, string exceptFromClass = null)
        {
            bool conf;

            if (a == b || b.IsLq) return;
            
            foreach (string s in b.AllDgClassesList)
            {
                foreach (string dgClass in dgClasses)
                {
                    if (!s.StartsWith(dgClass)) continue;
                    if (!string.IsNullOrEmpty(exceptFromClass) && s.StartsWith(exceptFromClass)) continue;
                    conf = SegregationConflictCheck(a, b, (byte)segregationCase, ship);
                    a.AddConflict(conf, segr, sscode, b);
                    
                    if(conf) return;
                    break;
                }
            }
        }

        private static void CheckSegregationWithUnno(Dg a, Dg b, ushort unno, SegregationCase segregationCase,
                                            string sscode, Transport.ShipProfile ship)
        {
            bool conf;

            if (a == b || b.IsLq) return;
            if (b.Unno != unno) return;

            conf = SegregationConflictCheck(a, b, (byte)segregationCase, ship);
            a.AddConflict(conf, segr, sscode, b);
        }

        private static void CheckSegregationWithSubstance(Dg a, Dg b, string[] nameContains, SegregationCase segregationCase,
                                            string sscode, Transport.ShipProfile ship)
        {
            bool conf;
            if (a == b || b.IsLq) return;

            foreach (string s in nameContains)
                if (!b.Name.ToLower().Contains(s.ToLower())) return;

            conf = SegregationConflictCheck(a, b, (byte)segregationCase, ship);
            a.AddConflict(conf, segr, sscode, b);
        }

        private static void CheckSegregationWithSubstance(Dg a, Dg b, string nameContains, SegregationCase segregationCase,
                                            string sscode, Transport.ShipProfile ship)
        {
            CheckSegregationWithSubstance(a, b, new[] { nameContains }, segregationCase, sscode, ship);
        }

        private static void CheckClass8CanBeStowedTogether(Dg a, Dg b)
        {
            if (a.DgClass == "8" && b.DgClass == "8"
                && (a.PackingGroupByte > 1) && (b.PackingGroupByte > 1))
                a.AddConflict(true, segr, "SGC201", b);
        }
    }
}
