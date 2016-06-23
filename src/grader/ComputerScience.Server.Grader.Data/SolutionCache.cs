using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using ComputerScience.Server.Common;
using Newtonsoft.Json;

namespace ComputerScience.Server.Grader.Data
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class SolutionCache : ISolutionCache<Solution>
    {
        public IDatabase Database { get; }

        public string Table { get; }
        
        public SolutionCache(IDatabase database, string table = "solutionQueue")
        {
            Database = database;
            Table = table;
        }

        public async Task<Solution> Fetch()
        {
            var result = await Database.SortedSetRangeByScoreAsync(Table, take: 1L);
            var obj = JsonConvert.DeserializeObject<Solution>(result.FirstOrDefault());
            await Database.SortedSetRemoveAsync(Table, obj.TimeStamp);
            await Database.KeyDeleteAsync(obj.Id);
            return obj;
        }
    }
}
