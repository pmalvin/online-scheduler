using System;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineScheduler.Models;
using OnlineScheduler.Repository;
using System.Linq;

namespace OnlineScheduler.Controllers
{
    [Route("api/[controller]")]
    public class PlanController : Controller
    {
        private readonly IProvider provider;
        public PlanController(IProvider provider)
        {
            this.provider = provider;
        }

        private string CurrentUserEmail
        {
            get
            {
                return (User as ClaimsPrincipal).FindFirst(ClaimTypes.Email).Value;
            }
        }

        [HttpGet]
        public IActionResult GetPlans()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var plans = provider.GetPlansByUser(CurrentUserEmail);
            return new ObjectResult(plans);
        }

        [HttpGet("{id}", Name = "GetPlan")]
        public IActionResult GetPlan(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (!provider.HasFullAccess(CurrentUserEmail, id))
            {
                return Unauthorized();
            }
            return new ObjectResult(provider.GetPlan(id));
        }

        [HttpPost]
        public IActionResult AddPlan([FromBody] Plan plan)
        {
            if (plan == null)
            {
                return BadRequest();
            }
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            plan.UserEmail = CurrentUserEmail;
            provider.CreatePlan(plan);
            return CreatedAtRoute("GetPlan", plan);
        }
        [HttpPut("{id}")]
        public IActionResult UpdatePlan(int id, [FromBody] Plan newPlan)
        {
            if (newPlan == null)
            {
                return BadRequest();
            }
            if (!User.Identity.IsAuthenticated || !provider.HasFullAccess(CurrentUserEmail, id))
            {
                return Unauthorized();
            }
            provider.UpdatePlan(id, newPlan);
            return new NoContentResult();
        }

        [HttpPatch("{id}")]
        public IActionResult MarkAsFinished(int id, [FromBody] bool isFinished)
        {
            if (!User.Identity.IsAuthenticated || !provider.HasFullAccess(CurrentUserEmail, id))
            {
                return Unauthorized();
            }
            provider.MarkFinished(id, isFinished);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlan(int id)
        {
            if (NewMethod(id))
            {
                return Unauthorized();
            }
            provider.DeletePlan(id);
            return new NoContentResult();
        }

        private bool NewMethod(int id)
        {
            return !User.Identity.IsAuthenticated || !provider.HasFullAccess(CurrentUserEmail, id);
        }
    }
}
