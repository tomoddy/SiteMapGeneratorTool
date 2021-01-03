using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace SiteMapGeneratorTool.Controllers.Tests
{
    [TestFixture()]
    public class PrivacyControllerTests
    {
        [Test()]
        public void IndexTest()
        {
            PrivacyController controller = new PrivacyController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}