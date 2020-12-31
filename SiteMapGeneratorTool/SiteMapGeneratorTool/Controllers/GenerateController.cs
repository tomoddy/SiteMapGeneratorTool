using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;

namespace SiteMapGeneratorTool.Controllers
{
    public class GenerateController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly FirebaseHelper FirebaseHelper;
        private readonly S3Helper S3Helper;

        public GenerateController(IConfiguration configuration)
        {
            Configuration = configuration;
            FirebaseHelper = new FirebaseHelper(Configuration.GetValue<string>("Firebase:BasePath"), Configuration.GetValue<string>("Firebase:AuthSecret"));
            S3Helper = new S3Helper(Configuration.GetValue<string>("AWS:Credentials:AccessKey"), Configuration.GetValue<string>("AWS:Credentials:SecretKey"), Configuration.GetValue<string>("AWS:S3:BucketName"));
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
                Valid = FirebaseHelper.UserExists(guid),
                Complete = S3Helper.FileExists(guid, Configuration.GetValue<string>("AWS:S3:Files:Information"))
            };
            return View();
        }
    }
}
