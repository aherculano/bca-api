using Domain.Models.Vehicle;

namespace Application.Responses.VehicleResponses;

internal static class VehicleResponseMapper
{
    public static VehicleResponse MapToResponse(this Vehicle vehicle)
    {
        if (vehicle is null) return null;

        if (vehicle is Suv) return CreateSuvResponse(vehicle as Suv);

        if (vehicle is Truck) return CreateTruckResponse(vehicle as Truck);

        if (vehicle is Sedan) return CreateSedanResponse(vehicle as Sedan);

        return null;
    }

    private static SuvResponse CreateSuvResponse(Suv suv)
    {
        return new SuvResponse(
            suv.UniqueIdentifier,
            suv.Manufacturer,
            suv.Model,
            suv.Year,
            suv.StartingBid,
            suv.NumberOfSeats);
    }

    private static TruckResponse CreateTruckResponse(Truck truck)
    {
        return new TruckResponse(
            truck.UniqueIdentifier,
            truck.Manufacturer,
            truck.Model,
            truck.Year,
            truck.StartingBid,
            truck.LoadCapacity);
    }

    private static SedanResponse CreateSedanResponse(Sedan sedan)
    {
        return new SedanResponse(
            sedan.UniqueIdentifier,
            sedan.Manufacturer,
            sedan.Model,
            sedan.Year,
            sedan.StartingBid,
            sedan.NumberOfDoors);
    }
}