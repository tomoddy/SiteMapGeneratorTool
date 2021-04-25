using GiGraph.Dot.Entities.Graphs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.WebCrawler;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.Workers
{
    /// <summary>
    /// Worker for web crawling
    /// </summary>
    public class WebCrawlerWorker : IWorker
    {
        // Variables
        private readonly IConfiguration Configuration;
        private readonly EmailHelper EmailHelper;
        private readonly FirebaseHelper FirebaseHelper;
        private readonly ILogger<ParentWorker> Logger;
        private readonly NotificationHelper NotificationHelper;
        private readonly SQSHelper SQSHelper;
        private readonly S3Helper S3Helper;

        // Properties
        private int Id { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected configuration</param>
        /// <param name="logger">Injected logger</param>
        public WebCrawlerWorker(IConfiguration configuration, ILogger<ParentWorker> logger, int id)
        {
            Configuration = configuration;
            EmailHelper = new EmailHelper(
                Configuration.GetValue<string>("SMTP:UserName"),
                Configuration.GetValue<string>("SMTP:DisplayName"),
                Configuration.GetValue<string>("SMTP:Password"),
                Configuration.GetValue<string>("SMTP:Host"),
                Configuration.GetValue<string>("SMTP:Port"));
            FirebaseHelper = new FirebaseHelper(
                Configuration.GetValue<string>("Firebase:KeyPath"),
                Configuration.GetValue<string>("Firebase:Database"),
                Configuration.GetValue<string>("Firebase:RequestCollection"));
            Logger = logger;
            NotificationHelper = new NotificationHelper(
                Configuration.GetValue<string>("Firebase:KeyPath"),
                Configuration.GetValue<string>("Firebase:Database"),
                Configuration.GetValue<string>("Firebase:NotificationCollection"),
                Configuration.GetValue<string>("VAPID:Subject"),
                Configuration.GetValue<string>("VAPID:PublicKey"),
                Configuration.GetValue<string>("VAPID:PrivateKey"));
            SQSHelper = new SQSHelper(
                Configuration.GetValue<string>("AWS:Credentials:AccessKey"),
                Configuration.GetValue<string>("AWS:Credentials:SecretKey"),
                Configuration.GetValue<string>("AWS:SQS:ServiceUrl"),
                Configuration.GetValue<string>("AWS:SQS:QueueName"),
                Configuration.GetValue<string>("AWS:Credentials:AccountId"));
            S3Helper = new S3Helper(Configuration.GetValue<string>("AWS:Credentials:AccessKey"),
                Configuration.GetValue<string>("AWS:Credentials:SecretKey"),
                Configuration.GetValue<string>("AWS:S3:BucketName"));

            Id = id;
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
                WebCrawlerRequestModel request = SQSHelper.DeleteAndReceiveFirstMessage();

                if (request is null)
                    await Task.Delay(Configuration.GetValue<int>("Workers") * Configuration.GetValue<int>("Delay"), cancellationToken);
                else
                {
                    try
                    {
                        // Check depth and page maximum
                        int depth = request.Depth == 0 ? Configuration.GetValue<int>("Depth") : request.Depth;
                        int maxPages = request.MaxPages == 0 ? Configuration.GetValue<int>("MaxPages") : request.MaxPages;

                        // Run web crawler
                        Logger.LogInformation($"Web Crawler {Id}: Crawling {request}");
                        Crawler crawler = new Crawler(request.Url.ToString(), depth, maxPages, request.Files, request.Robots, Configuration.GetValue<int>("Threads"), Configuration.GetValue<int>("PolitenessPolicy"));
                        crawler.Run();

                        // Upload files
                        Logger.LogInformation($"Web Crawler {Id}: Uploading files");
                        S3Helper.UploadFile(request.Guid.ToString(), Configuration.GetValue<string>("AWS:S3:Files:Structure"), crawler.GetStructureJson());
                        S3Helper.UploadFile(request.Guid.ToString(), Configuration.GetValue<string>("AWS:S3:Files:Sitemap"), crawler.GetSitemapXml());
                        S3Helper.UploadFile(request.Guid.ToString(), Configuration.GetValue<string>("AWS:S3:Files:Graph"), GraphHelper.Render(crawler.Webpages));

                        // Upload information
                        Logger.LogInformation($"Web Crawler {Id}: Uploading information");
                        FirebaseHelper.Add(request.Guid.ToString(), crawler.GetCrawlerData(request.Guid.ToString(), request.Depth, request.MaxPages));

                        // Send email notification
                        if (!(request.Email is null))
                        {
                            Logger.LogInformation($"Web Crawler {Id}: Sending email to {request.Email}");
                            EmailHelper.SendEmail(request.Email, request.Domain, request.Guid.ToString());
                        }

                        // Send desktop notification
                        NotificationHelper.SendNotification(request.Guid.ToString(), new List<string> { $"Web crawl request for {request.Url} is complete", request.Guid.ToString() });

                        // Return completed task
                        Logger.LogInformation($"Web Crawler {Id}: Task completed");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Web Crawler {Id}: {ex.Message}");
                        FirebaseHelper.Add(request.Guid.ToString(), new CrawlerData() { Guid = "FAILURE", Message = ex.Message });
                    }
                }
            }
            Logger.LogError($"Web Crawler {Id}: Task cancelled");
        }
    }
}
