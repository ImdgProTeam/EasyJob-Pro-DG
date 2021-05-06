
using System.Collections.Generic;


namespace EasyJob_Pro_DG
{
    partial class Segregation
    {
        /// <summary>
        /// Checks according to Special segregation codes acc to col 16b of DG List. 
        /// For each individual dg check is carried out against the whole dg list, where required, and conflict is added to the unit.
        /// Otherwise segregator (class to be used for segregation instead of original class) is appointed.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="dglist"></param>
        public static void SpecialSegregationCheck(Dg a, DgList dglist, ShipProfile ship)
        {
            foreach (string sscode in a.segregationSG)
            {
                bool _conf = false;
                switch (sscode)
                {
                    case "SG1": //For packages carrying a subsidiary risk label of class 1, segregation as for class 1, division 1.3.
                        foreach (string s in a.dgsubclass)
                        {
                            if (s.StartsWith("1")) a.segregatorClass = "1.3";
                        }
                        break;
                    case "SG2": //Segregation as for class 1.2G.
                        a.segregatorClass = "1.2G";
                        break;
                    case "SG3": //Segregation as for class 1.3G.
                        a.segregatorClass = "1.3G";
                        break;
                    case "SG4": //Segregation as for class 2.1.
                        a.segregatorClass = "2.1";
                        break;
                    case "SG5": // Segregation as for class 3.
                        a.segregatorClass = "3";
                        break;
                    case "SG6": //Segregation as for class 5.1.
                        a.segregatorClass = "5.1";
                        break;
                    case "SG7": //Stow “away from” class 3.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (string s in b.allDgClasses)
                                if (s == "3")
                                {
                                    _conf = segregationCase1(a, b, ship);
                                    a.AddConflict(_conf, segr, sscode, b);
                                }
                        }
                        break;
                    case "SG8": //Stow “away from” class 4.1.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "4.1")
                            {
                                _conf = segregationCase1(a, b, ship);
                                if (!_conf) continue;
                                a.AddConflict();
                                a.conflict.AddSegrConflict(sscode, b);
                            }
                            else foreach (string s in b.dgsubclass)
                                {
                                    if (s == "4.1") _conf = segregationCase1(a, b, ship);
                                    if (!_conf) continue;
                                    a.AddConflict();
                                    a.conflict.AddSegrConflict(sscode, b);
                                }
                        }
                        break;
                    case "SG9": //Stow “away from” class 4.3.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "4.3")
                            {
                                _conf = segregationCase1(a, b, ship);
                                if (!_conf) continue;
                                a.AddConflict();
                                a.conflict.AddSegrConflict(sscode, b);
                            }
                            else foreach (string s in b.dgsubclass)
                                {
                                    if (s == "4.3") _conf = segregationCase1(a, b, ship);
                                    if (!_conf) continue;
                                    a.AddConflict();
                                    a.conflict.AddSegrConflict(sscode, b);
                                }
                        }
                        break;
                    case "SG10": //Stow “away from” class 5.1.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "5.1")
                            {
                                _conf = segregationCase1(a, b, ship);
                                if (!_conf) continue;
                                a.AddConflict();
                                a.conflict.AddSegrConflict(sscode, b);
                            }
                            else foreach (string s in b.dgsubclass)
                                {
                                    if (s == "5.1") _conf = segregationCase1(a, b, ship);
                                    if (!_conf) continue;
                                    a.AddConflict();
                                    a.conflict.AddSegrConflict(sscode, b);
                                }
                        }
                        break;
                    case "SG11": //Stow “away from” class 6.2.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "6.2")
                            {
                                _conf = segregationCase1(a, b, ship);
                                if (!_conf) continue;
                                a.AddConflict();
                                a.conflict.AddSegrConflict(sscode, b);
                            }
                            else foreach (string s in b.dgsubclass)
                                {
                                    if (s == "6.2") _conf = segregationCase1(a, b, ship);
                                    if (!_conf) continue;
                                    a.AddConflict();
                                    a.conflict.AddSegrConflict(sscode, b);
                                }
                        }
                        break;
                    case "SG12": //Stow “away from” class 7.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "7")
                            {
                                _conf = segregationCase1(a, b, ship);
                                if (!_conf) continue;
                                a.AddConflict();
                                a.conflict.AddSegrConflict(sscode, b);
                            }
                            else foreach (string s in b.dgsubclass)
                                {
                                    if (s == "7") _conf = segregationCase1(a, b, ship);
                                    if (!_conf) continue;
                                    a.AddConflict();
                                    a.conflict.AddSegrConflict(sscode, b);
                                }
                        }
                        break;
                    case "SG13": //Stow “away from” class 8.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "8")
                            {
                                _conf = segregationCase1(a, b, ship);
                                if (!_conf) continue;
                                a.AddConflict();
                                a.conflict.AddSegrConflict(sscode, b);
                            }
                            else foreach (string s in b.dgsubclass)
                                {
                                    if (s == "8") _conf = segregationCase1(a, b, ship);
                                    if (_conf)
                                    {
                                        a.AddConflict();
                                        a.conflict.AddSegrConflict(sscode, b);
                                    }
                                }
                        }
                        break;
                    case "SG14": //Stow “separated from” class 1 except for division 1.4S.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (string s in b.allDgClasses)
                                if (s.StartsWith("1") && b.dgclass != "1.4S")
                                {
                                    _conf = segregationCase2(a, b, ship);
                                    a.AddConflict(_conf, segr, sscode, b);
                                }
                        }
                        break;
                    case "SG15": // Stow “separated from” class 3.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "3")
                            {
                                _conf = segregationCase2(a, b, ship);
                                if (_conf)
                                {
                                    a.AddConflict();
                                    a.conflict.AddSegrConflict(sscode, b);
                                }
                            }
                            else foreach (string s in b.dgsubclass)
                                {
                                    if (s == "3") _conf = segregationCase2(a, b, ship);
                                    if (_conf)
                                    {
                                        a.AddConflict();
                                        a.conflict.AddSegrConflict(sscode, b);
                                    }
                                }
                        }
                        break;
                    case "SG16": //Stow “separated from” class 4.1.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (string clb in b.allDgClasses)
                                if (clb == "4.1")
                                {
                                    _conf = segregationCase2(a, b, ship);
                                    a.AddConflict(_conf, segr, sscode, b);
                                }
                        }
                        break;
                    case "SG17": //Stow “separated from” class 5.1.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "5.1")
                            {
                                _conf = segregationCase2(a, b, ship);
                                if (_conf)
                                {
                                    a.AddConflict();
                                    a.conflict.AddSegrConflict(sscode, b);
                                }
                            }
                            else foreach (string s in b.dgsubclass)
                                {
                                    if (s == "5.1") _conf = segregationCase2(a, b, ship);
                                    if (_conf)
                                    {
                                        a.AddConflict();
                                        a.conflict.AddSegrConflict(sscode, b);
                                    }
                                }
                        }
                        break;
                    case "SG18": //Stow “separated from” class 6.2.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "6.2")
                            {
                                _conf = segregationCase2(a, b, ship);
                                if (_conf)
                                {
                                    a.AddConflict();
                                    a.conflict.AddSegrConflict(sscode, b);
                                }
                            }
                            else foreach (string s in b.dgsubclass)
                                {
                                    if (s == "6.2") _conf = segregationCase2(a, b, ship);
                                    if (_conf)
                                    {
                                        a.AddConflict();
                                        a.conflict.AddSegrConflict(sscode, b);
                                    }
                                }
                        }
                        break;
                    case "SG19": //Stow “separated from” class 7.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "7")
                            {
                                _conf = segregationCase2(a, b, ship);
                                if (_conf)
                                {
                                    a.AddConflict();
                                    a.conflict.AddSegrConflict(sscode, b);
                                }
                            }
                            else foreach (string s in b.dgsubclass)
                                {
                                    if (s == "7") _conf = segregationCase2(a, b, ship);
                                    if (_conf)
                                    {
                                        a.AddConflict();
                                        a.conflict.AddSegrConflict(sscode, b);
                                    }
                                }
                        }
                        break;
                    case "SG20": //Stow “away from” acids.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                _conf = false;
                                if (s == (int)SegregationGroup.acids) _conf = segregationCase1(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf && a.dgclass == "8" && b.dgclass == "8" && (a.PKG == "2" || a.PKG == "3") && (b.PKG == "2" || b.PKG == "3")) a.AddConflict(_conf, segr, "SGC201", b);
                            }
                        }
                        break;
                    case "SG21": //Stow “away from” alkalis.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                _conf = false;
                                if (s == (int)SegregationGroup.alkalis) _conf = segregationCase1(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf && a.dgclass == "8" && b.dgclass == "8" && (a.PKG == "2" || a.PKG == "3") && (b.PKG == "2" || b.PKG == "3")) a.AddConflict(_conf, segr, "SGC201", b);
                            }
                        }
                        break;
                    case "SG22": //Stow “away from” ammonium salts.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 2) _conf = segregationCase1(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                            }
                        }
                        break;
                    case "SG23": //Stow “away from” animal or vegetable oils.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.name.ToLower().Contains("oil"))
                                _conf = segregationCase1(a, b, ship);
                            a.AddConflict(_conf, segr, sscode, b);
                        }
                        break;
                    case "SG24": //Stow “away from” azides.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 17) _conf = segregationCase1(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                            }
                        }
                        break;
                    case "SG25": //Stow “separated from” goods of classes 2.1 and 3.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.dgclass == "2.1" || b.dgclass == "3")
                            {
                                _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                            }
                            foreach (string s in b.dgsubclass)
                            {
                                if (s == "2.1" || s == "3") _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                            }
                        }
                        break;
                    case "SG26": //In addition: from goods of classes 2.1 and 3 when stowed on deck of a containership a minimum distance of two container spaces athwartship shall be maintained, when stowed on ro-ro ships a distance of 6 m athwartship shall be maintained.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if ((b.allDgClasses.Contains("2.1") || b.allDgClasses.Contains("3")) && !a.underdeck && !b.underdeck)
                            {
                                _conf = Athwartships(a, b, 2, ship.row00exists) && ForeAndAft(a, b, 1);
                                a.AddConflict(_conf, segr, sscode, b);
                            }
                        }
                        break;
                    case "SG27": //Stow “away from” explosives containing chlorates or perchlorates.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.chlorates || s == (int)SegregationGroup.perchlorates)
                                {
                                    foreach (string cl in b.allDgClasses) if (cl.StartsWith("1")) _conf = segregationCase1(a, b, ship);
                                }
                                a.AddConflict(_conf, segr, sscode, b);
                            }
                        }
                        break;
                    case "SG28": //Stow “away from” ammonium compounds and explosives containing ammonium compounds or salts.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.ammonium_compounds) _conf = segregationCase1(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG29": //Segregation from foodstuffs as in 7.3.4.2.2, 7.6.3.1.2 or 7.7.3.7.
                                 //not in same cargo transport unit
                        _conf = false;
                        break;
                    case "SG30": //Stow “away from” heavy metals and their salts.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.heavy_metals_and_their_salts) _conf = segregationCase1(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG31": //Stow “away from” lead and its compounds.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.lead_and_its_compounds) _conf = segregationCase1(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG32": //Stow “away from” liquid halogenated hydrocarbons.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.liquid_halogenated_hydrocarbons) _conf = segregationCase1(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG33": //Stow “away from” powdered metals.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.powdered_metals) _conf = segregationCase1(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG34": //When containing ammonium compounds, “away from” chlorates or perchlorates and explosives containing chlorates or perchlorates.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 4 || s == 13) _conf = segregationCase1(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG35": //Stow “separated from” acids.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                _conf = false;
                                if (s == (int)SegregationGroup.acids) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf && a.dgclass == "8" && b.dgclass == "8" && (a.PKG == "2" || a.PKG == "3") && (b.PKG == "2" || b.PKG == "3")) a.AddConflict(_conf, segr, "SGC201", b);
                                break;
                            }
                        }
                        break;
                    case "SG36": //Stow “separated from” alkalis.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                _conf = false;
                                if (s == 18) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf && a.dgclass == "8" && b.dgclass == "8" && (a.PKG == "2" || a.PKG == "3") && (b.PKG == "2" || b.PKG == "3")) a.AddConflict(_conf, segr, "SGC201", b);
                                break;
                            }
                        }
                        break;
                    case "SG37": //Stow “separated from” ammonia.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.name.Contains("AMMONIA") || b.name.Contains("ammonia")) _conf = segregationCase2(a, b, ship);
                            a.AddConflict(_conf, segr, sscode, b);
                            if (_conf) break;
                        }
                        break;
                    case "SG38": //Stow “separated from” ammonium compounds.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.ammonium_compounds) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG39": //Stow “separated from” ammonium compounds other than AMMONIUM PERSULPHATE (UN 1444).
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 2 && b.unno != 1444) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG40": //Stow “separated from” ammonium compounds other than mixtures of ammonium persulphates and/or potassium persulphates and/or sodium persulphates.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 2 && (b.name.Contains("POTASSIUM PERSULPHATE") || b.name.Contains("AMMONIUM PERSULPHATE")))
                                    _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG41": //Stow “separated from” animal or vegetable oil.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.name.Contains("oil") || b.name.Contains("OIL")) _conf = segregationCase2(a, b, ship);
                            a.AddConflict(_conf, segr, sscode, b);
                            if (_conf) break;
                        }
                        break;
                    case "SG42": //Stow “separated from” bromates.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 3) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG43": //Stow “separated from” bromine.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.name.Contains("BROMINE")) _conf = segregationCase2(a, b, ship);
                            a.AddConflict(_conf, segr, sscode, b);
                            if (_conf) break;
                        }
                        break;
                    case "SG44": //Stow “separated from” CARBON TETRACHLORIDE (UN 1846).
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.unno == 1846) _conf = segregationCase2(a, b, ship);
                            a.AddConflict(_conf, segr, sscode, b);
                            if (_conf) break;
                        }
                        break;
                    case "SG45": //Stow “separated from” chlorates.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.chlorates) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG46": //Stow “separated from” chlorine.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.name.Contains("CHLORINE") || b.name.Contains("chlorine")) _conf = segregationCase2(a, b, ship);
                            a.AddConflict(_conf, segr, sscode, b);
                            if (_conf) break;
                        }
                        break;
                    case "SG47": //Stow “separated from” chlorites.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.chlorites) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG48": //Stow “separated from” combustible material(particularly liquids). Combustible material does not include packing materials or dunnage.
                        a.AddConflict(_conf, segr, sscode, a);
                        break;
                    case "SG49": //Stow “separated from” cyanides.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.cyanides) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG50": //Segregation from foodstuffs as in 7.3.4.2.1, 7.6.3.1.2 or 7.7.3.6.
                        _conf = false;
                        break;
                    case "SG51": //Stow “separated from” hypochlorites.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 8) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG52": //Stow “separated from” iron oxide.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if (b.name.Contains("IRON OXIDE")) _conf = segregationCase2(a, b, ship);
                            a.AddConflict(_conf, segr, sscode, b);
                            if (_conf) break;
                        }
                        break;
                    case "SG53": //Stow “separated from” liquid organic substances.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            if ((b.name.Contains("LIQUID") && b.name.Contains("ORGANIC")) || (b.name.Contains("liquid") && b.name.Contains("organic"))) _conf = segregationCase2(a, b, ship);
                            a.AddConflict(_conf, segr, sscode, b);
                        }
                        break;
                    case "SG54": //Stow “separated from” mercury and mercury compounds.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 11) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG55": //Stow “separated from” mercury salts.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 11) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG56": //Stow “separated from” nitrites.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 12) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG57": //Stow “separated from” odour-absorbing cargoes.
                        a.AddConflict();
                        a.conflict.AddSegrConflict(sscode, a);
                        break;
                    case "SG58": //Stow “separated from” perchlorates.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 13) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG59": //Stow “separated from” permanganates.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 14) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG60": //Stow “separated from” peroxides.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 16) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG61": //Stow “separated from” powdered metals.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == 15) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG62": //Stow “separated from” sulphur.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;

                            if (b.name.Contains("SULPHUR")) _conf = segregationCase2(a, b, ship);
                            a.AddConflict(_conf, segr, sscode, b);
                        }
                        break;
                    case "SG63": //Stow “separated longitudinally by an intervening complete compartment or hold from” class 1.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (string s in b.allDgClasses)
                                if (s.StartsWith("1"))
                                {
                                    _conf = segregationCase4(a, b, ship);
                                    a.AddConflict(_conf, segr, sscode, b);
                                }
                        }
                        break;
                    case "SG64": //[Reserved]
                        _conf = false;
                        break;
                    case "SG65": //Stow “separated by a complete compartment or hold from” class 1 except for division 1.4.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.dgclass.StartsWith("1.4") || b.LQ) continue;
                            foreach (string s in b.allDgClasses)
                                if (s.StartsWith("1"))
                                {
                                    _conf = segregationCase3(a, b, ship);
                                    a.AddConflict(_conf, segr, sscode, b);
                                }
                        }
                        break;
                    case "SG66": //[Reserved]
                        _conf = false;
                        break;
                    case "SG67": //Stow “separated from” division 1.4 and “separated longitudinally by an intervening complete compartment of hold from” divisions 1.1, 1.2, 1.3, 1.5 and 1.6 except from explosives of compatibility group J.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (string s in b.allDgClasses)
                                if (s.StartsWith("1.4"))
                                {
                                    _conf = segregationCase2(a, b, ship);
                                    a.AddConflict(_conf, segr, sscode, b);
                                }
                                else if (s.StartsWith("1") && b.compatibilityGroup != 'J')
                                {
                                    _conf = segregationCase4(a, b, ship);
                                    a.AddConflict(_conf, segr, sscode, b);
                                }
                        }
                        break;
                    case "SG68": //If flashpoint 60°C c.c.or below, segregation as for class 3 but “away from” class 4.1.
                        if (a.dgfp <= 60)
                        {
                            foreach (Dg b in dglist)
                            {
                                if (a == b || b.LQ) continue;
                                foreach (string s in b.allDgClasses)
                                    if (s.StartsWith("4.1"))
                                    {
                                        _conf = segregationCase1(a, b, ship);
                                        a.AddConflict(_conf, segr, sscode, b);
                                    }
                            }
                            a.segregatorClass = "3";
                        }
                        break;
                    case "SG69": //For AEROSOLS with a maximum capacity of 1 L: segregation as for class 9.
                                 //Stow “separated from” class 1 except for division 1.4.
                                 //For AEROSOLS with a capacity above 1 L: segregation as for the appropriate subdivision of class 2.
                                 //For WASTE AEROSOLS: segregation as for the appropriate subdivision of class 2.
                        break;
                    case "SG70": //For arsenic sulphides, “separated from” acids.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (byte)SegregationGroup.acids) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                                if (_conf) break;
                            }
                        }
                        break;
                    case "SG71": //Within the appliance, to the extent that the dangerous goods are integral parts of the complete life-saving appliance, there is no need to apply the provisions on segregation of substances in chapter 7.2.
                        break;
                    case "SG72": //See 7.2.6.3.2.
                        _conf = false;
                        break;
                    case "SG73": //[Reserved]
                        _conf = false;
                        break;
                    case "SG74": //Segregation as for 1.4G.
                        a.segregatorClass = "1.4G";
                        break;
                    case "SG75": //Stow “separated from” strong acids.
                        foreach (Dg b in dglist)
                        {
                            if (a == b || b.LQ) continue;
                            foreach (int s in b.segregationGroup)
                            {
                                if (s == (int)SegregationGroup.strong_acids) _conf = segregationCase2(a, b, ship);
                                a.AddConflict(_conf, segr, sscode, b);
                            }
                        }
                        break;
                }
            }
        }
    }
}
