using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            WebCrawlerWorker1 = new WebCrawlerWorker(Configuration, Logger, 1);
            WebCrawlerWorker2 = new WebCrawlerWorker(Configuration, Logger, 2);
            WebCrawlerWorker3 = new WebCrawlerWorker(Configuration, Logger, 3);
            WebCrawlerWorker4 = new WebCrawlerWorker(Configuration, Logger, 4);
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
                while (!cancellationToken.IsCancellationRequested)
                    await Task.WhenAll(new Task[]
                    {
                        ScreenshotWorker.Start(cancellationToken),
                        WebCrawlerWorker1.Start(cancellationToken),
                        WebCrawlerWorker2.Start(cancellationToken),
                        WebCrawlerWorker3.Start(cancellationToken),
                        WebCrawlerWorker4.Start(cancellationToken)
                    });
            }, cancellationToken);
        }
    }
}
