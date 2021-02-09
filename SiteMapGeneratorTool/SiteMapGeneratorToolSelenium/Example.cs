using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SiteMapGeneratorToolSelenium
{
    public class Example
    {
        IWebDriver Driver;

        [SetUp]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Navigate().GoToUrl("https://localhost:5001");
        }

        [Test]
        public void ExampleTest()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            Driver.Close();
            Driver.Quit();
        }
    }
}