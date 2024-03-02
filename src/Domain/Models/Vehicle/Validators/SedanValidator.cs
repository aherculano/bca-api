using FluentValidation;

namespace Domain.Models.Vehicle.Validators;

public class SedanValidator : AbstractValidator<Sedan>
{
    public SedanValidator()
    {
        Include(new VehicleValidator());

        RuleFor(x => x.NumberOfDoors)
            .InclusiveBetween(2, 5)
            .When(x => x is not null)
            .WithMessage("Number Of Doors Needs To Be Between 2 and 5");
    }
}