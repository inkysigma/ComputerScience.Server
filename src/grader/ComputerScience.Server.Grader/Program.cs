using System.Collections.Concurrent;
using System.Threading;

namespace ComputerScience.Server.Grader
{
    public class Program
    {
        public static ConcurrentBag<Thread> Threads { get; set; }
        public ServerConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
        }
    }
}
