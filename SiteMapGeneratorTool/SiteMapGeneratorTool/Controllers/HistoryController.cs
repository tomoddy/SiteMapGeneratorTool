using Microsoft.AspNetCore.Mvc;

namespace SiteMapGeneratorTool.Controllers
{
    /// <summary>
    /// History controller
    /// </summary>
    public class HistoryController : Controller
    {
        /// <summary>
        /// Index page
        /// </summary>
        /// <param name="order">Sort order</param>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}