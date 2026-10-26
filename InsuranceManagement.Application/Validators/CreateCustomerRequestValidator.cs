using FluentValidation;
using InsuranceManagement.Application.DTOs;

namespace InsuranceManagement.Application.Validators;

/// <summary>Validates customer creation requests.</summary>
internal sealed class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(request => request.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Customer name must contain a non-whitespace character.")
            .MaximumLength(150);
        RuleFor(request => request.Email)
            .EmailAddress()
            .When(request => !string.IsNullOrWhiteSpace(request.Email))
            .MaximumLength(254);
        RuleFor(request => request.PhoneNumber).MaximumLength(32);
        RuleFor(request => request.Address).MaximumLength(500);
    }
}
