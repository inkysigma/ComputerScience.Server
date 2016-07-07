using System;
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
            var thread = new Thread(Web.Program.Main);
            thread.Start();
            while (IsRunning)
            {
                var key = System.Console.ReadKey();

                if (key == new ConsoleKeyInfo('c', ConsoleKey.C, false, false, false))
                    break;
            }
        }
    }
}
