using Domain.Models.Vehicle;
using Domain.Models.Vehicle.ValueObjects;
using Domain.Repositories;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EntityFramework.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly AuctionDbContext _dbContext;

    public VehicleRepository(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Vehicle>> CreateVehicleAsync(Vehicle vehicle)
    {
        try
        {
            await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Vehicles.AddAsync(vehicle);

            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();

            return Result.Ok(vehicle);
        }
        catch (Exception e)
        {
            await _dbContext.Database.CurrentTransaction?.RollbackAsync();
            return Result.Fail(new Error(e.Message).CausedBy(e));
        }
    }

    public async Task<Result<Vehicle>> GetVehicleByUniqueIdentifierAsync(Guid uniqueIdentifier)
    {
        try
        {
            var vehicle = await _dbContext.Vehicles
                .FirstOrDefaultAsync(x => x.UniqueIdentifier == uniqueIdentifier);

            return Result.Ok(vehicle);
        }
        catch (Exception e)
        {
            return Result.Fail(new Error(e.Message).CausedBy(e));
        }
    }

    public async Task<Result<IEnumerable<Vehicle>>> ListVehiclesAsync(VehicleType? type, string? manufacturer,
        string? model, int? year)
    {
        try
        {
            IQueryable<Vehicle> vehiclesQuery = _dbContext.Vehicles;

            if (type.HasValue)
                switch (type)
                {
                    case VehicleType.Sedan:
                        vehiclesQuery = vehiclesQuery.OfType<Sedan>();
                        break;
                    case VehicleType.Truck:
                        vehiclesQuery = vehiclesQuery.OfType<Truck>();
                        break;
                    case VehicleType.Suv:
                        vehiclesQuery = vehiclesQuery.OfType<Suv>();
                        break;
                }

            if (!string.IsNullOrEmpty(manufacturer))
                vehiclesQuery = vehiclesQuery.Where(v => v.Definition.Manufacturer == manufacturer);

            if (!string.IsNullOrEmpty(model)) vehiclesQuery = vehiclesQuery.Where(v => v.Definition.Model == model);

            if (year.HasValue) vehiclesQuery = vehiclesQuery.Where(v => v.Definition.Year == year);

            var result = await vehiclesQuery.ToListAsync();
            return Result.Ok(result.AsEnumerable());
        }
        catch (Exception e)
        {
            return Result.Fail(new Error(e.Message).CausedBy(e));
        }
    }
}