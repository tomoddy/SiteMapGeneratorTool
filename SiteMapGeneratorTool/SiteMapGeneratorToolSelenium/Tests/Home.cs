using NUnit.Framework;
using System;

namespace SiteMapGeneratorToolSelenium.Tests
{
    [TestFixture]
    public class Home : Base
    {
        [SetUp]
        public void HomeSetup()
        {
            Driver.Navigate().GoToUrl(Domain);
        }

        [Test]
        public void FormValidation()
        {
            // Set values
            int depth = new Random().Next(Configuration["Maximum Depth"]), maxPages = new Random().Next(Configuration["Maximum Pages"]);

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
            MoveSlider("depthInput", depth - Configuration["Maximum Depth"] / 2);
            MoveSlider("maxPagesInput", maxPages - Configuration["Maximum Pages"] / 2);
            Click("filesInput");
            Click("robotsInput");

            // Check entered values
            ValueEqual(Url, "urlInput");
            ValueEqual(Email, "emailInput");
            ValueEqual(depth, "depthInput");
            TextEqual(depth.ToString(), "depthOutput");
            ValueEqual(maxPages, "maxPagesInput");
            TextEqual(maxPages.ToString(), "maxPagesOutput");
            IsSelected(true, "filesInput");
            IsSelected(true, "robotsInput");
        }
    }
}
