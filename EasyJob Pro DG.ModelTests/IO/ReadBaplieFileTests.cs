using EasyJob_ProDG.Model.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyJob_ProDG.ModelTests.IO
{
    [TestClass]
    public class ReadBaplieFileTests
    {
        PrivateObject privateObject;
        ReadBaplieFile instance;

        public ReadBaplieFileTests()
        {
            instance = new ReadBaplieFile();
            privateObject = new PrivateObject(instance);   
        }

        [TestMethod]
        public void TestGenerateNextNonamerNumber()
        {
            int[] inputs = {0, 1, 2, 100, 200, 1000, 1001, 1099, 1100, 3599, 3600, 3601, 3609, 3610, 3611,
            3859, 3860, 3871, 5000, 10001, 10359, 10360, 10361, 20000, 27935};
            string[] expectedResults = { "000", "001", "002", "100", "200", "A00", "A01", "A99", "B00", "Z99", "AA0", "AA1", "AA9", "AB0", "AB1",
            "AZ9", "BA0", "BB1", "FK0", "YQ1", "ZZ9", "AAA", "AAB", "OGU", "ZZZ"};

            for (int i=0; i<inputs.Length; i++)
            {
                var returnValue = privateObject.Invoke("GenerateNextNonamerNumber", inputs[i]);
                Assert.AreEqual(returnValue, expectedResults[i]);
            }
        }
    }
}
