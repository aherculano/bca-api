using System.Diagnostics.CodeAnalysis;
using Application.Errors;
using Application.Features.CreateAuction;
using AutoFixture;
using Domain.Models.Auction;
using Domain.Models.Vehicle;
using Domain.Repositories;
using FluentAssertions;
using FluentResults;
using NSubstitute;

namespace UnitTests.Application.Features.CreateAuction;

[ExcludeFromCodeCoverage]
public class CreateAuctionCommandHandlerTests : TestsBase
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAuctionRepository _auctionRepository;
    private readonly CreateAuctionCommandHandler _handler;

    public CreateAuctionCommandHandlerTests()
    {
        _vehicleRepository = Fixture.Freeze<IVehicleRepository>();
        _auctionRepository = Fixture.Freeze<IAuctionRepository>();
        _handler = new CreateAuctionCommandHandler(
            _vehicleRepository,
            _auctionRepository);
    }

    [Fact]
    public async void Handle_GetVehicleFails_ThrowsAsync()
    {
        //Arrange
        var command = Fixture.Create<CreateAuctionCommand>();
        _vehicleRepository.GetVehicleByUniqueIdentifierAsync(Arg.Any<Guid>())
            .Returns(Result.Fail(Fixture.Create<Error>()));
        
        //Act
        var call = async () => await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier);
        await _auctionRepository.Received(0).GetAuctionsByVehicleUniqueIdentifier(Arg.Any<Guid>());
        await _auctionRepository.Received(0).CreateAuction(Arg.Any<Auction>());
    }

    [Fact]
    public async void Handle_VehicleDoesNotExist_ReturnsFail()
    {
        //Arrange
        var command = Fixture.Create<CreateAuctionCommand>();
        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier)
            .Returns(Result.Ok(null as Vehicle));
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is NotFoundError).Should().BeTrue();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier);
        await _auctionRepository.Received(0).GetAuctionsByVehicleUniqueIdentifier(Arg.Any<Guid>());
        await _auctionRepository.Received(0).CreateAuction(Arg.Any<Auction>());
    }

    [Fact]
    public async void Handle_AuctionExistsWithOpenStatus_ReturnsFail()
    {
        //Arrange
        var command = Fixture.Create<CreateAuctionCommand>();
        var suv = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, command.VehicleUniqueIdentifier)
            .Create();

        var auction = Fixture.Build<Auction>()
            .With(x => x.Status, AuctionStatus.Open)
            .CreateMany(1);
        
        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier)
            .Returns(Result.Ok(suv as Vehicle));
        
        _auctionRepository
            .GetAuctionsByVehicleUniqueIdentifier(command.VehicleUniqueIdentifier)
            .Returns(Result.Ok(auction));
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is AlreadyExistsError).Should().BeTrue();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifier(command.VehicleUniqueIdentifier);
        await _auctionRepository.Received(0).CreateAuction(Arg.Any<Auction>());
    }

    [Fact]
    public async void Handle_GetAuctionFails_ThrowsAsync()
    {
        //Arrange
        var command = Fixture.Create<CreateAuctionCommand>();
        var suv = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, command.VehicleUniqueIdentifier)
            .Create();
        
        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier)
            .Returns(Result.Ok(suv as Vehicle));
        
        _auctionRepository
            .GetAuctionsByVehicleUniqueIdentifier(command.VehicleUniqueIdentifier)
            .Returns(Result.Fail(Fixture.Create<Error>()));
        
        //Act
        var call = async () =>  await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifier(command.VehicleUniqueIdentifier);
        await _auctionRepository.Received(0).CreateAuction(Arg.Any<Auction>());
    }

    [Fact]
    public async void Handle_CreateAuctionFails_ThrowsAsync()
    {
        //Arrange
        var command = Fixture.Create<CreateAuctionCommand>();
        var suv = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, command.VehicleUniqueIdentifier)
            .Create();

        var auctions = Fixture.CreateMany<Auction>(0);
        
        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier)
            .Returns(Result.Ok(suv as Vehicle));
        
        _auctionRepository
            .GetAuctionsByVehicleUniqueIdentifier(command.VehicleUniqueIdentifier)
            .Returns(Result.Ok(auctions));

        _auctionRepository.CreateAuction(Arg.Any<Auction>())
            .Returns(Result.Fail(Fixture.Create<Error>()));
        
        //Act
        var call = async () =>  await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifier(command.VehicleUniqueIdentifier);
        await _auctionRepository.Received(1).CreateAuction(Arg.Any<Auction>());
    }

    [Fact]
    public async void Handle_CreateAuctionWorks_ResultOk()
    {
        //Arrange
        var command = Fixture.Create<CreateAuctionCommand>();
        var suv = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, command.VehicleUniqueIdentifier)
            .Create();
        
        var auctions = Fixture.CreateMany<Auction>(0);

        var createdAuction = new Auction(command.VehicleUniqueIdentifier,
            suv.StartingBid);
        
        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier)
            .Returns(Result.Ok(suv as Vehicle));
        
        _auctionRepository
            .GetAuctionsByVehicleUniqueIdentifier(command.VehicleUniqueIdentifier)
            .Returns(Result.Ok(auctions));

        _auctionRepository.CreateAuction(Arg.Any<Auction>())
            .Returns(Result.Ok(createdAuction));
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UniqueIdentifier.Should().Be(createdAuction.UniqueIdentifier);
        result.Value.VehicleUniqueIdentifier.Should().Be(suv.UniqueIdentifier);
        result.Value.Bids.Should().BeEmpty();
        result.Value.StartingBid.Should().Be(suv.StartingBid);
        result.Value.Status.Should().Be(AuctionStatus.Closed.ToString());
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(command.VehicleUniqueIdentifier);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifier(command.VehicleUniqueIdentifier);
        await _auctionRepository.Received(1).CreateAuction(Arg.Any<Auction>());
    }
}