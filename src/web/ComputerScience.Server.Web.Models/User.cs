using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ComputerScience.Server.Web.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
