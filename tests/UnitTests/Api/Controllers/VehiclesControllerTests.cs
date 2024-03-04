using System.Diagnostics.CodeAnalysis;
using Api.Controllers;
using Api.Requests;
using Api.Responses;
using Application.Features.CreateVehicle;
using Application.Features.GetVehicleByUniqueIdentifier;
using Application.Features.ListVehicles;
using Application.Requests.ListVehicleRequests;
using Application.Responses.VehicleResponses;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace UnitTests.Api.Controllers;

[ExcludeFromCodeCoverage]
public class VehiclesControllerTests : TestsBase
{
    private readonly VehiclesController _controller;
    private readonly IMediator _mediator;

    public VehiclesControllerTests()
    {
        _mediator = Fixture.Freeze<IMediator>();
        _controller = new VehiclesController(_mediator);
    }

    [Fact]
    public async void CreateVehicleAsync_ReturnsOk_ReturnsCreated()
    {
        //Arrange
        var request = Fixture.Create<CreateVehicleRequest>();
        var response = Fixture.Create<SuvResponse>();
        _mediator.Send(Arg.Any<CreateVehicleCommand>()).Returns(response);

        //Act
        var result = await _controller.CreateVehicleAsync(request);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var actionResult = result as CreatedAtActionResult;
        actionResult.ActionName.Should().Be("GetVehicleByVehicleId");
        actionResult.RouteValues.Any(x => x.Key == "vehicleUniqueIdentifier").Should().BeTrue();
        actionResult.RouteValues.Any(x => x.Value.Equals(response.UniqueIdentifier)).Should().BeTrue();
        actionResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async void GetVehicleByUniqueId_ReturnsOk_ReturnsOk()
    {
        //Arrange
        var vehicleUniqueIdentifier = Fixture.Create<Guid>();
        var response = Fixture.Create<TruckResponse>();
        _mediator.Send(Arg.Any<GetVehicleByUniqueIdentifierQuery>()).Returns(response);

        //Act
        var result = await _controller.GetVehicleByVehicleIdAsync(vehicleUniqueIdentifier);

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async void ListVehicles_ReturnsOk_ReturnsOk()
    {
        //Arrange
        var request = Fixture.Create<ListVehicleRequest>();
        var response = Fixture.Create<VehicleListResponse>();
        _mediator.Send(Arg.Any<ListVehiclesQuery>()).Returns(response);

        //Act
        var result = await _controller.ListVehiclesAsync(request);

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(response.Vehicles.Select(x => x.MapToApiResponse()));
    }
}