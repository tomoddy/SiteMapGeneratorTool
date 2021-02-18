using System.Collections.Generic;

namespace SiteMapGeneratorTool.Models
{
    public class HistoryModel
    {
        private string Domain { get; set; }
        public List<CrawlerData> Data { get; set; }

        public HistoryModel(string domain, List<CrawlerData> data)
        {
            Domain = domain;
            Data = data;

            foreach(CrawlerData crawlerData in Data)
                crawlerData.Link = $"https://{Domain}/results?guid={crawlerData.Guid}";
        }
    }
}
