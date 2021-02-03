using Newtonsoft.Json;
using SiteMapGeneratorTool.WebCrawler.Helpers;
using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
        private readonly HtmlHelper HtmlHelper;
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
            HtmlHelper = new HtmlHelper();
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
        public void Run()
        {
            Stopwatch.Start();
            Visit(Domain);
            Stopwatch.Stop();
            Structure.GenerateLink(Domain);
        }

        /// <summary>
        /// Returns information json string
        /// </summary>
        /// <returns>Json string</returns>
        public string GetInformationJson()
        {
            // Record results
            Completion = DateTime.Now;
            Pages = Visited.Count;
            Elapsed = Stopwatch.ElapsedMilliseconds / 1000.0;

            // Return results
            return JsonConvert.SerializeObject(this);
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
            Sitemap sitemap = SitemapHelper.GenerateSitemap(Webpages);
            return SitemapHelper.GenerateXml(sitemap);
        }

        /// <summary>
        /// Visits url
        /// </summary>
        /// <param name="url">Url to visit</param>
        private void Visit(Uri url)
        {
            // Add url to visited
            Visited.Add(url);

            // Return if URL is fragment, tel, or emailto
            if (url.AbsoluteUri.Contains(FRAGMENT) || url.AbsoluteUri.StartsWith(TEL) || url.AbsoluteUri.StartsWith(EMAILTO))
                return;

            // Add url to structure
            Structure.Add(url.AbsolutePath[1..]);

            // Create document for webpage
            Webpage newWebpage = new Webpage(url) { LastModified = HtmlHelper.CreateDocument(url) };

            // Get and format all hrefs from document
            List<string> hrefs = HtmlHelper.GenerateTags();
            List<Uri> links = FormatLinks(hrefs);
            newWebpage.AddLinks(links);

            // Iterate through all, remove if visited otherwise visit
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