using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineScheduler.Models;
namespace OnlineScheduler.BasicOperation
{
    public interface IProvider
    {
        void RegisterUser(string username, string password, string email);
        User GetUser(string email);
        void CreatePlan(User owner, string name, DateTimeOffset start, DateTimeOffset due, bool autoFinish, string description = "");
        void DeletePlan(int planId);
        void UpdatePlan(int planId, Plan newPlan);
        IEnumerable<Plan> GetPlansByUser(User owner);
        IEnumerable<Plan> GetPlansByUser(string userEmail);
        Plan GetPlan(int planId);
        void MarkAsFinished(int planId);
        void MarkAsUnFinished(int planId);
    }
}