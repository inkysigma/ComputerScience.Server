using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComputerScience.Server.Web.Business
{
    public class UserContext : IdentityDbContext<User>
    {
        public UserContext(DbContextOptions options) : base (options) { }
    }
}
