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
        public void Render(string guid, List<Webpage> pages)
        {
            // Add edges to graph
            DotGraph graph = new DotGraph(true);
            foreach (Webpage page in pages)
                foreach (Uri link in page.Links)
                    graph.Edges.Add(page.Url.AbsolutePath, link.AbsolutePath);

            // Request image and write contents to 
            File.WriteAllBytes($"wwwroot/graphs/{guid}.png", new RestClient($"https://image-charts.com/chart?cht=gv:dot&chl={graph.Build()}").Execute(new RestRequest(Method.GET)).RawBytes);
        }
    }
}
