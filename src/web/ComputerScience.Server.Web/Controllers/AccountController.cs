using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ComputerScience.Server.Web.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<User> UserManager { get; set; }

        [HttpPost]
        public string Register()
        {
            return null;
        }
    }
}
