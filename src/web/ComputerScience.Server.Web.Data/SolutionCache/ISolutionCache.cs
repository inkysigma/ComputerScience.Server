using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Data.SolutionCache
{
    public interface ISolutionCache<TSolution>
    {
        Task PutAsync(TSolution solution, CancellationToken cancellationToken);

        Task<TSolution> FetchAsync(string id, CancellationToken cancellationToken);

        Task<long> FetchSizeAsync(CancellationToken cancellationToken);
    }
}