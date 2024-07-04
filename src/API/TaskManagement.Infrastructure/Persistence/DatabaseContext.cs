using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagement.Domain.Entities;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace TaskManagement.Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<TaskDb> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

                var root = builder.Build();
                var connectString = root.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskDb>().
                Property(e => e.TaskStatus)
                .HasDefaultValue(TaskStatusDb.ToDo)
                .HasConversion<string>();

            modelBuilder.Entity<TaskDb>().
                Property(e => e.Priority)
                .HasDefaultValue(Priority.Low)
                .HasConversion<string>();
        }
    }
}
