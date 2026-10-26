using FluentValidation;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.Validators;

/// <summary>Validates claim submission requests.</summary>
internal sealed class CreateClaimRequestValidator : AbstractValidator<CreateClaimRequest>
{
    public CreateClaimRequestValidator()
    {
        RuleFor(request => request.PolicyId).GreaterThan(0);
        RuleFor(request => request.ClaimAmount).GreaterThan(0);
        RuleFor(request => request.Reason).NotEmpty().MaximumLength(1000);
    }
}
