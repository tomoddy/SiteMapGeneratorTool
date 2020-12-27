using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiteMapGeneratorTool.WebCrawler.Helpers
{
    /// <summary>
    /// Xml helper
    /// </summary>
    class SitemapHelper
    {
        // Variables
        private readonly XmlSerializerNamespaces Namespaces;
        private readonly XmlSerializer Serlializer;
        private readonly StringWriter StringWriter;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SitemapHelper()
        {
            Namespaces = new XmlSerializerNamespaces();
            Serlializer = new XmlSerializer(typeof(Sitemap));
            StringWriter = new StringWriter();
        }

        /// <summary>
        /// Generates sitemap from webpages
        /// </summary>
        /// <param name="webpages">List of webpages</param>
        /// <returns>Sitemap object</returns>
        public Sitemap GenerateSitemap(List<Webpage> webpages)
        {
            // Create return object and sort webpages by url
            Sitemap retVal = new Sitemap();
            webpages = webpages.OrderBy(x => x.Url.AbsoluteUri).ToList();

            // Add all webpages to Sitemap and return
            foreach (Webpage webpage in webpages)
                retVal.Urls.Add(new Url { Location = webpage.Url.AbsoluteUri, LastModified = webpage.LastModified != null ? webpage.LastModified.Value.ToString("yyyy-MM-dd") : "NULL" });
            return retVal;
        }

        /// <summary>
        /// Serializes sitemap to xml
        /// </summary>
        /// <param name="sitemap">Sitemap object</param>
        /// <returns>Xml string</returns>
        public string GenerateXml(Sitemap sitemap)
        {
            Namespaces.Add("", "http://www.sitemaps.org/schemas/sitemap/0.9");
            Serlializer.Serialize(StringWriter, sitemap, Namespaces);
            return StringWriter.ToString();
        }
    }
}
