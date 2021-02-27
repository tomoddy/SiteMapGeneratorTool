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
            SetDepth = new Random().Next(Configuration["Maximum Depth"]);
            SetMaxPages = new Random().Next(Configuration["Maximum Pages"]);

            ClickById("generateLink");
        }

        [Test]
        public void FormValidation()
        {
            // Check labels
            TextEqualById("Website URL", "urlLabel");
            TextEqualById("Email Address", "emailLabel");
            TextEqualById("Maximum Subdirectory Level :", "depthLabel");
            TextEqualById("Maximum Number of Pages :", "maxPagesLabel");
            TextEqualById("Include Files", "filesLabel");
            TextEqualById("Respect Robots", "robotsLabel");
            ValueEqual("Submit", "submitInput");

            // Check initial values
            ValueEqual(string.Empty, "urlInput");
            ValueEqual(string.Empty, "emailInput");
            ValueEqual(Configuration["Maximum Depth"] / 2, "depthInput");
            TextEqualById((Configuration["Maximum Depth"] / 2).ToString(), "depthOutput");
            ValueEqual(Configuration["Maximum Pages"] / 2, "maxPagesInput");
            TextEqualById((Configuration["Maximum Pages"] / 2).ToString(), "maxPagesOutput");
            IsSelected(false, "filesInput");
            IsSelected(false, "robotsInput");

            // Enter values
            SendKeys("urlInput", Url);
            SendKeys("emailInput", Email);
            MoveSlider("depthInput", SetDepth - Configuration["Maximum Depth"] / 2);
            MoveSlider("maxPagesInput", SetMaxPages - Configuration["Maximum Pages"] / 2);
            ClickById("filesInput");
            ClickById("robotsInput");

            // Check entered values
            ValueEqual(Url, "urlInput");
            ValueEqual(Email, "emailInput");
            ValueEqual(SetDepth, "depthInput");
            TextEqualById(SetDepth == 0 ? "Unlimited" : SetDepth.ToString(), "depthOutput");
            ValueEqual(SetMaxPages, "maxPagesInput");
            TextEqualById(SetMaxPages == 0 ? "Unlimited" : SetMaxPages.ToString(), "maxPagesOutput");
            IsSelected(true, "filesInput");
            IsSelected(true, "robotsInput");
        }

        [Test]
        public void CreateRequest()
        {
            // Send request
            SendKeys("urlInput", Url);
            MoveSlider("depthInput", SetDepth - Configuration["Maximum Depth"] / 2);
            MoveSlider("maxPagesInput", SetMaxPages - Configuration["Maximum Pages"] / 2);
            if (new Random().Next(2) == 0)
                ClickById("filesInput");
            if (new Random().Next(2) == 0)
                ClickById("robotsInput");
            ClickById("submitInput");

            // Check loading page
            TextEqualById("Generating site map...", "generatingMessage");
            ElementExists("generatingInformation");

            // Wait for results to completed
            TextEqualById($"Request complete for {Url}/", "completeMessage", 15);
            TextContainsById("1 pages found", "completeInformation");
            TextContainsById($"{SetMaxPages} page limit", "completeInformation");
            TextContainsById($"{SetDepth} depth limit", "completeInformation");
            TextEqualById("Structure", "structureLink");
            TextEqualById("Sitemap", "sitemapLink");
            TextEqualById("Graph", "graphLink");
        }
    }
}
