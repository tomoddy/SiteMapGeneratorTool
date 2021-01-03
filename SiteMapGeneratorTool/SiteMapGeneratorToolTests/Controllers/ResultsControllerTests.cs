using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;

namespace SiteMapGeneratorTool.Controllers.Tests
{
    [TestFixture()]
    public class ResultsControllerTests
    {
        [Test()]
        public void IndexTest()
        {
            ResultsController controller = new ResultsController();
            Guid guid = Guid.NewGuid();

            ViewResult result = controller.Index(guid.ToString()) as ViewResult;
            result.ViewData.TryGetValue("Message", out object message);

            Assert.AreEqual("Index", result.ViewName);
            Assert.AreEqual(guid.ToString(), message.ToString());
        }
    }
}