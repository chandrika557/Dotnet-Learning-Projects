using AutoMapper;
using FluentValidation;
using InsuranceManagement.Application.CQRS;
using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Application.Services;
using InsuranceManagement.Application.Services.Mapping;
using InsuranceManagement.Application.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceManagement.Application;

/// <summary>Registers application services, validators, mappings, and use cases.</summary>
public static class DependencyInjection
{
    /// <summary>Adds insurance application dependencies to the container.</summary>
    /// <param name="services">Application service collection.</param>
    /// <param name="configuration">Configuration used for AutoMapper licensing.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddInsuranceApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ICustomerQueryService, CustomerQueryService>();
        services.AddScoped<ICustomerCommandService, CustomerCommandService>();
        services.AddScoped<IPolicyService, PolicyService>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasher<InsuranceManagement.Domain.Models.UserAccount>,
            PasswordHasher<InsuranceManagement.Domain.Models.UserAccount>>();

        services.AddAutoMapper(
            options => options.LicenseKey = configuration["AutoMapper:LicenseKey"],
            typeof(InsuranceMappingProfile).Assembly);

        services.AddScoped<IValidator<CreateCustomerRequest>, CreateCustomerRequestValidator>();
        services.AddScoped<IValidator<UpdateCustomerRequest>, UpdateCustomerRequestValidator>();
        services.AddScoped<IValidator<CreatePolicyRequest>, CreatePolicyRequestValidator>();
        services.AddScoped<IValidator<UpdatePolicyRequest>, UpdatePolicyRequestValidator>();
        services.AddScoped<IValidator<CreateClaimRequest>, CreateClaimRequestValidator>();
        services.AddScoped<IValidator<UpdateClaimStatusRequest>, UpdateClaimStatusRequestValidator>();
        services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddScoped<IValidator<UpdateUserRoleRequest>, UpdateUserRoleRequestValidator>();

        return services;
    }
}
