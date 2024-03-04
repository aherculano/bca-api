using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using Domain.Models.Auction;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure.Data.EntityFramework.Repositories;

namespace UnitTests.Infrastructure.Data.EntityFramework.Repositories;

[ExcludeFromCodeCoverage]
public class AuctionRepositoryTests : RepositoryTestsBase
{
    private readonly IAuctionRepository _repository;

    public AuctionRepositoryTests()
    {
        _repository = new AuctionRepository(AuctionDbContext);
    }

    [Fact]
    public async void CreateAuction_Works_ReturnsOk()
    {
        //Arrange
        var auction = Fixture.Create<Auction>();

        //Act
        var result = await _repository.CreateAuctionAsync(auction);

        //Assert
        result.IsSuccess.Should().BeTrue();
        AuctionDbContext.Auctions
            .FirstOrDefault(x => x.UniqueIdentifier == auction.UniqueIdentifier).Should()
            .BeEquivalentTo(auction);
    }


    [Fact]
    public async Task UpdateAuctionAsync_ExistingAuction_ShouldReturnOkResult()
    {
        // Arrange
        var auction = AuctionDbContext.Auctions.First();
        auction.Status = AuctionStatus.Closed;

        // Act
        var result = await _repository.UpdateAuctionAsync(auction);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetAuctionsByVehicleUniqueIdentifierAsync_ExistingVehicle_ShouldReturnAuctions()
    {
        // Arrange
        var expectedAuctions = AuctionDbContext.Auctions.First();

        // Act
        var result =
            await _repository.GetAuctionsByVehicleUniqueIdentifierAsync(expectedAuctions.VehicleUniqueIdentifier);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(1);
        result.Value.First().Should().Be(expectedAuctions);
    }

    [Fact]
    public async Task GetAuctionByUniqueIdentifierAsync_ExistingAuction_ShouldReturnAuction()
    {
        // Arrange
        var auction = AuctionDbContext.Auctions.First();

        // Act
        var result = await _repository.GetAuctionByUniqueIdentifierAsync(auction.UniqueIdentifier);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(auction);
    }
}