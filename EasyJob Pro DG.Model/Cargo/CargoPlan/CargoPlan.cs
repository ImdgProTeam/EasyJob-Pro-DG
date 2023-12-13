using EasyJob_ProDG.Model.Transport;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public class CargoPlan : ICargoPlan
    {
        public List<Dg> DgList { get; set; }
        public ICollection<Container> Containers { get; set; }
        public ICollection<Container> Reefers { get; set; }
        public Voyage VoyageInfo { get; set; }

        /// <summary>
        /// True if <see cref="CargoPlan"/> contains no <see cref="Container"/> nor <see cref="Dg"/>
        /// </summary>
        public bool IsEmpty => Containers.Count <= 0 && DgList.Count <= 0 && Reefers.Count <= 0;
        public decimal TotalDgNetWeight
        {
            get
            {
                decimal sum = 0M;
                foreach (var dg in DgList)
                {
                    sum += dg.DgNetWeight;
                }
                return sum;
            }
        }
        public decimal TotalMPNetWeight
        {
            get
            {
                decimal sum = 0M;
                foreach (var dg in DgList)
                {
                    if (dg.IsMp)
                        sum += dg.DgNetWeight;
                }
                return sum;
            }
        }

        /// <summary>
        /// True if <see cref="CargoPlan"/> contains <see cref="Container"/>s without numbers.
        /// </summary>
        internal bool HasNonamers => Containers.Any(c => c.HasNoNumber);
        internal int NextNonamerNumber;


        // -------------- Constructors ----------------------------------------------

        /// <summary>
        /// Creates CargoPlan with initiated blank Lists.
        /// </summary>
        public CargoPlan()
        {
            DgList = new List<Dg>();
            Reefers = new List<Container>();
            Containers = new List<Container>();
            VoyageInfo = new Voyage();
        }
    }
}
