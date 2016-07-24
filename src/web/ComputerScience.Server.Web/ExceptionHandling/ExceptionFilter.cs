using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ComputerScience.Server.Web.ExceptionHandling
{
    public class ExceptionFilter : IExceptionFilter
    {
        public ILogger<ExceptionFilter> Logger { get; }

        public IExceptionPage Page { get; }

        public ExceptionFilter(ILogger<ExceptionFilter> logger, IExceptionPage page)
        {
            Logger = logger;
            Page = page;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.HttpContext.Response.HasStarted || !context.HttpContext.Response.Body.CanWrite)
                return;
            var response = context.HttpContext.Response;
            var exception = context.Exception;
            Page.Setup(exception);
            var result = Encoding.UTF8.GetBytes(Page.Render());
            response.StatusCode = Page.StatusCode;
            foreach (var i in Page.Headers)
            {
                if (response.Headers.ContainsKey(i.Key))
                    continue;
                response.Headers[i.Key] = i.Value;
            }
            response.Body.Write(result, 0, result.Length);
            response.Body.Flush();
            Logger.LogError(Page.StatusCode, exception, exception.Message);
            context.ExceptionHandled = true;
        }
    }
}
