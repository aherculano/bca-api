using Domain.Models.Vehicle.ValueObjects;

namespace Domain.Models.Vehicle;

public class Truck : Vehicle
{
    public Truck(
        Guid uniqueIdentifier,
        VehicleDefinition definition,
        decimal startingBid,
        decimal loadCapacity) : base(uniqueIdentifier, definition, startingBid)
    {
        LoadCapacity = loadCapacity;
    }

    public decimal LoadCapacity { get; set; }
}