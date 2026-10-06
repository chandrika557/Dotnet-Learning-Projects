using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;
using InsuranceManagement.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InsuranceManagement.API.Endpoints;

/// <summary>Maps HTTP endpoints for insurance claims.</summary>
public static class ClaimEndpoints
{
    /// <summary>Maps claim routes onto the application.</summary>
    public static IEndpointRouteBuilder MapClaims(this IEndpointRouteBuilder endpoints)
    {
        var claims = endpoints.MapGroup("/api/claims")
            .WithTags("Claims")
            .RequireAuthorization(policy =>
                policy.RequireRole(UserRoles.ClaimsAdjuster, UserRoles.Administrator));

        claims.MapGet("/", async Task<Ok<IReadOnlyList<ClaimResponse>>> (
            IClaimService service,
            CancellationToken cancellationToken) =>
                TypedResults.Ok(await service.GetAllAsync(cancellationToken)))
            .WithName("GetClaims")
            .WithSummary("Get all claims")
            .WithDescription("Returns claims ordered by submission date.")
            .Produces<IReadOnlyList<ClaimResponse>>(StatusCodes.Status200OK);

        claims.MapGet("/{id:int}", async Task<Results<Ok<ClaimResponse>, NotFound>> (
            int id,
            IClaimService service,
            CancellationToken cancellationToken) =>
        {
            var claim = await service.GetByIdAsync(id, cancellationToken);
            return claim is null ? TypedResults.NotFound() : TypedResults.Ok(claim);
        })
        .WithName("GetClaimById")
        .WithSummary("Get a claim by ID")
        .WithDescription("Returns a claim or 404 when it does not exist.")
        .Produces<ClaimResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        claims.MapPost("/", async Task<Created<ClaimResponse>> (
            CreateClaimRequest request,
            IClaimService service,
            CancellationToken cancellationToken) =>
        {
            var claim = await service.CreateAsync(request, cancellationToken);
            return TypedResults.Created($"/api/claims/{claim.ClaimId}", claim);
        })
        .AddEndpointFilter<ValidationFilter<CreateClaimRequest>>()
        .WithName("CreateClaim")
        .WithSummary("Submit a claim")
        .WithDescription("Creates a pending claim against an existing policy.")
        .Produces<ClaimResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        claims.MapPut("/{id:int}/status", async Task<Results<Ok<ClaimResponse>, NotFound>> (
            int id,
            UpdateClaimStatusRequest request,
            IClaimService service,
            CancellationToken cancellationToken) =>
        {
            var claim = await service.UpdateStatusAsync(id, request, cancellationToken);
            return claim is null ? TypedResults.NotFound() : TypedResults.Ok(claim);
        })
        .AddEndpointFilter<ValidationFilter<UpdateClaimStatusRequest>>()
        .RequireAuthorization(policy => policy.RequireRole(UserRoles.ClaimsAdjuster, UserRoles.Administrator))
        .WithName("ReviewClaim")
        .WithSummary("Approve or reject a claim")
        .WithDescription("Only a pending claim can be reviewed by an adjuster or administrator.")
        .Produces<ClaimResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }
}
