using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace SiteMapGeneratorToolSelenium
{
    [TestFixture]
    public class Base
    {
        private const int DURATION = 2;
        public const int WAIT = 500;

        public string Domain { get; set; }
        public ChromeOptions Options { get; set; }
        public IWebDriver Driver { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public Dictionary<string, int> Settings { get; set; }

        [OneTimeSetUp]
        public void BaseSetup()
        {
            Domain = "https://tadataka.azurewebsites.net/";
            Options = new ChromeOptions();
            Options.AddArgument("--disable-notifications");
            Options.AddArgument("--headless");
            Options.AddArgument("--window-size=1920,1080");
            Options.AddArgument("--no-sandbox");
            Options.AddArgument("--disable-dev-shm-usage");
            Driver = new ChromeDriver(Options);

            Url = "https://example.com";
            Email = "example@example.com";

            Settings = GetSettings();
            Driver.Navigate().GoToUrl(Domain);
        }

        [OneTimeTearDown]
        public void BaseTearDown()
        {
            Driver.Close();
            Driver.Quit();
        }

        #region General

        public IWebElement FindElement(string xPath, int duration = DURATION)
        {
            IWebElement retVal = null;
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(duration);

            try
            {
                retVal = Driver.FindElement(By.XPath(xPath));
            }
            catch (NoSuchElementException)
            {
                Assert.Fail($"Element with xPath \"{xPath}\" could not be found.");
            }

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            return retVal;
        }

        public IWebElement FindElementById(string id, int duration = DURATION)
        {
            return FindElement($"//*[@id=\"{id}\"]", duration);
        }

        public List<IWebElement> FindElements(string xPath, int duration = DURATION)
        {
            List<IWebElement> retVal = null;
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(duration);

            try
            {
                retVal = new List<IWebElement>(Driver.FindElements(By.XPath(xPath)));
            }
            catch (NoSuchElementException)
            {
                Assert.Fail($"Elements with xPath \"{xPath}\" could not be found.");
            }

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            return retVal;
        }

        public string GetUrl()
        {
            return Driver.Url;
        }

        public void GoBack()
        {
            Driver.Navigate().Back();
        }

        public void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        #endregion

        #region Collection

        public string GetText(string xPath, int duration = DURATION)
        {
            return FindElement(xPath, duration).Text;
        }

        public string GetTextById(string id, int duration = DURATION)
        {
            return FindElementById(id, duration).Text;
        }

        private T GetValue<T>(string id)
        {
            return (T)Convert.ChangeType(FindElementById(id).GetAttribute("value"), typeof(T));
        }

        #endregion

        #region Actions

        public void Click(string xPath, int duration = DURATION)
        {
            FindElement(xPath, duration).Click();
        }

        public void ClickById(string id, int duration = DURATION)
        {
            FindElementById(id, duration).Click();
        }

        public void SendKeys(string xPath, string text)
        {
            FindElement(xPath).SendKeys(text);
        }

        public void SendKeysById(string id, string text)
        {
            FindElementById(id).SendKeys(text);
        }

        public void Clear(string xPath)
        {
            FindElement(xPath).Clear();
        }

        public void MoveSlider(string id, int size)
        {
            if (size > 0)
                for (int i = 0; i < size; i++)
                    SendKeysById(id, Keys.Right);
            else if (size < 0)
                for (int i = 0; i < -size; i++)
                    SendKeysById(id, Keys.Left);
            else { }
        }

        #endregion

        #region Comparisons

        public void TextEqual(string expected, string xPath, int duration = DURATION)
        {
            Assert.AreEqual(expected, GetText(xPath, duration));
        }

        public void TextEqualById(string expected, string id, int duration = DURATION)
        {
            Assert.AreEqual(expected, GetTextById(id, duration));
        }

        public void TextContains(string expected, string xPath, int duration = DURATION)
        {
            StringAssert.Contains(expected, GetText(xPath, duration));
        }

        public void TextContainsById(string expected, string id, int duration = DURATION)
        {
            StringAssert.Contains(expected, GetTextById(id, duration));
        }

        public void ValueEqual<T>(T expected, string id)
        {
            Assert.AreEqual(expected, GetValue<T>(id));
        }

        public void IsSelectedById(bool expected, string id)
        {
            Assert.AreEqual(expected, FindElementById(id).Selected);
        }

        public bool ElementExists(string xPath)
        {
            return !(FindElement(xPath) is null);
        }

        public bool ElementExistsById(string id)
        {
            return !(FindElementById(id) is null);
        }

        #endregion

        #region Convinience

        public Dictionary<string, int> GetSettings()
        {
            Dictionary<string, int> retVal = new Dictionary<string, int>();
            Driver.Navigate().GoToUrl(Domain + "/about");
            List<IWebElement> rows = new List<IWebElement>(FindElementById("configurationTable").FindElement(By.XPath("tbody")).FindElements(By.TagName("tr")));

            foreach (IWebElement row in rows)
                retVal.Add(row.FindElement(By.XPath("td[1]")).Text, int.Parse(row.FindElement(By.XPath("td[2]")).Text));
            return retVal;
        }

        public void UrlEquals(string subDirectory)
        {
            Assert.AreEqual(Domain + subDirectory, Driver.Url);
        }

        public void CheckGenerateDefaults()
        {
            ValueEqual(string.Empty, "urlInput");
            ValueEqual(string.Empty, "emailInput");
            ValueEqual(Settings["Maximum Depth"] / 2, "depthInput");
            TextEqualById((Settings["Maximum Depth"] / 2).ToString(), "depthOutput");
            ValueEqual(Settings["Maximum Pages"] / 2, "maxPagesInput");
            TextEqualById((Settings["Maximum Pages"] / 2).ToString(), "maxPagesOutput");
            IsSelectedById(false, "filesInput");
            IsSelectedById(false, "robotsInput");
        }

        public void CheckGenerateLabels()
        {
            TextEqualById("Website URL", "urlLabel");
            TextEqualById("Email Address", "emailLabel");
            TextEqualById("Maximum Subdirectory Level :", "depthLabel");
            TextEqualById("Maximum Number of Pages :", "maxPagesLabel");
            TextEqualById("Include Files", "filesLabel");
            TextEqualById("Respect Robots", "robotsLabel");
            ValueEqual("Submit", "submitInput");
        }

        public DateTime DateTimeTryParse(string xPath)
        {
            if (DateTime.TryParse(GetText(xPath), out DateTime retVal))
                return retVal;
            else
                throw new AssertionException(GetText(xPath));
        }

        #endregion
    }
}