using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace SiteMapGeneratorTool.Controllers.Tests
{
    [TestFixture()]
    public class AboutControllerTests
    {
        IConfiguration Configuration;

        [SetUp]
        public void AboutControllerSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [Test()]
        public void IndexTest()
        {
            AboutController controller = new AboutController(Configuration);
            ViewResult result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}