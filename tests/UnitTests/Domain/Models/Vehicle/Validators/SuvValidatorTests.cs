using System.Diagnostics.CodeAnalysis;
using Domain.Models.Vehicle;
using Domain.Models.Vehicle.Validators;
using Domain.Models.Vehicle.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.Models.Vehicle.Validators;

[ExcludeFromCodeCoverage]
public class SuvValidatorTests : TestsBase
{
    private readonly SuvValidator _validator;

    public SuvValidatorTests()
    {
        _validator = new SuvValidator();
    }
    
    
    [Fact]
    public void Validate_InvalidSuv_IsInvalid()
    {
        //Arrange
        var suv = new Suv(
            Guid.Empty,
            new VehicleDefinition(
                "",
                "",
                1800),
            0,
            1);
        
        //Act
        var result = _validator.Validate(suv);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().Be(6);
        result.Errors.Any(x => x.ErrorMessage == "Vehicle Unique Identifier Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Manufacturer Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Model Cannot Be Empty").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Invalid Year").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Starting Bid Must Be Greater Then 0").Should().BeTrue();
        result.Errors.Any(x => x.ErrorMessage == "Number Of Seats Needs To Be Between 2 and 9").Should().BeTrue();
    }
}