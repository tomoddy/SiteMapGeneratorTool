using SiteMapGeneratorTool.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;

namespace SiteMapGeneratorTool.Controllers.Tests
{
    [TestFixture()]
    public class ResultsControllerTests
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
            ResultsController controller = new ResultsController(Configuration, null);
            Guid guid = Guid.NewGuid();

            ViewResult result = controller.Index(guid.ToString()) as ViewResult;
            result.ViewData.TryGetValue("Message", out object message);

            Assert.AreEqual("Index", result.ViewName);
            Assert.AreEqual(guid.ToString(), message.ToString());
        }

        [Test()]
        public void SitemapTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GraphTest()
        {
            Assert.Fail();
        }
    }
}