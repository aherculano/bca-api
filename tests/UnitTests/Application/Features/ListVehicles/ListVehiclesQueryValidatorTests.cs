using System.Diagnostics.CodeAnalysis;
using Application.Features.ListVehicles;
using Application.Requests.ListVehicleRequests;
using Domain.Models.Vehicle;
using FluentAssertions;

namespace UnitTests.Application.Features.ListVehicles;

[ExcludeFromCodeCoverage]
public class ListVehiclesQueryValidatorTests : TestsBase
{
    private readonly ListVehiclesQueryValidator _validator;

    public ListVehiclesQueryValidatorTests()
    {
        _validator = new ListVehiclesQueryValidator();
    }

    [Fact]
    public void Validate_ValidParameters_ReturnsValid()
    {
        //Arrange
        var request = new ListVehicleRequest(Suv.SuvType, "Toyota", "Corolla", 1995);
        var query = new ListVehiclesQuery(request);

        //Act
        var validationResult = _validator.Validate(query);

        //Assert
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_NullParamenters_ReturnsValid()
    {
        //Arrange
        var request = new ListVehicleRequest(null, null, null, null);
        var query = new ListVehiclesQuery(request);

        //Act
        var validationResult = _validator.Validate(query);

        //Assert
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidParameters_ReturnsInvalid()
    {
        //Arrange
        var request = new ListVehicleRequest("Plane", "", "", 1750);
        var query = new ListVehiclesQuery(request);

        //Act
        var validationResult = _validator.Validate(query);

        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(4);
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Vehicle Type");
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Vehicle Manufacturer");
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Vehicle Model");
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Vehicle Year");
    }
}