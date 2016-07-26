using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Models.Requests
{
    public class RegisterViewModel
    {
        /// <summary>
        /// The username of the new user.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The password of the new user
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// The email to be used. This will be confirmed later on.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The challenge key issued to the user. This should be obtained through
        /// Google's reCaptcha service with the site key 6Ldi9SUTAAAAAKVzSuUv6agU3Rqa2O-prgn0-SpQ
        /// </summary>
        public string Challenge { get; set; }
    }
}
