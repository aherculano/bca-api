namespace Application.Requests.VehicleRequests;

public record SuvRequest(
    Guid UniqueIdentifier, 
    string Manufacturer, 
    string Model,
    int Year,
    decimal StartingBid,
    int NumberOfSeats) : VehicleRequest(UniqueIdentifier, Manufacturer, Model, Year, StartingBid);