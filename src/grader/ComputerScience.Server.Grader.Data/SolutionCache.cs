using StackExchange.Redis;
using ComputerScience.Server.Commons;

namespace ComputerScience.Server.Grader.Data
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class SolutionCache
    {
        public IDatabase Database { get; }
        
        public SolutionCache(IDatabase database)
        {
            Database = database;
        }

        public async Task<Solution> Fetch(){
            Database.
        }
    }
}
