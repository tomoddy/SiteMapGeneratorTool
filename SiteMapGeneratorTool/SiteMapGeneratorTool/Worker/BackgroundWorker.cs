using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.Worker
{
    public class BackgroundWorker : IHostedService, IDisposable
    {
        private readonly ILogger<BackgroundWorker> logger;
        private Timer timer;
        private int number;

        public BackgroundWorker(ILogger<BackgroundWorker> logger)
        {
            this.logger = logger;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(o =>
            {
                Interlocked.Increment(ref number);
                logger.LogInformation($"Printing the worker number {number}");
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
