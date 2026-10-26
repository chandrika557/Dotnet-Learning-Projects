using AutoMapper;
using InsuranceManagement.Application.CQRS;
using InsuranceManagement.Application.CQRS.Commands;
using InsuranceManagement.Application.CQRS.Queries;
using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Domain.Models;
using InsuranceManagement.Application.Repositories;
using InsuranceManagement.Application.Services;
using InsuranceManagement.Application.Services.Mapping;
using InsuranceManagement.Application.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;

namespace InsuranceManagement.API.Tests;

public sealed class CustomerUseCaseTests
{
    [Fact]
    public async Task SearchAsync_RejectsPageSizeAboveMaximum()
    {
        var repository = new FakeCustomerRepository();
        var service = new CustomerQueryService(repository, null!);

        var exception = await Assert.ThrowsAsync<InsuranceManagement.Application.Services.ValidationException>(() =>
            service.SearchAsync(new SearchCustomersQuery(null, 1, 101, "name", false), CancellationToken.None));

        Assert.Equal("Page size must be between 1 and 100.", exception.Message);
        Assert.False(repository.SearchWasCalled);
    }

    [Fact]
    public async Task DeleteAsync_RejectsCustomerWithPolicies()
    {
        var repository = new FakeCustomerRepository { HasPolicies = true };
        var service = new CustomerCommandService(repository, null!);

        await Assert.ThrowsAsync<ConflictException>(() =>
            service.DeleteAsync(new DeleteCustomerCommand(17), CancellationToken.None));

        Assert.False(repository.DeleteWasCalled);
    }

    [Fact]
    public async Task CreateAsync_NormalizesNameAndEmailBeforePersisting()
    {
        var repository = new FakeCustomerRepository();
        var service = new CustomerCommandService(repository, CreateMapper());

        var result = await service.CreateAsync(
            new CreateCustomerCommand(new CreateCustomerRequest
            {
                Name = "  Jordan Lee  ",
                Email = "  JORDAN@example.com "
            }),
            CancellationToken.None);

        Assert.Equal("Jordan Lee", repository.AddedCustomer!.Name);
        Assert.Equal("JORDAN@example.com", repository.AddedCustomer.Email);
        Assert.Equal("Jordan Lee", result.Name);
    }

    [Fact]
    public async Task CreateCustomerValidator_RejectsWhitespaceNameAndInvalidEmail()
    {
        var validator = new CreateCustomerRequestValidator();

        var result = await validator.ValidateAsync(new CreateCustomerRequest
        {
            Name = " ",
            Email = "not-an-email"
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateCustomerRequest.Name));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateCustomerRequest.Email));
    }

    [Fact]
    public async Task RegisterAsync_StoresPasswordHashAndAssignsCustomerRole()
    {
        var repository = new FakeUserAccountRepository();
        var service = new AuthService(
            repository,
            new PasswordHasher<UserAccount>(),
            new FakeTokenService());

        await service.RegisterAsync(
            new RegisterRequest
            {
                Email = "  JORDAN@example.com ",
                Password = "Strong-Password-91!"
            },
            CancellationToken.None);

        Assert.Equal("jordan@example.com", repository.SavedUser!.Email);
        Assert.Equal(UserRoles.Customer, repository.SavedUser.Role);
        Assert.NotEqual("Strong-Password-91!", repository.SavedUser.PasswordHash);
        Assert.Equal(
            PasswordVerificationResult.Success,
            new PasswordHasher<UserAccount>().VerifyHashedPassword(
                repository.SavedUser,
                repository.SavedUser.PasswordHash,
                "Strong-Password-91!"));
    }

    private static IMapper CreateMapper()
    {
        var configuration = new MapperConfiguration(
            options =>
            {
                options.LicenseKey = Environment.GetEnvironmentVariable("AutoMapper__LicenseKey");
                options.AddProfile<InsuranceMappingProfile>();
            },
            NullLoggerFactory.Instance);
        return configuration.CreateMapper();
    }

    private sealed class FakeCustomerRepository : ICustomerRepository
    {
        public bool HasPolicies { get; init; }
        public bool SearchWasCalled { get; private set; }
        public bool DeleteWasCalled { get; private set; }
        public Customer? AddedCustomer { get; private set; }

        public Task<(IReadOnlyList<Customer> Items, int TotalCount)> SearchAsync(
            string? search,
            int pageNumber,
            int pageSize,
            string sortBy,
            bool sortDescending,
            CancellationToken cancellationToken)
        {
            SearchWasCalled = true;
            return Task.FromResult<(IReadOnlyList<Customer>, int)>((Array.Empty<Customer>(), 0));
        }

        public Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            Task.FromResult<Customer?>(null);

        public Task<bool> HasPoliciesAsync(int customerId, CancellationToken cancellationToken) =>
            Task.FromResult(HasPolicies);

        public Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken)
        {
            customer.Id = 41;
            AddedCustomer = customer;
            return Task.FromResult(customer);
        }

        public Task<Customer?> UpdateAsync(Customer customer, CancellationToken cancellationToken) =>
            Task.FromResult<Customer?>(customer);

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            DeleteWasCalled = true;
            return Task.FromResult(true);
        }
    }

    private sealed class FakeUserAccountRepository : IUserAccountRepository
    {
        public UserAccount? SavedUser { get; private set; }

        public Task<UserAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
            Task.FromResult(SavedUser?.Email == email ? SavedUser : null);

        public Task<UserAccount?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            Task.FromResult(SavedUser?.UserAccountId == id ? SavedUser : null);

        public Task<UserAccount> AddAsync(UserAccount userAccount, CancellationToken cancellationToken)
        {
            userAccount.UserAccountId = 1;
            SavedUser = userAccount;
            return Task.FromResult(userAccount);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class FakeTokenService : ITokenService
    {
        public AuthResponse CreateAccessToken(UserAccount userAccount) =>
            new("test-token", DateTimeOffset.UtcNow.AddMinutes(5), userAccount.Role, userAccount.UserAccountId);
    }
}
