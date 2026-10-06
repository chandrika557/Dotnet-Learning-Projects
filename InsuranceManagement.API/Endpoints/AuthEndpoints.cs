using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;
using InsuranceManagement.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InsuranceManagement.API.Endpoints;

/// <summary>Maps public account and administrator role-management endpoints.</summary>
public static class AuthEndpoints
{
    /// <summary>Maps registration, login, and role-management routes.</summary>
    public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder endpoints)
    {
        var auth = endpoints.MapGroup("/api/auth").WithTags("Authentication");

        auth.MapPost("/register", async Task<Created<AuthResponse>> (
            RegisterRequest request,
            IAuthService service,
            CancellationToken cancellationToken) =>
        {
            var result = await service.RegisterAsync(request, cancellationToken);
            return TypedResults.Created("/api/auth/login", result);
        })
        .AddEndpointFilter<ValidationFilter<RegisterRequest>>()
        .WithName("Register")
        .WithSummary("Register a customer account")
        .WithDescription("Creates an account with the Customer role. This endpoint never accepts a role from the caller.")
        .Produces<AuthResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict);

        auth.MapPost("/login", async Task<Results<Ok<AuthResponse>, UnauthorizedHttpResult>> (
            LoginRequest request,
            IAuthService service,
            CancellationToken cancellationToken) =>
        {
            var result = await service.LoginAsync(request, cancellationToken);
            return result is null
                ? TypedResults.Unauthorized()
                : TypedResults.Ok(result);
        })
        .AddEndpointFilter<ValidationFilter<LoginRequest>>()
        .WithName("Login")
        .WithSummary("Exchange credentials for a JWT")
        .WithDescription("Returns a short-lived signed bearer token. Invalid credentials receive a generic 401 response.")
        .Produces<AuthResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .ProducesValidationProblem();

        auth.MapPut("/users/{id:int}/role", async Task<Results<NoContent, NotFound>> (
            int id,
            UpdateUserRoleRequest request,
            IAuthService service,
            CancellationToken cancellationToken) =>
        {
            var updated = await service.UpdateRoleAsync(id, request.Role, cancellationToken);
            return updated ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .AddEndpointFilter<ValidationFilter<UpdateUserRoleRequest>>()
        .RequireAuthorization(policy => policy.RequireRole(UserRoles.Administrator))
        .WithName("UpdateUserRole")
        .WithSummary("Change an account role")
        .WithDescription("Administrator-only operation for provisioning claims adjusters and administrators.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        return endpoints;
    }
}
