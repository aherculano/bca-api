using System.Diagnostics.CodeAnalysis;
using Application.Features.CreateAuction;
using AutoFixture;
using Domain.Models.Auction;
using Domain.Services.Auctions;
using FluentAssertions;
using FluentResults;
using NSubstitute;

namespace UnitTests.Application.Features.CreateAuction;

[ExcludeFromCodeCoverage]
public class CreateAuctionCommandHandlerTests : TestsBase
{
    private readonly IAuctionService _auctionService;
    private readonly CreateAuctionCommandHandler _handler;

    public CreateAuctionCommandHandlerTests()
    {
        _auctionService = Fixture.Freeze<IAuctionService>();
        _handler = new CreateAuctionCommandHandler(_auctionService);
    }

    [Fact]
    public async void Handle_ServiceReturnsOk_ReturnsOk()
    {
        //Arrange
        var command = Fixture.Create<CreateAuctionCommand>();
        var auction = Fixture.Build<Auction>()
            .With(x => x.VehicleUniqueIdentifier, command.VehicleUniqueIdentifier)
            .Create();
        _auctionService.CreateAuction(command.VehicleUniqueIdentifier)
            .Returns(Result.Ok(auction));

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UniqueIdentifier.Should().Be(auction.UniqueIdentifier);
        result.Value.VehicleUniqueIdentifier.Should().Be(auction.VehicleUniqueIdentifier);
        result.Value.StartingBid.Should().Be(auction.StartingBid);
        result.Value.Status.Should().Be(auction.Status.ToString());
    }

    [Fact]
    public async void Handle_ServiceReturnsFail_ReturnsFail()
    {
        //Arrange
        var command = Fixture.Create<CreateAuctionCommand>();
        var failedResult = Result.Fail(Fixture.Create<Error>());
        _auctionService.CreateAuction(command.VehicleUniqueIdentifier)
            .Returns(failedResult);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(failedResult.Errors);
    }
}