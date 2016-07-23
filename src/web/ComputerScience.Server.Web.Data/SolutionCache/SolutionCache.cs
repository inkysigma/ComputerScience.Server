using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerScience.Server.Common;
using ComputerScience.Server.Web.Models.Solutions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ComputerScience.Server.Web.Data.SolutionCache
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class SolutionCache : ISolutionCache<Solution>
    {
        public IDatabase Database { get; }
        public string Table { get; }

        public SolutionCache(IConnectionMultiplexer connection, string table = "solutionQueue")
        {
            Database = connection.GetDatabase(0);
            Table = table;
        }

        public async Task PutAsync(Solution solution, CancellationToken cancellationToken)
        {
            if (solution == null)
                throw new ArgumentNullException(nameof(solution));
            cancellationToken.ThrowIfCancellationRequested();
            var seralized = JsonConvert.SerializeObject(solution);
            await Database.SortedSetAddAsync(Table, seralized, solution.TimeStamp);
            await Database.StringSetAsync(solution.Id, solution.TimeStamp);
        }

        public async Task<Solution> FetchAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException();
            cancellationToken.ThrowIfCancellationRequested();
            var time = long.Parse(await Database.StringGetAsync(id));
            var entries = await Database.SortedSetRangeByScoreWithScoresAsync(Table, time, time);
            if (entries.Length == 0)
                return null;
            return
                entries.Select(sortedSetEntry => JsonConvert.DeserializeObject<Solution>(sortedSetEntry.Element))
                    .FirstOrDefault(result => result.Id == id);
        }

        public async Task<long> FetchSizeAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Database.SortedSetLengthAsync(Table);
        }
    }
}
