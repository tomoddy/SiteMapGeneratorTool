using Microsoft.AspNetCore.Mvc;
using SiteMapGeneratorTool.WebCrawler;

namespace SiteMapGeneratorTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrawlerController : ControllerBase
    {
        [HttpGet("start")]
        public string Start(string url)
        {
            Crawler crawler = new Crawler(url);
            crawler.Start();
            return crawler.Stop();
        }
    }
}