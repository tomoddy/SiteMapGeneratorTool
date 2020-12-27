using Microsoft.AspNetCore.Mvc;

namespace SiteMapGeneratorTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrawlerController : ControllerBase
    {
        [HttpGet("start")]
        public string Start(string url)
        {
            return "Crawling " + url;
        }
    }
}
