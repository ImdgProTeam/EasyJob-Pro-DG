using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyJob_Pro_DG
{
    public class Conflict
    {
        public bool failedStowage = false,
                    failedSegregation = false;

        public readonly List<string> _stowConflicts;

        List<string> surroundingContainers = new List<string>();

        public readonly List<SegregationConflict> _segrConflicts;

        //Constructior
        public Conflict()
        {
            _stowConflicts = new List<string>();
            _segrConflicts = new List<SegregationConflict>();
        }

        //Methods to add conflicts to the list
        public void AddStowConflict(string code)
        {
            if(!_stowConflicts.Contains(code)) _stowConflicts.Add(code);
            failedStowage = true;
        }
        public void AddSegrConflict(string _code, Dg unit)
        {
            if(!Contains(_code, unit)) _segrConflicts.Add(new SegregationConflict (_code, unit));
            failedSegregation = true;
        }

        public bool Contains(Dg b)
        {
            if (_segrConflicts.Count == 0) return false;
            foreach (SegregationConflict conf in _segrConflicts)
                if (conf.conflictContainerNr == b.cntrNr)
                    return true;
            return false;
        }

        public bool Contains(int unno)
        {
            if (_segrConflicts.Count == 0) return false;
            foreach (SegregationConflict conf in _segrConflicts)
                if (conf.conflictContainerUNNO == unno)
                    return true;
            return false;
        }

        public bool Contains(int[] unnos)
        {
            if (_segrConflicts.Count == 0) return false;
            foreach (SegregationConflict conf in _segrConflicts)
                foreach (int unno in unnos)
                {
                   if (this.Contains(unno))
                        return true;
                }
            return false;
        }

        public bool Contains(string code, Dg b)
        {
            if (_segrConflicts.Count == 0) return false;
            foreach (SegregationConflict conf in _segrConflicts)
                if (conf.conflictContainerNr == b.cntrNr && conf.code == code)
                    return true;
            return false;
        }

        public static void RemoveConflict(Dg a, Dg b)
        {
            foreach (SegregationConflict conf in a.conflict._segrConflicts)
            {
                if (conf.conflictContainerNr != b.cntrNr) continue;
                //remove conflict from a
                a.conflict._segrConflicts.Remove(conf);
                if (a.conflict._segrConflicts.Count == 0 && a.conflict._stowConflicts.Count == 0) a.conflicted = false;
                //remove conflict from b
                if (b.conflicted && b.conflict.Contains(a)) foreach (SegregationConflict bconf in b.conflict._segrConflicts)
                    if (bconf.conflictContainerNr == a.cntrNr)
                        b.conflict._segrConflicts.Remove(bconf);
                if (b.conflict._segrConflicts.Count == 0 && b.conflict._stowConflicts.Count == 0) a.conflicted = false;
            }
        }

        public static void ReplaceConflict(Dg a, Dg b, string newCode)
        {
            if (a.conflict == null) return;
            foreach (SegregationConflict conf in a.conflict._segrConflicts)
                if (conf.conflictContainerNr == b.cntrNr)
                {
                    conf.code = newCode;
                    if (b.conflicted && b.conflict.Contains(a))
                        foreach (var bconf in b.conflict._segrConflicts)
                            if (bconf.conflictContainerNr == a.cntrNr) bconf.code = newCode;
                }
        }

        //Methods to display conflicts
        public string ShowStowageConflicts()
        {
            
            string result = null;
            foreach(string s in _stowConflicts)
            {
                if (s.StartsWith("SW19") || s.StartsWith("SW22")) continue;
                if(s.StartsWith("SW") || s.StartsWith("H"))
                {
                    result += s + " " + CodesDictionary.stowage[s] + ". ";
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

            foreach (SegregationConflict s in _segrConflicts)
            {
                string  codeDiscr = s.code.StartsWith("SGC") ? CodesDictionary.ConflictCodes[s.code] : CodesDictionary.segregation[s.code];
                result += s.conflictContainerLocation +" (class " + s.conflictContainerClassStr + (s.conflictContainerClassStr == "Reefer"? "": (" unno " + s.conflictContainerUNNO)) + ") " + s.code + " - " + codeDiscr+"\n";
            }
            return result;
        }
        public class SegregationConflict
        {
            public string code;
            internal readonly string conflictContainerNr;
            internal readonly string conflictContainerLocation;
            internal readonly string conflictContainerClassStr;
            internal readonly int conflictContainerUNNO;
            public Dg dgInConflict;


            internal SegregationConflict(string _code, Dg unit)
            {
                code = _code;
                dgInConflict = unit;
                conflictContainerNr = unit.cntrNr;
                conflictContainerLocation = unit.cntrLocation;
                conflictContainerUNNO = unit.unno;
                conflictContainerClassStr = unit.dgclass +
                                         (unit.dgsubclass.Any() ? ", " + unit.dgsubclass[0] : "") +
                                         (unit.dgsubclass.Count > 1 ? ", " + unit.dgsubclass[1] : "");
            }
       
        }
    }
}
