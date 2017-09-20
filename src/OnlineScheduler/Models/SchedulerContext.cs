using Microsoft.EntityFrameworkCore;

namespace OnlineScheduler.Models
{
    public class SchedulerContext : DbContext
    {
        public SchedulerContext(DbContextOptions options)
        : base(options)
        { }
        public SchedulerContext() { }
        public DbSet<User> Users { get; set; }
        public DbSet<Plan> Plans { get; set; }

    }
}