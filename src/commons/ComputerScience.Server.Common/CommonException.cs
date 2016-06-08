using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Common
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class CommonException : Exception
    {
        public bool ClientError { get; }

        public int Code { get; }

        public string Information { get; }

        public string Developer { get; }

        public CommonException(int code, bool clientError, string message, string information, string developer) : base(message)
        {
            Code = code;
            ClientError = clientError;
            Information = information;
            Developer = developer;
        }
    }
}
