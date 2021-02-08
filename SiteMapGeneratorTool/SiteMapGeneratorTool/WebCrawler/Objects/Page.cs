using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteMapGeneratorTool.WebCrawler.Objects
{
    // Page object
    public class Page
    {
        // Constants
        private const string SEPERATOR = "/";

        // Properties
        public string Address { get; set; }
        public string Link { get; set; }
        public int Level { get; set; }
        public List<Page> Pages { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="address">Domain</param>
        public Page(string address, int level)
        {
            Address = address;
            Link = string.Empty;
            Level = level;
            Pages = new List<Page>();
        }

        /// <summary>
        /// Add page to pages
        /// </summary>
        /// <param name="address">Address of page</param>
        public void Add(string address)
        {
            // Split address into components
            List<string> addressComponents = address.Split(SEPERATOR).ToList();

            // Ignore if addresss  is empty
            if (addressComponents[0] != string.Empty)
            {
                // Check if page exists
                if (Pages.Find(x => x.Address == addressComponents[0]) is null)
                    Pages.Add(new Page(addressComponents[0], Level + 1));

                // Add new page to pages
                Pages.Find(x => x.Address == addressComponents[0]).Add(string.Join(SEPERATOR, addressComponents.Skip(1)));
            }
        }

        /// <summary>
        /// Populate link property for current node and all below
        /// </summary>
        /// <param name="url">Base url</param>
        public void GenerateLink(Uri url)
        {
            Link = $"{url.AbsoluteUri.Trim('/')}";
            foreach (Page page in Pages)
                page.GenerateLink(Link);
        }

        /// <summary>
        /// Populate link property for current node and all below
        /// </summary>
        /// <param name="url">Base url</param>
        /// <param name="parent">Link section of parent</param>
        public void GenerateLink(string url)
        {
            Link = $"{url}/{Address}";
            foreach (Page page in Pages)
                page.GenerateLink(Link);
        }

        /// <summary>
        /// Sort page tree into alphabetical order
        /// </summary>
        /// <returns>This page object</returns>
        public Page Sort()
        {
            Pages = Pages.OrderBy(x => x.Address).ToList();
            foreach (Page pages in Pages)
                pages.Sort();
            return this;
        }
    }
}
