﻿using System.Collections.Generic;
using System.Linq;
using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.Model.Cargo
{
    public partial class Stowage
    {
        private const string STOW = "stowage";
        public static SpecialStowageGroups SWgroups = new SpecialStowageGroups(true);
        static ShipProfile ship => ShipProfile.Instance;


        /// <summary>
        /// Method checks stowage of all dg in cargoplan
        /// </summary>
        /// <param name="cargoplan"></param>
        public static void CheckStowage(CargoPlan cargoplan)
        {
            SWgroups.Clear();
            foreach (Dg dg in cargoplan.DgList)
                CheckUnitStowage(unit: dg, containers: cargoplan.Containers);
            Data.LogWriter.Write($"Stowage checked");
        }

        /// <summary>
        /// Method checks stowage compliance of a selected unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="containers"></param>
        public static void CheckUnitStowage(Dg unit, ICollection<Container> containers)
        {

            if (unit.IsLq) return;

            //Check special stowage
            foreach (string sscode in unit.StowageSWList)
                unit.AddConflict(SpecialStowageCheck(sscode, unit, containers), STOW, sscode);

            //Check stowage category
            unit.AddConflict(StowageCatCheck(unit), STOW, "SSC1");

            //Check against DOC
            unit.AddConflict(!CheckDoc(unit), STOW, "SSC2");

            //Check in respect of explosives
            CheckStowageOfExplosives(unit);

            //Check in respect of marine pollutants
            CheckMarinePollutantAndInfectiousSubstancesStowage(unit);

            //Additional requirements
            CheckAdditionalStowageRequirements(unit, containers);

            //Additional requirements for class 7
            CheckRadioactiveStowage(unit);
        }


        /// <summary>
        /// Checking of protection from direct sun (surrounded by containers from 3 sides).
        /// Returns true if protected
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="containers"></param>
        /// <param name="row00Exists"></param>
        /// <returns></returns>
        internal static bool CheckProtectedUnit(Dg unit, ICollection<Container> containers, bool row00Exists)
        {
            //reset any previous protection
            bool result = false;
            int protectedFromSides = 0;
            bool protectedFromTop = false;
            unit.Surrounded = string.Empty;
            string top = "Nil", left = "Nil", right = "Nil";

            //Checks only containers on deck in the same bay
            var units = from entry in containers
                         where entry.Bay <= (unit.Bay + 1) &&
                               entry.Bay >= (unit.Bay - 1) &&
                               entry.IsUnderdeck == false
                         select entry;

            foreach (var entry in units)
            {
                //check if entry is the same container with reference unit
                if (entry.Location == unit.Location) continue;

                //check if protected from top - if already protected then will skip protection
                if (!protectedFromTop && entry.Row == unit.Row && (entry.Tier > unit.Tier))
                {
                    protectedFromTop = true;
                    top = entry.Location;

                    //checking if already fully protected => return result
                    if (protectedFromSides == 4)
                    {
                        result = true;
                        break;
                    }
                    continue;
                }

                //check if in the same Tier and entry is next to reference unit
                if (protectedFromSides < 4 && entry.Tier == unit.Tier)
                {
                    bool _isProtected = true;
                    if (entry.Row - 2 == unit.Row)
                    {
                        if (unit.Row % 2 == 0)
                            SetSideProtectedEntry(ref left, entry.Location);
                        else
                            SetSideProtectedEntry(ref right, entry.Location);
                    }
                    else if (entry.Row + 2 == unit.Row)
                    {
                        if (unit.Row % 2 == 0)
                            SetSideProtectedEntry(ref right, entry.Location);
                        else
                            SetSideProtectedEntry(ref left, entry.Location);
                    }
                    else if ((entry.Row == 0 && unit.Row == 1) ||
                        (entry.Row == 2 && unit.Row == 1 && !row00Exists))
                    {
                        SetSideProtectedEntry(ref left, entry.Location);
                    }
                    else if ((entry.Row == 1 && unit.Row == 0) ||
                        (entry.Row == 1 && unit.Row == 2 && !row00Exists))
                    {
                        SetSideProtectedEntry(ref right, entry.Location);
                    }
                    else
                    {
                        //no protection
                        _isProtected = false;
                    }

                    if (_isProtected)
                    {
                        if (entry.Size == 40 || entry.Size == unit.Size)
                        {
                            protectedFromSides += 2;
                        }
                        else
                        {
                            protectedFromSides++;
                        }
                    }
                }

                //check for cummulutive result
                result = protectedFromSides == 4 && protectedFromTop;
                if (result) break;
            }

            //finalizing Surrounding and returning the result
            unit.Surrounded = $"port by {left}, starboard by {right}, top by {top}";

            return result;
        }

        /// <summary>
        /// Checks if unit can be NOT considered protected from source of heat.
        /// Returns true if Not protected, false if protected.
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="containers"></param>
        /// <returns></returns>
        internal static bool CheckNotProtectedFromSourceOfHeat(Dg dg, ICollection<Container> containers)
        {
            if (ship.IsInHeatedStructures(dg))
                return true;
            if (!dg.IsUnderdeck)
                return !CheckProtectedUnit(dg, containers, ship.Row00Exists);

            return false;
        }


        /// <summary>
        /// Method to determine Stowage category in accordance with IMDG Code paragraph 7.1.3.
        /// true = conflict
        /// Method StowageCatCheck implemented only for cargo ships!!!
        /// </summary>
        private static bool StowageCatCheck(Dg dg)
        {
            bool result = false;
            if (!dg.IsUnderdeck && !char.IsDigit(dg.StowageCat)) return false;
            switch (dg.StowageCat)
            {
                case 'A':
                    result = false;
                    break;
                case 'B':
                    result = false;
                    break;
                case 'C':
                    result = true;
                    break;
                case 'D':
                    result = true;
                    break;
                case 'E':
                    result = false;
                    break;
                //ADD CHECK FOR CLOSED CARGO TRANSPORT UNITS
                case '1':
                    if (!dg.IsClosed && !dg.IsUnderdeck) result = true;
                    break;
                case '2':
                    if (!dg.IsClosed && !dg.IsUnderdeck) result = true;
                    break;
                case '3':
                    if (!dg.IsClosed && !dg.IsUnderdeck) result = true;
                    break;
                case '4':
                    if (!dg.IsClosed) result = true;
                    break;
                case '5':
                    if (!dg.IsClosed || dg.IsUnderdeck) result = true;
                    break;
                default: break;
            }
            return result;
        }

        /// <summary>
        /// Method to check compliant stowage with DOC
        /// (true = in compliance, false = conflict)
        /// </summary>
        /// <param name="dg"></param>
        /// <returns>True if in compliance with DOC</returns>
        private static bool CheckDoc(Dg dg)
        {
            var hold = dg.IsUnderdeck ? dg.HoldNr : 0;
            dg.AssignRowFromDOC();
            byte fromtable = ship.Doc.DOCtable[hold, dg.DgRowInDOC];
            return fromtable != 0;
        }

        /// <summary>
        /// Checks for any additional stowage requirements which may exist in chapter 7.4
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="containers"></param>
        private static void CheckAdditionalStowageRequirements(Dg unit, ICollection<Container> containers)
        {
            if (IMDGCode.Fishmeal.Contains(unit.Unno))
            {
                unit.AddConflict(STOW, "SSC3");
                unit.AddConflict(unit.IsUnderdeck, STOW, "SSC3a");
                unit.AddConflict(CheckNotProtectedFromSourceOfHeat(unit, containers), STOW, "SSC3b");
            }
            if (IMDGCode.AmmoniumNitrate.Contains(unit.Unno))
            {
                unit.AddConflict(STOW, "SSC4");
            }
            if (unit.IsAsCoolantOrConditioner)
            {
                ReplaceStowageConflicts(unit, "EXC9", "SSC2");
            }
            if (unit.Unno == 1415 || unit.Unno == 1418)
            {
                unit.AddConflict(STOW, "FF1");
            }
        }

        /// <summary>
        /// Removes any stowage conflict of the unit with [single] newCode with exception of exceptCode. 
        /// If no conflict has been removed, the newCode will not be set.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="newCode">New code to be set.</param>
        /// <param name="exceptCode">Code which will be ignorred and will remain in Conflicts of the unit.</param>
        private static void ReplaceStowageConflicts(Dg unit, string newCode, string exceptCode = "none")
        {
            if (!unit.IsConflicted || !unit.Conflicts.FailedStowage) return;

            bool removed = false;
            for (byte i = 0; i < unit.Conflicts.StowageConflictsList.Count; i++)
            {
                var conflict = unit.Conflicts.StowageConflictsList[i];
                if (conflict != exceptCode)
                {
                    unit.RemoveStowageConflict(conflict);
                    i--;
                    removed = true;
                }
            }
            if (removed)
                unit.AddConflict(STOW, newCode);
        }

        /// <summary>
        /// Method checks if a marine pollutant or infectious waste as specified by UNno is stowed on deck on a seaside and adds a conflict in that case.
        /// </summary>
        /// <param name="unit"></param>
        private static void CheckMarinePollutantAndInfectiousSubstancesStowage(Dg unit)
        {
            if (unit.IsUnderdeck || !unit.IsMp &&
                 unit.Unno != 2814 && unit.Unno != 2900 && unit.Unno != 3549) return;

            char[] categories = new char[] { 'A', 'B', 'E' };
            unit.AddConflict(categories.Contains(unit.StowageCat), STOW, "SSC5a");
            unit.AddConflict(ship.IsOnSeaSide(unit), STOW, "SSC5");
        }

        /// <summary>
        /// Method checks stowage of unit of class 7 in respect to accommodation and adds a stowage conflict if the cargo is within 6 Bays of an accommodation.
        /// </summary>
        /// <param name="unit"></param>
        private static void CheckRadioactiveStowage(Dg unit)
        {
            if (unit.DgClass != "7") return;
            List<byte> bays = ship.BaysSurroundingSuperstructure;
            foreach (byte accBay in ship.BaysInFrontOfSuperstructures)
            {
                for (int i = 3; i <= 12; i++)
                {
                    bays.Add((byte)(accBay + 1 + i));
                    bays.Add((byte)(accBay + 1 - i));
                }
            }
            unit.AddConflict(bays.Contains(unit.Bay), STOW, "SCC8");
        }

        /// <summary>
        /// Checks special stowage requirements are met if the unit is an explosive
        /// </summary>
        /// <param name="unit"></param>
        private static void CheckStowageOfExplosives(Dg unit)
        {
            if (string.IsNullOrEmpty(unit.DgClass)) return;

            //Class 1 general stowage
            //Not less than 12 m from Living quarters and LSA
            //Not less than 2,4 m from ship side
            if (unit.DgClass.StartsWith("1") && !unit.DgClass.StartsWith("1.4"))
            {
                unit.AddConflict(ship.IsNotClearOfLSA(unit), STOW, "SSC6");
                unit.AddConflict(ship.IsOnSeaSide(unit), STOW, "SSC7");
            }
        }

        /// <summary>
        /// Sets 'protected from' side value with correct language and spacing.
        /// </summary>
        /// <param name="side"></param>
        /// <param name="value"></param>
        private static void SetSideProtectedEntry(ref string side, string value)
        {
            if (string.Equals(side, "Nil"))
                side = value;
            else
                side += " and " + value;
        }

    }
}
