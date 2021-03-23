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
            DateTime sendTime = DateTime.Now;
            emailHelper.SendEmail(Configuration.GetValue<string>("SMTP:UserName"), Configuration.GetValue<string>("Test:Domain"), guid);

            using ImapClient client = new ImapClient();
            client.Connect("outlook.office365.com", 993);
            client.Authenticate(Configuration.GetValue<string>("SMTP:UserName"), Configuration.GetValue<string>("SMTP:Password"));
            client.Inbox.Open(FolderAccess.ReadOnly);

            int count = 0;
            do
            {
                List<UniqueId> uids = new List<UniqueId>(client.Inbox.Search(SearchQuery.All));
                foreach (UniqueId uid in uids)
                {
                    MimeMessage message = client.Inbox.GetMessage(uid);
                    if (message.Date.DateTime > sendTime)
                    {
                        StringAssert.Contains(guid, message.HtmlBody);
                        client.Disconnect(true);
                        return;
                    }
                }
                count++;
                Thread.Sleep(2500);
            }
            while (count < 5);
            client.Disconnect(true);
            Assert.Fail("Email could not be found");
        }
    }
}