using FluentValidation;
using RevenueRecognitionSystem.Services.Contracts.Requests;

namespace RevenueRecognitionSystem.Services.Validators;

public class AddIndividualRequestValidator: AbstractValidator<AddIndividualRequest>
{
    public AddIndividualRequestValidator()
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
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Firstname is required")
            .MaximumLength(50)
            .WithMessage("Firstname must be less than 51 characters.")
            .Matches(@"^[A-Za-z]+$")
            .WithMessage("Firstname must contain only alphanumeric characters.");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Lastname is required")
            .MaximumLength(50)
            .WithMessage("Lastname must be less than 51 characters.")
            .Matches(@"^[A-Za-z]+$")
            .WithMessage("Lastname must contain only alphanumeric characters.");
        RuleFor(x => x.PESEL)
            .NotEmpty()
            .WithMessage("PESEL number is required")
            .Length(11)
            .WithMessage("PESEL number must be 11 digits long.")
            .Matches(@"^\d+$")
            .WithMessage("PESEL number must contain only digits.");
    }
}