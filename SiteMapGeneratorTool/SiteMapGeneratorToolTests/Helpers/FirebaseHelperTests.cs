using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteMapGeneratorTool.Helpers.Tests
{
    [TestFixture()]
    public class FirebaseHelperTests
    {
        [FirestoreData]
        private class TestData
        {
            [FirestoreProperty]
            public string Name { get; set; }
            [FirestoreProperty]
            public int Age { get; set; }
        }

        IConfiguration Configuration;

        [SetUp()]
        public void FirebaseHelperSetup()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [Test()]
        public void AddTest()
        {
            FirebaseHelper firebaseHelper = new FirebaseHelper(
                Configuration.GetValue<string>("Firebase:KeyPath"),
                Configuration.GetValue<string>("Firebase:Database"),
                "addtest");

            TestData expected = new TestData { Name = "TestName", Age = 100 };

            Guid guid = Guid.NewGuid();
            firebaseHelper.Add(guid.ToString(), expected);

            TestData actual = firebaseHelper.Get<TestData>(guid.ToString());
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Age, actual.Age);
        }

        [Test()]
        public void GetTest()
        {
            FirebaseHelper firebaseHelper = new FirebaseHelper(
                   Configuration.GetValue<string>("Firebase:KeyPath"),
                   Configuration.GetValue<string>("Firebase:Database"),
                   "gettest");

            TestData actual = firebaseHelper.Get<TestData>("test");
            Assert.AreEqual("TestName1", actual.Name);
            Assert.AreEqual(100, actual.Age);
        }

        [Test()]
        public void GetTestSort()
        {
            FirebaseHelper firebaseHelper = new FirebaseHelper(
                   Configuration.GetValue<string>("Firebase:KeyPath"),
                   Configuration.GetValue<string>("Firebase:Database"),
                   "gettest");

            List<TestData> actual = firebaseHelper.Get<TestData>("asc", "Age", string.Empty, string.Empty);
            Assert.AreEqual("TestName1", actual[0].Name);
            Assert.AreEqual("TestName2", actual[1].Name);
            Assert.AreEqual("TestName3", actual[2].Name);

            List<TestData> actual2 = firebaseHelper.Get<TestData>("desc", "Age", string.Empty, string.Empty);
            Assert.AreEqual("TestName3", actual2[0].Name);
            Assert.AreEqual("TestName2", actual2[1].Name);
            Assert.AreEqual("TestName1", actual2[2].Name);
        }

        [Test()]
        public void GetTestSearch()
        {
            FirebaseHelper firebaseHelper = new FirebaseHelper(
                   Configuration.GetValue<string>("Firebase:KeyPath"),
                   Configuration.GetValue<string>("Firebase:Database"),
                   "gettest");

            List<TestData> actual = firebaseHelper.Get<TestData>("asc", "Name", "Name", string.Empty);
            Assert.AreEqual(3, actual.Count);
            List<TestData> actual2 = firebaseHelper.Get<TestData>("asc", "Name", "Name", "TestName1");
            Assert.AreEqual(1, actual2.Count);
        }

        [Test()]
        public void GetAllTest()
        {
            FirebaseHelper firebaseHelper = new FirebaseHelper(
                   Configuration.GetValue<string>("Firebase:KeyPath"),
                   Configuration.GetValue<string>("Firebase:Database"),
                   "getalltest");

            List<TestData> actual = firebaseHelper.GetAll<TestData>();
            actual = actual.OrderBy(x => x.Age).ToList();

            Assert.AreEqual("One", actual[0].Name);
            Assert.AreEqual(1, actual[0].Age);
            Assert.AreEqual("Two", actual[1].Name);
            Assert.AreEqual(2, actual[1].Age);
            Assert.AreEqual("Three", actual[2].Name);
            Assert.AreEqual(3, actual[2].Age);
            Assert.AreEqual("Four", actual[3].Name);
            Assert.AreEqual(4, actual[3].Age);
            Assert.AreEqual("Five", actual[4].Name);
            Assert.AreEqual(5, actual[4].Age);
        }
    }
}