using SiteMapGeneratorTool.WebCrawler;

namespace SiteMapGeneratorTool.Models
{
    public class DataTableModel
    {
        public string Guid { get; set; }
        public Crawler Information { get; set; }
        public string Link { get; set; }

        public DataTableModel(ResultsModel results, string domain)
        {
            Guid = results.Guid;
            Information = results.Information;
            Link = $"https://{domain}/results?guid={Guid}";
        }
    }
}
