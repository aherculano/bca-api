using Domain.Models.Vehicle;

namespace Application.Requests.VehicleRequests;


internal static class VehicleRequestMapper
{
    public static Vehicle MapToDomain(this VehicleRequest request)
    {
        if (request is SuvRequest) return CreateSuv(request as SuvRequest);

        if (request is SedanRequest) return CreateSedan(request as SedanRequest);

        if (request is TruckRequest) return CreateTruck(request as TruckRequest);

        return null;
    }

    private static Suv CreateSuv(SuvRequest request)
    {
        return new Suv(
            request.UniqueIdentifier,
            request.Manufacturer,
            request.Model,
            request.Year,
            request.StartingBid,
            request.NumberOfSeats);
    }

    private static Sedan CreateSedan(SedanRequest request)
    {
        return new Sedan(
            request.UniqueIdentifier,
            request.Manufacturer,
            request.Model,
            request.Year,
            request.StartingBid,
            request.NumberOfDoors);
    }

    private static Truck CreateTruck(TruckRequest request)
    {
        return new Truck(
            request.UniqueIdentifier,
            request.Manufacturer,
            request.Model,
            request.Year,
            request.StartingBid,
            request.LoadCapacity);
    }
}