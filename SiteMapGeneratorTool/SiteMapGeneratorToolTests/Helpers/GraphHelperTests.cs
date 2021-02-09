using NUnit.Framework;
using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace SiteMapGeneratorTool.Helpers.Tests
{
    [TestFixture()]
    public class GraphHelperTests
    {
        List<Webpage> Pages;

        [Test()]
        public void RenderSmallTest()
        {
            Pages = new List<Webpage>()
            {
                new Webpage(new Uri("http://sitemaps.org/"), null) { Links = new List<Uri>()
                {
                    new Uri("http://sitemaps.org/faq.php"),
                    new Uri("http://sitemaps.org/protocol.php"),
                    new Uri("http://sitemaps.org/terms.php")
                }},
                new Webpage(new Uri("http://sitemaps.org/protocol.php"), null) { Links = new List<Uri>()
                {
                    new Uri("http://sitemaps.org/faq.php"),
                    new Uri("http://sitemaps.org/#"),
                    new Uri("http://sitemaps.org/terms.php")
                }},
                new Webpage(new Uri("http://sitemaps.org/terms.php"), null) { Links = new List<Uri>()
                {
                    new Uri("http://sitemaps.org/faq.php"),
                    new Uri("http://sitemaps.org/protocol.php"),
                    new Uri("http://sitemaps.org/#")
                }},
                new Webpage(new Uri("http://sitemaps.org/faq.php"), null) { Links = new List<Uri>()
                {
                    new Uri("http://sitemaps.org/protocol.php"),
                    new Uri("http://sitemaps.org/#"),
                    new Uri("http://sitemaps.org/terms.php")
                }}
            };

            Assert.AreEqual(File.ReadAllBytes("Static/valid-graph.png"), GraphHelper.Render(Pages));
        }

        [Test()]
        public void RenderLargeTest()
        {
            Pages = new List<Webpage>();
            for (int i = 0; i <= 200; i++)
                Pages.Add(new Webpage(new Uri("http://dummy1.com"), DateTime.Now) { Links = new List<Uri> { new Uri("http://dummy2.com"), new Uri("http://dummy3.com") } });

            Assert.AreEqual(File.ReadAllBytes("Static/invalid-graph.png"), GraphHelper.Render(Pages));
        }
    }
}