using FluentValidation;

namespace Domain.Models.Vehicle.Validators;

public class SuvValidator : AbstractValidator<Suv>
{
    public SuvValidator()
    {
        Include(new VehicleValidator());

        RuleFor(x => x.NumberOfSeats)
            .InclusiveBetween(2, 9)
            .When(x => x is not null)
            .WithMessage("Number Of Seats Needs To Be Between 2 and 9");
    }
}