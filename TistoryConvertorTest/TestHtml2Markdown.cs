using Microsoft.VisualStudio.TestTools.UnitTesting;
using TistoryConvertor;

namespace TistoryConvertorTest
{
    [TestClass]
    public class TestHtml2Markdown
    {
        public string GetConvert(string content)
        {
            return Html2Markdown.Convert(content).Trim();

        }

        [TestMethod]
        public void TestBold()
        {
            Assert.AreEqual(GetConvert("<b>kim jinwook</b>"), "**kim jinwook**");
        }
    }
}
