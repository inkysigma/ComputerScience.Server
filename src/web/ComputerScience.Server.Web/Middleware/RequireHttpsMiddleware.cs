using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ComputerScience.Server.Web.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RequireHttps
    {
        private readonly RequestDelegate _next;

        public RequireHttps(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.IsHttps)
                return;
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequireHttpsExtensions
    {
        public static IApplicationBuilder UseRequireHttps(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequireHttps>();
        }
    }
}
