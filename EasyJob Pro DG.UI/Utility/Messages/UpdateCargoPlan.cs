using System.Collections.Generic;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Messages
{
    internal class UpdateCargoPlan
    {
        private IEnumerable<DgWrapper> _dgWrappers;

        public IEnumerable<DgWrapper> DgWrappersChanged
        {
            get => _dgWrappers;
            set { _dgWrappers = value; }
        }


        public UpdateCargoPlan()
        {

        }

        public UpdateCargoPlan(IEnumerable<DgWrapper> dgWrappersChanged)
        {
            _dgWrappers = dgWrappersChanged;
        }
    }
}