using Microsoft.AspNetCore.Mvc;

namespace SiteMapGeneratorTool.Controllers
{
    public class GenerateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Results(string guid)
        {
            ViewBag.Message = guid;
            return View();
        }
    }
}
