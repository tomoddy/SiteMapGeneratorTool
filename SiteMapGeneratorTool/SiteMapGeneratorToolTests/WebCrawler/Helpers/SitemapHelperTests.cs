using NUnit.Framework;
using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;

namespace SiteMapGeneratorTool.WebCrawler.Helpers.Tests
{
    [TestFixture()]
    public class SitemapHelperTests
    {
        private const string LINK = "http://www.teststring.com";

        private const string EXT_1 = "/link1";
        private const string EXT_2 = "/link2";
        private const string EXT_3 = "/link3";

        [Test()]
        public void GenerateSitemapTest()
        {
            List<Webpage> webpages = new List<Webpage>
            {
                new Webpage(LINK + EXT_1, new DateTime(2021, 2, 3), new List<string> { LINK + EXT_2, LINK + EXT_3 }),
                new Webpage(LINK + EXT_2, new DateTime(2024, 5, 6), new List<string> { LINK + EXT_3, LINK + EXT_1 }),
                new Webpage(LINK + EXT_3, new DateTime(2027, 8, 9), new List<string> { LINK + EXT_1, LINK + EXT_2 })
            };

            Sitemap expected = new Sitemap();
            foreach (Webpage webpage in webpages)
                expected.Urls.Add(new Url { Location = webpage.Url.AbsoluteUri, LastModified = webpage.LastModified != null ? webpage.LastModified.Value.ToString("yyyy-MM-dd") : "NULL" });

            Sitemap actual = SitemapHelper.GenerateSitemap(webpages);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test()]
        public void GenerateXmlTest()
        {
            List<Webpage> webpages = new List<Webpage>
            {
                new Webpage(LINK + EXT_1, new DateTime(2021, 2, 3), new List<string> { LINK + EXT_2, LINK + EXT_3 }),
                new Webpage(LINK + EXT_2, new DateTime(2024, 5, 6), new List<string> { LINK + EXT_3, LINK + EXT_1 }),
                new Webpage(LINK + EXT_3, new DateTime(2027, 8, 9), new List<string> { LINK + EXT_1, LINK + EXT_2 })
            };

            Sitemap sitemap = new Sitemap();
            foreach (Webpage webpage in webpages)
                sitemap.Urls.Add(new Url { Location = webpage.Url.AbsoluteUri, LastModified = webpage.LastModified != null ? webpage.LastModified.Value.ToString("yyyy-MM-dd") : "NULL" });

            SitemapHelper sitemapHelper = new SitemapHelper();

            string actual = sitemapHelper.GenerateXml(sitemap);
            string expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n  <url>\n    <loc>http://www.teststring.com/link1</loc>\n    <lastmod>2021-02-03</lastmod>\n  </url>\n  <url>\n    <loc>http://www.teststring.com/link2</loc>\n    <lastmod>2024-05-06</lastmod>\n  </url>\n  <url>\n    <loc>http://www.teststring.com/link3</loc>\n    <lastmod>2027-08-09</lastmod>\n  </url>\n</urlset>";

            Assert.AreEqual(expected, actual);
        }
    }
}