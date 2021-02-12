using System.Collections.Generic;

namespace SiteMapGeneratorTool.Models
{
    public class HistoryModel
    {
        public string Domain { get; set; }
        public List<ResultsModel> Results { get; set; }
        public List<DataTableModel> Data { get
            {
                List<DataTableModel> retVal = new List<DataTableModel>();
                foreach (ResultsModel result in Results)
                    retVal.Add(new DataTableModel(result, Domain));
                return retVal;
            }
        }

        public HistoryModel(string domain, List<ResultsModel> results)
        {
            Domain = domain;
            Results = results;
        }
    }
}
