using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SiteMapGeneratorTool.WebCrawler.Helpers.Tests
{
    [TestFixture()]
    public class RobotsHelperTests
    {
        private IConfiguration Configuration;
        private string Target;
        private RobotsHelper RobotsHelper;

        [OneTimeSetUp]
        public void CrawlerOneTimeSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Target = Configuration.GetValue<string>("Test:Target");
        }

        [SetUp()]
        public void RobotsHelperSetUp()
        {
            RobotsHelper = new RobotsHelper();
        }

        [Test()]
        public void FindExclusionsTest()
        {
            List<string> expected = new List<string>
            {
                "# google.com landing page quality check",
                "User-agent: AdsBot-Google",
                "Disallow: ",
                string.Empty,
                "User-agent: *",
                "Disallow: /example1",
                "Disallow: /example2",
                "Disallow: /example3",
                string.Empty
            };

            Assert.AreEqual(new List<string>(), RobotsHelper.Exclusions);
            RobotsHelper.FindExclusions(new Uri(Target));
            Assert.AreEqual(expected, RobotsHelper.Exclusions);
        }

        [Test()]
        public void GetExlusionsTest()
        {
            RobotsHelper.FindExclusions(new Uri(Target));

            List<string> expected = new List<string> 
            { 
                "/example1",
                "/example2",
                "/example3",
            };
            List<string> actual = RobotsHelper.GetExlusions();

            Assert.AreEqual(expected, actual);
        }
    }
}