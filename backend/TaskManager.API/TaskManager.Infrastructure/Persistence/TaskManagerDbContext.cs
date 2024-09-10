using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options)
        {

        }

        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Domain.Entities.TaskProject> Tasks { get; set; }
        public DbSet<TimeTracker> TimeTrackers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public void Seed()
        {
            if (!Users.Any())
            {
               var users = new List<User>();
                users.Add(new User("admin", "admin"));

                Users.AddRange(users);
                SaveChanges();
            }
        }
    }
}
