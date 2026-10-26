using FluentValidation;
using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Domain.Models;

namespace InsuranceManagement.Application.Validators;

/// <summary>Validates claim review requests.</summary>
internal sealed class UpdateClaimStatusRequestValidator : AbstractValidator<UpdateClaimStatusRequest>
{
    public UpdateClaimStatusRequestValidator()
    {
        RuleFor(request => request.Status)
            .Must(status => status is ClaimStatus.Approved or ClaimStatus.Rejected)
            .WithMessage("A claim can only be approved or rejected.");
    }
}
