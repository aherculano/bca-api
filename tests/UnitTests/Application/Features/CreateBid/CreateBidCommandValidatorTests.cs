using Application.Features.CreateBid;
using Application.Requests.BidRequests;
using AutoFixture;
using FluentAssertions;

namespace UnitTests.Application.Features.CreateBid;

public class CreateBidCommandValidatorTests : TestsBase
{
    private readonly CreateBidCommandValidator _validator;

    public CreateBidCommandValidatorTests()
    {
        _validator = new CreateBidCommandValidator();
    }

    [Fact]
    public void Validate_InvalidCommand_ReturnsInvalid()
    {
        //Arrange
        var command = new CreateBidCommand(Guid.Empty, new BidRequest(string.Empty, 0));
        
        //Act
        var validationResult = _validator.Validate(command);
        
        //Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Count.Should().Be(3);
    }

    [Fact]
    public void Validate_ValidCommand_ReturnsValid()
    {
        //Arrange
        var command = Fixture.Create<CreateBidCommand>();
        
        //Act
        var validationResult = _validator.Validate(command);
        
        //Assert
        validationResult.IsValid.Should().BeTrue();
    }
}