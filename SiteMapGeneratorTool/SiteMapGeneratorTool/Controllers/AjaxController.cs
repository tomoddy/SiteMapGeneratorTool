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
            // Get values from form
            string draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            string start = Request.Form["start"].FirstOrDefault();
            string length = Request.Form["length"].FirstOrDefault();
            string sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();
            string sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            string searchValue = Request.Form["search[value]"].FirstOrDefault();

            // Get data
            List<Crawler> results = new HistoryModel((HttpContext ?? null) is null ? Configuration.GetValue<string>("Test:Domain") : HttpContext.Request.Host.Value, FirebaseHelper.GetAll()).Results.Select(x => x.Information).ToList();

            // Sort results
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                results = Sort(sortColumn, sortColumnDirection, results);

            // Convinience method for sorting
            static List<Crawler> Sort(string column, string direction, List<Crawler> results)
            {
                if (direction == "asc")
                    return column switch
                    {
                        "domain" => results.OrderBy(x => x.Domain.AbsoluteUri).ToList(),
                        "pages" => results.OrderBy(x => x.Pages).ToList(),
                        "elapsed" => results.OrderBy(x => x.Elapsed).ToList(),
                        "completion" => results.OrderBy(x => x.Completion).ToList(),
                        _ => results
                    };
                else
                    return column switch
                    {
                        "domain" => results.OrderByDescending(x => x.Domain.AbsoluteUri).ToList(),
                        "pages" => results.OrderByDescending(x => x.Pages).ToList(),
                        "elapsed" => results.OrderByDescending(x => x.Elapsed).ToList(),
                        "completion" => results.OrderByDescending(x => x.Completion).ToList(),
                        _ => results
                    };
            }

            // Get paging information
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            List<Crawler> data = results.Skip(skip).Take(pageSize).ToList();

            //Returning Json Data  
            return Json(new { draw, recordsFiltered = results.Count, results.Count, data });
        }
    }
}
