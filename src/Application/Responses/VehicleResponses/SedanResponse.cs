using Domain.Models.Vehicle;

namespace Application.Responses.VehicleResponses;

public record SedanResponse(
    Guid UniqueIdentifier, 
    string Manufacturer, 
    string Model,
    int Year, 
    decimal StartingBid,
    int NumberOfDoors) :VehicleResponse(UniqueIdentifier, Sedan.SedanType, Manufacturer, Model, Year, StartingBid);