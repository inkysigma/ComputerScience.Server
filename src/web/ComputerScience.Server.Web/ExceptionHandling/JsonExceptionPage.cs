using System;
using System.Collections.Generic;
using ComputerScience.Server.Common;
using ComputerScience.Server.Web.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ComputerScience.Server.Web.ExceptionHandling
{
    public class JsonExceptionPage : IExceptionPage
    {
        public int StatusCode { get; private set; } = 503;
        public IEnumerable<KeyValuePair<string, string>> Headers { get; set; }
        private Exception Exception { get; set; }
        private ILogger<IExceptionPage> Logger { get; }
        private bool IsClientError { get; set; } = false;
        private string Developer { get; set; } = null;
        private string Message { get; set; } = "Something bad happened";
        private string Information { get; set; }

        public JsonExceptionPage(ILogger<IExceptionPage> logger)
        {
            Logger = logger;
        }

        public void Setup(Exception exception)
        {
            Exception = exception;
            var commonException = exception as CommonException;
            if (commonException != null)
            {
                var e = commonException;
                StatusCode = 200;
                IsClientError = e.ClientError;
                Developer = e.Developer;
                Message = e.Message;
                Information = e.Information;
            }
            Headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", "application/json")
            };
        }

        public string Render()
        {
            var response = new StandardResponse
            {
                Code = StatusCode,
                ClientError = IsClientError,
                Developer = Developer,
                Information = Information,
                Message = Message,
                Succeeded = false
            };
            Logger.LogError(StatusCode, Exception, Exception.Message);
            return JsonConvert.SerializeObject(response);
        }
    }
}
