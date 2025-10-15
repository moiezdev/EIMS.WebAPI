using Microsoft.EntityFrameworkCore;
using EIMS.WebAPI.Models;

namespace EIMS.WebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SeasonalTest> SeasonalTests { get; set; } // Add SeasonalTest DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}