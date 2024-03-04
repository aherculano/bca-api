using System.Diagnostics.CodeAnalysis;
using Domain.Models.Vehicle;
using Domain.Models.Vehicle.Validators;
using Domain.Models.Vehicle.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.Models.Vehicle.Validators;

[ExcludeFromCodeCoverage]
public class SedanValidatorTests : TestsBase
{
    private readonly SedanValidator _validator;

    public SedanValidatorTests()
    {
        _validator = new SedanValidator();
    }

    [Fact]
    public void Validate_InvalidSedan_IsInvalid()
    {
        //Arrange
        var sedan = new Sedan(
            Guid.Empty,
            new VehicleDefinition(
                "",
                "",
                1800),
            0,
            1);

        //Act
        var result = _validator.Validate(sedan);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(6);
        result.Errors.Any(x => x.ErrorMessage == "Vehicle Unique Identifier Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Manufacturer Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Model Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Invalid Year").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Starting Bid Must Be Greater Then 0").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Number Of Doors Needs To Be Between 2 and 5").Should().BeTrue();
    }
}