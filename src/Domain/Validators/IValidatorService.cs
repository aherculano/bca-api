using Domain.Models.Vehicle;
using FluentValidation.Results;

namespace Domain.Validators;

public interface IValidatorService
{
    ValidationResult ValidateVehicle(Vehicle vehicle);
}