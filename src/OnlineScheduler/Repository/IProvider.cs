using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineScheduler.Models;
namespace OnlineScheduler.Repository
{
    public interface IProvider
    {
        void RegisterUser(string username, string password, string email);
        User LoginUser(string email, string password);
        User GetUser(string email);
        void CreatePlan(Plan plan);
        void DeletePlan(int planId);
        void UpdatePlan(int planId, Plan newPlan);
        IEnumerable<Plan> GetPlansByUser(User owner);
        IEnumerable<Plan> GetPlansByUser(string userEmail);
        Plan GetPlan(int planId);
        void MarkFinished(int planId, bool isFinished);
        bool HasFullAccess(User user, Plan plan);
        bool HasFullAccess(string userEmail, int planId);

    }
}