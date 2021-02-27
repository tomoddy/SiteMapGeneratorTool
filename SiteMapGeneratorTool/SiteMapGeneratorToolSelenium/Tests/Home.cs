using NUnit.Framework;
using OpenQA.Selenium;
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
            Driver.Navigate().GoToUrl(Domain);

            SetDepth = new Random().Next(Configuration["Maximum Depth"]);
            SetMaxPages = new Random().Next(Configuration["Maximum Pages"]);
        }

        [Test]
        public void FormValidation()
        {
            // Check labels
            TextEqual("Website URL", "urlLabel");
            TextEqual("Email Address", "emailLabel");
            TextEqual("Maximum Subdirectory Level :", "depthLabel");
            TextEqual("Maximum Number of Pages :", "maxPagesLabel");
            TextEqual("Include Files", "filesLabel");
            TextEqual("Respect Robots", "robotsLabel");
            ValueEqual("Submit", "submitInput");

            // Check initial values
            ValueEqual(string.Empty, "urlInput");
            ValueEqual(string.Empty, "emailInput");
            ValueEqual(Configuration["Maximum Depth"] / 2, "depthInput");
            TextEqual((Configuration["Maximum Depth"] / 2).ToString(), "depthOutput");
            ValueEqual(Configuration["Maximum Pages"] / 2, "maxPagesInput");
            TextEqual((Configuration["Maximum Pages"] / 2).ToString(), "maxPagesOutput");
            IsSelected(false, "filesInput");
            IsSelected(false, "robotsInput");

            // Enter values
            SendKeys("urlInput", Url);
            SendKeys("emailInput", Email);
            MoveSlider("depthInput", SetDepth - Configuration["Maximum Depth"] / 2);
            MoveSlider("maxPagesInput", SetMaxPages - Configuration["Maximum Pages"] / 2);
            Click("filesInput");
            Click("robotsInput");

            // Check entered values
            ValueEqual(Url, "urlInput");
            ValueEqual(Email, "emailInput");
            ValueEqual(SetDepth, "depthInput");
            TextEqual(SetDepth == 0 ? "Unlimited" : SetDepth.ToString(), "depthOutput");
            ValueEqual(SetMaxPages, "maxPagesInput");
            TextEqual(SetMaxPages == 0 ? "Unlimited" : SetMaxPages.ToString(), "maxPagesOutput");
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
                Click("filesInput");
            if (new Random().Next(2) == 0)
                Click("robotsInput");
            Click("submitInput");

            // Wait for results to completed
            TextEqual($"Request complete for {Url}/", "completeMessage", 15);
            TextContains("1 pages found", "completeInformation");
            TextContains($"{SetMaxPages} page limit", "completeInformation");
            TextContains($"{SetDepth} depth limit", "completeInformation");
            TextEqual("Structure", "structureLink");
            TextEqual("Sitemap", "sitemapLink");
            TextEqual("Graph", "graphLink");
        }
    }
}
