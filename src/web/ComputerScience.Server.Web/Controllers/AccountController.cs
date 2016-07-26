using System.Collections.Generic;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Models;
using ComputerScience.Server.Web.Models.Exception;
using ComputerScience.Server.Web.Models.Requests;
using ComputerScience.Server.Web.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Threading;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ComputerScience.Server.Web.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<User> UserManager { get; set; }
        public SignInManager<User> SignInManager { get; set; }

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HttpPost]
        public async Task<RegisterResponse> Register([FromBody] RegisterViewModel model)
        {
            if (model == null)
                throw new WebArgumentException(nameof(model), nameof(Register), null);
            if (string.IsNullOrEmpty(model.UserName))
                throw new WebArgumentException(nameof(model.UserName), nameof(Register), null);
            if (string.IsNullOrEmpty(model.Email))
                throw new WebArgumentException(nameof(model.Email), nameof(Register), null);
            if (string.IsNullOrEmpty(model.Name))
                throw new WebArgumentException(nameof(model.Name), nameof(Register), null);
            if (string.IsNullOrEmpty(model.Password))
                throw new WebArgumentException(nameof(model.Password), nameof(Register), null);
            var user = new User()
            {
                Email = model.Email,
                UserName = model.UserName,
                Name = model.Name
            };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                IEnumerable<string> errors = result.Errors.Select(f => f.Description);
                return new RegisterResponse
                {
                    Errors = errors,
                    Succeeded = false
                };
            }
            return new RegisterResponse
            {
                Succeeded = true
            };
        }

        [HttpPost]
        public async Task<string> RequestRegister(CancellationToken cancellationToken)
        {
            var request = new HttpClient();
            var response = await request.PostAsync();
        }

        [HttpPost]
        public async Task<LoginResponse> Login([FromBody] LoginViewModel model)
        {
            if (model == null)
                throw new WebArgumentException(nameof(model), "Login", null);
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, false, true);
            if (!result.Succeeded)
            {
                return new LoginResponse
                {
                    LockedOut = result.IsLockedOut,
                    CanLogin = !result.IsNotAllowed,
                    RequiresTwoFactor = result.RequiresTwoFactor,
                    Succeeded = result.Succeeded
                };
            }
            return new LoginResponse
            {
                Succeeded = true
            };
        }
    }
}
