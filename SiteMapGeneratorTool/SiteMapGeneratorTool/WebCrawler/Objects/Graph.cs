using System.Collections.Generic;
using System.Xml.Serialization;

namespace SiteMapGeneratorTool.WebCrawler.Objects
{
    /// <summary>
    /// Class for xml serlialization
    /// </summary>
    public class Graph
    {
        [XmlRoot]
        public struct DirectedGraph
        {
            public List<Node> Nodes;
            public List<Link> Links;

            public DirectedGraph(List<Node> nodes, List<Link> links)
            {
                Nodes = nodes;
                Links = links;
            }
        }

        public struct Node
        {
            [XmlAttribute]
            public string Id;
            [XmlAttribute]
            public string Label;

            public Node(string id, string label)
            {
                Id = id;
                Label = label;
            }
        }

        public struct Link
        {
            [XmlAttribute]
            public string Source;
            [XmlAttribute]
            public string Target;
            [XmlAttribute]
            public string Label;

            public Link(string source, string target, string label)
            {
                Source = source;
                Target = target;
                Label = label;
            }
        }
    }
}
