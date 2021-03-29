using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using System;

namespace SiteMapGeneratorToolSelenium.Tests
{
    [TestFixture]
    public class Layout : Base
    {
        [SetUp]
        public void LayoutSetup()
        {
            ClickById("homeLink");
        }

        [Test]
        public void Power()
        {
            // Configure firebase helper
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            FirebaseHelper firebaseHelper = new FirebaseHelper(
                    configuration.GetValue<string>("Firebase:KeyPath"),
                    configuration.GetValue<string>("Firebase:Database"),
                    configuration.GetValue<string>("Firebase:SystemCollection"));

            // Check default value
            string id = new Uri(Domain).Host;
            Assert.AreEqual(true, firebaseHelper.Get<ConfigurationData>(id).Power);

            try
            {
                // Turn power off
                firebaseHelper.Add(id, new ConfigurationData { Power = false });
                Assert.AreEqual(false, firebaseHelper.Get<ConfigurationData>(id).Power);
                Refresh();

                // Check error message
                TextEqualById("System Offline", "offlineTitle");
                TextEqualById("The system has been disabled for maintenance, please try again later.", "offlineMessage");

                // Turn power on
                firebaseHelper.Add(id, new ConfigurationData { Power = true });
                Assert.AreEqual(true, firebaseHelper.Get<ConfigurationData>(id).Power);
                Refresh();

                // Check correct layout
                CheckGenerateLabels();
                CheckGenerateDefaults();
            }
            catch (Exception ex)
            {
                if (!firebaseHelper.Get<ConfigurationData>(id).Power)
                {
                    firebaseHelper.Add(id, new ConfigurationData { Power = true });
                    Assert.AreEqual(true, firebaseHelper.Get<ConfigurationData>(id).Power);
                    Assert.Fail("Power had to be restored.");
                }
                else
                    throw ex;
            }
        }

        [Test]
        public void NavBar()
        {
            // Check home button
            ClickById("homeLink");
            UrlEquals(string.Empty);

            // Check generate button
            ClickById("generateLink");
            UrlEquals(string.Empty);

            // Check history button
            ClickById("historyLink");
            UrlEquals("history");

            // Check privacy button
            ClickById("privacyLink");
            UrlEquals("privacy");

            // Check about button
            ClickById("aboutLink");
            UrlEquals("about");
        }

        [Test]
        public void Footer()
        {
            // Check footer text
            TextEqualById($"© {DateTime.Now.Year} - Tom Oddy - Privacy", "footer");

            // Check link
            Click("//*[@id=\"footer\"]/a");
            UrlEquals("privacy");
        }
    }
}
