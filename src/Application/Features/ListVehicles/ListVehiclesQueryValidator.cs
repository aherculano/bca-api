using Domain.Models.Vehicle;
using FluentValidation;

namespace Application.Features.ListVehicles;

public class ListVehiclesQueryValidator : AbstractValidator<ListVehiclesQuery>
{
    public ListVehiclesQueryValidator()
    {
        RuleFor(x => x.Request.Type)
            .Must(BeValidType)
            .When(x => x.Request.Type is not null)
            .WithMessage("Invalid Vehicle Type");

        RuleFor(x => x.Request.Manufacturer)
            .NotEmpty()
            .When(x => x.Request.Manufacturer is not null)
            .WithMessage("Invalid Vehicle Manufacturer");

        RuleFor(x => x.Request.Model)
            .NotEmpty()
            .When(x => x.Request.Type is not null)
            .WithMessage("Invalid Vehicle Model");

        RuleFor(x => x.Request.Year)
            .InclusiveBetween(1900, DateTime.Now.Year)
            .When(x => x.Request.Year is not null)
            .WithMessage("Invalid Vehicle Year");
    }

    public bool BeValidType(string type)
    {
        switch (type)
        {
            case var _ when type.Equals(Suv.SuvType, StringComparison.InvariantCultureIgnoreCase)
                            || type.Equals(Truck.TruckType, StringComparison.InvariantCultureIgnoreCase)
                            || type.Equals(Sedan.SedanType, StringComparison.InvariantCultureIgnoreCase):
                return true;
            default:
                return false;
        }
    }
}