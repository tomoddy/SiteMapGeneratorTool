﻿using GiGraph.Dot.Entities.Graphs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SiteMapGeneratorTool.Helpers;
using SiteMapGeneratorTool.Models;
using SiteMapGeneratorTool.WebCrawler;
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
                Configuration.GetValue<string>("Firebase:BasePath"),
                Configuration.GetValue<string>("Firebase:AuthSecret"));
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
                Logger.LogInformation($"Web Crawler {Id}: Obtaining messages");
                WebCrawlerRequestModel request = SQSHelper.DeleteAndReieveFirstMessage();

                if (request is null)
                    await Task.Delay(Configuration.GetValue<int>("Workers") * Configuration.GetValue<int>("Delay"), cancellationToken);
                else
                {
                    // Run web crawler
                    Logger.LogInformation($"Web Crawler {Id}: Crawling {request}");
                    Crawler crawler = new Crawler(request.Url.ToString(), request.Files, request.Robots);
                    crawler.Run();

                    // Upload files
                    Logger.LogInformation($"Web Crawler {Id}: Uploading files");
                    S3Helper.UploadFile(request.Guid.ToString(), Configuration.GetValue<string>("AWS:S3:Files:Structure"), crawler.GetStructureJson());
                    S3Helper.UploadFile(request.Guid.ToString(), Configuration.GetValue<string>("AWS:S3:Files:Sitemap"), crawler.GetSitemapXml());
                    S3Helper.UploadFile(request.Guid.ToString(), Configuration.GetValue<string>("AWS:S3:Files:Graph"), GraphHelper.Render(crawler.Webpages));

                    // Upload information
                    Logger.LogInformation($"Web Crawler {Id}: Uploading information");
                    FirebaseHelper.Add(request.Guid.ToString(), crawler);

                    // Send email notification
                    if (!(request.Email is null))
                    {
                        Logger.LogInformation($"Web Crawler {Id}: Sending email to {request.Email}");
                        EmailHelper.SendEmail(request.Email, request.Domain, request.Guid.ToString());
                    }

                    // Return completed task
                    Logger.LogInformation($"Web Crawler {Id}: Task completed");
                }
            }
            Logger.LogError($"Web Crawler {Id}: Task cancelled");
        }
    }
}
