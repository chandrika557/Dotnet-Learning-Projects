using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } // Constructor that accepts DbContextOptions and passes them to the base DbContext class

    public DbSet<Author> Authors => Set<Author>(); // Represents the Authors table in the database
    public DbSet<Book> Books => Set<Book>(); // Represents the Books table in the database
    public DbSet<User> Users => Set<User>(); // Represents the Users table in the database

    protected override void OnModelCreating(ModelBuilder modelBuilder) // Configure the model relationships and constraints
    {
        modelBuilder.Entity<Author>() // Configure the Author entity
            .Property(a => a.Name).IsRequired().HasMaxLength(100); // Set the Name property as required with a maximum length of 100 characters

        modelBuilder.Entity<User>(u =>
{
    u.Property(x => x.Email).IsRequired().HasMaxLength(200);
    u.HasIndex(x => x.Email).IsUnique();
});    

        modelBuilder.Entity<Book>(b => // Configure the Book entity
        {
            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.Price).HasPrecision(10, 2);
            b.HasOne(x => x.Author)
             .WithMany(a => a.Books)
             .HasForeignKey(x => x.AuthorId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}