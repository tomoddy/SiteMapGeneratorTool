using Newtonsoft.Json;
using System;

namespace SiteMapGeneratorTool.Models
{
    public class WebCrawlerRequestModel
    {
        public Guid Guid { get; set; }
        public string Domain { get; set; }
        public Uri Url { get; set; }
        public string Email { get; set; }
        public bool Files { get; set; }
        public bool Robots { get; set; }

        public WebCrawlerRequestModel(string domain, string url, string email, bool files, bool robots)
        {
            Guid = Guid.NewGuid();
            Domain = domain;
            Url = new Uri(url);
            Email = email;
            Files = files;
            Robots = robots;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
