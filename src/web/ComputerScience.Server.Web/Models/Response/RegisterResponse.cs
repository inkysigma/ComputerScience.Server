using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Models.Response
{
    public class RegisterResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
