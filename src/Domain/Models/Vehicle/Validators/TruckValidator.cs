using FluentValidation;

namespace Domain.Models.Vehicle.Validators;

public class TruckValidator : AbstractValidator<Truck>
{
    public TruckValidator()
    {
        Include(new VehicleValidator());

        RuleFor(x => x.LoadCapacity)
            .GreaterThan(0)
            .When(x => x is not null)
            .WithMessage("Load Capacity Must Be Greater Then 0");
    }
}