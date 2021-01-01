using Microsoft.AspNetCore.Mvc;

namespace SiteMapGeneratorTool.Controllers
{
    /// <summary>
    /// Privacy controller
    /// </summary>
    public class PrivacyController : Controller
    {
        /// <summary>
        /// Index
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
