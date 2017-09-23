using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineScheduler.Models;
using OnlineScheduler.Repository;
using OnlineScheduler.Exceptions;

namespace OnlineScheduler.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IProvider _provider;
        public RegisterModel(IProvider provider)
        {
            _provider = provider;
        }
        [BindProperty]
        public User NewUser { get; set; }

        public IActionResult OnPost()
        {
            if ((!ModelState.IsValid))
            {
                return Page();
            }
            try
            {
                _provider.RegisterUser(NewUser.Username, NewUser.Password, NewUser.EmailAddress);
                return RedirectToPage("/Account/RegisterResult", new { username = NewUser.Username, RegisterResult = RegisterResult.Succeed });
            }
            catch (EmailAlreadyExistsException)
            {
                return Page();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToPage("/Account/RegisterResult", new { RegisterResult = RegisterResult.Fail });
            }
        }

    }
}