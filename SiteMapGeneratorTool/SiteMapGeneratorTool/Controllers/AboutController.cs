using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SiteMapGeneratorTool.Controllers
{
    /// <summary>
    /// About controller
    /// </summary>
    public class AboutController : Controller
    {
        // Variables
        private readonly IConfiguration Configuration;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected dependancy</param>
        public AboutController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Index page
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
