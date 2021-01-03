using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace SiteMapGeneratorTool.Controllers.WebCrawler.Tests
{
    [TestFixture()]
    public class RequestControllerTests
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
            RequestController requestController = new RequestController(Configuration, new NullLogger<RequestController>());
            requestController.Index("", "", false, false);
        }
    }
}