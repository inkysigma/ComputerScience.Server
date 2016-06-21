using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace ComputerScience.Server.Grader
{
    public class Program
    {
        public static ConcurrentBag<Thread> Threads { get; set; }
        public ServerConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder();
        }
    }
}
