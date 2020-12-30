using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.S3;
using SiteMapGeneratorTool.WebCrawler;

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

        [HttpPost("")]
        public string Index(string url, bool files, bool robots)
        {
            // Create request
            WebCrawlerRequestModel requestInformation = new WebCrawlerRequestModel(url, files, robots);

            // Run web crawler
            Crawler crawler = new Crawler(url, files, robots);
            crawler.Configure();
            crawler.Run();

            // Upload information
            S3Client s3Client = new S3Client(Configuration.GetValue<string>("S3:AccessKey"), Configuration.GetValue<string>("S3:SecretKey"), Configuration.GetValue<string>("S3:BucketName"));
            s3Client.UploadFile(requestInformation.Guid.ToString(), "information.json", crawler.GetInformationJson());
            s3Client.UploadFile(requestInformation.Guid.ToString(), "sitemap.xml", crawler.GetSitemapXml());
            s3Client.UploadFile(requestInformation.Guid.ToString(), "graph.dgml", crawler.GetGraphXml());

            // Return request information
            return requestInformation.ToString();
        }
    }
}
