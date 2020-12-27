using Newtonsoft.Json;
using SiteMapGeneratorTool.WebCrawler.Helpers;
using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static SiteMapGeneratorTool.WebCrawler.Objects.Graph;

namespace SiteMapGeneratorTool.WebCrawler
{
    /// <summary>
    /// Main crawler class
    /// </summary>
    class Crawler
    {
        // Constants
        private const string FRAGMENT = "#";
        private const string TEL = "tel:";
        private const string EMAILTO = "emailto:";
        private const string EXTENSIONS = ".html,.htm,.php";

        // Variables
        private readonly HtmlHelper HtmlHelper;
        private readonly RobotsHelper RobotsHelper;
        private readonly SitemapHelper SitemapHelper;
        private readonly Stopwatch Stopwatch;

        // Properties
        [JsonProperty]
        private Uri Domain { get; set; }
        [JsonProperty]
        private int Pages { get; set; }
        [JsonProperty]
        private double Elapsed { get; set; }
        [JsonProperty]
        public List<Webpage> Webpages { get; private set; }
        private List<Uri> Visited { get; set; }
        private bool Files { get; set; }
        private bool Robots { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="domain">Base domain of website</param>
        public Crawler(string domain, bool files, bool robots)
        {
            HtmlHelper = new HtmlHelper();
            RobotsHelper = new RobotsHelper();
            SitemapHelper = new SitemapHelper();
            Stopwatch = new Stopwatch();

            Domain = new Uri(domain);
            Webpages = new List<Webpage>();
            Visited = new List<Uri>();

            Files = files;
            Robots = robots;
        }

        /// <summary>
        /// Configures web crawler before it is started
        /// </summary>
        /// <param name="robots">Boolean for if robots exclusion is enabled</param>
        public void Configure()
        {
            if (Robots)
            {
                // Get exclusions from file
                RobotsHelper.FindExclusions(Domain);

                // Add exclusions to visited list
                foreach (string exclusion in RobotsHelper.GetExlusions())
                    Visited.Add(new Uri(Domain, exclusion));
            }
        }

        /// <summary>
        /// Starts web crawler
        /// </summary>
        public void Start()
        {
            Stopwatch.Start();
            Visit(Domain);
            Stopwatch.Stop();
        }

        /// <summary>
        /// Stops web crawler
        /// </summary>
        /// <returns>Json string</returns>
        public string Stop()
        {
            // Record results
            Pages = Visited.Count;
            Elapsed = Stopwatch.ElapsedMilliseconds / 1000.0;

            // Return results
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Generates sitemap xml
        /// </summary>
        /// <returns>Xml string</returns>
        public string GetSitemapXml()
        {
            Sitemap sitemap = SitemapHelper.GenerateSitemap(Webpages);
            return SitemapHelper.GenerateXml(sitemap);
        }

        /// <summary>
        /// Generates graph xml
        /// </summary>
        /// <returns>Xml string</returns>
        public string GetGraphXml()
        {
            DirectedGraph directedGraph = GraphHelper.GenerateGraph(Webpages);
            return GraphHelper.GenerateXml(directedGraph);
        }

        /// <summary>
        /// Visits url
        /// </summary>
        /// <param name="url">Url to visit</param>
        private void Visit(Uri url)
        {
            // Add url to visited and return if URL is fragment, tel, or emailto
            Visited.Add(url);
            if (url.AbsoluteUri.Contains(FRAGMENT) || url.AbsoluteUri.StartsWith(TEL) || url.AbsoluteUri.StartsWith(EMAILTO))
                return;

            // Remove unwanted files
            if (!Files)
            {
                string extension = Path.GetExtension(url.AbsoluteUri);
                if (extension == string.Empty || new List<string>(EXTENSIONS.Split(",")).Contains(extension)) { }
                else
                    return;
            }

            // Create document for webpage
            Webpage newWebpage = new Webpage(url) { LastModified = HtmlHelper.CreateDocument(url) };

            // Get and format all hrefs from document
            List<string> hrefs = HtmlHelper.GenerateTags();
            List<Uri> links = FormatLinks(hrefs);
            newWebpage.AddLinks(links);

            // Iterate through all, remove if visited otherwise vist
            while (links.Count > 0)
            {
                if (IsVisited(links.First()))
                    links.RemoveAt(0);
                else
                    Visit(links.First());
            }

            // Add completed webpage
            Webpages.Add(newWebpage);
        }

        /// <summary>
        /// Checks links are well formed and in the correct domain
        /// </summary>
        /// <param name="tags">List of string links</param>
        /// <returns>List of uri links</returns>
        private List<Uri> FormatLinks(List<string> tags)
        {
            // Return value
            List<Uri> retVal = new List<Uri>();

            // Iterate through all hrefs and check if links is valid
            foreach (string href in tags)
            {
                if (Uri.IsWellFormedUriString(href, UriKind.Absolute) && href.StartsWith(Domain.AbsoluteUri))
                    retVal.Add(new Uri(href));
                else if (Uri.IsWellFormedUriString(href, UriKind.Absolute))
                    continue;
                else if (Uri.IsWellFormedUriString(new Uri(Domain, href).AbsoluteUri, UriKind.Absolute))
                    retVal.Add(new Uri(Domain, href));
                else
                    continue;
            }

            // Return ordered list of formatted links
            return retVal.GroupBy(x => x.AbsoluteUri).Select(x => x.First()).ToList();
        }

        /// <summary>
        /// Checks if website has already been visited
        /// </summary>
        /// <param name="link">Url of webpage to check</param>
        /// <returns>Boolean for if visited</returns>
        private bool IsVisited(Uri link)
        {
            foreach (Uri visited in Visited)
                if (link.AbsoluteUri == visited.AbsoluteUri)
                    return true;
            return false;
        }
    }
}