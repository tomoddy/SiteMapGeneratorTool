using Newtonsoft.Json;
using System;

namespace SiteMapGeneratorTool.Models
{
    public class WebCrawlerRequestModel
    {
        public Guid Guid { get; set; }
        public Uri Url { get; set; }
        public bool Files { get; set; }
        public bool Robots { get; set; }

        public WebCrawlerRequestModel(string url, bool files, bool robots)
        {
            Guid = Guid.NewGuid();
            Url = new Uri(url);
            Files = files;
            Robots = robots;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
