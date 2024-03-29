﻿using System.Diagnostics.CodeAnalysis;
using Application.Features.ListVehicles;
using Application.Responses.VehicleResponses;
using AutoFixture;
using Domain.Errors;
using Domain.Models.Vehicle;
using Domain.Models.Vehicle.ValueObjects;
using Domain.Repositories;
using FluentAssertions;
using FluentResults;
using NSubstitute;

namespace UnitTests.Application.Features.ListVehicles;

[ExcludeFromCodeCoverage]
public class ListVehiclesQueryHandlerTests : TestsBase
{
    private readonly ListVehiclesQueryHandler _handler;
    private readonly IVehicleRepository _repository;

    public ListVehiclesQueryHandlerTests()
    {
        _repository = Fixture.Freeze<IVehicleRepository>();
        _handler = new ListVehiclesQueryHandler(_repository);
    }

    [Fact]
    public async void Handle_RepositoryFails_ThrowsAsync()
    {
        //Arrange
        var query = Fixture.Create<ListVehiclesQuery>();
        _repository
            .ListVehiclesAsync(Arg.Any<VehicleType?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<int?>())
            .Returns(Result.Fail(Fixture.Create<Error>()));

        //Act
        var call = async
            () => await _handler.Handle(query, CancellationToken.None);

        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _repository.Received(1)
            .ListVehiclesAsync(Arg.Any<VehicleType?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<int?>());
    }

    [Fact]
    public async void Handle_RepositoryReturnsEmpty_ResultFail()
    {
        //Arrange
        var query = Fixture.Create<ListVehiclesQuery>();
        _repository.ListVehiclesAsync(Arg.Any<VehicleType?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<int?>())
            .Returns(Result.Ok(Fixture.CreateMany<Suv>(0) as IEnumerable<Vehicle>));

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is NotFoundError);
        await _repository.Received(1)
            .ListVehiclesAsync(Arg.Any<VehicleType?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<int?>());
    }

    [Fact]
    public async void Handle_RepositoryReturnsOk_ReturnsOk()
    {
        //Arrange
        var query = Fixture.Create<ListVehiclesQuery>();
        var definition = new VehicleDefinition(
            query.Request.Manufacturer,
            query.Request.Model,
            query.Request.Year.Value);

        var vehicles = Fixture.Build<Suv>()
            .With(x => x.Definition, definition)
            .CreateMany(3);

        _repository.ListVehiclesAsync(Arg.Any<VehicleType?>(), Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<int?>())
            .Returns(Result.Ok(vehicles as IEnumerable<Vehicle>));

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new VehicleListResponse(vehicles.Select(x => x.MapToResponse())));
    }
}