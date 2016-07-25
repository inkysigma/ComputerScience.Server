using System.Text;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Models.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ComputerScience.Server.Web.Extentions
{
    public static class MiddlewareResponseExtensions
    {
        public static async Task WriteStandardResponseAsync(this HttpResponse context, StandardResponse data)
        {
            if (context.HasStarted || !context.Body.CanWrite)
                return;
            var response = context;
            response.StatusCode = data.Code;
            response.Headers.Add("Content-Type", "text/json");
            var serialized = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(serialized);
            await response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
