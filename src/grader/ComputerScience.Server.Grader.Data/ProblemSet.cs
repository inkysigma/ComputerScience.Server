using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Common;
using Dapper;

namespace ComputerScience.Server.Grader.Data
{
    public class ProblemSet : IProblemSet<Problem>
    {
        public DbConnection Connection { get; set; }
        public string Table { get; }
        public bool IsDisposed { get; private set; }

        public ProblemSet(DbConnection connection, string table = "problems")
        {
            Connection = connection;
            Table = table;
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
                throw new ObjectDisposedException(nameof(ProblemSet));
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task<Problem> FetchProblemAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            Handle(cancellationToken);
            var result = await Connection.QueryAsync<Problem>($"SELECT * FROM {Table} WHERE Id=@id", new {id});
            return result.FirstOrDefault();
        }
    }
}
