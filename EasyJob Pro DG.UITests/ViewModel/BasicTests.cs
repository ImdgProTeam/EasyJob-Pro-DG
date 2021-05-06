using EasyJob_ProDG.Model.Cargo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyJob_ProDG.UI.Wrapper.Tests
{
    [TestClass()]
    public class BasicTests
    {
        private Dg _dg;

        [TestInitialize]
        public void Initialize()
        {
            _dg = new Dg
            {
                ContainerNumber = "ABCD1234567",
                Location = "0111288",
                Unno = 1225

            };
        }


        [TestMethod()]
        public void ShouldContainModelInModelProperty()
        {
            var wrapper = new DgWrapper(_dg);
            Assert.AreEqual(_dg, wrapper.Model);
        }


    }
}