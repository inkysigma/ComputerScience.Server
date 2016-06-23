using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ComputerScience.Server.Grader
{
    public class Program
    {
        public static volatile ConcurrentBag<Thread> Threads;
        public static volatile ServerConfiguration Configuration;
        public static volatile IConfigurationRoot ConfigurationRoot;
        public static volatile bool IsRunning = false;

        public static void Main(string[] args)
        {
            LoadConfigurations();
        }

        public static void LoadConfigurations()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("configuration.json");
            configuration.AddEnvironmentVariables();
            ConfigurationRoot = configuration.Build();

            Configuration = JsonConvert.DeserializeObject<ServerConfiguration>(ConfigurationRoot["ServerConfiguration"]);
        }
    }
}
