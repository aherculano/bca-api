using Application.Requests.VehicleRequests;
using FluentValidation;

namespace Application.Features.CreateVehicle;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.Request).NotNull();

        RuleFor(x => x.Request)
            .Must(BeValidRequestType)
            .When(x => x.Request is not null)
            .WithMessage("Invalid VehicleRequest type");

        RuleFor(x => x.Request.UniqueIdentifier)
            .Must(x => x != Guid.Empty)
            .When(x => x.Request is not null)
            .WithMessage("Invalid Unique Identifier");

        RuleFor(x => x.Request.Manufacturer)
            .NotEmpty()
            .When(x => x.Request is not null)
            .WithMessage("Invalid Manufacturer");

        RuleFor(x => x.Request.Model)
            .NotEmpty()
            .When(x => x.Request is not null)
            .WithMessage("Invalid Model");

        RuleFor(x => x.Request.Year)
            .InclusiveBetween(1900, DateTime.Now.Year)
            .When(x => x.Request is not null)
            .WithMessage("Invalid Year");

        RuleFor(x => x.Request.StartingBid)
            .GreaterThan(0)
            .When(x => x.Request is not null)
            .WithMessage("Invalid Starting Bid");

        RuleFor(x => x.Request)
            .Must(BeValidTruck)
            .WithMessage("Invalid Truck Properties");

        RuleFor(x => x.Request)
            .Must(BeValidSuv)
            .WithMessage("Invalid Suv Properties");

        RuleFor(x => x.Request)
            .Must(BeValidSedan)
            .WithMessage("Invalid Sedan Properties");
    }

    private bool BeValidRequestType(VehicleRequest request)
    {
        if (request is SedanRequest) return true;

        if (request is SuvRequest) return true;

        if (request is TruckRequest) return true;

        return false;
    }

    private bool BeValidTruck(VehicleRequest request)
    {
        if (request is TruckRequest truckRequest) return truckRequest.LoadCapacity > 0;

        return true;
    }

    private bool BeValidSuv(VehicleRequest request)
    {
        if (request is SuvRequest suvRequest) return suvRequest.NumberOfSeats is > 1 and < 10;

        return true;
    }

    private bool BeValidSedan(VehicleRequest request)
    {
        if (request is SedanRequest sedanRequest) return sedanRequest.NumberOfDoors is > 1 and < 6;
        return true;
    }
}