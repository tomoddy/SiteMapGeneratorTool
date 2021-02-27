using NUnit.Framework;
using System;

namespace SiteMapGeneratorToolSelenium.Tests
{
    [TestFixture]
    public class Layout : Base
    {
        [SetUp]
        public void LayoutSetup()
        {
            ClickById("homeLink");
        }

        [Test]
        public void NavBar()
        {
            // Check home button
            ClickById("homeLink");
            UrlEquals(string.Empty);

            // Check generate button
            ClickById("generateLink");
            UrlEquals(string.Empty);

            // Check history button
            ClickById("historyLink");
            UrlEquals("history");

            // Check privacy button
            ClickById("privacyLink");
            UrlEquals("privacy");

            // Check about button
            ClickById("aboutLink");
            UrlEquals("about");
        }

        [Test]
        public void Footer()
        {
            // Check footer text
            TextEqualById($"© {DateTime.Now.Year} - tzer0m - Privacy", "footer");

            // Check link
            Click("//*[@id=\"footer\"]/a");
            UrlEquals("privacy");
        }
    }
}
