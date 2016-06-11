using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Models.Problems;

namespace ComputerScience.Server.Web.Business.Problems
{
    public interface IProblemService<TProblem>
    {
        Task AddProblemAsync(TProblem problem, CancellationToken cancellationToken);

        Task<TProblem> FetchProblemAsync(string id, CancellationToken cancellationToken);

        Task<IEnumerable<TProblem>> FetchProblemByTitleAsync(string title, CancellationToken cancellationToken);

        Task<IEnumerable<TProblem>> FetchProblemByRankAsync(CancellationToken cancellationToken);

        Task UpdateProblemAsync(string id, TProblem problem, CancellationToken cancellationToken);

        Task RemoveProblemAsync(string id, CancellationToken cancellelToken);
    }
}