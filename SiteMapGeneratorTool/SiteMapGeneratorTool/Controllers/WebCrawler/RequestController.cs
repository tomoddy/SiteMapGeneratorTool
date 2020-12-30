using Microsoft.AspNetCore.Mvc;
using SiteMapGeneratorTool.S3;

namespace SiteMapGeneratorTool.Controllers.WebCrawler
{
    [Route("api/webcrawler/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        [HttpGet("")]
        public string Index(string url, bool files, bool robots)
        {
            // Create request
            Request request = new Request(url, files, robots);

            // Return request information
            return "Success";
        }
    }
}
