namespace Application.Requests.ListVehicleRequests;

public record ListVehicleRequest(
    string? Type,
    string? Manufacturer,
    string? Model,
    int? Year);