using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ParentWorker> Logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger">Injected dependancy</param>
        public ScreenshotWorker(ILogger<ParentWorker> logger)
        {
            Logger = logger;
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
                Logger.LogInformation("Screenshotter : Waiting for task");
                await Task.Delay(REST, cancellationToken);
            }
        }
    }
}
