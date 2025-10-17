using Microsoft.EntityFrameworkCore;
using EIMS.WebAPI.Models;

namespace EIMS.WebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SeasonalTest> SeasonalTests { get; set; }
        public DbSet<SportsRecord> SportsRecords { get; set; }
        public DbSet<FeeRecord> FeeRecords { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Project> Projects { get; set; } // Add Project DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}