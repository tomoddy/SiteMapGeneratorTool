namespace SiteMapGeneratorTool.Models
{
    public class ScreenshotRequestModel
    {
        public string Guid { get; set; }
        public string Address { get; set; }

        public ScreenshotRequestModel(string guid, string address)
        {
            Guid = guid;
            Address = address;
        }
    }
}
