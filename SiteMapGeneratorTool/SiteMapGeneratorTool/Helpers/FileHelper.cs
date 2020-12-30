using System.Collections.Generic;

namespace SiteMapGeneratorTool.Helpers
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
