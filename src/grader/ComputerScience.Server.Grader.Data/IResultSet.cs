using System;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Common;

namespace ComputerScience.Server.Grader.Data
{
    public interface IResultSet<TResult> : IDisposable
    {
        Task AddResult(TResult result, CancellationToken cancellationToken);
    }
}