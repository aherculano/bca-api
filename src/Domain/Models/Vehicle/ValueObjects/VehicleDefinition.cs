namespace Domain.Models.Vehicle.ValueObjects;

public class VehicleDefinition : ValueObject
{
    public VehicleDefinition(
        string manufacturer,
        string model,
        int year)
    {
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
    }

    public string Manufacturer { get; protected set; }

    public string Model { get; protected set; }

    public int Year { get; protected set; }
}