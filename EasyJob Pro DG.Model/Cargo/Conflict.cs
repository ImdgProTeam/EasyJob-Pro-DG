using System.Collections.Generic;
using System.Linq;
using EasyJob_ProDG.Data.Info_data;

namespace EasyJob_ProDG.Model.Cargo
{
    public class Conflict
    {
        // ---------- private fields --------------------------------


        // ---------- public fields ---------------------------------
        public bool FailedStowage,
                    FailedSegregation;
        public readonly List<string> StowConflicts;
        public readonly List<SegregationConflict> SegrConflicts;

        // ---------- Constructor ----------------------------------
        public Conflict()
        {
            StowConflicts = new List<string>();
            SegrConflicts = new List<SegregationConflict>();
            // ReSharper disable once UnusedVariable
            List<string> surroundingContainers = new List<string>();
        }

        // --- Methods to add/remove/replace conflicts in the list ---
        public void AddStowConflict(string code)
        {
            if(!StowConflicts.Contains(code)) StowConflicts.Add(code);
            FailedStowage = true;
        }
        public void AddSegrConflict(string code, Dg unit)
        {
            if(!Contains(code, unit)) SegrConflicts.Add(new SegregationConflict (code, unit));
            FailedSegregation = true;
        }
        public static void RemoveConflict(Dg a, Dg b)
        {
            foreach (SegregationConflict conf in a.Conflict.SegrConflicts)
            {
                if (conf.ConflictContainerNr != b.ContainerNumber) continue;
                //remove conflict from a
                a.Conflict.SegrConflicts.Remove(conf);
                if (a.Conflict.SegrConflicts.Count == 0 && a.Conflict.StowConflicts.Count == 0) a.IsConflicted = false;
                //remove conflict from b
                if (b.IsConflicted && b.Conflict.Contains(a)) foreach (SegregationConflict bconf in b.Conflict.SegrConflicts)
                    if (bconf.ConflictContainerNr == a.ContainerNumber)
                        b.Conflict.SegrConflicts.Remove(bconf);
                if (b.Conflict.SegrConflicts.Count == 0 && b.Conflict.StowConflicts.Count == 0) a.IsConflicted = false;
            }
        }
        public static void ReplaceConflict(Dg a, Dg b, string newCode)
        {
            if (a.Conflict == null) return;
            foreach (SegregationConflict conf in a.Conflict.SegrConflicts)
                if (conf.ConflictContainerNr == b.ContainerNumber)
                {
                    conf.Code = newCode;
                    if (b.IsConflicted && b.Conflict.Contains(a))
                        foreach (var bconf in b.Conflict.SegrConflicts)
                            if (bconf.ConflictContainerNr == a.ContainerNumber) bconf.Code = newCode;
                }
        }

        // --------- Contains methods -------------------------------
        public bool Contains(Dg b)
        {
            if (SegrConflicts.Count == 0) return false;
            foreach (SegregationConflict conf in SegrConflicts)
                if (conf.ConflictContainerNr == b.ContainerNumber)
                    return true;
            return false;
        }
        public bool Contains(int unno)
        {
            if (SegrConflicts.Count == 0) return false;
            foreach (SegregationConflict conf in SegrConflicts)
                if (conf.ConflictContainerUnno == unno)
                    return true;
            return false;
        }
        public bool Contains(int[] unnos)
        {
            if (SegrConflicts.Count == 0) return false;
            foreach (SegregationConflict unused in SegrConflicts)
                //foreach (int unno in unnos)
                //{
                //    if (this.Contains(unno))
                //        return true;
                //}
                //
                // !!!Code was replaced with linq expression below. Must be checked !!!
                if (unnos.Any(Contains))
                    return true;
            return false;
        }
        public bool Contains(string code, Dg b)
        {
            if (SegrConflicts.Count == 0) return false;
            foreach (SegregationConflict conf in SegrConflicts)
                if (conf.ConflictContainerNr == b.ContainerNumber && conf.Code == code)
                    return true;
            return false;
        }


        // --------- Methods to display conflicts ------------------
        public string ShowStowageConflicts()
        {
            string result = null;
            foreach(string s in StowConflicts)
            {
                if (s.StartsWith("SW19") || s.StartsWith("SW22")) continue;
                if(s.StartsWith("SW") || s.StartsWith("H"))
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

            foreach (SegregationConflict s in SegrConflicts)
            {
                string  codeDiscr = s.Code.StartsWith("SGC") 
                    ? CodesDictionary.ConflictCodes[s.Code] 
                    : CodesDictionary.Segregation[s.Code];
                result += s.ConflictContainerLocation 
                          +" (class " + s.ConflictContainerClassStr 
                          + (s.ConflictContainerClassStr == "Reefer"
                              ? ""
                              : (" unno " + s.ConflictContainerUnno)) 
                          + ") " + s.Code + " - " + codeDiscr+"\n";
            }
            return result;
        }

        public override string ToString()
        {
            string temp = $"";
            if (SegrConflicts.Count > 1)
            {
                temp = "conflicts";
            }
            else if (SegrConflicts.Count > 0)
            {
                temp = "conflict";
            }

            return (FailedStowage ? $"stowage conflict " : "") +
                   (FailedSegregation ? ($"segregation " + temp) : "");
        }

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
                string subclass="";
                Code = code;
                DgInConflict = unit;
                ConflictContainerNr = unit.ContainerNumber;
                ConflictContainerLocation = unit.Location;
                ConflictContainerUnno = unit.Unno;
                if (unit.DgClass != "Reefer")
                    subclass = (unit.DgSubclass.Any() ? ", " + unit.DgSubclass[0] : "") +
                                (unit.DgsubclassCount > 1 ? ", " + unit.DgSubclass[1] : "");
                ConflictContainerClassStr = unit.DgClass + subclass;
            }
        }
    }
}
