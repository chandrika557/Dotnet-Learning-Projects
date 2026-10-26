using InsuranceManagement.Application.CQRS.Commands;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.CQRS;

/// <summary>Handles customer write commands.</summary>
public interface ICustomerCommandService
{
    /// <summary>Creates a customer and returns its response DTO.</summary>
    Task<CustomerResponse> CreateAsync(
        CreateCustomerCommand command,
        CancellationToken cancellationToken);

    /// <summary>Replaces customer details, or returns null if the ID does not exist.</summary>
    Task<CustomerResponse?> UpdateAsync(
        UpdateCustomerCommand command,
        CancellationToken cancellationToken);

    /// <summary>Deletes a customer and reports whether a row was deleted.</summary>
    Task<bool> DeleteAsync(DeleteCustomerCommand command, CancellationToken cancellationToken);
}
