using Microsoft.AspNetCore.Mvc;

namespace SiteMapGeneratorTool.Controllers
{
    public class ResultsController : Controller
    {
        public IActionResult Index(string guid)
        {
            ViewBag.Message = guid;
            return View();
        }
    }
}
