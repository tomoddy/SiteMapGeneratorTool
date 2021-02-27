using NUnit.Framework;
using System;

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
        public void GetResult()
        {
            // Get values from table
            string url = GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[1]", 5);
            Assert.IsTrue(int.TryParse(GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[2]"), out int pages));
            Assert.IsTrue(double.TryParse(GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[3]"), out double elapsed));
            Assert.IsTrue(int.TryParse(GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[4]"), out int depth));
            Assert.IsTrue(int.TryParse(GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[5]"), out int maxPages));
            Assert.IsTrue(DateTime.TryParse(GetText($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[6]"), out DateTime completion));
            Assert.IsTrue(DateTime.Now > completion);

            // Click link
            Click($"//*[@id=\"historyTable\"]/tbody/tr[{Index}]/td[1]/a");

            // Check results
            TextEqual($"Request complete for {url}", "completeMessage", 5);
            TextContains($"{pages} pages found", "completeInformation");
            TextContains($"in {elapsed} seconds", "completeInformation");
            TextContains($"{maxPages} page limit", "completeInformation");
            TextContains($"{depth} depth limit", "completeInformation");
        }
    }
}
