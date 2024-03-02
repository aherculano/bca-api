using FluentValidation;

namespace Domain.Models.Vehicle;

public class VehicleValidator : AbstractValidator<Vehicle>
{
    public VehicleValidator()
    {
        RuleFor(x => x.UniqueIdentifier)
            .NotEmpty()
            .WithMessage("Unique Identifier Should Not Be Empty");
    }
}