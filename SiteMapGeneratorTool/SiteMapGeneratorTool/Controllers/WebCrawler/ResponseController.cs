using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SiteMapGeneratorTool.Helpers;
using System.IO;

namespace SiteMapGeneratorTool.Controllers.WebCrawler
{
    /// <summary>
    /// Web crawler response controller
    /// </summary>
    [Route("api/webcrawler/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
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
        public ResponseController(IConfiguration configuration, ILogger<RequestController> logger)
        {
            Configuration = configuration;
            Logger = logger;
            S3Helper = new S3Helper(Configuration.GetValue<string>("S3:Credentials:AccessKey"), Configuration.GetValue<string>("S3:Credentials:SecretKey"), Configuration.GetValue<string>("S3:Credentials:BucketName"));
        }

        /// <summary>
        /// Checks if information file exists for given guid
        /// </summary>
        /// <param name="guid">GUID of file</param>
        /// <returns>True if file exists, false if doesnt</returns>
        [HttpGet("")]
        public bool Check(string guid)
        {
            return S3Helper.FileExists(guid, Configuration.GetValue<string>("S3:Files:Information"));
        }

        /// <summary>
        /// Downloads information file for given guid
        /// </summary>
        /// <param name="guid">GUID of file</param>
        /// <returns>Action result</returns>
        [HttpGet("information")]
        public ActionResult Information(string guid)
        {
            Logger.LogInformation($"Returning information file for {guid}");
            return DownloadFile(guid, Configuration.GetValue<string>("S3:Files:Information"));
        }

        /// <summary>
        /// Downloads sitemap file for given guid
        /// </summary>
        /// <param name="guid">GUID of file</param>
        /// <returns>Action result</returns>
        [HttpGet("sitemap")]
        public ActionResult Sitemap(string guid)
        {
            Logger.LogInformation($"Returning sitemap file for {guid}");
            return DownloadFile(guid, Configuration.GetValue<string>("S3:Files:Sitemap"));
        }

        /// <summary>
        /// Downloads graph file for given guid
        /// </summary>
        /// <param name="guid">GUID of file</param>
        /// <returns>Action result</returns>
        [HttpGet("graph")]
        public ActionResult Graph(string guid)
        {
            Logger.LogInformation($"Returning graph file for {guid}");
            return DownloadFile(guid, Configuration.GetValue<string>("S3:Files:Graph"));
        }

        /// <summary>
        /// Downloads file
        /// </summary>
        /// <param name="guid">GUID of file</param>
        /// <param name="name">Name of file</param>
        /// <returns>Action result</returns>
        private ActionResult DownloadFile(string guid, string name)
        {
            FileInfo fileInfo = new FileInfo(name);
            MemoryStream memoryStream = S3Helper.DownloadResponse(guid, fileInfo);
            if (memoryStream is null)
                return new JsonResult(INVALID_RESPONSE);
            else
                return File(memoryStream, FileHelper.GetMimeTypes()[fileInfo.Extension], fileInfo.Name);
        }
    }
}
