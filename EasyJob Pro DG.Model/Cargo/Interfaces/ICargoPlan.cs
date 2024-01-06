using System.Collections.Generic;

namespace EasyJob_ProDG.Model.Cargo
{
    public interface ICargoPlan
    {
        List<Dg> DgList { get; }
        List<Container> Containers { get; }
        List<Container> Reefers { get; }
    }
}