using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;

namespace SiteMapGeneratorTool.Controllers
{
    public class GenerateController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly S3Helper S3Helper;

        public GenerateController(IConfiguration configuration)
        {
            Configuration = configuration;
            S3Helper = new S3Helper(Configuration.GetValue<string>("S3:Credentials:AccessKey"), Configuration.GetValue<string>("S3:Credentials:SecretKey"), Configuration.GetValue<string>("S3:Credentials:BucketName"));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Results(string guid)
        {
            ViewBag.Message = new ResultsModel
            {
                Guid = guid,
                Complete = S3Helper.FileExists(guid, Configuration.GetValue<string>("S3:Files:Information"))
            };
            return View();
        }
    }
}
