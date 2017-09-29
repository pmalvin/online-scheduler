using System;
namespace OnlineScheduler.Models
{
    public class PlanDTO
    {
        public PlanDTO(Plan plan)
        {
            PlanId = plan.PlanId;
            StartTime = plan.StartTime;
            DueTime = plan.DueTime;
            PlanName = plan.PlanName;
            Description = plan.Description;
            AutoFinish = plan.AutoFinish;
            IsFinished = plan.IsFinished;
        }
        public int PlanId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset DueTime { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public bool AutoFinish { get; set; }
        public bool IsFinished { get; set; }
    }
}