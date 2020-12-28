using NUnit.Framework;
using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;
using static SiteMapGeneratorTool.WebCrawler.Objects.Graph;

namespace SiteMapGeneratorTool.WebCrawler.Helpers.Tests
{
    [TestFixture()]
    public class GraphHelperTests
    {
        private const string LINK = "http://www.teststring.com";

        private const string EXT_1 = "/link1";
        private const string EXT_2 = "/link2";
        private const string EXT_3 = "/link3";

        [Test()]
        public void GenerateGraphTest()
        {
            List<Webpage> webpages = new List<Webpage>
            {
                new Webpage(LINK + EXT_1, null, new List<string> { LINK + EXT_2, LINK + EXT_3 }),
                new Webpage(LINK + EXT_2, null, new List<string> { LINK + EXT_3, LINK + EXT_1 }),
                new Webpage(LINK + EXT_3, null, new List<string> { LINK + EXT_1, LINK + EXT_2 })
            };

            DirectedGraph expected = new DirectedGraph
            {
                Nodes = new List<Node>
                {
                    new Node(EXT_1, EXT_1),
                    new Node(EXT_2, EXT_2),
                    new Node(EXT_3, EXT_3)
                },
                Links = new List<Link>
                {
                    new Link(EXT_1, EXT_2, string.Empty),
                    new Link(EXT_1, EXT_3, string.Empty),
                    new Link(EXT_2, EXT_3, string.Empty),
                    new Link(EXT_2, EXT_1, string.Empty),
                    new Link(EXT_3, EXT_1, string.Empty),
                    new Link(EXT_3, EXT_2, string.Empty),
                }
            };

            DirectedGraph actual = GraphHelper.GenerateGraph(webpages);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test()]
        public void GenerateXmlTest()
        {
            DirectedGraph directedGraph = new DirectedGraph
            {
                Nodes = new List<Node>
                {
                    new Node(EXT_1, EXT_1),
                    new Node(EXT_2, EXT_2),
                    new Node(EXT_3, EXT_3)
                },
                Links = new List<Link>
                {
                    new Link(EXT_1, EXT_2, string.Empty),
                    new Link(EXT_1, EXT_3, string.Empty),
                    new Link(EXT_2, EXT_3, string.Empty),
                    new Link(EXT_2, EXT_1, string.Empty),
                    new Link(EXT_3, EXT_1, string.Empty),
                    new Link(EXT_3, EXT_2, string.Empty),
                }
            };

            string expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<DirectedGraph xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://schemas.microsoft.com/vs/2009/dgml\">\r\n  <Nodes>\r\n    <Node Id=\"/link1\" Label=\"/link1\" />\r\n    <Node Id=\"/link2\" Label=\"/link2\" />\r\n    <Node Id=\"/link3\" Label=\"/link3\" />\r\n  </Nodes>\r\n  <Links>\r\n    <Link Source=\"/link1\" Target=\"/link2\" Label=\"\" />\r\n    <Link Source=\"/link1\" Target=\"/link3\" Label=\"\" />\r\n    <Link Source=\"/link2\" Target=\"/link3\" Label=\"\" />\r\n    <Link Source=\"/link2\" Target=\"/link1\" Label=\"\" />\r\n    <Link Source=\"/link3\" Target=\"/link1\" Label=\"\" />\r\n    <Link Source=\"/link3\" Target=\"/link2\" Label=\"\" />\r\n  </Links>\r\n</DirectedGraph>";
            string actual = GraphHelper.GenerateXml(directedGraph);
            Assert.AreEqual(expected, actual);
        }
    }
}