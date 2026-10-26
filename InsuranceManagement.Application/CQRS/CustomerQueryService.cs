using AutoMapper;
using InsuranceManagement.Application.CQRS.Queries;
using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Application.Repositories;
using InsuranceManagement.Application.Services;

namespace InsuranceManagement.Application.CQRS;

/// <summary>Implements customer read use cases.</summary>
internal sealed class CustomerQueryService(
    ICustomerRepository repository,
    IMapper mapper) : ICustomerQueryService
{
    public async Task<PagedResponse<CustomerResponse>> SearchAsync(
        SearchCustomersQuery query,
        CancellationToken cancellationToken)
    {
        if (query.PageNumber < 1)
        {
            throw new ValidationException("Page number must be greater than zero.");
        }

        if (query.PageSize is < 1 or > 100)
        {
            throw new ValidationException("Page size must be between 1 and 100.");
        }

        var sortBy = string.IsNullOrWhiteSpace(query.SortBy) ? "name" : query.SortBy.Trim();
        if (!sortBy.Equals("name", StringComparison.OrdinalIgnoreCase)
            && !sortBy.Equals("email", StringComparison.OrdinalIgnoreCase))
        {
            throw new ValidationException("Sort field must be either 'name' or 'email'.");
        }

        var (customers, totalCount) = await repository.SearchAsync(
            query.Search,
            query.PageNumber,
            query.PageSize,
            sortBy,
            query.SortDescending,
            cancellationToken);

        return new PagedResponse<CustomerResponse>(
            mapper.Map<IReadOnlyList<CustomerResponse>>(customers),
            query.PageNumber,
            query.PageSize,
            totalCount);
    }

    public async Task<CustomerResponse?> GetByIdAsync(
        GetCustomerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(query.CustomerId, cancellationToken);
        return customer is null ? null : mapper.Map<CustomerResponse>(customer);
    }
}
