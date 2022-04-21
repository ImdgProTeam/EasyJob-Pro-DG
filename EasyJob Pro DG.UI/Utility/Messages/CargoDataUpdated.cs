using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Messages
{
    /// <summary>
    /// Message class used for manipulation with CargoPlan units.
    /// </summary>
    internal class CargoDataUpdated
    {
        private bool _reeferRemoved;
        private DgWrapper _dgRemoved;

        public bool ReeferRemoved
        {
            get { return _reeferRemoved; }
            set { _reeferRemoved = value; }
        }

        public DgWrapper DgRemoved
        {
            get => _dgRemoved;
            set { _dgRemoved = value; }
        }


        public CargoDataUpdated()
        {

        }


        public CargoDataUpdated(bool reeferRemoved)
        {
            _reeferRemoved = reeferRemoved;
        }

        public CargoDataUpdated(DgWrapper dgRemoved)
        {

        }
    }
}
