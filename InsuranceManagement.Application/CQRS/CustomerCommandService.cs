using AutoMapper;
using InsuranceManagement.Application.CQRS.Commands;
using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Domain.Models;
using InsuranceManagement.Application.Repositories;
using InsuranceManagement.Application.Services;

namespace InsuranceManagement.Application.CQRS;

/// <summary>Implements customer write use cases and business rules.</summary>
internal sealed class CustomerCommandService(
    ICustomerRepository repository,
    IMapper mapper) : ICustomerCommandService
{
    public async Task<CustomerResponse> CreateAsync(
        CreateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(command.Customer);
        Normalize(customer);
        var created = await repository.AddAsync(customer, cancellationToken);
        return mapper.Map<CustomerResponse>(created);
    }

    public async Task<CustomerResponse?> UpdateAsync(
        UpdateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(command.Customer);
        customer.Id = command.CustomerId;
        Normalize(customer);
        var updated = await repository.UpdateAsync(customer, cancellationToken);
        return updated is null ? null : mapper.Map<CustomerResponse>(updated);
    }

    public async Task<bool> DeleteAsync(
        DeleteCustomerCommand command,
        CancellationToken cancellationToken)
    {
        if (await repository.HasPoliciesAsync(command.CustomerId, cancellationToken))
        {
            throw new ConflictException("A customer with existing policies cannot be deleted.");
        }

        return await repository.DeleteAsync(command.CustomerId, cancellationToken);
    }

    private static void Normalize(Customer customer)
    {
        customer.Name = customer.Name.Trim();
        customer.Email = customer.Email?.Trim();
        customer.PhoneNumber = customer.PhoneNumber?.Trim();
        customer.Address = customer.Address?.Trim();
    }
}
