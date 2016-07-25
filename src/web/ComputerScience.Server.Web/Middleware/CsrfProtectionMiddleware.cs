using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ComputerScience.Server.Web.Extentions;
using ComputerScience.Server.Web.Models.Response;
using Microsoft.Extensions.Primitives;

namespace ComputerScience.Server.Web.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CsrfProtectionMiddleware
    {
        private readonly RequestDelegate _next;
        private string Origin { get; }
        private string Referrer { get; }

        public CsrfProtectionMiddleware(RequestDelegate next)
        {
            _next = next;
            Origin = CsrfProtectionMiddlewareExtensions.Origin;
            Referrer = CsrfProtectionMiddlewareExtensions.Referer;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                await httpContext.Response.WriteStandardResponseAsync(new StandardResponse
                {
                    Code = 400,
                    ClientError = true,
                    Developer = "Include X-Requested-With as XMLHttpRequest",
                    Information = "Headers",
                    Message = "A client error has occurred. Try contacting the developers.",
                    Succeeded = false
                });
                return;
            }
            if (Origin != null && httpContext.Request.Headers["Origin"] != StringValues.Empty
                && !httpContext.Request.Headers["Origin"].ToString().StartsWith(httpContext.Request.Scheme + "://" + Origin))
            {
                await httpContext.Response.WriteStandardResponseAsync(new StandardResponse
                {
                    Code = 400,
                    ClientError = true,
                    Developer = "Origin was not matched",
                    Information = "Headers",
                    Message = "A client error has occurred. Try contacting the developers.",
                    Succeeded = false
                });
                return;
            }
            if (Referrer != null && httpContext.Request.Headers["Referer"] != StringValues.Empty
                && !httpContext.Request.Headers["Referer"].ToString().StartsWith(httpContext.Request.Scheme + "://" + Referrer))
            {
                await httpContext.Response.WriteStandardResponseAsync(new StandardResponse
                {
                    Code = 400,
                    ClientError = true,
                    Developer = "Origin was not matched",
                    Information = "Headers",
                    Message = "A client error has occurred. Try contacting the developers.",
                    Succeeded = false
                });
                return;
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CsrfProtectionMiddlewareExtensions
    {
        internal static string Origin { get; set; }
        internal static string Referer { get; set; }
        public static IApplicationBuilder UseCsrfProtectionMiddleware(this IApplicationBuilder builder, string origin = null, string referer = null)
        {
            Origin = origin;
            Referer = referer;
            return builder.UseMiddleware<CsrfProtectionMiddleware>();
        }
    }
}
