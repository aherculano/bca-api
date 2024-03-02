using Domain.Models.Vehicle;
using Domain.Models.Vehicle.ValueObjects;
using FluentResults;

namespace Domain.Repositories;

public interface IVehicleRepository
{
    Task<Result<Vehicle>> CreateVehicleAsync(Vehicle vehicle);

    Task<Result<Vehicle>> GetVehicleByUniqueIdentifierAsync(Guid uniqueIdentifier);

    Task<Result<IEnumerable<Vehicle>>> ListVehiclesAsync(
        VehicleType? type,
        string? manufacturer,
        string? model,
        int? year);
}