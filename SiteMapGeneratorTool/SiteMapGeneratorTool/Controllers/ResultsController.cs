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
            return View("Index");
        }

        /// <summary>
        /// View graph file
        /// </summary>
        /// <param name="guid">Guid of request</param>
        /// <returns>Graph image</returns>
        public FileResult Graph(string guid)
        {
            return File($"graphs/{guid}.png", "image/png");
        }
    }
}
