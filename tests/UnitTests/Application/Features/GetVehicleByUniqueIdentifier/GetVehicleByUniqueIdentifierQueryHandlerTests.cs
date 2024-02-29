using System.Diagnostics.CodeAnalysis;
using Application.Errors;
using Application.Features.GetVehicleByUniqueIdentifier;
using Application.Responses.VehicleResponses;
using AutoFixture;
using Domain.Models.Vehicle;
using Domain.Repositories;
using FluentAssertions;
using FluentResults;
using NSubstitute;

namespace UnitTests.Application.Features.GetVehicleByUniqueIdentifier;

[ExcludeFromCodeCoverage]
public class GetVehicleByUniqueIdentifierQueryHandlerTests : TestsBase
{
    private readonly IVehicleRepository _repository;
    private readonly GetVehicleByUniqueIdentifierQueryHandler _handler;

    public GetVehicleByUniqueIdentifierQueryHandlerTests()
    {
        _repository = Fixture.Freeze<IVehicleRepository>();
        _handler = new GetVehicleByUniqueIdentifierQueryHandler(_repository);
    }

    [Fact]
    public async void Handle_RepositoryFails_ThrowsAsync()
    {
        //Arrange
        var query = Fixture.Create<GetVehicleByUniqueIdentifierQuery>();
        _repository.GetVehicleByUniqueIdentifierAsync(query.UniqueIdentifier)
            .Returns(Result.Fail(Fixture.Create<Error>()));

        //Act
        Func<Task<Result<VehicleResponse>>> call = async () => await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _repository.Received(1).GetVehicleByUniqueIdentifierAsync(query.UniqueIdentifier);
    }

    [Fact]
    public async void Handle_RepositoryReturnsNull_ReturnsFail()
    {
        //Arrange
        var query = Fixture.Create<GetVehicleByUniqueIdentifierQuery>();
        _repository.GetVehicleByUniqueIdentifierAsync(query.UniqueIdentifier).Returns(Result.Ok(null as Vehicle));
        
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is VehicleNotFoundError).Should().BeTrue();
        await _repository.Received(1).GetVehicleByUniqueIdentifierAsync(query.UniqueIdentifier);
    }

    [Fact]
    public async void Handle_RepositoryReturnsOk_ReturnsOk()
    {
        //Arrange
        var query = Fixture.Create<GetVehicleByUniqueIdentifierQuery>();
        var suv = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, query.UniqueIdentifier)
            .Create();

        _repository.GetVehicleByUniqueIdentifierAsync(query.UniqueIdentifier).Returns(Result.Ok(suv as Vehicle));
        
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(suv.MapToResponse());
        await _repository.Received(1).GetVehicleByUniqueIdentifierAsync(query.UniqueIdentifier);
    }
}