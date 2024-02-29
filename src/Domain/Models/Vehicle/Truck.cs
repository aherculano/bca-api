namespace Domain.Models.Vehicle;

public class Truck: Vehicle
{
    public const string TruckType = "Truck";

    public Truck(
        Guid uniqueIdentifier,
        string manufacturer,
        string model,
        int year,
        decimal startingBid,
        decimal loadCapacity) : base(uniqueIdentifier, TruckType, manufacturer, model, year, startingBid)
    {
        LoadCapacity = loadCapacity;
    }

    public decimal LoadCapacity { get; set; }
}