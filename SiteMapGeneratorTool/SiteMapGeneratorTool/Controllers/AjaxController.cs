using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.WebCrawler;
using System.IO;

namespace SiteMapGeneratorTool.Controllers
{
    /// <summary>
    /// Ajax controoler
    /// </summary>
    public class AjaxController : Controller
    {
        // Variables
        private readonly IConfiguration Configuration;
        private readonly FirebaseHelper FirebaseHelper;
        private readonly S3Helper S3Helper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected dependency</param>
        public AjaxController(IConfiguration configuration)
        {
            Configuration = configuration;
            FirebaseHelper = new FirebaseHelper(Configuration.GetValue<string>("Firebase:BasePath"), Configuration.GetValue<string>("Firebase:AuthSecret"));
            S3Helper = new S3Helper(Configuration.GetValue<string>("AWS:Credentials:AccessKey"), Configuration.GetValue<string>("AWS:Credentials:SecretKey"), Configuration.GetValue<string>("AWS:S3:BucketName"));
        }

        /// <summary>
        /// Performs background refresh for results
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>Result or status code</returns>
        public IActionResult Results(string guid)
        {
            // Create variables
            Crawler information = FirebaseHelper.Get(guid);

            // Return results if completed otherwise processing http code
            if (information != null)
                return new JsonResult(new ResultsModel(guid, information));
            else
                return StatusCode(202);
        }
    }
}
