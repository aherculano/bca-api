using Domain.Models.Vehicle.ValueObjects;

namespace Domain.Models.Vehicle;

public abstract class Vehicle : Entity
{
    public Vehicle()
    {
    }

    public Vehicle(
        Guid uniqueIdentifier,
        VehicleDefinition definition,
        decimal startingBid)
    {
        UniqueIdentifier = uniqueIdentifier;
        Definition = definition;
        StartingBid = startingBid;
    }

    public VehicleDefinition Definition { get; set; }
    public decimal StartingBid { get; set; }
}