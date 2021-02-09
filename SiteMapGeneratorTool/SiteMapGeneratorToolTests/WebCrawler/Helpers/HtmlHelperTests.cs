using SiteMapGeneratorTool.WebCrawler.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SiteMapGeneratorTool.WebCrawler.Helpers.Tests
{
    [TestFixture()]
    public class HtmlHelperTests
    {
        // TODO Replace with test domain
        private const string URL = "http://www.sitemaps.org";

        private HtmlHelper HtmlHelper;

        [SetUp]
        public void HtmlHelperSetup()
        {
            HtmlHelper = new HtmlHelper();
        }

        [Test()]
        public void CreateDocumentTest()
        {
            DateTime? expected = new DateTime(2020, 01, 01);
            DateTime? actual = HtmlHelper.CreateDocument(new Uri(URL));

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Value < actual.Value);
        }

        [Test()]
        public void GenerateTagsTest()
        {
            HtmlHelper.CreateDocument(new Uri(URL));

            List<string> expected = new List<string> { "faq.php", "protocol.php", "#", "protocol.php", "http://creativecommons.org/licenses/by-sa/2.5/", "terms.php", "/sitemaps.css" };
            List<string> actual = HtmlHelper.GenerateTags();

            Assert.AreEqual(expected, actual);
        }

        [Test(), Order(3)]
        public void GetLastModifiedTest()
        {
            DateTime? expected = new DateTime(2020, 01, 01);
            DateTime? actual = HtmlHelper.GetLastModified(new Uri(URL));

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Value < actual.Value);
        }
    }
}