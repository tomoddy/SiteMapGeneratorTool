using GiGraph.Dot.Entities.Graphs;
using GiGraph.Dot.Extensions;
using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;

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
        /// <param name="pages">List of webpages from site</param>
        /// <returns>GV representation of graph</returns>
        public static string Render(List<Webpage> pages)
        {
            // Add edges to graph
            DotGraph graph = new DotGraph(true);
            graph.Attributes.Layout.ConcentrateEdges = true;
            foreach (Webpage page in pages)
                foreach (Uri link in page.Links)
                    graph.Edges.Add(page.Url.AbsolutePath, link.AbsolutePath);
            return graph.Build();
        }
    }
}