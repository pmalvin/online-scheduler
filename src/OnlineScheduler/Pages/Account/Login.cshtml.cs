using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineScheduler.Models;
using OnlineScheduler.Repository;

namespace OnlineScheduler.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IProvider _provider;
        public LoginModel(IProvider provider)
        {
            _provider = provider;
        }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPost()
        {
            User userToLogin = null;
            if ((userToLogin = _provider.LoginUser(Email, Password)) != null)
            {
                var claims = new Claim[] {
                    new Claim(ClaimTypes.Email, Email),
                    new Claim(ClaimTypes.Name,userToLogin.Username)
                };
                var userIdentity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync("UserCookieAuthentication", principal);
                return Redirect("/");
            }
            return Page();
        }
    }
}