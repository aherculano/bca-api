using System.Diagnostics.CodeAnalysis;
using Application.Features.GetAuctionByUniqueIdentifier;
using Application.Responses.AuctionResponses;
using AutoFixture;
using Domain.Errors;
using Domain.Models.Auction;
using Domain.Repositories;
using FluentAssertions;
using FluentResults;
using NSubstitute;

namespace UnitTests.Application.Features.GetAuctionByUniqueIdentifier;

[ExcludeFromCodeCoverage]
public class GetAuctionByUniqueIdentifierQueryHandlerTests : TestsBase
{
    private readonly GetAuctionByUniqueIdentifierQueryHandler _handler;
    private readonly IAuctionRepository _repository;

    public GetAuctionByUniqueIdentifierQueryHandlerTests()
    {
        _repository = Fixture.Freeze<IAuctionRepository>();
        _handler = new GetAuctionByUniqueIdentifierQueryHandler(_repository);
    }

    [Fact]
    public async void Handle_RepositoryFails_ThrowsAsync()
    {
        //Arrange
        var query = Fixture.Create<GetAuctionByUniqueIdentifierQuery>();
        _repository.GetAuctionByUniqueIdentifierAsync(Arg.Any<Guid>())
            .Returns(Result.Fail(Fixture.Create<Error>()));

        //Act
        var call = async () => await _handler.Handle(query, CancellationToken.None);

        //Assert
        await call.Should().ThrowAsync<Exception>();
        await _repository.Received(1).GetAuctionByUniqueIdentifierAsync(query.UniqueIdentifier);
    }

    [Fact]
    public async void Handle_DoesNotExist_ReturnFails()
    {
        //Arrange
        var query = Fixture.Create<GetAuctionByUniqueIdentifierQuery>();
        _repository.GetAuctionByUniqueIdentifierAsync(Arg.Any<Guid>())
            .Returns(Result.Ok(null as Auction));

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Any(x => x is NotFoundError).Should().BeTrue();
        await _repository.Received(1).GetAuctionByUniqueIdentifierAsync(query.UniqueIdentifier);
    }

    [Fact]
    public async void Handle_AuctionExists_ResultOk()
    {
        //Arrange
        var query = Fixture.Create<GetAuctionByUniqueIdentifierQuery>();

        var auction = Fixture.Build<Auction>()
            .With(x => x.UniqueIdentifier, query.UniqueIdentifier)
            .Create();

        _repository.GetAuctionByUniqueIdentifierAsync(Arg.Any<Guid>())
            .Returns(Result.Ok(auction));

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(auction.MapToResponse());
        await _repository.Received(1).GetAuctionByUniqueIdentifierAsync(query.UniqueIdentifier);
    }
}