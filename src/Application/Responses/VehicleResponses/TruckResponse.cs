using Domain.Models.Vehicle;

namespace Application.Responses.VehicleResponses;

public record TruckResponse(
    Guid UniqueIdentifier,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid,
    decimal LoadCapacity) : VehicleResponse(UniqueIdentifier, Truck.TruckType, Manufacturer, Model, Year, StartingBid);