using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using static SiteMapGeneratorTool.WebCrawler.Objects.Graph;

namespace SiteMapGeneratorTool.WebCrawler.Helpers
{
    /// <summary>
    /// Graph helper
    /// </summary>
    class GraphHelper
    {
        /// <summary>
        /// Generates directed graph from webpages
        /// </summary>
        /// <param name="webpages">List of webpages</param>
        /// <returns>DirectedGraph object</returns>
        public static DirectedGraph GenerateGraph(List<Webpage> webpages)
        {
            // Create rerutn value and loop through all webpages
            DirectedGraph retVal = new DirectedGraph(new List<Node>(), new List<Link>());
            foreach (Webpage webpage in webpages)
            {
                // Add nodes and links
                retVal.Nodes.Add(new Node(webpage.Url.AbsolutePath, webpage.Url.AbsolutePath));
                foreach (Uri link in webpage.Links)
                    retVal.Links.Add(new Link(webpage.Url.AbsolutePath, link.AbsolutePath, string.Empty));
            }
            return retVal;
        }

        /// <summary>
        /// Serlializes graph to xml
        /// </summary>
        /// <param name="graph">DirectedGraph object</param>
        /// <returns>Xml string</returns>
        public static string GenerateXml(DirectedGraph graph)
        {
            // Configure serlialization
            XmlRootAttribute root = new XmlRootAttribute("DirectedGraph") { Namespace = "http://schemas.microsoft.com/vs/2009/dgml" };
            XmlSerializer serializer = new XmlSerializer(typeof(DirectedGraph), root);
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

            // Write xml to string and return
            using StringWriter stringWriter = new StringWriter();
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                serializer.Serialize(xmlWriter, graph);
            return stringWriter.ToString();
        }
    }
}
