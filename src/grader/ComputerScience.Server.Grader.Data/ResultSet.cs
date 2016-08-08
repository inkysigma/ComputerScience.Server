using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Common;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ComputerScience.Server.Grader.Data
{
    public class ResultSet : IResultSet<Result>
    {
        public DbConnection Connection { get; }
        public bool IsDisposed { get; private set; } = false;
        public string Table { get; }
        public ILogger<IResultSet<Result>> Logger { get; set; }

        public ResultSet(DbConnection connection, ILogger<IResultSet<Result>> logger, string table = "results")
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

        public async Task AddResult(Result result, CancellationToken cancellationToken)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (string.IsNullOrEmpty(result.Id))
                throw new ArgumentNullException(nameof(result.Id));
            Handle(cancellationToken);
            using (var transcation = Connection.BeginTransaction())
            {
                var numResults = (await Connection.QueryAsync<int>($"SELECT COUNT(*) FROM {Table} WHERE id=@id", new { result.Id })).FirstOrDefault();
                if (numResults != 0)
                {
                    Logger.LogInformation($"A duplicate at {result.Id} was found.");
                    return;
                }
                await
                    Connection.ExecuteAsync(
                        "INSERT INTO results(Id, TimeStamp, Result, Error) VALUES(@id, @timeStamp, @result, @error)", new
                        {
                            result.Id,
                            result.TimeStamp,
                            TestCases = JsonConvert.SerializeObject(result.TestCases),
                            result.Error
                        });
                transcation.Commit();
            }
        }
    }
}
