using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DesignTimeViewModel
    {
        private CargoPlanWrapper _workingCargoPlan;
        public CargoPlanWrapper WorkingCargoPlan
        {
            get { return _workingCargoPlan;}
            set { _workingCargoPlan=value; }
        }

        public DgWrapper SelectedDg { get; set; }

        public CargoPlanWrapper CargoPlan => WorkingCargoPlan;


        public DesignTimeViewModel()
        {
            _workingCargoPlan = new CargoPlanWrapper(new CargoPlan())
            {
                DgList = new DgWrapperList()
                {

                    new DgWrapper()
                    {
                        ContainerNumber = "ABCD1234567",
                        POL = "Miami",
                        POD = "Nakhodka",
                        Location = "0050688",
                        Unno = 1234,
                        DgClass = "1.1G",
                        DgSubClass = "6.2",
                        DgNetWeight = 1000.1M,
                        Name = "Test dangerous goods, NOS",
                        PackingGroup = "III",
                        FlashPoint = "-13.1",
                        IsLq = false,
                        IsMp = false,
                        IsClosed = true,
                        IsRf = true,
                        StowageCat = 'D',
                        SegregationGroup = "test"
                    },

                    new DgWrapper()
                    {
                        ContainerNumber = "ABCD1234567",
                        POL = "Miami",
                        POD = "Nakhodka",
                        Location = "0050688",
                        Unno = 1234,
                        DgClass = "1.1G",
                        DgSubClass = "6.2",
                        DgNetWeight = 1000.1M,
                        Name = "Test dangerous goods, NOS",
                        PackingGroup = "III",
                        FlashPoint = "-13.1",
                        IsLq = false,
                        IsMp = false,
                        IsClosed = true,
                        IsRf = true,
                        StowageCat = 'D',
                        SegregationGroup = "test"
                    }
                }
            };

            SelectedDg = _workingCargoPlan.DgList[0];
        }
    }
}
