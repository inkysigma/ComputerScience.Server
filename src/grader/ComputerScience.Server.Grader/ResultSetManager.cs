using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Common;
using ComputerScience.Server.Grader.Data;
using Newtonsoft.Json;

namespace ComputerScience.Server.Grader
{
    public interface IResultSetService<TResult> : IDisposable
    {
        Task AddResultAsync(TResult result, CancellationToken cancellationToken);
    }

    public class ResultSetService<TResult> : IResultSetService<TResult>
    {
        public IResultSet<TResult> ResultSet { get; }
        public bool IsDisposed { get; private set; } = false;

        public ResultSetService(IResultSet<TResult> resultSet)
        {
            ResultSet = resultSet;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            ResultSet.Dispose();
            IsDisposed = true;
        }

        public async Task AddResultAsync(TResult result, CancellationToken cancellationToken)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(ResultSetService<TResult>));
            cancellationToken.ThrowIfCancellationRequested();
            await
                ResultSet.AddResult(result, cancellationToken);
        }
    }
}
