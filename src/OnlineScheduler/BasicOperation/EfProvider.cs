using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineScheduler.Models;
using OnlineScheduler.Exceptions;
namespace OnlineScheduler.BasicOperation
{
    public class EfProvider : IProvider
    {
        public void CreatePlan(User owner, string name, DateTimeOffset start, DateTimeOffset due, string description = "")
        {
            using (var context = new SchedulerContext())
            {
                Plan newPlan = new Plan()
                {
                    UserEmail = owner.EmailAddress,
                    StartTime = start,
                    DueTime = due,
                    PlanName = name,
                    Description = description
                };
                context.Plans.Add(newPlan);
                context.SaveChanges();
            }
        }

        public void DeletePlan(int planId)
        {
            using (var context = new SchedulerContext())
            {
                Plan toDel = context.Plans.Single(plan => plan.PlanId == planId);
                context.Plans.Remove(toDel);
                context.SaveChanges();
            }
        }

        public Plan GetPlan(int planId)
        {
            using (var context = new SchedulerContext())
            {
                return context.Plans.Single(plan => plan.PlanId == planId);
            }
        }

        public IEnumerable<Plan> GetPlansByUser(User owner)
        {
            using (var context = new SchedulerContext())
            {
                return context.Plans.Where(plan => plan.UserEmail.Equals(owner.EmailAddress));
            }
        }

        public IEnumerable<Plan> GetPlansByUser(string userEmail)
        {
            using (var context = new SchedulerContext())
            {
                return context.Plans.Where(plan => plan.UserEmail.Equals(userEmail));
            }
        }

        public User GetUser(string email)
        {
            using (var context = new SchedulerContext())
            {
                return context.Users.Single(user => user.EmailAddress.Equals(email));
            }
        }

        public void RegisterUser(string username, string password, string email)
        {
            using (var context = new SchedulerContext())
            {
                if (context.Users.Any(user => user.EmailAddress.Equals(email)))
                {
                    throw new EmailAlreadyExistsException();
                }
                User newUser = new User()
                {
                    EmailAddress = email,
                    Username = username,
                    Password = password
                };
                context.Users.Add(newUser);
                context.SaveChanges();
            }
        }

        public void UpdatePlan(int planId, Plan newPlan)
        {
            using (var context = new SchedulerContext())
            {
                Plan planInDb = context.Plans.Single(plan => plan.PlanId == planId);
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
}