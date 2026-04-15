using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public class Conflicts
    {
        // ---------- private fields --------------------------------


        #region Public fields
        // ---------- public fields ---------------------------------
        public bool FailedStowage => StowageConflictsList?.Count > 0;
        public bool FailedSegregation => SegregationConflictsList?.Count > 0;
        public bool IsEmpty => !FailedStowage && !FailedSegregation;


        public readonly List<string> StowageConflictsList;
        public readonly List<SegregationConflict> SegregationConflictsList;
        #endregion

        #region Constructors

        // ---------- Constructor ----------------------------------
        public Conflicts()
        {
            StowageConflictsList = new List<string>();
            SegregationConflictsList = new List<SegregationConflict>();
        }

        #endregion

        #region Contains method logic

        // --------- Contains methods -------------------------------
        public bool Contains(Dg b)
        {
            if (SegregationConflictsList.Count == 0) return false;
            foreach (SegregationConflict conf in SegregationConflictsList)
                if (conf.ConflictContainerNr == b.ContainerNumber)
                    return true;
            return false;
        }
        public bool Contains(ushort unno)
        {
            if (SegregationConflictsList.Count == 0) return false;
            foreach (SegregationConflict conf in SegregationConflictsList)
                if (conf.ConflictContainerUnno == unno)
                    return true;
            return false;
        }
        public bool Contains(ushort[] unnos)
        {
            if (SegregationConflictsList.Count == 0) return false;
            foreach (SegregationConflict unused in SegregationConflictsList)
                if (unnos.Any(Contains))
                    return true;
            return false;
        }
        public bool Contains(string code, Dg b)
        {
            if (SegregationConflictsList.Count == 0) return false;
            foreach (SegregationConflict conf in SegregationConflictsList)
                if (conf.ConflictContainerNr == b.ContainerNumber && conf.Code == code)
                    return true;
            return false;
        }
        #endregion


        #region Display conflicts

        // --------- Methods to display conflicts ------------------

        public override string ToString()
        {
            string temp = $"";
            if (SegregationConflictsList.Count > 1)
            {
                temp = "conflicts";
            }
            else if (SegregationConflictsList.Count > 0)
            {
                temp = "conflict";
            }

            return (FailedStowage ? $"stowage conflict " : "") +
                   (FailedSegregation ? ($"segregation " + temp) : "");
        }

        #endregion


        #region SegregationConflict class
        // --------- Supporting class Segregstion Conflict ---------
        public class SegregationConflict
        {
            public string Code;
            internal readonly string ConflictContainerNr;
            internal readonly string ConflictContainerLocation;
            internal readonly string ConflictContainerClassStr;
            internal readonly int ConflictContainerUnno;
            public Dg DgInConflict;

            internal SegregationConflict(string code, Dg unit)
            {
                string subclass = "";
                Code = code;
                DgInConflict = unit;
                ConflictContainerNr = unit.ContainerNumber;
                ConflictContainerLocation = unit.Location;
                ConflictContainerUnno = unit.Unno;
                if (unit.DgClass != "Reefer")
                    subclass = (unit.DgSubclassCount > 0 ? ", " + unit.DgSubClass[0] : "") +
                                (unit.DgSubclassCount > 1 ? ", " + unit.DgSubClass[1] : "");
                ConflictContainerClassStr = unit.DgClass + subclass;
            }
        }

        #endregion
    }
}
