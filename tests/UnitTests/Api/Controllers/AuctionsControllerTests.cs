using System.Diagnostics.CodeAnalysis;
using Api.Controllers;
using Api.Requests;
using Application.Features.CreateAuction;
using Application.Features.CreateBid;
using Application.Features.GetAuctionByUniqueIdentifier;
using Application.Features.UpdateAuctionStatus;
using Application.Requests.BidRequests;
using Application.Responses.AuctionResponses;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace UnitTests.Api.Controllers;

[ExcludeFromCodeCoverage]
public class AuctionsControllerTests : TestsBase
{
    private readonly AuctionsController _controller;
    private readonly IMediator _mediator;

    public AuctionsControllerTests()
    {
        _mediator = Fixture.Freeze<IMediator>();
        _controller = new AuctionsController(_mediator);
    }

    [Fact]
    public async void CreateAuction_ReturnsOk_ReturnsCreated()
    {
        //Arrange
        var auctionRequest = Fixture.Create<CreateAuctionRequest>();
        var response = Fixture.Create<AuctionResponse>();
        _mediator.Send(Arg.Any<CreateAuctionCommand>()).Returns(response);

        //Act
        var result = await _controller.CreateAuctionAsync(auctionRequest);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult.Value.Should().BeEquivalentTo(response);
        createdResult.ActionName.Should().Be("GetAuctionByAuctionId");
        createdResult.RouteValues.Keys.Any(x => x == "auctionUniqueIdentifier").Should().BeTrue();
        createdResult.RouteValues.GetValueOrDefault("auctionUniqueIdentifier").Should()
            .BeEquivalentTo(response.UniqueIdentifier);
    }

    [Fact]
    public async void GetAuctionByAuctionId_ReturnsOk_ReturnsOk()
    {
        //Arrange
        var auctionId = Fixture.Create<Guid>();
        var auctionResponse = Fixture.Create<AuctionResponse>();
        _mediator.Send(Arg.Any<GetAuctionByUniqueIdentifierQuery>()).Returns(auctionResponse);

        //Act
        var result = await _controller.GetAuctionByAuctionIdAsync(auctionId);

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(auctionResponse);
    }

    [Fact]
    public async void UpdateAuctionStatus_ReturnsOk_ReturnsOk()
    {
        //Arrange
        var auctionId = Fixture.Create<Guid>();
        var updateAuctionRequest = Fixture.Create<UpdateAuctionStatusRequest>();
        var updateAuctionResponse = Fixture.Create<AuctionStatusResponse>();
        _mediator.Send(Arg.Any<UpdateAuctionStatusCommand>()).Returns(updateAuctionResponse.AuctionStatus);

        //Act
        var result = await _controller.UpdateAuctionStatusAsync(auctionId, updateAuctionRequest);

        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(updateAuctionResponse);
    }

    [Fact]
    public async void CreateBid_ReturnsOk_ReturnsOk()
    {
        //Arrange
        var auctionId = Fixture.Create<Guid>();
        var bidRequest = Fixture.Create<BidRequest>();
        var bidResponse = Fixture.Create<BidResponse>();
        _mediator.Send(Arg.Any<CreateBidCommand>()).Returns(bidResponse);

        //Act
        var result = await _controller.CreateBidAsync(auctionId, bidRequest);

        //Assert
        result.Should().BeOfType<CreatedResult>();
    }
}