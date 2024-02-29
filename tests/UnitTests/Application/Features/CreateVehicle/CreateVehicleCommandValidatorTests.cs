using System.Diagnostics.CodeAnalysis;
using Application.Features.CreateVehicle;
using Application.Requests.VehicleRequests;
using AutoFixture;
using FluentAssertions;

namespace UnitTests.Application.Features.CreateVehicle;

[ExcludeFromCodeCoverage]
public class CreateVehicleCommandValidatorTests : TestsBase
{
    private readonly CreateVehicleCommandValidator _validator;
    
    public CreateVehicleCommandValidatorTests()
    {
        _validator = new CreateVehicleCommandValidator();
    }

    [Fact]
    public void Validate_ValidSuv_IsValid()
    {
        //Arrange
        var suvRequest = Fixture.Build<SuvRequest>()
            .With(x => x.Year, 2000)
            .With(x => x.NumberOfSeats, 5)
            .Create();

        var command = new CreateVehicleCommand(suvRequest);

        //Act
        var validationResult = _validator.Validate(command);

        //Assert
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidSuv_IsValidFalse()
    {
        //Arrange
        var suvRequest = Fixture.Build<SuvRequest>()
            .With(x => x.UniqueIdentifier, Guid.Empty)
            .With(x => x.Manufacturer, string.Empty)
            .With(x => x.Model, string.Empty)
            .With(x => x.Year, 0)
            .With(x => x.StartingBid, 0)
            .With(x => x.NumberOfSeats, 0)
            .Create();

        var command = new CreateVehicleCommand(suvRequest);

        //Act
        var validationResult = _validator.Validate(command);

        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(6);
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Unique Identifier").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Manufacturer").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Model").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Year").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Starting Bid").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Suv Properties").Should().BeTrue();
    }

    [Fact]
    public void Validate_ValidTruck_IsValid()
    {
        //Arrange
        var truckRequest = Fixture.Build<TruckRequest>()
            .With(x => x.Year, 2000)
            .With(x => x.LoadCapacity, 555.5m)
            .Create();

        var command = new CreateVehicleCommand(truckRequest);

        //Act
        var validationResult = _validator.Validate(command);

        //Assert
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidTruckRequest_IsValidFalse()
    {
        //Arrange
        var suvRequest = Fixture.Build<TruckRequest>()
            .With(x => x.UniqueIdentifier, Guid.Empty)
            .With(x => x.Manufacturer, string.Empty)
            .With(x => x.Model, string.Empty)
            .With(x => x.Year, 0)
            .With(x => x.StartingBid, 0)
            .With(x => x.LoadCapacity, 0)
            .Create();

        var command = new CreateVehicleCommand(suvRequest);

        //Act
        var validationResult = _validator.Validate(command);

        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(6);
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Unique Identifier").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Manufacturer").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Model").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Year").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Starting Bid").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Truck Properties").Should().BeTrue();
    }

    [Fact]
    public void Validate_ValidSedan_IsValid()
    {
        //Arrange
        var sedanRequest = Fixture.Build<SedanRequest>()
            .With(x => x.Year, 2000)
            .With(x => x.NumberOfDoors, 3)
            .Create();

        var command = new CreateVehicleCommand(sedanRequest);

        //Act
        var validationResult = _validator.Validate(command);

        //Assert
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidSedanRequest_IsValidFalse()
    {
        //Arrange
        var suvRequest = Fixture.Build<SedanRequest>()
            .With(x => x.UniqueIdentifier, Guid.Empty)
            .With(x => x.Manufacturer, string.Empty)
            .With(x => x.Model, string.Empty)
            .With(x => x.Year, 0)
            .With(x => x.StartingBid, 0)
            .With(x => x.NumberOfDoors, 0)
            .Create();

        var command = new CreateVehicleCommand(suvRequest);

        //Act
        var validationResult = _validator.Validate(command);

        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(6);
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Unique Identifier").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Manufacturer").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Model").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Year").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Starting Bid").Should().BeTrue();
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Sedan Properties").Should().BeTrue();
    }
}