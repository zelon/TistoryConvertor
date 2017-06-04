using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TistoryConvertorTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCheckTestFile()
        {
            Assert.IsTrue(File.Exists("../../../test_data.xml"));
        }
    }
}
