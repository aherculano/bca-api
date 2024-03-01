namespace Domain.Models.Vehicle;

public abstract class Vehicle
{
    public Vehicle(
        Guid uniqueIdentifier,
        string type,
        string manufacturer,
        string model,
        int year,
        decimal startingBid)
    {
        UniqueIdentifier = uniqueIdentifier;
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        Type = type;
        StartingBid = startingBid;
    }

    public Guid UniqueIdentifier { get; set; }

    public string Type { get; set; }

    public string Manufacturer { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public decimal StartingBid { get; set; }
}