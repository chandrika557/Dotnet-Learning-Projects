using InsuranceManagement.MVC.Models;
using Microsoft.EntityFrameworkCore;    

namespace InsuranceManagement.MVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Policy>()
                .Property(policy => policy.CoverageAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Policy>()
                .Property(policy => policy.PremiumAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Claim>()
                .Property(claim => claim.ClaimAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Policy>()
                .HasOne(policy => policy.Customer)
                .WithMany(customer => customer.Policies)
                .HasForeignKey(policy => policy.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne(claim => claim.Policy)
                .WithMany(policy => policy.Claims)
                .HasForeignKey(claim => claim.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}