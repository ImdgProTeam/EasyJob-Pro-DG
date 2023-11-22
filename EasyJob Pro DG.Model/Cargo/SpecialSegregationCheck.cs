using System;
using System.Collections.Generic;
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
                    // Define Segregation group
                    if (sscode.StartsWith("SGG"))
                    {
                        dg.SegregationGroupByte = (byte)Array.IndexOf(IMDGCode.SegregationGroupsCodes, sscode);
                        continue;
                    }

                    // Work with other special segregation requirements
                    switch (sscode)
                    {
                        case "SG1": //For packages carrying a subsidiary risk label of class 1, segregation as for class 1, division 1.3.
                            foreach (string s in dg.DgSubclassArray)
                            {
                                if (s.StartsWith("1"))
                                {
                                    dg.SegregatorClass = "1.3";
                                    dg.SegregatorException = new SegregatorException("1", SegregationCase.SeparationBetweenExplosives);
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
                            if (dg.FlashPointDouble <= 60.5 || Math.Abs(dg.FlashPointDouble - 9999) < 10)
                            {
                                dg.SegregatorClass = "3";
                                dg.SegregatorException = new SegregatorException("4.1", SegregationCase.AwayFrom);
                            }
                            break;
                        case "SG69": //For AEROSOLS with a maximum capacity of 1 L: segregation as for class 9.
                            //Stow “separated from” class 1 except for division 1.4.
                            //For AEROSOLS with a capacity above 1 L: segregation as for the appropriate subdivision of class 2.
                            //For WASTE AEROSOLS: segregation as for the appropriate subdivision of class 2.
                            if (dg.IsMax1L)
                            {
                                dg.SegregatorClass = "9";
                                dg.SegregatorException = new SegregatorException("1", SegregationCase.SeparatedFrom);
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
                            dg.SegregatorException = new SegregatorException("7", SegregationCase.None);
                            break;

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
        public static void SpecialSegregationCheck(Dg a, IEnumerable<Dg> dglist)
        {
            foreach (string sscode in a.SegregationSGList)
            {
                bool conf = false;
                switch (sscode)
                {
                    case "SG7": //Stow “away from” class 3.
                        CheckSegregationWithClass(a, dglist, "3", SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG8": //Stow “away from” class 4.1.
                        CheckSegregationWithClass(a, dglist, "4.1", SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG9": //Stow “away from” class 4.3.
                        CheckSegregationWithClass(a, dglist, "4.3", SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG10": //Stow “away from” class 5.1.
                        CheckSegregationWithClass(a, dglist, "5.1", SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG11": //Stow “away from” class 6.2.
                        CheckSegregationWithClass(a, dglist, "6.2", SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG12": //Stow “away from” class 7.
                        CheckSegregationWithClass(a, dglist, "7", SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG13": //Stow “away from” class 8.
                        CheckSegregationWithClass(a, dglist, "8", SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG14": //Stow “separated from” class 1 except for division 1.4S.
                        CheckSegregationWithClass(a, dglist, "1", SegregationCase.SeparatedFrom, sscode, exceptFromClass: "1.4S");
                        break;

                    case "SG15": // Stow “separated from” class 3.
                        CheckSegregationWithClass(a, dglist, "3", SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG16": //Stow “separated from” class 4.1.
                        CheckSegregationWithClass(a, dglist, "4.1", SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG17": //Stow “separated from” class 5.1.
                        CheckSegregationWithClass(a, dglist, "5.1", SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG18": //Stow “separated from” class 6.2.
                        CheckSegregationWithClass(a, dglist, "6.2", SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG19": //Stow “separated from” class 7.
                        CheckSegregationWithClass(a, dglist, "7", SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG20": //Stow “away from” acids.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.acids, SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG21": //Stow “away from” alkalis.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.alkalis, SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG22": //Stow “away from” ammonium salts.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.ammonium_compounds, SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG23": //Stow “away from” animal or vegetable oils.
                        CheckSegregationWithSubstance(a, dglist, "oil", SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG24": //Stow “away from” azides.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.azides, SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG25": //Stow “separated from” goods of classes 2.1 and 3.
                        CheckSegregationWithClasses(a, dglist, new[] { "2.1", "3" }, SegregationCase.SeparatedFrom, sscode);
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

                    case "SG27": //Stow “separated from” explosives containing chlorates or perchlorates.
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
                                        conf = SegregationCase2(a, b);
                                        a.AddConflict(conf, segr, sscode, b);
                                    }
                                }
                                break;
                            }
                        }
                        break;

                    case "SG28": //Stow “separated from” ammonium compounds and explosives containing ammonium compounds or salts.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.ammonium_compounds, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG29": //Segregation from foodstuffs as in 7.3.4.2.2, 7.6.3.1.2 or 7.7.3.7.
                                 //not in same cargo transport unit
                        break;

                    case "SG30": //Stow “away from” heavy metals and their salts.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.heavy_metals_and_their_salts, SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG31": //Stow “away from” lead and its compounds.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.lead_and_its_compounds, SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG32": //Stow “away from” liquid halogenated hydrocarbons.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.liquid_halogenated_hydrocarbons, SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG33": //Stow “away from” powdered metals.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.powdered_metals, SegregationCase.AwayFrom, sscode);
                        break;

                    case "SG34": //When containing ammonium compounds, “separated from” SGG4 – chlorates or SGG13 – perchlorates and explosives containing chlorates or perchlorates.
                        if (!a.SegregationGroupList.Contains((byte)IMDGCode.SegregationGroup.ammonium_compounds)) break;
                        foreach (Dg b in dglist)
                        {
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.chlorates, SegregationCase.SeparatedFrom, sscode);
                            CheckSegregationWithSpecialGroup(a, b, IMDGCode.SegregationGroup.perchlorates, SegregationCase.SeparatedFrom, sscode);
                        }
                        break;

                    case "SG35": //Stow “separated from” acids.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.acids, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG36": //Stow “separated from” alkalis.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.alkalis, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG37": //Stow “separated from” ammonia.
                        CheckSegregationWithSubstance(a, dglist, "ammonia", SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG38": //Stow “separated from” ammonium compounds.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.ammonium_compounds, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG39": //Stow “separated from” ammonium compounds other than AMMONIUM PERSULPHATE (UN 1444).
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.IsLq || b.Unno == 1444) continue;
                            foreach (int s in b.SegregationGroupList)
                            {
                                if (s != (byte)IMDGCode.SegregationGroup.ammonium_compounds) continue;
                                conf = SegregationCase2(a, b);
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
                                conf = SegregationCase2(a, b);
                                a.AddConflict(conf, segr, sscode, b);
                                break;
                            }
                        }
                        break;

                    case "SG41": //Stow “separated from” animal or vegetable oil.
                        CheckSegregationWithSubstance(a, dglist, "oil", SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG42": //Stow “separated from” bromates.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.bromates, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG43": //Stow “separated from” bromine.
                        CheckSegregationWithSubstance(a, dglist, "bromine", SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG44": //Stow “separated from” CARBON TETRACHLORIDE (UN 1846).
                        CheckSegregationWithUnno(a, dglist, 1846, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG45": //Stow “separated from” chlorates.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.chlorates, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG46": //Stow “separated from” chlorine.
                        CheckSegregationWithSubstance(a, dglist, "chlorine", SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG47": //Stow “separated from” chlorites.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.chlorites, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG48": //Stow “separated from” combustible material (particularly liquids).
                        a.AddConflict(conf, segr, sscode, a);
                        break;

                    case "SG49": //Stow “separated from” cyanides.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.cyanides, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG50": //Segregation from foodstuffs as in 7.3.4.2.1, 7.6.3.1.2 or 7.7.3.6.
                        break;

                    case "SG51": //Stow “separated from” hypochlorites.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.hypochlorites, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG52": //Stow “separated from” iron oxide.
                        CheckSegregationWithSubstance(a, dglist, new[] { "iron", "oxide" }, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG53": //Shall not be stowed together with combustible material in the same cargo transport unit.
                        a.AddConflict(conf, segr, sscode, a);
                        break;

                    case "SG54": //Stow “separated from” mercury and mercury compounds.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.mercury_and_mercury_compounds, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG55": //Stow “separated from” mercury salts.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.mercury_and_mercury_compounds, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG56": //Stow “separated from” nitrites.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.nitrites_and_their_mixtures, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG57": //Stow “separated from” odour-absorbing cargoes.
                        a.AddConflict(true, segr, sscode, a);
                        break;

                    case "SG58": //Stow “separated from” perchlorates.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.perchlorates, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG59": //Stow “separated from” permanganates.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.perchlorates, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG60": //Stow “separated from” peroxides.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.peroxides, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG61": //Stow “separated from” powdered metals.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.powdered_metals, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG62": //Stow “separated from” sulphur.
                        CheckSegregationWithSubstance(a, dglist, "sulphur", SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG63": //Stow “separated longitudinally by an intervening complete compartment or hold from” class 1.
                        CheckSegregationWithClass(a, dglist, "1", SegregationCase.SeparatedLongitudinallyByCompleteCompartmentFrom, sscode);
                        break;

                    case "SG64": //[Reserved]
                        break;

                    case "SG65": //Stow “separated by a complete compartment or hold from” class 1 except for division 1.4.
                        CheckSegregationWithClass(a, dglist, "1", SegregationCase.SeparatedLongitudinallyByCompleteCompartmentFrom,
                                                sscode, exceptFromClass: "1.4");
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
                                    conf = SegregationCase2(a, b);
                                    a.AddConflict(conf, segr, sscode, b);
                                }
                                else if (s.StartsWith("1") && b.CompatibilityGroup != 'J')
                                {
                                    conf = SegregationCase4(a, b);
                                    a.AddConflict(conf, segr, sscode, b);
                                }
                        }
                        break;

                    case "SG70": //For arsenic sulphides, “separated from” acids.
                        CheckSegregationWithSpecialGroup(a, dglist, IMDGCode.SegregationGroup.acids, SegregationCase.SeparatedFrom, sscode);
                        break;

                    case "SG71": //Within the appliance, to the extent that the dangerous goods are integral parts of the complete life-saving appliance, there is no need to apply the provisions on segregation of substances in chapter 7.2.
                        break;

                    case "SG72": //See 7.2.6.3.2.
                        break;

                    case "SG73": //[Reserved]
                        break;

                    case "SG78": //Stow “separated longitudinally by an intervening complete compartment or hold from” division 1.1, 1.2, and 1.5.
                        CheckSegregationWithClasses(a, dglist, new[] { "1.1", "1.2", "1.5" }, 
                                                SegregationCase.SeparatedLongitudinallyByCompleteCompartmentFrom, sscode);
                        break;

                }
            }
        }


        #region Various private segregation methods

        private static void CheckSegregationWithSpecialGroup(Dg a, IEnumerable<Dg> dgList, IMDGCode.SegregationGroup segregation,
            SegregationCase segregationCase, string sscode)
        {
            foreach (Dg b in dgList)
            {
                CheckSegregationWithSpecialGroup(a, b, segregation, segregationCase, sscode);
            }
        }

        private static void CheckSegregationWithSpecialGroup(Dg a, Dg b, IMDGCode.SegregationGroup segregationGroup,
                                SegregationCase segregationCase, string sscode)
        {
            bool conf;

            if (a == b || b.IsLq) return;
            foreach (byte s in b.SegregationGroupList)
            {
                if (s != (byte)segregationGroup) continue;
                conf = SegregationConflictCheck(a, b, (byte)segregationCase);
                a.AddConflict(conf, segr, sscode, b);

                //For acids & alkalis cases of class 8 - 7.2.6.5
                if (conf &&
                    (segregationGroup == IMDGCode.SegregationGroup.acids
                    || segregationGroup == IMDGCode.SegregationGroup.alkalis))
                    CheckClass8CanBeStowedTogether(a, b);
                break;
            }
        }


        private static void CheckSegregationWithClass(Dg a, IEnumerable<Dg> dgList, string dgClass, SegregationCase segregationCase,
                                                    string sscode, string exceptFromClass = null)
        {
            foreach (Dg b in dgList)
            {
                CheckSegregationWithClass(a, b, dgClass, segregationCase, sscode, exceptFromClass);
            }
        }

        private static void CheckSegregationWithClass(Dg a, Dg b, string dgClass, SegregationCase segregationCase,
                                                    string sscode, string exceptFromClass = null)
        {
            bool conf;
            if (a == b || b.IsLq) return;
            foreach (string s in b.AllDgClassesList)
            {
                if (!s.StartsWith(dgClass)) continue;
                if (!string.IsNullOrEmpty(exceptFromClass) && s.StartsWith(exceptFromClass)) continue;
                conf = SegregationConflictCheck(a, b, (byte)segregationCase);
                a.AddConflict(conf, segr, sscode, b);
                break;
            }
        }


        private static void CheckSegregationWithClasses(Dg a, IEnumerable<Dg> dgList, string[] dgClasses, SegregationCase segregationCase,
                                            string sscode, string exceptFromClass = null)
        {
            foreach (Dg b in dgList)
            {
                CheckSegregationWithClasses(a, b, dgClasses, segregationCase, sscode, exceptFromClass);
            }
        }
        private static void CheckSegregationWithClasses(Dg a, Dg b, string[] dgClasses, SegregationCase segregationCase,
                                            string sscode, string exceptFromClass = null)
        {
            bool conf;

            if (a == b || b.IsLq) return;

            foreach (string s in b.AllDgClassesList)
            {
                foreach (string dgClass in dgClasses)
                {
                    if (!s.StartsWith(dgClass)) continue;
                    if (!string.IsNullOrEmpty(exceptFromClass) && s.StartsWith(exceptFromClass)) continue;
                    conf = SegregationConflictCheck(a, b, (byte)segregationCase);
                    a.AddConflict(conf, segr, sscode, b);

                    if (conf) return;
                    break;
                }
            }
        }


        private static void CheckSegregationWithUnno(Dg a, IEnumerable<Dg> dgList, ushort unno, SegregationCase segregationCase, string sscode)
        {
            foreach (Dg b in dgList)
            {
                CheckSegregationWithUnno(a, b, unno, segregationCase, sscode);
            }
        }
        private static void CheckSegregationWithUnno(Dg a, Dg b, ushort unno, SegregationCase segregationCase, string sscode)
        {
            bool conf;

            if (a == b || b.IsLq) return;
            if (b.Unno != unno) return;

            conf = SegregationConflictCheck(a, b, (byte)segregationCase);
            a.AddConflict(conf, segr, sscode, b);
        }


        private static void CheckSegregationWithSubstance(Dg a, IEnumerable<Dg> dgList, string[] nameContains, SegregationCase segregationCase, string sscode)
        {
            foreach (Dg b in dgList)
            {
                CheckSegregationWithSubstance(a, b, nameContains, segregationCase, sscode);
            }
        }
        private static void CheckSegregationWithSubstance(Dg a, Dg b, string[] nameContains, SegregationCase segregationCase, string sscode)
        {
            bool conf;
            if (a == b || b.IsLq) return;

            foreach (string s in nameContains)
                if (!b.Name.ToLower().Contains(s.ToLower())) return;

            conf = SegregationConflictCheck(a, b, (byte)segregationCase);
            a.AddConflict(conf, segr, sscode, b);
        }


        private static void CheckSegregationWithSubstance(Dg a, IEnumerable<Dg> dgList, string nameContains, SegregationCase segregationCase, string sscode)
        {
            foreach (Dg b in dgList)
            {
                CheckSegregationWithSubstance(a, b, nameContains, segregationCase, sscode);
            }
        }
        private static void CheckSegregationWithSubstance(Dg a, Dg b, string nameContains, SegregationCase segregationCase, string sscode)
        {
            CheckSegregationWithSubstance(a, b, new[] { nameContains }, segregationCase, sscode);
        }


        private static void CheckClass8CanBeStowedTogether(Dg a, Dg b)
        {
            if (a.DgClass == "8" && b.DgClass == "8"
                && (a.PackingGroupByte > 1) && (b.PackingGroupByte > 1))
                a.AddConflict(true, segr, "SGC201", b);
        }
        #endregion
    }
}
