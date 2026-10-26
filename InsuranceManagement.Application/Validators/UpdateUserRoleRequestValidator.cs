using FluentValidation;
using InsuranceManagement.Application.DTOs;
using InsuranceManagement.Domain.Models;

namespace InsuranceManagement.Application.Validators;

/// <summary>Validates role assignment requests.</summary>
internal sealed class UpdateUserRoleRequestValidator : AbstractValidator<UpdateUserRoleRequest>
{
    public UpdateUserRoleRequestValidator()
    {
        RuleFor(request => request.Role)
            .Must(role => role is UserRoles.Customer or UserRoles.ClaimsAdjuster or UserRoles.Administrator)
            .WithMessage("Role must be Customer, ClaimsAdjuster, or Administrator.");
    }
}
