using NUnit.Framework;

namespace SiteMapGeneratorToolSelenium.Tests
{
    [TestFixture]
    public class Privacy : Base
    {
        [SetUp]
        public void HistorySetup()
        {
            ClickById("privacyLink");
        }

        [Test]
        public void PrivacyPolicy()
        {
            TextEqualById("Use this page to detail your site's privacy policy.", "privacyInformation");
            TextEqualById("Privacy Policy", "privacyMessage");
        }
    }
}