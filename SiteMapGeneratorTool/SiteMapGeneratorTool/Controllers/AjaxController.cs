using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.WebCrawler;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SiteMapGeneratorTool.Extensions;

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
        public string Domain { get; }

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

            Domain = (HttpContext ?? null) is null ? Configuration.GetValue<string>("Test:Domain") : HttpContext.Request.Host.Value;
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
            if (data != null)
                return new JsonResult(data);
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
            string query = Request.Form["search[value]"].FirstOrDefault();

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
            List<CrawlerData> data = new HistoryModel(Domain, FirebaseHelper.GetAll<CrawlerData>()).Data;

            // Return generated json
            JsonDataModel jsonData = GenerateJson(data, query, direction, column, draw, page, skip);
            return Json(new { jsonData.Draw, recordsFiltered = jsonData.Count, recordsTotal = jsonData.Count, data = jsonData.Data });
        }

        /// <summary>
        /// Seaches, sorts, and paginates results
        /// </summary>
        /// <param name="data">Data to sort/search/paginate</param>
        /// <param name="query">Search query</param>
        /// <param name="column">Sort column</param>
        /// <param name="direction">Sort direction</param>
        /// <param name="draw">Draw context</param>
        /// <param name="page">Page size</param>
        /// <param name="skip">Number of values to skip</param>
        /// <returns>Json data model</returns>
        private JsonDataModel GenerateJson(List<CrawlerData> data, string query, string direction, string column, string draw, int page, int skip)
        {
            // Create return model
            JsonDataModel retVal = new JsonDataModel { Draw = draw };

            // Search results
            if (!string.IsNullOrEmpty(query))
                for (int i = data.Count - 1; i >= 0; i--)
                    if (!data[i].Domain.Contains(query))
                        data.RemoveAt(i);

            // Sort results
            if (!(string.IsNullOrEmpty(column) && string.IsNullOrEmpty(direction)))
                data = data.Sort(column, direction);

            // Paginate data
            retVal.Count = data.Count;
            retVal.Data = data.Skip(skip).Take(page).ToList();

            // Return json data
            return retVal;
        }
    }
}
