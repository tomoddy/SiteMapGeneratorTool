using NUnit.Framework;
using System;

namespace SiteMapGeneratorToolSelenium.Tests
{
    [TestFixture]
    public class Home : Base
    {
        private int SetDepth;
        private int SetMaxPages;

        [SetUp]
        public void HomeSetup()
        {
            SetDepth = new Random().Next(Settings["Maximum Depth"]);
            SetMaxPages = new Random().Next(Settings["Maximum Pages"]);

            ClickById("generateLink");
        }

        [Test]
        public void FormValidation()
        {
            // Check labels
            CheckGenerateLabels();

            // Check initial values
            CheckGenerateDefaults();

            // Enter values
            SendKeysById("urlInput", Url);
            SendKeysById("emailInput", Email);
            MoveSlider("depthInput", SetDepth - Settings["Maximum Depth"] / 2);
            MoveSlider("maxPagesInput", SetMaxPages - Settings["Maximum Pages"] / 2);
            ClickById("filesInput");
            ClickById("robotsInput");

            // Check entered values
            ValueEqual(Url, "urlInput");
            ValueEqual(Email, "emailInput");
            ValueEqual(SetDepth, "depthInput");
            TextEqualById(SetDepth == 0 ? "Unlimited" : SetDepth.ToString(), "depthOutput");
            ValueEqual(SetMaxPages, "maxPagesInput");
            TextEqualById(SetMaxPages == 0 ? "Unlimited" : SetMaxPages.ToString(), "maxPagesOutput");
            IsSelectedById(true, "filesInput");
            IsSelectedById(true, "robotsInput");
        }

        [Test]
        public void CreateRequest()
        {
            // Send request
            SendKeysById("urlInput", Url);
            MoveSlider("depthInput", SetDepth - Settings["Maximum Depth"] / 2);
            MoveSlider("maxPagesInput", SetMaxPages - Settings["Maximum Pages"] / 2);
            if (new Random().Next(2) == 0)
                ClickById("filesInput");
            if (new Random().Next(2) == 0)
                ClickById("robotsInput");
            ClickById("submitInput");

            // Check loading page
            TextEqualById("Generating site map...", "generatingMessage");
            ElementExistsById("generatingInformation");

            // Wait for results to completed
            TextEqualById($"Request complete for {Url}/", "completeMessage", 15);
            TextContainsById("1 pages found", "completeInformation");
            TextContainsById($"{(SetMaxPages == 0 ? "unlimited" : SetMaxPages.ToString())} page limit", "completeInformation");
            TextContainsById($"{(SetDepth == 0 ? "unlimited" : SetDepth.ToString())} depth limit", "completeInformation");
            TextEqualById("Structure", "structureLink");
            TextEqualById("Sitemap", "sitemapLink");
            TextEqualById("Graph", "graphLink");
        }
    }
}
