using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;
using RestSharp;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.Workers
{
    /// <summary>
    /// Screenshot worker
    /// </summary>
    public class ScreenshotWorker : IWorker
    {
        // Constants
        private const int REST = 500;

        // Variables
        private readonly ChromeDriver ChromeDriver;
        private readonly IConfiguration Configuration;
        private readonly ILogger<ParentWorker> Logger;
        private readonly SQSHelper SQSHelper;
        private readonly S3Helper S3Helper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger">Injected dependancy</param>
        public ScreenshotWorker(IConfiguration configuration, ILogger<ParentWorker> logger)
        {
            Configuration = configuration;
            Logger = logger;
            SQSHelper = new SQSHelper(
                Configuration.GetValue<string>("AWS:Credentials:AccessKey"),
                Configuration.GetValue<string>("AWS:Credentials:SecretKey"),
                Configuration.GetValue<string>("AWS:SQS:ServiceUrl"),
                Configuration.GetValue<string>("AWS:SQS:QueueNameScreenshots"),
                Configuration.GetValue<string>("AWS:Credentials:AccountId"));
            S3Helper = new S3Helper(Configuration.GetValue<string>("AWS:Credentials:AccessKey"),
                Configuration.GetValue<string>("AWS:Credentials:SecretKey"),
                Configuration.GetValue<string>("AWS:S3:BucketName"));
        }

        /// <summary>
        /// Start worker
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Completed task</returns>
        public async Task Start(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Get request information
                Logger.LogInformation($"Screenshotter : Obtaining messages");
                ScreenshotRequestModel request = SQSHelper.DeleteAndReieveFirstMessage<ScreenshotRequestModel>();

                if (request is null)
                    await Task.Delay(REST, cancellationToken);
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}