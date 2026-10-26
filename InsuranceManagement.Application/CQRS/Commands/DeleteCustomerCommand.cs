namespace InsuranceManagement.Application.CQRS.Commands;

/// <summary>Write request for deleting a customer.</summary>
/// <param name="CustomerId">Identifier of the customer to delete.</param>
public sealed record DeleteCustomerCommand(int CustomerId);
