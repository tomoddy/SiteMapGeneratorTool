using SiteMapGeneratorTool.WebCrawler;

namespace SiteMapGeneratorTool.Models
{
    public class ResultsModel
    {
        public string Guid { get; set; }
        public Crawler Information { get; set; }

        public ResultsModel(string guid, Crawler information)
        {
            Guid = guid;
            Information = information;
        }
    }
}
