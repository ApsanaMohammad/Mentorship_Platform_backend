using MentorshipPlatformAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace MentorshipPlatformAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for your models
        
        public DbSet<Register> Registers { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<MentorshipRequest> MentorshipRequests {  get; set; }


        // Merge the two OnModelCreating methods into one
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            // Define the primary key for Register
            modelBuilder.Entity<Register>()
                .HasKey(r => r.Id); // Primary key for Register

            


        }

        
    }
}
