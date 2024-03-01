namespace Application.Requests.VehicleRequests;

public record SedanRequest(
    Guid UniqueIdentifier,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid,
    int NumberOfDoors) : VehicleRequest(UniqueIdentifier, Manufacturer, Model, Year, StartingBid);