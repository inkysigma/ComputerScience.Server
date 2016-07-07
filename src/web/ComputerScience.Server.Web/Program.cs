﻿using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ComputerScience.Server.Web
{
    public class Program
    {
        public static void Main()
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            Console.WriteLine("Stable");
            host.Run();
        }
    }
}
