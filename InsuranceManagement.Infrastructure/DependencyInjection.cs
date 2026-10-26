using InsuranceManagement.Infrastructure.Data;
using InsuranceManagement.Infrastructure.Repositories;
using InsuranceManagement.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceManagement.Infrastructure;

/// <summary>Registers SQL Server persistence and repository implementations.</summary>
public static class DependencyInjection
{
    /// <summary>Adds infrastructure dependencies to the application container.</summary>
    /// <param name="services">Application service collection.</param>
    /// <param name="connectionString">SQL Server connection string.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddInsuranceInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlServer => sqlServer.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IPolicyRepository, PolicyRepository>();
        services.AddScoped<IClaimRepository, ClaimRepository>();
        services.AddScoped<IUserAccountRepository, UserAccountRepository>();

        return services;
    }
}
