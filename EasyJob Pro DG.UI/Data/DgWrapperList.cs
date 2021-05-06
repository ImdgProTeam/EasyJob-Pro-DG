using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace EasyJob_ProDG.UI.Data
{
    public class DgWrapperList : ObservableCollection<DgWrapper>
    {
        // ---------------- Public properties -------------------------------------

        public int ContainerCount { get; set; }
        public bool IsCollectionNew { get; set; }


        // ----------------- Constructors -----------------------------------------

        public DgWrapperList()
        {
            IsCollectionNew = true;
        }

        public DgWrapperList(ICollection<Dg> dgList)
        {
            IsCollectionNew = true;

            foreach (var dg in dgList)
            {
                DgWrapper dgW = new DgWrapper(dg)
                {
                    IsInList = true
                };
                Add(dgW);
            }
        }


        // ----------------- Methods ----------------------------------------------

        /// <summary>
        /// Method to count actual number of containers in dg list
        /// </summary>
        public void ReCountDgContainers()
        {
            string number = "";
            int count = 0;
            foreach (var unit in this)
            {
                if (unit.ContainerNumber == number) continue;
                count++;
                number = unit.ContainerNumber;
            }

            ContainerCount = count;
            OnPropertyChanged(new PropertyChangedEventArgs("ContainerCount"));
        }

        /// <summary>
        /// Method searches dgList for a dg with specified container number
        /// </summary>
        /// <param name="containerNumber"></param>
        /// <returns></returns>
        public bool Contains(string containerNumber)
        {
            return this.Any(c => c.ContainerNumber == containerNumber);
        }

        /// <summary>
        /// Method invokes an event when property of dg changes that requires collection to be re-checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnDgPropertyChanged(object sender, EventArgs e)
        {
            dgInListPropertyChanged.Invoke(this, e);
        }

        /// <summary>
        /// Converts DgWrapperList into List of Dg
        /// </summary>
        /// <returns>List of POCO Dg</returns>
        public List<Dg> ConvertToDgList()
        {
            var dgList = new List<Dg>();

            foreach (var dgWrapper in this)
            {
                dgList.Add((Dg)dgWrapper);
            }

            return dgList;
        }

        // --------------- Events -------------------------------------------------

        public event EventHandler dgInListPropertyChanged = null;


        // --------------- Explicit operators -------------------------------------

        /// <summary>
        /// Explicitly converts DgWrapperList to DgList<Dg>
        /// </summary>
        /// <param name="dgWrapperList"></param>
        public static explicit operator List<Dg>(DgWrapperList dgWrapperList)
        {
            return dgWrapperList.ConvertToDgList();
        }

    }
}
