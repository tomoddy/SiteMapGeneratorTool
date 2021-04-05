using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Extensions;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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

        // Properties
        public string SearchField { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected dependency</param>
        public AjaxController(IConfiguration configuration)
        {
            Configuration = configuration;
            FirebaseHelper = new FirebaseHelper(
                Configuration.GetValue<string>("Firebase:KeyPath"),
                Configuration.GetValue<string>("Firebase:Database"),
                Configuration.GetValue<string>("Firebase:RequestCollection"));
            S3Helper = new S3Helper(
                Configuration.GetValue<string>("AWS:Credentials:AccessKey"),
                Configuration.GetValue<string>("AWS:Credentials:SecretKey"),
                Configuration.GetValue<string>("AWS:S3:BucketName"));
            SearchField = Configuration.GetValue<string>("Firebase:SearchField");
        }

        /// <summary>
        /// Performs background refresh for results
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>Result or status code</returns>
        public IActionResult Results(string guid)
        {
            // Create variables
            CrawlerData data = FirebaseHelper.Get<CrawlerData>(guid);

            // Return results if completed otherwise processing http code
            if (data == null)
                return StatusCode(404);
            else if (data.Completion > 0)
                return new JsonResult(data);
            else if (data.Guid == "FAILURE")
                return StatusCode(500, data.Message);
            else
                return StatusCode(202);
        }

        /// <summary>
        /// Performans background get for history
        /// </summary>
        /// <returns>Json of history</returns>
        public JsonResult History()
        {
            // Get search query
            string search = Request.Form["search[value]"].FirstOrDefault();

            // Get sort column and direction
            string direction = Request.Form["order[0][dir]"].FirstOrDefault();
            string column = Request.Form[$"columns[{Request.Form["order[0][column]"].FirstOrDefault()}][data]"].FirstOrDefault();
            if (string.IsNullOrEmpty(column))
                column = Request.Form[$"columns[{Request.Form["order[0][column]"].FirstOrDefault()}][data][domain]"].FirstOrDefault();

            // Get paging information
            string draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            int page = Request.Form["length"].FirstOrDefault() != null ? Convert.ToInt32(Request.Form["length"].FirstOrDefault()) : 25;
            int skip = Request.Form["start"].FirstOrDefault() != null ? Convert.ToInt32(Request.Form["start"].FirstOrDefault()) : 0;

            // Get data table results
            List<CrawlerData> data = FirebaseHelper.Get<CrawlerData>(direction, column, SearchField, search);

            // Add links to data
            string domain = (HttpContext ?? null) is null ? Configuration.GetValue<string>("Test:Domain") : HttpContext.Request.Host.Value;
            foreach (CrawlerData crawlerData in data)
                crawlerData.Link = $"https://{domain}/results?guid={crawlerData.Guid}";

            // Server side sort if search is used
            if (!string.IsNullOrEmpty(search))
                data = data.Sort(column, direction);

            // Paginate data
            int count = data.Count;
            data = data.Skip(skip).Take(page).ToList();

            // Return json
            return Json(new { draw, recordsFiltered = count, recordsTotal = count, data });
        }
    }
}
