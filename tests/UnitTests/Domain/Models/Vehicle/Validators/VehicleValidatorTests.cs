using System.Diagnostics.CodeAnalysis;
using Domain.Models.Vehicle;
using Domain.Models.Vehicle.Validators;
using Domain.Models.Vehicle.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.Models.Vehicle.Validators;

[ExcludeFromCodeCoverage]
public class VehicleValidatorTests : TestsBase
{
    private readonly VehicleValidator _vehicleValidatorTests;

    public VehicleValidatorTests()
    {
        _vehicleValidatorTests = new VehicleValidator();
    }

    [Fact]
    public void Validate_InvalidVehicle_IsInvalid()
    {
        //Arrange
        var suv = new Suv(
            Guid.Empty,
            new VehicleDefinition(
                "",
                "",
                1800),
            0,
            3);
        
        //Act
        var result = _vehicleValidatorTests.Validate(suv);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(5);
        result.Errors.Any(x => x.ErrorMessage == "Vehicle Unique Identifier Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Manufacturer Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Model Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Invalid Year").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Starting Bid Must Be Greater Then 0").Should().BeTrue();
    }
}