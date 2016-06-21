using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Data.ProblemSet
{
    public interface IProblemSet<TProblem> : IDisposable
    {
        Task<TProblem> FetchProblemAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<TProblem>> FetchProblemByTitleAsync(string title, CancellationToken cancellationToken);
        Task<int> AddProblemAsync(string id, TProblem problem, CancellationToken cancellationToken);
        Task<int> UpdateProblemAsync(string id, TProblem problem, CancellationToken cancellationToken);
        Task<int> RemoveProblemAsync(string id, CancellationToken cancellationToken);
    }
}