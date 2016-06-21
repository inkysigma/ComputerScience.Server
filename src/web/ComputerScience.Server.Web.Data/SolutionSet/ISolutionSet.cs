using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Data.SolutionSet
{
    public interface ISolutionSet<TSolution> : IDisposable
    {
        Task<TSolution> FetchSolutionMetdataAsync(string id, CancellationToken cancellationToken);

        Task AddSolutionMetadataAsync(string id, TSolution solution, CancellationToken cancellationToken);

        Task RemoveSolutionMetadataAsync(string id, CancellationToken cancellationToken);

        Task<int> FetchSizeAsync(CancellationToken cancellationToken);

        Task ClearSolutionMetadaAsync(CancellationToken cancellationToken);
    }
}