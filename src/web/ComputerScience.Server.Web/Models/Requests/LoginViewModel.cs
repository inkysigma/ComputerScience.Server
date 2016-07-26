using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Models.Requests
{
    /// <summary>
    /// The view model to login
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// The requested username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The requested password
        /// </summary>
        public string Password { get; set; }
    }
}
