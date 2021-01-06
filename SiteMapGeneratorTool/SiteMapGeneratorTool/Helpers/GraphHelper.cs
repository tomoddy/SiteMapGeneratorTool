using GiGraph.Dot.Entities.Graphs;
using GiGraph.Dot.Extensions;
using RestSharp;
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
        /// <param name="graph">Dotgraph object</param>
        public void Render(string guid, DotGraph graph)
        {
            File.WriteAllBytes($"wwwroot/graphs/{guid}.png", new RestClient($"https://image-charts.com/chart?cht=gv:dot&chl={graph.Build()}").Execute(new RestRequest(Method.GET)).RawBytes);
        }
    }
}
