using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace SiteMapGeneratorTool.Controllers.Tests
{
    [TestFixture()]
    public class HomeControllerTests
    {
        IConfiguration Configuration;

        [SetUp]
        public void HomeControllerSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [Test()]
        public void IndexTest()
        {
            HomeController controller = new HomeController(Configuration, null);
            ViewResult result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}