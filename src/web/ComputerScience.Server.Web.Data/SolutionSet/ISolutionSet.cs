using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Data.SolutionSet
{
    public interface ISolutionMetadataSet<TSolution>
    {
        Task<TSolution> FetchSolutionMetdataAsync(string id, CancellationToken cancellationToken);

        Task AddSolutionMetadataAsync(TSolution solution, CancellationToken cancellationToken);

        Task RemoveSolutionMetadataAsync(string id, CancellationToken cancellationToken);

        Task<int> FetchSizeAsync();

        Task ClearSolutionMetadaAsync();
    }
}