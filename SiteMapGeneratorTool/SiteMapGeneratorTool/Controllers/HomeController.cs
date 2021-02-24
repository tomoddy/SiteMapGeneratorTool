using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SiteMapGeneratorTool.Models;
using System.Diagnostics;

namespace SiteMapGeneratorTool.Controllers
{
    /// <summary>
    /// Home controoler
    /// </summary>
    public class HomeController : Controller
    {
        // Variables
        private readonly IConfiguration Configuration;
        private readonly ILogger<HomeController> Logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected dependancy</param>
        /// <param name="logger">Injected dependancy</param>
        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            ViewBag.applicationServerKey = Configuration.GetValue<string>("VAPID:PublicKey");
            return View("Index");
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <returns>View</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
