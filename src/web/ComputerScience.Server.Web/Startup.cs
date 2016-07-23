using System;
using System.Data.Common;
using System.Text;
using ComputerScience.Server.Web.Business.Problems;
using ComputerScience.Server.Web.Business.Solutions;
using ComputerScience.Server.Web.Configuration;
using ComputerScience.Server.Web.Data.ProblemSet;
using ComputerScience.Server.Web.Data.SolutionCache;
using ComputerScience.Server.Web.Data.SolutionSet;
using ComputerScience.Server.Web.Middleware;
using ComputerScience.Server.Web.Models.Solutions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using StackExchange.Redis;
using ComputerScience.Server.Web.Formatters;
using System.Buffers;
using ComputerScience.Server.Common;
using ComputerScience.Server.Web.Business;
using ComputerScience.Server.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Problem = ComputerScience.Server.Web.Models.Problems.Problem;
using ComputerScience.Server.Web.Extentions;

namespace ComputerScience.Server.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsEnvironment("Development"))
                builder.AddApplicationInsightsSettings(true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<UserContext>(options => options.UseNpgsql(Configuration["Data:Default:EntityFramework"]));

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                })
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<DbConnection>(provider => new NpgsqlConnection(Configuration["Data:Default:ConnectionString"]));

            services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                var builder = new StringBuilder();
                var servers = JsonConvert.DeserializeObject<RedisConfiguration>(Configuration["Data:Redis"]);
                foreach (var server in servers.Servers)
                {
                    builder.Append(server.Key + ":" + server.Value);
                }
                return ConnectionMultiplexer.Connect(builder.ToString());
            });

            services.AddSingleton(provider => provider.GetService<IConnectionMultiplexer>().GetServer("localhost", 6372));

            services.AddTransient<ISolutionCache<Solution>>(
                provider =>
                        new SolutionCache(provider.GetService<IConnectionMultiplexer>()));

            services.AddSolutionServices(f => f.GetService<DbConnection>(), f => f.GetService<ISolutionCache<Solution>>());

            services.AddTransient<IProblemSet<Problem>>(
                provider => new ProblemSet(provider.GetService<DbConnection>(), "problems" ,10));

            services.AddTransient<IProblemService<Problem>>(provider => new ProblemService<Problem>(provider.GetService<IProblemSet<Problem>>()));

            services.AddSingleton(provider => new SolutionConfiguration
            {
                FileLocation = Configuration["FileLocation"]
            });

            services.AddMvc(c => {
                c.OutputFormatters.Clear();
                c.OutputFormatters.Add(new StandardOutputFormatter(new JsonSerializerSettings(), ArrayPool<char>.Shared));
             });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseRequireHttps();

            app.UseIdentity();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();
        }
    }
}
