using System.Diagnostics.CodeAnalysis;
using Application.Errors;
using Application.Features.ListVehicles;
using Application.Responses.VehicleResponses;
using AutoFixture;
using Domain.Models.Vehicle;
using Domain.Repositories;
using FluentAssertions;
using FluentResults;
using NSubstitute;

namespace UnitTests.Application.Features.ListVehicles;

[ExcludeFromCodeCoverage]
public class ListVehiclesQueryHandlerTests : TestsBase
{
    private readonly IVehicleRepository _repository;
    private readonly ListVehiclesQueryHandler _handler;

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
        _repository.ListVehiclesAsync(Arg.Any<Func<Vehicle, bool>>())
            .Returns(Result.Fail(Fixture.Create<Error>()));

        //Act
        Func<Task<Result<IEnumerable<VehicleResponse>>>> call = async 
            () => await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _repository.Received(1).ListVehiclesAsync(Arg.Any<Func<Vehicle, bool>>());
    }

    [Fact]
    public async void Handle_RepositoryReturnsEmpty_ResultFail()
    {
        //Arrange
        var query = Fixture.Create<ListVehiclesQuery>();
        _repository.ListVehiclesAsync(Arg.Any<Func<Vehicle, bool>>())
            .Returns(Result.Ok(Fixture.CreateMany<Suv>(0) as IEnumerable<Vehicle>));

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is VehicleNotFoundError);
        await _repository.Received(1).ListVehiclesAsync(Arg.Any<Func<Vehicle, bool>>());
    }

    [Fact]
    public async void Handle_RepositoryReturnsOk_ReturnsOk()
    {
        //Arrange
        var query = Fixture.Create<ListVehiclesQuery>();
        var vehicles = Fixture.Build<Suv>()
            .With(x => x.Manufacturer, query.Request.Manufacturer)
            .With(x => x.Year, query.Request.Year)
            .With(x => x.Model, query.Request.Model)
            .With(x => x.Type, query.Request.Type)
            .CreateMany(3);

        _repository.ListVehiclesAsync(Arg.Any<Func<Vehicle, bool>>())
            .Returns(Result.Ok(vehicles as IEnumerable<Vehicle>));

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(vehicles.Select(x => x.MapToResponse()));
    }
}