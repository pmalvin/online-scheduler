using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineScheduler.Models;
using OnlineScheduler.Repository;

namespace OnlineScheduler.Pages
{
    [Route("/Create")]
    public class TempCreateModel : PageModel
    {
        private readonly IProvider provider;
        public TempCreateModel(IProvider provider)
        {
            this.provider = provider;
        }
        [BindProperty]
        public Plan NewPlan { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            var email = (User as ClaimsPrincipal).FindFirst(ClaimTypes.Email).Value;
            NewPlan.UserEmail = email;
            NewPlan.IsFinished = false;
            provider.CreatePlan(NewPlan);
            return RedirectToPage("Timetable");
        }
    }
}