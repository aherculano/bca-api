namespace Application.Requests.VehicleRequests;

public record TruckRequest(
    Guid UniqueIdentifier,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid,
    decimal LoadCapacity) : VehicleRequest(UniqueIdentifier, Manufacturer, Model, Year, StartingBid);