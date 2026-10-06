using InsuranceManagement.API.Models; 
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagement.API.Data;

/// <summary>Provides EF Core access to the insurance API database.</summary>
public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    /// <summary>Gets the customers table.</summary>
    public DbSet<Customer> Customers => Set<Customer>();

    /// <summary>Gets the insurance policies table.</summary>
    public DbSet<Policy> Policies => Set<Policy>();

    /// <summary>Gets the insurance claims table.</summary>
    public DbSet<Claim> Claims => Set<Claim>();

    /// <summary>Gets the user accounts table.</summary>
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

    /// <summary>Configures the insurance entities and their database constraints.</summary>
    /// <param name="modelBuilder">The builder used to configure the entity model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(customer =>
        {
            customer.Property(entity => entity.Name)
                .IsRequired()
                .HasMaxLength(150);

            customer.Property(entity => entity.Email)
                .HasMaxLength(254);

            customer.Property(entity => entity.PhoneNumber)
                .HasMaxLength(32);

            customer.Property(entity => entity.Address)
                .HasMaxLength(500);
        });

        modelBuilder.Entity<Policy>(policy =>
        {
            policy.Property(entity => entity.PolicyName)
                .IsRequired()
                .HasMaxLength(150);
            policy.Property(entity => entity.CoverageAmount).HasPrecision(18, 2);
            policy.Property(entity => entity.PremiumAmount).HasPrecision(18, 2);
            policy.HasOne(entity => entity.Customer)
                .WithMany(customer => customer.Policies)
                .HasForeignKey(entity => entity.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Claim>(claim =>
        {
            claim.Property(entity => entity.ClaimAmount).HasPrecision(18, 2);
            claim.Property(entity => entity.Reason)
                .IsRequired()
                .HasMaxLength(1000);
            claim.HasOne(entity => entity.Policy)
                .WithMany(policy => policy.Claims)
                .HasForeignKey(entity => entity.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserAccount>(user =>
        {
            user.Property(entity => entity.Email)
                .IsRequired()
                .HasMaxLength(254);
            user.Property(entity => entity.PasswordHash).IsRequired();
            user.Property(entity => entity.Role)
                .IsRequired()
                .HasMaxLength(64);
            user.HasIndex(entity => entity.Email).IsUnique();
        });
    }
}
