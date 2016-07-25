using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Models.Response
{
    public class LoginResponse
    {
        public bool Succeeded { get; set; }
        public bool LockedOut { get; set; }
        public bool RequiresTwoFactor { get; set; }
        public bool CanLogin { get; set; }
    }
}
