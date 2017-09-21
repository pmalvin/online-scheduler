using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineScheduler.Models
{
    public class Plan
    {
        [Key]
        public int PlanId { get; set; }
        [ForeignKey("Owner")]
        public string UserEmail { get; set; }
        public User Owner { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset DueTime { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public bool AutoFinish { get; set; }
        public bool IsFinished { get; set; }
    }
}