using System.Collections.Generic;

namespace SiteMapGeneratorTool.Models
{
    public class JsonDataModel
    {
        public string Draw { get; set; }
        public int Count { get; set; }
        public List<CrawlerData> Data { get; set; }
    }
}
