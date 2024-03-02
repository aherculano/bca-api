using Domain.Models.Vehicle.ValueObjects;

namespace Domain.Models.Vehicle;

public class Suv : Vehicle
{
    public Suv()
    {
        
    }
    public Suv(
        Guid uniqueIdentifier,
        VehicleDefinition definition,
        decimal startingBid,
        int numberOfSeats) : base(uniqueIdentifier, definition, startingBid)
    {
        NumberOfSeats = numberOfSeats;
    }

    public int NumberOfSeats { get; set; }
}