using System.Collections.Generic;
using System.Linq;
using EasyJob_ProDG.Data.Info_data;

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

        #region Add/Replace conflict logic

        // --- Methods to add/remove/replace conflicts in the list ---
        public void AddStowConflict(string code)
        {
            if (!StowageConflictsList.Contains(code)) StowageConflictsList.Add(code);
        }
        public void AddSegrConflict(string code, Dg unit)
        {
            if (!Contains(code, unit)) SegregationConflictsList.Add(new SegregationConflict(code, unit));
        }

        /// <summary>
        /// Clears all stowage conflicts
        /// </summary>
        /// <param name="dg"></param>
        public void ClearStowageConflicts()
        {
            StowageConflictsList.Clear();
        }

        /// <summary>
        /// Emties mutual segregation conflicts from both dg SegregationConflictLists
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void RemoveSegregationConflict(Dg a, Dg b)
        {
            foreach (SegregationConflict conf in a.Conflicts.SegregationConflictsList)
            {
                if (conf.ConflictContainerNr != b.ContainerNumber) continue;
                //remove conflict from a
                a.Conflicts.SegregationConflictsList.Remove(conf);

                //remove conflict from b
                if (b.IsConflicted && b.Conflicts.Contains(a)) foreach (SegregationConflict bconf in b.Conflicts.SegregationConflictsList)
                        if (bconf.ConflictContainerNr == a.ContainerNumber)
                            b.Conflicts.SegregationConflictsList.Remove(bconf);
            }
        }

        /// <summary>
        /// Renews mutual segregation confilcts in both dg SegregationConflictLists with new code
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="newCode"></param>
        public static void ReplaceConflict(Dg a, Dg b, string newCode)
        {
            if (a.Conflicts == null) return;
            foreach (SegregationConflict conf in a.Conflicts.SegregationConflictsList)
                if (conf.ConflictContainerNr == b.ContainerNumber)
                {
                    conf.Code = newCode;
                    if (b.IsConflicted && b.Conflicts.Contains(a))
                        foreach (var bconf in b.Conflicts.SegregationConflictsList)
                            if (bconf.ConflictContainerNr == a.ContainerNumber) bconf.Code = newCode;
                }
        }

        /// <summary>
        /// Finds all segregation conflicts and replaces their codes with the newCode on both conflicted units.
        /// </summary>
        /// <param name="dg">Dg to search segregation conflicts in.</param>
        /// <param name="newCode">New code to be set for all conflicts.</param>
        internal static void ReplaceAllSegregationConflicts(Dg dg, string newCode)
        {
            if (dg.Conflicts == null) return;

            foreach (SegregationConflict conf in dg.Conflicts.SegregationConflictsList)
            {
                conf.Code = newCode;
                foreach (SegregationConflict mutualConflict in conf.DgInConflict.Conflicts.SegregationConflictsList)
                {
                    if (mutualConflict.ConflictContainerNr == dg.ContainerNumber)
                        mutualConflict.Code = newCode;
                }
            }
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
        public string ShowStowageConflicts()
        {
            string result = null;
            foreach (string s in StowageConflictsList)
            {
                if (s.StartsWith("SW19") || s.StartsWith("SW22")) continue;
                if (s.StartsWith("SW") || s.StartsWith("H"))
                {
                    result += s + " " + CodesDictionary.Stowage[s] + ". ";
                }
                else
                {
                    result += " " + CodesDictionary.ConflictCodes[s];
                }
            }
            return result;
        }
        public string ShowSegregationConflicts()
        {
            string result = null;

            foreach (SegregationConflict s in SegregationConflictsList)
            {
                string codeDiscr = s.Code.StartsWith("SGC")
                    ? CodesDictionary.ConflictCodes[s.Code]
                    : CodesDictionary.Segregation[s.Code];
                result += s.ConflictContainerLocation
                          + " (class " + s.ConflictContainerClassStr
                          + (s.ConflictContainerClassStr == "Reefer"
                              ? ""
                              : (" unno " + s.ConflictContainerUnno))
                          + ") " + s.Code + " - " + codeDiscr + "\n";
            }
            return result;
        }

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
