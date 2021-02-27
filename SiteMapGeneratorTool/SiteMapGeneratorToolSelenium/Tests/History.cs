﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SiteMapGeneratorToolSelenium.Tests
{
    [TestFixture]
    public class History : Base
    {
        private int Index;

        [SetUp]
        public void HistorySetup()
        {
            Index = new Random().Next(1, 26);

            ClickById("historyLink");
        }

        [Test]
        public void TableValidation()
        {
            // Check titles
            TextEqualById("Domain", "domainTitle");
            TextEqualById("Pages", "pagesTitle");
            TextEqualById("Elapsed", "elapsedTitle");
            TextEqualById("Depth", "depthTitle");
            TextEqualById("Max. Pages", "maxPagesTitle");
            TextEqualById("Completion", "completionTitle");

            // Check length, filter, and count
            TextContains("Show", "//*[@id=\"historyTable_length\"]/label");
            TextContains("entries", "//*[@id=\"historyTable_length\"]/label");
            TextEqual("10", "//*[@id=\"historyTable_length\"]/label/select/option[1]");
            TextEqual("25", "//*[@id=\"historyTable_length\"]/label/select/option[2]");
            TextEqual("50", "//*[@id=\"historyTable_length\"]/label/select/option[3]");
            TextEqual("100", "//*[@id=\"historyTable_length\"]/label/select/option[4]");
            TextEqual("Search:", "//*[@id=\"historyTable_filter\"]/label");

            // Check count
            TextContains("Showing 1 to 25 of ", "//*[@id=\"historyTable_info\"]");
            Assert.AreEqual(25, FindElements("//*[@id=\"historyTable\"]/tbody/tr").Count);
        }

        [Test]
        public void SortDomain()
        {
            // Sort by domain asc
            ClickById("domainTitle");
            Thread.Sleep(WAIT);
            string url1 = GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[1]/a");
            string url2 = GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[1]/a");
            Assert.LessOrEqual(string.Compare(url1, url2), 0);

            // Sort by domain desc
            ClickById("domainTitle");
            Thread.Sleep(WAIT);
            url1 = GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[1]/a");
            url2 = GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[1]/a");
            Assert.GreaterOrEqual(string.Compare(url1, url2), 0);
        }

        [Test]
        public void SortPages()
        {

            // Sort by pages asc
            ClickById("pagesTitle");
            Thread.Sleep(WAIT);
            Assert.IsTrue(int.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[2]"), out int pages1));
            Assert.IsTrue(int.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[2]"), out int pages2));
            Assert.LessOrEqual(pages1, pages2);

            // Sort by  desc
            ClickById("pagesTitle");
            Thread.Sleep(WAIT);
            Assert.IsTrue(int.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[2]"), out pages1));
            Assert.IsTrue(int.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[2]"), out pages2));
            Assert.GreaterOrEqual(pages1, pages2);
        }

        [Test]
        public void SortElapsed()
        {
            // Sort by elapsed asc
            ClickById("elapsedTitle");
            Thread.Sleep(WAIT);
            Assert.IsTrue(double.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[3]"), out double elapsed1));
            Assert.IsTrue(double.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[3]"), out double elapsed2));
            Assert.LessOrEqual(elapsed1, elapsed2);

            // Sort by elapsed desc
            ClickById("elapsedTitle");
            Thread.Sleep(WAIT);
            Assert.IsTrue(double.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[3]"), out elapsed1));
            Assert.IsTrue(double.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[3]"), out elapsed2));
            Assert.GreaterOrEqual(elapsed1, elapsed2);
        }

        [Test]
        public void SortDepth()
        {
            // Variables
            int depth1, depth2;
            string depthText1, depthText2;

            // Sort by depth asc
            ClickById("depthTitle");
            Thread.Sleep(WAIT);

            // Get depth text
            depthText1 = GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[4]");
            depthText2 = GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[4]");

            // Set depth1 value
            if (depthText1 == "Unlimited")
                depth1 = 0;
            else
                Assert.IsTrue(int.TryParse(depthText1, out depth1));

            // Set depth 2 value
            if (depthText2 == "Unlimited")
                depth2 = 0;
            else
                Assert.IsTrue(int.TryParse(depthText2, out depth2));

            // Compare
            Assert.LessOrEqual(depth1, depth2);

            // Sort by depth desc
            ClickById("depthTitle");
            Thread.Sleep(WAIT);

            // Get depth text
            depthText1 = GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[4]");
            depthText2 = GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[4]");

            // Set depth1 value
            if (depthText1 == "Unlimited")
                depth1 = 0;
            else
                Assert.IsTrue(int.TryParse(depthText1, out depth1));

            // Set depth 2 value
            if (depthText2 == "Unlimited")
                depth2 = 0;
            else
                Assert.IsTrue(int.TryParse(depthText2, out depth2));

            // Compare
            Assert.GreaterOrEqual(depth1, depth2);
        }

        [Test]
        public void SortMaxPages()
        {
            // Variables
            int maxPages1, maxPages2;
            string maxPagesText1, maxPagesText2;

            // Sort by max pages asc
            ClickById("maxPagesTitle");
            Thread.Sleep(WAIT);

            // Get depth text
            maxPagesText1 = GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[5]");
            maxPagesText2 = GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[5]");

            // Set depth1 value
            if (maxPagesText1 == "Unlimited")
                maxPages1 = 0;
            else
                Assert.IsTrue(int.TryParse(maxPagesText1, out maxPages1));

            // Set depth 2 value
            if (maxPagesText2 == "Unlimited")
                maxPages2 = 0;
            else
                Assert.IsTrue(int.TryParse(maxPagesText2, out maxPages2));

            // Compare
            Assert.LessOrEqual(maxPages1, maxPages2);

            // Sort by max pages desc
            ClickById("maxPagesTitle");
            Thread.Sleep(WAIT);

            // Get depth text
            maxPagesText1 = GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[5]");
            maxPagesText2 = GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[5]");

            // Set depth1 value
            if (maxPagesText1 == "Unlimited")
                maxPages1 = 0;
            else
                Assert.IsTrue(int.TryParse(maxPagesText1, out maxPages1));

            // Set depth 2 value
            if (maxPagesText2 == "Unlimited")
                maxPages2 = 0;
            else
                Assert.IsTrue(int.TryParse(maxPagesText2, out maxPages2));

            // Compare
            Assert.GreaterOrEqual(maxPages1, maxPages2);
        }

        [Test]
        public void SortCompletion()
        {
            // Sort by completion asc
            ClickById("completionTitle");
            Thread.Sleep(WAIT);
            Assert.IsTrue(DateTime.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[6]"), out DateTime completion1));
            Assert.IsTrue(DateTime.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[6]"), out DateTime completion2));
            Assert.LessOrEqual(completion1, completion2);

            // Sort by completion desc
            ClickById("completionTitle");
            Thread.Sleep(WAIT);
            Assert.IsTrue(DateTime.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[6]"), out completion1));
            Assert.IsTrue(DateTime.TryParse(GetText("//*[@id=\"historyTable\"]/tbody/tr[2]/td[6]"), out completion2));
            Assert.GreaterOrEqual(completion1, completion2);
        }

        [Test]
        public void Show()
        {
            // Check entry count and get total
            Assert.AreEqual(25, FindElements("//*[@id=\"historyTable\"]/tbody/tr").Count);
            string info = GetTextById("historyTable_info");
            Assert.IsTrue(int.TryParse(info.Substring(info.IndexOf(" of ") + 4, info.Length - (info.IndexOf("entries") + 5)), out int total));

            // Test for different settings
            List<int> values = new List<int> { 10, 25, 50, 100 };
            for (int i = 0; i < values.Count; i++)
            {
                // Change entry count
                Click("//*[@id=\"historyTable_length\"]/label/select");
                Click($"//*[@id=\"historyTable_length\"]/label/select/option[{i + 1}]");
                Thread.Sleep(WAIT);

                // Check entry count
                Assert.AreEqual(values[i] <= total ? values[i] : total, FindElements("//*[@id=\"historyTable\"]/tbody/tr").Count);
            }
        }

        [Test]
        public void Search()
        {
            // Set query
            string query = GetText("//*[@id=\"historyTable\"]/tbody/tr[1]/td[1]/a");
            SendKeys("//*[@id=\"historyTable_filter\"]/label/input", query);
            Thread.Sleep(1000);

            // Check values
            for (int i = 0; i < FindElements("//*[@id=\"historyTable\"]/tbody/tr").Count; i++)
                TextEqual(query, "//*[@id=\"historyTable\"]/tbody/tr[1]/td[1]/a");

            // Set invalid query
            Clear("//*[@id=\"historyTable_filter\"]/label/input");
            SendKeys("//*[@id=\"historyTable_filter\"]/label/input", "XXX");
            Thread.Sleep(1000);

            // Check values
            TextEqual("No data available in table", "//*[@id=\"historyTable\"]/tbody/tr[1]/td[1]");
        }

        [Test]
        public void GetResult()
        {
            // Get values from table
            string url = GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[1]");
            Assert.IsTrue(int.TryParse(GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[2]"), out int pages));
            Assert.IsTrue(double.TryParse(GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[3]"), out double elapsed));
            Assert.IsTrue(int.TryParse(GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[4]"), out int depth));
            Assert.IsTrue(int.TryParse(GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[5]"), out int maxPages));
            Assert.IsTrue(DateTime.TryParse(GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[6]"), out DateTime completion));
            Assert.IsTrue(DateTime.Now > completion);

            // Click link
            Click($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[1]/a");

            // Check results
            TextEqualById($"Request complete for {url}", "completeMessage");
            TextContainsById($"{pages} pages found", "completeInformation");
            TextContainsById($"in {elapsed} seconds", "completeInformation");
            TextContainsById($"{maxPages} page limit", "completeInformation");
            TextContainsById($"{depth} depth limit", "completeInformation");
        }
    }
}