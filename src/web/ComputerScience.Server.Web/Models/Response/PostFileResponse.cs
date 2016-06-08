using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Models.Response
{
    public enum PostFileResponse
    {
        Success,
        Failure,
        FileSizeError,
        IdentificationError,
        ProblemIdentificationError
    }
}
