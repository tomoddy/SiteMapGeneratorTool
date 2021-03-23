using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;

namespace SiteMapGeneratorTool.WebCrawler.Tests
{
    [TestFixture()]
    public class CrawlerTests
    {
        private Crawler Crawler;
        private IConfiguration Configuration;
        private string Target;

        [OneTimeSetUp]
        public void CrawlerOneTimeSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Target = Configuration.GetValue<string>("Test:Target");
            Crawler = new Crawler(Target, 1000, 1000, false, false, Configuration.GetValue<int>("Threads"));
            Crawler.Run();
        }

        [Test()]
        public void RunTest()
        {
            Assert.That(DateTime.Now, Is.EqualTo(Crawler.Completion).Within(TimeSpan.FromSeconds(15)));
            Assert.AreEqual(Target, Crawler.Domain.AbsoluteUri);
            Assert.Less(1, Crawler.Elapsed);
            Assert.AreEqual(104, Crawler.Pages);
        }

        [Test()]
        public void GetStructureJsonTest()
        {
            string actual = Crawler.GetStructureJson();

            StringAssert.Contains("{\"Address\":\"/\",\"Link\":\"https://tomoddy.github.io\",\"Level\":0,\"Pages\":[", actual);
            StringAssert.Contains("[{\"Address\":\"smelly\",\"Link\":\"https://tomoddy.github.io/smelly\",\"Level\":1,\"Pages\":[", actual);
            StringAssert.Contains("[{\"Address\":\"ajar\",\"Link\":\"https://tomoddy.github.io/smelly/ajar\",\"Level\":2,\"Pages\":[", actual);
            StringAssert.Contains("[{\"Address\":\"tent\",\"Link\":\"https://tomoddy.github.io/smelly/ajar/tent\",\"Level\":3,\"Pages\":[", actual);
        }

        [Test()]
        public void GetSitemapXmlTest()
        {
            StringAssert.Contains("<loc>https://tomoddy.github.io/</loc>", Crawler.GetSitemapXml());
        }
    }
}