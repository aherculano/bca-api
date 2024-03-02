using System.Diagnostics.CodeAnalysis;
using Application.Responses.VehicleResponses;
using AutoFixture;
using Domain.Models.Vehicle;
using FluentAssertions;

namespace UnitTests.Application.Responses.VehicleResponses;

[ExcludeFromCodeCoverage]
public class VehicleResponseMapperTests : TestsBase
{
    [Fact]
    public void MapToResponse_ValidSuv_ReturnsSuvResponse()
    {
        //Arrange
        var suv = Fixture.Create<Suv>();

        //Act
        var response = suv.MapToResponse();

        //Assert
        (response is SuvResponse).Should().BeTrue();
        response.UniqueIdentifier.Should().Be(suv.UniqueIdentifier);
        response.Manufacturer.Should().Be(suv.Definition.Manufacturer);
        response.Model.Should().Be(suv.Definition.Model);
        response.Year.Should().Be(suv.Definition.Year);
        response.StartingBid.Should().Be(suv.StartingBid);
        (response as SuvResponse).NumberOfSeats.Should().Be(suv.NumberOfSeats);
    }

    [Fact]
    public void MapToResponse_ValidTruck_ReturnsTruckResponse()
    {
        //Arrange
        var truck = Fixture.Create<Truck>();

        //Act
        var response = truck.MapToResponse();

        //Assert
        (response is TruckResponse).Should().BeTrue();
        response.UniqueIdentifier.Should().Be(truck.UniqueIdentifier);
        response.Manufacturer.Should().Be(truck.Definition.Manufacturer);
        response.Model.Should().Be(truck.Definition.Model);
        response.Year.Should().Be(truck.Definition.Year);
        response.StartingBid.Should().Be(truck.StartingBid);
        (response as TruckResponse).LoadCapacity.Should().Be(truck.LoadCapacity);
    }

    [Fact]
    public void MapToResponse_ValidSedan_ReturnsSedanResponse()
    {
        //Arrange
        var sedan = Fixture.Create<Sedan>();

        //Act
        var response = sedan.MapToResponse();

        //Assert
        (response is SedanResponse).Should().BeTrue();
        response.UniqueIdentifier.Should().Be(sedan.UniqueIdentifier);
        response.Manufacturer.Should().Be(sedan.Definition.Manufacturer);
        response.Model.Should().Be(sedan.Definition.Model);
        response.Year.Should().Be(sedan.Definition.Year);
        response.StartingBid.Should().Be(sedan.StartingBid);
        (response as SedanResponse).NumberOfDoors.Should().Be(sedan.NumberOfDoors);
    }
}