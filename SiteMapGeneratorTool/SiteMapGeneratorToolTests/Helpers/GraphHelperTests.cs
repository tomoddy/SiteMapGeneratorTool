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
        [Test()]
        public void RenderSmallTest()
        {
            List<Webpage> pages = new List<Webpage>()
            {
                new Webpage(new Uri("http://example.org/"), null) { Links = new List<Uri>()
                {
                    new Uri("http://example.org/example1"),
                    new Uri("http://sitemaps.org/example2"),
                    new Uri("http://sitemaps.org/example3")
                }},
                new Webpage(new Uri("http://example.org/example1"), null) { Links = new List<Uri>() { } },
                new Webpage(new Uri("http://example.org/example2"), null) { Links = new List<Uri>() { } },
                new Webpage(new Uri("http://example.org/example3"), null) { Links = new List<Uri>() { } }
            };

            Assert.AreEqual(File.ReadAllBytes("Static/valid-graph.png"), GraphHelper.Render(pages));
        }

        [Test()]
        public void RenderLargeTest()
        {
            List<Webpage> pages = new List<Webpage>();
            for (int i = 0; i <= 200; i++)
                pages.Add(new Webpage(new Uri("http://dummy1.com"), DateTime.Now) { Links = new List<Uri> { new Uri("http://dummy2.com"), new Uri("http://dummy3.com") } });

            Assert.AreEqual(File.ReadAllBytes("Static/invalid-graph.png"), GraphHelper.Render(pages));
        }
    }
}