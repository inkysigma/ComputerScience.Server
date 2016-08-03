using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Grader.Data
{
    public interface IResultSet : IDisposable
    {
        Task AddResult(string id, DateTime timeStamp, string result, string error, CancellationToken cancellationToken);
    }
}