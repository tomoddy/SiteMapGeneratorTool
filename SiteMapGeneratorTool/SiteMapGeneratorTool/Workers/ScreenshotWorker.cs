using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.Workers
{
    /// <summary>
    /// Screenshot worker
    /// </summary>
    public class ScreenshotWorker
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
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");
            options.AddArgument("window-size=1280, 720");
            ChromeDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);

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
                    // Navigate to web page and take screenshot
                    ChromeDriver.Navigate().GoToUrl(request.Address);
                    (ChromeDriver as ITakesScreenshot).GetScreenshot().SaveAsFile($"wwwroot/screenshots/{request.Guid}.png");
                }
            }
        }
    }
}