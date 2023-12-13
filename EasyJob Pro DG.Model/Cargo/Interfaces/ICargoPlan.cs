using System.Collections.Generic;

namespace EasyJob_ProDG.Model.Cargo
{
    public interface ICargoPlan
    {
        List<Dg> DgList { get; }
        ICollection<Container> Containers { get; }
        ICollection<Container> Reefers { get; }
    }
}