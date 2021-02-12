using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SiteMapGeneratorTool.Models;

namespace SiteMapGeneratorTool.Controllers.Tests
{
    [TestFixture()]
    public class AjaxControllerTests
    {
        AjaxController AjaxController;
        IConfiguration Configuration;

        [SetUp]
        public void AjaxControllerSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            AjaxController = new AjaxController(Configuration);
        }

        [Test()]
        public void ResultsValidTest()
        {
            string guid = Configuration.GetValue<string>("Test:Guid");
            JsonResult result = (JsonResult)AjaxController.Results(guid);
            Assert.AreEqual(guid, ((ResultsModel)result.Value).Guid);
        }

        [Test()]
        public void ResultsInvalidTest()
        {
            string guid = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            StatusCodeResult result = (StatusCodeResult)AjaxController.Results(guid);
            Assert.AreEqual(202, result.StatusCode);
        }
    }
}