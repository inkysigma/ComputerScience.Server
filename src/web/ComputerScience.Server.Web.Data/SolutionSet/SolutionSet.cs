using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Common;
using Dapper;

namespace ComputerScience.Server.Web.Data.SolutionSet
{
    public class SolutionSet : ISolutionSet<Solution>
    {
        public SolutionSetConfiguration Configuration { get; }

        public DbConnection Connection { get; }

        public string Table { get; }

        public bool IsDisposed { get; private set; }

        public SolutionSet(SolutionSetConfiguration configuration, DbConnection connection, string table = "solutions")
        {
            Configuration = configuration;
            Connection = connection;
            Table = table;
        }

        private void Handle(CancellationToken cancellationToken)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(SolutionSet));
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task<Solution> FetchSolutionMetdataAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            Handle(cancellationToken);
            var result = await Connection.QueryAsync<Solution>($"SELECT * FROM {Table} WHERE Id=@Id", new { Id = id });
            return result.FirstOrDefault();
        }

        public async Task AddSolutionMetadataAsync(string id, Solution solution, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            if (solution == null)
                throw new ArgumentNullException(nameof(solution));
            Handle(cancellationToken);
            await
                Connection.ExecuteAsync(
                    $"INSERT INTO {Table} (Id, User, TimeStamp, FileLocation, File, ProblemId, SolutionType) " +
                    $"VALUES(@Id, @User, @TimeStamp, @FileLocation, @File, @ProblemId, @SolutionType)", new
                    {
                        solution.Id,
                        solution.User,
                        solution.TimeStamp,
                        solution.FileLocation,
                        solution.File,
                        solution.ProblemId,
                        solution.SolutionType
                    });
        }

        public async Task RemoveSolutionMetadataAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            Handle(cancellationToken);
            await Connection.ExecuteAsync($"REMOVE FROM {Table} WHERE Id=@Id", new
            {
                Id = id
            });
        }

        public async Task<int> FetchSizeAsync(CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            return (await Connection.QueryAsync<int>($"SELECT COUNT(*) FROM {Table}")).FirstOrDefault();
        }

        public async Task ClearSolutionMetadaAsync(CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            await Connection.ExecuteAsync($"REMOVE FROM {Table} WHERE TimeStamp + @time > @current", 
                new
                {
                    time = Configuration.SolutionSetTime,
                    current = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds
                });
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            Connection.Dispose();
            IsDisposed = true;
        }
    }
}
