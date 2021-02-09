using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SiteMapGeneratorTool.Helpers.Tests
{
    [TestFixture()]
    public class S3HelperTests
    {
        IConfiguration Configuration;
        S3Helper S3Helper;

        [OneTimeSetUp]
        public void S3HelperOneTimeSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            S3Helper = new S3Helper(Configuration.GetValue<string>("AWS:Credentials:AccessKey"), Configuration.GetValue<string>("AWS:Credentials:SecretKey"), Configuration.GetValue<string>("AWS:S3:BucketName"));
        }

        [Test()]
        public void UploadFileByteArrayTest()
        {
            string fileName = DateTime.Now.ToFileTimeUtc().ToString();
            S3Helper.UploadFile("testing", fileName, File.ReadAllBytes("Static/example.json"));

            MemoryStream actual = S3Helper.DownloadResponse("testing", new FileInfo(fileName));
            Assert.IsNotNull(actual);
        }

        [Test()]
        public void UploadFileStringTest()
        {
            string fileName = DateTime.Now.ToFileTimeUtc().ToString();
            S3Helper.UploadFile("testing", fileName, string.Join(string.Empty, File.ReadAllLines("Static/example.json")));

            MemoryStream actual = S3Helper.DownloadResponse("testing", new FileInfo(fileName));
            Assert.IsNotNull(actual);
        }

        [Test()]
        public void DownloadResponseTest()
        {
            string fileName = DateTime.Now.ToFileTimeUtc().ToString();
            S3Helper.UploadFile("testing", fileName, File.ReadAllBytes("Static/example.json"));

            MemoryStream actual = S3Helper.DownloadResponse("testing", new FileInfo(fileName));
            Assert.AreEqual(0, actual.Position);
            Assert.AreEqual(59, actual.Length);
            Assert.AreEqual(true, actual.CanRead);
            Assert.AreEqual(true, actual.CanSeek);
            Assert.AreEqual(false, actual.CanTimeout);
            Assert.AreEqual(true, actual.CanWrite);
            Assert.AreEqual(256, actual.Capacity);
        }

        [Test()]
        public void DownloadObjectTest()
        {
            string fileName = DateTime.Now.ToFileTimeUtc().ToString();
            S3Helper.UploadFile("testing", fileName, File.ReadAllBytes("Static/example.json"));
            Assert.AreEqual(new List<string> { "one", "two", "three", "four", "five" }, S3Helper.DownloadObject<List<string>>("testing", new FileInfo(fileName)));
        }
    }
}