using System.Diagnostics.CodeAnalysis;
using Application.Requests.VehicleRequests;
using AutoFixture;
using Domain.Models.Vehicle;
using FluentAssertions;

namespace UnitTests.Application.Requests.VehicleRequests;

[ExcludeFromCodeCoverage]
public class VehicleRequestMapperTests : TestsBase
{
    [Fact]
    public void MapToDomain_ValidTruckRequest_ReturnsTruck()
    {
        //Arrange
        var request = Fixture.Create<TruckRequest>();

        //Act
        var domain = request.MapToDomain();

        //Assert
        (domain is Truck).Should().BeTrue();
        domain.UniqueIdentifier.Should().Be(request.UniqueIdentifier);
        domain.Definition.Manufacturer.Should().Be(request.Manufacturer);
        domain.Definition.Model.Should().Be(request.Model);
        domain.Definition.Year.Should().Be(request.Year);
        domain.StartingBid.Should().Be(request.StartingBid);
        (domain is Truck).Should().BeTrue();
        var truck = domain as Truck;
        truck.LoadCapacity.Should().Be(request.LoadCapacity);
    }

    [Fact]
    public void MapToDomain_ValidSedanRequest_ReturnsSedan()
    {
        //Arrange
        var request = Fixture.Create<SedanRequest>();

        //Act
        var domain = request.MapToDomain();

        //Assert
        (domain is Sedan).Should().BeTrue();
        domain.UniqueIdentifier.Should().Be(request.UniqueIdentifier);
        domain.Definition.Manufacturer.Should().Be(request.Manufacturer);
        domain.Definition.Model.Should().Be(request.Model);
        domain.Definition.Year.Should().Be(request.Year);
        domain.StartingBid.Should().Be(request.StartingBid);
        (domain is Sedan).Should().BeTrue();
        var sedan = domain as Sedan;
        sedan.NumberOfDoors.Should().Be(request.NumberOfDoors);
    }

    [Fact]
    public void MapToDomain_ValidSuvRequest_ReturnsTruck()
    {
        //Arrange
        var request = Fixture.Create<SuvRequest>();

        //Act
        var domain = request.MapToDomain();

        //Assert
        (domain is Suv).Should().BeTrue();
        domain.UniqueIdentifier.Should().Be(request.UniqueIdentifier);
        domain.Definition.Manufacturer.Should().Be(request.Manufacturer);
        domain.Definition.Model.Should().Be(request.Model);
        domain.Definition.Year.Should().Be(request.Year);
        domain.StartingBid.Should().Be(request.StartingBid);
        (domain is Suv).Should().BeTrue();
        var suv = domain as Suv;
        suv.NumberOfSeats.Should().Be(request.NumberOfSeats);
    }
}