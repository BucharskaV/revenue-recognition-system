using FluentValidation;
using RevenueRecognitionSystem.Services.Contracts.Requests;

namespace RevenueRecognitionSystem.Services.Validators;

public class CreateUpfrontContractRequestValidator : AbstractValidator<CreateUpfrontContractRequest>
{
    public CreateUpfrontContractRequestValidator()
    {
        RuleFor(x => x.SoftwareVersion)
            .NotEmpty()
            .WithMessage("Software version is required")
            .MaximumLength(50)
            .WithMessage("Software version must be less than 51 characters.");
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required")
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be earlier than end date.");
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be later than start date.");
        RuleFor(x => x.ExtraSupportYears)
            .NotEmpty()
            .WithMessage("Software version is required")
            .InclusiveBetween(0, 3)
            .WithMessage("Extra support years must be between 0 and 3.");
    }
}