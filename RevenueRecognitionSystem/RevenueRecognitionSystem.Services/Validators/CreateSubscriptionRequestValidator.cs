using FluentValidation;
using RevenueRecognitionSystem.Services.Contracts.Requests;

namespace RevenueRecognitionSystem.Services.Validators;

public class CreateSubscriptionRequestValidator : AbstractValidator<CreateSubscriptionRequest>
{
    public CreateSubscriptionRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Company name is required")
            .MaximumLength(20)
            .WithMessage("Company name must be less than 21 characters.");
        RuleFor(x => x.RenewalPeriodInMonths)
            .NotEmpty()
            .WithMessage("Renewal period is required")
            .InclusiveBetween(1, 24)
            .WithMessage("Renewal period must be between 1 and 24 months");;
    }
}