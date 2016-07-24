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
        public static IConfigurationRoot Configuration;
        public static void Main()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("host.json");
            Configuration = config.Build();

            Host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.UseHttps("cert.cer");
                })
                .UseUrls(Configuration["Servers"])
                .UseIISIntegration()
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
