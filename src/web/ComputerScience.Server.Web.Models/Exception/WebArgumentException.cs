using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerScience.Server.Common;

namespace ComputerScience.Server.Web.Models.Exception
{
    public class WebArgumentException : CommonException
    {
        public WebArgumentException(string parameter, string api)
            : base(
                403, true, "The client appears to have not worked.", parameter,
                $"Please look at the API for {api} to determine the correct parameters")
        {

        }
    }
}
