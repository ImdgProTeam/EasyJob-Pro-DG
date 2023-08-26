using System.Collections.Generic;

namespace EasyJob_ProDG.Model.Cargo
{
    internal static class SegregationExtensionMethods
    {
        /// <summary>
        /// Checks special segregation of a dg according to SG.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="dglist"></param>
        /// <param name="ship"></param>
        public static void SpecialSegregationCheck(this Dg dg, IEnumerable<Dg> dglist, Transport.ShipProfile ship)
        {
            Segregation.SpecialSegregationCheck(dg, dglist, ship);
        }
    }
}
