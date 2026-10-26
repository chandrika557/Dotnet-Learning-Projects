using FluentValidation;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.Validators;

/// <summary>Validates policy creation requests.</summary>
internal sealed class CreatePolicyRequestValidator : AbstractValidator<CreatePolicyRequest>
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
