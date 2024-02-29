namespace Domain.Models.Vehicle;

public class Suv:Vehicle
{
    public const string SuvType = "SUV";

    public Suv(
        Guid uniqueIdentifier,
        string manufacturer,
        string model,
        int year,
        decimal startingBid,
        int numberOfSeats) : base(uniqueIdentifier, SuvType, manufacturer, model, year, startingBid)
    {
        NumberOfSeats = numberOfSeats;
    }

    public int NumberOfSeats { get; set; }
}