using System.Collections.Generic;

// ReSharper disable RedundantAssignment

namespace EasyJob_ProDG.Model.Cargo
{
    public partial class Stowage
    {
        /// <summary>
        /// Checking of stowage in accordance with special stowage requirements (SW codes)
        /// </summary>
        /// <param name="sscode"></param>
        /// <param name="dg"></param>
        /// <param name="containers"></param>
        /// <returns></returns>
        public static bool SpecialStowageCheck
            (string sscode, Dg dg, ICollection<Container> containers)
        {
            //Method to define if stowage in accordance with special stowage provisions ("SW" or "H") is violated
            //Returns 'true' if in conflict
            bool result = false;
            switch (sscode)
            {
                case "SW1":
                    //Protected from sources of heat means that packages and cargo transport units shall be stowed at least 2.4 m from heated ship structures, where the surface temperature is liable to exceed 55°C.Examples of heated structures are steam pipes, heating coils, top or side walls of heated fuel and cargo tanks, and bulkheads of machinery spaces. In addition, packages not loaded inside a cargo transport unit and stowed on deck shall be shaded from direct sunlight. The surface of a cargo transport unit can heat rapidly when in direct sunlight in nearly windless conditions and the cargo may also become heated. Depending on the nature of the goods in the cargo transport unit and the planned voyage precautions shall be taken to ensure that exposure to direct sunlight is reduced.
                    result = CheckNotProtectedFromSourceOfHeat(dg, containers);
                    break;
                case "SW2":
                    //
                    //Clear of living quarters means that packages or cargo transport units 
                    //shall be stowed a minimum distance of 3 m from accommodation, air intakes, 
                    //machinery spaces and other enclosed work areas.
                    //
                    result = ship.IsInLivingQuarters(dg);
                    break;
                case "SW3":
                    if (!dg.IsRf) result = true;
                    break;
                case "SW4":
                    result = true;
                    break;
                case "SW5":
                    if(dg.IsUnderdeck) SWgroups.AddSW5 = dg;
                    break;
                case "SW6":
                    result = (dg.IsUnderdeck && ship.Doc.DOCtable[dg.HoldNr,6] == 0);
                    if (dg.IsUnderdeck) SWgroups.AddSW6 = dg;
                    break;
                case "SW7":
                    result = true;
                    break;
                case "SW8":
                    result = true;
                    break;
                case "SW9":
                    result = true;
                    break;
                case "SW10":
                    result = true;
                    break;
                case "SW11":
                    if (!dg.IsUnderdeck) result = !CheckProtectedUnit(dg, containers, ship.Row00Exists);
                    break;
                case "SW12":
                    result = true;
                    break;
                case "SW13":
                    result = true;
                    break;
                case "SW14":
                    result = dg.IsUnderdeck;
                    break;
                case "SW15":
                    result = dg.IsUnderdeck;
                    break;
                case "SW16":
                    if (dg.IsClosed == false) { dg.StowageCat = 'B'; result = true; }
                    break;
                case "SW17":
                    result = dg.IsUnderdeck;
                    if (dg.IsClosed) { dg.StowageCat = 'E'; result = true; }
                    break;
                case "SW18":
                    result = dg.IsUnderdeck;
                    break;
                case "SW19":
                    if (dg.IsUnderdeck)
                        SWgroups.AddSW19 = dg;
                    break;
                case "SW20":
                    result = dg.IsUnderdeck;
                    break;
                case "SW21":
                    result = dg.IsUnderdeck;
                    break;
                
                //For AEROSOLS with a maximum capacity of 1 L: category A.
                //For AEROSOLS with a capacity above 1 L: category B.
                //For WASTE AEROSOLS or WASTE GAS CARTRIDGES: category C, clear of living quarters.
                case "SW22":
                    if (dg.StowageCat == '-' || dg.StowageCat == ' ' || dg.StowageCat == '0') dg.StowageCat = 'B';
                    if (!dg.IsWaste && (dg.IsUnderdeck || ship.IsInLivingQuarters(dg)))
                        SWgroups.AddSW22 = dg;
                    else if (dg.IsWaste && (dg.IsUnderdeck || ship.IsInLivingQuarters(dg)))
                    {
                        dg.AddConflict(true, STOW, "SSC22");
                        SWgroups.ListSW22List.Remove(dg);
                    }
                    break;
                case "SW23":
                    //4.3.4.1 Flexible bulk containers are only allowed in the holds of general cargo ships. They are not allowed to be transported in cargo transport units.
                    result = false;
                    break;
                case "SW24":
                    result = true;
                    break;
                case "SW25":
                    result = true;
                    break;
                case "SW26":
                    result = true;
                    break;


                case "SW27":
                    result = false;
                    break;
                case "SW28":
                    result = true;
                    break;
                case "SW29":
                    if (dg.FlashPointAsDecimal >= 23) dg.StowageCat = 'A';
                    result = true;
                    break;
                case "SW30":
                    result = true;
                    break;
                case "H1":
                    result = true;
                    break;
                case "H2":
                    result = true;
                    break;
                case "H3":
                    result = true;
                    break;
                case "H4":
                    result = true;
                    break;
                case "H5":
                    result = true;
                    break;
                default: break;
            }
            return result;
        }
    }
}
