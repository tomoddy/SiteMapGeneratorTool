using NUnit.Framework;
using SiteMapGeneratorTool.WebCrawler.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMapGeneratorTool.WebCrawler.Helpers.Tests
{
    [TestFixture()]
    public class RobotsHelperTests
    {
        private const string URL = "http://www.latex-project.org";

        private RobotsHelper RobotsHelper;

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
                "Disallow: /cgi-bin/",
                "Disallow: /publications/eurotex-2005-notes/",
                "Disallow: /publications/pdfTeX-meeting-2005-09-24/",
                string.Empty
            };

            Assert.AreEqual(new List<string>(), RobotsHelper.Exclusions);
            RobotsHelper.FindExclusions(new Uri(URL));
            Assert.AreEqual(expected, RobotsHelper.Exclusions);
        }

        [Test()]
        public void GetExlusionsTest()
        {
            RobotsHelper.FindExclusions(new Uri(URL));

            List<string> expected = new List<string> 
            { 
                "/cgi-bin/",
                "/publications/eurotex-2005-notes/",
                "/publications/pdfTeX-meeting-2005-09-24/",
            };
            List<string> actual = RobotsHelper.GetExlusions();

            Assert.AreEqual(expected, actual);
        }
    }
}