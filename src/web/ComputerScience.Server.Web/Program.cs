using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace ComputerScience.Server.Web
{
    public class Program
    {
        public static IWebHost Host;
        public static void Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("host.settings");

            Host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("https://unix:/etc/inetpub/compsci/ComputerScience.Server.Web/kestrel.sock")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            Console.WriteLine("Stable");
            Host.Run();
        }

        public static void Stop()
        {
            Host.Dispose();
        }
    }
}
