using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using System;

namespace SiteMapGeneratorTool.Controllers.API
{
    /// <summary>
    /// Web crawler request controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WebCrawlerController : ControllerBase
    {
        // Variables
        private readonly IConfiguration Configuration;
        private readonly FirebaseHelper RequestFirebaseHelper;
        private readonly FirebaseHelper NotificationFirebaseHelper;
        private readonly ILogger Logger;
        private readonly SQSHelper SQSHelper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected object</param>
        /// <param name="logger">Injected object</param>
        public WebCrawlerController(IConfiguration configuration, ILogger<WebCrawlerController> logger)
        {
            Configuration = configuration;
            RequestFirebaseHelper = new FirebaseHelper(
                Configuration.GetValue<string>("Firebase:KeyPath"),
                Configuration.GetValue<string>("Firebase:Database"),
                Configuration.GetValue<string>("Firebase:RequestCollection"));
            NotificationFirebaseHelper = new FirebaseHelper(
                Configuration.GetValue<string>("Firebase:KeyPath"),
                Configuration.GetValue<string>("Firebase:Database"),
                Configuration.GetValue<string>("Firebase:NotificationCollection"));
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
        [HttpGet]
        public IActionResult Index(string url, string email, bool files, bool robots, string endpoint, string p256dh, string auth)
        {
            // Get domain
            string domain = (HttpContext ?? null) is null ? Configuration.GetValue<string>("Test:Domain") : HttpContext.Request.Host.Value;

            // Create request
            Logger.LogInformation($"Creating request for {url}");
            WebCrawlerRequestModel requestInformation;
            try
            {
                requestInformation = new WebCrawlerRequestModel(domain, url, email, files, robots);
            }
            catch (UriFormatException)
            {
                return Redirect($"https://{domain}/generate/results?guid=invalid");
            }

            // Submit message
            Logger.LogInformation("Submitting request to SQS");
            SQSHelper.SendMessage(requestInformation);

            // Upload notification information
            NotificationFirebaseHelper.Add(requestInformation.Guid.ToString(), new SubscriptionModel(endpoint, p256dh, auth));

            // Return request information
            Logger.LogInformation("Request complete");
            return Redirect($"https://{domain}/results?guid={requestInformation.Guid}");
        }
    }
}
