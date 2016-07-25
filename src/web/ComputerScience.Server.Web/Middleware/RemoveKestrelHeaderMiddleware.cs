using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace ComputerScience.Server.Web.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RemoveKestrelHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public RemoveKestrelHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Response.HasStarted)
                httpContext.Response.Headers["Server"] = StringValues.Empty;
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RemoveKestrelHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseRemoveKestrelHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RemoveKestrelHeaderMiddleware>();
        }
    }
}
