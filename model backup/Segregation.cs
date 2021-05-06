using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace EasyJob_Pro_DG
{
    public partial class Segregation
    {
        private static string segr = "segregation";
        public ShipProfile ship;

        #region Segregation table
        private static readonly int[,] segregationTable ={
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
#endregion

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

        public static string[] segregationGroups = 
        {
            "none",
            "acids",
            "ammonium compounds",
            "bromates",
            "chlorates",
            "chlorites",
            "cyanides",
            "heavy metals and their salts",
            "hypochlorites",
            "lead and its compounds",
            "liquid halogenated hydrocarbons",
            "mercury and mercury compounds",
            "nitrites and their mixtures",
            "perchlorates",
            "permanganates",
            "powdered metals",
            "peroxides",
            "azides",
            "alkalis",
            "strong acids"
        };
#endregion


        //----------------------------- Public constructors ---------------------------------------------------

        public Segregation()
        {}

        public Segregation(ShipProfile _ship)
        {
            ship = _ship;
        }

        //--------------------- Segregation methods ----------------------------------------------------------------

        /// <summary>
        /// Method checks segregation as per segregation table of a DG unit with dg list and adds a conflict if any.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="dglist"></param>
        /// <param name="reefers"></param>
        /// <param name="ship"></param>
        public static void Segregate(Dg a, DgList dglist, ObservableCollection<Container> reefers, ShipProfile ship)
        {
            if (a.LQ) return;
            bool _conf=false;
            byte seglevel,
                _seglevel,
                _seglevel5; //for segregation between class 1
            //1. Check if there are any special requirements in column 16b of CH 3
            SpecialSegregationCheck(a, dglist, ship);

            //2. Segregation between classes according to table 7.2.4
            //taking into account segregation requirements from //1.

            //if no special requirements concerning class (segregation as for class...)
            if (a.segregatorClass == null)
            {
                //check segregation with every unit in dglist
                foreach (Dg b in dglist)
                {
                    if (b.LQ) continue;
                    seglevel = 0;
                    _seglevel5 = 0;
                    _conf = false;
                    if (a.cntrNr == b.cntrNr) continue; //to exclude segregation with the same container
                    //find maximum segregation level, other than 5 (between explosives) => to be found separately

                    foreach (string cla in a.allDgClasses)
                    {
                        foreach(string clb in b.allDgClasses)
                        {
                            _seglevel = (byte)Segregate(cla, clb);
                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                    //check conflicts between the two and record
                    if (_seglevel5 != 0) _conf = SegregationCheck(a, b, _seglevel5, ship);
                    if (SegregationCheck(a, b, seglevel, ship)) _conf = true;
                    //substances of the same class may be stowed together without regard to segregation required by secondary hazards:
                    a.AddConflict(_conf, segr, a.dgclass == b.dgclass ? "SGC21" : "SGC1", b);
                }
            }
            //if special segregation requirement (segregator class) exists
            else
            {
                foreach (Dg b in dglist)
                {
                    if (b.LQ) continue;
                    seglevel = 0;
                    _seglevel5 = 0;
                    if (a.cntrNr == b.cntrNr) continue; //to exclude segregation with the same container
                    //find maximum segregation level, other than 5 (between explosives) => to be found separately

                    foreach (string clb in b.allDgClasses)
                    {
                        _seglevel = (byte)Segregate(a.segregatorClass, clb);
                        if (_seglevel == 5) _seglevel5 = _seglevel;
                        else seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                    }
                    //check conflicts between the two and record
                    if (_seglevel5 != 0) _conf = SegregationCheck(a, b, _seglevel5, ship);
                    if (SegregationCheck(a, b, seglevel, ship)) _conf = true;
                    //substances of the same class may be stowed together without regard to segregation required by secondary hazards:
                    a.AddConflict(_conf, segr, "SGC11", b);
                }
            }
            //Checking segregation with the reefers
            ReefersComplianceSegregationCheck(a, reefers, ship.row00exists, ship.rfMotor);

            //Checking of segregation requirements for class 7
            RadioactiveSegregationCheck(a, dglist, ship.row00exists);
        }


        /// <summary>
        /// Method gets segregation level of 2 DG classes
        /// </summary>
        /// <param name="class1"></param>
        /// <param name="class2"></param>
        /// <returns></returns>
        public static int Segregate(string class1, string class2)
        {
            if (class1 == class2 && 
                ((class1.Length == 1 && class1 != "1") || 
                 (class1.Length < 4 && !class1.StartsWith("1"))))
                return 0;
            if (class1 == "9" || class2 == "9") return 0;
            Dg a = new Dg(class1);
            Dg b = new Dg(class2);
            return segregationTable[a.dgRowInTable, b.dgRowInTable];
        }


        /// <summary>
        /// Method checks if two dg containers are properly segregated according to segregation level
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="seglevel"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static bool SegregationCheck(Dg a, Dg b, byte seglevel, ShipProfile ship)
        //implemented only for container ships with closed cargo holds
        {
            //true if conflicted
            bool _segConflict;
            switch (seglevel)
            {
                case 1:
                    _segConflict = segregationCase1(a, b, ship);
                    break;
                case 2:
                    _segConflict = segregationCase2(a, b, ship);
                    break;
                case 3:
                    _segConflict = segregationCase3(a, b, ship);
                    break;
                case 4:
                    _segConflict = segregationCase4(a, b, ship);
                    break;
                case 5:
                    _segConflict = segregationCase5(a, b, ship);
                    break;
                case 0:
                    _segConflict = false;
                    break;
                default:
                    _segConflict = false;
                    break;
            }
            return _segConflict;
        }


        /// <summary>
        /// Method checks segregation other than segregation table and 16b
        /// </summary>
        /// <param name="dglist"></param>
        public static  void postSegregation(DgList dglist, ShipProfile ship, ObservableCollection<Container> reefers)
        {
            bool fullReCheck = false;
            int[] Table72631 = { 2014, 2984, 3105, 3107, 3109, 3149 };
            int[] Table72632 = { 1295, 1818, 2189 };
            int[] Table72633 = { 3391, 3392, 3393, 3394, 3395, 3396, 3397, 3398, 3399, 3400 };
            int[] Classes72721 = { 1942, 2067, 1451, 2722, 1486, 1477, 1498, 1446, 2464, 1454, 1474, 1507 };
            int[] BlastingExplosives = { 81, 82, 84, 241, 331, 332};

            foreach (Dg a in dglist)
            { 
            
                //1. 7.2.6.3.2
                #region compatible cargoes
                if (Table72631.Contains(a.unno))
                {
                    foreach(Dg b in dglist)
                    {
                        if (Table72631.Contains(b.unno))
                        {
                            Conflict.ReplaceConflict(a, b, "SGC2");
                        }
                    }
                }
                if (Table72632.Contains(a.unno))
                {
                    foreach (Dg b in dglist)
                    {
                        if (Table72632.Contains(b.unno))
                        {
                            Conflict.ReplaceConflict(a, b, "SGC2");
                        }
                    }
                }
                if (Table72633.Contains(a.unno))
                {
                    foreach (Dg b in dglist)
                    {
                        if (Table72633.Contains(b.unno))
                        {
                            Conflict.ReplaceConflict(a, b, "SGC2");
                        }
                    }
                }
#endregion

                //2. 3.4.4.2                
                #region class 1.4S in limited quantity
                if (a.LQ && (a.dgclass == "1.4" && (a.compatibilityGroup == 'S' || a.compatibilityGroup == '0')))
                    foreach (var dgB in dglist)
                    {
                        if (!dgB.dgclass.StartsWith("1")) continue;
                        switch (dgB.compatibilityGroup)
                        {
                            case 'A':
                            case 'L':
                                a.AddConflict((a.holdNr == dgB.holdNr || a.cntrNr == dgB.cntrNr), segr, "SGC5", dgB);
                                break;
                            case '0':
                                a.AddConflict((a.holdNr == dgB.holdNr || a.cntrNr == dgB.cntrNr), segr, "SGC6", dgB);
                                break;
                        }
                    }

                #endregion

                //3. 7.2.7.2.1
                #region Segregation of class 1 from goods of other classes
                if (a.conflicted && Classes72721.Contains(a.unno) && a.conflict.Contains(BlastingExplosives))
                    foreach (Dg b in dglist)
                    {
                        if (BlastingExplosives.Contains(b.unno) && a.conflict.Contains(b))
                            Conflict.ReplaceConflict(a, b, "EXPLOTHERS");
                        if (Athwartships(a, b, 1, ship.row00exists) && ForeAndAft(a, b, 1) &&
                            a.underdeck == b.underdeck || !a.closed && !b.closed && a.underdeck &&
                            b.underdeck && a.holdNr == b.holdNr)
                                {
                                    a.segregatorClass = b.dgclass;
                                    fullReCheck = true;
                                }
                    }
                    #endregion

             }

            if(fullReCheck)
                foreach (Dg a in dglist)
                    Segregate(a, dglist, reefers, ship);
        }

        /// <summary>
        /// Method checks segregation of class 1, 2.1 and 3 from reefers and adds conflict to Dg if failure.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="reefers"></param>
        /// <param name="row00exists"></param>
        /// <param name="reeferMotorFacing"></param>
        /// <returns></returns>
        public static void ReefersComplianceSegregationCheck(Dg unit, ObservableCollection<Container> reefers, bool row00exists, byte reeferMotorFacing)
        {
            bool result = false;
            //Check for explosives
            if (unit.dgclass.StartsWith("1") || (unit.segregatorClass != null && unit.segregatorClass.StartsWith("1")))
            {
                ExplosivesWithReefersCheck(unit, reefers, row00exists, reeferMotorFacing);
                return;
            }
            //Check for classes 2.1 and 3
            if (!unit.allDgClasses.Contains("2.1") && !unit.allDgClasses.Contains("3") ||
                (unit.allDgClasses.Contains("3") && (unit.dgfp >= 23 && !(Math.Abs(unit.dgfp - 9999) < 1))))
                return;
            //Check for reefers in the same hold
            else if (unit.underdeck)
            {
                foreach (var reeferU in reefers)
                        unit.AddConflict((reeferU.holdNr == unit.holdNr && reeferU.underdeck), segr, "SGC3", reeferU.ConvertToDg());
            }
            else
                foreach (var reeferOn in reefers)
                {
                    if (reeferOn.tier > unit.tier + 2 || reeferOn.tier < unit.tier - 2) continue;
                    result = false;
                    if (!reeferOn.underdeck)
                        switch (reeferMotorFacing)
                        {
                            case (byte)ShipProfile.MotorFacing.Aft:
                                if (reeferOn.bay - unit.bay <= 1 && reeferOn.bay - unit.bay > -4 ||
                                    reeferOn.size == 40 && reeferOn.bay == unit.bay - 4)
                                    result = Athwartships(reeferOn.ConvertToDg(), unit, 1, row00exists);
                                break;
                            case (byte)ShipProfile.MotorFacing.Forward:
                                if (reeferOn.bay - unit.bay < 4 && reeferOn.bay - unit.bay >= -1 ||
                                    reeferOn.size == 40 && reeferOn.bay == unit.bay + 4)
                                    result = Athwartships(reeferOn.ConvertToDg(), unit, 1, row00exists);
                                break;
                            default:
                                if ((reeferOn.bay - unit.bay < 4 && reeferOn.bay - unit.bay > -4) ||
                                    (reeferOn.size == 40 && (reeferOn.bay == unit.bay + 4 || reeferOn.bay == unit.bay - 4)))
                                    result = Athwartships(reeferOn.ConvertToDg(), unit, 1, row00exists);
                                break;
                        }
                        unit.AddConflict(result, segr, "SGC4", reeferOn.ConvertToDg());
                }
        }

        /// <summary>
        /// Method checks if an explosive segregated with reefers (without checking wheather it is an explosive)
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="reefers"></param>
        /// <param name="row00exists"></param>
        /// <param name="reeferMotorFacing"></param>
        public static void ExplosivesWithReefersCheck(Dg unit, ObservableCollection<Container> reefers, bool row00exists, byte reeferMotorFacing)
        {
                foreach (var reeferOn in reefers)
                {
                    bool result = false;
                    switch (reeferMotorFacing)
                    {
                        case (byte) ShipProfile.MotorFacing.Aft:
                            if (reeferOn.bay - unit.bay <= 1 && reeferOn.bay - unit.bay > -4 ||
                                reeferOn.size == 40 && reeferOn.bay == unit.bay - 4)
                                result = Athwartships(reeferOn.ConvertToDg(), unit, 3, row00exists);
                            break;
                        case (byte) ShipProfile.MotorFacing.Forward:
                            if (reeferOn.bay - unit.bay < 4 && reeferOn.bay - unit.bay >= -1 ||
                                reeferOn.size == 40 && reeferOn.bay == unit.bay + 4)
                                result = Athwartships(reeferOn.ConvertToDg(), unit, 3, row00exists);
                            break;
                        default:
                            if ((reeferOn.bay - unit.bay < 4 && reeferOn.bay - unit.bay > -4) ||
                                (reeferOn.size == 40 &&
                                        (reeferOn.bay == unit.bay + 4 || reeferOn.bay == unit.bay - 4)))
                                    result = Athwartships(reeferOn.ConvertToDg(), unit, 3, row00exists);
                                break;
                        }
                        unit.AddConflict(result, segr, "SGC7", reeferOn.ConvertToDg());
                }
        }

        /// <summary>
        /// Method checks if a dg unit of class 7 has another unit of class 7 within 6 mtrs and adds conflct in that case
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="dglist"></param>
        /// <param name="row00exists"></param>
        private static void RadioactiveSegregationCheck(Dg unit, DgList dglist, bool row00exists)
        {
            if (unit.dgclass != "7") return;
            foreach (Dg dg in dglist)
                if (dg.dgclass == "7" && dg.cntrNr != unit.cntrNr)
                    unit.AddConflict((ForeAndAft(unit, dg, 1) && Athwartships(unit, dg, 3, row00exists)), segr, "SGC8", dg);
        }


        //---------------------------------- Supporting methods -------------------------------------------------------------------


        /// <summary>
        /// Supporting methods used inside other public methods
        /// </summary>
        private static bool segregationCase5(Dg a, Dg b, ShipProfile ship)
        {
        //Closed cargo transport units carrying different goods of class 1 do not require segregation from each other provided 7.2.7.1.4 authorizes the goods to be transported together. Where this is not permitted, closed cargotransport unit shall be “separated from” one another.
            bool _segConflict;

            byte[,] explosivesSeg =
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

            char[] cGroupCodes = new char[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'N', 'S'};

            //segregation according to compatibility group
            if (a.dgclass.Length > 3 && b.dgclass.Length > 3)
            {
                if (a.compatibilityGroup == '0') a.compatibilityGroup = char.ToUpper(char.Parse(a.dgclass.Remove(0, 3)));
                if (b.compatibilityGroup == '0') b.compatibilityGroup = char.ToUpper(char.Parse(b.dgclass.Remove(0, 3)));
                byte _permit = explosivesSeg[Array.IndexOf(cGroupCodes, a.compatibilityGroup), 
                                             Array.IndexOf(cGroupCodes, b.compatibilityGroup)];
                if (_permit == 9) _segConflict = false;
                //closed units
                else if (a.closed && b.closed) _segConflict = segregationCase2(a, b, ship);
                //under deck
                else if (a.underdeck && b.underdeck)
                {
                    //not in the same compartment
                    _segConflict = a.holdNr == b.holdNr;
                }
                else _segConflict = true;

                if (!_segConflict) return _segConflict;
                switch (_permit)
                {
                    case 1:
                        a.AddConflict(true, segr, "EXPL1", b);
                        _segConflict = false;
                        break;
                    case 2:
                        a.AddConflict(true, segr, "EXPL2", b);
                        _segConflict = false;
                        break;
                    case 3:
                        a.AddConflict(true, segr, "EXPL3", b);
                        _segConflict = false;
                        break;
                    case 4:
                        a.AddConflict(true, segr, "EXPL4", b);
                        _segConflict = false;
                        break;
                    case 5:
                        a.AddConflict(true, segr, "EXPL5", b);
                        _segConflict = false;
                        break;
                    case 6:
                        a.AddConflict(true, segr, "EXPL6", b);
                        _segConflict = false;
                        break;
                    default: break;
                }
            }
            //when no compatibility group provided
            else
            {
                
                //closed cargo transport units
                if (a.closed && b.closed) _segConflict = segregationCase2(a, b, ship);
                //others - under deck
                else
                {
                    a.AddConflict(true, segr, "EXPL0", b);
                    if (a.underdeck && b.underdeck)
                    {
                        //not in the same compartment
                        _segConflict = a.holdNr == b.holdNr;
                    }
                    //all other cases
                    else _segConflict = true;

                }
            }
            return _segConflict;
        }

        private static bool segregationCase4(Dg a, Dg b, ShipProfile ship)
        {
            bool _segConflict=false;
            //vertical and athwartships = prohibited
            if (a.bay == b.bay || a.bay == b.bay + 1 || a.bay == b.bay - 1) _segConflict = true;
            //Minimum horizontal distance of 24 m on deck for any combination
            else if ((a.underdeck != b.underdeck) || (!a.underdeck && !b.underdeck))
            {
                if ((a.bay - b.bay <= 9) && (b.bay - a.bay <= 9)) _segConflict = true;
            }
            //Under deck
            //open versus closed or two open inside hold
            else if (!a.closed || !b.closed)
            {
                //minimum two bulkheads
                if (a.holdNr - b.holdNr <=1 && b.holdNr - a.holdNr <= 1) _segConflict = true;
            }
            //closed versus closed
            else 
            {
                if ((a.bay - b.bay <= 9 && b.bay-a.bay <=9) ||  //less than 24m
                    a.holdNr == b.holdNr ||                     //same hold
                    //loaction of a referent container closer than 6m from intervening bulkhead:
                    (a.bay > b.bay && 
                        (a.bay == ship.Holds[a.holdNr - 1].firstBay || a.bay == ship.Holds[a.holdNr - 1].firstBay + 1 ||
                        b.bay == ship.Holds[b.holdNr - 1].lastBay || b.bay == ship.Holds[b.holdNr - 1].lastBay - 1))
                    ||
                    (a.bay < b.bay &&
                     (a.bay == ship.Holds[a.holdNr - 1].lastBay || a.bay == ship.Holds[a.holdNr - 1].lastBay - 1 ||
                      b.bay == ship.Holds[b.holdNr - 1].firstBay || b.bay == ship.Holds[b.holdNr - 1].firstBay + 1)))
               _segConflict = true;
            }
            return _segConflict;
        }

        private static bool segregationCase3(Dg a, Dg b, ShipProfile ship)
        {
            bool _segConflict=false;

            //if one on deck and another is underdeck (segregated by deck) = no conflict
            if (a.underdeck != b.underdeck) return _segConflict;

            //two open containers
            if (!a.closed && !b.closed)
            {
                //underdeck - segregated by 2 bulkheads
                byte[] _holds = {a.holdNr, a.holdNr++, a.holdNr-- };
                if (a.underdeck)
                {
                    _segConflict = _holds.Contains(b.holdNr);
                }

                //on deck - segregated by two container spaces fore and aft, and three spaces athwartships
                //fore and aft - two container space
                else
                {
                    _segConflict = Athwartships(a, b, 2, ship.row00exists) && ForeAndAft(a, b, 2);
                    if (!_segConflict) _segConflict = Athwartships(a, b, 3, ship.row00exists) && ForeAndAft(a, b, 1);
                }

            }
            //two closed or open versus closed
            else
            {
                //One container space or one bulkhead
                //Underdeck = to be segregated by bulkhead
                if (a.underdeck) _segConflict = a.holdNr == b.holdNr;
                // On deck
                //find if within one space fore and aft (on deck or in the same hold)
                else
                {
                    _segConflict = Athwartships(a, b, 2, ship.row00exists) && ForeAndAft(a, b, 1);
                }
            }
            return _segConflict;
        }

        private static  bool segregationCase2(Dg a, Dg b, ShipProfile ship)
        {
            bool _segConflict=false;
            //if one on deck and another is underdeck (segregated by deck) = no conflict
            //In the same compartment or on deck
            if ((a.underdeck != b.underdeck) || (a.underdeck && (a.holdNr != b.holdNr)))
                return _segConflict;
            //two closed containers |or| open versus closed on deck
            if ((a.closed && b.closed) || (a.closed != b.closed && !a.underdeck))
            {
                //One container space or one bulkhead
                //find if within one space fore and aft and one container space athwartships(on deck or in the same hold)
                _segConflict = Athwartships(a, b, 1, ship.row00exists) && ForeAndAft(a,b,1);
            }
            //open versus closed in hold |or| two open on deck
            else if ((a.closed != b.closed && a.underdeck) ||
                     (!a.closed && !b.closed && !a.underdeck))
                //fore and aft - One container space, two container space athwartships
            {
                _segConflict = Athwartships(a, b, 2, ship.row00exists) && ForeAndAft(a, b, 1);
            }
            //two open containers in the same cargo hold
            else _segConflict = true;
            return _segConflict;
        }

        private static bool segregationCase1(Dg a, Dg b, ShipProfile ship)
        {
            bool _segConflict=false;
            //if one on deck and another is underdeck (segregated by deck) = no conflict            
            //and two closed containers = no conflict
            if ((!a.closed || !b.closed) && (a.underdeck == b.underdeck))
            {
                //open versus closed in the same stack
                if (a.closed != b.closed && a.row == b.row)
                {
                    a.AssignStack();
                    //open on top of closed = permitted
                    //closed over open and NOT segregated by deck = conflict
                    if (a.Stack.Contains(b.bay) && (a.closed && a.tier > b.tier || b.closed && b.tier > a.tier))
                       _segConflict = true;
                }
                //two open containers
                else if (!a.closed && !b.closed)
                {
                    //in the same vertical line = not allowed unless segregated by deck
                    //Horizontal - one container space unless segregated by bulkhead
                    //find if within one space fore and aft and athwartships (on deck or in the same hold)
                    if ((a.underdeck && a.holdNr == b.holdNr) || !a.underdeck)
                    {
                        _segConflict = (ForeAndAft(a, b, 1) && Athwartships(a, b, 1, ship.row00exists));
                    }
                }
            }
            return _segConflict;
        }


        /// <summary>
        /// Method determines row numbers port and stbd of a selected container at required amount of spacing.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="_rows"></param>
        /// <param name="_row00exists"></param>
        /// <returns></returns>
        private static List<byte> Athwartships (Dg a, byte _rows, bool _row00exists)
        {
            List<byte> ath=new List<byte>();
            switch (_rows)
            {
                case 1:
                    ath.Add(a.row);
                    ath.Add((byte)(a.row + 2));
                    ath.Add((byte)(a.row - 2));
                    if (a.row == 0) ath.Add(1);
                    if (a.row == 1) ath.Add((byte)(_row00exists ? 0 : 2));
                    if (a.row == 2 && !_row00exists) ath.Add(1);
                    break;
                case 2:
                    ath.Add(a.row);
                    ath.Add((byte)(a.row + 2));
                    ath.Add((byte)(a.row + 4));
                    ath.Add((byte)(a.row - 2));
                    ath.Add((byte)(a.row - 4));
                    if (a.row == 0) { ath.Add(1); ath.Add(3); }
                    if (a.row == 1)
                    {
                        ath.Add(2);
                        if (!_row00exists) ath.Add(4);
                    }
                    if (a.row == 2)
                    {
                        ath.Add(1);
                        if (!_row00exists) ath.Add(3);
                    }
                    if (a.row == 3 && !_row00exists) ath.Add(2);
                    if (a.row == 4 && !_row00exists) ath.Add(1);
                    if (a.row <= 4) ath.Add(0);
                    break;
                case 3:
                    ath.Add(a.row);
                    ath.Add((byte)(a.row + 2));
                    ath.Add((byte)(a.row + 4));
                    ath.Add((byte)(a.row + 6));
                    ath.Add((byte)(a.row - 2));
                    ath.Add((byte)(a.row - 4));
                    ath.Add((byte)(a.row - 6));
                    if (a.row == 0) { ath.Add(1); ath.Add(3); ath.Add(5); }
                    if (a.row == 1)
                    {
                        ath.Add(2); ath.Add(4);
                        if (!_row00exists) ath.Add(6);
                    }
                    if (a.row == 2)
                    {
                        ath.Add(1); ath.Add(3);
                        if (!_row00exists) ath.Add(5);
                    }
                    if (a.row == 3)
                    {
                        ath.Add(2);
                        if (!_row00exists) ath.Add(4);
                    }
                    if (a.row == 4)
                    {
                        ath.Add(1);
                        if (!_row00exists) ath.Add(3);
                    }
                    if(a.row == 5 && !_row00exists) ath.Add(2);
                    if (a.row == 6 && !_row00exists) ath.Add(1);
                    if (a.row <= 6 && !ath.Contains(0)) ath.Add(0);
                    break;
                default:
                    break;
            }
            return ath;
        }

        /// <summary>
        /// Method determines if dg b falls within given container spaces from dg a.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="_rows"></param>
        /// <param name="_row00exists"></param>
        /// <returns></returns>
        public static bool Athwartships(Dg a, Dg b, byte _rows, bool _row00exists)
        {
            return Athwartships(a, _rows, _row00exists).Contains(b.row);
        }

        /// <summary>
        /// Method determines bay numbers fore and aft from the unit at required number of spacing.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="_bays"></param>
        /// <returns></returns>
        private static List<byte> ForeAndAft(Dg a, byte _bays)
        {
            List<byte> bays = new List<byte>();
            int x;
            switch (_bays)
            {
                case 1:
                    x = a.size == 40 ? 4 : 3;
                    break;
                case 2:
                    x = 5;
                    break;
                default:
                    x = 0;
                    break;
            }

            for (int i = -x; i <= x; i++)
            {
                bays.Add((byte)(a.bay+i));
            }
            return bays;
        }

        /// <summary>
        /// Method determines if dg b falls under given spacing fore and aft from dg a.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="_bays"></param>
        /// <returns></returns>
        public static bool ForeAndAft(Dg a, Dg b, byte _bays)
        {
            return ForeAndAft(a,_bays).Contains(b.bay);
        }

    }

    
}
