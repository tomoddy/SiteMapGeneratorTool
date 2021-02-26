using NUnit.Framework;
using System.Linq;

namespace SiteMapGeneratorTool.WebCrawler.Objects.Tests
{
    [TestFixture()]
    public class PageTests
    {
        [Test()]
        public void AddTest()
        {
            Page page = new Page("http://example.org", 0, 5);
            page.Add("test/test2");

            Assert.AreEqual("http://example.org", page.Address);
            Assert.AreEqual(0, page.Level);
            Assert.AreEqual(1, page.Pages.Count);

            Assert.AreEqual("test", page.Pages.FirstOrDefault().Address);
            Assert.AreEqual(1, page.Pages.FirstOrDefault().Level);
            Assert.AreEqual(1, page.Pages.FirstOrDefault().Pages.Count);

            Assert.AreEqual("test2", page.Pages.FirstOrDefault().Pages.FirstOrDefault().Address);
            Assert.AreEqual(2, page.Pages.FirstOrDefault().Pages.FirstOrDefault().Level);
            Assert.AreEqual(0, page.Pages.FirstOrDefault().Pages.FirstOrDefault().Pages.Count);
        }

        [Test()]
        public void AddWithLimitTest()
        {
            Page page = new Page("http://example.org", 0, 1);
            page.Add("test/test2");

            Assert.AreEqual("http://example.org", page.Address);
            Assert.AreEqual(0, page.Level);
            Assert.AreEqual(1, page.Pages.Count);

            Assert.AreEqual("test", page.Pages.FirstOrDefault().Address);
            Assert.AreEqual(1, page.Pages.FirstOrDefault().Level);
            Assert.AreEqual(0, page.Pages.FirstOrDefault().Pages.Count);
        }

        [Test()]
        public void GenerateLinkTest()
        {
            Page page = new Page("http://example.org", 0, 5);
            page.Add("test");
            Assert.AreEqual(string.Empty, page.Pages.FirstOrDefault().Link);

            page.Pages.FirstOrDefault().GenerateLink("http://example.org");
            Assert.AreEqual("http://example.org/test", page.Pages.FirstOrDefault().Link);
        }

        [Test()]
        public void SortTest()
        {
            Page page = new Page("http://example.org", 0, 5);
            page.Add("c");
            page.Add("a");
            page.Add("b");

            Assert.AreEqual("c", page.Pages[0].Address);
            Assert.AreEqual("a", page.Pages[1].Address);
            Assert.AreEqual("b", page.Pages[2].Address);

            page.Sort();

            Assert.AreEqual("a", page.Pages[0].Address);
            Assert.AreEqual("b", page.Pages[1].Address);
            Assert.AreEqual("c", page.Pages[2].Address);
        }
    }
}