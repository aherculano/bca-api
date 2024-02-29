namespace Domain.Models.Vehicle;

public class Sedan : Vehicle
{
    public const string SedanType = "Sedan";

    public Sedan(
        Guid uniqueIdentifier,
        string manufacturer,
        string model,
        int year,
        decimal startingBid,
        int numberOfDoors) : base(uniqueIdentifier, SedanType, manufacturer, model, year, startingBid)
    {
        NumberOfDoors = numberOfDoors;
    }

    public int NumberOfDoors { get; set; }
}