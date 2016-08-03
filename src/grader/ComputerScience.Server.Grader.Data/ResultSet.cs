using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ComputerScience.Server.Grader.Data
{
    public class ResultSet : IResultSet
    {
        public DbConnection Connection { get; }
        public bool IsDisposed { get; private set; } = false;
        public string Table { get; }
        public ILogger<ResultSet> Logger { get; set; }

        public ResultSet(DbConnection connection, ILogger<ResultSet> logger, string table)
        {
            Connection = connection;
            Table = table;
            Logger = logger;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            Connection.Dispose();
            IsDisposed = true;
        }

        private void Handle(CancellationToken cancellationToken)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(ResultSet));
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task AddResult(string id, DateTime timeStamp, string result, string error, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(result))
                throw new ArgumentNullException(nameof(result));
            Handle(cancellationToken);
            using (var transcation = Connection.BeginTransaction())
            {
                var numResults = (await Connection.QueryAsync<int>($"SELECT COUNT(*) FROM {Table} WHERE id=@id", new { id })).FirstOrDefault();
                if (numResults != 0)
                {
                    Logger.LogInformation($"A duplicate at {id} was found.");
                    return;
                }
                await
                    Connection.ExecuteAsync(
                        "INSERT INTO results(Id, TimeStamp, Result, Error) VALUES(@id, @timeStamp, @result, @error)", new
                        {
                            id,
                            timeStamp,
                            result,
                            error
                        });
                transcation.Commit();
            }
        }
    }
}
