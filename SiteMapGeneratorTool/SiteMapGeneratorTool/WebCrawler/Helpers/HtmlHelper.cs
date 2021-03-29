using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace SiteMapGeneratorTool.WebCrawler.Helpers
{
    /// <summary>
    /// Html helper
    /// </summary>
    public class HtmlHelper
    {
        // Constants
        const string TAGS = "a,area,link";

        // Variables
        private readonly HtmlWeb HtmlWeb;

        // Properties
        private HtmlDocument Document { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public HtmlHelper()
        {
            HtmlWeb = new HtmlWeb();
        }

        /// <summary>
        /// Create html document from webpage
        /// </summary>
        /// <param name="url">Url of webpage</param>
        /// <returns>Last modified date of webpage</returns>
        public DateTime? CreateDocument(Uri url, int sleep)
        {
            try
            {
                // Load document and return webpage
                Document = HtmlWeb.Load(url);
                Thread.Sleep(sleep);
                return GetLastModified(url);
            }
            catch (HtmlWebException)
            {
                // Page is invalid, return null time
                return null;
            }
        }

        /// <summary>
        /// Find all hrefs from html tags
        /// </summary>
        /// <returns>List of href links</returns>
        public List<string> GenerateTags()
        {
            // Create return value and iterate through all tag types
            List<string> retVal = new List<string>();
            foreach (string tag in TAGS.Split(","))
            {
                // Get all nodes from tag
                HtmlNodeCollection nodes = Document.DocumentNode.SelectNodes($"//{tag}");
                if (nodes != null)
                    foreach (HtmlNode node in nodes)
                        retVal.Add(node.Attributes["href"]?.Value);
            }

            // Return list without nulls
            return retVal.Where(x => x != null).ToList();
        }

        /// <summary>
        /// Get last modified date of webpags
        /// </summary>
        /// <param name="url">Webpage url</param>
        /// <returns>Last modified DateTime</returns>
        public static DateTime? GetLastModified(Uri url)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                return webResponse.StatusCode == HttpStatusCode.OK ? webResponse.LastModified : new DateTime(0);
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}
