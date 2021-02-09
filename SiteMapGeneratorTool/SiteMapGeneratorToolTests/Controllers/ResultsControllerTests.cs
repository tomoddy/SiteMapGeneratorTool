using SiteMapGeneratorTool.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using SiteMapGeneratorTool.WebCrawler.Objects;

namespace SiteMapGeneratorTool.Controllers.Tests
{
    [TestFixture()]
    public class ResultsControllerTests
    {
        IConfiguration Configuration;
        ResultsController Controller;

        [SetUp]
        public void ResultsControllerSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Controller = new ResultsController(Configuration, null);
        }

        [Test()]
        public void IndexTest()
        {
            Guid guid = new Guid();

            ViewResult result = Controller.Index(guid.ToString()) as ViewResult;
            result.ViewData.TryGetValue("Message", out object message);

            Assert.AreEqual("Index", result.ViewName);
            Assert.AreEqual(guid.ToString(), message.ToString());
        }

        [Test()]
        public void StructureTest()
        {
            ViewResult result = Controller.Structure(Configuration.GetValue<string>("Test:Guid")) as ViewResult;
            result.ViewData.TryGetValue("Message", out object message);

            Assert.AreEqual("Structure", result.ViewName);
            Assert.AreEqual("/", ((Page)message).Address);
            Assert.AreEqual(0, ((Page)message).Level);
            Assert.AreEqual("http://sitemaps.org", ((Page)message).Link);
            Assert.AreEqual(3, ((Page)message).Pages.Count);
        }

        [Test()]
        public void SitemapTest()
        {
            FileResult result = Controller.Sitemap(Configuration.GetValue<string>("Test:Guid"));

            Assert.AreEqual("application/xml", result.ContentType);
            Assert.AreEqual(false, result.EnableRangeProcessing);
            Assert.AreEqual(null, result.EntityTag);
            Assert.AreEqual(string.Empty, result.FileDownloadName);
            Assert.AreEqual(null, result.LastModified);
        }

        [Test()]
        public void GraphTest()
        {
            FileResult result = Controller.Graph(Configuration.GetValue<string>("Test:Guid"));

            Assert.AreEqual("image/png", result.ContentType);
            Assert.AreEqual(false, result.EnableRangeProcessing);
            Assert.AreEqual(null, result.EntityTag);
            Assert.AreEqual(string.Empty, result.FileDownloadName);
            Assert.AreEqual(null, result.LastModified);
        }
    }
}