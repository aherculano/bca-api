using Application.Features.CreateVehicle;
using Application.Requests.VehicleRequests;
using Domain.Models.Vehicle.ValueObjects;

namespace Api.Requests;

public record CreateVehicleRequest(
    Guid UniqueIdentifier,
    string Type,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid,
    decimal? LoadCapacity,
    int? NumberOfDoors,
    int? NumberOfSeats);

internal static class CreateVehicleToCommandMapper
{
    public static CreateVehicleCommand MapToCommand(this CreateVehicleRequest request)
    {
        if (request.Type.Equals(VehicleType.Suv.ToString(), StringComparison.InvariantCultureIgnoreCase))
            return new CreateVehicleCommand(new SuvRequest(
                request.UniqueIdentifier,
                request.Manufacturer,
                request.Model,
                request.Year,
                request.StartingBid,
                request.NumberOfSeats.GetValueOrDefault()));

        if (request.Type.Equals(VehicleType.Sedan.ToString(), StringComparison.InvariantCultureIgnoreCase))
            return new CreateVehicleCommand(new SedanRequest(
                request.UniqueIdentifier,
                request.Manufacturer,
                request.Model,
                request.Year,
                request.StartingBid,
                request.NumberOfDoors.GetValueOrDefault()));

        if (request.Type.Equals(VehicleType.Truck.ToString(), StringComparison.InvariantCultureIgnoreCase))
            return new CreateVehicleCommand(new TruckRequest(
                request.UniqueIdentifier,
                request.Manufacturer,
                request.Model,
                request.Year,
                request.StartingBid,
                request.LoadCapacity.GetValueOrDefault()));

        return null;
    }
}