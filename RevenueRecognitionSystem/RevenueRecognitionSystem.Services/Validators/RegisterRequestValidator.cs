using FluentValidation;
using RevenueRecognitionSystem.Services.Contracts.Requests;

namespace RevenueRecognitionSystem.Services.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .WithMessage("Login is required")
            .MaximumLength(20)
            .WithMessage("Login must be less than 21 characters.")
            .MinimumLength(5)
            .WithMessage("Login must be between 5 and 20 characters.");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MaximumLength(20)
            .WithMessage("Password must be less than 21 characters.")
            .Matches(@"^.{8,}$")
            .WithMessage("Password must contain at least 8 characters.")
            .Matches(@"^(?=.*[A-Z]).*$")
            .WithMessage("Password must contain at least 1 upper case letter.")
            .Matches(@"^(?=.*[a-z]).*$")
            .WithMessage("Password must contain at least 1 lower case letter.")
            .Matches(@"^(?=.*\d).*$")
            .WithMessage("Password must contain at least 1 digit.")
            .Matches(@"^(?=.*[!@#$%^&*]).*$")
            .WithMessage("Password must contain at least 1 special character.");
        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Role must be a valid Employee Role.");
    }
}