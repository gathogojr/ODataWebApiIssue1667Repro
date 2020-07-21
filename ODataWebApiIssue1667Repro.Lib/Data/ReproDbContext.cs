using Repro.Lib.Models;
using System.Data.Entity;

namespace Repro.Lib.Data
{
    public class ReproDbContext : DbContext
    {
        public ReproDbContext(string connectionStringName) : base(connectionStringName)
        {
        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Milestone> Milestones { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
    }
}
