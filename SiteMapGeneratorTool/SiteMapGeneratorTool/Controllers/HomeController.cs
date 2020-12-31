using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using System;
using System.Diagnostics;
using System.Threading;

namespace SiteMapGeneratorTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            SQSHelper sqsHelper = new SQSHelper(
                Configuration.GetValue<string>("AWS:Credentials:AccessKey"),
                Configuration.GetValue<string>("AWS:Credentials:SecretKey"),
                Configuration.GetValue<string>("AWS:SQS:ServiceUrl"),
                Configuration.GetValue<string>("AWS:SQS:QueueName"),
                Configuration.GetValue<string>("AWS:Credentials:AccountId"));

            sqsHelper.SendMessage(Guid.NewGuid().ToString(), new ResultsModel
            {
                Guid = "xd",
                Complete = true,
                Valid = true
            });

            object response = sqsHelper.DeleteAndReieveFirstMessage();

            return new JsonResult(response.ToString());

            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
