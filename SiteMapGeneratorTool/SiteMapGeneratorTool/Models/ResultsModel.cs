using SiteMapGeneratorTool.WebCrawler;

namespace SiteMapGeneratorTool.Models
{
    public class ResultsModel
    {
        public string Guid { get; set; }
        public bool Valid { get; set; }
        public bool Complete { get; set; }
        public Crawler Information { get; set; }
    }
}
