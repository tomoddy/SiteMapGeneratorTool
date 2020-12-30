using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
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
            S3Helper s3helper = new S3Helper(Configuration.GetValue<string>("S3:Credentials:AccessKey"), Configuration.GetValue<string>("S3:Credentials:SecretKey"), Configuration.GetValue<string>("S3:Credentials:BucketName"));
            s3helper.UploadFile(requestInformation.Guid.ToString(), Configuration.GetValue<string>("S3:Files:Information"), crawler.GetInformationJson());
            s3helper.UploadFile(requestInformation.Guid.ToString(), Configuration.GetValue<string>("S3:Files:Sitemap"), crawler.GetSitemapXml());
            s3helper.UploadFile(requestInformation.Guid.ToString(), Configuration.GetValue<string>("S3:Files:Graph"), crawler.GetGraphXml());

            // Return request information
            return requestInformation.ToString();
        }
    }
}
