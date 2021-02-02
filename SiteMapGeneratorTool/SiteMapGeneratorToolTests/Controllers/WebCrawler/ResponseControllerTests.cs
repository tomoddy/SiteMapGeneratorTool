using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace SiteMapGeneratorTool.Controllers.WebCrawler.Tests
{
    [TestFixture()]
    public class ResponseControllerTests
    {
        IConfiguration Configuration;

        [SetUp]
        public void Init()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [TestCase("7c785e83-1b47-420e-940b-500c3ef20e43", ExpectedResult = true)]
        [TestCase("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", ExpectedResult = false)]
        public bool CheckTest(string guid)
        {
            ResponseController requestController = new ResponseController(Configuration, new NullLogger<RequestController>());
            return requestController.Check(guid);
        }

        [Test()]
        public void InformationTest()
        {
            ResponseController requestController = new ResponseController(Configuration, new NullLogger<RequestController>());
            FileResult file = (FileResult)requestController.Information("7c785e83-1b47-420e-940b-500c3ef20e43");
            Assert.AreEqual("application/json", file.ContentType);
            Assert.AreEqual(Configuration.GetValue<string>("AWS:S3:Files:Information"), file.FileDownloadName);
        }

        [Test()]
        public void SitemapTest()
        {
            ResponseController requestController = new ResponseController(Configuration, new NullLogger<RequestController>());
            FileResult file = (FileResult)requestController.Sitemap("7c785e83-1b47-420e-940b-500c3ef20e43");
            Assert.AreEqual("application/xml", file.ContentType);
            Assert.AreEqual(Configuration.GetValue<string>("AWS:S3:Files:Sitemap"), file.FileDownloadName);
        }
    }
}