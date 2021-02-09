using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.WebCrawler;
using System;
using System.IO;
using System.Linq;

namespace SiteMapGeneratorTool.Controllers
{
    /// <summary>
    /// History controller
    /// </summary>
    public class HistoryController : Controller
    {
        // Variables
        private readonly IConfiguration Configuration;
        private readonly FirebaseHelper FirebaseHelper;
        private readonly S3Helper S3Helper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected dependancy</param>
        public HistoryController(IConfiguration configuration)
        {
            Configuration = configuration;
            FirebaseHelper = new FirebaseHelper(Configuration.GetValue<string>("Firebase:BasePath"), Configuration.GetValue<string>("Firebase:AuthSecret"));
            S3Helper = new S3Helper(Configuration.GetValue<string>("AWS:Credentials:AccessKey"), Configuration.GetValue<string>("AWS:Credentials:SecretKey"), Configuration.GetValue<string>("AWS:S3:BucketName"));
        }

        /// <summary>
        /// Index page
        /// </summary>
        /// <param name="order">Sort order</param>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            HistoryModel message = new HistoryModel((HttpContext ?? null) is null ? Configuration.GetValue<string>("Test:Domain") : HttpContext.Request.Host.Value, FirebaseHelper.GetAll());
            ViewBag.Message = message;
            return View();
        }
    }
}
