using Domain.Models.Vehicle;
using Domain.Models.Vehicle.ValueObjects;
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

    public async Task<Result<IEnumerable<Vehicle>>> ListVehiclesAsync(string? type, string? manufacturer, string? model,
        int? year)
    {
        var vehicles = _vehicles;
        if (type is not null)
        {
            if (type.Equals(VehicleType.Suv.ToString())) vehicles = vehicles.Where(x => x is Suv).ToList();

            if (type.Equals(VehicleType.Sedan.ToString())) vehicles = vehicles.Where(x => x is Sedan).ToList();

            if (type.Equals(VehicleType.Truck.ToString())) vehicles = vehicles.Where(x => x is Truck).ToList();
        }

        if (manufacturer is not null)
            vehicles = vehicles
                .Where(x => x.Definition.Manufacturer.Equals(manufacturer, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

        if (model is not null)
            vehicles = vehicles
                .Where(x => x.Definition.Model.Equals(model, StringComparison.InvariantCultureIgnoreCase)).ToList();

        if (year is not null) vehicles = vehicles.Where(x => x.Definition.Year == year).ToList();

        return Result.Ok(vehicles.AsEnumerable());
    }
}