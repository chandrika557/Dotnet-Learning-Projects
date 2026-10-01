using InsuranceManagement.MVC.Models;
using InsuranceManagement.MVC.Repositories;

namespace InsuranceManagement.MVC.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _customerRepository.GetAllAsync(cancellationToken);
    }

    public Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _customerRepository.GetByIdAsync(id, cancellationToken);
    }

    public Task<bool> HasPoliciesAsync(int customerId, CancellationToken cancellationToken)
    {
        return _customerRepository.HasPoliciesAsync(customerId, cancellationToken);
    }

    public async Task CreateAsync(Customer customer, CancellationToken cancellationToken)
    {
        Normalize(customer);
        await _customerRepository.AddAsync(customer, cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        int id,
        Customer changes,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
        {
            return false;
        }

        customer.Name = changes.Name;
        customer.Email = changes.Email;
        customer.PhoneNumber = changes.PhoneNumber;
        customer.Address = changes.Address;
        Normalize(customer);

        await _customerRepository.UpdateAsync(customer, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        if (customer is null ||
            await _customerRepository.HasPoliciesAsync(id, cancellationToken))
        {
            return false;
        }

        await _customerRepository.DeleteAsync(customer, cancellationToken);
        return true;
    }

    private static void Normalize(Customer customer)
    {
        customer.Name = customer.Name?.Trim();
        customer.Email = customer.Email?.Trim();
        customer.PhoneNumber = customer.PhoneNumber?.Trim();
        customer.Address = customer.Address?.Trim();
    }
}
