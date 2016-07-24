using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Common;
using Dapper;

namespace ComputerScience.Server.Web.Data.ProblemSet
{
    public class ProblemSet : IProblemSet<Problem>
    {
        public DbConnection Connection { get; }

        public bool IsDisposed { get; private set; }
        public string Table { get; }
        public int Limit { get; set; }

        public ProblemSet(DbConnection connection, string table = "problems", int limit = -1)
        {
            Connection = connection;
            Table = table;
            Limit = limit;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            Connection.Dispose();
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

            var problems = await Connection.QueryAsync<Problem>($"SELECT * FROM {Table} WHERE Id=@id", new {id});
            return problems.FirstOrDefault();
        }

        public async Task<IEnumerable<Problem>> FetchProblemByTitleAsync(string title, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));
            Handle(cancellationToken);

            if (Limit <= 0)
                return await Connection.QueryAsync<Problem>($"SELECT * FROM {Table} WHERE Title=@title", new {title});
            return
                await
                    Connection.QueryAsync<Problem>($"SELECT * FROM {Table} WHERE Title=@title LIMIT @Limit",
                        new {title, Limit});
        }

        public async Task<int> AddProblemAsync(string id, Problem problem, CancellationToken cancellationToken)
        {
            if (problem == null)
                throw new ArgumentNullException(nameof(problem));
            Handle(cancellationToken);
            return
                await
                    Connection.ExecuteAsync(
                        $"INSERT INTO {Table}(Id, Title, ProblemStatement, SolutionSize, ProblemPath, ProblemFile, TestCases) " +
                        "VALUES(@Id, @Title, @ProblemStatement, @SolutionSize, @ProblemPath, @TestCases)",
                        new
                        {
                            Id = id,
                            problem.Title,
                            problem.ProblemStatement,
                            problem.SolutionSize,
                            problem.ProblemPath,
                            problem.TestCases
                        });
        }

        public async Task<int> UpdateProblemAsync(string id, Problem problem, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            if (problem == null)
                throw new ArgumentNullException(nameof(problem));
            Handle(cancellationToken);
            return
                await
                    Connection.ExecuteAsync(
                        $"UPDATE {Table}" +
                        "SET Title=@Title, ProblemStatement=@ProblemStatement, SolutionSize=@SolutionSize, ProblemPath=@ProblemPath, TestCases=@TestCases" +
                        "WHERE Id=@Id",
                        new
                        {
                            problem.Id,
                            problem.Title,
                            problem.ProblemStatement,
                            problem.SolutionSize,
                            problem.ProblemPath,
                            problem.TestCases
                        });
        }

        public async Task<int> RemoveProblemAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            Handle(cancellationToken);
            return await Connection.ExecuteAsync(
                       $"REMOVE FROM {Table} WHERE Id=@Id",
                       new
                       {
                           Id = id
                       });
        }
    }
}
