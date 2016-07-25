using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ComputerScience.Server.Web.Controllers
{
    public class TestController : Controller
    {
        [HttpPost]
        public string Reflect(ReflectViewModel model)
        {
            return model.Input;
        }
    }
}
