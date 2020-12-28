using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.Controllers
{
    public class GenerateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
