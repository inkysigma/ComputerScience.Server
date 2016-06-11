using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Data.ProblemSet;

namespace ComputerScience.Server.Web.Business.Problems
{
    public class ProblemService<TProblem> : IProblemService<TProblem>
    {
        public IProblemSet<TProblem> ProblemSet { get; }

        public bool IsDisposed { get; } = false;

        public ProblemService(IProblemSet<TProblem> problemSet, bool isDisposed)
        {
            ProblemSet = problemSet;
            IsDisposed = isDisposed;
        }

        public Task AddProblemAsync(TProblem problem, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TProblem> FetchProblemAsync(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TProblem>> FetchProblemByTitleAsync(string title, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TProblem>> FetchProblemByRankAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProblemAsync(string id, TProblem problem, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProblemAsync(string id, CancellationToken cancellelToken)
        {
            throw new NotImplementedException();
        }
    }
}
