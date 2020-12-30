using Microsoft.AspNetCore.Mvc;
using SiteMapGeneratorTool.WebCrawler;
using SiteMapGeneratorTool.WebCrawler.Helpers;
using System.IO;
using System.Threading.Tasks;

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
            crawler.Run();
            return crawler.GetInformationJson();
        }

        [HttpGet("sitemap")]
        public string Sitemap(string url, bool files, bool robots)
        {
            Crawler crawler = new Crawler(url, files, robots);
            crawler.Configure();
            crawler.Run();
            crawler.GetInformationJson();
            return crawler.GetSitemapXml();
        }

        [HttpGet("sitemap/download")]
        public async Task<IActionResult> SitemapDownload(string url, bool files, bool robots)
        {
            Crawler crawler = new Crawler(url, files, robots);
            crawler.Configure();
            crawler.Run();
            crawler.GetInformationJson();

            FileInfo fileInfo = crawler.GetSitemapXmlFile();
            if (fileInfo.Exists)
            {
                MemoryStream memory = new MemoryStream();
                using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
                    await fileStream.CopyToAsync(memory);
                memory.Position = 0;
                return File(memory, FileHelper.GetMimeTypes()[fileInfo.Extension], fileInfo.Name);
            }
            else
                return NotFound("NO FILE FOUND");
        }

        [HttpGet("graph")]
        public string Graph(string url, bool files, bool robots)
        {
            Crawler crawler = new Crawler(url, files, robots);
            crawler.Configure();
            crawler.Run();
            return crawler.GetGraphXml();
        }
    }
}