using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webServer = new Process {StartInfo = new ProcessStartInfo("dotnet", "run ComputerScience.Server.Web")};
            webServer.StartInfo.CreateNoWindow = false;
            webServer.Start();
            System.Console.WriteLine("Web server has started.");
            System.Console.ReadKey();
            webServer.Dispose();
        }
    }
}
