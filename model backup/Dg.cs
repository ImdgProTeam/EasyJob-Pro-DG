using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Windows;
using System.Xml.Linq;
using EasyJob_Pro_DG.ViewModel;


namespace EasyJob_Pro_DG
{
    public class Dg : Container, INotifyPropertyChanged
    {
        
        #region Fields Declarations
        //Read from edi.
        //DGS+IMD+3(Class):+1234(UN)+-23:CEL(FP)+2(PG)+F-AS-E(EMS)+++:+::'
        //FTX+AAD+++NIL(description):125(NW):'
        public string dgclass;
        public List<string> dgsubclass;
        public List<string> allDgClasses;
        public int unno;
        public double dgfp;
        public byte dgpg;
        public string dgems;
        public float dgnetwt;
        public byte dgRowInTable;
        public byte[] Stack;


        //assign from DG List
        public bool mp;
        public bool mpDetermined;
        public string name;
        public List<int> special;
        public char stowageCat;
        public List<string> stowageSW;
        public List<string> segregationSG;
        public string properties;
        public string dgClassFromList;
        public bool liquid;
        public bool flammable;
        public bool emitFlamVapours;
        public bool differentClass;

        //from other sources
        public List<int> segregationGroup;
        public bool LQ;
        public bool conflicted;
        public Conflict conflict;
        public string surrounded;
        public char compatibilityGroup = '0';
        public string segregatorClass;
        public byte dgRowInDOC;
        public SegregatorException segregatorException;
        public bool IsInList;
        public bool IsNameChanged;

        public event PropertyChangedEventHandler PropertyChanged;
#endregion

        //--------------------- Public properties -----------------------------------------------------------------------------

        #region Public properties

        //----------------- Properties for public access -----------------------------


        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
            //if (DgList.changeCompleted && this.completedDg) MainViewModel.dgListObs.CollectionUpdate();
        }


        public int UNNO
        {
            get { return unno; }
            set
            {
                unno = value;
                if (IsInList)
                {
                    ProgramFiles.UpdateDGInfo(this, MainViewModel.dgDataBase, true);
                    OnPropertyChanged(new PropertyChangedEventArgs(""));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
                OnPropertyChanged(new PropertyChangedEventArgs("UNNO"));
            }
        }

        /// <summary>
        /// Set: adds string dg class to dgclass and will update allDgClasses.
        /// Get: returns string with dgclass.
        /// </summary>
        public string Dgclass
        {
            get { return dgclass; }
            set
            {
                dgclass = value;
                AllDgClasses=value;
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("Dgclass"));
                    OnPropertyChanged(new PropertyChangedEventArgs("AllDgClasses"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }

        /// <summary>
        /// Set: will add string dg class to dgsubclass and will update allDgClasses.
        /// Get: will return string with all subclasses listed and separated with comma.
        /// </summary>
        public string Dgsubclass
        {
            get
            {
                string temp = null;
                if (dgsubclass.Count <= 0) return null;
                foreach (var x in dgsubclass) temp += temp == null ? x.ToString(CultureInfo.InvariantCulture) : ", " + x.ToString(CultureInfo.InvariantCulture);
                return temp;
            }

            set
            {
                if (dgsubclass.Contains(value)) return;
                dgsubclass.Add(value);
                AllDgClasses = value;
                if(IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("Dgsubclass"));
                    OnPropertyChanged(new PropertyChangedEventArgs("AllDgClasses"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        /// <summary>
        /// Set: will parse string with packing group and record it to dgpg.
        /// Get: will return string containing better view of a packing group.
        /// </summary>

        public string PKG
        {
            get
            {
                string temp = null;
                for (int i = 0; i < dgpg; i++)
                    temp += "I";
                return temp;
            }

            set
            {
                try
                {
                    dgpg = Convert.ToByte(value);
                }
                catch (Exception)
                {
                    switch (value.ToUpper())
                    {
                        case "I":
                            dgpg = 1;
                            break;
                        case "II":
                            dgpg = 2;
                            break;
                        case "III":
                            dgpg = 3;
                            break;
                        default:
                            dgpg = 0;
                            break;
                    }
                }

                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("PKG"));
                    AssignFromDGList(MainViewModel.dgDataBase, false, true);
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        public string DgFP
        {
            get { return Math.Abs(dgfp - 9999) < 1? "": dgfp.ToString(CultureInfo.InvariantCulture); }
            set
            {
                dgfp = double.Parse(value); 
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("DgFP"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        public float DgNetWt
        {
            get { return dgnetwt; }
            set
            {
                dgnetwt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DgNetWt"));
            }
        }
        public string CntrNr
        {
            get { return cntrNr; }
            set
            {
                cntrNr = value;
                
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("CntrNr"));
                    MainViewModel.conflicts.CollectionUpdate();
                }
            }
        }
        public string CntrLocation
        {
            get { return cntrLocation; }
            set
            {
                cntrLocation = value;

                if (this.IsInList)
                {
                    DefineContainerLocation();
                    if (MainViewModel.ownship != null) holdNr = MainViewModel.ownship.DefineCargoHoldNumber(bay);
                    //add cargo hold
                    OnPropertyChanged(new PropertyChangedEventArgs(""));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        public string CntrType
        {
            get { return cntrType; }
            set
            {
                cntrType = value;
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("CntrType"));
                    OnPropertyChanged(new PropertyChangedEventArgs("IsClosed"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        public bool IsClosed
        {
            get { return closed; }
            set
            {
                closed = value;
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("IsClosed"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        public bool IsLQ
        {
            get { return LQ; }
            set
            {
                LQ = value;
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("IsLQ"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        public bool IsRF
        {
            get { return RF; }
            set
            {
                RF = value;
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("IsRF"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        public string CnPOL
        {
            get { return cnPOL; }
            set
            {
                cnPOL = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CnPOL"));
            }
        }
        public string CnPOD
        {
            get { return cnPOD; }
            set
            {
                cnPOD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CnPOD"));
            }
        }
        public bool MP
        {
            get { return mp; }
            set
            {
                mp = value;
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("MP"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                IsNameChanged = true;
            }
        }   //Proper shipping name
        public bool Liquid
        {
            get { return liquid; }
            set
            {
                liquid = value;
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("Liquid"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        public bool Flammable
        {
            get { return flammable; }
            set
            {
                flammable = value;
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("Flammable"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }
        public bool EmitFlammableVapours
        {
            get { return emitFlamVapours; }
            set
            {
                emitFlamVapours = value;
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("EmitFlammableVapours"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }

        /// <summary>
        /// Set: will check if segregation group exists in the list and will assign it to dg unit.
        /// Get: returns string containing all segregation groups separated with comma.
        /// </summary>
        public string SegregationGroup
        {
            get
            {
                string result=null;
                foreach (int itemNr in segregationGroup)
                {
                    string group = Segregation.segregationGroups[itemNr];
                    result += result == null ? group : ", " + group;
                }
                return result;
            }
            set
            {
                if(Segregation.segregationGroups.Contains(value) &&
                   !segregationGroup.Contains(Array.IndexOf(Segregation.segregationGroups, value)))
                    segregationGroup.Add(Array.IndexOf(Segregation.segregationGroups,value));
                if (IsInList)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("SegregationGroup"));
                    MainViewModel.dgListObs.CollectionUpdate();
                }
            }
        }


        //--------------- Readonly properties -----------------------------------------------
        public bool Underdeck
        {
            get { return underdeck; }
        }
        public byte HoldNr
        {
            get { return holdNr; }
            //set
            //{
            //    holdNr = value;
            //    OnPropertyChanged(new PropertyChangedEventArgs("HoldNr"));
            //}
        }
        public byte Bay => bay;
        public byte Row => row;
        public byte Tier => tier;
        public byte Size => size;
        public string DgEMS
        {
            get { return dgems; }
            //set
            //{
            //    dgems = value;
            //    OnPropertyChanged(new PropertyChangedEventArgs("DgEMS"));
            //}
        }
        public string Special
        {
            get
            {
                string result=null;
                foreach (int item in special)
                {
                    result += (result == null ? "" : ", ") + item;
                }
                return result;
            }
        }
        public char StowageCat
        {
            get { return stowageCat; }
            //set
            //{
            //    stowageCat = value;
            //    if (IsInList)
            //    {
            //        OnPropertyChanged(new PropertyChangedEventArgs("StowageCat"));
            //        MainViewModel.dgListObs.CollectionUpdate();
            //    }
            //}
        }
        public string StowageSW
        {
            get
            {
                string result = null;
                foreach (string item in stowageSW)
                {
                    result += (result == null ? "" : ", ") + item;
                }
                return result;
            }
        }
        public string SegregationSG
        {
            get
            {
                string result = null;
                foreach (string item in segregationSG)
                {
                    result += (result == null ? "" : ", ") + item;
                }
                return result;
            }
        }
        public string Properties
        {
            get { return properties; }
        }


        #endregion

        #region Other properties
        // ------------------- Other properties --------------------------------------------        

        /// <summary>
        /// Set: reads array of string and records them to dgsubclass as well as to allDgclasses.
        /// </summary>
        public string[] DgsubclassArray
        {
            set
            {
                foreach (string x in value) if (x != "") Dgsubclass = x;
            }
            get {return dgsubclass.ToArray(); }
        }

        /// <summary>
        /// Set: will add string dg class to allDgClasses.
        /// Get: will return string with allDgClasses listed and separated with a comma.
        /// </summary>
        public string AllDgClasses
        {
            get
            {
                string _temp="";
                foreach (string x in allDgClasses)
                    _temp += _temp == "" ? x : ", " + x;
                return _temp;
            }
            set
            {
                if(!allDgClasses.Contains(value)) allDgClasses.Add(value);
            }
        }

        internal byte[] ALocation => new[] { bay, row, tier };
        #endregion

        
        #region Constructors
        //------------------------ Constructors --------------------------------------------------------------------------------------

        /// <summary>
        /// Public constructor of a blanc Dg
        /// </summary>
        public Dg()
        {
            dgclass = null;
            dgsubclass = new List<string>();
            allDgClasses=new List<string>();
            unno = 0;
            dgfp = 9999; //TO ADD CONVERTION TO/FROM FARENHEIGHT
            dgpg = 0;
            dgems = null;
            dgnetwt = 0;
            mp = false;
            mpDetermined = false;
            special = new List<int>();
            stowageSW = new List<string>();
            segregationSG = new List<string>();
            segregationGroup = new List<int>();
            LQ = false;
            flammable = false;
            liquid = false;
            emitFlamVapours = false;
            IsInList = false;
        }

        /// <summary>
        /// Constructor to create Dg unit having only class and row for means of segregation between two classes
        /// </summary>
        /// <param name="_class"></param>
        public Dg(string _class)
        {
            dgclass = _class;
            AssignRowNumber();
            IsInList = false;
        }
#endregion



        //---------------------- Assisting methods ----------------------------------------------------------------------

        /// <summary>
        /// Method assigns a stack to dg unit, taking into account 20 and 40'
        /// </summary>
        public void AssignStack()
        {
            Stack = new byte[3];
            Stack[0] = (byte)(bay - 1);
            Stack[1] = bay;
            Stack[2] = (byte)(bay + 1);
            
        }


        /// <summary>
        /// Method to assign row number in segregation table.
        /// </summary>
        public void AssignRowNumber()
        {
            byte tableRow;

            var _index = dgclass.Length > 3 ? dgclass.Substring(0, 3) : dgclass;
            switch (_index)
            {
                case "1.1":
                    tableRow = 0;
                    break;
                case "1.2":
                    tableRow = 0;
                    break;
                case "1.3":
                    tableRow = 1;
                    break;
                case "1.4":
                    tableRow = 2;
                    break;
                case "1.5":
                    tableRow = 0;
                    break;
                case "1.6":
                    tableRow = 1;
                    break;
                case "2.1":
                    tableRow = 3;
                    break;
                case "2.2":
                    tableRow = 4;
                    break;
                case "2.3":
                    tableRow = 5;
                    break;
                case "3":
                    tableRow = 6;
                    break;
                case "4.1":
                    tableRow = 7;
                    break;
                case "4.2":
                    tableRow = 8;
                    break;
                case "4.3":
                    tableRow = 9;
                    break;
                case "5.1":
                    tableRow = 10;
                    break;
                case "5.2":
                    tableRow = 11;
                    break;
                case "6.1":
                    tableRow = 12;
                    break;
                case "6.2":
                    tableRow = 13;
                    break;
                case "7":
                    tableRow = 14;
                    break;
                case "8":
                    tableRow = 15;
                    break;
                case "9":
                    tableRow = 16;
                    break;
                default:
                    tableRow = 0;
                    break;
            }
            dgRowInTable = tableRow;
        }


        /// <summary>
        /// Method to update Dg unit (derived from a file) with info from dglist.xml
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="unitIsNew"></param>
        public void AssignFromDGList(XDocument xmlDoc, bool unitIsNew = false, bool pkgChanged = false)
        {
            
            var chosenEntries = (from entry in xmlDoc.Descendants("DG")
                                 where (int)entry.Attribute("unno") == unno
                                 select entry);

            List<Dg> list = new List<Dg>();
            
            #region GetFromXML
            ///Assigning data to temporary item (record) from chosenEntries and complete temporary list
            foreach (var entry in chosenEntries)
            {
                Dg record = new Dg
                {
                    dgclass = entry.Attribute("class").Value,
                    dgpg = byte.Parse(entry.Attribute("pg")?.Value)
                };


                string[] array = entry.Attribute("subrisk")?.Value.Split('/');
                foreach (string x in array) if (x != "–") record.Dgsubclass = x;

                var temp = entry.Attribute("MP").Value;
                if (temp == "true")record.mp = true;

                record.name = entry.Element("name").Value;

                array = entry.Attribute("specialprovisions").Value.Split(' ');
                foreach (string x in array)record.special.Add(x != "–" ? Convert.ToInt16(x) : 0);

                record.stowageCat = (entry.Element("Stowage").Attribute("category").Value).Length > 1 ? 
                                    (entry.Element("Stowage").Attribute("category").Value)[1] : 
                                    (entry.Element("Stowage").Attribute("category").Value)[0];

                array = entry.Element("Stowage").Attribute("provision").Value.Split(' ');
                foreach (string x in array) record.stowageSW.Add(x);

                array = entry.Element("Segregation").Value.Split(' ');
                foreach (string x in array) record.segregationSG.Add(x);

                record.properties = entry.Element("Propertiesandobservations").Value;
                record.dgems = entry.Element("EMS").Value;

                list.Add(record);
            }
            #endregion

            #region MultipleEntries
            //Solving case with multiple entries found
            int orderInList = 0;
            if (list.Count > 1)
            {
                if (dgpg != 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].dgpg == dgpg) orderInList = i;
                    }
                }
                else
                {
                    //Find highest risk from choise (lowest packing group)
                    var _pkg = 3;
                    for (int i = 0; i < list.Count; i++)
                    {
                        _pkg = list[i].dgpg == 1 ? list[i].dgpg : (_pkg < list[i].dgpg ? _pkg : list[i].dgpg);
                        if (list[i].dgpg == _pkg)orderInList = i;
                    }
                }
            }
#endregion

            //Transfer data from record to dg item
            Dg tr = list[orderInList];

            if (pkgChanged)
            {
                stowageCat = tr.stowageCat;
                stowageSW = tr.stowageSW;
                segregationSG = tr.segregationSG;
                special = tr.special;
                properties = tr.properties;
                return;
            }
            tr.SpecialClass();
            dgClassFromList = tr.dgclass;

            if (unitIsNew)
            {
                Dgclass = tr.dgclass;
            }
            else
            {
                if (tr.dgclass != dgclass)
                {
                    //AEROSOLS
                    if (tr.unno == 1950)
                    {
                        if (dgclass == "2") dgclass = tr.dgclass;
                        else
                            Output.DisplayLine(
                                "Caution! For correct assigning of dg class and subrisk of AEROSOLS (UNNO 1950) in unit {0} refer to DG manifest and special provision 63 of IMDG code Ch 3.", cntrNr);
                    }
                    //Unno 2037
                    else if (tr.unno == 2037)
                        if (dgclass == "2")
                            Dgclass = tr.dgclass;
                        else
                            differentClass = false;
                    //All other cases
                    else differentClass = true;
                }
            }
            if (dgsubclass.Count == 0) DgsubclassArray = tr.dgsubclass.ToArray();
            
            dgpg = dgpg != 0 ? dgpg : tr.dgpg;
            dgems = dgems ?? tr.dgems;
            mp = mpDetermined ? mp : tr.mp;
            special = tr.special;
            stowageSW = tr.stowageSW;
            segregationSG = tr.segregationSG;
            stowageCat = tr.stowageCat;
            name = name ?? tr.name;
        }

        /// <summary>
        /// Method clears all the data of selected dg.
        /// If unno is given, then the unit parameter will be updated accordingly. By default unno will become '0'.
        /// </summary>
        /// <param name="_unno"></param>
        public void Clear(int _unno=0)
        {
            unno = _unno;
            dgsubclass.Clear();
            allDgClasses.Clear();
            dgpg = 0;
            dgems = null;
            mpDetermined = false;
            mp = false;
            special.Clear();
            stowageSW.Clear();
            segregationSG.Clear();
            segregationGroup.Clear();
            stowageCat = 'A';
            name = null;
            flammable = false;
            liquid = false;
            emitFlamVapours = false;
            dgClassFromList = null;
            dgRowInDOC = 0;
            dgRowInTable = 0;
            
        }


        /// <summary>
        /// Method to assign row number for dg in DOC table
        /// </summary>
        public void AssignRowFromDOC()
        {
            if(dgclass.Substring(0,1)=="1") dgRowInDOC = (byte)(dgclass == "1.4S" ? 1 : 0);
            else switch (dgclass)
            {
                case "2.1":
                    dgRowInDOC = 2;
                    break;
                case "2.2":
                    dgRowInDOC = 3;
                    break;
                case "2.3":
                    dgRowInDOC = (byte)(flammable?4:5);
                    break;
                    case "3":
                    dgRowInDOC = (byte) (dgfp < 23 ? 6 : 7);
                    break;
                case "4.1":
                    dgRowInDOC = 8;
                    break;
                case "4.2":
                    dgRowInDOC = 9;
                    break;
                case "4.3":
                    dgRowInDOC = (byte) (liquid ? 10 : 11);
                    break;
                case "5.1":
                    dgRowInDOC = 12;
                    break;
                case "5.2":
                    dgRowInDOC = 13;
                    break;
                case "6.1":
                    if (liquid)
                    {
                        if (flammable) dgRowInDOC = (byte) (dgfp < 23 ? 14 : 15);
                        else dgRowInDOC = 16;
                    }
                    else dgRowInDOC = 17;
                    break;
                case "8":
                    if (liquid)
                    {
                        if (flammable) dgRowInDOC = (byte) (dgfp < 23 ? 18 : 19);
                        else dgRowInDOC = 20;
                    }
                    else dgRowInDOC = 21;
                    break;
                case "9":
                    dgRowInDOC = 22;
                    break;
                default:
                    dgRowInDOC = 22;
                    break;
            }
        }

        
        /// <summary>
        /// Method to deal with SP in subrisk in DG list
        /// </summary>
        private void SpecialClass()
        {
            foreach(string s in dgsubclass)
            {
                var clearSubclass = false;
                if (s == "–") dgsubclass.Remove(s);
                if (s.StartsWith("See SP"))
                {
                    switch (s.Remove(0,4))
                    {
                        case "SP63":
                            dgclass = "2.1";
                            dgsubclass.Clear();
                            clearSubclass = true;
                            break; 
                        case "172":
                            dgsubclass.Clear();
                            clearSubclass = true;
                            break;
                        case "181":
                            dgsubclass.Clear();
                            dgsubclass.Add("1.3");
                            Output.DisplayLine("Caution! Class 1.3 added to {0} in accordance with SP181.", cntrNr);
                            clearSubclass = true;
                            break;
                        case "204":
                            break;
                        case "271":
                            break;
                        case "290":
                            break;
                        case "943":
                            break;
                        default:
                            Output.DisplayLine("For subrisk of unit {0} refer to special provision list {1}");
                            break;
                    }
                }
                if (clearSubclass) break;
            }
            //unno 2037 assigned class 2 in imdg code with responsibility to determine subdivision given to shippers.
            //for the purpose of calculation, if not specifie in .edi, it will be assigned the most stringent subdivision
            if (unno == 2037)
                dgclass = "2.1";
        }


        /// <summary>
        /// Method to assign segregaion group from Ch 3.3
        /// </summary>
        public void AssignSegregationGroup()
        {
            int[] acids =   //1
{
            1052,1182,1183,1238,1242,1250,1295,1298,1305,1572,1595,1715,1716,1717,
            1718,1722,1723,1724,1725,1726,1727,1728,1729,1730,1731,1732,1733,1736,
            1737,1738,1739,1740,1742,1743,1744,1745,1746,1747,1750,1751,1752,1753,
            1754,1755,1756,1757,1758,1762,1763,1764,1765,1766,1767,1768,1769,1770,
            1771,1773,1775,1776,1777,1778,1779,1780,1781,1782,1784,1786,1787,1788,
            1789,1790,1792,1793,1794,1796,1798,1799,1800,1801,1802,1803,1804,1805,
            1806,1807,1808,1809,1810,1811,1815,1816,1817,1818,1826,1827,1828,1829,
            1830,1831,1832,1833,1834,1836,1837,1838,1839,1840,1848,1873,1898,1902,
            1905,1906,1938,1939,1940,2031,2032,2214,2215,2218,2225,2226,2240,2262,
            2267,2305,2308,2331,2353,2395,2407,2434,2435,2437,2438,2439,2440,2442,
            2443,2444,2475,2495,2496,2502,2503,2506,2507,2508,2509,2511,2513,2531,
            2564,2571,2576,2577,2578,2580,2581,2582,2583,2584,2585,2586,2604,2626,
            2642,2670,2691,2692,2698,2699,2739,2740,2742,2743,2744,2745,2746,2748,
            2751,2789,2790,2794,2796,2798,2799,2802,2817,2819,2820,2823,2826,2829,
            2834,2851,2865,2869,2879,2967,2985,2986,2987,2988,3246,3250,3260,3261,
            3264,3265,3277,3361,3362,3412,3419,3420,3421,3425,3453,3456,3463,3472,
            3498
        };

            int[] ammonium_compounds =  //2
                {
                4,222,402,1310,1439,1442,1444,1512,1546,1630,1727,1835,1843,1942,2067,
                2071,2073,2426,2505,2506,2683,2687,2817,2818,2854,2859,2861,2863,3375,
                3423,3424
            };

            int[] bromates =    //3
            {
            1450,1473,1484,1494,2469,2719,3213,3213
        };

            int[] chlorates =   //4
            {
            1445,1452,1458,1459,1461,1485,1495,1506,1513,2427,2428,2429,2573,2721,2723,3405,3407
        };

            int[] chlorites =   //5
            {
            1453,1462,1496,1908
        };

            int[] cyanides =    //6
            {
            1541,1565,1575,1587,1588,1620,1626,1636,1642,1653,1679,1680,1684,1689,1694,
            1713,1889,1935,2205,2316,2317,3413,3414,3449
        };

            int[] heavy_metals =    //7
            {
            129,130,135,1347,1366,1370,1389,1392,1435,1436,1469,1470,1493,1512,1513,
            1514,1515,1516,1587,1616,1617,1618,1620,1623,1624,1625,1626,1627,1629,1630,
            1631,1634,1636,1637,1638,1639,1640,1641,1642,1643,1644,1645,1646,1649,1653,
            1674,1683,1684,1712,1713,1714,1794,1838,1840,1872,1894,1895,1931,1931,2024,
            2025,2026,2291,2331,2441,2469,2546,2714,2777,2778,2809,2855,2869,2878,2881,
            2989,3011,3012,3089,3174,3181,3189,3401,3402,3408,3483
        };

            int[] hypochlorites =   //8
            {
            1471,1748,1791,2208,2741,2880,3212,3255,3485,3486,3487
        };

            int[] lead =    //9
            {
            129,130,130,1469,1470,1616,1617,1618,1620,1649,1794,1872,2291,2989,3408,3483
        };

            int[] hydrocarbons =    //10
            {
            1099,1100,1107,1126,1127,1134,1150,1152,1184,1278,1279,1303,1591,1593,1605,
            1647,1669,1701,1702,1710,1723,1737,1738,1846,1887,1888,1891,1897,1991,2234,
            2238,2279,2321,2322,2339,2341,2342,2343,2344,2356,2362,2387,2388,2390,2391,
            2392,2456,2504,2515,2554,2644,2646,2664,2688,2831,2872
        };

            int[] mercury = //11
            {
            135,1389,1392,1623,1624,1625,1626,1627,1629,1630,1631,1634,1636,1637,1638,
            1639,1640,1641,1642,1643,1644,1645,1646,1894,1895,2024,2025,2026,2777,2778,
            2809,3011,3012,3401,3402
        };

            int[] nitrites =    //12
                {
            1487,1488,1500,1512,2627,2726,3219
        };

            int[] perchlorates =    //13
            {
            1442,1447,1455,1470,1475,1481,1489,1502,1508,3211,3406,3408
        };

            int[] permanganates =   //14
            {
            1448,1456,1482,1490,1503,1515,3214
        };

            int[] powdered_metals = //15
                {
            1309,1326,1352,1358,1383,1396,1398,1418,1435,1436,1854,2008,2009,2545,2546,2878,
            2881,2950,3078,3089,3170,3189
        };

            int[] peroxides =   //16
            {
            1449,1457,1472,1476,1483,1491,1504,1509,1516,2014,2015,2466,2547,3149,3377,3378
        };

            int[] azides =  //17
            {
            129,224,1571,1687
        };

            int[] alkalis = //18
            {
            1005,1160,1163,1235,1244,1382,1385,1604,1719,1813,1814,1819,1823,1824,1825,
            1835,1847,1849,1907,1922,2029,2030,2033,2073,2079,2259,2270,2318,2320,2379,
            2382,2386,2399,2401,2491,2579,2671,2672,2677,2678,2679,2680,2681,2682,2683,
            2733,2734,2735,2795,2797,2818,2949,3028,3073,3253,3259,3262,3263,3266,3267,
            3293,3318,3320,3423,3484 };

            int[] strongAcids = //19
                {
                1052,1777,1786,1787,1788,1789,1790,1796,1798,1802,1826,
                1830,1831,1832,1873,1906,2031,2032,2240,2308,2796 };

            if (acids.Contains(unno))
                segregationGroup.Add(1);
            if (ammonium_compounds.Contains(unno))
                segregationGroup.Add(2);
            if (bromates.Contains(unno))
                segregationGroup.Add(3);
            if (chlorates.Contains(unno))
                segregationGroup.Add(4);
            if (chlorites.Contains(unno))
                segregationGroup.Add(5);
            if (cyanides.Contains(unno))
                segregationGroup.Add(6);
            if (heavy_metals.Contains(unno))
                segregationGroup.Add(7);
            if (hypochlorites.Contains(unno))
                segregationGroup.Add(8);
            if (lead.Contains(unno))
                segregationGroup.Add(9);
            if (hydrocarbons.Contains(unno))
                segregationGroup.Add(10);
            if (mercury.Contains(unno))
                segregationGroup.Add(11);
            if (nitrites.Contains(unno))
                segregationGroup.Add(12);
            if (perchlorates.Contains(unno))
                segregationGroup.Add(13);
            if (permanganates.Contains(unno))
                segregationGroup.Add(14);
            if (powdered_metals.Contains(unno))
                segregationGroup.Add(15);
            if (peroxides.Contains(unno))
                segregationGroup.Add(16);
            if (azides.Contains(unno))
                segregationGroup.Add(17);
            if (alkalis.Contains(unno))
                segregationGroup.Add(18);
            if (strongAcids.Contains(unno))
                segregationGroup.Add(19);
        }


        /// <summary>
        /// Method creates dg.conflict if not yet created
        /// </summary>
        public void AddConflict()
        {
            if (this.conflict == null)
                this.conflict = new Conflict();

            conflicted = true;
        }

        /// <summary>
        /// Method adds conflict to Dg if 'add' is 'true'. Stowage or segregation conflict will be added as defined. Method combines other conflict related methods in Dg class to reduce nesting.
        /// </summary>
        /// <param name="add"></param>
        /// <param name="stoworsegr"></param>
        /// <param name="code"></param>
        /// <param name="b"></param>
        public void AddConflict(bool add, string stoworsegr, string code, Dg b = null)
        {
            if (!add) return;
            AddConflict();
            if (stoworsegr == "stowage") conflict.AddStowConflict(code);
            else conflict.AddSegrConflict(code, b);
        }


        /// <summary>
        /// Method to copy information from dg container to dg unit
        /// </summary>
        /// <param name="a"></param>
        public void CopyContainerInfo(Container a)
        {
            cntrNr = a.cntrNr;
            cntrType = a.cntrType;
            cntrLocation = a.cntrLocation;
            bay = a.bay;
            row = a.row;
            tier = a.tier;
            underdeck = a.underdeck;
            size = a.size;
            holdNr = a.holdNr;
            closed = a.closed;
            cnPOD = a.cnPOD;
            cnPOL = a.cnPOL;
            RF = a.RF;

        }


        /// <summary>
        /// Defines compatibility group for segregation of class 1
        /// </summary>
        public void DefineCompatibilityGroup()
        {
            foreach(string s in allDgClasses)
                if(s.StartsWith("1"))
                    compatibilityGroup = dgclass.Length > 3 ? char.ToUpper(dgclass[3]) : '0';
        }

        /// <summary>
        /// Method clears all conflicts in dg unit and changes 'conflicted' status to false
        /// </summary>
        public void ClearConflicts()
        {
            if (!conflicted) return;
            this.conflicted = false;
            this.conflict._segrConflicts.Clear();
            this.conflict._stowConflicts.Clear();
            this.conflict.failedSegregation = false;
            this.conflict.failedStowage = false;
        }




        //---------------------- Supporting classes -----------------------------------------------------------------------

        /// <summary>
        /// Class to contain exception for Segregator (class which should be used for segregation instead of given class from DG list)
        /// </summary>
        public class SegregatorException
        {
            public string segrClass;
            public byte segrCase;
            public SegregatorException (string _class, byte _segrCase)
            {
                segrClass = _class;
                segrCase = _segrCase;
            }
        }

        /// <summary>
        /// Supporting method for easier identification of units when debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CntrNr + " in " + cntrLocation + " class " + dgclass + " (unno " + UNNO + ")";
        }

    }
}
