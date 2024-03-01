using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using Domain.Errors;
using Domain.Models.Auction;
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
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAuctionRepository _auctionRepository;
    private readonly IAuctionService _auctionService;

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
        await _auctionRepository.Received(0).GetAuctionsByVehicleUniqueIdentifier(Arg.Any<Guid>());
        await _auctionRepository.Received(0).CreateAuction(Arg.Any<Auction>());
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
        await _auctionRepository.Received(0).GetAuctionsByVehicleUniqueIdentifier(Arg.Any<Guid>());
        await _auctionRepository.Received(0).CreateAuction(Arg.Any<Auction>());
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
             .GetAuctionsByVehicleUniqueIdentifier(vehicleUniqueIdentifier)
             .Returns(Result.Ok(auction));
        
         //Act
         var result = await _auctionService.CreateAuction(vehicleUniqueIdentifier);
        
         //Assert
         result.IsFailed.Should().BeTrue();
         result.Errors.Any(x => x is AlreadyExistsError).Should().BeTrue();
         await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
         await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifier(vehicleUniqueIdentifier);
         await _auctionRepository.Received(0).CreateAuction(Arg.Any<Auction>());
     }
     
     [Fact]
     public async void CreateAuction__GetAuctionFails_ThrowsAsync()
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
             .GetAuctionsByVehicleUniqueIdentifier(vehicleUniqueIdentifier)
             .Returns(Result.Fail(Fixture.Create<Error>()));
        
         //Act
         var call = async () =>  await _auctionService.CreateAuction(vehicleUniqueIdentifier);
        
         //Assert
         await call.Should().ThrowAsync<Exception>();
         await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
         await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifier(vehicleUniqueIdentifier);
         await _auctionRepository.Received(0).CreateAuction(Arg.Any<Auction>());
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
             .GetAuctionsByVehicleUniqueIdentifier(vehicleUniqueIdentifier)
             .Returns(Result.Ok(auctions));

         _auctionRepository.CreateAuction(Arg.Any<Auction>())
             .Returns(Result.Fail(Fixture.Create<Error>()));
        
         //Act
         var call = async () =>  await _auctionService.CreateAuction(vehicleUniqueIdentifier);
        
         //Assert
         await call.Should().ThrowAsync<Exception>();
         await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
         await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifier(vehicleUniqueIdentifier);
         await _auctionRepository.Received(1).CreateAuction(Arg.Any<Auction>());
     }
     
     [Fact]
     public async void CreateAuction__CreateAuctionWorks_ResultOk()
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
             .GetAuctionsByVehicleUniqueIdentifier(vehicleUniqueIdentifier)
             .Returns(Result.Ok(auctions));

         _auctionRepository.CreateAuction(Arg.Any<Auction>())
             .Returns(Result.Ok(createdAuction));
        
         //Act
         var result = await _auctionService.CreateAuction(vehicleUniqueIdentifier);
        
         //Assert
         result.IsSuccess.Should().BeTrue();
         result.Value.UniqueIdentifier.Should().Be(createdAuction.UniqueIdentifier);
         result.Value.VehicleUniqueIdentifier.Should().Be(suv.UniqueIdentifier);
         result.Value.Bids.Should().BeEmpty();
         result.Value.StartBid.Should().Be(suv.StartingBid);
         result.Value.Status.Should().Be(AuctionStatus.Closed);
         await _vehicleRepository.Received(1).GetVehicleByUniqueIdentifierAsync(vehicleUniqueIdentifier);
         await _auctionRepository.Received(1).GetAuctionsByVehicleUniqueIdentifier(vehicleUniqueIdentifier);
         await _auctionRepository.Received(1).CreateAuction(Arg.Any<Auction>());
     }
}