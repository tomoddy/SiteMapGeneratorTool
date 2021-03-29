using Newtonsoft.Json;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.WebCrawler.Helpers;
using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Concurrent;
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
        public Uri Domain { get; private set; }
        public DateTime Completion { get; set; }
        public int Pages { get; private set; }
        public double Elapsed { get; private set; }
        public List<Webpage> Webpages { get; private set; }
        private ConcurrentBag<Uri> Visited { get; set; }
        private List<Uri> ToVisit { get; set; }
        private Page Structure { get; set; }
        private int MaxPages { get; set; }
        private bool Files { get; set; }
        private bool Robots { get; set; }
        private int Threads { get; set; }
        private int Politeness { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="domain">Base domain of website</param>
        /// <param name="depth">Maximum search depth</param>
        /// <param name="maxPages">Maximum number of pages</param>
        /// <param name="files">Include files</param>
        /// <param name="robots">Respect robots.txt</param>
        /// <param name="threads">Number of threads</param>
        public Crawler(string domain, int depth, int maxPages, bool files, bool robots, int threads, int politeness)
        {
            RobotsHelper = new RobotsHelper();
            SitemapHelper = new SitemapHelper();
            Stopwatch = new Stopwatch();

            Domain = new Uri(domain);
            Webpages = new List<Webpage>();
            Visited = new ConcurrentBag<Uri>();
            ToVisit = new List<Uri> { Domain };
            Structure = new Page("/", 0, depth);

            MaxPages = maxPages;
            Files = files;
            Robots = robots;
            Threads = threads;
            Politeness = politeness;
        }

        /// <summary>
        /// Configures and web crawler
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

            // Start stopwatch
            Stopwatch.Start();

            // Structure counter
            int structureCount = 0;

            // Run while there are pages to visit
            while (Visited.Count <= MaxPages && ToVisit.Count > 0)
            {
                // Create temporary link store and copy of to visit list
                List<string> structureLinks = new List<string>();
                List<Uri> toVisitLocal = ToVisit;
                ToVisit = new List<Uri>();

                // Visit links in parallel
                Parallel.ForEach(toVisitLocal, new ParallelOptions() { MaxDegreeOfParallelism = Threads }, (url) =>
                {
                    // Create html helper
                    HtmlHelper htmlHelper = new HtmlHelper();

                    // Create local list of visited 
                    if (!(Visited.Any(x => x.AbsoluteUri == url.AbsoluteUri) || url.AbsoluteUri.Contains(FRAGMENT) || url.AbsoluteUri.StartsWith(TEL) || url.AbsoluteUri.StartsWith(EMAILTO)))
                    {
                        // Add link to structure
                        structureLinks.Add(url.AbsolutePath[1..]);

                        // Generate webpage object for link
                        Webpage webpage = new Webpage(url, htmlHelper.CreateDocument(url, Politeness));

                        // Generate and format tags
                        List<string> hrefs = htmlHelper.GenerateTags();
                        List<Uri> links = FormatLinks(url, hrefs);
                        links.Remove(url);

                        // Add tags to visit list and webpage and add to webpages
                        ToVisit.AddRange(links);
                        webpage.Links.AddRange(links);
                        Webpages.Add(webpage);
                    }

                    // Add current link to visited list
                    Visited.Add(url);
                });

                // Add links to structure
                foreach (string sL in structureLinks)
                {
                    if (structureCount < MaxPages)
                    {
                        Structure.Add(sL);
                        structureCount++;
                    }
                }

                // Remove nulls and duplicate to-visit links
                ToVisit.RemoveAll(x => x == null);
                ToVisit = ToVisit.GroupBy(x => x.AbsoluteUri).Select(x => x.First()).ToList();
            }
            
            // Impose maximum page limits
            Visited = new ConcurrentBag<Uri>(Visited.Take(MaxPages));
            Webpages = Webpages.Take(MaxPages).ToList();

            // Stop clock and generate links in structure
            Stopwatch.Stop();
            Structure.GenerateLink(Domain);

            // Record results
            Completion = DateTime.Now;
            Pages = Visited.Count;
            Elapsed = Stopwatch.ElapsedMilliseconds / 1000.0;
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
        /// Generates crawler data object (with firestore properties(
        /// </summary>
        /// <returns>CrawlerData object</returns>
        public CrawlerData GetCrawlerData(string guid, int depth, int maxPages)
        {
            return new CrawlerData
            {
                Guid = guid,
                Depth = depth,
                MaxPages = maxPages,
                Domain = Domain.AbsoluteUri,
                Pages = Pages,
                Elapsed = Elapsed,
                Completion = new DateTimeOffset(Completion).ToUnixTimeMilliseconds()
            };
        }

        /// <summary>
        /// Checks links are well formed and in the correct domain
        /// </summary>
        /// <param name="tags">List of string links</param>
        /// <returns>List of uri links</returns>
        private List<Uri> FormatLinks(Uri url, List<string> tags)
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
                    retVal.Add(new Uri(url, href));
                else
                    continue;
            }

            // Return ordered list of formatted links
            return retVal.GroupBy(x => x.AbsoluteUri).Select(x => x.First()).ToList();
        }
    }
}