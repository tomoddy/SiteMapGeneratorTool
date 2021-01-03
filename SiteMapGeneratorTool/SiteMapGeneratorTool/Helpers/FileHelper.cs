using System.Collections.Generic;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// File helper
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// Stores dictionary of mime types
        /// </summary>
        /// <returns>Mim type dictionary</returns>
        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                { ".xml", "application/xml" },
                { ".dgml", "application/xml" },
                { ".json", "application/json" },
                { ".png", "image/png" }
            };
        }
    }
}
