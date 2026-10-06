using FluentValidation;
using InsuranceManagement.API.DTOs;

namespace InsuranceManagement.API.Validators;

/// <summary>Validates login requests.</summary>
public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Email).NotEmpty().EmailAddress().MaximumLength(254);
        RuleFor(request => request.Password).NotEmpty();
    }
}
