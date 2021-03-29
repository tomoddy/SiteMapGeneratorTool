using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SiteMapGeneratorTool.Helpers.Tests
{
    [TestFixture()]
    public class EmailHelperTests
    {
        IConfiguration Configuration;

        [SetUp()]
        public void FirebaseHelperSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [Test()]
        public void SendEmailTest()
        {
            EmailHelper emailHelper = new EmailHelper(
                Configuration.GetValue<string>("SMTP:UserName"),
                Configuration.GetValue<string>("SMTP:DisplayName"),
                Configuration.GetValue<string>("SMTP:Password"),
                Configuration.GetValue<string>("SMTP:Host"),
                Configuration.GetValue<string>("SMTP:Port"));

            string guid = Guid.NewGuid().ToString();
            emailHelper.SendEmail(Configuration.GetValue<string>("SMTP:UserName"), Configuration.GetValue<string>("Test:Domain"), guid);
        }
    }
}