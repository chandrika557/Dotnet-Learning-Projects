using FluentValidation;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.Validators;

/// <summary>Validates login requests.</summary>
internal sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Email).NotEmpty().EmailAddress().MaximumLength(254);
        RuleFor(request => request.Password).NotEmpty();
    }
}
