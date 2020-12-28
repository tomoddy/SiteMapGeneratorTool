using System.Collections.Generic;

namespace SiteMapGeneratorTool.WebCrawler.Helpers
{
    public class FileHelper
    {
        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                { ".xml", "application/xml" },
                { ".json", "application/json" }
            };
        }
    }
}
