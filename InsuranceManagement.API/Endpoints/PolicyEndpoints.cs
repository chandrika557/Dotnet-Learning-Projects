using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;
using InsuranceManagement.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InsuranceManagement.API.Endpoints;

/// <summary>Maps HTTP endpoints for insurance policies.</summary>
public static class PolicyEndpoints
{
    /// <summary>Maps policy routes onto the application.</summary>
    public static IEndpointRouteBuilder MapPolicies(this IEndpointRouteBuilder endpoints)
    {
        var policies = endpoints.MapGroup("/api/policies")
            .WithTags("Policies")
            .RequireAuthorization(policy =>
                policy.RequireRole(UserRoles.ClaimsAdjuster, UserRoles.Administrator));

        policies.MapGet("/", async Task<Ok<IReadOnlyList<PolicyResponse>>> (
            IPolicyService service,
            CancellationToken cancellationToken) =>
                TypedResults.Ok(await service.GetAllAsync(cancellationToken)))
            .WithName("GetPolicies")
            .WithSummary("Get all policies")
            .WithDescription("Returns policies ordered by policy name.")
            .Produces<IReadOnlyList<PolicyResponse>>(StatusCodes.Status200OK);

        policies.MapGet("/{id:int}", async Task<Results<Ok<PolicyResponse>, NotFound>> (
            int id,
            IPolicyService service,
            CancellationToken cancellationToken) =>
        {
            var policy = await service.GetByIdAsync(id, cancellationToken);
            return policy is null ? TypedResults.NotFound() : TypedResults.Ok(policy);
        })
        .WithName("GetPolicyById")
        .WithSummary("Get a policy by ID")
        .WithDescription("Returns a policy or 404 when it does not exist.")
        .Produces<PolicyResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        policies.MapPost("/", async Task<Created<PolicyResponse>> (
            CreatePolicyRequest request,
            IPolicyService service,
            CancellationToken cancellationToken) =>
        {
            var policy = await service.CreateAsync(request, cancellationToken);
            return TypedResults.Created($"/api/policies/{policy.PolicyId}", policy);
        })
        .AddEndpointFilter<ValidationFilter<CreatePolicyRequest>>()
        .WithName("CreatePolicy")
        .WithSummary("Issue a policy")
        .WithDescription("Creates an insurance policy for an existing customer.")
        .Produces<PolicyResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        policies.MapPut("/{id:int}", async Task<Results<Ok<PolicyResponse>, NotFound>> (
            int id,
            UpdatePolicyRequest request,
            IPolicyService service,
            CancellationToken cancellationToken) =>
        {
            var policy = await service.UpdateAsync(id, request, cancellationToken);
            return policy is null ? TypedResults.NotFound() : TypedResults.Ok(policy);
        })
        .AddEndpointFilter<ValidationFilter<UpdatePolicyRequest>>()
        .WithName("UpdatePolicy")
        .WithSummary("Replace a policy")
        .WithDescription("Replaces policy details for an existing policy.")
        .Produces<PolicyResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        policies.MapDelete("/{id:int}", async Task<Results<NoContent, NotFound>> (
            int id,
            IPolicyService service,
            CancellationToken cancellationToken) =>
        {
            var deleted = await service.DeleteAsync(id, cancellationToken);
            return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeletePolicy")
        .WithSummary("Delete a policy")
        .WithDescription("Deletes a policy only when it has no associated claims.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }
}
