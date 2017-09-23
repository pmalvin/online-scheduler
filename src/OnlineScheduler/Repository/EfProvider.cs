using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineScheduler.Models;
using OnlineScheduler.Exceptions;
namespace OnlineScheduler.Repository
{
    public class EfProvider : IProvider
    {
        private readonly SchedulerContext context;
        public EfProvider(SchedulerContext context)
        {
            this.context = context;
        }
        public void CreatePlan(User owner, string name, DateTimeOffset start, DateTimeOffset due, bool autoFinish, string description = "")
        {
            Plan newPlan = new Plan
            {
                UserEmail = owner.EmailAddress,
                StartTime = start,
                DueTime = due,
                PlanName = name,
                Description = description,
                AutoFinish = autoFinish
            };
            newPlan.IsFinished = false;
            if (autoFinish && DateTimeOffset.Compare(DateTimeOffset.UtcNow, due) >= 0)
            {
                newPlan.IsFinished = true;
            }
            context.Plans.Add(newPlan);
            context.SaveChanges();
        }

        public void DeletePlan(int planId)
        {
            Plan toDel = context.Plans.SingleOrDefault(plan => plan.PlanId == planId);
            context.Plans.Remove(toDel);
            context.SaveChanges();
        }

        public Plan GetPlan(int planId)
        {
            return context.Plans.SingleOrDefault(plan => plan.PlanId == planId);
        }

        public IEnumerable<Plan> GetPlansByUser(User owner)
        {
            return context.Plans.Where(plan => plan.UserEmail.Equals(owner.EmailAddress));
        }

        public IEnumerable<Plan> GetPlansByUser(string userEmail)
        {
            return context.Plans.Where(plan => plan.UserEmail.Equals(userEmail));
        }

        public User GetUser(string email)
        {
            return context.Users.SingleOrDefault(user => user.EmailAddress.ToLower().Equals(email.ToLower()));
        }

        /// <summary>
        /// Used when logging in
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>If user exists then return the user, otherwise return null</returns>
        public User LoginUser(string email, string password)
        {
            return context.Users.SingleOrDefault(user => user.EmailAddress.ToLower().Equals(email.ToLower())
                && user.Password == password);
        }

        public void MarkAsFinished(int planId)
        {
            var target = context.Plans.SingleOrDefault(plan => plan.PlanId == planId);
            target.IsFinished = true;
            context.SaveChanges();
        }

        public void MarkAsUnFinished(int planId)
        {
            var target = context.Plans.SingleOrDefault(plan => plan.PlanId == planId);
            target.IsFinished = false;
            context.SaveChanges();
        }

        public void RegisterUser(string username, string password, string email)
        {
            if (context.Users.Any(user => user.EmailAddress.ToLower().Equals(email.ToLower())))
            {
                throw new EmailAlreadyExistsException();
            }
            User newUser = new User
            {
                EmailAddress = email,
                Username = username,
                Password = password,
                RegisterDate = DateTimeOffset.UtcNow
            };
            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public void UpdatePlan(int planId, Plan newPlan)
        {
            Plan planInDb = context.Plans.SingleOrDefault(plan => plan.PlanId == planId);
            if (planInDb == null)
            {
                throw new PlanNotExistException();
            }
            else
            {
                planInDb.StartTime = newPlan.StartTime;
                planInDb.DueTime = newPlan.DueTime;
                planInDb.PlanName = newPlan.PlanName;
                planInDb.Description = newPlan.Description;
            }
            context.SaveChanges();
        }
    }
}