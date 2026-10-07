using InsuranceManagement.API.DTOs;

namespace InsuranceManagement.API.Services;

/// <summary>Defines customer use cases for the API.</summary>
public interface ICustomerService
{
    Task<PagedResponse<CustomerResponse>> SearchAsync(
        CustomerSearchOptions options,
        CancellationToken cancellationToken);

    Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken);
    Task<CustomerResponse?> UpdateAsync(int id, UpdateCustomerRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
