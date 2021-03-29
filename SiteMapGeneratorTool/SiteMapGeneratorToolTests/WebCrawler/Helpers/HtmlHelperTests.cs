using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SiteMapGeneratorTool.WebCrawler.Helpers.Tests
{
    [TestFixture()]
    public class HtmlHelperTests
    {
        private IConfiguration Configuration;
        private string Target;
        private HtmlHelper HtmlHelper;

        [OneTimeSetUp]
        public void CrawlerOneTimeSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Target = Configuration.GetValue<string>("Test:Target");
        }

        [SetUp]
        public void HtmlHelperSetup()
        {
            HtmlHelper = new HtmlHelper();
        }

        [Test()]
        public void CreateDocumentTest()
        {
            DateTime? expected = new DateTime(2020, 01, 01);
            DateTime? actual = HtmlHelper.CreateDocument(new Uri(Target), 0);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Value < actual.Value);
        }

        [Test()]
        public void GenerateTagsTest()
        {
            HtmlHelper.CreateDocument(new Uri(Target + "smelly"), 0);
            Assert.AreEqual(new List<string> { "smelly/ajar", "smelly/addition", "smelly/vest", "https://www.ajar.com", "https://www.addition.com", "https://www.vest.com" }, HtmlHelper.GenerateTags());
        }

        [Test()]
        public void GetLastModifiedTest()
        {
            DateTime? expected = new DateTime(2020, 01, 01);
            DateTime? actual = HtmlHelper.GetLastModified(new Uri(Target));

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Value < actual.Value);
        }
    }
}