using Domain.Models.Vehicle;
using FluentResults;

namespace Domain.Repositories;

public interface IVehicleRepository
{
    Task<Result<Vehicle>> CreateVheicleAsync(Vehicle vehicle);

    Task<Result<Vehicle>> GetVehicleByUniqueIdentifierAsync(Guid uniqueIdentifier);

    Task<Result<IEnumerable<Vehicle>>> ListVehiclesAsync(Func<Vehicle, bool> filter);
}