using Application.Responses.VehicleResponses;

namespace Api.Responses;

public record VehicleApiResponse
{
    public Guid UniqueIdentifier { get; init; }

    public string Type { get; init; }

    public string Manufacturer { get; init; }

    public string Model { get; init; }

    public int Year { get; init; }

    public decimal StartingBid { get; init; }

    public int? NumberOfDoors { get; init; }

    public int? NumberOfSeats { get; init; }

    public decimal? LoadCapacity { get; init; }
}

internal static class VehicleApiResponseMapper
{
    public static VehicleApiResponse MapToApiResponse(this VehicleResponse source)
    {
        if (source is TruckResponse)
            return new VehicleApiResponse
            {
                UniqueIdentifier = source.UniqueIdentifier,
                Manufacturer = source.Manufacturer,
                Model = source.Model,
                Type = source.Type,
                Year = source.Year,
                StartingBid = source.StartingBid,
                LoadCapacity = (source as TruckResponse).LoadCapacity
            };
        if (source is SedanResponse)
            return new VehicleApiResponse
            {
                UniqueIdentifier = source.UniqueIdentifier,
                Manufacturer = source.Manufacturer,
                Model = source.Model,
                Type = source.Type,
                Year = source.Year,
                StartingBid = source.StartingBid,
                NumberOfDoors = (source as SedanResponse).NumberOfDoors
            };
        if (source is SuvResponse)
            return new VehicleApiResponse
            {
                UniqueIdentifier = source.UniqueIdentifier,
                Manufacturer = source.Manufacturer,
                Model = source.Model,
                Type = source.Type,
                Year = source.Year,
                StartingBid = source.StartingBid,
                NumberOfSeats = (source as SuvResponse).NumberOfSeats
            };

        return null;
    }
}