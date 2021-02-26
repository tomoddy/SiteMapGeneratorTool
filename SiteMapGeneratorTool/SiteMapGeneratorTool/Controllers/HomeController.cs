using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
            ViewBag.ApplicationServerKey = Configuration.GetValue<string>("VAPID:PublicKey");
            ViewBag.Depth = Configuration.GetValue<int>("Depth");
            ViewBag.DepthMid = Configuration.GetValue<int>("Depth") / 2;
            ViewBag.MaxPages = Configuration.GetValue<int>("MaxPages");
            ViewBag.MaxPagesMid = Configuration.GetValue<int>("MaxPages") / 2;
            return View("Index");
        }
    }
}
