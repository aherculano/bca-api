using Application.Features.CreateVehicle;
using Application.Requests.VehicleRequests;
using Domain.Models.Vehicle;

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
        if (request.Type.Equals(Suv.SuvType, StringComparison.InvariantCultureIgnoreCase))
            return new CreateVehicleCommand(new SuvRequest(
                request.UniqueIdentifier,
                request.Manufacturer,
                request.Model,
                request.Year,
                request.StartingBid,
                request.NumberOfSeats.GetValueOrDefault()));

        if (request.Type.Equals(Sedan.SedanType, StringComparison.InvariantCultureIgnoreCase))
            return new CreateVehicleCommand(new SedanRequest(
                request.UniqueIdentifier,
                request.Manufacturer,
                request.Model,
                request.Year,
                request.StartingBid,
                request.NumberOfDoors.GetValueOrDefault()));

        if (request.Type.Equals(Truck.TruckType, StringComparison.InvariantCultureIgnoreCase))
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