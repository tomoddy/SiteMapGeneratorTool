using System.Threading;
using System.Threading.Tasks;

namespace SiteMapGeneratorTool.Workers
{
    interface IWorker
    {
        Task Start(CancellationToken cancellationToken);
    }
}
