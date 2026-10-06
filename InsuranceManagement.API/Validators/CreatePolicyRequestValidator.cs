using FluentValidation;
using InsuranceManagement.API.DTOs;

namespace InsuranceManagement.API.Validators;

/// <summary>Validates policy creation requests.</summary>
public sealed class CreatePolicyRequestValidator : AbstractValidator<CreatePolicyRequest>
{
    public CreatePolicyRequestValidator()
    {
        RuleFor(request => request.PolicyName).NotEmpty().MaximumLength(150);
        RuleFor(request => request.PolicyType).IsInEnum();
        RuleFor(request => request.CoverageAmount).GreaterThan(0);
        RuleFor(request => request.PremiumAmount).GreaterThan(0);
        RuleFor(request => request.CustomerId).GreaterThan(0);
    }
}
