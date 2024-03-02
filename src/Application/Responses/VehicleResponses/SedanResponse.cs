using Domain.Models.Vehicle.ValueObjects;

namespace Application.Responses.VehicleResponses;

public record SedanResponse(
    Guid UniqueIdentifier,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid,
    int NumberOfDoors) : VehicleResponse(UniqueIdentifier, VehicleType.Sedan.ToString(), Manufacturer, Model, Year,
    StartingBid);