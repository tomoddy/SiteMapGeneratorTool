using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace SiteMapGeneratorTool.Controllers.Tests
{
    [TestFixture()]
    public class HistoryControllerTests
    {
        [Test()]
        public void IndexTest()
        {
            HistoryController controller = new HistoryController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}