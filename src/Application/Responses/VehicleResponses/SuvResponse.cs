using Domain.Models.Vehicle.ValueObjects;

namespace Application.Responses.VehicleResponses;

public record SuvResponse(
    Guid UniqueIdentifier,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid,
    int NumberOfSeats)
    : VehicleResponse(UniqueIdentifier, VehicleType.Suv.ToString(), Manufacturer, Model, Year, StartingBid);