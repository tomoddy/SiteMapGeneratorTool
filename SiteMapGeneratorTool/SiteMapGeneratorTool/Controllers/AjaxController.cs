using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.WebCrawler;
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

        /// <summary>
        /// Performans background get for history
        /// </summary>
        /// <returns>Json of history</returns>
        public JsonResult History()
        {
            // Get data
            HistoryModel history = new HistoryModel((HttpContext ?? null) is null ? Configuration.GetValue<string>("Test:Domain") : HttpContext.Request.Host.Value, FirebaseHelper.GetAll());

            // Create data table results
            List<DataTableModel> data = new List<DataTableModel>();
            foreach (ResultsModel result in history.Results)
                data.Add(new DataTableModel(result, history.Domain));

            // Get search query
            string query = Request.Form["search[value]"].FirstOrDefault();

            // Get sort column and direction
            string column = Request.Form[$"columns[{Request.Form["order[0][column]"].FirstOrDefault()}][data]"].FirstOrDefault();
            if (string.IsNullOrEmpty(column))
                column = Request.Form[$"columns[{Request.Form["order[0][column]"].FirstOrDefault()}][data][domain]"].FirstOrDefault();
            string direction = Request.Form["order[0][dir]"].FirstOrDefault();

            // Search results
            if (!string.IsNullOrEmpty(query))
                for (int i = data.Count - 1; i >= 0; i--)
                    if (!data[i].Information.Domain.AbsoluteUri.Contains(query))
                        data.RemoveAt(i);

            // Sort results
            if (!(string.IsNullOrEmpty(column) && string.IsNullOrEmpty(direction)))
                data = Sort(column, direction, data);

            // Convinience method for sorting
            static List<DataTableModel> Sort(string column, string direction, List<DataTableModel> results)
            {
                if (direction == "asc")
                    return column switch
                    {
                        "information.domain" => results.OrderBy(x => x.Information.Domain.AbsoluteUri).ToList(),
                        "information.pages" => results.OrderBy(x => x.Information.Pages).ToList(),
                        "information.elapsed" => results.OrderBy(x => x.Information.Elapsed).ToList(),
                        "information.completion" => results.OrderBy(x => x.Information.Completion).ToList(),
                        _ => results
                    };
                else
                    return column switch
                    {
                        "information.domain" => results.OrderByDescending(x => x.Information.Domain.AbsoluteUri).ToList(),
                        "information.pages" => results.OrderByDescending(x => x.Information.Pages).ToList(),
                        "information.elapsed" => results.OrderByDescending(x => x.Information.Elapsed).ToList(),
                        "information.completion" => results.OrderByDescending(x => x.Information.Completion).ToList(),
                        _ => results
                    };
            }

            // Get paging information
            string draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            int pageSize = Request.Form["length"].FirstOrDefault() != null ? Convert.ToInt32(Request.Form["length"].FirstOrDefault()) : 25;
            int skip = Request.Form["start"].FirstOrDefault() != null ? Convert.ToInt32(Request.Form["start"].FirstOrDefault()) : 0;

            // Paginate data
            int count = data.Count;
            data = data.Skip(skip).Take(pageSize).ToList();

            // Return json data
            return Json(new { draw, recordsFiltered = count, recordsTotal = count, data });
        }
    }
}
