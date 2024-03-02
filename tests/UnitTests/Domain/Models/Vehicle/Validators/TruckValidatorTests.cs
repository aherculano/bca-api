using System.Diagnostics.CodeAnalysis;
using Domain.Models.Vehicle;
using Domain.Models.Vehicle.Validators;
using Domain.Models.Vehicle.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.Models.Vehicle.Validators;

[ExcludeFromCodeCoverage]
public class TruckValidatorTests : TestsBase
{
    private readonly TruckValidator _validator;

    public TruckValidatorTests()
    {
        _validator = new TruckValidator();
    }
    
    
    [Fact]
    public void Validate_InvalidTruck_IsInvalid()
    {
        //Arrange
        var truck = new Truck(
            Guid.Empty,
            new VehicleDefinition(
                "",
                "",
                1800),
            0,
            0);
        
        //Act
        var result = _validator.Validate(truck);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(6);
        result.Errors.Any(x => x.ErrorMessage == "Vehicle Unique Identifier Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Manufacturer Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Model Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Invalid Year").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Starting Bid Must Be Greater Then 0").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Load Capacity Must Be Greater Then 0").Should().BeTrue();
    }
}