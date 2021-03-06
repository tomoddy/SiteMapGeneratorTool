﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace SiteMapGeneratorTool.Controllers
{
    /// <summary>
    /// About controller
    /// </summary>
    public class DebugController : Controller
    {
        // Variables
        private readonly IConfiguration Configuration;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected dependancy</param>
        public DebugController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Index page
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            ViewBag.Message = new Dictionary<string, string>
            {
                { "Number of Workers", Configuration.GetValue<string>("Workers") },
                { "Timeout (ms)", Configuration.GetValue<string>("Delay") },
                { "Thread Count", Configuration.GetValue<string>("Threads") },
                { "Maximum Depth", Configuration.GetValue<string>("Depth") },
                { "Maximum Pages", Configuration.GetValue<string>("MaxPages") },
                { "Politeness Policy", Configuration.GetValue<string>("PolitenessPolicy") }
            };
            return View("Index");
        }
    }
}
