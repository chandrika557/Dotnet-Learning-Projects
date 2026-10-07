using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;
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
            ICustomerService customerService,
            CancellationToken cancellationToken,
            int pageNumber = 1,
            int pageSize = 20,
            string? sortBy = "name",
            bool sortDescending = false) =>
        {
            var response = await customerService.SearchAsync(
                new CustomerSearchOptions(search, pageNumber, pageSize, sortBy, sortDescending),
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
            ICustomerService customerService,
            CancellationToken cancellationToken) =>
        {
            var customer = await customerService.GetByIdAsync(id, cancellationToken);
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
            ICustomerService customerService,
            CancellationToken cancellationToken) =>
        {
            var customer = await customerService.CreateAsync(request, cancellationToken);
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
            ICustomerService customerService,
            CancellationToken cancellationToken) =>
        {
            var customer = await customerService.UpdateAsync(id, request, cancellationToken);
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
            ICustomerService customerService,
            CancellationToken cancellationToken) =>
        {
            var deleted = await customerService.DeleteAsync(id, cancellationToken);
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
