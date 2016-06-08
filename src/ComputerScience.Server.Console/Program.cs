using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace ComputerScience.Server.Console
{
    public class Program
    {
        public static volatile bool IsRunning = true;
        public static volatile Process WebServer;

        public static ConcurrentDictionary<string, Thread> OutputThreads { get; set; } 

        public static void Main(string[] args)
        {
            WebServer = new Process
            {
                StartInfo = new ProcessStartInfo("dotnet", "run ComputerScience.Server.Web")
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WorkingDirectory = "..\\ComputerScience.Server.Web"
                }
            };

            WebServer.Start();

            System.Console.WriteLine("Web server has started.");
            while (IsRunning)
            {
                if (!string.IsNullOrEmpty(WebServer.StandardOutput.ReadLine()))
                    System.Console.WriteLine(WebServer.StandardOutput.ReadLine());
            }
        }
    }
}
