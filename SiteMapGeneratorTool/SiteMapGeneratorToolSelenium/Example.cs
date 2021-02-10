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
        }

        [Test]
        public void ExampleTest()
        {
            Assert.Ignore();
        }

        [TearDown]
        public void TearDown()
        {
            Driver.Close();
            Driver.Quit();
        }
    }
}