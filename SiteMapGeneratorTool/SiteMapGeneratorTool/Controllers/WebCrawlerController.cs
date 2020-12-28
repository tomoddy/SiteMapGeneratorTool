using Microsoft.AspNetCore.Mvc;
using SiteMapGeneratorTool.WebCrawler;

namespace SiteMapGeneratorTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebCrawlerController : ControllerBase
    {
        [HttpGet("")]
        public string Index(string url, bool files, bool robots)
        {
            Crawler crawler = new Crawler(url, files, robots);
            crawler.Configure();
            crawler.Start();
            return crawler.Stop();
        }

        [HttpGet("sitemap")]
        public string Sitemap(string url, bool files, bool robots)
        {
            Crawler crawler = new Crawler(url, files, robots);
            crawler.Configure();
            crawler.Start();
            crawler.Stop();
            return crawler.GetSitemapXml();
        }

        [HttpGet("graph")]
        public string Graph(string url, bool files, bool robots)
        {
            Crawler crawler = new Crawler(url, files, robots);
            crawler.Configure();
            crawler.Start();
            return crawler.GetGraphXml();
        }
    }
}