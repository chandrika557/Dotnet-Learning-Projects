using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Application.CQRS;
using InsuranceManagement.Application.CQRS.Commands;
using InsuranceManagement.Application.CQRS.Queries;
using InsuranceManagement.Application.Services;
using InsuranceManagement.Domain.Models;
using InsuranceManagement.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InsuranceManagement.API.Endpoints;

/// <summary>Maps HTTP endpoints for customer management.</summary>
public static class CustomerEndpoints
{
    /// <summary>Maps the customer CRUD routes onto the application.</summary>
    public static IEndpointRouteBuilder MapCustomers(this IEndpointRouteBuilder endpoints)
    {
        var customers = endpoints.MapGroup("/api/customers")
            .WithTags("Customers")
            .RequireAuthorization(policy =>
                policy.RequireRole(UserRoles.ClaimsAdjuster, UserRoles.Administrator));

        customers.MapGet("/", async Task<Ok<PagedResponse<CustomerResponse>>> (
            string? search,
            ICustomerQueryService customerService,
            CancellationToken cancellationToken,
            int pageNumber = 1,
            int pageSize = 20,
            string? sortBy = "name",
            bool sortDescending = false) =>
        {
            var response = await customerService.SearchAsync(
                new SearchCustomersQuery(search, pageNumber, pageSize, sortBy, sortDescending),
                cancellationToken);
            return TypedResults.Ok(response);
        })
        .WithName("GetCustomers")
        .WithSummary("Search, sort, and page customers")
        .WithDescription("Searches customer name, email, or phone; sorting supports name and email.")
        .Produces<PagedResponse<CustomerResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        customers.MapGet("/{id:int}", async Task<Results<Ok<CustomerResponse>, NotFound>> (
            int id,
            ICustomerQueryService customerService,
            CancellationToken cancellationToken) =>
        {
            var customer = await customerService.GetByIdAsync(
                new GetCustomerByIdQuery(id),
                cancellationToken);
            return customer is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(customer);
        })
        .WithName("GetCustomerById")
        .WithSummary("Get a customer by ID")
        .WithDescription("Returns a customer's details, or 404 if the ID does not exist.")
        .Produces<CustomerResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        customers.MapPost("/", async Task<Created<CustomerResponse>> (
            CreateCustomerRequest request,
            ICustomerCommandService customerService,
            CancellationToken cancellationToken) =>
        {
            var customer = await customerService.CreateAsync(
                new CreateCustomerCommand(request),
                cancellationToken);
            return TypedResults.Created($"/api/customers/{customer.Id}", customer);
        })
        .AddEndpointFilter<ValidationFilter<CreateCustomerRequest>>()
        .WithName("CreateCustomer")
        .WithSummary("Create a customer")
        .WithDescription("Creates a customer and returns its resource URL.")
        .Produces<CustomerResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        customers.MapPut("/{id:int}", async Task<Results<Ok<CustomerResponse>, NotFound>> (
            int id,
            UpdateCustomerRequest request,
            ICustomerCommandService customerService,
            CancellationToken cancellationToken) =>
        {
            var customer = await customerService.UpdateAsync(
                new UpdateCustomerCommand(id, request),
                cancellationToken);
            return customer is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(customer);
        })
        .AddEndpointFilter<ValidationFilter<UpdateCustomerRequest>>()
        .WithName("UpdateCustomer")
        .WithSummary("Replace a customer's details")
        .WithDescription("Replaces all editable customer details for the specified ID.")
        .Produces<CustomerResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();

        customers.MapDelete("/{id:int}", async Task<Results<NoContent, NotFound>> (
            int id,
            ICustomerCommandService customerService,
            CancellationToken cancellationToken) =>
        {
            var deleted = await customerService.DeleteAsync(
                new DeleteCustomerCommand(id),
                cancellationToken);
            return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteCustomer")
        .WithSummary("Delete a customer")
        .WithDescription("Deletes the specified customer if it exists.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
