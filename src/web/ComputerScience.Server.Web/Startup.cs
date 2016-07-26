using System.Data.Common;
using ComputerScience.Server.Web.Business.Problems;
using ComputerScience.Server.Web.Configuration;
using ComputerScience.Server.Web.Data.ProblemSet;
using ComputerScience.Server.Web.Data.SolutionCache;
using ComputerScience.Server.Web.Middleware;
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
using ComputerScience.Server.Web.ExceptionHandling;
using ComputerScience.Server.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ComputerScience.Server.Web.Extentions;

namespace ComputerScience.Server.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory factory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsEnvironment("Development"))
                builder.AddApplicationInsightsSettings(true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            Factory = factory;
        }

        public IConfigurationRoot Configuration { get; }
        public ILoggerFactory Factory { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<UserContext>(options => options.UseNpgsql(Configuration["Data:EntityFramework"]));

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<DbConnection>(provider => new NpgsqlConnection(Configuration["Data:Default:ConnectionString"]));

            services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(Configuration["Data:Redis"]));

            services.AddSingleton(provider => provider.GetService<IConnectionMultiplexer>().GetServer("localhost", 6372));

            services.AddTransient<ISolutionCache<Solution>>(
                provider =>
                        new SolutionCache(provider.GetService<IConnectionMultiplexer>()));

            services.AddSolutionServices(f => f.GetService<DbConnection>(), f => f.GetService<ISolutionCache<Solution>>());

            services.AddTransient(f => new SolutionConfiguration
            {
                FileLocation = Configuration["FileLocation"]
            });

            services.AddTransient<IProblemSet<Problem>>(
                provider => new ProblemSet(provider.GetService<DbConnection>(), "problems" ,10));

            services.AddTransient<IProblemService<Problem>>(provider => new ProblemService<Problem>(provider.GetService<IProblemSet<Problem>>()));

            services.AddTransient(provider => new AccountControllerConfiguration
            {
                CaptchaSecret = Configuration["Captcha:Secret"]
            });

            services.AddMvc(c => {
                c.OutputFormatters.Clear();
                c.OutputFormatters.Add(new StandardOutputFormatter(new JsonSerializerSettings(), ArrayPool<char>.Shared));
                c.Filters.Add(new ExceptionFilter(Factory.CreateLogger<ExceptionFilter>(), 
                    new JsonExceptionPage(Factory.CreateLogger<IExceptionPage>())));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            app.UseRequireHttps();

            app.UseRequireUtfMiddleware();

            app.UseCsrfProtectionMiddleware(Configuration["Server:Origin"], Configuration["Server:Referer"]);

            app.UseApplicationInsightsRequestTelemetry();

            app.UseRemoveKestrelHeader();

            app.UseIdentity();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc(f =>
            {
                f.MapRoute("", "api/{controller}/{action}");
            });
        }
    }
}
