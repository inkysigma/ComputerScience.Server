using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.ExceptionHandling
{
    public interface IExceptionPage
    {
        int StatusCode { get; }
        IEnumerable<KeyValuePair<string, string>> Headers { get; set; }
        void Setup(Exception exception);
        string Render();
    }
}
