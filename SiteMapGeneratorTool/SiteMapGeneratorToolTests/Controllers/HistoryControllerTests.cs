using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace SiteMapGeneratorTool.Controllers.Tests
{
    [TestFixture()]
    public class HistoryControllerTests
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
            HistoryController controller = new HistoryController(Configuration);
            ViewResult result = controller.Index() as ViewResult;
            result.ViewData.TryGetValue("Message", out object message);
        }
    }
}