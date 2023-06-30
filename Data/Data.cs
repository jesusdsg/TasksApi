using Microsoft.EntityFrameworkCore;
using TasksApi.Models;

namespace TasksApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        /* Migrations per model */
        public DbSet<Activity> Activities { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(e => e.Email)
            .IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}