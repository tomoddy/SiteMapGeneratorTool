using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace SiteMapGeneratorTool.Controllers.API.Tests
{
    [TestFixture()]
    public class WebCrawlerControllerTests
    {
        IConfiguration Configuration;

        [SetUp]
        public void Init()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [Test()]
        public void IndexTest()
        {
            WebCrawlerController requestController = new WebCrawlerController(Configuration, new NullLogger<WebCrawlerController>());
            RedirectResult actual = requestController.Index("http://sitemaps.org", null, false, false) as RedirectResult;
            StringAssert.StartsWith($"https://{Configuration.GetValue<string>("TestDomain")}/results?guid=", actual.Url);
        }
    }
}