namespace Application.Responses.VehicleResponses;

public abstract record VehicleResponse(
    Guid UniqueIdentifier,
    string Type,
    string Manufacturer,
    string Model,
    int Year,
    decimal StartingBid);