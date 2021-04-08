using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace SiteMapGeneratorToolSelenium.Tests
{
    [TestFixture]
    public class Results : Base
    {
        IConfiguration Configuration;

        [SetUp]
        public void ResultsSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

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
        public void GraphSmall()
        {
            string url = GetUrl();
            Navigate($"{Domain}/results/graph?guid={Configuration.GetValue<string>("Test:Guid:Small")}");

            TextEqualById("Graph", "graphTitle");
            TextEqualById("Download File", "graphFileLink");
            ElementExistsById("graphRender");

            Navigate(url);
        }

        [Test]
        public void GraphLarge()
        {
            string url = GetUrl();
            Navigate($"{Domain}/results/graph?guid={Configuration.GetValue<string>("Test:Guid:Large")}");

            TextEqualById("Graph", "graphTitle");
            TextEqualById("Download File", "graphFileLink");
            TextEqualById("Graph is too large to be rendered.", "graphError");

            Navigate(url);
        }

        [Test]
        public void GraphLink()
        {
            ClickById("graphLink");
            ClickById("graphFileLink");
            ElementExists("//pre");
            GoBack();
            GoBack();
        }
    }
}
