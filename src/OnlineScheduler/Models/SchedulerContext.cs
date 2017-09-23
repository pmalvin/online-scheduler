using Microsoft.EntityFrameworkCore;

namespace OnlineScheduler.Models
{
    public class SchedulerContext : DbContext
    {
        public SchedulerContext(DbContextOptions<SchedulerContext> options)
        : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Plan> Plans { get; set; }

    }
}