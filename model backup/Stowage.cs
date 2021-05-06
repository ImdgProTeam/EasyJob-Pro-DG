
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace EasyJob_Pro_DG
{
    public partial class Stowage
    {
        private static string stow = "stowage";
        public static SpecialStowageGroups SWgroups = new SpecialStowageGroups(true);

        public ShipProfile ship;

        public Stowage()
        {
            ship = new ShipProfile();
        }

        public static void CheckStowage(DgList dgList, ShipProfile ship, ObservableCollection<Container> containers)
        {
            SWgroups.Clear();
            foreach (Dg dg in dgList)
                CheckUnitStowage(unit: dg, ship: ship, containers: containers);
            ProgramFiles.EnterLog(ProgramFiles.logStreamWriter, "Stowage checked");
        }

        public static void CheckUnitStowage(Dg unit, ShipProfile ship, ObservableCollection<Container> containers)
        {

            if (unit.LQ) return;
            //Check special stowage
            foreach (string sscode in unit.stowageSW)
                unit.AddConflict(SpecialStowageCheck(sscode, unit, containers, ship), stow, sscode);
            //Check stowage category
            unit.AddConflict(StowageCatCheck(unit), "stowage","SSC1");

            //Check against DOC
            unit.AddConflict(!checkDOC(unit, ship), "stowage", "SSC2");

            //Class 1 general stowage
            if (unit.dgclass.StartsWith("1") && !unit.dgclass.StartsWith("1.4"))
            {
                unit.AddConflict(ship.IsNotClearOfLSA(unit), "stowage", "SSC6");
                unit.AddConflict(ship.IsOnSeaSide(unit), "stowage", "SSC7");
            }

            //Check in respect of marine pollutants
            CheckMarinePollutantStowage(unit, ship);

            //Additional requirements
            CheckAdditionalStowageRequirements(unit);

            //Additional requirements for class 7
            CheckRadioactiveStowage(unit, ship);
        }


        

        /// <summary>
        /// Method to determine Stowage category in accordance with IMDG Code paragraph 7.1.3.
        /// true = conflict
        /// Method StowageCatCheck implemented only for cargo ships!!!
        /// </summary>
        public static bool StowageCatCheck(Dg dg)
        {
            bool result = false;
            if (!dg.underdeck && !char.IsDigit(dg.stowageCat)) return false;
            switch (dg.stowageCat)
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
                    if (!dg.closed && !dg.underdeck) result = true;
                    break;
                case '2':
                    if (!dg.closed && !dg.underdeck) result = true;
                    break;
                case '3':
                    if (!dg.closed && !dg.underdeck) result = true;
                    break;
                case '4':
                    if (!dg.closed) result = true;
                    break;
                case '5':
                    if(!dg.closed || dg.underdeck) result = true;
                    break;
                default: break;
            }
            return result;
        }

        /// <summary>
        /// Checking of protection from direct sun (surrounded by containers from 3 sides).
        /// Returns true if protected
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="containers"></param>
        /// <param name="row00exists"></param>
        /// <returns></returns>
        public static bool CheckProtectedUnit(Dg unit, ObservableCollection<Container> containers, bool row00exists)
        {
            bool result=false;
            int protectedFromSides = 0;
            bool protectedFromTop = false;

            //Checks only containers on deck
            //implementation for containers of the same size
            var Units = (from entry in containers
                where entry.bay <= (unit.bay + 1) && 
                      entry.bay >= (unit.bay - 1) && 
                      entry.underdeck == false
                select entry);

            foreach (var entry in Units)
            {
                //check if entry is the same container with reference unit
                if (entry.cntrLocation == unit.cntrLocation) continue;
                //check if protected from top - if already protected then will skip protection
                if (entry.row == unit.row && (entry.tier > unit.tier) && !protectedFromTop)
                {
                    protectedFromTop = true;
                    unit.surrounded += " top by "+entry.cntrLocation;
                    if (protectedFromSides == 4)
                    {
                        result = true;
                        break;
                    }
                    continue;
                }
                //check if in the same tier and entry is next to reference unit
                if (entry.tier == unit.tier && protectedFromSides < 4)
                {
                    if ((entry.row - 2 == unit.row) || (entry.row + 2 == unit.row) ||
                        (entry.row == 0 && unit.row == 1) || (entry.row == 1 && unit.row == 0) ||
                        (entry.row == 1 && unit.row == 2 && !row00exists) ||
                        (entry.row == 2 && unit.row == 1 && !row00exists))
                    {
                        if (entry.size == 40 || entry.size == unit.size)
                        {
                            protectedFromSides += 2;
                            unit.surrounded += " side by " + entry.cntrLocation;
                        }
                        else
                        {
                            protectedFromSides++;
                            unit.surrounded += " side by " + entry.cntrLocation;
                        }
                    }
                }
                //check for cummulutive result
                result = (protectedFromSides == 4 && protectedFromTop);
                if (result) break;
            }
            return result;
        }

        /// <summary>
        /// Method to check compliant stowage with DOC
        /// (true = in compliance)
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="_ship"></param>
        /// <returns></returns>
        public static bool checkDOC(Dg dg, ShipProfile _ship)
        {
            ShipProfile ship = _ship;
            var hold = dg.underdeck ? dg.holdNr : ship.numberOfHolds+1;
            byte fromtable = ship.doc.DOCtable[hold-1, dg.dgRowInDOC];
            return fromtable != 0;
        }

        public static void CheckAdditionalStowageRequirements(Dg unit)
        {
            List<int> fishmeal =new List<int>()
            {
                1374, 2216, 3497
            };
            List<int> ammonium = new List<int>()
            {
                1942, 2067, 2071
            };
            if (fishmeal.Contains(unit.unno))
            {
                unit.AddConflict();
                unit.conflict.AddStowConflict("SSC3");
            }
            if (ammonium.Contains(unit.unno))
            {
                unit.AddConflict();
                unit.conflict.AddStowConflict("SSC4");
            }

        }

        /// <summary>
        /// Method checks if a marine pollutant stowed on deck on a seaside and adds a conflict in that case.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="ship"></param>
        private static void CheckMarinePollutantStowage(Dg unit, ShipProfile ship)
        {
            if (!unit.mp || unit.underdeck) return;
            unit.AddConflict(ship.IsOnSeaSide(unit), "stowage", "SSC5");
        }

        /// <summary>
        /// Method checks stowage of unit of class 7 in respect to accommodation and adds a stowage conflict if the cargo is within 6 bays of an accommodation.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="ship"></param>
        private static void CheckRadioactiveStowage(Dg unit, ShipProfile ship)
        {
            if (unit.dgclass != "7") return;
            List<byte> bays = ship.Accommodation;
            foreach (byte accBay in ship.accommodationBays)
            {
                for (int i = 3; i <= 12; i++)
                {
                    bays.Add((byte)(accBay + 1 + i));
                    bays.Add((byte)(accBay + 1 - i));
                }
            }
            unit.AddConflict(bays.Contains(unit.bay), stow, "SCC8");
        }

    }
}
