using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.WebCrawler;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.Workers
{
    /// <summary>
    /// Worker for web crawling
    /// </summary>
    public class WebCrawlerWorker : IHostedService, IDisposable
    {
        // Constants
        private const int REST = 500;

        // Variables
        private readonly IConfiguration Configuration;
        private readonly EmailHelper EmailHelper;
        private readonly ILogger<WebCrawlerWorker> Logger;
        private readonly SQSHelper SQSHelper;
        private readonly S3Helper S3Helper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected configuration</param>
        /// <param name="logger">Injected logger</param>
        public WebCrawlerWorker(IConfiguration configuration, ILogger<WebCrawlerWorker> logger)
        {
            Configuration = configuration;
            EmailHelper = new EmailHelper(
                Configuration.GetValue<string>("SMTP:UserName"),
                Configuration.GetValue<string>("SMTP:DisplayName"),
                Configuration.GetValue<string>("SMTP:Password"),
                Configuration.GetValue<string>("SMTP:Host"),
                Configuration.GetValue<string>("SMTP:Port"));
            Logger = logger;
            SQSHelper = new SQSHelper(
                Configuration.GetValue<string>("AWS:Credentials:AccessKey"),
                Configuration.GetValue<string>("AWS:Credentials:SecretKey"),
                Configuration.GetValue<string>("AWS:SQS:ServiceUrl"),
                Configuration.GetValue<string>("AWS:SQS:QueueName"),
                Configuration.GetValue<string>("AWS:Credentials:AccountId"));
            S3Helper = new S3Helper(Configuration.GetValue<string>("AWS:Credentials:AccessKey"),
                Configuration.GetValue<string>("AWS:Credentials:SecretKey"),
                Configuration.GetValue<string>("AWS:S3:BucketName"));
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            return;
        }

        /// <summary>
        /// Method to begin checking crawl queue and perfoming crawls
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Get request information
                Logger.LogInformation("Obtaining messages");
                WebCrawlerRequestModel request = SQSHelper.DeleteAndReieveFirstMessage();

                if (request is null)
                {
                    Logger.LogInformation("No reqests, sleeping");
                    Thread.Sleep(REST);
                }
                else
                {
                    // Run web crawler
                    Logger.LogInformation($"Crawling {request}");
                    Crawler crawler = new Crawler(request.Url.ToString(), request.Files, request.Robots);
                    crawler.Configure();
                    crawler.Run();

                    // Upload information
                    Logger.LogInformation("Uploading files");
                    S3Helper.UploadFile(request.Guid.ToString(), Configuration.GetValue<string>("AWS:S3:Files:Information"), crawler.GetInformationJson());
                    S3Helper.UploadFile(request.Guid.ToString(), Configuration.GetValue<string>("AWS:S3:Files:Sitemap"), crawler.GetSitemapXml());
                    S3Helper.UploadFile(request.Guid.ToString(), Configuration.GetValue<string>("AWS:S3:Files:Graph"), crawler.GetGraphXml());

                    // Send email notification
                    Logger.LogInformation($"Sending email to {request.Email}");
                    EmailHelper.SendEmail(request.Email, request.Domain, request.Guid.ToString());

                    // Return completed task
                    Logger.LogInformation("Task completed");
                }
            }
            Logger.LogError("Task cancelled");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method to stop crawl
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Completed task</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogError("Task cancelled");
            return Task.CompletedTask;
        }
    }
}
