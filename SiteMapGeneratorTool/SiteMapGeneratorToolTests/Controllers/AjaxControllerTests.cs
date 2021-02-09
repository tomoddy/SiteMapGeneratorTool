using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SiteMapGeneratorTool.Models;

namespace SiteMapGeneratorTool.Controllers.Tests
{
    [TestFixture()]
    public class AjaxControllerTests
    {
        IConfiguration Configuration;

        [SetUp]
        public void Init()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [Test()]
        public void ResultsValidTest()
        {
            string guid = "7941fec7-4b16-4f5d-ae75-174e8948d747";
            AjaxController controller = new AjaxController(Configuration);
            JsonResult result = (JsonResult)controller.Results(guid);
            Assert.AreEqual(guid, ((ResultsModel)result.Value).Guid);
        }

        [Test()]
        public void ResultsInvalidTest()
        {
            string guid = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            AjaxController controller = new AjaxController(Configuration);
            StatusCodeResult result = (StatusCodeResult)controller.Results(guid);
            Assert.AreEqual(202, result.StatusCode);
        }
    }
}