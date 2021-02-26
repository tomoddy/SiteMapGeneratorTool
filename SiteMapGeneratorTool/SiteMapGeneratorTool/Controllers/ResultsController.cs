using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.WebCrawler.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SiteMapGeneratorTool.Controllers
{
    /// <summary>
    /// Results controller
    /// </summary>
    public class ResultsController : Controller
    {
        // Variables
        private readonly IConfiguration Configuration;
        private readonly S3Helper S3Helper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected object</param>
        /// <param name="logger">Injected object</param>
        public ResultsController(IConfiguration configuration)
        {
            Configuration = configuration;
            S3Helper = new S3Helper(Configuration.GetValue<string>("AWS:Credentials:AccessKey"), Configuration.GetValue<string>("AWS:Credentials:SecretKey"), Configuration.GetValue<string>("AWS:S3:BucketName"));
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>View</returns>
        public ActionResult Index(string guid)
        {
            List<string> facts = System.IO.File.ReadAllLines("wwwroot/res/facts.txt").ToList();
            ViewBag.Fact = facts[new Random().Next(facts.Count)];

            ViewBag.Message = guid;
            return View("Index");
        }

        /// <summary>
        /// Structure
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>View</returns>
        public ActionResult Structure(string guid)
        {
            ViewBag.Message = S3Helper.DownloadObject<Page>(guid, new FileInfo(Configuration.GetValue<string>("AWS:S3:Files:Structure"))).Sort();
            return View("Structure");
        }

        /// <summary>
        /// View sitemap file
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>Sitemap file</returns>
        public FileResult Sitemap(string guid)
        {
            return ReturnFile(guid, "Sitemap", "application/xml");
        }

        /// <summary>
        /// View graph file
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>Graph file</returns>
        public FileResult Graph(string guid)
        {
            return ReturnFile(guid, "Graph", "image/png");
        }

        /// <summary>
        /// Downloads and returns file from s3
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <param name="file">Name of file on server</param>
        /// <param name="contentType">Content type of returned file</param>
        private FileResult ReturnFile(string guid, string file, string contentType)
        {
            return File(S3Helper.DownloadResponse(guid, new FileInfo(Configuration.GetValue<string>($"AWS:S3:Files:{file}"))).ToArray(), contentType);
        }
    }
}
