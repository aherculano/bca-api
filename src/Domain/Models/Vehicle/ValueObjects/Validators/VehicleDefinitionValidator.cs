using FluentValidation;

namespace Domain.Models.Vehicle.ValueObjects.Validators;

public class VehicleDefinitionValidator : AbstractValidator<VehicleDefinition>
{
    public VehicleDefinitionValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("Definition Cannot Be Null");

        RuleFor(x => x.Manufacturer)
            .NotEmpty()
            .When(x => x is not null)
            .WithMessage("Manufacturer Cannot Be Null");

        RuleFor(x => x.Model)
            .NotEmpty()
            .When(x => x is not null)
            .WithMessage("Model Cannot Be Null");

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, 2024)
            .When(x => x is not null)
            .WithMessage("Invalid Year");
    }
}