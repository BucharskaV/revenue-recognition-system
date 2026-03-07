using FluentValidation;
using RevenueRecognitionSystem.Services.Contracts.Requests;

namespace RevenueRecognitionSystem.Services.Validators;

public class AddCompanyRequestValidator : AbstractValidator<AddCompanyRequest>
{
    public AddCompanyRequestValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(255)
            .WithMessage("Address must be less than 256 characters.");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .MaximumLength(50)
            .WithMessage("Email must be less than 51 characters.")
            .Matches(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")
            .WithMessage("Email must be a valid email address.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Length(9)
            .WithMessage("Phone number must be 9 digits long.")
            .Matches(@"^\d+$")
            .WithMessage("Phone number must contain only digits.");
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("Company name is required")
            .MaximumLength(50)
            .WithMessage("Company name must be less than 51 characters.");
        RuleFor(x => x.KRSNumber)
            .NotEmpty()
            .WithMessage("KRS number is required")
            .Length(9)
            .WithMessage("KRS number must be 9 digits long.")
            .Matches(@"^\d+$")
            .WithMessage("KRS number must contain only digits.");
    }
}