using AutoMapper;
using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;
using InsuranceManagement.API.Repositories;

namespace InsuranceManagement.API.Services;

/// <summary>Implements customer use cases and maps domain entities to API contracts.</summary>
public sealed class CustomerService(ICustomerRepository repository, IMapper mapper) : ICustomerService
{
    public async Task<PagedResponse<CustomerResponse>> SearchAsync(
        CustomerSearchOptions options,
        CancellationToken cancellationToken)
    {
        if (options.PageNumber < 1)
        {
            throw new ValidationException("Page number must be greater than zero.");
        }

        if (options.PageSize is < 1 or > 100)
        {
            throw new ValidationException("Page size must be between 1 and 100.");
        }

        var sortBy = string.IsNullOrWhiteSpace(options.SortBy) ? "name" : options.SortBy.Trim();
        if (!sortBy.Equals("name", StringComparison.OrdinalIgnoreCase)
            && !sortBy.Equals("email", StringComparison.OrdinalIgnoreCase))
        {
            throw new ValidationException("Sort field must be either 'name' or 'email'.");
        }

        var (customers, totalCount) = await repository.SearchAsync(
            options.Search,
            options.PageNumber,
            options.PageSize,
            sortBy,
            options.SortDescending,
            cancellationToken);

        return new PagedResponse<CustomerResponse>(
            mapper.Map<IReadOnlyList<CustomerResponse>>(customers),
            options.PageNumber,
            options.PageSize,
            totalCount);
    }

    public async Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(id, cancellationToken);
        return customer is null ? null : mapper.Map<CustomerResponse>(customer);
    }

    public async Task<CustomerResponse> CreateAsync(
        CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(request);
        Normalize(customer);
        var created = await repository.AddAsync(customer, cancellationToken);
        return mapper.Map<CustomerResponse>(created);
    }

    public async Task<CustomerResponse?> UpdateAsync(
        int id,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(request);
        customer.Id = id;
        Normalize(customer);
        var updated = await repository.UpdateAsync(customer, cancellationToken);
        return updated is null ? null : mapper.Map<CustomerResponse>(updated);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (await repository.HasPoliciesAsync(id, cancellationToken))
        {
            throw new ConflictException("A customer with existing policies cannot be deleted.");
        }

        return await repository.DeleteAsync(id, cancellationToken);
    }

    private static void Normalize(Customer customer)
    {
        customer.Name = customer.Name.Trim();
        customer.Email = customer.Email?.Trim();
        customer.PhoneNumber = customer.PhoneNumber?.Trim();
        customer.Address = customer.Address?.Trim();
    }
}
