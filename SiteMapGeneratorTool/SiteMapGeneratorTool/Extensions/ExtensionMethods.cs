using SiteMapGeneratorTool.Models;
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
        public static List<DataTableModel> Sort(this List<DataTableModel> results, string column, string direction)
        {
            if (direction == "asc")
                return column switch
                {
                    "information.domain" => results.OrderBy(x => x.Information.Domain.AbsoluteUri).ToList(),
                    "information.pages" => results.OrderBy(x => x.Information.Pages).ToList(),
                    "information.elapsed" => results.OrderBy(x => x.Information.Elapsed).ToList(),
                    "information.completion" => results.OrderBy(x => x.Information.Completion).ToList(),
                    _ => results
                };
            else
                return column switch
                {
                    "information.domain" => results.OrderByDescending(x => x.Information.Domain.AbsoluteUri).ToList(),
                    "information.pages" => results.OrderByDescending(x => x.Information.Pages).ToList(),
                    "information.elapsed" => results.OrderByDescending(x => x.Information.Elapsed).ToList(),
                    "information.completion" => results.OrderByDescending(x => x.Information.Completion).ToList(),
                    _ => results
                };
        }
    }
}
