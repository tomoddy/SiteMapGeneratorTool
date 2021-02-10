using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;

namespace SiteMapGeneratorTool.WebCrawler.Tests
{
    [TestFixture()]
    public class CrawlerTests
    {
        Crawler Crawler;
        IConfiguration Configuration;

        [OneTimeSetUp]
        public void CrawlerOneTimeSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // TODO Change to test domain and add tests for different configurations
            Crawler = new Crawler("http://sitemaps.org", false, false, Configuration.GetValue<int>("Threads"));
            Crawler.Run();
        }

        [Test()]
        public void RunTest()
        {
            Assert.That(DateTime.Now, Is.EqualTo(Crawler.Completion).Within(TimeSpan.FromSeconds(15)));
            Assert.AreEqual("http://sitemaps.org/", Crawler.Domain.AbsoluteUri);
            Assert.Less(1, Crawler.Elapsed);
            Assert.AreEqual(8, Crawler.Pages);
            Assert.AreEqual(4, Crawler.Webpages.Count);
        }

        [Test()]
        public void GetStructureJsonTest()
        {
            string actual = Crawler.GetStructureJson();

            StringAssert.Contains("{\"Address\":\"/\",\"Link\":\"http://sitemaps.org\",\"Level\":0,\"Pages\":[", actual);
            StringAssert.Contains("{\"Address\":\"faq.php\",\"Link\":\"http://sitemaps.org/faq.php\",\"Level\":1,\"Pages\":[]}", actual);
            StringAssert.Contains("{\"Address\":\"protocol.php\",\"Link\":\"http://sitemaps.org/protocol.php\",\"Level\":1,\"Pages\":[]}", actual);
            StringAssert.Contains("{\"Address\":\"terms.php\",\"Link\":\"http://sitemaps.org/terms.php\",\"Level\":1,\"Pages\":[]}", actual);
        }

        [Test()]
        public void GetSitemapXmlTest()
        {
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\r\n  <url>\r\n    <loc>http://sitemaps.org/</loc>\r\n    <lastmod>2020-05-15</lastmod>\r\n  </url>\r\n  <url>\r\n    <loc>http://sitemaps.org/faq.php</loc>\r\n    <lastmod>2020-05-15</lastmod>\r\n  </url>\r\n  <url>\r\n    <loc>http://sitemaps.org/protocol.php</loc>\r\n    <lastmod>2020-05-15</lastmod>\r\n  </url>\r\n  <url>\r\n    <loc>http://sitemaps.org/terms.php</loc>\r\n    <lastmod>2020-05-15</lastmod>\r\n  </url>\r\n</urlset>", Crawler.GetSitemapXml());
        }
    }
}