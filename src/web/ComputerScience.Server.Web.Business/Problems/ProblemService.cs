using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Data.ProblemSet;

namespace ComputerScience.Server.Web.Business.Problems
{
    public class ProblemService<TProblem> : IProblemService<TProblem>
    {
        public IProblemSet<TProblem> ProblemSet { get; }

        public bool IsDisposed { get; private set; } = false;

        private void Handle(CancellationToken cancellationToken)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(ProblemService<TProblem>));
            cancellationToken.ThrowIfCancellationRequested();
        }

        public ProblemService(IProblemSet<TProblem> problemSet)
        {
            ProblemSet = problemSet;
        }

        public async Task AddProblemAsync(string id, TProblem problem, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            Handle(cancellationToken);
            await ProblemSet.AddProblemAsync(id, problem, cancellationToken);
        }

        public async Task<TProblem> FetchProblemAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            Handle(cancellationToken);
            return await ProblemSet.FetchProblemAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<TProblem>> FetchProblemByTitleAsync(string title, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));
            Handle(cancellationToken);
            return await ProblemSet.FetchProblemByTitleAsync(title, cancellationToken);
        }

        public Task<IEnumerable<TProblem>> FetchProblemByRankAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TProblem>> FetchRandomProblemsAsync(int number, CancellationToken cancellationToken)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number));
            Handle(cancellationToken);
            return await ProblemSet.FetchRandomProblems(number, cancellationToken);
        }

        public async Task<bool> Exists(string guid, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentNullException(nameof(guid));
            Handle(cancellationToken);
            return (await FetchProblemAsync(guid, cancellationToken)) != null;
        }

        public Task UpdateProblemAsync(string id, TProblem problem, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProblemAsync(string id, CancellationToken cancellelToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            ProblemSet.Dispose();
            IsDisposed = true;
        }
    }
}
