using Domain.Models.Vehicle.ValueObjects;

namespace Domain.Models.Vehicle;

public static class VehicleFactory
{
    public static Suv CreateSuv(
        Guid uniqueIdentifier,
        string manufacturer,
        string model,
        int year,
        decimal startingBid,
        int numberOfSeats)
    {
        var definition = CreateDefinition(manufacturer, model, year);
        return new Suv(uniqueIdentifier, definition, startingBid, numberOfSeats);
    }

    public static Truck CreateTruck(
        Guid uniqueIdentifier,
        string manufacturer,
        string model,
        int year,
        decimal startingBid,
        decimal loadCapacity)
    {
        var definition = CreateDefinition(manufacturer, model, year);
        return new Truck(uniqueIdentifier, definition, startingBid, loadCapacity);
    }

    public static Sedan CreateSedan(
        Guid uniqueIdentifier,
        string manufacturer,
        string model,
        int year,
        decimal startingBid,
        int numberOfDoors)
    {
        var definition = CreateDefinition(manufacturer, model, year);
        return new Sedan(uniqueIdentifier, definition, startingBid, numberOfDoors);
    }

    private static VehicleDefinition CreateDefinition(
        string manufacturer,
        string model,
        int year)
    {
        return new VehicleDefinition(manufacturer, model, year);
    }
}