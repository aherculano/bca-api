using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using Domain.Errors;
using Domain.Models.Auction;
using Domain.Models.Auction.ValueObjects;
using Domain.Models.Vehicle;
using Domain.Repositories;
using Domain.Services.Auctions;
using FluentAssertions;
using FluentResults;
using NSubstitute;

namespace UnitTests.Domain.Services.Auctions;

[ExcludeFromCodeCoverage]
public class AuctionServiceTests : TestsBase
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IAuctionService _auctionService;
    private readonly IVehicleRepository _vehicleRepository;

    public AuctionServiceTests()
    {
        _vehicleRepository = Fixture.Freeze<IVehicleRepository>();
        _auctionRepository = Fixture.Freeze<IAuctionRepository>();
        _auctionService = new AuctionService(_vehicleRepository, _auctionRepository);
    }

    [Fact]
    public async void CreateAuction_GetVehicleFails_ThrowsAsync()
    {
        //Arrange
        var vehicleUniqueIdentifier = Fixture.Create<Guid>();
        _vehicleRepository.GetVehicleByUniqueIdentifierAsync(Arg.Any<Guid>())
            .Returns(Result.Fail(Fixture.Create<Error>()));

        //Act
        var call = async () => await _auctionService.CreateAuction(vehicleUniqueIdentifier);

        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(0).GetAuctionsByVehicleUniqueIdentifierAsync(Arg.Any<Guid>());
        await _auctionRepository.Received(0).CreateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void CreateAuction_VehicleDoesNotExist_ReturnsFail()
    {
        //Arrange
        var vehicleUniqueIdentifier = Fixture.Create<Guid>();
        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Ok(null as Vehicle));

        //Act
        var result = await _auctionService.CreateAuction(vehicleUniqueIdentifier);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is NotFoundError).Should().BeTrue();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(0).GetAuctionsByVehicleUniqueIdentifierAsync(Arg.Any<Guid>());
        await _auctionRepository.Received(0).CreateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void CreateAuction_AuctionExistsWithOpenStatus_ReturnsFail()
    {
        //Arrange
        var vehicleUniqueIdentifier = Fixture.Create<Guid>();
        var suv = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, vehicleUniqueIdentifier)
            .Create();

        var auction = Fixture.Build<Auction>()
            .With(x => x.Status, AuctionStatus.Open)
            .CreateMany(1);

        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Ok(suv as Vehicle));

        _auctionRepository
            .GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Ok(auction));

        //Act
        var result = await _auctionService.CreateAuction(vehicleUniqueIdentifier);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is AlreadyExistsError).Should().BeTrue();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(0).CreateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void CreateAuction_GetAuctionFails_ThrowsAsync()
    {
        //Arrange
        var vehicleUniqueIdentifier = Fixture.Create<Guid>();
        var suv = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, vehicleUniqueIdentifier)
            .Create();

        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Ok(suv as Vehicle));

        _auctionRepository
            .GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Fail(Fixture.Create<Error>()));

        //Act
        var call = async () => await _auctionService.CreateAuction(vehicleUniqueIdentifier);

        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(0).CreateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void CreateAuction_InvalidAuction_ReturnsFail()
    {
        //Arrange
        var vehicleUniqueIdentifier = Fixture.Create<Guid>();
        var suv = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, vehicleUniqueIdentifier)
            .With(x => x.StartingBid, -10)
            .Create();

        var auctions = Fixture.CreateMany<Auction>(0);

        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Ok(suv as Vehicle));

        _auctionRepository
            .GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Ok(auctions));

        //Act
        var result = await _auctionService.CreateAuction(vehicleUniqueIdentifier);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is ValidationError).Should().BeTrue();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(0).CreateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void CreateAuction_CreateAuctionFails_ThrowsAsync()
    {
        //Arrange
        var vehicleUniqueIdentifier = Fixture.Create<Guid>();
        var suv = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, vehicleUniqueIdentifier)
            .Create();

        var auctions = Fixture.CreateMany<Auction>(0);

        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Ok(suv as Vehicle));

        _auctionRepository
            .GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Ok(auctions));

        _auctionRepository.CreateAuctionAsync(Arg.Any<Auction>())
            .Returns(Result.Fail(Fixture.Create<Error>()));

        //Act
        var call = async () => await _auctionService.CreateAuction(vehicleUniqueIdentifier);

        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(1).CreateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void CreateAuction_CreateAuctionWorks_ResultOk()
    {
        //Arrange
        var vehicleUniqueIdentifier = Fixture.Create<Guid>();
        var suv = Fixture.Build<Suv>()
            .With(x => x.UniqueIdentifier, vehicleUniqueIdentifier)
            .Create();

        var auctions = Fixture.CreateMany<Auction>(0);

        var createdAuction = new Auction(vehicleUniqueIdentifier,
            suv.StartingBid);

        _vehicleRepository
            .GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Ok(suv as Vehicle));

        _auctionRepository
            .GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueIdentifier)
            .Returns(Result.Ok(auctions));

        _auctionRepository.CreateAuctionAsync(Arg.Any<Auction>())
            .Returns(Result.Ok(createdAuction));

        //Act
        var result = await _auctionService.CreateAuction(vehicleUniqueIdentifier);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UniqueIdentifier.Should().Be(createdAuction.UniqueIdentifier);
        result.Value.VehicleUniqueIdentifier.Should().Be(suv.UniqueIdentifier);
        result.Value.Bids.Should().BeEmpty();
        result.Value.StartingBid.Should().Be(suv.StartingBid);
        result.Value.Status.Should().Be(AuctionStatus.Closed);
        await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueIdentifier);
        await _auctionRepository.Received(1).CreateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void UpdateAuctionStatus_AuctionDoesNotExist_ReturnsFail()
    {
        //Arrange
        var auctionId = Fixture.Create<Guid>();
        _auctionRepository.GetAuctionByUniqueIdentifierAsync(auctionId)
            .Returns(Result.Ok(null as Auction));

        //Act
        var result = await _auctionService.UpdateAuctionStatus(auctionId, AuctionStatus.Open);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is NotFoundError).Should().BeTrue();
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionId);
        await _auctionRepository.Received(0).GetAuctionsByVehicleUniqueIdentifierAsync(Arg.Any<Guid>());
        await _auctionRepository.Received(0).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void UpdateAuctionStatus_AuctionAlreadyOpen_ReturnsFail()
    {
        //Arrange
        var auctionId = Fixture.Create<Guid>();
        var openAuction = Fixture
            .Build<Auction>()
            .With(x => x.UniqueIdentifier, auctionId)
            .With(x => x.Status, AuctionStatus.Open)
            .With(x => x.Bids, new List<Bid>())
            .Create();

        _auctionRepository.GetAuctionByUniqueIdentifierAsync(auctionId)
            .Returns(Result.Ok(openAuction));

        //Act
        var result = await _auctionService.UpdateAuctionStatus(auctionId, AuctionStatus.Open);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is ConflictAuctionError).Should().BeTrue();
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionId);
        await _auctionRepository.Received(0).GetAuctionsByVehicleUniqueIdentifierAsync(Arg.Any<Guid>());
        await _auctionRepository.Received(0).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void UpdateAuctionStatus_AuctionAlreadyClosed_ReturnsFail()
    {
        //Arrange
        var auctionId = Fixture.Create<Guid>();
        var closedAuction = Fixture
            .Build<Auction>()
            .With(x => x.UniqueIdentifier, auctionId)
            .With(x => x.Status, AuctionStatus.Closed)
            .With(x => x.Bids, new List<Bid>())
            .Create();

        _auctionRepository.GetAuctionByUniqueIdentifierAsync(auctionId)
            .Returns(Result.Ok(closedAuction));

        //Act
        var result = await _auctionService.UpdateAuctionStatus(auctionId, AuctionStatus.Closed);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is ConflictAuctionError).Should().BeTrue();
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionId);
        await _auctionRepository.Received(0).GetAuctionsByVehicleUniqueIdentifierAsync(Arg.Any<Guid>());
        await _auctionRepository.Received(0).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void UpdateAuctionStatus_AnotherAuctionForTheVehicleIsOpen_ReturnsFail()
    {
        //Arrange
        var auctionId = Fixture.Create<Guid>();
        var vehicleUniqueId = Fixture.Create<Guid>();
        var closedAuction = Fixture
            .Build<Auction>()
            .With(x => x.UniqueIdentifier, auctionId)
            .With(x => x.Status, AuctionStatus.Closed)
            .With(x => x.Bids, new List<Bid>())
            .With(x => x.VehicleUniqueIdentifier, vehicleUniqueId)
            .Create();

        var openAuction = Fixture
            .Build<Auction>()
            .With(x => x.Status, AuctionStatus.Open)
            .With(x => x.Bids, new List<Bid>())
            .With(x => x.VehicleUniqueIdentifier, vehicleUniqueId)
            .Create();

        _auctionRepository.GetAuctionByUniqueIdentifierAsync(auctionId)
            .Returns(Result.Ok(closedAuction));

        _auctionRepository.GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueId)
            .Returns(Result.Ok(new List<Auction> { openAuction, closedAuction }.AsEnumerable()));

        //Act
        var result = await _auctionService.UpdateAuctionStatus(auctionId, AuctionStatus.Open);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is ConflictAuctionError).Should().BeTrue();
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionId);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifierAsync(vehicleUniqueId);
        await _auctionRepository.Received(0).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void UpdateAuctionStatus_OpenAuctionWorks_ReturnsOk()
    {
        //Arrange
        var auctionId = Fixture.Create<Guid>();
        var auction = Fixture
            .Build<Auction>()
            .With(x => x.UniqueIdentifier, auctionId)
            .With(x => x.Status, AuctionStatus.Closed)
            .With(x => x.Bids, new List<Bid>())
            .Create();

        _auctionRepository.GetAuctionByUniqueIdentifierAsync(auctionId)
            .Returns(Result.Ok(auction));

        _auctionRepository.GetAuctionsByVehicleUniqueIdentifierAsync(auction.VehicleUniqueIdentifier)
            .Returns(Result.Ok(new List<Auction> { auction }.AsEnumerable()));

        _auctionRepository.UpdateAuctionAsync(Arg.Any<Auction>()).Returns(Result.Ok(true));

        //Act
        var result = await _auctionService.UpdateAuctionStatus(auctionId, AuctionStatus.Open);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(AuctionStatus.Open);
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionId);
        await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifierAsync(auction.VehicleUniqueIdentifier);
        await _auctionRepository.Received(1).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void UpdateAuctionStatus_CloseAuctionWorks_ReturnsOk()
    {
        //Arrange
        var auctionId = Fixture.Create<Guid>();
        var auction = Fixture
            .Build<Auction>()
            .With(x => x.UniqueIdentifier, auctionId)
            .With(x => x.Status, AuctionStatus.Open)
            .Create();

        _auctionRepository.GetAuctionByUniqueIdentifierAsync(auctionId)
            .Returns(Result.Ok(auction));

        _auctionRepository.GetAuctionsByVehicleUniqueIdentifierAsync(auction.VehicleUniqueIdentifier)
            .Returns(Result.Ok(new List<Auction> { auction }.AsEnumerable()));

        _auctionRepository.UpdateAuctionAsync(Arg.Any<Auction>()).Returns(Result.Ok(true));

        //Act
        var result = await _auctionService.UpdateAuctionStatus(auctionId, AuctionStatus.Closed);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(AuctionStatus.Closed);
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionId);
        await _auctionRepository.Received(0).GetAuctionsByVehicleUniqueIdentifierAsync(auction.VehicleUniqueIdentifier);
        await _auctionRepository.Received(1).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void AddBid_InvalidBid_ReturnsFail()
    {
        //Arrange
        var invalidBid = new Bid("", 0);
        var auctionUniqueIdentifier = Fixture.Create<Guid>();

        //Act
        var result = await _auctionService.AddBid(auctionUniqueIdentifier, invalidBid);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is ValidationError).Should().BeTrue();
        await _auctionRepository.Received(0).GetAuctionByUniqueIdentifierAsync(auctionUniqueIdentifier);
        await _auctionRepository.Received(0).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void AddBid_AuctionDoesNotExists_ReturnsFail()
    {
        //Arrange
        var auctionUniqueIdentifier = Fixture.Create<Guid>();
        var bid = Fixture.Create<Bid>();

        _auctionRepository.GetAuctionByUniqueIdentifierAsync(Arg.Any<Guid>())
            .Returns(Result.Ok(null as Auction));

        //Act
        var result = await _auctionService.AddBid(auctionUniqueIdentifier, bid);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is NotFoundError).Should().BeTrue();
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionUniqueIdentifier);
        await _auctionRepository.Received(0).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void AddBid_BidLowerThenStartingBid_ReturnsFail()
    {
        //Arrange
        var auctionUniqueIdentifier = Fixture.Create<Guid>();
        var auction = Fixture.Build<Auction>()
            .With(x => x.UniqueIdentifier)
            .With(x => x.Bids, new List<Bid>())
            .Create();
        var bid = new Bid(Fixture.Create<string>(), auction.StartingBid - 10);

        _auctionRepository.GetAuctionByUniqueIdentifierAsync(Arg.Any<Guid>())
            .Returns(Result.Ok(auction));

        //Act
        var result = await _auctionService.AddBid(auctionUniqueIdentifier, bid);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is InvalidBidError).Should().BeTrue();
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionUniqueIdentifier);
        await _auctionRepository.Received(0).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void AddBid_BidLowerThenHighestBid_ReturnsFail()
    {
        //Arrange
        var auctionUniqueIdentifier = Fixture.Create<Guid>();
        var startingBid = Fixture.Create<decimal>();
        var currentBid = new Bid(Fixture.Create<string>(), startingBid + 10);
        var auction = Fixture.Build<Auction>()
            .With(x => x.UniqueIdentifier)
            .With(x => x.StartingBid, startingBid)
            .With(x => x.Bids, new List<Bid> { currentBid })
            .Create();

        var bid = new Bid(Fixture.Create<string>(), auction.StartingBid + 1);

        _auctionRepository.GetAuctionByUniqueIdentifierAsync(Arg.Any<Guid>())
            .Returns(Result.Ok(auction));

        //Act
        var result = await _auctionService.AddBid(auctionUniqueIdentifier, bid);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is InvalidBidError).Should().BeTrue();
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionUniqueIdentifier);
        await _auctionRepository.Received(0).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void AddBid_ClosedAuction_ReturnsFail()
    {
        //Arrange
        var auctionUniqueIdentifier = Fixture.Create<Guid>();
        var auction = Fixture.Build<Auction>()
            .With(x => x.Status, AuctionStatus.Closed)
            .With(x => x.UniqueIdentifier, auctionUniqueIdentifier)
            .Create();

        var bid = Fixture.Create<Bid>();

        _auctionRepository.GetAuctionByUniqueIdentifierAsync(Arg.Any<Guid>())
            .Returns(Result.Ok(auction));

        //Act
        var result = await _auctionService.AddBid(auctionUniqueIdentifier, bid);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is ClosedAuctionError).Should().BeTrue();
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionUniqueIdentifier);
        await _auctionRepository.Received(0).UpdateAuctionAsync(Arg.Any<Auction>());
    }

    [Fact]
    public async void AddBid_BidIsValid_ReturnsOk()
    {
        //Arrange
        var auctionUniqueIdentifier = Fixture.Create<Guid>();
        var bid = Fixture.Create<Bid>();
        var auction = Fixture
            .Build<Auction>()
            .With(x => x.StartingBid, bid.BidValue - 100)
            .With(x => x.UniqueIdentifier, auctionUniqueIdentifier)
            .With(x => x.Bids, new List<Bid>())
            .Create();

        _auctionRepository.GetAuctionByUniqueIdentifierAsync(Arg.Any<Guid>())
            .Returns(Result.Ok(auction));

        _auctionRepository.UpdateAuctionAsync(Arg.Any<Auction>())
            .Returns(Result.Ok());

        //Act
        var result = await _auctionService.AddBid(auctionUniqueIdentifier, bid);

        //Assert
        result.IsSuccess.Should().BeTrue();
        await _auctionRepository.Received(1).GetAuctionByUniqueIdentifierAsync(auctionUniqueIdentifier);
        await _auctionRepository.Received(1).UpdateAuctionAsync(Arg.Any<Auction>());
    }
}