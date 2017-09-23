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
        public void CreatePlan(User owner, string name, DateTimeOffset start, DateTimeOffset due, bool autoFinish, string description = "")
        {
            using (var context = new SchedulerContext())
            {
                Plan newPlan = new Plan()
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
        }

        public void DeletePlan(int planId)
        {
            using (var context = new SchedulerContext())
            {
                Plan toDel = context.Plans.SingleOrDefault(plan => plan.PlanId == planId);
                context.Plans.Remove(toDel);
                context.SaveChanges();
            }
        }

        public Plan GetPlan(int planId)
        {
            using (var context = new SchedulerContext())
            {
                return context.Plans.SingleOrDefault(plan => plan.PlanId == planId);
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
                return context.Users.SingleOrDefault(user => user.EmailAddress.Equals(email));
            }
        }

        public void MarkAsFinished(int planId)
        {
            using (var context = new SchedulerContext())
            {
                var target = context.Plans.SingleOrDefault(plan => plan.PlanId == planId);
                target.IsFinished = true;
                context.SaveChanges();
            }
        }

        public void MarkAsUnFinished(int planId)
        {
            using (var context = new SchedulerContext())
            {
                var target = context.Plans.SingleOrDefault(plan => plan.PlanId == planId);
                target.IsFinished = false;
                context.SaveChanges();
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
}