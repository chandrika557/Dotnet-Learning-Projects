using FluentValidation;
using InsuranceManagement.API.DTOs;

namespace InsuranceManagement.API.Validators;

/// <summary>Validates claim submission requests.</summary>
public sealed class CreateClaimRequestValidator : AbstractValidator<CreateClaimRequest>
{
    public CreateClaimRequestValidator()
    {
        RuleFor(request => request.PolicyId).GreaterThan(0);
        RuleFor(request => request.ClaimAmount).GreaterThan(0);
        RuleFor(request => request.Reason).NotEmpty().MaximumLength(1000);
    }
}
