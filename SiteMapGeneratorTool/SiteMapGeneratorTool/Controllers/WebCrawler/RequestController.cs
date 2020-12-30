using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.WebCrawler;

namespace SiteMapGeneratorTool.Controllers.WebCrawler
{
    /// <summary>
    /// Web crawler request controller
    /// </summary>
    [Route("api/webcrawler/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    { 
        // Variables
        private readonly IConfiguration Configuration;
        private readonly ILogger Logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected object</param>
        /// <param name="logger">Injected object</param>
        public RequestController(IConfiguration configuration, ILogger<RequestController> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        /// <summary>
        /// Creates web crawl request using url and parameters
        /// </summary>
        /// <param name="url">Url to crawl</param>
        /// <param name="files">Include files</param>
        /// <param name="robots">Respect robots</param>
        /// <returns>GUID</returns>
        [HttpGet("")]
        public IActionResult Index(string url, bool files, bool robots)
        {
            // Create request
            Logger.LogInformation($"Creating request for {url}");
            WebCrawlerRequestModel requestInformation = new WebCrawlerRequestModel(url, files, robots);

            // Run web crawler
            Logger.LogInformation($"Crawling {requestInformation}");
            Crawler crawler = new Crawler(url, files, robots);
            crawler.Configure();
            crawler.Run();

            // Upload information
            Logger.LogInformation("Uploading files");
            S3Helper s3helper = new S3Helper(Configuration.GetValue<string>("S3:Credentials:AccessKey"), Configuration.GetValue<string>("S3:Credentials:SecretKey"), Configuration.GetValue<string>("S3:Credentials:BucketName"));
            s3helper.UploadFile(requestInformation.Guid.ToString(), Configuration.GetValue<string>("S3:Files:Information"), crawler.GetInformationJson());
            s3helper.UploadFile(requestInformation.Guid.ToString(), Configuration.GetValue<string>("S3:Files:Sitemap"), crawler.GetSitemapXml());
            s3helper.UploadFile(requestInformation.Guid.ToString(), Configuration.GetValue<string>("S3:Files:Graph"), crawler.GetGraphXml());

            // Return request information
            Logger.LogInformation("Request complete");
            return Redirect($"https://{HttpContext.Request.Host}/generate/results?guid={requestInformation.Guid}");
        }
    }
}
