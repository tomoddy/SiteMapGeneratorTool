using GiGraph.Dot.Entities.Graphs;
using GiGraph.Dot.Extensions;
using RestSharp;
using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// Graph helper
    /// </summary>
    public class GraphHelper
    {
        /// <summary>
        /// Renders dotgraph object to png image
        /// </summary>
        /// <param name="guid">GUID of request</param>
        /// <param name="pages">List of webpages from site</param>
        public byte[] Render(List<Webpage> pages)
        {
            // Add edges to graph
            DotGraph graph = new DotGraph(true);
            graph.Attributes.Layout.ConcentrateEdges = true;
            foreach (Webpage page in pages)
                foreach (Uri link in page.Links)
                    graph.Edges.Add(page.Url.AbsolutePath, link.AbsolutePath);

            // Request image and write contents to byte array
            if (graph.Nodes.Count <= 200 && graph.Edges.Count <= 400)
                return new RestClient($"https://image-charts.com/chart?cht=gv:dot&chl={graph.Build()}").Execute(new RestRequest(Method.GET)).RawBytes;
            else
                return File.ReadAllBytes("wwwroot/empty-graph.png");
        }
    }
}
