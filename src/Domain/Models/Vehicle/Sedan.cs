using Domain.Models.Vehicle.ValueObjects;

namespace Domain.Models.Vehicle;

public class Sedan : Vehicle
{
    public Sedan()
    {
        
    }
    
    public Sedan(
        Guid uniqueIdentifier,
        VehicleDefinition definition,
        decimal startingBid,
        int numberOfDoors) : base(uniqueIdentifier, definition, startingBid)
    {
        NumberOfDoors = numberOfDoors;
    }

    public int NumberOfDoors { get; set; }
}