using System.Diagnostics.CodeAnalysis;
using Application.Features.GetAuctionByUniqueIdentifier;
using AutoFixture;
using FluentAssertions;

namespace UnitTests.Application.Features.GetAuctionByUniqueIdentifier;

[ExcludeFromCodeCoverage]
public class GetAuctionByUniqueIdentifierQueryValidatorTests : TestsBase
{
    private readonly GetAuctionByUniqueIdentifierQueryValidator _validator;

    public GetAuctionByUniqueIdentifierQueryValidatorTests()
    {
        _validator = new GetAuctionByUniqueIdentifierQueryValidator();
    }

    [Fact]
    public void Validate_EmtpyGuid_ReturnsFail()
    {
        //Arrange
        var uniqueIdentifier = Guid.Empty;
        var query = new GetAuctionByUniqueIdentifierQuery(uniqueIdentifier);

        //Act
        var validationResult = _validator.Validate(query);

        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(1);
        validationResult.Errors.Any(x => x.ErrorMessage == "Invalid Auction Unique Identifier");
    }

    [Fact]
    public void Validate_CorrectGuid_ReturnsFail()
    {
        //Arrange
        var uniqueIdentifier = Fixture.Create<Guid>();
        var query = new GetAuctionByUniqueIdentifierQuery(uniqueIdentifier);

        //Act
        var validationResult = _validator.Validate(query);

        //Assert
        validationResult.IsValid.Should().BeTrue();
    }
}