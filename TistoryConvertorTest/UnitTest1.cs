using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TistoryConvertor;

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

        [TestMethod]
        public void TestImagePart1()
        {
            string test_msg = "<br />[##_1C|test.png|width=\"500\" height=\"333\" filename=\"test.png\" alt=\"사용자 삽입 이미지\"|_##] <br />";
            var result = MarkdownConvertor.TistoryImageReplacerToHtmlImageTag(test_msg);
            Assert.IsTrue(result == "<br /><img src=\"test.png\" width=\"500\" height=\"333\" /> <br />");
        }

        [TestMethod]
        public void TestImagePart2()
        {
            string test_msg = "<br />[##_1C|cfile23.uf.12420A0E49CB545E771456.png|width=\"500\" height=\"333\" alt=\"사용자 삽입 이미지\"|_##] <br />";
            var result = MarkdownConvertor.TistoryImageReplacerToHtmlImageTag(test_msg);
            Assert.IsTrue(result == "<br /><img src=\"cfile23.uf.12420A0E49CB545E771456.png\" width=\"500\" height=\"333\" /> <br />");
        }

        [TestMethod]
        public void TestImagePart3()
        {
            string test_msg = "<br />[##_1R|cfile23.uf.12420A0E49CB545E771456.png|width=\"500\" height=\"333\" alt=\"사용자 삽입 이미지\"|_##] <br />";
            var result = MarkdownConvertor.TistoryImageReplacerToHtmlImageTag(test_msg);
            Assert.IsTrue(result == "<br /><img src=\"cfile23.uf.12420A0E49CB545E771456.png\" width=\"500\" height=\"333\" /> <br />");
        }

        [TestMethod]
        public void TestImagePart4()
        {
            string test_msg = "[##_1C|test.png|width=\"500\" height=\"333\" alt=\"\"|_##]";
            var result = MarkdownConvertor.TistoryImageReplacerToHtmlImageTag(test_msg);
            Assert.IsTrue(result == "<img src=\"test.png\" width=\"500\" height=\"333\" />");
        }
    }
}
