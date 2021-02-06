using SiteMapGeneratorTool.WebCrawler;
using System.Collections.Generic;

namespace SiteMapGeneratorTool.Models
{
    public class HistoryModel
    {
        public string Domain { get; set; }
        public List<ResultsModel> Results { get; set; }

        public HistoryModel(string domain, List<ResultsModel> results)
        {
            Domain = domain;
            Results = results;
        }
    }
}
