using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SiteMapGeneratorTool.WebCrawler.Objects
{
    /// <summary>
    /// Class for xml serlialization
    /// </summary>
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class Sitemap
    {
        private readonly List<Url> ReadUrls = new List<Url>();
        [XmlElement("url")]
        public List<Url> Urls { get { return ReadUrls; } }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
