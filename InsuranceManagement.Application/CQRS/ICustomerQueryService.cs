using InsuranceManagement.Application.CQRS.Queries;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.CQRS;

/// <summary>Handles read-only customer queries.</summary>
public interface ICustomerQueryService
{
    /// <summary>Searches customers and returns a page of results.</summary>
    Task<PagedResponse<CustomerResponse>> SearchAsync(
        SearchCustomersQuery query,
        CancellationToken cancellationToken);

    /// <summary>Gets one customer, or null if it does not exist.</summary>
    Task<CustomerResponse?> GetByIdAsync(
        GetCustomerByIdQuery query,
        CancellationToken cancellationToken);
}
