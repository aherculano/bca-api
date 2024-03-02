namespace Domain.Models.Vehicle.ValueObjects;

public class VehicleDefinition
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

    public string Manufacturer { get; }

    public string Model { get; }

    public int Year { get; }
}