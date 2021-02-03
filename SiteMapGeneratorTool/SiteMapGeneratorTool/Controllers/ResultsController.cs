using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SiteMapGeneratorTool.Helpers;
using System.IO;

namespace SiteMapGeneratorTool.Controllers
{
    /// <summary>
    /// Results controller
    /// </summary>
    public class ResultsController : Controller
    {
        // Constants
        private const string INVALID_RESPONSE = "Invalid GUID";

        // Variables
        private readonly IConfiguration Configuration;
        private readonly ILogger Logger;
        private readonly S3Helper S3Helper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected object</param>
        /// <param name="logger">Injected object</param>
        public ResultsController(IConfiguration configuration, ILogger<ResultsController> logger)
        {
            Configuration = configuration;
            Logger = logger;
            S3Helper = new S3Helper(Configuration.GetValue<string>("AWS:Credentials:AccessKey"), Configuration.GetValue<string>("AWS:Credentials:SecretKey"), Configuration.GetValue<string>("AWS:S3:BucketName"));
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>View</returns>
        public IActionResult Index(string guid)
        {
            ViewBag.Message = guid;
            return View("Index");
        }

        /// <summary>
        /// View sitemap file
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>Sitemap file</returns>
        public FileResult Sitemap(string guid)
        {
            Logger.LogInformation($"Returning sitemap file for {guid}");

            if (!System.IO.File.Exists($"wwwroot/sitemaps/{guid}.xml"))
                DownloadFile(guid, "sitemaps", Configuration.GetValue<string>("AWS:S3:Files:Sitemap"));

            return File($"sitemaps/{guid}.xml", "application/xml");
        }

        /// <summary>
        /// View graph file
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>Graph file</returns>
        public FileResult Graph(string guid)
        {
            Logger.LogInformation($"Returning graph file for {guid}");
            return File($"graphs/{guid}.png", "image/png");
        }

        /// <summary>
        /// Downloads file from s3
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <param name="path">Local save path</param>
        /// <param name="name">Name of file on server</param>
        private void DownloadFile(string guid, string path, string name)
        {
            FileInfo fileInfo = new FileInfo(name);
            MemoryStream memoryStream = S3Helper.DownloadResponse(guid, fileInfo);

            using FileStream fileStream = new FileStream($"wwwroot/{path}/{guid}{fileInfo.Extension}", FileMode.OpenOrCreate);
            memoryStream.CopyTo(fileStream);
            fileStream.Flush();
        }
    }
}
