using Microsoft.AspNetCore.Mvc;
using SiteMapGeneratorTool.WebCrawler;

namespace SiteMapGeneratorTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebCrawlerController : ControllerBase
    {
        [HttpGet("start")]
        public string Start(string url, bool files, bool robots)
        {
            Crawler crawler = new Crawler(url, files, robots);
            crawler.Start();
            crawler.Stop();
            crawler.GetSitemapXml();
            return crawler.GetGraphXml();
        }
    }
}