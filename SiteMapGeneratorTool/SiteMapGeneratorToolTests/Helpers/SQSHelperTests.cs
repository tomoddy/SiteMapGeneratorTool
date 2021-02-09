using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SiteMapGeneratorTool.Models;
using System;

namespace SiteMapGeneratorTool.Helpers.Tests
{
    [TestFixture()]
    public class SQSHelperTests
    {
        IConfiguration Configuration;
        SQSHelper SQSHelper;

        [OneTimeSetUp]
        public void SQSHelperOneTimeSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            SQSHelper = new SQSHelper(
                Configuration.GetValue<string>("AWS:Credentials:AccessKey"),
                Configuration.GetValue<string>("AWS:Credentials:SecretKey"),
                Configuration.GetValue<string>("AWS:SQS:ServiceUrl"),
                Configuration.GetValue<string>("AWS:SQS:TestQueueName"),
                Configuration.GetValue<string>("AWS:Credentials:AccountId"));
        }

        [Test(), Order(1)]
        public void SendMessageTest()
        {
            WebCrawlerRequestModel messageBody = new WebCrawlerRequestModel("http://example.com/", "http://example.com/", "example@example.com", true, true);
            SQSHelper.SendMessage(messageBody);
        }

        [Test(), Order(2)]
        public void DeleteAndReieveFirstMessageTest()
        {
            WebCrawlerRequestModel messageResonse = SQSHelper.DeleteAndReieveFirstMessage();
            Assert.AreEqual("http://example.com/", messageResonse.Domain);
            Assert.AreEqual(new Uri("http://example.com/"), messageResonse.Url);
            Assert.AreEqual("example@example.com", messageResonse.Email);
            Assert.AreEqual(true, messageResonse.Files);
            Assert.AreEqual(true, messageResonse.Robots);
        }
    }
}