using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyJob_Pro_DG.ViewModel;
using System.Xml.Linq;


namespace EasyJob_Pro_DG
{
    public class DgList : ObservableCollection<Dg>
    {
        public static bool changeCompleted;
        public bool collectionNew;
        private int containerCount;
        private ShipProfile ownship = MainViewModel.ownship;

        public int ContainerCount
        {
            get { return containerCount; }
            set
            {
                if(containerCount != value)
                {
                    containerCount = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ContainerCount"));
                }
            }
        }

        public DgList() { }

        public void CollectionUpdate()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            if (MainWindow.started && changeCompleted)
            {
                ownship = MainViewModel.ownship;
                MainViewModel.RecheckDgList(this);

                //Display info
                MainViewModel.conflicts.CreateConflictList(this);
            }
        }

        public DgList(List<Dg> dgList)
        {
            foreach (var dg in dgList)
            {
                this.Add(dg);
            }
        }

        public void CreateDGList(string fileName, ShipProfile ownship, ObservableCollection<Container> Reefers, ObservableCollection<Container> containers, XDocument dgDataBase, OpenFile.FileTypes filetype = OpenFile.FileTypes.EDI)
        {
            changeCompleted = false;
            this.Clear();
            Reefers?.Clear();
            containers?.Clear();

            List<Dg> dgList;
            List<Container> _reefers = new List<Container>();
            List<Container> _containers = new List<Container>();

            switch (filetype)
            {
                case OpenFile.FileTypes.EDI:
                    var edi = new ReadFile(fileName, ownship, out _reefers);
                    dgList = edi.dgList;
                    _containers = edi.Containers;
                    break;
                case OpenFile.FileTypes.EXCEL:
                    dgList = WithXl.Import(fileName, ownship, out containers);
                    
                    foreach (var c in containers)
                        if (c.RF)
                            _reefers.Add(c);
                    break;
                default:
                    dgList = new List<Dg>();
                    _containers = new List<Container>();
                    break;
            }

            ProgramFiles.UpdateDGInfo(dgList, dgDataBase);
            ProgramFiles.CheckDGList(dgList, (byte)OpenFile.FileTypes.EDI);

            foreach (var _cont in _containers)
            {
                containers?.Add(_cont);
            }

            foreach (var _ref in _reefers)
            {
                Reefers?.Add(_ref);
            }

            foreach (var dg in dgList)
            {
                dg.IsInList = true;
                Add(dg);
            }
            ReCount();
            changeCompleted = true;
            collectionNew = true;
        }

        public void Replace(Dg unit)
        {

        }

        /// <summary>
        /// Method to count actual number of containers in dg list
        /// </summary>
        public void ReCount()
        {
            string number="";
            int count = 0;
            foreach (var unit in this)
            {
                if (unit.cntrNr == number) continue;
                count++;
                number = unit.cntrNr;
            }

            ContainerCount = count;

        }

        /// <summary>
        /// Method searches dgList for a dg with specified container number
        /// </summary>
        /// <param name="cntrNr"></param>
        /// <returns></returns>
        public bool Contains(string cntrNr)
        {
            foreach (var unit in this)
            {
                if (unit.cntrNr == cntrNr)
                    return true;
            }

            return false;
        }

    }
}
