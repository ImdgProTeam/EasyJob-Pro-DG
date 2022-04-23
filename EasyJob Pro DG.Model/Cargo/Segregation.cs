using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public partial class Segregation
    {
        private const string segr = "segregation";

        private readonly ShipProfile Ship;

        public enum SegregationCase : byte
        {
            None = 0,
            AwayFrom = 1,
            SeparatedFrom,
            SeparatedByCompleteCompartmentFrom,
            SeparatedLongitudinallyByCompleteCompartmentFrom,
            SeparationBetweenExplosives
        }

        private static readonly int[,] SegregationTable = IMDGCode.SegregationTable;



        //----------------------------- Public constructors ---------------------------------------------------

        public Segregation()
        { }

        public Segregation(ShipProfile ship)
        {
            Ship = ship;
        }


        //--------------------- Segregation methods ----------------------------------------------------------------

        /// <summary>
        /// Method finds if there is a segregation conflict between two dg units 
        /// taking into account special segregation requirements (segregatorClass and segregatorException).
        /// Returns true if conflict exists.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        public static bool Segregate(Dg a, Dg b, ShipProfile ship)
        {
            bool _conf = false;
            byte seglevel = 0,
                _seglevel,
                _seglevel5 = 0;

            //case dg a has NO segregator class
            if (a.SegregatorClass == null)
            {

                //case there are no segregator classes
                if (b.SegregatorClass == null)
                {
                    foreach (string cla in a.AllDgClassesList)
                    {
                        foreach (string clb in b.AllDgClassesList)
                        {
                            _seglevel = (byte)Segregate(cla, clb);
                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }

                }

                //case only dg b has a segregator class
                else
                {
                    if (b.SegregatorException == null)
                    {
                        foreach (string cla in a.AllDgClassesList)
                        {
                            _seglevel = (byte)Segregate(cla, b.SegregatorClass);
                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                    else //b.segregatorException != null
                    {
                        foreach (string cla in a.AllDgClassesList)
                        {
                            if (cla == b.SegregatorException.SegrClass) _seglevel = b.SegregatorException.SegrCase; //except for class ... 
                            else if (b.SegregatorException.SegrClass == "1" && cla.StartsWith("1")) _seglevel = b.SegregatorException.SegrCase; //in relation to goods of class 1
                            else _seglevel = (byte)Segregate(cla, b.SegregatorClass);
                            if (_seglevel == 99) _seglevel = (byte)Segregate(cla, b.DgClass); //segregation as for the primary hazard 7.2.8

                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;

                        }
                    }
                }
            }

            //case dg a has a segregator class
            else
            {
                //case ONLY dg a has a segregator class
                if (b.SegregatorClass == null)
                {
                    if (a.SegregatorException == null)
                    {
                        foreach (string clb in b.AllDgClassesList)
                        {
                            _seglevel = (byte)Segregate(a.SegregatorClass, clb);
                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                    else //a.segregatorException != null
                    {
                        foreach (string clb in b.AllDgClassesList)
                        {
                            if (clb == a.SegregatorException.SegrClass) _seglevel = a.SegregatorException.SegrCase; //except for class ... 
                            else if (a.SegregatorException.SegrClass == "1"
                                && clb.StartsWith("1")) _seglevel = a.SegregatorException.SegrCase; //in relation to goods of class 1
                            else _seglevel = (byte)Segregate(a.SegregatorClass, clb);
                            if (_seglevel == 99) _seglevel = (byte)Segregate(a.DgClass, clb); //segregation as for the primary hazard 7.2.8

                            if (_seglevel == 5) _seglevel5 = _seglevel;
                            else seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                }
                //case both dgs have segregator classes
                else
                {
                    if (a.SegregatorException == null)
                    {
                        //both have NO segregatorException
                        if (b.SegregatorException == null)
                        {
                            seglevel = (byte)Segregate(a.SegregatorClass, b.SegregatorClass);
                        }
                        else //b.segregatorException != null
                        {
                            if (a.SegregatorClass == b.SegregatorException.SegrClass) _seglevel = b.SegregatorException.SegrCase; //except for class ... 
                            else if (b.SegregatorException.SegrClass == "1"
                                && a.SegregatorClass.StartsWith("1")) _seglevel = b.SegregatorException.SegrCase; //in relation to goods of class 1
                            else _seglevel = (byte)Segregate(a.SegregatorClass, b.SegregatorClass);
                            if (_seglevel == 99) _seglevel = (byte)Segregate(a.SegregatorClass, b.DgClass); //segregation as for the primary hazard 7.2.8

                            seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                    else //a.segregatorException != null
                    {
                        if (b.SegregatorException == null)
                        {
                            if (a.SegregatorException.SegrClass == b.SegregatorClass) _seglevel = a.SegregatorException.SegrCase;
                            else if (a.SegregatorException.SegrClass == "1"
                                && b.SegregatorClass.StartsWith("1")) _seglevel = a.SegregatorException.SegrCase; //in relation to goods of class 1
                            else _seglevel = (byte)Segregate(a.SegregatorClass, b.SegregatorClass);
                            if (_seglevel == 99) _seglevel = (byte)Segregate(a.DgClass, b.SegregatorClass); //segregation as for the primary hazard 7.2.8

                            seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                        else //both have segregatorException
                        {
                            byte s1 = 0, s2 = 0;
                            if (a.SegregatorClass == b.SegregatorException.SegrClass
                                || a.SegregatorException.SegrClass == b.SegregatorClass)
                            {
                                byte l1 = a.SegregatorClass == b.SegregatorException.SegrClass
                                    ? a.SegregatorException.SegrCase : (byte)0;
                                byte l2 = a.SegregatorException.SegrClass == b.SegregatorClass
                                    ? b.SegregatorException.SegrCase : (byte)0;
                                _seglevel = l1 > l2 ? l1 : l2;
                            }

                            else if (a.SegregatorException.SegrClass == "1" && b.SegregatorClass.StartsWith("1")
                                || b.SegregatorException.SegrClass == "1" && a.SegregatorClass.StartsWith("1"))
                            {
                                s1 = a.SegregatorException.SegrClass == "1" && b.SegregatorClass.StartsWith("1")
                                    ? a.SegregatorException.SegrCase : (byte)0;
                                s2 = b.SegregatorException.SegrClass == "1" && a.SegregatorClass.StartsWith("1")
                                    ? b.SegregatorException.SegrCase : (byte)0;
                                _seglevel = s1 > s2 ? s1 : s2;
                            } //in relation to goods of class 1

                            else _seglevel = (byte)Segregate(a.SegregatorClass, b.SegregatorClass);
                            if (_seglevel == 99)
                            {
                                _seglevel = s1 > s2
                                    ? (byte)Segregate(a.DgClass, b.SegregatorClass)
                                    : (byte)Segregate(b.DgClass, a.SegregatorClass);
                            }
                            //segregation as for the primary hazard 7.2.8

                            seglevel = _seglevel > seglevel ? _seglevel : seglevel;
                        }
                    }
                }
            }

            if (_seglevel5 != 0) _conf = SegregationConflictCheck(a, b, _seglevel5, ship);
            if (SegregationConflictCheck(a, b, seglevel, ship)) _conf = true;
            return _conf;
        }

        /// <summary>
        /// Method checks if two dg containers are properly segregated according to segregation level
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="seglevel"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        public static bool SegregationConflictCheck(Dg a, Dg b, byte seglevel, ShipProfile ship)
        //implemented only for container ships with closed cargo holds
        {
            //true if conflicted
            bool segConflict;
            switch (seglevel)
            {
                case 1:
                    segConflict = SegregationCase1(a, b, ship);
                    break;
                case 2:
                    segConflict = SegregationCase2(a, b, ship);
                    break;
                case 3:
                    segConflict = SegregationCase3(a, b, ship);
                    break;
                case 4:
                    segConflict = SegregationCase4(a, b, ship);
                    break;
                case 5:
                    segConflict = SegregationCase5(a, b, ship);
                    break;
                case 0:
                    segConflict = false;
                    break;
                default:
                    segConflict = false;
                    break;
            }
            return segConflict;
        }

        /// <summary>
        /// Method gets segregation level of 2 DG classes in Segregation Table
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
            return SegregationTable[a.dgRowInTable, b.dgRowInTable];
        }

        /// <summary>
        /// Method checks if an explosive segregated with reefers (without checking wheather it is an explosive)
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="reefers"></param>
        /// <param name="row00Exists"></param>
        /// <param name="reeferMotorFacing"></param>
        private static void ExplosivesWithReefersCheck(Dg unit, IEnumerable<Container> reefers, bool row00Exists, byte reeferMotorFacing)
        {
            foreach (var reeferOn in reefers)
            {
                bool result = false;
                switch (reeferMotorFacing)
                {
                    case (byte)ShipProfile.MotorFacing.Aft:
                        if (reeferOn.Bay - unit.Bay <= 1 && reeferOn.Bay - unit.Bay > -4 ||
                            reeferOn.Size == 40 && reeferOn.Bay == unit.Bay - 4)
                            result = Athwartship(reeferOn.ConvertToDg(), unit, 3, row00Exists);
                        break;
                    case (byte)ShipProfile.MotorFacing.Forward:
                        if (reeferOn.Bay - unit.Bay < 4 && reeferOn.Bay - unit.Bay >= -1 ||
                            reeferOn.Size == 40 && reeferOn.Bay == unit.Bay + 4)
                            result = Athwartship(reeferOn.ConvertToDg(), unit, 3, row00Exists);
                        break;
                    default:
                        if ((reeferOn.Bay - unit.Bay < 4 && reeferOn.Bay - unit.Bay > -4) ||
                            (reeferOn.Size == 40 &&
                                    (reeferOn.Bay == unit.Bay + 4 || reeferOn.Bay == unit.Bay - 4)))
                            result = Athwartship(reeferOn.ConvertToDg(), unit, 3, row00Exists);
                        break;
                }
                unit.AddConflict(result, segr, "SGC7", reeferOn.ConvertToDg());
            }
        }

        /// <summary>
        /// Method checks segregation other than segregation table and 16b
        /// </summary>
        /// <param name="cargoplan"></param>
        /// <param name="ship"></param>
        internal static void PostSegregation(CargoPlan cargoplan, ShipProfile ship)
        {
            ICollection<Dg> dglist = cargoplan.DgList;

            #region Definitions
            bool fullReCheck = false;
            ushort[] table72631 = IMDGCode.Table72631;
            ushort[] table72632 = IMDGCode.Table72632;
            ushort[] table72633 = IMDGCode.Table72633;
            ushort[] Table72634 = IMDGCode.Table72634;
            ushort[] classes72721 = IMDGCode.Classes72721;
            ushort[] blastingExplosives = IMDGCode.BlastingExplosives;
            #endregion

            foreach (Dg a in dglist)
            {

                //1. 7.2.6.3.2
                #region compatible cargoes
                if (table72631.Contains(a.Unno))
                {
                    foreach (Dg b in dglist)
                    {
                        if (a == b || a.Unno == b.Unno) continue;
                        if (table72631.Contains(b.Unno))
                        {
                            Conflicts.ReplaceConflict(a, b, "SGC2");
                        }
                    }
                }
                if (table72632.Contains(a.Unno))
                {
                    foreach (Dg b in dglist)
                    {
                        if (a == b || a.Unno == b.Unno) continue;
                        if (table72632.Contains(b.Unno))
                        {
                            Conflicts.ReplaceConflict(a, b, "SGC2");
                        }
                    }
                }
                if (table72633.Contains(a.Unno))
                {
                    foreach (Dg b in dglist)
                    {
                        if (a == b || a.Unno == b.Unno) continue;
                        if (table72633.Contains(b.Unno))
                        {
                            Conflicts.ReplaceConflict(a, b, "SGC2");
                        }
                    }
                }
                if (Table72634.Contains(a.Unno))
                {
                    foreach (Dg b in dglist)
                    {
                        if (a == b || a.Unno == b.Unno) continue;
                        if (Table72634.Contains(b.Unno))
                        {
                            if (a.Unno == 1325 || b.Unno == 1325) Conflicts.ReplaceConflict(a, b, "SGC203");
                            else Conflicts.ReplaceConflict(a, b, "SGC202");
                        }
                    }
                }
                #endregion

                //2. 3.4.4.2                
                #region class 1.4S in limited quantity
                if (a.IsLq && (a.DgClass == "1.4" && (a.CompatibilityGroup == 'S' || a.CompatibilityGroup == '0')))
                    foreach (var dgB in dglist)
                    {
                        if (!dgB.DgClass.StartsWith("1")) continue;
                        switch (dgB.CompatibilityGroup)
                        {
                            case 'A':
                            case 'L':
                                a.AddConflict((a.HoldNr == dgB.HoldNr || a.ContainerNumber == dgB.ContainerNumber), segr, "SGC5", dgB);
                                break;
                            case '0':
                                a.AddConflict((a.HoldNr == dgB.HoldNr || a.ContainerNumber == dgB.ContainerNumber), segr, "SGC6", dgB);
                                break;
                        }
                    }

                #endregion

                //3. 7.2.7.2.1
                #region Segregation of class 1 from goods of other classes
                if (a.IsConflicted && classes72721.Contains(a.Unno) && a.Conflicts.Contains(blastingExplosives))
                    foreach (Dg b in dglist)
                    {
                        if (blastingExplosives.Contains(b.Unno) && a.Conflicts.Contains(b))
                            Conflicts.ReplaceConflict(a, b, "EXPLOTHERS");
                        if (Athwartship(a, b, 1, ship.Row00Exists) && ForeAndAft(a, b, 1) &&
                            a.IsUnderdeck == b.IsUnderdeck || !a.IsClosed && !b.IsClosed && a.IsUnderdeck &&
                            b.IsUnderdeck && a.HoldNr == b.HoldNr)
                        {
                            a.SegregatorClass = b.DgClass;
                            fullReCheck = true;
                        }
                    }
                #endregion

                //4. 1950 aerosols:
                #region Aerosols (UN 1950)

                //Max 1L: ...separated from class 1 except 1.4
                if (a.Unno == 1950 && a.IsMax1L && a.IsConflicted)
                {
                    for (int i = 0; i < a.Conflicts.SegregationConflictsList.Count; i++)
                    {
                        var conflict = a.Conflicts.SegregationConflictsList[i];
                        if (conflict.ConflictContainerClassStr.StartsWith("1.4"))
                        {
                            foreach (var dgB in dglist)
                            {
                                if (dgB.ContainerNumber == conflict.ConflictContainerNr
                                    && dgB.Unno == conflict.ConflictContainerUnno)
                                {
                                    foreach (var dgBSegrConflict in dgB.Conflicts.SegregationConflictsList)
                                    {
                                        if (dgBSegrConflict.ConflictContainerNr == a.ContainerNumber
                                            && dgBSegrConflict.ConflictContainerUnno == 1950
                                            && dgBSegrConflict.ConflictContainerClassStr.StartsWith("2"))
                                        {
                                            dgB.Conflicts.SegregationConflictsList.Remove(dgBSegrConflict);
                                            break;
                                        }
                                    }
                                }
                            }
                            a.Conflicts.SegregationConflictsList.RemoveAt(i);
                            i--;
                        }
                    }
                }


                #endregion

            }

            if (fullReCheck)
                foreach (Dg a in dglist)
                    Segregate(a, cargoplan, ship);
        }

        /// <summary>
        /// Method checks segregation of class 1, 2.1 and 3 from reefers and adds conflict to Dg if failure.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="reefers"></param>
        /// <param name="row00Exists"></param>
        /// <param name="reeferMotorFacing"></param>
        /// <returns></returns>
        private static void ReefersComplianceSegregationCheck(Dg unit, IEnumerable<Container> reefers, bool row00Exists, byte reeferMotorFacing)
        {
            bool result;
            //Check for explosives
            if (unit.DgClass.StartsWith("1") || (unit.SegregatorClass != null && unit.SegregatorClass.StartsWith("1")))
            {
                ExplosivesWithReefersCheck(unit, reefers, row00Exists, reeferMotorFacing);
                return;
            }

            //Check for classes 2.1 and 3
            if (!unit.AllDgClassesList.Contains("2.1") && !unit.AllDgClassesList.Contains("3") ||
                (unit.AllDgClassesList.Contains("3") && (unit.FlashPointDouble >= 23 && !(Math.Abs(unit.FlashPointDouble - 9999) < 1))))
                return;

            //Check for reefers in the same hold
            if (unit.IsUnderdeck)
            {
                foreach (var reeferU in reefers)
                {
                    //check for the same container
                    if (unit.ContainerNumber == reeferU.ContainerNumber) continue;

                    unit.AddConflict((reeferU.HoldNr == unit.HoldNr && reeferU.IsUnderdeck), segr, "SGC3", reeferU.ConvertToDg());
                }

            }
            else
                foreach (var reeferOn in reefers)
                {
                    //check for the same container
                    if (unit.ContainerNumber == reeferOn.ContainerNumber) continue;

                    result = false;
                    if (!reeferOn.IsUnderdeck)
                        switch (reeferMotorFacing)
                        {
                            case (byte)ShipProfile.MotorFacing.Aft:
                                if (reeferOn.Bay - unit.Bay <= 1 && reeferOn.Bay - unit.Bay > -4 ||
                                    reeferOn.Size == 40 && reeferOn.Bay == unit.Bay - 4)
                                    result = Athwartship(reeferOn.ConvertToDg(), unit, 1, row00Exists);
                                break;
                            case (byte)ShipProfile.MotorFacing.Forward:
                                if (reeferOn.Bay - unit.Bay < 4 && reeferOn.Bay - unit.Bay >= -1 ||
                                    reeferOn.Size == 40 && reeferOn.Bay == unit.Bay + 4)
                                    result = Athwartship(reeferOn.ConvertToDg(), unit, 1, row00Exists);
                                break;
                            default:
                                if ((reeferOn.Bay - unit.Bay < 4 && reeferOn.Bay - unit.Bay > -4) ||
                                    (reeferOn.Size == 40 && (reeferOn.Bay == unit.Bay + 4 || reeferOn.Bay == unit.Bay - 4)))
                                    result = Athwartship(reeferOn.ConvertToDg(), unit, 1, row00Exists);
                                break;
                        }
                    unit.AddConflict(result, segr, "SGC4", reeferOn.ConvertToDg());
                }
        }

        /// <summary>
        /// Method checks segregation as per segregation table of a DG unit with dg list and adds a conflict if any.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="cargoPlan"></param>
        /// <param name="ship"></param>
        internal static void Segregate(Dg a, CargoPlan cargoPlan, ShipProfile ship)
        {
            bool _conf = false;
            ICollection<Dg> dglist = cargoPlan.DgList;

            //No segregation required for units in limited quantities
            if (a.IsLq) return;

            //1. Check if there are any special requirements in column 16b of CH 3
            SpecialSegregationCheck(a, dglist, ship);

            //2. Segregation between classes according to table 7.2.4
            //taking into account segregation requirements from //1.

            //checking segregation with every unit in dglist
            foreach (Dg b in dglist)
            {
                if (b.IsLq) continue;
                if (a.Unno == b.Unno) continue; //to exclude segregation of same substances
                if (a.ContainerNumber == b.ContainerNumber) continue; //to exclude segregation with the same container

                //checking if conflict exists
                _conf = Segregate(a, b, ship);

                //adding conflict if exists
                //substances of the same class may be stowed together without regard to segregation required by secondary hazards:
                a.AddConflict(_conf, segr, a.DgClass == b.DgClass ? "SGC21" : "SGC1", b);
            }

            //Checking segregation with the reefers
            ReefersComplianceSegregationCheck(a, cargoPlan.Reefers, ship.Row00Exists, ship.RfMotor);

            //Checking of segregation requirements for class 7
            RadioactiveSegregationCheck(a, cargoPlan, ship.Row00Exists);
        }

        /// <summary>
        /// Method checks if a dg unit of class 7 has another unit of class 7 within 6 mtrs and adds conflct in that case
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="cargoplan"></param>
        /// <param name="row00Exists"></param>
        private static void RadioactiveSegregationCheck(Dg unit, CargoPlan cargoplan, bool row00Exists)
        {
            ICollection<Dg> dglist = cargoplan.DgList;
            if (unit.DgClass != "7") return;
            foreach (Dg dg in dglist)
                if (dg.DgClass == "7" && dg.ContainerNumber != unit.ContainerNumber)
                    unit.AddConflict((ForeAndAft(unit, dg, 1) && Athwartship(unit, dg, 3, row00Exists)), segr, "SGC8", dg);
        }


        //---------------------------------- Supporting methods -------------------------------------------------------------------

        // Supporting methods used inside other public methods

        #region SegregationCases methods
        /// <summary>
        /// Checks segregation between goods of class 1.
        /// </summary>
        /// <param name="a">Dg (class 1)</param>
        /// <param name="b">Dg (class 1)</param>
        /// <param name="ship">ShipProfile</param>
        /// <returns></returns>
        private static bool SegregationCase5(Dg a, Dg b, ShipProfile ship)
        {
            //Closed cargo transport units carrying different goods of class 1 do not require segregation from each other provided 7.2.7.1.4 authorizes the goods to be transported together. Where this is not permitted, closed cargotransport unit shall be “separated from” one another.
            bool segConflict;

            byte[,] explosivesSeg = IMDGCode.ExplosivesPermittedMixedStowage;
            char[] cGroupCodes = IMDGCode.ExplosivesCompatibilityGroupCodes;

            //segregation according to compatibility group
            if (a.DgClass.Length > 3 && b.DgClass.Length > 3)
            {
                if (a.CompatibilityGroup == '0') a.CompatibilityGroup = char.ToUpper(char.Parse(a.DgClass.Remove(0, 3)));
                if (b.CompatibilityGroup == '0') b.CompatibilityGroup = char.ToUpper(char.Parse(b.DgClass.Remove(0, 3)));
                byte permit = explosivesSeg[Array.IndexOf(cGroupCodes, a.CompatibilityGroup),
                                             Array.IndexOf(cGroupCodes, b.CompatibilityGroup)];
                if (permit == 9) segConflict = false;
                //closed units
                else if (a.IsClosed && b.IsClosed) segConflict = SegregationCase2(a, b, ship);
                //under deck
                else if (a.IsUnderdeck && b.IsUnderdeck)
                {
                    //not in the same compartment
                    segConflict = a.HoldNr == b.HoldNr;
                }
                else segConflict = true;

                if (!segConflict) return false;
                switch (permit)
                {
                    case 1:
                        a.AddConflict(true, segr, "EXPL1", b);
                        segConflict = false;
                        break;
                    case 2:
                        a.AddConflict(true, segr, "EXPL2", b);
                        segConflict = false;
                        break;
                    case 3:
                        a.AddConflict(true, segr, "EXPL3", b);
                        segConflict = false;
                        break;
                    case 4:
                        a.AddConflict(true, segr, "EXPL4", b);
                        segConflict = false;
                        break;
                    case 5:
                        a.AddConflict(true, segr, "EXPL5", b);
                        segConflict = false;
                        break;
                    case 6:
                        a.AddConflict(true, segr, "EXPL6", b);
                        segConflict = false;
                        break;
                    default: break;
                }
            }
            //when no compatibility group provided
            else
            {

                //closed cargo transport units
                if (a.IsClosed && b.IsClosed) segConflict = SegregationCase2(a, b, ship);
                //others - under deck
                else
                {
                    a.AddConflict(true, segr, "EXPL0", b);
                    if (a.IsUnderdeck && b.IsUnderdeck)
                    {
                        //not in the same compartment
                        segConflict = a.HoldNr == b.HoldNr;
                    }
                    //all other cases
                    else segConflict = true;

                }
            }
            return segConflict;
        }


        /// <summary>
        /// Checks "Separated longitudinally by an intervening complete compartment or hold from"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static bool SegregationCase4(Dg a, Dg b, ShipProfile ship)
        {
            bool segConflict = false;


            //vertical and athwartship = prohibited
            if (a.Bay == b.Bay || a.Bay == b.Bay + 1 || a.Bay == b.Bay - 1) segConflict = true;

            //Minimum horizontal distance of 24 m on deck for any combination
            else if ((a.IsUnderdeck != b.IsUnderdeck) || (!a.IsUnderdeck && !b.IsUnderdeck))
            {
                if ((a.Bay - b.Bay <= 9) && (b.Bay - a.Bay <= 9)) segConflict = true;
            }

            //Under deck
            //open versus closed or two open inside hold
            else if (!a.IsClosed || !b.IsClosed)
            {
                //minimum two bulkheads
                if (a.HoldNr - b.HoldNr <= 1 && b.HoldNr - a.HoldNr <= 1) segConflict = true;
            }

            //closed versus closed
            else
            {
                //less than 24m
                if ((a.Bay - b.Bay <= 9 && b.Bay - a.Bay <= 9) || a.HoldNr == b.HoldNr)
                    segConflict = true;

                //if two bulkheads - no conflict
                else if (a.HoldNr - b.HoldNr > 1 || b.HoldNr - a.HoldNr > 1)
                    segConflict = false;

                //location of a referent container closer than 6m from intervening bulkhead:
                else if ((a.Bay > b.Bay &&
                        (a.Bay == ship.Holds[a.HoldNr - 1].FirstBay || a.Bay == ship.Holds[a.HoldNr - 1].FirstBay + 1 ||
                        b.Bay == ship.Holds[b.HoldNr - 1].LastBay || b.Bay == ship.Holds[b.HoldNr - 1].LastBay - 1))
                    ||
                    (a.Bay < b.Bay &&
                     (a.Bay == ship.Holds[a.HoldNr - 1].LastBay || a.Bay == ship.Holds[a.HoldNr - 1].LastBay - 1 ||
                      b.Bay == ship.Holds[b.HoldNr - 1].FirstBay || b.Bay == ship.Holds[b.HoldNr - 1].FirstBay + 1)))
                    segConflict = true;
            }
            return segConflict;
        }

        /// <summary>
        /// Checks "Separated by a complete compartment or hold from"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static bool SegregationCase3(Dg a, Dg b, ShipProfile ship)
        {
            bool segConflict;

            //if one on deck and another is underdeck (segregated by deck) = no conflict
            if (a.IsUnderdeck != b.IsUnderdeck) return false;

            //two open containers
            if (!a.IsClosed && !b.IsClosed)
            {
                //underdeck - segregated by 2 bulkheads
                byte[] holds = { a.HoldNr, (byte)(a.HoldNr + 1), (byte)(a.HoldNr - 1) };
                if (a.IsUnderdeck)
                    segConflict = holds.Contains(b.HoldNr);

                //on deck - segregated by two container spaces fore and aft, and three spaces athwartships
                //fore and aft - two container space
                else
                {
                    segConflict = Athwartship(a, b, 2, ship.Row00Exists) && ForeAndAft(a, b, 2);
                    if (!segConflict) segConflict = Athwartship(a, b, 3, ship.Row00Exists) && ForeAndAft(a, b, 1);
                }

            }
            //two closed or open versus closed
            else
            {
                //One container space or one bulkhead
                //Underdeck = to be segregated by bulkhead
                if (a.IsUnderdeck) segConflict = a.HoldNr == b.HoldNr;
                // On deck
                //find if within one space fore and aft (on deck or in the same hold)
                else
                {
                    segConflict = Athwartship(a, b, 2, ship.Row00Exists) && ForeAndAft(a, b, 1);
                }
            }
            return segConflict;
        }

        /// <summary>
        /// Checks "separated from"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static bool SegregationCase2(Dg a, Dg b, ShipProfile ship)
        {
            bool segConflict;
            //if one on deck and another is underdeck (segregated by deck) = no conflict
            //In the same compartment or on deck
            if ((a.IsUnderdeck != b.IsUnderdeck) || (a.IsUnderdeck && (a.HoldNr != b.HoldNr)))
                return false;
            //two closed containers |or| open versus closed on deck
            if ((a.IsClosed && b.IsClosed) || (a.IsClosed != b.IsClosed && !a.IsUnderdeck))
            {
                //One container space or one bulkhead
                //find if within one space fore and aft and one container space athwartships(on deck or in the same hold)
                segConflict = Athwartship(a, b, 1, ship.Row00Exists) && ForeAndAft(a, b, 1);
            }
            //open versus closed in hold |or| two open on deck
            else if ((a.IsClosed != b.IsClosed && a.IsUnderdeck) ||
                     (!a.IsClosed && !b.IsClosed && !a.IsUnderdeck))
            //fore and aft - One container space, two container space athwartships
            {
                segConflict = Athwartship(a, b, 2, ship.Row00Exists) && ForeAndAft(a, b, 1);
            }
            //two open containers in the same cargo hold
            else segConflict = true;
            return segConflict;
        }

        /// <summary>
        /// Checks "away from"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static bool SegregationCase1(Dg a, Dg b, ShipProfile ship)
        {
            bool segConflict = false;
            //if one on deck and another is underdeck (segregated by deck) = no conflict            
            //and two closed containers = no conflict
            if ((!a.IsClosed || !b.IsClosed) && (a.IsUnderdeck == b.IsUnderdeck))
            {
                //open versus closed in the same stack
                if (a.IsClosed != b.IsClosed && a.Row == b.Row)
                {
                    a.AssignStack();
                    //open on top of closed = permitted
                    //closed over open and NOT segregated by deck = conflict
                    if (a.Stack.Contains(b.Bay) && (a.IsClosed && a.Tier > b.Tier || b.IsClosed && b.Tier > a.Tier))
                        segConflict = true;
                }
                //two open containers
                else if (!a.IsClosed && !b.IsClosed)
                {
                    //in the same vertical line = not allowed unless segregated by deck
                    //Horizontal - one container space unless segregated by bulkhead
                    //find if within one space fore and aft and athwartships (on deck or in the same hold)
                    if ((a.IsUnderdeck && a.HoldNr == b.HoldNr) || !a.IsUnderdeck)
                    {
                        segConflict = (ForeAndAft(a, b, 1) && Athwartship(a, b, 1, ship.Row00Exists));
                    }
                }
            }
            return segConflict;
        } 
        #endregion


        /// <summary>
        /// Method determines row numbers port and stbd of a selected container at required amount of spacing.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="rows"></param>
        /// <param name="row00Exists"></param>
        /// <returns></returns>
        private static List<byte> Athwartship(Dg a, byte rows, bool row00Exists)
        {
            List<byte> ath = new List<byte>();
            switch (rows)
            {
                case 1:
                    ath.Add(a.Row);
                    ath.Add((byte)(a.Row + 2));
                    ath.Add((byte)(a.Row - 2));
                    if (a.Row == 0) ath.Add(1);
                    if (a.Row == 1) ath.Add((byte)(row00Exists ? 0 : 2));
                    if (a.Row == 2 && !row00Exists) ath.Add(1);
                    break;
                case 2:
                    ath.Add(a.Row);
                    ath.Add((byte)(a.Row + 2));
                    ath.Add((byte)(a.Row + 4));
                    ath.Add((byte)(a.Row - 2));
                    ath.Add((byte)(a.Row - 4));
                    if (a.Row == 0) { ath.Add(1); ath.Add(3); }
                    if (a.Row == 1)
                    {
                        ath.Add(2);
                        if (!row00Exists) ath.Add(4);
                    }
                    if (a.Row == 2)
                    {
                        ath.Add(1);
                        if (!row00Exists) ath.Add(3);
                    }
                    if (a.Row == 3 && !row00Exists) ath.Add(2);
                    if (a.Row == 4 && !row00Exists) ath.Add(1);
                    if (a.Row <= 4) ath.Add(0);
                    break;
                case 3:
                    ath.Add(a.Row);
                    ath.Add((byte)(a.Row + 2));
                    ath.Add((byte)(a.Row + 4));
                    ath.Add((byte)(a.Row + 6));
                    ath.Add((byte)(a.Row - 2));
                    ath.Add((byte)(a.Row - 4));
                    ath.Add((byte)(a.Row - 6));
                    if (a.Row == 0) { ath.Add(1); ath.Add(3); ath.Add(5); }
                    if (a.Row == 1)
                    {
                        ath.Add(2); ath.Add(4);
                        if (!row00Exists) ath.Add(6);
                    }
                    if (a.Row == 2)
                    {
                        ath.Add(1); ath.Add(3);
                        if (!row00Exists) ath.Add(5);
                    }
                    if (a.Row == 3)
                    {
                        ath.Add(2);
                        if (!row00Exists) ath.Add(4);
                    }
                    if (a.Row == 4)
                    {
                        ath.Add(1);
                        if (!row00Exists) ath.Add(3);
                    }
                    if (a.Row == 5 && !row00Exists) ath.Add(2);
                    if (a.Row == 6 && !row00Exists) ath.Add(1);
                    if (a.Row <= 6 && !ath.Contains(0)) ath.Add(0);
                    break;
                default:
                    break;
            }
            return ath;
        }

        /// <summary>
        /// Method determines Bay numbers fore and aft from the unit at required number of spacing.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="bays"></param>
        /// <returns></returns>
        private static List<byte> ForeAndAft(Dg a, byte bays)
        {
            List<byte> baysList = new List<byte>();
            int x;
            switch (bays)
            {
                case 1:
                    x = a.Size == 40 ? 4 : 3;
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
                baysList.Add((byte)(a.Bay + i));
            }
            return baysList;
        }


        /// <summary>
        /// Method determines if dg b falls within given container spaces from dg a.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="rows"></param>
        /// <param name="row00Exists"></param>
        /// <returns></returns>
        private static bool Athwartship(Dg a, Dg b, byte rows, bool row00Exists)
        {
            return Athwartship(a, rows, row00Exists).Contains(b.Row);
        }

        /// <summary>
        /// Method determines if dg b falls under given spacing fore and aft from dg a.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="bays"></param>
        /// <returns></returns>
        private static bool ForeAndAft(Dg a, Dg b, byte bays)
        {
            return ForeAndAft(a, bays).Contains(b.Bay);
        }

    }
}
