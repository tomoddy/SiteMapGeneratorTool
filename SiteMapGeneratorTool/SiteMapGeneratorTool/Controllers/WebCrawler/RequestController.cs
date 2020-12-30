using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SiteMapGeneratorTool.Controllers.WebCrawler
{
    [Route("api/webcrawler/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public RequestController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet("")]
        public string Index(string url, bool files, bool robots)
        {
            // Create request
            Request request = new Request(url, files, robots);

            // Return request information
            return request.ToString();
        }
    }
}
