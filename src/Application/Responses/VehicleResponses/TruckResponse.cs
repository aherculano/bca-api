using Domain.Models.Vehicle.ValueObjects;

namespace Application.Responses.VehicleResponses;

public record TruckResponse(
    Guid UniqueIdentifier,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid,
    decimal LoadCapacity) : VehicleResponse(UniqueIdentifier, VehicleType.Truck.ToString(), Manufacturer, Model, Year,
    StartingBid);