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
            suv.Definition.Manufacturer,
            suv.Definition.Model,
            suv.Definition.Year,
            suv.StartingBid,
            suv.NumberOfSeats);
    }

    private static TruckResponse CreateTruckResponse(Truck truck)
    {
        return new TruckResponse(
            truck.UniqueIdentifier,
            truck.Definition.Manufacturer,
            truck.Definition.Model,
            truck.Definition.Year,
            truck.StartingBid,
            truck.LoadCapacity);
    }

    private static SedanResponse CreateSedanResponse(Sedan sedan)
    {
        return new SedanResponse(
            sedan.UniqueIdentifier,
            sedan.Definition.Manufacturer,
            sedan.Definition.Model,
            sedan.Definition.Year,
            sedan.StartingBid,
            sedan.NumberOfDoors);
    }
}