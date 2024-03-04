using Domain.Models.Vehicle.ValueObjects.Validators;
using FluentValidation;

namespace Domain.Models.Vehicle.Validators;

public class VehicleValidator : AbstractValidator<Vehicle>
{
    public VehicleValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("Vehicle Cannot Be Null");

        RuleFor(x => x.UniqueIdentifier)
            .Must(x => x != Guid.Empty)
            .WithMessage("Vehicle Unique Identifier Cannot Be Empty");

        RuleFor(x => x.Definition)
            .SetValidator(new VehicleDefinitionValidator())
            .When(x => x.Definition is not null);

        RuleFor(x => x.StartingBid)
            .GreaterThan(0)
            .When(x => x is not null)
            .WithMessage("Starting Bid Must Be Greater Then 0");
    }
}