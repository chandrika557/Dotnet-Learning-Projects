using InsuranceManagement.MVC.Models;
using InsuranceManagement.MVC.Services;
using InsuranceManagement.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InsuranceManagement.MVC.Controllers;

public class ClaimController : Controller 
{
    private readonly IClaimService _claimService; //private field to hold the claim service instance
    private readonly IPolicyService _policyService;

    public ClaimController(IClaimService claimService, IPolicyService policyService) //constructor injection for claim and policy services
    {
        _claimService = claimService;
        _policyService = policyService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken) //method to retrieve all claims and display them in the view
    {
        var claims = await _claimService.GetAllAsync(cancellationToken);
        var viewModel = claims.Select(claim => new ClaimListItemViewModel
        {
            ClaimId = claim.ClaimId,
            PolicyId = claim.PolicyId,
            CustomerName = claim.Policy.Customer.Name ?? string.Empty, // Use null-coalescing operator to handle potential null values
            PolicyName = claim.Policy.PolicyName,
            ClaimAmount = claim.ClaimAmount,
            ClaimDate = claim.ClaimDate,
            Reason = claim.Reason,
            Status = claim.Status
        }).ToList();

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken) 
    {
        var input = new ClaimCreateViewModel(); // Create a new instance of ClaimCreateViewModel to hold the form data
        await PopulatePoliciesAsync(input, cancellationToken); // Populate the PolicyOptions property of the view model with available policies
        return View(input);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ClaimCreateViewModel input,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) // Check if the model state is valid, which means that all required fields are filled and meet the validation criteria
        {
            await PopulatePoliciesAsync(input, cancellationToken); // Repopulate the PolicyOptions property of the view model with available policies in case of validation errors
            return View(input);
        }

        var result = await _claimService.CreateAsync(new Claim // Call the CreateAsync method of the claim service to create a new claim
        {
            PolicyId = input.PolicyId, // Create a new Claim object and populate its properties with the form data
            ClaimAmount = input.ClaimAmount,
            Reason = input.Reason
        }, cancellationToken);

        if (result == ClaimCreationResult.PolicyNotFound) 
        {
            ModelState.AddModelError(nameof(input.PolicyId), "Select an existing policy."); // Add a model error to indicate that the selected policy does not exist
            await PopulatePoliciesAsync(input, cancellationToken);
            return View(input);
        }

        if (result == ClaimCreationResult.AmountExceedsCoverage) // Add a model error to indicate that the claim amount exceeds the policy coverage amount
        {
            ModelState.AddModelError(
                nameof(input.ClaimAmount),
                "The claim amount cannot be greater than the policy coverage amount.");
            await PopulatePoliciesAsync(input, cancellationToken);
            return View(input);
        }

        TempData["SuccessMessage"] = "Claim submitted with Pending status."; // Store a success message in TempData to display it on the next page
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // to prevent CSRF attacks
    public Task<IActionResult> Approve(int id, CancellationToken cancellationToken)
    {
        return UpdateStatusAsync(id, ClaimStatus.Approved, cancellationToken);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // to prevent CSRF attacks
    public Task<IActionResult> Reject(int id, CancellationToken cancellationToken)
    {
        return UpdateStatusAsync(id, ClaimStatus.Rejected, cancellationToken);
    }

    private async Task<IActionResult> UpdateStatusAsync(
        int id,
        ClaimStatus status,
        CancellationToken cancellationToken)
    {
        var updated = await _claimService.SetStatusAsync(id, status, cancellationToken);
        if (!updated)
        {
            TempData["ErrorMessage"] =
                "Claim was not found or has already been processed.";
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = $"Claim {status.ToString().ToLowerInvariant()}.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulatePoliciesAsync(
        ClaimCreateViewModel input,
        CancellationToken cancellationToken)
    {
        var policies = await _policyService.GetAllAsync(cancellationToken);
        input.PolicyOptions = policies
            .Select(policy => new SelectListItem(
                $"{policy.PolicyName} - {policy.Customer.Name} (coverage {policy.CoverageAmount:C})",
                policy.PolicyId.ToString(),
                policy.PolicyId == input.PolicyId))
            .ToList(); // Populate the PolicyOptions property of the view model with available policies, including the policy name, customer name, and coverage amount
    }
}
