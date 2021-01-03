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
        public int Level { get; set; }
        public List<Page> Pages { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="address">Domain</param>
        public Page(string address)
        {
            Address = address;
            Level = 0;
            Pages = new List<Page>();
        }

        /// <summary>
        /// Secondary constructor
        /// </summary>
        /// <param name="address">Subdirectory</param>
        /// <param name="level">Directory level</param>
        public Page(string address, int level)
        {
            Address = address;
            Level = level;
            Pages = new List<Page>();
        }

        /// <summary>
        /// Adds new page
        /// </summary>
        /// <param name="subDirectory">Subdirectory</param>
        public void Add(string subDirectory)
        {
            List<string> directory = subDirectory.Split(SEPERATOR).ToList();
            Page page = Pages.FirstOrDefault(x => x.Address == directory[0]);
            if (!(page is null))
                page.Add(string.Join(SEPERATOR, directory.GetRange(1, directory.Count - 1)));
            else
                Pages.Add(new Page(subDirectory, Level + 1));
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>String value of object</returns>
        public override string ToString()
        {
            string retVal = string.Concat(Enumerable.Repeat("    ", Level)) + Address + "\n";
            foreach (Page page in Pages)
                retVal += page.ToString();
            return retVal;
        }
    }
}
