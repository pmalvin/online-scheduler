using System;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineScheduler.Models;
using OnlineScheduler.Repository;
using System.Linq;

namespace OnlineScheduler.Controllers
{
    /// <summary>
    /// Route: api/plan
    /// </summary>
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

        private bool IsUserInvalid(int planId)
        {
            return !User.Identity.IsAuthenticated || !provider.HasFullAccess(CurrentUserEmail, planId);
        }

        /*
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
        */
        [HttpGet]
        public IActionResult GetPlanForDate([FromQuery] DateTimeOffset date)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var plans = provider.GetPlansForDate(CurrentUserEmail, date);
            return new ObjectResult(plans.Select(plan => new PlanDTO(plan)));
        }

        [HttpGet("{id}", Name = "GetPlan")]
        public IActionResult GetPlan(int id)
        {
            if (IsUserInvalid(id))
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
            if (IsUserInvalid(id))
            {
                return Unauthorized();
            }
            provider.UpdatePlan(id, newPlan);
            return new NoContentResult();
        }

        [HttpPatch("{id}")]
        public IActionResult MarkAsFinished(int id, [FromBody] bool isFinished)
        {
            if (IsUserInvalid(id))
            {
                return Unauthorized();
            }
            provider.MarkFinished(id, isFinished);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlan(int id)
        {
            if (IsUserInvalid(id))
            {
                return Unauthorized();
            }
            provider.DeletePlan(id);
            return new NoContentResult();
        }
    }
}
