using SiteMapGeneratorTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteMapGeneratorTool.Extensions
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Sorts results using column and direction
        /// </summary>
        /// <param name="column">Sort column</param>
        /// <param name="direction">Sort direction</param>
        /// <param name="results">Results to sort</param>
        /// <returns>Sorted results</returns>
        public static List<CrawlerData> Sort(this List<CrawlerData> results, string column, string direction)
        {            
            if (direction == "asc")
                return column switch
                {
                    "domain" => results.OrderBy(x => x.Domain).ToList(),
                    "pages" => results.OrderBy(x => x.Pages).ToList(),
                    "elapsed" => results.OrderBy(x => x.Elapsed).ToList(),
                    "completion" => results.OrderBy(x => x.Completion).ToList(),
                    _ => results
                };
            else
                return column switch
                {
                    "domain" => results.OrderByDescending(x => x.Domain).ToList(),
                    "pages" => results.OrderByDescending(x => x.Pages).ToList(),
                    "elapsed" => results.OrderByDescending(x => x.Elapsed).ToList(),
                    "completion" => results.OrderByDescending(x => x.Completion).ToList(),
                    _ => results
                };
        }
    }
}
