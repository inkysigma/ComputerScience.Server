using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Threading;
using ComputerScience.Server.Common;
using ComputerScience.Server.Grader.Compiler;
using ComputerScience.Server.Grader.Data;
using ComputerScience.Server.Grader.Executor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Newtonsoft.Json;
using Npgsql;

namespace ComputerScience.Server.Grader
{
    public class Program
    {
        public static volatile ConcurrentBag<Thread> Threads;
        public static volatile ServerConfiguration Configuration;
        public static volatile IConfigurationRoot ConfigurationRoot;
        public static volatile IServiceProvider Provider;
        public static volatile ISolutionCache<Solution> SolutionCache;
        public static volatile ConcurrentDictionary<string, ILogger> Loggers;
        public static volatile ServiceContainer Container;
        public static volatile bool IsRunning = false;
        public static LoggerFactory Factory;

        public static void Main()
        {
            LoadConfigurations();
            var directories = Configuration.Directories;
            IsRunning = true;
            for (int i = 0; i < Configuration.ThreadCount; i++)
            {
                var grader = new Grader(SolutionCache, Container.CreateNew<IProblemSet<Problem>>(),
                    Container.CreateNew<IResultSetService<Result>>(),
                    new Dictionary<SolutionType, ICompiler>()
                    {
                        {SolutionType.Cpp, new CppCompiler()}
                    }, new Dictionary<SolutionType, IExecutor>()
                    {
                        {
                            SolutionType.Cpp, new CppExecutor(Path.Combine(Directory.GetCurrentDirectory(), 
                            directories[i]), 100, new List<string>
                            {
                                "clone",
                                "fork"
                            })                         
                        }
                    }, Factory.CreateLogger<Grader>(), Path.Combine(Directory.GetCurrentDirectory(),
                            directories[i]));
                Threads.Add(new Thread(grader.Start));
            }

            foreach (var t in Threads)
            {
                t.Start();
            }
        }

        public static void Terminate()
        {
            IsRunning = false;
            foreach (var t in Threads)
            {
                t.Join();
            }
        }

        public static void LoadConfigurations()
        {
            Factory = new LoggerFactory();
            Factory.AddConsole();
            var configuration = new ConfigurationBuilder();
            configuration.AddEnvironmentVariables();
            configuration.AddJsonFile("appsettings.json");
            ConfigurationRoot = configuration.Build();

            Configuration = JsonConvert.DeserializeObject<ServerConfiguration>(File.ReadAllText("configuration.json"));

            Container = new ServiceContainer();

            var connectionMultiplexer = ConnectionMultiplexer.Connect(ConfigurationRoot["Data:Redis"]);
            var database = connectionMultiplexer.GetDatabase(0);

            SolutionCache = new SolutionCache(database);

            Container = new ServiceContainer();
            
            Factory.AddConsole();
            var consoleLogger = Factory.CreateLogger<IResultSet<Result>>();
            Container.Add<DbConnection>(f => new NpgsqlConnection(ConfigurationRoot["Server:Data:ConnectionString"]));
            var dbConnection = Container.CreateNew<DbConnection>();
            Container.Add<IResultSet<Result>>(f => new ResultSet(dbConnection, consoleLogger));
            Container.Add<IResultSetService<Result>>(f => new ResultSetService<Result>(f.CreateNew<IResultSet<Result>>()));
            Container.Add<IProblemSet<Problem>>(f => new ProblemSet(dbConnection));
        }
    }
}
