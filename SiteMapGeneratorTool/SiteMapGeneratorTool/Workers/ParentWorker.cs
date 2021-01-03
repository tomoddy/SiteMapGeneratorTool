using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.Workers
{
    /// <summary>
    /// Parent worker
    /// </summary>
    public class ParentWorker : BackgroundService
    {
        // Variables
        private readonly IConfiguration Configuration;
        private readonly ILogger<ParentWorker> Logger;
        private readonly ScreenshotWorker ScreenshotWorker;
        private readonly WebCrawlerWorker WebCrawlerWorker1;
        private readonly WebCrawlerWorker WebCrawlerWorker2;
        private readonly WebCrawlerWorker WebCrawlerWorker3;
        private readonly WebCrawlerWorker WebCrawlerWorker4;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Injected dependancy</param>
        /// <param name="logger">Injected dependancy</param>
        public ParentWorker(IConfiguration configuration, ILogger<ParentWorker> logger)
        {
            Configuration = configuration;
            Logger = logger;
            ScreenshotWorker = new ScreenshotWorker(Configuration, Logger);
        }

        /// <summary>
        /// Execute method called on application launch
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Completed task</returns>
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                List<Task> tasks = new List<Task>() { ScreenshotWorker.Start(cancellationToken) };
                for (int i = 1; i <= Configuration.GetValue<int>("Workers"); i++)
                    tasks.Add(new WebCrawlerWorker(Configuration, Logger, i).Start(cancellationToken));

                while (!cancellationToken.IsCancellationRequested)
                    await Task.WhenAll(tasks);
            }, cancellationToken);
        }
    }
}
