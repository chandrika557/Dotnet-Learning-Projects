using FluentValidation;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.Validators;

/// <summary>Validates policy update requests.</summary>
internal sealed class UpdatePolicyRequestValidator : AbstractValidator<UpdatePolicyRequest>
{
    public UpdatePolicyRequestValidator()
    {
        RuleFor(request => request.PolicyName).NotEmpty().MaximumLength(150);
        RuleFor(request => request.PolicyType).IsInEnum();
        RuleFor(request => request.CoverageAmount).GreaterThan(0);
        RuleFor(request => request.PremiumAmount).GreaterThan(0);
        RuleFor(request => request.CustomerId).GreaterThan(0);
    }
}
