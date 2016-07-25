using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using ComputerScience.Server.Web.Extentions;
using ComputerScience.Server.Web.Models.Response;

namespace ComputerScience.Server.Web.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RequireUtfMiddleware
    {
        private readonly RequestDelegate _next;

        public RequireUtfMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers["charset"] != StringValues.Empty && httpContext.Request.Headers["charset"] != "utf-8")
            {
                return httpContext.Response.WriteStandardResponseAsync(new StandardResponse
                {
                    Code = 400,
                    ClientError = true,
                    Developer = "Please use UTF-8",
                    Information = "charset",
                    Message = "Something horrible has occurred. Please contact the developers",
                    Succeeded = false
                });
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequireUtfMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequireUtfMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequireUtfMiddleware>();
        }
    }
}
