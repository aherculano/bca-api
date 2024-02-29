using Domain.Models.Vehicle;

namespace Application.Responses.VehicleResponses;

public record SuvResponse(
    Guid UniqueIdentifier, 
    string Manufacturer,
    string Model, 
    int Year, 
    decimal StartingBid,
    int NumberOfSeats) :VehicleResponse(UniqueIdentifier, Suv.SuvType, Manufacturer, Model, Year, StartingBid);