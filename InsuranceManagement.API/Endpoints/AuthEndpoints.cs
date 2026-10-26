using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Application.Services;
using InsuranceManagement.Domain.Models;
using InsuranceManagement.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

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
            return TypedResults.Created("/api/auth/me", result);
        })
        .AddEndpointFilter<ValidationFilter<RegisterRequest>>()
        .AllowAnonymous()
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
        .AllowAnonymous()
        .WithName("Login")
        .WithSummary("Exchange credentials for a JWT")
        .WithDescription("Returns a short-lived signed bearer token. Invalid credentials receive a generic 401 response.")
        .Produces<AuthResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .ProducesValidationProblem();

        auth.MapGet("/me", async Task<Results<Ok<UserProfileResponse>, NotFound, UnauthorizedHttpResult>> (
            ClaimsPrincipal principal,
            IAuthService service,
            CancellationToken cancellationToken) =>
        {
            var idValue = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idValue, out var userId))
            {
                return TypedResults.Unauthorized();
            }

            var profile = await service.GetProfileAsync(userId, cancellationToken);
            return profile is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(profile);
        })
        .RequireAuthorization()
        .WithName("GetCurrentUser")
        .WithSummary("Get the authenticated account profile")
        .WithDescription("Returns the caller's account ID, email, and role without exposing password data.")
        .Produces<UserProfileResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

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
