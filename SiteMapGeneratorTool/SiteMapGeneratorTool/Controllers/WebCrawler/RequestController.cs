using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using System;

namespace SiteMapGeneratorTool.Controllers.WebCrawler
{
    /// <summary>
    /// Web crawler request controller
    /// </summary>
    [Route("api/webcrawler/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        // Variables
        private readonly IConfiguration Configuration;
        private readonly FirebaseHelper FirebaseHelper;
        private readonly ILogger Logger;
        private readonly SQSHelper SQSHelper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected object</param>
        /// <param name="logger">Injected object</param>
        public RequestController(IConfiguration configuration, ILogger<RequestController> logger)
        {
            Configuration = configuration;
            FirebaseHelper = new FirebaseHelper(Configuration.GetValue<string>("Firebase:BasePath"), Configuration.GetValue<string>("Firebase:AuthSecret"));
            Logger = logger;
            SQSHelper = new SQSHelper(
                Configuration.GetValue<string>("AWS:Credentials:AccessKey"),
                Configuration.GetValue<string>("AWS:Credentials:SecretKey"),
                Configuration.GetValue<string>("AWS:SQS:ServiceUrl"),
                Configuration.GetValue<string>("AWS:SQS:QueueName"),
                Configuration.GetValue<string>("AWS:Credentials:AccountId"));
        }

        /// <summary>
        /// Creates web crawl request using url and parameters
        /// </summary>
        /// <param name="url">Url to crawl</param>
        /// <param name="files">Include files</param>
        /// <param name="robots">Respect robots</param>
        /// <returns>GUID</returns>
        [HttpGet("")]
        public IActionResult Index(string url, string email, bool files, bool robots)
        {
            // Create request
            Logger.LogInformation($"Creating request for {url}");
            WebCrawlerRequestModel requestInformation;
            try
            {
                requestInformation = new WebCrawlerRequestModel(HttpContext.Request.Host.Value, url, email, files, robots);
            }
            catch (UriFormatException)
            {
                // TODO Make  unique error page
                return Redirect($"https://{HttpContext.Request.Host}/generate/results?guid=invalid");
            }

            // Submit message
            Logger.LogInformation("Submitting request to SQS");
            SQSHelper.SendMessage(requestInformation);

            // Add user to database
            Logger.LogInformation($"Adding user {requestInformation.Guid} to database");
            FirebaseHelper.AddUser(requestInformation.Guid.ToString());

            // Return request information
            Logger.LogInformation("Request complete");
            return Redirect($"https://{HttpContext.Request.Host}/generate/results?guid={requestInformation.Guid}");
        }
    }
}
