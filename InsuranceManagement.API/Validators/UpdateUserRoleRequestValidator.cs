using FluentValidation;
using InsuranceManagement.API.DTOs;
using InsuranceManagement.API.Models;

namespace InsuranceManagement.API.Validators;

/// <summary>Validates role assignment requests.</summary>
public sealed class UpdateUserRoleRequestValidator : AbstractValidator<UpdateUserRoleRequest>
{
    public UpdateUserRoleRequestValidator()
    {
        RuleFor(request => request.Role)
            .Must(role => role is UserRoles.Customer or UserRoles.ClaimsAdjuster or UserRoles.Administrator)
            .WithMessage("Role must be Customer, ClaimsAdjuster, or Administrator.");
    }
}
