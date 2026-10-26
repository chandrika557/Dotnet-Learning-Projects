using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.CQRS.Commands;

/// <summary>Write request for adding a customer.</summary>
/// <param name="Customer">Validated customer fields to create.</param>
public sealed record CreateCustomerCommand(CreateCustomerRequest Customer);
