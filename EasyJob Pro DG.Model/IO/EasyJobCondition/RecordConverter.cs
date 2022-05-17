using EasyJob_ProDG.Model.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyJob_ProDG.Model.IO.EasyJobCondition
{
    internal static class RecordConverter
    {
        internal static CargoPlan AddToCargoPlan(this CargoPlan cargoPlan)
        {
            return cargoPlan;
        }
    }
}
