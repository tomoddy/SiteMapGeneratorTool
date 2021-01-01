using SiteMapGeneratorTool.WebCrawler;
using System.Collections.Generic;

namespace SiteMapGeneratorTool.Models
{
    public class HistoryModel
    {
        public class Entry
        {
            public string Guid { get; set; }
            public Crawler Information { get; set; }
        }

        public string Domain { get; set; }
        public List<Entry> Entries { get; set; }

        public HistoryModel(string domain)
        {
            Domain = domain;
            Entries = new List<Entry>();
        }
    }
}
