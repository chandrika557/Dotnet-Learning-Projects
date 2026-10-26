using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.CQRS.Commands;

/// <summary>Write request for replacing a customer's details.</summary>
/// <param name="CustomerId">Identifier of the customer to update.</param>
/// <param name="Customer">Validated replacement fields.</param>
public sealed record UpdateCustomerCommand(int CustomerId, UpdateCustomerRequest Customer);
