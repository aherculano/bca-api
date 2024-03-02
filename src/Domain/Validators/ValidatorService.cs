using Domain.Models.Vehicle;
using Domain.Models.Vehicle.Validators;
using FluentValidation.Results;

namespace Domain.Validators;

public class ValidatorService : IValidatorService
{
    private readonly SedanValidator _sedanValidator;
    private readonly SuvValidator _suvValidator;
    private readonly TruckValidator _truckValidator;

    public ValidatorService()
    {
        _suvValidator = new SuvValidator();
        _truckValidator = new TruckValidator();
        _sedanValidator = new SedanValidator();
    }

    public ValidationResult ValidateVehicle(Vehicle vehicle)
    {
        if (vehicle is Suv) return _suvValidator.Validate(vehicle as Suv);
        if (vehicle is Truck) return _truckValidator.Validate(vehicle as Truck);
        if (vehicle is Sedan) return _sedanValidator.Validate(vehicle as Sedan);

        throw new InvalidOperationException(
            $"There Are No Validators For The Specified Vehicle Type ${vehicle.GetType()}");
    }
}