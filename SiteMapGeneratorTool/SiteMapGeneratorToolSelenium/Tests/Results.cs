using NUnit.Framework;

namespace SiteMapGeneratorToolSelenium.Tests
{
    [TestFixture]
    public class Results : Base
    {
        [SetUp]
        public void ResultsSetup()
        {
            ClickById("historyLink");
            Url = GetText($"//*[@id=\"historyTable\"]/tbody/tr[1]/td[1]");
            Click($"//*[@id=\"historyTable\"]/tbody/tr[1]/td[1]/a");
        }

        [Test]
        public void Structure()
        {
            ClickById("structureLink");
            ElementExistsById("structureLink");
            ElementExistsById("structureList");
        }

        [Test]
        public void Sitemap()
        {
            ClickById("sitemapLink");
            ElementExistsById("webkit-xml-viewer-source-xml");
            GoBack();
        }

        [Test]
        public void Graph()
        {
            ClickById("graphLink");
            ElementExists($"//img[@src=\"{GetUrl()}\"]");
            GoBack();
        }
    }
}
