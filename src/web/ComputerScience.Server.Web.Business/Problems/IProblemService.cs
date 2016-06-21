using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Business.Problems
{
    public interface IProblemService<TProblem> : IDisposable
    {
        Task AddProblemAsync(string id, TProblem problem, CancellationToken cancellationToken);

        Task<TProblem> FetchProblemAsync(string id, CancellationToken cancellationToken);

        Task<IEnumerable<TProblem>> FetchProblemByTitleAsync(string title, CancellationToken cancellationToken);

        Task<IEnumerable<TProblem>> FetchProblemByRankAsync(CancellationToken cancellationToken);

        Task UpdateProblemAsync(string id, TProblem problem, CancellationToken cancellationToken);

        Task<bool> Exists(string guid, CancellationToken cancellationToken);

        Task RemoveProblemAsync(string id, CancellationToken cancellelToken);
    }
}