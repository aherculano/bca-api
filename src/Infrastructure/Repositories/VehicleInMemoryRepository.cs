using Domain.Models.Vehicle;
using Domain.Repositories;
using FluentResults;

namespace Infrastructure.Repositories;

public class VehicleInMemoryRepository : IVehicleRepository
{
    public Task<Result<Vehicle>> CreateVheicleAsync(Vehicle vehicle)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Vehicle>> GetVehicleByUniqueIdentifierAsync(Guid uniqueIdentifier)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Vehicle>>> ListVehiclesAsync(Func<Vehicle, bool> filter)
    {
        throw new NotImplementedException();
    }
}