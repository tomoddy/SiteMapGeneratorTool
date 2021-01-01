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
        public IActionResult Index(string order)
        {
            HistoryModel message = new HistoryModel(HttpContext.Request.Host.Value);
            foreach (string guid in FirebaseHelper.GetAll())
                try
                {
                    message.Entries.Add(new HistoryModel.Entry { Guid = guid, Information = S3Helper.DownloadObject<Crawler>(guid, new FileInfo(Configuration.GetValue<string>("AWS:S3:Files:Information"))) });
                }
                catch (AggregateException)
                {
                    continue;
                }

            if (order == "descending")
                message.Entries = message.Entries.OrderByDescending(x => x.Information.Completion).ToList();
            else
                message.Entries = message.Entries.OrderBy(x => x.Information.Completion).ToList();

            ViewBag.Message = message;
            return View();
        }
    }
}
