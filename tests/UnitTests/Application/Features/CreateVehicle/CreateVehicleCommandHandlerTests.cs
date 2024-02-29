using System.Diagnostics.CodeAnalysis;
using Application.Errors;
using Application.Features.CreateVehicle;
using Application.Requests.VehicleRequests;
using Application.Responses.VehicleResponses;
using AutoFixture;
using Domain.Models.Vehicle;
using Domain.Repositories;
using FluentAssertions;
using FluentResults;
using NSubstitute;

namespace UnitTests.Application.Features.CreateVehicle;

[ExcludeFromCodeCoverage]
public class CreateVehicleCommandHandlerTests : TestsBase
{
    private readonly IVehicleRepository _repository;
    private readonly CreateVehicleCommandHandler _handler;
    
    public CreateVehicleCommandHandlerTests()
    {
        _repository = Fixture.Freeze<IVehicleRepository>();
        _handler = new CreateVehicleCommandHandler(_repository);
    }

    [Fact]
    public async void Handle_GetFails_ThrowsAsync()
    {
        //Arrange
        var command = new CreateVehicleCommand(Fixture.Create<SuvRequest>());
        _repository.GetVehicleByUniqueIdentifierAsync(command.Request.UniqueIdentifier)
            .Returns(Result.Fail(Fixture.Create<Error>()));
        
        //Act
        Func<Task<Result<VehicleResponse>>> call = async () => await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _repository.Received(1).GetVehicleByUniqueIdentifierAsync(command.Request.UniqueIdentifier);
        await _repository.Received(0).CreateVheicleAsync(Arg.Any<Vehicle>());
    }

    [Fact]
    public async void Handle_VehicleAlreadyExists_ReturnsFail()
    {
        //Arrange
        var command = new CreateVehicleCommand(Fixture.Create<SuvRequest>());
        var vehicle = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, command.Request.UniqueIdentifier)
            .Create();
        
        _repository.GetVehicleByUniqueIdentifierAsync(command.Request.UniqueIdentifier)
            .Returns(Result.Ok(vehicle as Vehicle));
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is VehicleAlreadyExistsError);
        await _repository.Received(1).GetVehicleByUniqueIdentifierAsync(command.Request.UniqueIdentifier);
        await _repository.Received(0).CreateVheicleAsync(Arg.Any<Vehicle>());
    }

    [Fact]
    public async void Handle_RepositoryCreateFails_ThrowsAsync()
    {
        //Arrange
        var command = new CreateVehicleCommand(Fixture.Create<SuvRequest>());
        
        _repository.GetVehicleByUniqueIdentifierAsync(command.Request.UniqueIdentifier)
            .Returns(Result.Ok(null as Vehicle));

        _repository.CreateVheicleAsync(Arg.Any<Vehicle>())
            .Returns(Result.Fail(Fixture.Create<Error>()));
            
        //Act
        Func<Task<Result<VehicleResponse>>> call = async () => await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _repository.Received(1).GetVehicleByUniqueIdentifierAsync(command.Request.UniqueIdentifier);
        await _repository.Received(1).CreateVheicleAsync(Arg.Any<Vehicle>());
    }
    
    [Fact]
    public async void Handle_CreateSucceeds_ResultOk()
    {
        //Arrange
        var command = new CreateVehicleCommand(Fixture.Create<SuvRequest>());

        var suv = command.Request.MapToDomain();
        
        _repository.GetVehicleByUniqueIdentifierAsync(command.Request.UniqueIdentifier)
            .Returns(Result.Ok(null as Vehicle));

        _repository.CreateVheicleAsync(Arg.Any<Vehicle>())
            .Returns(Result.Ok(suv as Vehicle));
            
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(suv.MapToResponse());
        await _repository.Received(1).GetVehicleByUniqueIdentifierAsync(command.Request.UniqueIdentifier);
        await _repository.Received(1).CreateVheicleAsync(Arg.Any<Vehicle>());
    }
}