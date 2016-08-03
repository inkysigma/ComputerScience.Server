using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Models.Requests
{
    public class ActivateAccountViewModel
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
