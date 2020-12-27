using System.Xml.Serialization;

namespace SiteMapGeneratorTool.WebCrawler.Objects
{
    /// <summary>
    /// Class for xml serlialization
    /// </summary>
    public class Url
    {
        [XmlElement("loc")]
        public string Location { get; set; }
        [XmlElement("lastmod")]
        public string LastModified { get; set; }
    }
}
