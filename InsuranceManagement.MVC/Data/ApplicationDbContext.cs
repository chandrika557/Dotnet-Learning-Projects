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
    }
}