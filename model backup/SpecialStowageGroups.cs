
using System.Collections.Generic;


namespace EasyJob_Pro_DG
{
    partial class Stowage
    {
        public struct SpecialStowageGroups
        {
            public readonly List<Dg> listSW19;
            public readonly List<Dg> listSW22;
            public readonly List<Dg> ventHoldUnits;
            public readonly List<byte> ventHolds;

            public SpecialStowageGroups(bool val)
            {
                listSW19 = new List<Dg>();
                listSW22 = new List<Dg>();
                ventHoldUnits = new List<Dg>();
                ventHolds = new List<byte>();
            }

            public void Add(Dg unit, string code)
            {
                string record = unit.cntrNr + " in " + unit.cntrLocation;
                switch (code)
                {
                    case "SW5":
                        {
                            ventHoldUnits.Add(unit);
                            if (!ventHolds.Contains(unit.holdNr))
                                ventHolds.Add(unit.holdNr);
                        }
                        break;
                    case "SW6":
                        {
                            ventHoldUnits.Add(unit);
                            if (!ventHolds.Contains(unit.holdNr))
                                ventHolds.Add(unit.holdNr);
                        }
                        break;
                    case "SW19":
                        listSW19.Add(unit);
                        break;
                    case "SW22":
                        listSW22.Add(unit);
                        break;
                }
            }

            public string ListSW19
            {
                get
                {
                    string result = "";
                    foreach (var record in listSW19)
                    {
                        result += (result == "" ? "" : ", ") + record.cntrNr + " in " + record.Location;
                    }

                    return result;
                }
            }
            public string ListSW22
            {
                get
                {
                    string result = "";
                    foreach (var record in listSW22)
                    {
                        result += (result == "" ? "" : ", ") + record.cntrNr + " in " + record.Location;
                    }
                    return result;
                }
            }

            public string VentHold
            {
                get
                {
                    string result = "";
                    foreach (var hold in ventHolds)
                    {
                        result += (result == "" ? "" : ", ") + "cargo hold " + hold;
                    }
                    return result;
                }
            }

            public Dg AddSW19
            {
                set
                {
                    Add(value, "SW19");
                }
            }

            public Dg AddSW22
            {
                set
                {
                    Add(value, "SW22");
                }
            }

            public Dg AddSW5
            {
                set
                {
                    Add(value, "SW5");
                }
            }
            public Dg AddSW6
            {
                set
                {
                    Add(value, "SW6");
                }
            }

            public void Clear()
            {
                listSW19.Clear();
                listSW22.Clear();
                ventHoldUnits.Clear();
                ventHolds.Clear();
            }
            public int CountVentHolds => ventHolds.Count;
            public int CountVentHoldUnits => ventHoldUnits.Count;
            public int CountSW19 => listSW19.Count;
            public int CountSW22 => listSW22.Count;
        }
    }
}
