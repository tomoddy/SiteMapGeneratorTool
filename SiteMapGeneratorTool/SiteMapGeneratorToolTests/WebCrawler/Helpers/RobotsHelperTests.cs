﻿using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SiteMapGeneratorTool.WebCrawler.Helpers.Tests
{
    [TestFixture()]
    public class RobotsHelperTests
    {
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
            RobotsHelper.FindExclusions(new Uri("http://www.latex-project.org"));
            Assert.AreEqual(expected, RobotsHelper.Exclusions);
        }

        [Test()]
        public void GetExlusionsTest()
        {
            RobotsHelper.FindExclusions(new Uri("http://www.latex-project.org"));

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