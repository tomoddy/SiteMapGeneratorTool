using System.Collections.Generic;

namespace SiteMapGeneratorTool.Models
{
    public class JsonDataModel
    {
        public string Draw { get; set; }
        public int Count { get; set; }
        public List<DataTableModel> Data { get; set; }
    }
}
