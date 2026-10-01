using InsuranceManagement.MVC.Models;
using InsuranceManagement.MVC.Services;
using InsuranceManagement.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsuranceManagement.MVC.Controllers;

public class PolicyController : Controller
{
    private readonly IPolicyService _policyService;
    private readonly ICustomerService _customerService;

    public PolicyController(IPolicyService policyService, ICustomerService customerService)
    {
        _policyService = policyService;
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var policies = await _policyService.GetAllAsync(cancellationToken);
        var viewModel = policies.Select(policy => new PolicyListItemViewModel
        {
            PolicyId = policy.PolicyId,
            PolicyName = policy.PolicyName,
            PolicyType = policy.PolicyType,
            CoverageAmount = policy.CoverageAmount,
            PremiumAmount = policy.PremiumAmount,
            CustomerId = policy.CustomerId,
            CustomerName = policy.Customer.Name ?? string.Empty
        }).ToList();

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var policy = await _policyService.GetByIdAsync(id, cancellationToken);
        if (policy is null)
        {
            return NotFound();
        }

        return View(await ToDetailsViewModelAsync(policy, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var input = new PolicyFormViewModel();
        await PopulateCustomersAsync(input, cancellationToken);
        return View(input);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        PolicyFormViewModel input,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCustomersAsync(input, cancellationToken);
            return View(input);
        }

        var created = await _policyService.CreateAsync(ToPolicy(input), cancellationToken);
        if (!created)
        {
            ModelState.AddModelError(nameof(input.CustomerId), "Select an existing customer.");
            await PopulateCustomersAsync(input, cancellationToken);
            return View(input);
        }

        TempData["SuccessMessage"] = "Policy created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var policy = await _policyService.GetByIdAsync(id, cancellationToken);
        if (policy is null)
        {
            return NotFound();
        }

        var input = ToFormViewModel(policy);
        await PopulateCustomersAsync(
            input,
            cancellationToken,
            canChangeCustomer: !await _policyService.HasClaimsAsync(id, cancellationToken));
        return View(input);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        PolicyFormViewModel input,
        CancellationToken cancellationToken)
    {
        var existingPolicy = await _policyService.GetByIdAsync(id, cancellationToken);
        if (existingPolicy is null)
        {
            return NotFound();
        }

        var canChangeCustomer = !await _policyService.HasClaimsAsync(id, cancellationToken);
        if (!ModelState.IsValid)
        {
            await PopulateCustomersAsync(input, cancellationToken, canChangeCustomer);
            return View(input);
        }

        var updated = await _policyService.UpdateAsync(id, ToPolicy(input), cancellationToken);
        if (!updated)
        {
            if (existingPolicy.CustomerId != input.CustomerId && !canChangeCustomer)
            {
                ModelState.AddModelError(
                    nameof(input.CustomerId),
                    "A policy with claims cannot be assigned to another customer.");
            }
            else
            {
                ModelState.AddModelError(nameof(input.CustomerId), "Select an existing customer.");
            }

            await PopulateCustomersAsync(input, cancellationToken, canChangeCustomer);
            return View(input);
        }

        TempData["SuccessMessage"] = "Policy updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var policy = await _policyService.GetByIdAsync(id, cancellationToken);
        if (policy is null)
        {
            return NotFound();
        }

        return View(await ToDetailsViewModelAsync(policy, cancellationToken));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        if (await _policyService.GetByIdAsync(id, cancellationToken) is null)
        {
            return NotFound();
        }

        if (await _policyService.HasClaimsAsync(id, cancellationToken))
        {
            TempData["ErrorMessage"] = "This policy has claims and cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }

        var deleted = await _policyService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Policy deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCustomersAsync(
        PolicyFormViewModel input,
        CancellationToken cancellationToken,
        bool canChangeCustomer = true)
    {
        input.CanChangeCustomer = canChangeCustomer;
        var customers = await _customerService.GetAllAsync(cancellationToken);
        input.CustomerOptions = customers
            .Select(customer => new SelectListItem(
                customer.Name ?? $"Customer {customer.Id}",
                customer.Id.ToString(),
                customer.Id == input.CustomerId))
            .ToList();
    }

    private async Task<PolicyDetailsViewModel> ToDetailsViewModelAsync(
        Policy policy,
        CancellationToken cancellationToken)
    {
        return new PolicyDetailsViewModel
        {
            PolicyId = policy.PolicyId,
            PolicyName = policy.PolicyName,
            PolicyType = policy.PolicyType,
            CoverageAmount = policy.CoverageAmount,
            PremiumAmount = policy.PremiumAmount,
            CustomerId = policy.CustomerId,
            CustomerName = policy.Customer.Name ?? string.Empty,
            HasClaims = await _policyService.HasClaimsAsync(policy.PolicyId, cancellationToken)
        };
    }

    private static Policy ToPolicy(PolicyFormViewModel input)
    {
        return new Policy
        {
            PolicyName = input.PolicyName,
            PolicyType = input.PolicyType!.Value,
            CoverageAmount = input.CoverageAmount,
            PremiumAmount = input.PremiumAmount,
            CustomerId = input.CustomerId
        };
    }

    private static PolicyFormViewModel ToFormViewModel(Policy policy)
    {
        return new PolicyFormViewModel
        {
            PolicyName = policy.PolicyName,
            PolicyType = policy.PolicyType,
            CoverageAmount = policy.CoverageAmount,
            PremiumAmount = policy.PremiumAmount,
            CustomerId = policy.CustomerId
        };
    }
}
