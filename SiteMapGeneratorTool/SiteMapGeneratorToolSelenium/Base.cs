using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace SiteMapGeneratorToolSelenium
{
    [TestFixture]
    public class Base
    {
        public string Domain { get; set; }
        public ChromeOptions Options { get; set; }
        public IWebDriver Driver { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public Dictionary<string, int> Configuration { get; set; }

        [OneTimeSetUp]
        public void BaseSetup()
        {
            Domain = "https://tadataka.azurewebsites.net/";
            Options = new ChromeOptions();
            Options.AddArgument("--disable-notifications");
            Driver = new ChromeDriver();

            Url = "https://example.com";
            Email = "example@example.com";

            Configuration = GetConfiguration();
            Driver.Navigate().GoToUrl(Domain);
        }

        [OneTimeTearDown]
        public void BaseTearDown()
        {
            Driver.Close();
            Driver.Quit();
        }

        #region General

        public IWebElement FindElement(string xPath)
        {
            return Driver.FindElement(By.XPath(xPath));
        }

        public IWebElement FindElementById(string id)
        {
            return FindElement($"//*[@id=\"{id}\"]");
        }

        public IWebElement FindElement(string xPath, int duration)
        {
            IWebElement retVal = null;
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(duration);

            try
            {
                retVal = FindElement(xPath);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail($"Element with xPath \"{xPath}\" could not be found.");
            }

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            return retVal;
        }

        public IWebElement FindElementById(string id, int duration)
        {
            IWebElement retVal = null;
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(duration);

            try
            {
                retVal = FindElementById(id);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail($"Element with id \"{id}\" could not be found.");
            }

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            return retVal;
        }

        #endregion

        #region Collection

        public string GetText(string xPath, int duration = 0)
        {
            return FindElement(xPath, duration).Text;
        }

        public string GetTextById(string id, int duration = 0)
        {
            return FindElementById(id, duration).Text;
        }

        private T GetValue<T>(string id)
        {
            return (T)Convert.ChangeType(FindElementById(id).GetAttribute("value"), typeof(T));
        }

        #endregion

        #region Actions

        public void Click(string xPath)
        {
            FindElement(xPath).Click();
        }

        public void ClickById(string id)
        {
            FindElementById(id).Click();
        }

        public void SendKeys(string id, string text)
        {
            FindElementById(id).SendKeys(text);
        }

        public void MoveSlider(string id, int size)
        {
            if (size > 0)
                for (int i = 0; i < size; i++)
                    SendKeys(id, Keys.Right);
            else if (size < 0)
                for (int i = 0; i < -size; i++)
                    SendKeys(id, Keys.Left);
            else { }
        }

        #endregion

        #region Comparisons

        public void TextEqual(string expected, string id, int duration = 0)
        {
            Assert.AreEqual(expected, GetTextById(id, duration));
        }

        public void TextContains(string expected, string id, int duration = 0)
        {
            StringAssert.Contains(expected, GetTextById(id, duration));
        }

        public void ValueEqual<T>(T expected, string id)
        {
            Assert.AreEqual(expected, GetValue<T>(id));
        }

        public void IsSelected(bool expected, string id)
        {
            Assert.AreEqual(expected, FindElementById(id).Selected);
        }

        public bool ElementExists(string id)
        {
            return !(FindElementById(id) is null);
        }

        #endregion

        #region Convinience

        public Dictionary<string, int> GetConfiguration()
        {
            Dictionary<string, int> retVal = new Dictionary<string, int>();
            Driver.Navigate().GoToUrl(Domain + "/about");
            List<IWebElement> rows = new List<IWebElement>(FindElementById("configurationTable").FindElement(By.XPath("tbody")).FindElements(By.TagName("tr")));

            foreach (IWebElement row in rows)
                retVal.Add(row.FindElement(By.XPath("td[1]")).Text, int.Parse(row.FindElement(By.XPath("td[2]")).Text));
            return retVal;
        }

        #endregion
    }
}