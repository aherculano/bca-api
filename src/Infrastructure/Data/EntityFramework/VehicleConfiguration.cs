using Domain.Models.Vehicle;
using Domain.Models.Vehicle.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFramework;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.OwnsOne(v => v.Definition);
        builder.HasDiscriminator<string>("VehicleType")
            .HasValue<Truck>("Truck")
            .HasValue<Sedan>("Sedan")
            .HasValue<Suv>("Suv");
    }
}