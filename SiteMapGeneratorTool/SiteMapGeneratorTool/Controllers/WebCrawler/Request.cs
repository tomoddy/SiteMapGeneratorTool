using Newtonsoft.Json;
using System;

namespace SiteMapGeneratorTool.Controllers.WebCrawler
{
    public class Request
    {
        public Guid Guid { get; set; }
        public Uri Url { get; set; }
        public bool Files { get; set; }
        public bool Robots { get; set; }

        public Request(string url, bool files, bool robots)
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
