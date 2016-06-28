using System.Data.Common;
using System.Text;
using ComputerScience.Server.Web.Business.Problems;
using ComputerScience.Server.Web.Business.Solutions;
using ComputerScience.Server.Web.Configuration;
using ComputerScience.Server.Web.Data.ProblemSet;
using ComputerScience.Server.Web.Data.SolutionCache;
using ComputerScience.Server.Web.Data.SolutionSet;
using ComputerScience.Server.Web.Middleware;
using ComputerScience.Server.Web.Models.Problems;
using ComputerScience.Server.Web.Models.Solutions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using StackExchange.Redis;

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

            services.AddTransient<DbConnection>(provider => new NpgsqlConnection(Configuration["ConnectionString"]));

            services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                var builder = new StringBuilder();
                var servers = JsonConvert.DeserializeObject<RedisConfiguration>(Configuration["Redis"]);
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

            services.AddTransient<ISolutionSet<Solution>>(provider => new SolutionSet(new SolutionSetConfiguration(), provider.GetService<DbConnection>()));

            services.AddTransient<IProblemSet<Problem>>(
                provider => new ProblemSet(provider.GetService<DbConnection>(), "problems" ,10));

            services.AddTransient<IProblemService<Problem>>(provider => new ProblemService<Problem>(provider.GetService<IProblemSet<Problem>>()));

            services.AddTransient<ISolutionService<Solution>>(
                provider =>
                    new SolutionService<Solution>(provider.GetService<ISolutionCache<Solution>>(),
                        provider.GetService<ISolutionSet<Solution>>(), new SolutionValidator(),
                        new SolutionServiceConfiguration()));

            services.AddTransient<IProblemService<Problem>>(
                provider => new ProblemService<Problem>(provider.GetService<IProblemSet<Problem>>()));

            services.AddSingleton(provider => new SolutionConfiguration
            {
                FileLocation = Configuration["FileLocation"]
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseRequireHttps();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();
        }
    }
}
