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
        public void Render()
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

            Assert.AreEqual("digraph\r\n{\r\n    concentrate = true\r\n\r\n    \"/\" -> \"/example1\"\r\n    \"/\" -> \"/example2\"\r\n    \"/\" -> \"/example3\"\r\n}", GraphHelper.Render(pages));
        }
    }
}