using System;
using System.Collections.Generic;
using System.Net;

namespace SiteMapGeneratorTool.WebCrawler.Helpers
{
    /// <summary>
    /// Robots helper
    /// </summary>
    class RobotsHelper
    {
        // Constants
        private const string ROBOTS = "robots.txt";
        private const string USER_AGENT = "User-agent: *";
        private const string DISALLOW = "Disallow: ";

        // Properties
        public List<string> Exclusions { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RobotsHelper()
        {
            Exclusions = new List<string>();
        }

        /// <summary>
        /// Gets contents of robots file
        /// </summary>
        /// <param name="domain">Base domain of website</param>
        public void FindExclusions(Uri domain)
        {
            try
            {
                using WebClient webClient = new WebClient();
                Exclusions = new List<string>(webClient.DownloadString(new Uri(domain, ROBOTS)).Split("\n"));
            }
            catch (WebException) { }
        }

        /// <summary>
        /// Gets exclusions for default agent from file
        /// </summary>
        /// <returns></returns>
        public List<string> GetExlusions()
        {
            // Create return value and create index
            List<string> retVal = new List<string>();
            int index = Exclusions.FindIndex(x => x == USER_AGENT) + 1;

            // Iterate through disallow lines
            if (index > 0)
                while (Exclusions[index].StartsWith(DISALLOW))
                {
                    // Add exlcusions to list
                    retVal.Add(Exclusions[index].Replace(DISALLOW, ""));
                    index++;
                }
            return retVal;
        }
    }
}
