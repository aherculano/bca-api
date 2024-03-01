using System.Diagnostics.CodeAnalysis;
using Application.Features.CreateBid;
using Application.Requests.BidRequests;
using Application.Responses.AuctionResponses;
using AutoFixture;
using Domain.Models.Auction;
using Domain.Services.Auctions;
using FluentAssertions;
using FluentResults;
using NSubstitute;

namespace UnitTests.Application.Features.CreateBid;

[ExcludeFromCodeCoverage]
public class CreateBidCommandHandlerTests : TestsBase
{
    private readonly IAuctionService _service;
    private readonly CreateBidCommandHandler _handler;

    public CreateBidCommandHandlerTests()
    {
        _service = Fixture.Freeze<IAuctionService>();
        _handler = new CreateBidCommandHandler(_service);
    }

    [Fact]
    public async void Handle_ServiceFails_ReturnsFail()
    {
        //Arrange
        var command = Fixture.Create<CreateBidCommand>();
        _service.AddBid(Arg.Any<Guid>(),Arg.Any<Bid>())
            .Returns(Result.Fail(Fixture.Create<Error>()));
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsFailed.Should().BeTrue();
        await _service.Received(1).AddBid(command.UniqueIdentifier, Arg.Any<Bid>());
    }
    [Fact]
    public async void Handle_ServiceOk_ResultOk()
    {
        //Arrange
        var command = Fixture.Create<CreateBidCommand>();
        _service.AddBid(Arg.Any<Guid>(),Arg.Any<Bid>())
            .Returns(Result.Ok(command.Request.MapToDomain()));
        
        //Act
        var result =  await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(command.Request.MapToDomain().MapToResponse());
        await _service.Received(1).AddBid(command.UniqueIdentifier, Arg.Any<Bid>());
    }
}