using Microsoft.AspNetCore.Mvc;

namespace SiteMapGeneratorTool.Controllers
{
    /// <summary>
    /// Results controller
    /// </summary>
    public class ResultsController : Controller
    {
        /// <summary>
        /// Index
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>View</returns>
        public IActionResult Index(string guid)
        {
            ViewBag.Message = guid;
            return View();
        }
    }
}
