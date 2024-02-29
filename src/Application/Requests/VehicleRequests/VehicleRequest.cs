namespace Application.Requests.VehicleRequests;

public abstract record VehicleRequest(
    Guid UniqueIdentifier,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid);