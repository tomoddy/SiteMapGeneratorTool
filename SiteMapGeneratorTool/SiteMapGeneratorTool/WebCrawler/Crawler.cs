using Newtonsoft.Json;
using SiteMapGeneratorTool.WebCrawler.Helpers;
using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.WebCrawler
{
    /// <summary>
    /// Main crawler class
    /// </summary>
    public class Crawler
    {

        // Constants
        private const string FRAGMENT = "#";
        private const string TEL = "tel:";
        private const string EMAILTO = "emailto:";
        private const string EXTENSIONS = ".html,.htm,.php";

        // Variables
        private readonly RobotsHelper RobotsHelper;
        private readonly SitemapHelper SitemapHelper;
        private readonly Stopwatch Stopwatch;

        // Properties
        [JsonProperty]
        public Uri Domain { get; private set; }
        [JsonProperty]
        public DateTime Completion { get; set; }
        [JsonProperty]
        public int Pages { get; private set; }
        [JsonProperty]
        public double Elapsed { get; private set; }
        [JsonIgnore]
        public List<Webpage> Webpages { get; private set; }
        private List<Uri> Visited { get; set; }
        private Page Structure { get; set; }
        private bool Files { get; set; }
        private bool Robots { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="domain">Base domain of website</param>
        public Crawler(string domain, bool files, bool robots)
        {
            RobotsHelper = new RobotsHelper();
            SitemapHelper = new SitemapHelper();
            Stopwatch = new Stopwatch();

            Domain = new Uri(domain);
            Webpages = new List<Webpage>();
            Visited = new List<Uri>();
            Structure = new Page("/", 0);

            Files = files;
            Robots = robots;
        }

        /// <summary>
        /// Configures and runs web crawler
        /// </summary>
        public void Run()
        {
            // Check robots flag
            if (Robots)
            {
                // Get exclusions from file
                RobotsHelper.FindExclusions(Domain);

                // Add exclusions to visited list
                foreach (string exclusion in RobotsHelper.GetExlusions())
                    Visited.Add(new Uri(Domain, exclusion));
            }

            // Start stopwatch and create initial to visit list
            Stopwatch.Start();
            List<Uri> toVisit = new List<Uri> { Domain };

            // Run while there are pages to visit
            while (toVisit.Count > 0)
            {
                // Create copy of to visit list
                List<Uri> visiting = toVisit;
                toVisit = new List<Uri>();

                // Visit links in parallel
                Parallel.ForEach(visiting, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (url) =>
                {
                    // Create new html helper
                    HtmlHelper htmlHelper = new HtmlHelper();

                    // Create local list of visited
                    List<Uri> visitedLocal = Visited;
                    if (!(visitedLocal.Any(x => x.AbsoluteUri == url.AbsoluteUri) || url.AbsoluteUri.Contains(FRAGMENT) || url.AbsoluteUri.StartsWith(TEL) || url.AbsoluteUri.StartsWith(EMAILTO)))
                    {
                        // Add link to structure
                        Structure.Add(url.AbsolutePath[1..]);

                        // Generate webpage object for link
                        Webpage webpage = new Webpage(url, htmlHelper.CreateDocument(url));

                        // Generate and format tags
                        List<string> hrefs = htmlHelper.GenerateTags();
                        List<Uri> links = FormatLinks(hrefs);
                        links.Remove(url);

                        // Add tags to visit list and webpage and add to webpages
                        toVisit.AddRange(links);
                        webpage.Links.AddRange(links);
                        Webpages.Add(webpage);
                    }

                    // Add current link to visited list
                    Visited.Add(url);
                });

                // Remove duplicate toVisit links
                toVisit = toVisit.GroupBy(x => x.AbsoluteUri).Select(x => x.First()).ToList();
            }

            // Stop clock and generate links in structure
            Stopwatch.Stop();
            Structure.GenerateLink(Domain);
        }

        /// <summary>
        /// Returns information json string
        /// </summary>
        /// <returns>Json string</returns>
        public Crawler GetInformation()
        {
            // Record results
            Completion = DateTime.Now;
            Pages = Visited.Count;
            Elapsed = Stopwatch.ElapsedMilliseconds / 1000.0;

            // Return results
            return this;
        }

        /// <summary>
        /// Returns structure json string
        /// </summary>
        /// <returns>Json string</returns>
        public string GetStructureJson()
        {
            return JsonConvert.SerializeObject(Structure);
        }

        /// <summary>
        /// Generates sitemap xml
        /// </summary>
        /// <returns>Xml string</returns>
        public string GetSitemapXml()
        {
            return SitemapHelper.GenerateXml(SitemapHelper.GenerateSitemap(Webpages));
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
                if (!Files && (Path.GetExtension(href) != string.Empty && !new List<string>(EXTENSIONS.Split(",")).Contains(Path.GetExtension(href))))
                    continue;
                else if (Uri.IsWellFormedUriString(href, UriKind.Absolute) && href.StartsWith(Domain.AbsoluteUri))
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
    }
}