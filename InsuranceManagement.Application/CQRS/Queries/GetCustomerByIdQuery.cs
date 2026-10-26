namespace InsuranceManagement.Application.CQRS.Queries;

/// <summary>Read request for one customer.</summary>
/// <param name="CustomerId">Identifier of the customer to retrieve.</param>
public sealed record GetCustomerByIdQuery(int CustomerId);
