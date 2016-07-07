using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace ComputerScience.Server.Web
{
    public class Program
    {
        static IWebHost host;
        public static void Main()
        {
            host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("https://unix:/etc/inetpub/compsci/ComputerScience.Server.Web/kestrel.sock")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
            
            host.Run();
        }

        public static void Stop()
        {
            host.Dispose();
        }
    }
}
