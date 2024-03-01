using Domain.Models.Vehicle;
using Domain.Repositories;
using FluentResults;

namespace Infrastructure.Repositories;

public class VehicleInMemoryRepository : IVehicleRepository
{
    private readonly IList<Vehicle> _vehicles;

    public VehicleInMemoryRepository()
    {
        _vehicles = new List<Vehicle>();
    }
    
    public async Task<Result<Vehicle>> CreateVheicleAsync(Vehicle vehicle)
    {
        _vehicles.Add(vehicle);
        return Result.Ok(vehicle);
    }

    public async Task<Result<Vehicle>> GetVehicleByUniqueIdentifierAsync(Guid uniqueIdentifier)
    {
        var vehicle = _vehicles.FirstOrDefault(x => x.UniqueIdentifier == uniqueIdentifier);

        return Result.Ok(vehicle);
    }

    public async Task<Result<IEnumerable<Vehicle>>> ListVehiclesAsync(string? type, string? manufacturer, string? model, int? year)
    {
        var vehicles = _vehicles;
        if (type is not null)
        {
            vehicles = vehicles.Where(x => x.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        if (manufacturer is not null)
        {
            vehicles = vehicles.Where(x => x.Manufacturer.Equals(manufacturer, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        if (model is not null)
        {
            vehicles = vehicles.Where(x => x.Model.Equals(model, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        if (year is not null)
        {
            vehicles = vehicles.Where(x => x.Year == year).ToList();
        }
        
        return Result.Ok(vehicles.AsEnumerable());
    }
}