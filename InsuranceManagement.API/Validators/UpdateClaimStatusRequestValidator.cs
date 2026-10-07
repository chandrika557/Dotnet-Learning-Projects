using FluentValidation;
using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.Validators;

/// <summary>Validates claim review requests.</summary>
public sealed class UpdateClaimStatusRequestValidator : AbstractValidator<UpdateClaimStatusRequest>
{
    public UpdateClaimStatusRequestValidator()
    {
        RuleFor(request => request.Status)
            .Must(status => status is ClaimStatus.Approved or ClaimStatus.Rejected)
            .WithMessage("A claim can only be approved or rejected.");
    }
}
